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
using XpertMobileApp.Api.Services.CODE_BARRE;
using XpertMobileApp.DAL;
using XpertMobileApp.Helpers;
using XpertMobileApp.Models;
using XpertMobileApp.Services;
using XpertMobileApp.ViewModels;

namespace XpertMobileApp.Views
{
	[XamlCompilation(XamlCompilationOptions.Compile)]
	public partial class RfidScanPage : ContentPage, ICallBackReciver
    {
        RfidScanViewModel viewModel;
        RFID_Manager rfid_manager;
        public Command loadLotsInfo { get; set; }
        private Command SaveRFIDsCommand { get; set; }
        ICODE_BARRE_Reder CBReder;
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
                    DisplayAlert(AppResources.alrt_msg_Alert, AppResources.Alrt_failInitRFID_Reader, AppResources.alrt_msg_Ok);
                    btn_Clear.IsEnabled = false;
                    btn_Scan.IsEnabled = false;
                    SaveRfids.IsEnabled = false;
                }
            }
            CBReder= DependencyService.Get<ICODE_BARRE_Reder>();
            CBReder.GetInstance();
                
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
            this.codebrarelot.Completed += Codebrarelot_Completed;

            MessagingCenter.Subscribe<LotsSelector, View_STK_STOCK>(this, MCDico.Lots_SELECTED, async (obj, item) =>
            {
                if (item != null) {
                    viewModel.CurrentLot = item;
                    viewModel.CODE_BARRE_LOT = item.CODE_BARRE_LOT;
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
            if (CBReder != null)
            {
                if (CBReder.GetIsOpen() == false)
                    CBReder.Init(this);
            }
        }
        protected override void OnDisappearing()
        {
            base.OnDisappearing();
            if (rfid_manager.LoopFlag) {
                rfid_manager.StopInventory();
                SaveRfids.IsEnabled = true;
            }
            if (CBReder != null)
            {
                CBReder.Close();
                CBReder.setIsOpen(false);
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
            if (btn_Scan.Text == AppResources.btn_Stop_Scan_Rfid)
            {
                rfid_manager.StopInventory();
                btn_Scan.Text = AppResources.btn_scan_Rfid_Text;
                btn_Clear.IsEnabled = true;
                AntiP.IsEnabled = true;
                CScan.IsEnabled = true;
                qvalue.IsEnabled = true;
                SaveRfids.IsEnabled = true;
                return;
            }
            if (!rfid_manager.LoopFlag) {
              
                if (viewModel.ContinuesScan)
                {
                    btn_Clear.IsEnabled = false;
                    AntiP.IsEnabled = false;
                    CScan.IsEnabled = false;
                    qvalue.IsEnabled = false;
                    byte q = 0;
                    byte anti = 0;
                    if (viewModel.Anti)
                    {
                        q = Convert.ToByte(viewModel.q);
                        anti = 1;
                    }
                    btn_Scan.Text = AppResources.btn_Stop_Scan_Rfid;
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
                    foreach (SCANED_RFID eleme in viewModel.Items)
                    {
                        STK_STOCK_RFID rfid = new STK_STOCK_RFID();
                        rfid.ID_STOCK = viewModel.CurrentLot.ID_STOCK;
                        rfid.EPC = eleme.EPC;
                        rfids.Add(rfid);
                    }
                    SaveRFIDsCommand = new Command(async () => await AddRfids(rfids));
                    SaveRFIDsCommand.Execute(null);
                }
        }

        private async Task AddRfids(List<STK_STOCK_RFID> rfids)
        {
            try
            {
                int result = await WebServiceClient.AddRfids(rfids);
                if(result==4) await DisplayAlert(AppResources.Alert_Deplicatrd_Rfid_Tages, AppResources.alrt_msg_Alert, AppResources.alrt_msg_Ok);
            }
            catch (XpertException e)
            {
                await DisplayAlert(e.Message, AppResources.alrt_msg_Alert, AppResources.alrt_msg_Ok);
            }
        }
        async Task ExecuteLoadLotCommand()
        {
            try
            {
                if (!string.IsNullOrEmpty(viewModel.CODE_BARRE_LOT)) {
                    viewModel.LOTS= await WebServiceClient.getStckFromCodeBarre(viewModel.CODE_BARRE_LOT);
                    if (viewModel.LOTS != null){
                        if (viewModel.LOTS.Count == 0)
                        {
                            await UserDialogs.Instance.AlertAsync(AppResources.alrt_msg_NotFondLotFromCodeBarre, AppResources.alrt_msg_Alert, AppResources.alrt_msg_Ok);
                            viewModel.CODE_BARRE_LOT = "";
                            viewModel.CurrentLot = new View_STK_STOCK();
                        }
                        else if ( viewModel.LOTS.Count == 1)
                        {
                            this.viewModel.CurrentLot = viewModel.LOTS[0];

                        } 
                        else if (viewModel.LOTS.Count > 1)
                        {
                            await UserDialogs.Instance.AlertAsync(AppResources.alrt_msg_multiLotForCodeBarre, AppResources.alrt_msg_Alert, AppResources.alrt_msg_Ok);
                            LotsSelector itemSelector = new LotsSelector(viewModel.LOTS);
                            await PopupNavigation.Instance.PushAsync(itemSelector);
                            viewModel.CODE_BARRE_LOT = "";
                            viewModel.CurrentLot = new View_STK_STOCK();
                        }
                    }
                }
             }
            catch (Exception ex)
            {
                await UserDialogs.Instance.AlertAsync(ex.Message, AppResources.alrt_msg_Alert,AppResources.alrt_msg_Ok);
            }
          
        }


        private void btn_Scan_CB_Clicked(object sender, EventArgs e)
        {
            CBReder.Scan();
        }

        public void ReciveData(string data)
        {
            _simpleAudioPlayer.Play();
            viewModel.CODE_BARRE_LOT = data;
            loadLotsInfo.Execute(null);
        }
        private void Codebrarelot_Completed(object sender, EventArgs e)
        {
            loadLotsInfo.Execute(null);
        }
    }
}