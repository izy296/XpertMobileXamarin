using Rg.Plugins.Popup.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Extended;
using Xamarin.Forms.Xaml;
using XpertMobileApp.DAL;
using XpertMobileApp.Helpers;
using XpertMobileApp.ViewModels;
using static XpertMobileApp.Views._04_Comercial.Achats.NewAchatPage;

namespace XpertMobileApp.Views._04_Comercial.Achats
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class AchatProduitSelectore : ContentPage
    {
        ProductSelectorViewModel viewModel;

        private View_STK_PRODUITS productSelected;
        private View_ACH_DOCUMENT_DETAIL itemRow;
        public View_ACH_DOCUMENT_DETAIL ItemRow
        {
            get
            {
                return itemRow;
            }
            set
            {
                itemRow = value;
            }
        }
        public View_STK_PRODUITS ProductSelected
        {
            get
            {
                return productSelected;
            }
            set
            {
                productSelected = value;
            }
        }

        public AchatProduitSelectore()
        {
            InitializeComponent();
            /* Definning the Binding context */
            BindingContext = viewModel = new ProductSelectorViewModel();
        }

        protected override void OnAppearing()
        {
            base.OnAppearing();

            if (viewModel.Items.Count == 0)
                viewModel.LoadItemsCommand.Execute(null);
        }

        private async void SearchProduct(object sender, EventArgs e)
        {
            await searchIcon.ScaleTo(0.75, 50, Easing.Linear);
            await searchIcon.ScaleTo(1, 50, Easing.Linear);
            viewModel.LoadItemsCommand.Execute(null);
        }

        private async void OnItemSelected(object sender, SelectedItemChangedEventArgs e)
        {
            try
            {
                if (e.SelectedItem != null)
                {
                    ProductSelected = e.SelectedItem as View_STK_PRODUITS;
                    if (ProductSelected != null)
                    {
                        ItemRow = new View_ACH_DOCUMENT_DETAIL();
                        ItemRow.DESIGNATION_PRODUIT = ProductSelected.DESIGNATION_PRODUIT;
                        ItemRow.CODE_PRODUIT = ProductSelected.CODE_PRODUIT;
                        ItemRow.CODE_BARRE = ProductSelected.CODE_BARRE;
                        ItemRow.SHP = ProductSelected.SHP;
                        ItemRow.PPA = ProductSelected.PPA;
                        ItemRow.DESIGNATION_PRODUIT = ProductSelected.DESIGNATION_PRODUIT;
                        ItemRow.PRIX_VENTE = ProductSelected.PRIX_VENTE_HT;
                        ItemRow.QUANTITE = ProductSelected.QUANTITY_SELECTIONNER;
                        ItemRow.PRIX_UNITAIRE = ProductSelected.PRIX_UNITAIRE;
                        ItemRow.Prix_ACHAT_HT = ProductSelected.PRIX_ACHAT_HT;
                    }
                    EditPrixUnitairePopup popup = new EditPrixUnitairePopup(ItemRow, TypeOperation.Add);
                    await PopupNavigation.Instance.PushAsync(popup);
                    if (await popup.PopupClosedTask)
                    {
                        if (popup.Result != null)
                        {
                            var Product = viewModel.Items.Where(el => el.CODE_PRODUIT == ItemRow.CODE_PRODUIT).FirstOrDefault();
                            viewModel.Items = new InfiniteScrollCollection<View_STK_PRODUITS>(viewModel.Items.Where(element => element.CODE_PRODUIT != Product.CODE_PRODUIT).ToList());
                            Product.PRIX_UNITAIRE = ItemRow.PRIX_UNITAIRE = popup.Result.PRIX_UNITAIRE;
                            Product.QUANTITY_SELECTIONNER = ItemRow.QUANTITE = (decimal)popup.Result.QUANTITY;
                            viewModel.Items.Add(Product);
                        }
                        MessagingCenter.Send(this, MCDico.ITEM_SELECTED, ItemRow);
                    }
                    ItemsListView.SelectedItem = null;
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        private async void SelectProduct(object sender, EventArgs e)
        {
            if (productSelected == null)
                return;
            await Navigation.PopAsync();
            // TO-DO WHEN CLICKING on the NIKE ICON GO TO THE NEW ACHAT PAGE ;
        }

    }
}