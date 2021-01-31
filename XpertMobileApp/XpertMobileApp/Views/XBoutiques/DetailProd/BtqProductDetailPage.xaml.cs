using Acr.UserDialogs;
using SampleBrowser.SfListView;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Input;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;
using Xpert.Common.WSClient.Helpers;
using XpertMobileApp.Api;
using XpertMobileApp.Api.Managers;
using XpertMobileApp.DAL;
using XpertMobileApp.Helpers;
using XpertMobileApp.Models;
using XpertMobileApp.Services;
using XpertMobileApp.ViewModels;
using XpertMobileApp.Views.Encaissement;
using XpertMobileApp.Views.Feedback;

namespace XpertMobileApp.Views
{
	[XamlCompilation(XamlCompilationOptions.Compile)]
	public partial class BtqProductDetailPage : ContentPage
	{
        ItemRowsDetailViewModel<Product, Product> viewModel;

        public BtqProductDetailPage(Product prod, bool extraOptions = false)
        {
            InitializeComponent();

            // Hide the menu
            NavigationPage.SetHasNavigationBar(this, false);

            if (this.viewModel == null)
            {
                BindingContext = viewModel = new ItemRowsDetailViewModel<Product, Product>(prod, prod.Id);
               // this.viewModel.LoadRowsCommand = new Command(async () => await ExecuteLoadRowsCommand());
            }
            // viewModel.LoadRowsCommand.Execute(null);

            viewModel.Title = prod.Name;

            this.UpdateSalesInfos();
        }

        protected override void OnAppearing()
        {
            base.OnAppearing();

            // var itm = await BoutiqueManager.GetProduitDetail(viewModel.Item.Id);
            // viewModel.Item.Description = itm.DESCRIPTION;
        }

        public void UpdateSalesInfos()
        {
            if(BoutiqueManager.PanierElem != null)
                Btn_HeaderCart.Text = "(" + BoutiqueManager.PanierElem.Count() + ")";
        }

        async void Cancel_Clicked(object sender, EventArgs e)
        {
            await Navigation.PopAsync();
        }

        private async void AddNewRow(Product product)
        {
            if (BoutiqueManager.PanierElem == null)
                BoutiqueManager.PanierElem = new List<View_PANIER>();
            
            var row = BoutiqueManager.PanierElem.Where(e => e.CODE_PRODUIT == product.Id).FirstOrDefault();

            if (row == null)
            {
                  row = new View_PANIER()
                  {
                      CODE_PRODUIT = product.Id,
                      DESIGNATION = product.Name,
                      ID_USER = App.User.Id,
                      //IMAGE_URL = product.Image,
                      QUANTITE = 1
                  };

                  row.QUANTITE = 1;

                  BoutiqueManager.PanierElem.Add(row);
            }
            else
            {
                row.QUANTITE += 1;
            }

            // 1 - Mise à jours du panier sur le serveur
            try
            {
                addToCard itm = new addToCard()
                {
                    CODE_PRODUIT = product.Id,
                    //ID_PANIER = BoutiqueManager.PanierElem[0]?.ID_PANIER,
                    ID_USER = App.User.Id,
                    QUANTITE = 1
                };
                UserDialogs.Instance.ShowLoading(AppResources.txt_Waiting);
                string res = await BoutiqueManager.AddCartItem(itm);
                row.ID_PANIER = res;
                UserDialogs.Instance.HideLoading();
            }
            catch
            {
                UserDialogs.Instance.HideLoading();
                // await UserDialogs.Instance.AlertAsync(WSApi2.GetExceptionMessage(ex), AppResources.alrt_msg_Alert, AppResources.alrt_msg_Ok);
            }

            // row.Index = App.CurrentSales.Details.Count();

        }

        private void AddLotToCart_Clicked(object sender, EventArgs e)
        {
            View_STK_STOCK lot = (sender as Button).BindingContext as View_STK_STOCK;

           // View_STK_STOCK lot = this.BindingContext as View_STK_STOCK;

            if (App.CurrentSales.Details == null)
                App.CurrentSales.Details = new List<View_VTE_VENTE_LOT>();

            var row = App.CurrentSales.Details.Where(x => x.ID_STOCK == lot.ID_STOCK).FirstOrDefault();

            if (row == null)
            {
                row = new View_VTE_VENTE_LOT();
                //row.CODE_VENTE = Item.CODE_VENTE;
                row.CODE_PRODUIT = lot.CODE_PRODUIT;
                row.IMAGE_URL = lot.IMAGE_URL;
                row.ID_STOCK = lot.ID_STOCK;
                row.CODE_BARRE_PRODUIT = lot.CODE_BARRE;
                row.DESIGNATION_PRODUIT = lot.DESIGNATION_PRODUIT;
                row.PRIX_VTE_TTC = lot.PRIX_VENTE; // TODO mettre le bon prix
                row.QUANTITE = 1;

                App.CurrentSales.Details.Add(row);
            }
            else
            {
                row.QUANTITE += 1;
            }

            row.Index = App.CurrentSales.Details.Count();
            UpdateSalesInfos();
        }

        private void cmd_Buy_Clicked(object sender, EventArgs e)
        {
            AddNewRow(viewModel.Item);

            this.UpdateSalesInfos();
        }

        private async void Btn_HeaderCart_Clicked(object sender, EventArgs e)
        {
            if(App.CurrentSales != null && App.CurrentSales.Details != null && App.CurrentSales.Details.Count > 0 ) 
            { 
                await Navigation.PushAsync(new BtqPanierPage());
            }
        }

        private async void Btn_NavigateBack_Clicked(object sender, EventArgs e)
        {
            await Navigation.PopAsync();
        }

        private void UpdateItemIndex<T>(List<T> items)
        {
            int i = 0;
            foreach (var item in items)
            {
                i += 1;
                (item as BASE_CLASS).Index = i;
            }

           // lisview.HeightRequest = (90 * items.Count) + (10 * items.Count);
        }

        async Task ExecuteLoadRowsCommand()
        {
            if (IsBusy)
                return;

            IsBusy = true;

            try
            {
                viewModel.ItemRows.Clear();
                var itemsC = await WebServiceClient.GetLots(viewModel?.Item?.Id);

                UpdateItemIndex(itemsC);

                foreach (var itemC in itemsC)
                {
                  ///  viewModel.ItemRows.Add(itemC);
                }
            }
            catch (Exception ex)
            {
                await UserDialogs.Instance.AlertAsync(WSApi2.GetExceptionMessage(ex), AppResources.alrt_msg_Alert,
                    AppResources.alrt_msg_Ok);
            }
            finally
            {
                IsBusy = false;
            }
        }

        private async void cmd_Rate_Clicked(object sender, EventArgs e)
        {
            await Navigation.PushAsync(new ReviewPage(viewModel.Item));
        }

        private async void allReviews_Clicked(object sender, EventArgs e)
        {
            await Navigation.PushAsync(new FeedbackPage());
        }
    }
}