using Acr.UserDialogs;
using Plugin.SimpleAudioPlayer;
using Rg.Plugins.Popup.Services;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;
using XpertMobileApp.Api.Services;
using XpertMobileApp.DAL;
using XpertMobileApp.Helpers;
using XpertMobileApp.Models;
using XpertMobileApp.Services;
using XpertMobileApp.ViewModels;

namespace XpertMobileApp.Views
{
	[XamlCompilation(XamlCompilationOptions.Compile)]
	public partial class RfidScanPage : ContentPage 
	{
        RfidScanViewModel viewModel;
        RFID_Manager rfid_manager;
        public Command loadLotsInfo { get; set; }
        private Command SaveRFIDsCommand { get; set; }
     
        private ISimpleAudioPlayer _simpleAudioPlayer;
        public RfidScanPage()
		{
			InitializeComponent ();
            _simpleAudioPlayer = CrossSimpleAudioPlayer.CreateSimpleAudioPlayer();
            Stream beepStream = GetType().Assembly.GetManifestResourceStream("XpertMobileApp.beep.wav");
            bool isSuccess = _simpleAudioPlayer.Load(beepStream);
            BindingContext = viewModel = new RfidScanViewModel();
            loadLotsInfo = new Command(async () => await ExecuteLoadLotCommand());
            loadLotsInfo.Execute(null);
            rfid_manager = new RFID_Manager();
            if (!rfid_manager.IsInit) {
                if (!rfid_manager.Init())
                {
                    DisplayAlert(AppResources.alrt_msg_Alert, "l'intialisation de lecteur Rfid est echoee", AppResources.alrt_msg_Ok);
                    btn_Clear.IsEnabled = false;
                    btn_Scan.IsEnabled = false;
                    SaveRfids.IsEnabled = false;
                }
            }
            
            MessagingCenter.Subscribe<RFID_Manager, string>(this, MCDico.RFID_SCANED, async (obj, item) =>
            {
                if (!string.IsNullOrEmpty(item)) {
                    string[] strs = item.Split('@');
                    int index = checkIsExistRfid(strs[0]);
                    _simpleAudioPlayer.Play();
                    if (index==-1)
                    {
                        SCANED_RFID Tag = new SCANED_RFID();
                        Tag.EPC = strs[0];
                        Tag.RSSI = strs[1];
                        Tag.COUNT = 1;
                        viewModel.Items.Add(Tag);
                        viewModel.ElementsCount++;
                    }
                    else
                    {
                        viewModel.Items[index].COUNT++;
                    }
                }
            });

            MessagingCenter.Subscribe<RfidScanPage, string>(this, MCDico.CODE_BARRE_SCANED, async (obj, item) =>
            {
                _simpleAudioPlayer.Play();
                if (string.IsNullOrEmpty(item))
                {
                   
                        codeBarrLot.Text = item + "\r\n";
               
                }
               
            });
            
        }


        public int checkIsExistRfid(string strEPC)
        {
            int existFlag = -1;
            if (string.IsNullOrEmpty(strEPC))
            {
                return existFlag;
            }
            for (int i = 0; i < viewModel.Items.Count; i++)
            {
                if (strEPC == viewModel.Items[i].EPC)
                {
                    existFlag = i;
                    break;
                }
            }
            return existFlag;
        }
        async void OnItemSelected(object sender, SelectedItemChangedEventArgs args)
        {
           
        }
        protected override void OnAppearing()
        {
            base.OnAppearing();
            if (rfid_manager.LoopFlag) {
                btn_Scan.Text = "Stop";
                SaveRfids.IsEnabled = true;
            }
        }
        protected override void OnDisappearing()
        {
            base.OnDisappearing();
            if (rfid_manager.LoopFlag) {
                rfid_manager.StopInventory();
                SaveRfids.IsEnabled = true;
            }
        }

         private void Filter_Clicked(object sender, EventArgs e)
        {
            FilterPanel.IsVisible = !FilterPanel.IsVisible;
        }

        private void btn_Clear_Clicked(object sender, EventArgs e)
        {
            viewModel.Items.Clear();
            viewModel.ElementsCount=0;
        }

        private void btn_Scan_Clicked(object sender, EventArgs e)
        {
            if (btn_Scan.Text == "Stop")
            {
                rfid_manager.StopInventory();
                btn_Scan.Text = "Scan";
                btn_Clear.IsEnabled = true;
                AntiP.IsEnabled = true;
                CScan.IsEnabled = true;
                qvalue.IsEnabled = true;
                SaveRfids.IsEnabled = true;
                return;
            }
            if (!rfid_manager.LoopFlag) {
                btn_Clear.IsEnabled = false;
                AntiP.IsEnabled = false;
                CScan.IsEnabled = false;
                qvalue.IsEnabled = false;
                if (!viewModel.ContinuesScan)
                {
                    byte q = 0;
                    byte anti = 0;
                    if (viewModel.Anti)
                    {
                        q = Convert.ToByte(viewModel.q);
                        anti = 1;
                    }
                    btn_Scan.Text = "Stop";
                    SaveRfids.IsEnabled = false;
                    rfid_manager.StartContenuesInventary(Convert.ToByte(viewModel.Anti), (byte)viewModel.q);
                }
                else
                {
                    rfid_manager.StartInventorySingl();
                }
            }
            

        }

        private void SaveRfids_Clicked(object sender, EventArgs e)
        {
            List<STK_STOCK_RFID> rfids = new List<STK_STOCK_RFID>();
            if (viewModel.Items.Count == 0) return;
            if (viewModel.CurrentLot.ID_STOCK == null) return;
            if (viewModel.Items != null && viewModel.Items.Count > 0)
            {
                foreach (SCANED_RFID eleme in viewModel.Items) {
                STK_STOCK_RFID rfid = new STK_STOCK_RFID();
                rfid.ID_STOCK = viewModel.CurrentLot.ID_STOCK;
                rfid.EPC = eleme.EPC;
                rfids.Add(rfid);
            }
                SaveRFIDsCommand = new Command(async () => await AddRfids(rfids));
                SaveRFIDsCommand.Execute(null);
            }
        }

        private async Task<bool> AddRfids(List<STK_STOCK_RFID> rfids)
        {
            try
            {
                bool result = await WebServiceClient.AddRfids(rfids);
                return result;
            }
            catch (XpertException e)
            {
               
                await DisplayAlert(e.Message, AppResources.alrt_msg_Alert, AppResources.alrt_msg_Ok);
             
                return false;
            }
        }
        async Task ExecuteLoadLotCommand()
        {
            try
            {
                if (!string.IsNullOrEmpty(viewModel.CODE_BARRE_LOT)) {
                    var lots = await WebServiceClient.getStckFromCodeBarre(viewModel.CODE_BARRE_LOT);
                    if (!(lots == null) && lots.Count > 0)
                    {
                        this.viewModel.CurrentLot = lots[0];
                    }
                    else
                    {
                        await UserDialogs.Instance.AlertAsync(AppResources.alrt_msg_multiLotForCodeBarre, AppResources.alrt_msg_Alert,
                   AppResources.alrt_msg_Ok);
                        viewModel.CurrentLot = new View_STK_STOCK();
                        viewModel.CODE_BARRE_LOT = "";
                    }
                }
             }
            catch (Exception ex)
            {
                await UserDialogs.Instance.AlertAsync(ex.Message, AppResources.alrt_msg_Alert,
                   AppResources.alrt_msg_Ok);
            }
          
        }

        private void codeBarrLot_Unfocused(object sender, FocusEventArgs e)
        {
            loadLotsInfo.Execute(null);
        }

        private void btn_Scan_CB_Clicked(object sender, EventArgs e)
        {
            // TODO scan code
        }
    }
}