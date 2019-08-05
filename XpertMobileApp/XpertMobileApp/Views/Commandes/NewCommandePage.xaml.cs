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
using XpertMobileApp.DAL;
using XpertMobileApp.Helpers;
using XpertMobileApp.Models;
using XpertMobileApp.Services;
using XpertMobileApp.ViewModels;
using ZXing.Net.Mobile.Forms;

namespace XpertMobileApp.Views.Encaissement
{
	[XamlCompilation(XamlCompilationOptions.Compile)]
	public partial class NewCommandePage : ContentPage
	{
        ItemRowsDetailViewModel<View_VTE_VENTE, View_VTE_VENTE_PRODUIT> viewModel;

        private View_VTE_VENTE item;
        public View_VTE_VENTE Item
        {
            get { return item; }
            set { item = value; }
        }

        public NewCommandePage(View_VTE_VENTE vente)
        {
            InitializeComponent();

            itemSelector = new ProductSelector();
            TiersSelector = new TiersSelector();

            this.Item = vente == null ? new View_VTE_VENTE() : vente;
            if (vente == null) // new item init object
            {
                this.Item.TYPE_VENTE = "CC";
                this.Item.DATE_VENTE = DateTime.Now;
                this.Item.DATE_ECHEANCE = DateTime.Now;
            }

            BindingContext = this.viewModel = new ItemRowsDetailViewModel<View_VTE_VENTE, View_VTE_VENTE_PRODUIT>(this.Item, this.Item?.CODE_VENTE);

            this.viewModel.LoadRowsCommand = new Command(async () => await ExecuteLoadRowsCommand());

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
                row = new View_VTE_VENTE_PRODUIT();
                row.CODE_VENTE = Item.CODE_VENTE;
                row.CODE_PRODUIT = product.CODE_PRODUIT;
                row.CODE_BARRE_PRODUIT = product.CODE_BARRE;
                row.DESIGNATION_PRODUIT = product.DESIGNATION_PRODUIT;
                row.PRIX_VTE_TTC = product.PRIX_VENTE_HT; // TODO mettre le bon prix
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

            if(this.Item != null && !string.IsNullOrEmpty(this.Item.CODE_VENTE))
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
            return true;
        } 

        async void Save_Clicked(object sender, EventArgs e)
        {
            /*
            if (dp_EcheanceDate.Date < DateTime.Now)
            {
                await DisplayAlert(AppResources.alrt_msg_Alert, AppResources.error_DateShouldBeGreaterThanToday, AppResources.alrt_msg_Ok);
                return;
            }
            */
            this.Item.Details = viewModel.ItemRows.ToList();

            if (string.IsNullOrEmpty(Item.CODE_VENTE))
            {
                MessagingCenter.Send(App.MsgCenter, MCDico.ADD_ITEM, Item);
            }
            else
            {
                MessagingCenter.Send(App.MsgCenter, MCDico.UPDATE_ITEM, Item);
            }

            await Navigation.PopModalAsync();
        }

        async void Cancel_Clicked(object sender, EventArgs e)
        {
            await Navigation.PopModalAsync();
        }
    }
}