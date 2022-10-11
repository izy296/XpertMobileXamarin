using Rg.Plugins.Popup.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;
using XpertMobileApp.Models;
using XpertMobileApp.ViewModels;

namespace XpertMobileApp.Views
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class TransfertStock : ContentPage
    {

        TransfertStockViewModel viewModel ;
        public TransfertStock()
        {
            InitializeComponent();

            BindingContext = viewModel = new TransfertStockViewModel();


        }

        async void OnItemSelected(object sender, Xamarin.Forms.SelectionChangedEventArgs args)
        {
            if (args.CurrentSelection.Count != 0)
                if (App.Online)
                {
                    var item = args.CurrentSelection[0] as STK_TRANSFERT;

                    if (item == null)
                        return;

                    await Navigation.PushAsync(new TransfertStockDetail(item));

                    // Manually deselect item.
                    ItemsListView.SelectedItem = null;
                }
        }


        protected override void OnAppearing()
        {
            base.OnAppearing();
            if (viewModel.Items.Count == 0)
                viewModel.LoadItemsCommand.Execute(null);

        }

        private async void ShowHideFilter(object sender, EventArgs e)
        {
            TransferStockPopupFilter filter = new TransferStockPopupFilter(viewModel);
            //Load data for the first time ...
            await PopupNavigation.Instance.PushAsync(filter);
        }

        private void RowScan_Clicked(object sender, EventArgs e)
        {

        }

    }
}