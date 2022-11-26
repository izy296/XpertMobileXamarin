using Acr.UserDialogs;
using Rg.Plugins.Popup.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;
using Xpert.Common.WSClient.Helpers;
using XpertMobileApp.Api.Managers;
using XpertMobileApp.Api.Services;
using XpertMobileApp.DAL;
using XpertMobileApp.Helpers;

using XpertMobileApp.Services;
using XpertMobileApp.SQLite_Managment;
using XpertMobileApp.ViewModels;
using ZXing.Net.Mobile.Forms;

namespace XpertMobileApp.Views
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class NewCommandePage : ContentPage
    {
        ItemRowsDetailViewModel<View_VTE_VENTE, View_VTE_VENTE_LOT> viewModel;
        CommandesViewModel CommandViewModel;
        public string CurrentStream = Guid.NewGuid().ToString();
        private View_VTE_VENTE item;
        public View_VTE_VENTE Item
        {
            get { return item; }
            set { item = value; }
        }
        public GoogleVisionBS gvsScannedBarcode;
        public NewCommandePage(View_VTE_VENTE vente, View_TRS_TIERS tiers = null)
        {
            InitializeComponent();

            itemSelector = new ProductSelector();
            TiersSelector = new TiersSelector(CurrentStream);
            CommandViewModel = new CommandesViewModel();

            this.Item = vente == null ? new View_VTE_VENTE() : vente;
            if (vente == null) // new item init object
            {
                this.Item.TYPE_VENTE = "CC";
                this.Item.DATE_VENTE = DateTime.Now;
                this.Item.DATE_ECHEANCE = DateTime.Now;
            }

            BindingContext = this.viewModel = new ItemRowsDetailViewModel<View_VTE_VENTE, View_VTE_VENTE_LOT>(this.Item, this.Item?.CODE_VENTE);

            // Si le tiers n'est pa null.
            // c'est à dire nous venons de la fiche tiers.
            // on n'as pas besoin de selectionner un tiers.
            // on affecte le tiers selectionner dans la fiche tiers.
            // et on l'affecte a selectedtiers pour la creation d'une commande.
            if (tiers != null)
            {
                tiersSelectorContainer.IsVisible = TiersSelectorLabel.IsVisible = false;
                viewModel.SelectedTiers = tiers;
                viewModel.Item.CODE_TIERS = tiers.CODE_TIERS;
            }

            this.viewModel.LoadRowsCommand = new Command(async () => await ExecuteLoadRowsCommand());

            MessagingCenter.Subscribe<ProductSelector, View_STK_PRODUITS>(this, MCDico.ITEM_SELECTED, async (obj, selectedItem) =>
            {
                Device.BeginInvokeOnMainThread(() =>
                {
                    this.AddNewRow(selectedItem);
                });
            });

            MessagingCenter.Subscribe<TiersSelector, View_TRS_TIERS>(this, CurrentStream, async (obj, selectedItem) =>
            {
                Device.BeginInvokeOnMainThread(() =>
                {
                    //ent_SelectedTiers.Text = selectedItem.NOM_TIERS1;
                    viewModel.SelectedTiers = selectedItem;
                    viewModel.Item.CODE_TIERS = selectedItem.CODE_TIERS;
                });
            });

            MessagingCenter.Subscribe<ProductSelector, View_STK_PRODUITS>(this, MCDico.REMOVE_ITEM, async (obj, selectedItem) =>
            {
                Device.BeginInvokeOnMainThread(() =>
                {
                    this.RemoveNewRow(selectedItem);
                });
            });
        }

        private void RemoveNewRow(View_STK_PRODUITS product)
        {
            var row = viewModel.ItemRows.Where(e => e.CODE_PRODUIT == product.CODE_PRODUIT).FirstOrDefault();
            if (row == null) return;

            if (row.QUANTITE > 0)
            {
                row.QUANTITE -= 1;
            }
            else
            {
                viewModel.ItemRows.Remove(row);
            }
        }

        private void AddNewRow(View_STK_PRODUITS product)
        {
            var row = viewModel.ItemRows.Where(e => e.CODE_PRODUIT == product.CODE_PRODUIT).FirstOrDefault();

            if (row == null)
            {
                row = new View_VTE_VENTE_LOT();
                row.CODE_VENTE = Item.CODE_VENTE;
                row.CODE_PRODUIT = product.CODE_PRODUIT;
                row.CODE_BARRE_PRODUIT = product.CODE_BARRE;
                row.DESIGNATION_PRODUIT = product.DESIGNATION_PRODUIT;
                row.PRIX_VTE_HT = product.PRIX_VENTE_HT; // TODO mettre le bon prix
                row.QUANTITE = 1;

                viewModel.ItemRows.Add(row);
            }
            else
            {
                row.QUANTITE += 1;
            }

            row.Index = viewModel.ItemRows.Count();
        }

        protected override void OnAppearing()
        {
            base.OnAppearing();

            if (this.Item != null && !string.IsNullOrEmpty(this.Item.CODE_VENTE))
            {
                viewModel.LoadRowsCommand.Execute(null);
            }
        }

        async Task ExecuteLoadRowsCommand()
        {
            if (string.IsNullOrEmpty(this.Item?.CODE_VENTE)) return;

            if (IsBusy)
                return;

            IsBusy = true;

            try
            {
                viewModel.ItemRows.Clear();
                var itemsC = await WebServiceClient.GetCommandeDetails(this.Item.CODE_VENTE);

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

        private void UpdateItemIndex<T>(List<T> items)
        {
            int i = 0;
            foreach (var item in items)
            {
                i += 1;
                (item as BASE_CLASS).Index = i;
            }
        }

        private ProductSelector itemSelector;
        private async void RowSelect_Clicked(object sender, EventArgs e)
        {
            await PopupNavigation.Instance.PushAsync(itemSelector);
        }

        private TiersSelector TiersSelector;
        private async void btn_Select_Clicked(object sender, EventArgs e)
        {
            TiersSelector.pargentPage = this;
            await PopupNavigation.Instance.PushAsync(TiersSelector);
        }

        private void RowScan_Clicked(object sender, EventArgs e)
        {

            gvsScannedBarcode = new GoogleVisionBS();
            Navigation.PushAsync(gvsScannedBarcode);
            gvsScannedBarcode.UserSubmitted += async (_, result) =>
            {
                Device.BeginInvokeOnMainThread(async () =>
                {
                    await Navigation.PopAsync();
                    await AddScanedProduct(result);
                });
            };
        }

        async Task<bool> AddScanedProduct(string cb_prod)
        {
            // Cas prdouit déjà ajouté
            var row = viewModel.ItemRows.Where(e => e.CODE_BARRE_PRODUIT == cb_prod).FirstOrDefault();
            if (row != null)
            {
                row.QUANTITE += 1;
                return true;
            }
            List<View_STK_PRODUITS> prods;
            // Cas prdouit pas déjà ajouté
            if (App.Online)
            {
                prods = await CrudManager.Products.SelectProduitByCodeBarre(cb_prod);
                if (prods.Count > 1)
                {
                    await UserDialogs.Instance.AlertAsync("Plusieurs produits pour ce code barre!", AppResources.alrt_msg_Alert, AppResources.alrt_msg_Ok);
                    return false;
                }
                else if (prods.Count == 0)
                {
                    await UserDialogs.Instance.AlertAsync("Aucun produit pour ce code barre!", AppResources.alrt_msg_Alert, AppResources.alrt_msg_Ok);
                    return false;
                }
                AddNewRow(prods[0]);
                return true;
            }

            else
            {
                prods = await SQLite_Manager.GetProductByBarCode(cb_prod);
            }


            if (prods != null)
            {
                if (prods.Count > 1)
                {
                    await UserDialogs.Instance.AlertAsync("Plusieurs produits pour ce code barre!", AppResources.alrt_msg_Alert, AppResources.alrt_msg_Ok);
                    return false;
                }
                else if (prods.Count == 0)
                {
                    await UserDialogs.Instance.AlertAsync("Aucun produit pour ce code barre!", AppResources.alrt_msg_Alert, AppResources.alrt_msg_Ok);
                    return false;
                }
                AddNewRow(prods[0]);
                return true;
            }
            return false;
        }

        async void Save_Clicked(object sender, EventArgs e)
        {
            try
            {
                this.Item.Details = viewModel.ItemRows.ToList();
                if (viewModel.SelectedTiers != null)
                {
                    if (string.IsNullOrEmpty(Item.CODE_VENTE))
                    {

                        View_VTE_COMMANDE obj = new View_VTE_COMMANDE();
                        obj = XpertHelper.CloneObject<View_VTE_COMMANDE>(Item);
                        if (App.Online)
                        {
                            await CommandViewModel.ExecuteAddItemCommand(obj);
                        }
                        else
                        {
                            await SQLite_Manager.AjoutCommande(obj);
                        }
                    }
                    else
                    {
                        MessagingCenter.Send(App.MsgCenter, MCDico.UPDATE_ITEM, Item);
                    }
                    await Navigation.PopModalAsync();
                }
                else
                {
                    await UserDialogs.Instance.AlertAsync("Veuillez sélectionner un client svp !", AppResources.alrt_msg_Alert, AppResources.alrt_msg_Ok);
                }


            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        async void Cancel_Clicked(object sender, EventArgs e)
        {
            await Navigation.PopModalAsync();
        }
    }
}