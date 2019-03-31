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
        private Command SaveRFIDsCommand { get; set; }

        public RfidScanPage()
		{
			InitializeComponent ();

            BindingContext = viewModel = new RfidScanViewModel();
            
            MessagingCenter.Subscribe<RfidScanViewModel, string>(this, MCDico.RFID_SCANED, async (obj, item) =>
            {
                viewModel.Items.Add(item);
                
                viewModel.ElementsCount++;
            });

        }

        async void OnItemSelected(object sender, SelectedItemChangedEventArgs args)
        {

        }

        protected override void OnAppearing()
        {
            base.OnAppearing();
            viewModel.loadLotsInfo = new Command(async () => await ExecuteLoadLotCommand());
            viewModel.loadLotsInfo.Execute(null);
        }

        private void Filter_Clicked(object sender, EventArgs e)
        {
            FilterPanel.IsVisible = !FilterPanel.IsVisible;
        }

        private void btn_Clear_Clicked(object sender, EventArgs e)
        {
            viewModel.Items.Clear();
        }

        private void btn_Scan_Clicked(object sender, EventArgs e)
        {
            if (btn_Scan.Text == "Stop")
            {
                viewModel.StopInventory();
                btn_Scan.Text = "Scan";
                return;
            }

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
                viewModel.SatrtContenuesInventary(Convert.ToByte(viewModel.Anti), (byte)viewModel.q);
            }
            else
            {
                viewModel.StartInventorySingl();
            }

        }

        private void SaveRfids_Clicked(object sender, EventArgs e)
        {
            int ID_stock = 15897;
            List<STK_STOCK_RFID> rfids = new List<STK_STOCK_RFID>();
            if (viewModel.Items.Count == 0) return;
            foreach (string eleme in viewModel.Items) {
                STK_STOCK_RFID rfid = new STK_STOCK_RFID();
                rfid.ID_STOCK = ID_stock;
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
                var lots = await WebServiceClient.getStckFroIdStock(viewModel.IdStock);
                if (!(lots == null) && lots.Count > 0)
                {
                    this.viewModel.CurrentLot = lots[0];

                //    this.viewModel.DesignationProd = lot.DESIGNATION_PRODUIT;
                }
            }
            catch (Exception ex)
            {
               
            }
          
        }
    }
}