using Rg.Plugins.Popup.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;
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

        public RfidScanPage()
		{
			InitializeComponent ();

            BindingContext = viewModel = new RfidScanViewModel();
            loadLotsInfo = new Command(async () => await ExecuteLoadLotCommand());
            loadLotsInfo.Execute(null);
            MessagingCenter.Subscribe<RFID_Manager, string>(this, MCDico.RFID_SCANED, async (obj, item) =>
            {
                if (!string.IsNullOrEmpty(item)) {
                    string[] strs = item.Split('@');
                    if (!viewModel.Items.Contains(strs[0]))
                    {
                        viewModel.Items.Add(strs[0]);
                        viewModel.ElementsCount++;
                    }
                }
            });

        }

        async void OnItemSelected(object sender, SelectedItemChangedEventArgs args)
        {
           
        }
        protected override void OnAppearing()
        {
            base.OnAppearing();
            if (rfid_manager.LoopFlag) btn_Scan.Text = "Stop";
        }
        protected override void OnDisappearing()
        {
            base.OnDisappearing();
            rfid_manager.StopInventory();
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
            foreach (string eleme in viewModel.Items) {
                STK_STOCK_RFID rfid = new STK_STOCK_RFID();
                rfid.ID_STOCK = viewModel.IdStock;
                rfid.EPC = eleme;
                rfids.Add(rfid);
            }
            SaveRFIDsCommand = new Command(async () => await AddRfids(rfids));
            SaveRFIDsCommand.Execute(null);
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
               
                await DisplayAlert(AppResources.alrt_msg_Alert, "Erreur", AppResources.alrt_msg_Ok);
             
                return false;
            }
        }
        async Task ExecuteLoadLotCommand()
        {
            try
            {
                rfid_manager = new RFID_Manager();
                if (!rfid_manager.Init()) await DisplayAlert(AppResources.alrt_msg_Alert, "l'intialisation de lecteur Rfid est echoee", AppResources.alrt_msg_Ok);
                var lots = await WebServiceClient.getStckFroIdStock(viewModel.IdStock);
                if (!(lots == null) && lots.Count > 0)
                {
                    this.viewModel.CurrentLot = lots[0];
                }
             }
            catch (Exception ex)
            {
               
            }
          
        }

    }
}