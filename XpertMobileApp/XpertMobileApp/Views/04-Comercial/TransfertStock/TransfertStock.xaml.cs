using Acr.UserDialogs;
using Rg.Plugins.Popup.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;
using XpertMobileApp.Api.Models;
using XpertMobileApp.DAL;
using XpertMobileApp.Models;
using XpertMobileApp.SQLite_Managment;
using XpertMobileApp.ViewModels;

namespace XpertMobileApp.Views
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class TransfertStock : ContentPage
    {

        TransfertStockViewModel viewModel;
        public TransfertStock()
        {
            InitializeComponent();

            BindingContext = viewModel = new TransfertStockViewModel();


        }

        async void OnItemSelected(object sender, Xamarin.Forms.SelectionChangedEventArgs args)
        {
            if (args.CurrentSelection.Count != 0)
            {
                var item = args.CurrentSelection[0] as STK_TRANSFERT;

                if (item == null)
                    return;

                await Navigation.PushAsync(new TransfertStockDetail(item));

                // Manually deselect item.
                ItemsListView.SelectedItem = null;
            }

        }


        protected async override void OnAppearing()
        {
            base.OnAppearing();

            if (App.CODE_MAGASIN == null && App.User.UserGroup!="AD")
            {
                var obj = await SQLite_Manager.GetInstance().Table<View_LIV_TOURNEE>().ToListAsync();
                if (obj != null && obj.Count > 0)
                {
                    App.CODE_MAGASIN = obj[0].CODE_MAGASIN;
                }
                if (App.CODE_MAGASIN == null)
                {
                    await UserDialogs.Instance.AlertAsync("Aucun Magasin est assigner a vous ,s'il vous plait synchroniser les tournes", AppResources.alrt_msg_Alert,
    AppResources.alrt_msg_Ok);
                    await((MainPage)App.Current.MainPage).NavigateFromMenu((int)MenuItemType.Synchronisation);
                }

            }

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