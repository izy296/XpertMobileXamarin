using Acr.UserDialogs;
using Plugin.SimpleAudioPlayer;
using Rg.Plugins.Popup.Services;
using Syncfusion.SfNumericTextBox.XForms;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Threading.Tasks;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;
using Xpert.Common.WSClient.Helpers;
using XpertMobileApp.Api;
using XpertMobileApp.Api.Managers;
using XpertMobileApp.Api.Services;
using XpertMobileApp.DAL;
using XpertMobileApp.Helpers;
using XpertMobileApp.Models;
using XpertMobileApp.Services;
using XpertMobileApp.SQLite_Managment;
using XpertMobileApp.Views._04_Comercial.Selectors.Lot;
using XpertMobileApp.Views.Achats;
using ZXing.Net.Mobile.Forms;

namespace XpertMobileApp.Views
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class RSFormPage : ContentPage
    {
        private RSFormViewModel viewModel;

        SYS_MOBILE_PARAMETRE parames;

        List<SYS_OBJET_PERMISSION> permissions;

        public Command AddItemCommand { get; set; }
        public View_TRS_TIERS SelectedTiers
        {
            get
            {
                return viewModel.SelectedTiers;
            }
            set
            {
                viewModel.SelectedTiers = value;
            }
        }

        public RSFormPage(View_STK_ENTREE pEntree, string typeDoc, View_TRS_TIERS tiers = null, string codeTourneeDetails = "")
        {
            InitializeComponent();

            bool disable = true;
            var entree = pEntree == null ? new View_STK_ENTREE() : pEntree;
            if (pEntree == null)
            {
                entree.ID_Random = XpertHelper.RandomString(7);
                entree.DATE_ENTREE = DateTime.Now.Date;
                entree.PropertyChanged += Vte_PropertyChanged;
            }

            BindingContext = this.viewModel = new RSFormViewModel(entree, entree?.CODE_ENTREE);


            if (tiers == null)
            {
                SelectedTiers = new View_TRS_TIERS()
                {
                    CODE_TIERS = "CXPERTCOMPTOIR",
                    NOM_TIERS1 = "COMPTOIR"
                };
            }
            else
            {
                SelectedTiers = tiers;
                disable = false;
            }

            btn_Search.IsVisible = disable;
            retourSelector = new RetourProducts(viewModel.CurrentStream);
            TiersSelector = new TiersSelector(viewModel.CurrentStream);

            // jobFieldAutoComplete.BindingContext = viewModel;

            this.viewModel.Title = string.IsNullOrEmpty(entree.CODE_ENTREE) ? "Nouv. Retour stock" : entree?.ToString();

            this.viewModel.LoadRowsCommand = new Command(async () => await ExecuteLoadRowsCommand());

           
            MessagingCenter.Subscribe<RetourProducts, List<View_STK_STOCK>>(this, viewModel.CurrentStream, async (obj, selectedItem) =>
            {
                Device.BeginInvokeOnMainThread(() =>
                {
                    AddNewRow(selectedItem, true); // true veut dire le type de produit ajouter est un retour
                });
            });


            MessagingCenter.Subscribe<RetourProducts, View_STK_PRODUITS>(this, "REMOVE" + viewModel.CurrentStream, async (obj, selectedItem) =>
            {
                Device.BeginInvokeOnMainThread(() =>
                {
                    RemoveNewRow(selectedItem);
                });
            });

            MessagingCenter.Subscribe<TiersSelector, View_TRS_TIERS>(this, viewModel.CurrentStream, async (obj, selectedItem) =>
            {
                Device.BeginInvokeOnMainThread(() =>
                {
                    viewModel.SelectedTiers = selectedItem;
                    viewModel.Item.CODE_TIERS = selectedItem.CODE_TIERS;
                    viewModel.Item.NOM_TIERS = selectedItem.NOM_TIERS1;
                });
            });

            if (viewModel.Item != null && !string.IsNullOrEmpty(this.viewModel.Item.CODE_ENTREE))
            {
                viewModel.LoadRowsCommand.Execute(null);
            }
        }

        protected override bool OnBackButtonPressed()
        {
            Device.BeginInvokeOnMainThread(async () =>
            {
                var result = await this.DisplayAlert(AppResources.msg_Confirmation, "Voulez vous fermer la entree ?", "Oui", "Non");
                if (result) await this.Navigation.PopAsync(); // or anything else
            });

            return true;

        }
        public void AddNewRow(List<View_STK_STOCK> stocks, bool retour)
        {
            foreach (var stock in stocks)
            {
                var row = viewModel.ItemRows.Where(e => e.ID_STOCK == stock.ID_STOCK && e.QUANTITE < 0).FirstOrDefault();
                if (row == null)
                {
                    row = new View_STK_ENTREE_DETAIL();
                    decimal qte = stock.SelectedQUANTITE == 0 ? 1 : stock.SelectedQUANTITE;
                    row.CODE_ENTREE_DETAIL = Guid.NewGuid().ToString();
                    row.CODE_ENTREE = viewModel.Item.CODE_ENTREE;
                    row.ID_STOCK = stock.ID_STOCK;
                    row.CODE_PRODUIT = stock.CODE_PRODUIT;
                    row.CODE_BARRE_LOT = stock.CODE_BARRE_LOT;
                    row.CODE_BARRE = stock.CODE_BARRE;
                    row.DESIGNATION_PRODUIT = stock.DESIGNATION_PRODUIT;
                    row.HAS_NEW_ID_STOCK = stock.HAS_NEW_ID_STOCK;

                    row.QUANTITE = qte;

                    row.PropertyChanged += Row_PropertyChanged;
                    viewModel.ItemRows.Add(row);
                    this.viewModel.Item.Details = viewModel.ItemRows.ToList();
                }
                else
                {
                    row.QUANTITE = (row.QUANTITE) + stock.SelectedQUANTITE;
                }

                // calcul du total du document
                decimal prix = GetPrix(stock, row.QUANTITE);
                row.PRIX_TTC = row.PRIX_UNITAIRE = row.PRIX_VENTE = prix;
                row.MT_HT = row.MT_TTC = row.PRIX_TTC * row.QUANTITE;

                row.Index = viewModel.ItemRows.Count();

            }
            viewModel.Item.TOTAL_TTC = viewModel.ItemRows.Sum(x => x.MT_TTC);
            viewModel.Item.TOTAL_HT = viewModel.Item.TOTAL_TTC;

        }

        private void Row_PropertyChanged(object sender, System.ComponentModel.PropertyChangedEventArgs e)
        {
            if (e.PropertyName.Equals("QUANTITE"))
            {
                if (sender is View_STK_ENTREE_DETAIL entreeDetail)
                {
                    if (entreeDetail != null)
                    {
                        entreeDetail.MT_HT = entreeDetail.MT_TTC = entreeDetail.PRIX_TTC * entreeDetail.QUANTITE;
                        viewModel.Item.TOTAL_TTC = viewModel.ItemRows.Sum(x => x.MT_TTC);
                    }
                }
            }
        }

        public static decimal GetPrix(View_STK_STOCK stock, decimal qte)
        {
            try
            {
                decimal prix = UpdateDatabase.getPrixByQuantitySync(stock.CODE_PRODUIT, qte);
                if (prix == 0)
                {
                    return stock.SelectedPrice;
                }
                else
                {
                    return prix;
                }
            }
            catch
            {
                return stock.SelectedPrice;
            }
        }

        private void Vte_PropertyChanged(object sender, System.ComponentModel.PropertyChangedEventArgs e)
        {

        }

        protected override async void OnAppearing()
        {
            base.OnAppearing();

            {
                parames = await AppManager.GetSysParams();
                permissions = await AppManager.GetPermissions();
            }

            // viewModel.ImmatriculationList = await GetImmatriculations("");

            if (!AppManager.HasAdmin)
            {
                ApplyVisibility();
            }
        }

        private void ApplyVisibility()
        {

        }

        #region Méthodes

        async Task ExecuteLoadRowsCommand()
        {
            if (string.IsNullOrEmpty(this.viewModel.Item?.CODE_ENTREE)) return;

            if (IsBusy)
                return;

            IsBusy = true;

            try
            {
                //viewModel.ItemRows.Clear();
                //var itemsC = await WebServiceClient.GetVenteLotDetails(this.viewModel.Item.CODE_ENTREE);

                //foreach (var itemC in itemsC)
                //{
                // //   itemC.Parent_Doc = viewModel.Item;
                //    viewModel.ItemRows.Add(itemC);
                //}

                //viewModel.UpdateMontants();
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

        #endregion

        #region Selectors


        private RetourProducts retourSelector;
        private async void RetourProduct_Clicked(object sender, EventArgs e)
        {
            /*
            if (string.IsNullOrEmpty(viewModel?.Item?.CODE_ENTREE))
            {
                await UserDialogs.Instance.AlertAsync("Vous devez valider l'en-têtes avant de pouvoir ajouter des produits !", AppResources.alrt_msg_Alert, AppResources.alrt_msg_Ok);

                return;
            }
            */
            retourSelector.CodeTiers = viewModel?.Item?.CODE_TIERS;
            //itemSelector.AutoriserReception = "1";
            await PopupNavigation.Instance.PushAsync(retourSelector);
        }





        #endregion 

        #region Events


        private async void Btn_Delete_Clicked(object sender, EventArgs e)
        {
            View_STK_ENTREE_DETAIL entreeDetail = (sender as Button).BindingContext as View_STK_ENTREE_DETAIL;

            if (entreeDetail != null)
            {
                if (await UserDialogs.Instance.ConfirmAsync(AppResources.txt_ConfimDelProductCmd, AppResources.msg_Confirmation, AppResources.alrt_msg_Ok, AppResources.alrt_msg_Cancel))
                {
                    int index = viewModel.ItemRows.IndexOf(entreeDetail);
                    viewModel.ItemRows.Remove(entreeDetail);
                    if (viewModel.Item?.Details?.Count - 1 >= index)
                    {
                        viewModel.Item.Details.RemoveAt(index);
                    }
                }
            }
        }

        private TiersSelector TiersSelector;
        private async void btn_Select_Clicked(object sender, EventArgs e)
        {
            TiersSelector.SearchedType = "CF";
            await PopupNavigation.Instance.PushAsync(TiersSelector);
        }

        private RSValidationPage RSValidationPage;
        private async void cmd_Buy_Clicked(object sender, EventArgs e)
        {
            if (viewModel.ItemRows.Count > 0)
            {
                RSValidationPage = new RSValidationPage(viewModel.CurrentStream, viewModel.Item, SelectedTiers);
                RSValidationPage.ParentviewModel = viewModel;
                await PopupNavigation.Instance.PushAsync(RSValidationPage);
            }
            else
            {
                await UserDialogs.Instance.AlertAsync("Veuillez entrer des produits avant de valider", AppResources.alrt_msg_Alert, AppResources.alrt_msg_Ok);
            }
        }

        #endregion

        #region Swip listview

        Image leftImage;
        Image rightImage;
        int itemIndex = -1;

        private void OpenVte()
        {
            if (itemIndex >= 0)
            {
                var item = viewModel.ItemRows[itemIndex];
                // item.IsFavorite = !item.IsFavorite;
            }
            this.listView.ResetSwipe();
        }

        private void Delete()
        {
            if (itemIndex >= 0)
            {
                var obj = viewModel.ItemRows[itemIndex];
                viewModel.ItemRows.RemoveAt(itemIndex);
                viewModel.Item.Details.Remove(obj);
            }
            this.listView.ResetSwipe();
            UpdateMontants();
        }

        private void ListView_SwipeStarted(object sender, Syncfusion.ListView.XForms.SwipeStartedEventArgs e)
        {
            itemIndex = -1;
        }
        private void ListView_SwipeEnded(object sender, Syncfusion.ListView.XForms.SwipeEndedEventArgs e)
        {
            itemIndex = e.ItemIndex;
        }

        private void edit_Vte(object sender, EventArgs e)
        {
            if (leftImage == null)
            {
                leftImage = sender as Image;
                (leftImage.Parent as View).GestureRecognizers.Add(new TapGestureRecognizer() { Command = new Command(OpenVte) });
                // leftImage.Source = ImageSource.FromResource("Swiping.Images.Favorites.png");
            }
        }

        private void delete_Vte(object sender, EventArgs e)
        {
            if (rightImage == null)
            {
                rightImage = sender as Image;
                (rightImage.Parent as View).GestureRecognizers.Add(new TapGestureRecognizer() { Command = new Command(Delete) });
                // rightImage.Source = ImageSource.FromResource("Swiping.Images.Delete.png");
            }
        }





        #endregion Swip listview





        public void UpdateMontants()
        {
            try
            {
                viewModel.Item.TOTAL_TTC = viewModel.ItemRows.Sum(e => e.MT_TTC);
                
            }
            catch (Exception ex)
            {
                UserDialogs.Instance.Alert(WSApi2.GetExceptionMessage(ex), AppResources.alrt_msg_Alert,
                     AppResources.alrt_msg_Ok);
            }
        }

        public void RemoveNewRow(View_STK_PRODUITS product)
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


        private async void btn_SelectTiers_Clicked(object sender, EventArgs e)
        {
            TiersSelector = new TiersSelector(viewModel.CurrentStream);
            await PopupNavigation.Instance.PushAsync(TiersSelector);
        }
    }
}