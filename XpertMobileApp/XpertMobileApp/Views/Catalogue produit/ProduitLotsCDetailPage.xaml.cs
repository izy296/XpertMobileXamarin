using Acr.UserDialogs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;
using Xpert.Common.WSClient.Helpers;
using XpertMobileApp.DAL;
using XpertMobileApp.Models;
using XpertMobileApp.Services;
using XpertMobileApp.ViewModels;
using XpertMobileApp.Views.Encaissement;

namespace XpertMobileApp.Views
{
	[XamlCompilation(XamlCompilationOptions.Compile)]
	public partial class ProduitLotsCDetailPage : ContentPage
	{
        ItemRowsDetailViewModel<View_STK_PRODUITS, View_STK_STOCK> viewModel;

        public ProduitLotsCDetailPage(View_STK_PRODUITS prod)
        {
            InitializeComponent();

            // Hide the menu
            NavigationPage.SetHasNavigationBar(this, false);

            if (this.viewModel == null)
            {
                BindingContext = viewModel = new ItemRowsDetailViewModel<View_STK_PRODUITS, View_STK_STOCK>(prod, prod.CODE_PRODUIT);
                this.viewModel.LoadRowsCommand = new Command(async () => await ExecuteLoadRowsCommand());
            }
            viewModel.LoadRowsCommand.Execute(null);

            viewModel.Title = prod.DESIGNATION;

            this.UpdateSalesInfos();
        }

        private void UpdateSalesInfos()
        {
            if(App.CurrentSales.Details != null)
                Btn_HeaderCart.Text = "(" + App.CurrentSales.Details.Count() + ")";
        }

        async void Cancel_Clicked(object sender, EventArgs e)
        {
            await Navigation.PopAsync();
        }

        

        private async void RefBtn_Clicked(object sender, EventArgs e)
        {            
            await Navigation.PushAsync(new ProduitRefDetailPage(viewModel.Item.REFERENCE));            
        }

        private void AddNewRow(View_STK_PRODUITS product)
        {
            if (App.CurrentSales.Details == null)
                App.CurrentSales.Details = new List<View_VTE_VENTE_PRODUIT>();

            var row = App.CurrentSales.Details.Where(e => e.CODE_PRODUIT == product.CODE_PRODUIT).FirstOrDefault();

            if (row == null)
            {
                row = new View_VTE_VENTE_PRODUIT();
                //row.CODE_VENTE = Item.CODE_VENTE;
                row.CODE_PRODUIT = product.CODE_PRODUIT;
                row.IMAGE_URL = product.IMAGE_URL;
                row.CODE_BARRE_PRODUIT = product.CODE_BARRE;
                row.DESIGNATION_PRODUIT = product.DESIGNATION_PRODUIT;
                row.PRIX_VTE_TTC = product.PRIX_VENTE_HT; // TODO mettre le bon prix
                row.QUANTITE = 1;

                App.CurrentSales.Details.Add(row);
            }
            else
            {
                row.QUANTITE += 1;
            }

            row.Index = App.CurrentSales.Details.Count();

           
        }

        private void cmd_Buy_Clicked(object sender, EventArgs e)
        {
            AddNewRow(viewModel.Item);

            this.UpdateSalesInfos();
        }

        private async void Btn_HeaderCart_Clicked(object sender, EventArgs e)
        {
            await Navigation.PushAsync(new CommandeSummaryPage(App.CurrentSales));
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

            ItemsListView.HeightRequest = (90 * items.Count) + (10 * items.Count);
        }

        async Task ExecuteLoadRowsCommand()
        {
            if (IsBusy)
                return;

            IsBusy = true;

            try
            {
                viewModel.ItemRows.Clear();
                var itemsC = await WebServiceClient.GetLots(viewModel?.Item?.CODE_PRODUIT);

                UpdateItemIndex(itemsC);

                foreach (var itemC in itemsC)
                {
                    viewModel.ItemRows.Add(itemC);
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
    }
}