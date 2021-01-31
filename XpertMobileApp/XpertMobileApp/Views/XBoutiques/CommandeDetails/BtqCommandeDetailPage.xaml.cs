using Acr.UserDialogs;
using SampleBrowser.SfListView;
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
using XpertMobileApp.Services;
using XpertMobileApp.ViewModels;
using XpertMobileSettingsPage.Helpers.Services;
using XpertWebApi.Models;

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
            var item = e.Item as View_COMMANDES_DETAILS;
            if (item == null)
                return;

            var pDetails = await BoutiqueManager.GetProduitDetail(item.CODE_PRODUIT);

            Product p = new Product()
            {
                Id        = item.CODE_PRODUIT,
                Name      = item.DESIGNATION_PRODUIT,
                Category  = pDetails.DESIGNATION_FAMILLE,
                Price     = pDetails.PRIX_VENTE,
                Description     = pDetails.DESCRIPTION,
                ReviewValue     = pDetails.NOTE,
                UserReviewValue = pDetails.NOTE_USER,
                IMAGE_URL       = pDetails.IMAGE_URL                
            };

            List<string> listImgurl = new List<string>();

            // Création des urls des images du produit
            if (pDetails.ImageList != null)
            {
                foreach (var str in pDetails.ImageList)
                {
                    string val = App.RestServiceUrl.Replace("api/", "") + string.Format("Images/GetImage?codeImage={0}", str);
                    listImgurl.Add(val);
                }
            }
            p.ImageList = listImgurl;

            await Navigation.PushAsync(new BtqProductDetailPage(p));

            // Manually deselect item.
            ItemsListView.SelectedItem = null;
        }
    }
}