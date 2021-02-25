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
	public partial class ProduitCDetailPage : ContentPage
	{
        ItemDetailViewModel<View_STK_PRODUITS> viewModel;

        public ProduitCDetailPage(View_STK_PRODUITS prod)
        {
            InitializeComponent();

            // Hide the menu
            NavigationPage.SetHasNavigationBar(this, false);

            BindingContext = viewModel = new ItemDetailViewModel<View_STK_PRODUITS>(prod);

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
                App.CurrentSales.Details = new List<View_VTE_VENTE_LOT>();

            var row = App.CurrentSales.Details.Where(e => e.CODE_PRODUIT == product.CODE_PRODUIT).FirstOrDefault();

            if (row == null)
            {
                row = new View_VTE_VENTE_LOT();
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
    }
}