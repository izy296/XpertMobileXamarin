using Acr.UserDialogs;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;
using Xpert.Common.WSClient.Helpers;
using XpertMobileApp.Api;
using XpertMobileApp.DAL;
using XpertMobileApp.Helpers;
using XpertMobileApp.Models;
using XpertMobileApp.ViewModels;

namespace XpertMobileApp.Views
{
	[XamlCompilation(XamlCompilationOptions.Compile)]
	public partial class BtqCommandeDetailPage : ContentPage
	{
        ItemRowsDetailViewModel<COMMANDES, View_COMMANDES_DETAILS> viewModel;

        public BtqCommandeDetailPage(COMMANDES vente)
        {
            BindingContext = this.viewModel = new ItemRowsDetailViewModel<COMMANDES, View_COMMANDES_DETAILS>(vente, vente.ID);
            viewModel.Title = "";

            InitializeComponent();
            // this.Item = vente;

            this.viewModel.LoadRowsCommand = new Command(async () => await ExecuteLoadRowsCommand());

        }

        protected override void OnAppearing()
        {
            base.OnAppearing();

            viewModel.LoadRowsCommand.Execute(null);
        }

        async Task ExecuteLoadRowsCommand()
        {

            if (IsBusy)
                return;

            IsBusy = true;

            try
            {
                UserDialogs.Instance.ShowLoading(AppResources.txt_Loading);

                viewModel.ItemRows.Clear();
                var itemsC = await BoutiqueManager.GetCommandeDetails(this.viewModel.Item.ID);

                UpdateItemIndex(itemsC);

                foreach (var itemC in itemsC)
                {
                    viewModel.ItemRows.Add(itemC);
                }

                UserDialogs.Instance.HideLoading();
            }
            catch (Exception ex)
            {
                UserDialogs.Instance.HideLoading();
                await UserDialogs.Instance.AlertAsync(WSApi2.GetExceptionMessage(ex), AppResources.alrt_msg_Alert,
                    AppResources.alrt_msg_Ok);
            }
            finally
            {

                IsBusy = false;
            }
        }

        private async void PrintAsync(object sender, EventArgs e)
        {
            // PrinterHelper.PrintBL(item);
        }
    
        private void UpdateItemIndex<T>(List<T> items)
        {
         
        }

        private async void ItemsListView_ItemTapped(object sender, ItemTappedEventArgs e)
        {
            try
            {
                var item = e.Item as View_COMMANDES_DETAILS;
                if (item == null)
                    return;

                Product p = await BoutiqueManager.LoadProdDetails(item.CODE_PRODUIT);

                await Navigation.PushAsync(new BtqProductDetailPage(p, false, true));

                ItemsListView.SelectedItem = null;
            }
            catch (Exception ex)
            {
                await UserDialogs.Instance.AlertAsync(WSApi2.GetExceptionMessage(ex), AppResources.alrt_msg_Alert,
                    AppResources.alrt_msg_Ok);
            }
        }
    }
}