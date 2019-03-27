using Rg.Plugins.Popup.Services;
using System;
using System.Linq;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;
using XpertMobileApp.DAL;
using XpertMobileApp.Helpers;
using XpertMobileApp.ViewModels;

namespace XpertMobileApp.Views
{
	[XamlCompilation(XamlCompilationOptions.Compile)]
	public partial class RfidScanPage : ContentPage
	{
        RfidScanViewModel viewModel;

        public RfidScanPage()
		{
			InitializeComponent ();

            BindingContext = viewModel = new RfidScanViewModel();
        }

        async void OnItemSelected(object sender, SelectedItemChangedEventArgs args)
        {

        }

        protected override void OnAppearing()
        {
            base.OnAppearing();

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
            
            if (viewModel.ContinuesScan)
            { 
                viewModel.StartInventorySingl();
            }
            else
            {

                viewModel.SatrtContenuesInventary(Convert.ToByte(viewModel.Anti), (byte)viewModel.q);
            }
        }

        private void btn_StopScan_Clicked(object sender, EventArgs e)
        {
            viewModel.StopInventory();
        }
    }
}