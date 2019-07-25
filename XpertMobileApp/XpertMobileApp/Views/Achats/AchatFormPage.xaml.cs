using Acr.UserDialogs;
using Rg.Plugins.Popup.Services;
using Syncfusion.SfNumericTextBox.XForms;
using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Linq;
using System.Threading.Tasks;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;
using Xpert.Common.WSClient.Helpers;
using XpertMobileApp.Api.Managers;
using XpertMobileApp.DAL;
using XpertMobileApp.Helpers;
using XpertMobileApp.Models;
using XpertMobileApp.Services;
using XpertMobileApp.Views.Achats;
using ZXing.Net.Mobile.Forms;

namespace XpertMobileApp.Views
{
	[XamlCompilation(XamlCompilationOptions.Compile)]
	public partial class AchatFormPage : ContentPage
	{
        AchatsFormViewModel viewModel;

        public Command AddItemCommand { get; set; }

        public AchatFormPage(View_ACH_DOCUMENT vente, string typeDoc)
        {
            InitializeComponent();

            itemSelector = new ProductSelector();
            TiersSelector = new TiersSelector();
            ChauffeurSelector = new ChauffeurSelector();
            EmballageSelector = new EmballageSelector();

            var ach = vente == null ? new View_ACH_DOCUMENT() : vente;
            ach.TYPE_DOC = typeDoc;

            BindingContext = this.viewModel = new AchatsFormViewModel(ach, ach?.CODE_DOC);

            this.viewModel.Title = string.IsNullOrEmpty(ach.CODE_DOC) ? AppResources.pn_NewPurchase : ach?.ToString();

            this.viewModel.LoadRowsCommand = new Command(async () => await ExecuteLoadRowsCommand());

            viewModel.ItemRows.CollectionChanged += ItemsRowsChanged;

            MessagingCenter.Subscribe<ProductSelector, View_STK_PRODUITS>(this, MCDico.ITEM_SELECTED, async (obj, selectedItem) =>
            {
                Device.BeginInvokeOnMainThread(() =>
                {
                    this.AddNewRow(selectedItem);
                });
            });

            MessagingCenter.Subscribe<TiersSelector, View_TRS_TIERS>(this, MCDico.ITEM_SELECTED, async (obj, selectedItem) =>
            {
                Device.BeginInvokeOnMainThread(() =>
                {
                    viewModel.SelectedTiers = selectedItem;
                    viewModel.Item.CODE_TIERS = selectedItem.CODE_TIERS;
                    viewModel.Item.TIERS_NomC = selectedItem.NOM_TIERS1;
                });
            });

            MessagingCenter.Subscribe<ChauffeurSelector, BSE_CHAUFFEUR>(this, MCDico.ITEM_SELECTED, async (obj, selectedItem) =>
            {
                Device.BeginInvokeOnMainThread(() =>
                {
                    viewModel.SelectedChauffeur = selectedItem;
                    viewModel.Item.CODE_CHAUFFEUR = selectedItem.CODE_CHAUFFEUR;
                    viewModel.Item.NOM_CHAUFFEUR = selectedItem.NOM_CHAUFFEUR;
                });
            });

            MessagingCenter.Subscribe<EmballageSelector, List<View_BSE_EMBALLAGE>>(this, MCDico.ITEM_SELECTED, async (obj, items) =>
            {
                Device.BeginInvokeOnMainThread(() =>
                {
                    currentRow.Embalages = items;
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

        private void ItemsRowsChanged(object sender, NotifyCollectionChangedEventArgs e)
        {
            UpdateTotaux();
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
                row = new View_ACH_DOCUMENT_DETAIL();
                row.ParentDoc = viewModel.Item;
                row.CODE_DOC = viewModel.Item.CODE_DOC;
                row.CODE_PRODUIT = product.CODE_PRODUIT;
                row.CODE_BARRE = product.CODE_BARRE;
                row.DESIGNATION_PRODUIT = product.DESIGNATION_PRODUIT;
                row.PRIX_UNITAIRE = product.PRIX_ACHAT_TTC; // TODO mettre le bon prix
                row.QUANTITE = 1;

                if (viewModel.ItemRows.Count == 0)
                {
                    row.IS_PRINCIPAL = true;
                    row.PESEE_BRUTE = viewModel.Item.PESEE_BRUTE;
                }

                viewModel.ItemRows.Add(row);
                this.viewModel.Item.Details = viewModel.ItemRows.ToList();


            }
            else
            {
                row.QUANTITE += 1;
            }

            viewModel.Item.TOTAL_TTC = viewModel.ItemRows.Sum(e => e.MT_TTC * e.QUANTITE);
            row.Index = viewModel.ItemRows.Count();
        }

        protected override void OnAppearing()
        {
            base.OnAppearing();

            ne_PESEE_ENTREE.IsEnabled = string.IsNullOrEmpty(this.viewModel.Item.CODE_DOC);
            ne_PESEE_SORTIE.IsEnabled = !string.IsNullOrEmpty(this.viewModel.Item.CODE_DOC);

            if (viewModel.Item != null && !string.IsNullOrEmpty(this.viewModel.Item.CODE_DOC))
            { 
                viewModel.LoadRowsCommand.Execute(null);
            }
        }


        async Task ExecuteLoadRowsCommand()
        {
            if (string.IsNullOrEmpty(this.viewModel.Item?.CODE_DOC)) return;

            if (IsBusy)
                return;

            IsBusy = true;

            try
            {
                viewModel.ItemRows.Clear();
                var itemsC = await WebServiceClient.GetAchatsDetails(this.viewModel.Item.CODE_DOC);

                UpdateItemIndex(itemsC);

                foreach (var itemC in itemsC)
                {
                    viewModel.ItemRows.Add(itemC);
                }

                UpdateTotaux();
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
            await PopupNavigation.Instance.PushAsync(TiersSelector);
        }

        private ChauffeurSelector ChauffeurSelector;
        private async void btn_ChauffeurSelect_Clicked(object sender, EventArgs e)
        {
            await PopupNavigation.Instance.PushAsync(ChauffeurSelector);
        }

        private void btn_SelectImmat_Clicked(object sender, EventArgs e)
        {

        }
        
        private void RowScan_Clicked(object sender, EventArgs e)
        {
            var scaner = new ZXingScannerPage();
            Navigation.PushAsync(scaner);
            scaner.OnScanResult += (result) =>
            {
                scaner.IsScanning = false;
                Device.BeginInvokeOnMainThread( async() =>
                    {
                        await Navigation.PopAsync();

                        await AddScanedProduct(result.Text);

                    });
            };
        }

        private void HeaderSettings_Clicked(object sender, EventArgs e)
        {
            pnl_Header.IsVisible = !pnl_Header.IsVisible;
        }

        async Task<bool> AddScanedProduct(string cb_prod)
        {
            // Cas prdouit déjà ajouté
            var row = viewModel.ItemRows.Where(e => e.CODE_BARRE == cb_prod).FirstOrDefault();
            if(row != null)
            { 
                row.QUANTITE += 1;
                return true;
            }

            // Cas prdouit pas déjà ajouté
            List<View_STK_PRODUITS> prods = await CrudManager.Products.SelectByCodeBarre(cb_prod);

            if(prods.Count > 0)
            {
                await UserDialogs.Instance.AlertAsync("Plusieurs produits pour ce code barre!", AppResources.alrt_msg_Alert, AppResources.alrt_msg_Ok);
                return false;
            }
            else if(prods.Count == 0)
            {
                await UserDialogs.Instance.AlertAsync("Aucun produit pour ce code barre!", AppResources.alrt_msg_Alert, AppResources.alrt_msg_Ok);
                return false;
            }

            AddNewRow(prods[0]);
            return true;
        }

        
        private void RemoveRow_CLicked(object sender, EventArgs e)
        {

        }

        private async void cmd_Buy_Clicked(object sender, EventArgs e)
        {
            this.viewModel.Item.Details = viewModel.ItemRows.ToList();
            this.viewModel.Item.CODE_TIERS = App.User.CODE_TIERS;
            this.viewModel.Item.DATE_DOC = DateTime.Now;

            if (string.IsNullOrEmpty(viewModel.Item.CODE_DOC))
            {
                await CrudManager.Achats.AddItemAsync(viewModel.Item);
                App.CurrentSales = null;
                await UserDialogs.Instance.AlertAsync(AppResources.txt_Cat_CommandesSaved, AppResources.alrt_msg_Alert, AppResources.alrt_msg_Ok);
            }
            else
            {
                await CrudManager.Achats.UpdateItemAsync(viewModel.Item);
                await UserDialogs.Instance.AlertAsync(AppResources.txt_Cat_CommandesUpdated, AppResources.alrt_msg_Alert, AppResources.alrt_msg_Ok);
            }
            
            for (var counter = 1; counter < 2; counter++)
            {
                Navigation.RemovePage(Navigation.NavigationStack[Navigation.NavigationStack.Count - 2]);
            }            
            await Navigation.PopAsync();
        }

        private EmballageSelector EmballageSelector;
        private View_ACH_DOCUMENT_DETAIL currentRow;
        private async void Btn_SelectCaiss_Clicked(object sender, EventArgs e)
        {
            currentRow = (sender as Button).BindingContext as View_ACH_DOCUMENT_DETAIL;
            await PopupNavigation.Instance.PushAsync(EmballageSelector);
        }

        private async void Btn_Delete_Clicked(object sender, EventArgs e)
        {
            var prodId = (sender as Button).ClassId;
            var vteD = viewModel.ItemRows.Where(x => x.CODE_PRODUIT == prodId).FirstOrDefault();
            if (vteD != null)
            {
                if (await UserDialogs.Instance.ConfirmAsync(AppResources.txt_ConfimDelProductCmd, AppResources.msg_Confirmation, AppResources.alrt_msg_Ok, AppResources.alrt_msg_Cancel))
                {
                    viewModel.ItemRows.Remove(vteD);
                }
            }
        }

        private void UpdateTotaux()
        {
            viewModel.Item.TOTAL_TTC = viewModel.ItemRows.Sum(x => x.MT_TTC);
        }

        private void NUD_Qte_ValueChanged(object sender, Syncfusion.SfNumericUpDown.XForms.ValueEventArgs e)
        {
            UpdateTotaux();
        }

        private void ne_PeseeBruteChanged(object sender, Syncfusion.SfNumericTextBox.XForms.ValueEventArgs e)
        {

            View_ACH_DOCUMENT_DETAIL currentItem = (sender as SfNumericTextBox).BindingContext as View_ACH_DOCUMENT_DETAIL;

            var principalItem = viewModel.ItemRows.ToList().Find(x => x.IS_PRINCIPAL = true);
            principalItem.PESEE_BRUTE = viewModel.Item.PESEE_BRUTE;

            foreach (var item in viewModel.ItemRows.ToList())
            {
                if (!item.IS_PRINCIPAL)
                {
                    principalItem.PESEE_BRUTE = principalItem.PESEE_BRUTE - item.PESEE_BRUTE;
                }
            }

            viewModel.Item.TOTAL_TTC = viewModel.ItemRows.Sum(x => x.MT_TTC);
        }
    }
}