using Acr.UserDialogs;
using Rg.Plugins.Popup.Services;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Threading.Tasks;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;
using Xpert.Common.WSClient.Helpers;
using XpertMobileApp.Api.Managers;
using XpertMobileApp.Api.Services;
using XpertMobileApp.DAL;
using XpertMobileApp.Helpers;
using XpertMobileApp.Models;
using XpertMobileApp.Services;
using XpertMobileApp.SQLite_Managment;

namespace XpertMobileApp.Views._04_Comercial.Achats
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class NewAchatPage : ContentPage
    {
        public string CurrentStream = Guid.NewGuid().ToString();

        private View_TRS_TIERS selectedFournisseur;
        public View_TRS_TIERS SelectedFournisseur
        {
            get
            {
                return selectedFournisseur;
            }
            set
            {
                selectedFournisseur = value;
                OnPropertyChanged("SelectedFournisseur");
            }
        }

        private ObservableCollection<View_ACH_DOCUMENT_DETAIL> itemRows;
        public ObservableCollection<View_ACH_DOCUMENT_DETAIL> ItemRows
        {
            get { return itemRows; }
            set
            {
                itemRows = value;
                OnPropertyChanged("ItemRows");
            }
        }

        private View_ACH_DOCUMENT item;
        public View_ACH_DOCUMENT Item
        {
            get { return item; }
            set { item = value; }
        }

        private DateTime selectedDateEcheance = DateTime.Now;
        public DateTime SelectedDateEcheance
        {
            get
            {
                return selectedDateEcheance;
            }
            set
            {
                selectedDateEcheance = value;
                OnPropertyChanged("SelectedDateEcheance");
            }
        }

        private string enteredNumFacture;

        public string EnteredNumFacture
        {
            get
            {
                return enteredNumFacture;
            }
            set
            {
                enteredNumFacture = value;
                OnPropertyChanged("EnteredNumFacture");
            }
        }

        private ObservableCollection<View_BSE_MAGASIN> magasinsList;
        public ObservableCollection<View_BSE_MAGASIN> MagasinsList
        {
            get
            {
                return magasinsList;
            }
            set
            {
                magasinsList = value;
                OnPropertyChanged("MagasinsList");
            }
        }

        //private View_BSE_MAGASIN selectedMagasin;
        //public View_BSE_MAGASIN SelectedMagasin
        //{
        //    get
        //    {
        //        return selectedMagasin;
        //    }
        //    set
        //    {
        //        selectedMagasin = value;
        //        OnPropertyChanged("SelectedMagasin");
        //    }
        //}

        private ProductSelector productSelector;

        private TiersSelector itemSelector;
        public enum TypeOperation
        {
            Edit,
            Add
        }
        private string achatOperation { get; set; }
        public NewAchatPage(View_TRS_TIERS selectedSuppliers = null, View_ACH_DOCUMENT item = null)
        {
            InitializeComponent();
            if (item != null)
            {
                achatOperation = TypeOperation.Edit.ToString();
                this.item = item;
                enteredNumFacture = item.REF_TIERS;
                this.Title = "Modification Achat";
            }
            else
            {
                Item = new View_ACH_DOCUMENT();
                //SelectedMagasin = new View_BSE_MAGASIN();
            }
            /* Tiers Selector */
            itemSelector = new TiersSelector(CurrentStream);
            MessagingCenter.Subscribe<TiersSelector, View_TRS_TIERS>(this, CurrentStream, async (obj, selectedItem) =>
            {
                Device.BeginInvokeOnMainThread(() =>
                {
                    SelectedFournisseur = selectedItem;
                    Item.CODE_TIERS = selectedItem.CODE_TIERS;
                });
            });

            if (selectedSuppliers != null)
                this.SelectedFournisseur = selectedSuppliers;
            else
            {
                Item.CODE_TIERS = item.CODE_TIERS;
                this.SelectedFournisseur = new View_TRS_TIERS
                {
                    NOM_TIERS1 = item.TIERS_NomC,
                    CODE_TIERS = item.CODE_TIERS
                };
            }
            /* Tiers Selector */

            /* Product Selector */

            productSelector = new ProductSelector();

            MessagingCenter.Subscribe<AchatProduitSelectore, View_ACH_DOCUMENT_DETAIL>(this, MCDico.ITEM_SELECTED, async (obj, selectedItem) =>
            {
                Device.BeginInvokeOnMainThread(() =>
                {
                    this.AddNewRow(selectedItem);
                });
            });

            /* Product Selector*/

            ItemRows = new ObservableCollection<View_ACH_DOCUMENT_DETAIL>();
            MagasinsList = new ObservableCollection<View_BSE_MAGASIN>();

            BindingContext = this;

            if (!App.Online)
            {
                //MagasinList.IsEnabled = false;
            }

        }
        protected override async void OnAppearing()
        {
            base.OnAppearing();
            if (achatOperation == TypeOperation.Edit.ToString())
            {
                await GetAchatDetailWhenModifiying();
            }
        }

        private async void AddProduct(object sender, EventArgs e)
        {
            try
            {
                await addProduct.ScaleTo(0.75, 50, Easing.Linear);
                await addProduct.ScaleTo(1, 50, Easing.Linear);
                await Navigation.PushAsync(new AchatProduitSelectore());
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        private async void CreateNewSell(object sender, EventArgs e)
        {
            try
            {
                if (selectedFournisseur == null)
                {
                    await UserDialogs.Instance.AlertAsync("Veuillez séléctionner un Fournisseur", AppResources.alrt_msg_Alert, AppResources.alrt_msg_Ok);
                    return;
                }
                else if (EnteredNumFacture == null)
                {
                    await UserDialogs.Instance.AlertAsync("Veuillez Saisir un Numéro de facture", AppResources.alrt_msg_Alert, AppResources.alrt_msg_Ok);
                    return;
                }
                foreach (var item in ItemRows)
                {
                    if (item.PRIX_UNITAIRE == 0)
                    {
                        await UserDialogs.Instance.AlertAsync("Veuillez saisir les prix unitaires des produits", AppResources.alrt_msg_Alert, AppResources.alrt_msg_Ok);
                        return;
                    }
                }
                if (Item != null)
                {
                    Item.DATE_DOC = DateTime.Now;
                    Item.DATE_ECHEANCE = selectedDateEcheance;
                    Item.CODE_TIERS = selectedFournisseur.CODE_TIERS;
                    Item.TIERS_NomC = SelectedFournisseur.NOM_TIERS;
                    Item.REF_TIERS = EnteredNumFacture.ToString();
                    Item.STATUS_DOC = "12";
                    this.Item.Details = ItemRows.ToList();
                }
                UserDialogs.Instance.ShowLoading(AppResources.txt_Waiting);
                if (App.Online)
                {
                    foreach (var item in this.Item.Details)
                    {
                        item.CODE_MAGASIN = "01";
                        item.QTE_RECUE = item.QUANTITE;
                    }

                    Item.CODE_MAGASIN = "01";
                    if (achatOperation != TypeOperation.Edit.ToString())
                    {
                        await CrudManager.Reception.AddItemAsync(Item);
                    }
                    else
                    {
                        await CrudManager.Reception.UpdateItemAsync(Item);
                    }
                    if (achatOperation != TypeOperation.Edit.ToString())
                    {
                        CustomPopup ComfirmPopup = new CustomPopup(Message: "Achat crée avec succées", trueMessage: "Ok");
                        await PopupNavigation.Instance.PushAsync(ComfirmPopup);
                        for (var counter = 1; counter < 2; counter++)
                        {
                            Navigation.RemovePage(Navigation.NavigationStack[Navigation.NavigationStack.Count - 2]);
                        }
                    }
                    await Navigation.PopAsync();
                }
                else
                {
                    foreach (var item in this.Item.Details)
                    {
                        item.CODE_MAGASIN = "01";
                        item.QTE_RECUE = item.QUANTITE;
                        Item.TOTAL_TTC_REEL += item.PRIX_UNITAIRE;
                        Item.TOTAL_HT += item.PRIX_UNITAIRE * item.QUANTITE;
                        Item.TOTAL_MARGE += item.PRIX_VENTE - item.PRIX_UNITAIRE;
                    }

                    Item.CODE_MAGASIN = "01";
                    Item.TAUX_MARGE = (Item.TOTAL_MARGE * 100) / Item.TOTAL_HT;
                    if (achatOperation != TypeOperation.Edit.ToString())
                    {
                        await SQLite_Manager.InsertAchat(Item);
                        CustomPopup ComfirmPopup = new CustomPopup(Message: "Achat crée avec succées", trueMessage: "Ok");
                        await PopupNavigation.Instance.PushAsync(ComfirmPopup);
                        for (var counter = 1; counter < 2; counter++)
                        {
                            Navigation.RemovePage(Navigation.NavigationStack[Navigation.NavigationStack.Count - 2]);
                        }
                    }
                    else
                    {
                        CustomPopup ComfirmPopup = new CustomPopup(Message: "Modification faites avec succées", trueMessage: "Ok");
                        await PopupNavigation.Instance.PushAsync(ComfirmPopup);
                        await SQLite_Manager.UpdateAchat(Item);
                    }
                    await Navigation.PopAsync();
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
            finally
            {
                UserDialogs.Instance.HideLoading();
            }
        }

        #region Produit 
        private void AddNewRow(View_STK_PRODUITS product)
        {
            var row = this.ItemRows.Where(e => e.CODE_PRODUIT == product.CODE_PRODUIT).FirstOrDefault();

            if (row == null)
            {
                row = new View_ACH_DOCUMENT_DETAIL();
                row.DESIGNATION_PRODUIT = product.DESIGNATION_PRODUIT;
                row.CODE_PRODUIT = product.CODE_PRODUIT;
                row.CODE_BARRE = product.CODE_BARRE;
                row.SHP = product.SHP;
                row.PPA = product.PPA;
                row.DESIGNATION_PRODUIT = product.DESIGNATION_PRODUIT;
                row.PRIX_VENTE = product.PRIX_VENTE_HT;
                row.QUANTITE = 1;
                this.ItemRows.Add(row);
            }
            else
            {
                row.QUANTITE += 1;
            }
            row.Index = ItemRows.Count();
        }
        private void AddNewRow(View_ACH_DOCUMENT_DETAIL product)
        {
            var row = this.ItemRows.Where(e => e.CODE_PRODUIT == product.CODE_PRODUIT).FirstOrDefault();
            if (row == null)
            {
                row = new View_ACH_DOCUMENT_DETAIL();
                this.ItemRows.Add(product);
            }
            else
            {
                ItemRows = new ObservableCollection<View_ACH_DOCUMENT_DETAIL>(ItemRows.Where(e => e.CODE_PRODUIT != product.CODE_PRODUIT).ToList());
                row.QUANTITE = product.QUANTITE;
                row.PRIX_UNITAIRE = product.PRIX_UNITAIRE;
                ItemRows.Add(row);
            }
            row.Index = ItemRows.Count();
        }
        #endregion

        public async Task LoadMagasins()
        {
            try
            {
                // Load Magasins
                MagasinsList.Clear();
                List<View_BSE_MAGASIN> ListMagasin;
                if (App.Online)
                {
                    ListMagasin = await CrudManager.BSE_MAGASINS.GetItemsAsync() as List<View_BSE_MAGASIN>;

                    View_BSE_MAGASIN allElem = new View_BSE_MAGASIN();
                    allElem.CODE = "";
                    allElem.DESIGNATION = "Aucun";
                    MagasinsList.Add(allElem);

                    foreach (var itemC in ListMagasin)
                    {
                        MagasinsList.Add(itemC);
                    }
                }
            }
            catch (Exception ex)
            {
                await UserDialogs.Instance.AlertAsync(WSApi2.GetExceptionMessage(ex), AppResources.alrt_msg_Alert,
                    AppResources.alrt_msg_Ok);
            }
        }

        private async void CollectionView_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            try
            {
                if (e.CurrentSelection.Count > 0)
                {
                    var Produit = (e.CurrentSelection[0] as View_ACH_DOCUMENT_DETAIL);
                    if (item == null)
                        return;
                    EditPrixUnitairePopup popup = new EditPrixUnitairePopup(Produit);
                    await PopupNavigation.Instance.PushAsync(popup);
                    if (await popup.PopupClosedTask)
                    {
                        if (popup.Result != null)
                        {
                            foreach (var tom in ItemRows.Where(w => w.CODE_PRODUIT == Produit.CODE_PRODUIT))
                            {
                                tom.PRIX_UNITAIRE = popup.Result.PRIX_UNITAIRE;
                                tom.QUANTITE = (decimal)popup.Result.QUANTITY;
                            }
                        }
                    }
                }
                if (collectionView.SelectedItem != null)
                {
                    collectionView.SelectedItem = null;
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        private async void ScanProduct(object sender, EventArgs e)
        {
            try
            {
                await scanProduct.ScaleTo(0.75, 50, Easing.Linear);
                await scanProduct.ScaleTo(1, 50, Easing.Linear);

                GoogleVisionBS gvsScannedBarcode = new GoogleVisionBS();
                MainPage RootPage = Application.Current.MainPage as MainPage;
                var detail = RootPage.Detail;
                gvsScannedBarcode.UserSubmitted += async (_, scannedText) =>
                {
                    await detail.Navigation.PopAsync();
                    await AddScanedProduct(scannedText);
                };

                detail.Navigation.PushAsync(gvsScannedBarcode);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        private async Task AddScanedProduct(string scannedText)
        {
            try
            {
                // If the product already exist 
                var row = ItemRows.Where(e => e.CODE_BARRE == scannedText || e.CODE_BARRE_LOT == scannedText).FirstOrDefault();
                if (row != null)
                {
                    row.QUANTITE += 1;
                    XpertHelper.PeepScan();
                    return;
                }
                List<View_STK_PRODUITS> prods = new List<View_STK_PRODUITS>();

                if (App.Online)
                {
                    prods.Add(await CrudManager.Products.GetProductByCodeBarre(scannedText));
                }
                else
                {
                    prods = await SQLite_Manager.GetProductByBarCode(scannedText);
                }

                XpertHelper.PeepScan();

                if (prods.Count > 1)
                {
                    await UserDialogs.Instance.AlertAsync("Plusieurs produits pour ce code barre!", AppResources.alrt_msg_Alert, AppResources.alrt_msg_Ok);
                    return;
                }
                else if (prods.Count == 0)
                {
                    await UserDialogs.Instance.AlertAsync("Aucun produit pour ce code barre!", AppResources.alrt_msg_Alert, AppResources.alrt_msg_Ok);
                    return;
                }
                AddNewRow(prods[0]); // false veut dire le type de produit ajouter est une vente (pas retour)
                return;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        private void OnUpdateSwipeItemInvoked(object sender, EventArgs e)
        {

        }

        private async Task GetAchatDetailWhenModifiying()
        {
            try
            {
                if (App.Online)
                {
                    var listeDetails = await WebServiceClient.GetAchatsDetails(this.Item.CODE_DOC);
                    foreach (var item in listeDetails)
                    {
                        this.ItemRows.Add(item);
                    }
                }
                else
                {
                    // Get the details from the sqlite ....
                    var AchatList = await SQLite_Manager.GetAchatDetails(this.Item.CODE_DOC);
                    foreach (var item in AchatList)
                    {
                        this.ItemRows.Add(XpertHelper.CloneObject<View_ACH_DOCUMENT_DETAIL>(item));
                    }
                }
                UserDialogs.Instance.HideLoading();
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        private async void btn_Select_Clicked(object sender, EventArgs e)
        {
            await searchIcon.ScaleTo(0.75, 50, Easing.Linear);
            await searchIcon.ScaleTo(1, 50, Easing.Linear);

            await PopupNavigation.Instance.PushAsync(itemSelector);
        }
    }
}