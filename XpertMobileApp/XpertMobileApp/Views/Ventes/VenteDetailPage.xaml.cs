using Acr.UserDialogs;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;
using Xpert.Common.WSClient.Helpers;
using XpertMobileApp.DAL;
using XpertMobileApp.Helpers;
using XpertMobileApp.Helpers.Services;
using XpertMobileApp.Services;
using XpertMobileApp.ViewModels;
using XpertWebApi.Models;

namespace XpertMobileApp.Views.Encaissement
{
	[XamlCompilation(XamlCompilationOptions.Compile)]
	public partial class VenteDetailPage : ContentPage
	{
        ItemRowsDetailViewModel<View_VTE_VENTE, View_VTE_JOURNAL_DETAIL> viewModel;

        private View_VTE_VENTE item;
        public View_VTE_VENTE Item
        {
            get { return item; }
            set { item = value; }
        }

        public VenteDetailPage(View_VTE_VENTE vente)
        {
            InitializeComponent();

            this.Item = vente;

            BindingContext = this.viewModel = new ItemRowsDetailViewModel<View_VTE_VENTE, View_VTE_JOURNAL_DETAIL>(vente, vente.CODE_VENTE);

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
                var itemsC = await WebServiceClient.GetVenteDetails(this.Item.CODE_VENTE);

                UpdateItemIndex(itemsC);

                foreach (var itemC in itemsC)
                {
                    if (itemC.PSYCHOTHROPE)
                    {
                        InfosPsyco.IsVisible = true;
                    }
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
            var tecketData = await WebServiceClient.GetDataTecketCaisseVente(item.CODE_VENTE);
            if (tecketData == null) return;
            if (tecketData.Count==0)
            {
                await DisplayAlert(AppResources.alrt_msg_Info, "Pas de donnees a imprimer !", AppResources.alrt_msg_Ok);
            }
            else
            {
                PrinterHelper.PrintBL(tecketData);
            }
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