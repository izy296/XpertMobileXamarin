using Acr.UserDialogs;
using Rg.Plugins.Popup.Services;
using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Linq;
using System.Threading.Tasks;
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
using ZXing.Net.Mobile.Forms;

namespace XpertMobileApp.Views
{
	[XamlCompilation(XamlCompilationOptions.Compile)]
	public partial class BtqPanierPage : ContentPage
	{
        ItemRowsDetailViewModel<PANIER, View_PANIER> viewModel;
        public string CurrentStream = Guid.NewGuid().ToString();
        public Command AddItemCommand { get; set; }

        public BtqPanierPage()
        {
            InitializeComponent();

            itemSelector = new ProductSelector();
            TiersSelector = new TiersSelector(CurrentStream);

            BindingContext = this.viewModel = new ItemRowsDetailViewModel<PANIER, View_PANIER>(null, "");

            viewModel.ItemRows.CollectionChanged += ItemsRowsChanged;

            // this.viewModel.LoadRowsCommand = new Command(async () => await ExecuteLoadRowsCommand());

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
                    //viewModel.SelectedTiers = selectedItem;
                    // viewModel.Item.CODE_TIERS = selectedItem.CODE_TIERS;
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
                /*
                row = new View_VTE_VENTE_LOT();
                row.CODE_VENTE = viewModel.Item.CODE_VENTE;
                row.CODE_PRODUIT = product.CODE_PRODUIT;
                row.IMAGE_URL = product.IMAGE_URL;
                row.CODE_BARRE_PRODUIT = product.CODE_BARRE;
                row.DESIGNATION_PRODUIT = product.DESIGNATION_PRODUIT;
                row.PRIX_VTE_TTC = product.PRIX_VENTE_HT; // TODO mettre le bon prix
                row.QUANTITE = 1;

                viewModel.ItemRows.Add(row);
                */
            }
            else
            {
                row.QUANTITE += 1;
            }

            // viewModel.Item.TOTAL_TTC = viewModel.ItemRows.Sum(e => e.MT_TTC * e.QUANTITE);
            // row.Index = viewModel.ItemRows.Count();
        }

        protected override void OnAppearing()
        {
            base.OnAppearing();

            if(viewModel.Item != null) // && !string.IsNullOrEmpty(this.viewModel.Item.CODE_VENTE)
            { 
                viewModel.LoadRowsCommand.Execute(null);
            }
        }


        async Task ExecuteLoadRowsCommand()
        {



            if (IsBusy)
                return;

            IsBusy = true;

            try
            {
                viewModel.ItemRows.Clear();
                var itemsC = await BoutiqueManager.GetPanier();

                // UpdateTotaux();

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
            await PopupNavigation.Instance.PushAsync(TiersSelector);
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

        async Task<bool> AddScanedProduct(string cb_prod)
        {
            /*
            // Cas prdouit déjà ajouté
            var row = viewModel.ItemRows.Where(e => e.CODE_BARRE_PRODUIT == cb_prod).FirstOrDefault();
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
            */
            return true;
        }

        
        private void RemoveRow_CLicked(object sender, EventArgs e)
        {

        }

        private async void cmd_Buy_Clicked(object sender, EventArgs e)
        {
            /*
            this.viewModel.Item.Details = viewModel.ItemRows.ToList();
            this.viewModel.Item.CODE_TIERS = App.User.CODE_TIERS;
            this.viewModel.Item.DATE_VENTE = DateTime.Now;
            try
            {
                UserDialogs.Instance.ShowLoading(AppResources.txt_Waiting);
                if (string.IsNullOrEmpty(viewModel.Item.CODE_VENTE))
                {
                    await CrudManager.Commandes.AddItemAsync(viewModel.Item);
                    App.CurrentSales = null;
                    await UserDialogs.Instance.AlertAsync(AppResources.txt_Cat_CommandesSaved, AppResources.alrt_msg_Alert, AppResources.alrt_msg_Ok);
                }
                else
                {
                    await CrudManager.Commandes.UpdateItemAsync(viewModel.Item);
                    await UserDialogs.Instance.AlertAsync(AppResources.txt_Cat_CommandesUpdated, AppResources.alrt_msg_Alert, AppResources.alrt_msg_Ok);
                }
            
                for (var counter = 1; counter < 2; counter++)
                {
                    Navigation.RemovePage(Navigation.NavigationStack[Navigation.NavigationStack.Count - 2]);
                }            
                await Navigation.PopAsync();
            }
            catch(Exception ex)
            {
                await UserDialogs.Instance.AlertAsync(ex.Message, AppResources.alrt_msg_Alert, AppResources.alrt_msg_Ok);
            }
            finally
            {
                UserDialogs.Instance.HideLoading();
            }
            */
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
           // viewModel.Item.TOTAL_TTC = viewModel.ItemRows.Sum(x => x.MT_TTC);
        }

        private void NUD_Qte_ValueChanged(object sender, Syncfusion.SfNumericUpDown.XForms.ValueEventArgs e)
        {
            UpdateTotaux();
        }
    }
}