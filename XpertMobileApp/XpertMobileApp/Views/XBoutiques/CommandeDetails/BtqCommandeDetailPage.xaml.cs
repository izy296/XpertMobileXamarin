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
using XpertMobileApp.Services;
using XpertMobileApp.ViewModels;
using XpertMobileSettingsPage.Helpers.Services;
using XpertWebApi.Models;

namespace XpertMobileApp.Views.Encaissement
{
	[XamlCompilation(XamlCompilationOptions.Compile)]
	public partial class BtqCommandeDetailPage : ContentPage
	{
        ItemRowsDetailViewModel<COMMANDES, COMMANDES_DETAILS> viewModel;

        private COMMANDES item;
        public COMMANDES Item
        {
            get { return item; }
            set { item = value; }
        }

        public BtqCommandeDetailPage(COMMANDES vente)
        {
            InitializeComponent();

            this.Item = vente;

            BindingContext = this.viewModel = new ItemRowsDetailViewModel<COMMANDES, COMMANDES_DETAILS>(vente, vente.ID);

            this.viewModel.LoadRowsCommand = new Command(async () => await ExecuteLoadRowsCommand());

            // TODO put into th generic view model 
            MessagingCenter.Subscribe<SessionsViewModel, View_VTE_VENTE>(this, MCDico.REFRESH_ITEM, async (obj, item) =>
            {
                // viewModel.Item = item;
            });
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
                var itemsC = await BoutiqueManager.GetCommandeDetails(this.Item.ID);

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
            int i = 0;
            foreach (var item in items)
            {
                i += 1;
                (item as BASE_CLASS).Index = i;
            }
        }
    }
}