using Acr.UserDialogs;
using Plugin.SimpleAudioPlayer;
using Rg.Plugins.Popup.Services;
using Syncfusion.SfNumericTextBox.XForms;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
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
using XpertMobileApp.Views._04_Comercial.Selectors.Lot;
using XpertMobileApp.Views.Achats;
using ZXing.Net.Mobile.Forms;

namespace XpertMobileApp.Views
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class VenteFormLivraisonPage : ContentPage
    {
        private VenteFormLivraisonViewModel viewModel;

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

        public VenteFormLivraisonPage(View_VTE_VENTE vente, string typeDoc, View_TRS_TIERS tiers = null, string codeTourneeDetails = "")
        {
            InitializeComponent();

            bool disable = true;
            var vte = vente == null ? new View_VTE_VENTE() : vente;
            if (vente == null)
            {
                vte.ID_Random = XpertHelper.RandomString(7);
                vte.TYPE_DOC = typeDoc;
                vte.TYPE_VENTE = typeDoc;
                vte.DATE_VENTE = DateTime.Now.Date;
                vte.MBL_CODE_TOURNEE_DETAIL = codeTourneeDetails;
                vte.PropertyChanged += Vte_PropertyChanged;
            }

            BindingContext = this.viewModel = new VenteFormLivraisonViewModel(vte, vte?.CODE_VENTE);
            viewModel.TypeDoc = typeDoc;

            //viewModel.IsEnabled = disable;


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
            btn_Scan.IsVisible = disable;
            itemSelector = new LotSelectorLivraisonUniteFamille(viewModel.CurrentStream);
            retourSelector = new RetourProducts(viewModel.CurrentStream);
            TiersSelector = new TiersSelector(viewModel.CurrentStream);

            // jobFieldAutoComplete.BindingContext = viewModel;

            this.viewModel.Title = string.IsNullOrEmpty(vte.CODE_VENTE) ? AppResources.pn_NewSales : vte?.ToString();

            this.viewModel.LoadRowsCommand = new Command(async () => await ExecuteLoadRowsCommand());

            // viewModel.ItemRows.CollectionChanged += ItemsRowsChanged;

            MessagingCenter.Subscribe<LotSelectorLivraisonUniteFamille, List<View_STK_STOCK>>(this, viewModel.CurrentStream, async (obj, selectedItem) =>
            {
                Device.BeginInvokeOnMainThread(() =>
                {
                    viewModel.AddNewRows(selectedItem, false); // false veut dire le type de produit ajouter est une vente (pas retour)
                });
            });

            MessagingCenter.Subscribe<VenteFormLivraisonPage, View_STK_STOCK>(this, viewModel.CurrentStream, async (obj, selectedItem) =>
            {
                Device.BeginInvokeOnMainThread(() =>
                {
                    var list = new List<View_STK_STOCK> { selectedItem };
                    viewModel.AddNewRows(list, false); // false veut dire le type de produit ajouter est une vente (pas retour)
                });
            });

            MessagingCenter.Subscribe<RetourProducts, List<View_STK_STOCK>>(this, viewModel.CurrentStream, async (obj, selectedItem) =>
            {
                Device.BeginInvokeOnMainThread(() =>
                {
                    viewModel.AddNewRows(selectedItem, true); // true veut dire le type de produit ajouter est un retour
                });
            });

            MessagingCenter.Subscribe<LotSelectorLivraisonUniteFamille, View_STK_PRODUITS>(this, "REMOVE" + viewModel.CurrentStream, async (obj, selectedItem) =>
            {
                Device.BeginInvokeOnMainThread(() =>
                {
                    viewModel.RemoveNewRow(selectedItem);
                });
            });

            MessagingCenter.Subscribe<RetourProducts, View_STK_PRODUITS>(this, "REMOVE" + viewModel.CurrentStream, async (obj, selectedItem) =>
            {
                Device.BeginInvokeOnMainThread(() =>
                {
                    viewModel.RemoveNewRow(selectedItem);
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

            if (viewModel.Item != null && !string.IsNullOrEmpty(this.viewModel.Item.CODE_VENTE))
            {
                viewModel.LoadRowsCommand.Execute(null);
            }
        }

        protected override bool OnBackButtonPressed()
        {
            Device.BeginInvokeOnMainThread(async () =>
            {
                var result = await this.DisplayAlert(AppResources.msg_Confirmation, "Voulez vous fermer la vente ?", "Oui", "Non");
                if (result) await this.Navigation.PopAsync(); // or anything else
            });

            return true;

        }

        private void Vte_PropertyChanged(object sender, System.ComponentModel.PropertyChangedEventArgs e)
        {

        }

        protected override async void OnAppearing()
        {
            base.OnAppearing();

            if (Constants.AppName != Apps.X_BOUTIQUE)
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
            //btn_Immatriculation.IsEnabled = false;

            string userGroup = App.User.GroupName;

            // Par défaut le header est caché s'il n'a pas le droit d'éditer le header
            // pnl_Header.IsVisible = viewModel.hasEditHeader;

            // Commandes pour la selection de produit
            //btn_RowSelect.IsEnabled = viewModel.hasEditDetails;
            //btn_RowScan.IsEnabled = viewModel.hasEditDetails;

            // Champs d'edition du header
            //dp_EcheanceDate.IsEnabled = viewModel.hasEditHeader;
            //btn_TeirsSearch.IsEnabled = viewModel.hasEditHeader;

        }

        #region Méthodes

        async Task ExecuteLoadRowsCommand()
        {
            if (string.IsNullOrEmpty(this.viewModel.Item?.CODE_VENTE)) return;

            if (IsBusy)
                return;

            IsBusy = true;

            try
            {
                viewModel.ItemRows.Clear();
                var itemsC = await WebServiceClient.GetVenteLotLivraisonDetails(this.viewModel.Item.CODE_VENTE);

                foreach (var itemC in itemsC)
                {
                    //   itemC.Parent_Doc = viewModel.Item;
                    viewModel.ItemRows.Add(itemC);
                }

                viewModel.UpdateMontants();
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

        private LotSelectorLivraisonUniteFamille itemSelector;
        private async void RowSelect_Clicked(object sender, EventArgs e)
        {
            /*
            if (string.IsNullOrEmpty(viewModel?.Item?.CODE_VENTE))
            {
                await UserDialogs.Instance.AlertAsync("Vous devez valider l'en-têtes avant de pouvoir ajouter des produits !", AppResources.alrt_msg_Alert, AppResources.alrt_msg_Ok);

                return;
            }
            */
            itemSelector.viewModel.Tier = viewModel.SelectedTiers;
            itemSelector.CodeTiers = viewModel?.Item?.CODE_TIERS;
            //itemSelector.AutoriserReception = "1";
            await PopupNavigation.Instance.PushAsync(itemSelector);

            if (viewModel.ItemRows != null || viewModel.ItemRows.Count > 0)
            {
                MessagingCenter.Send(this, "SelectedList", viewModel.ItemRows.Select(elm => elm.ID_STOCK).ToList());
            }
        }

        private RetourProducts retourSelector;
        private async void RetourProduct_Clicked(object sender, EventArgs e)
        {
            /*
            if (string.IsNullOrEmpty(viewModel?.Item?.CODE_VENTE))
            {
                await UserDialogs.Instance.AlertAsync("Vous devez valider l'en-têtes avant de pouvoir ajouter des produits !", AppResources.alrt_msg_Alert, AppResources.alrt_msg_Ok);

                return;
            }
            */
            retourSelector.CodeTiers = viewModel?.Item?.CODE_TIERS;
            //itemSelector.AutoriserReception = "1";
            await PopupNavigation.Instance.PushAsync(retourSelector);
        }



        public static View_ACH_DOCUMENT_DETAIL currentRow;



        private void btn_SelectImmat_Clicked(object sender, EventArgs e)
        {

        }
        #endregion 

        #region Events

        private void RowScan_Clicked(object sender, EventArgs e)
        {
            GoogleVisionBS gvsScannedBarcode = new GoogleVisionBS();
            MainPage RootPage = Application.Current.MainPage as MainPage;
            var detail = RootPage.Detail;
            gvsScannedBarcode.UserSubmitted += async (_, scannedText) =>
            {
                await detail.Navigation.PopAsync();
                var vteLot = await viewModel.AddScanedProduct(scannedText);
            };

            detail.Navigation.PushAsync(gvsScannedBarcode);
        }

        private void HeaderSettings_Clicked(object sender, EventArgs e)
        {
            // pnl_Header.IsVisible = !pnl_Header.IsVisible;
        }

        private async void Btn_Delete_Clicked(object sender, EventArgs e)
        {
            View_VTE_VENTE_LIVRAISON vteD = (sender as Button).BindingContext as View_VTE_VENTE_LIVRAISON;

            if (vteD != null)
            {
                if (await UserDialogs.Instance.ConfirmAsync(AppResources.txt_ConfimDelProductCmd, AppResources.msg_Confirmation, AppResources.alrt_msg_Ok, AppResources.alrt_msg_Cancel))
                {
                    int index = viewModel.ItemRows.IndexOf(vteD);
                    viewModel.ItemRows.Remove(vteD);
                    if (viewModel.Item?.DetailsDistrib?.Count - 1 >= index)
                    {
                        viewModel.Item.DetailsDistrib.RemoveAt(index);
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

        private VteValidationPage VteValidationPage;
        private async void cmd_Buy_Clicked(object sender, EventArgs e)
        {
            if (viewModel.ItemRows.Count > 0)
            {
                VteValidationPage = new VteValidationPage(viewModel.CurrentStream, viewModel.Item, SelectedTiers);
                VteValidationPage.ParentLivraisonviewModel = viewModel;
                await PopupNavigation.Instance.PushAsync(VteValidationPage);
            }
            else
            {
                await UserDialogs.Instance.AlertAsync("Veuillez entrer des produits avant de valider", AppResources.alrt_msg_Alert, AppResources.alrt_msg_Ok);
            }
        }

        private void initVteInterface()
        {
            // throw new NotImplementedException();
        }


        #endregion

        private async Task<List<string>> GetImmatriculations(string str)
        {
            List<string> result = null;

            //   if (string.IsNullOrEmpty(str)) return result;

            if (IsBusy)
                return result;

            IsBusy = true;

            try
            {
                result = await WebServiceClient.GetImmatriculations(str);
                return result;
            }
            catch (Exception ex)
            {
                await UserDialogs.Instance.AlertAsync(WSApi2.GetExceptionMessage(ex), AppResources.alrt_msg_Alert,
                    AppResources.alrt_msg_Ok);
                return result;
            }
            finally
            {
                IsBusy = false;
            }
        }

        private async void AchImpr_Clicked(object sender, EventArgs e)
        {
        }

        private AnnexTiersSelector AnnexTiersSelector;
        private async void Btn_SelectExtraUser_Clicked(object sender, EventArgs e)
        {

        }

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
                viewModel.Item.DetailsDistrib.Remove(obj);
            }
            this.listView.ResetSwipe();
            viewModel.UpdateMontants();
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

        private async void btn_SelectTiers_Clicked(object sender, EventArgs e)
        {
            TiersSelector = new TiersSelector(viewModel.CurrentStream);
            await PopupNavigation.Instance.PushAsync(TiersSelector);
        }

        private void btn_ScanTiers_Clicked(object sender, EventArgs e)
        {
            GoogleVisionBS gvsScannedBarcode = new GoogleVisionBS();
            MainPage RootPage = Application.Current.MainPage as MainPage;
            var detail = RootPage.Detail;
            gvsScannedBarcode.UserSubmitted += async (_, scannedText) =>
            {

                await detail.Navigation.PopAsync();
                await viewModel.SelectScanedTiers(scannedText);

            };

            detail.Navigation.PushAsync(gvsScannedBarcode);
        }

        private async void listView_ItemTapped(object sender, Syncfusion.ListView.XForms.ItemTappedEventArgs e)
        {
            var item = (View_VTE_VENTE_LIVRAISON)e.ItemData;
            var qteUpdater = new QteUpdater(item, viewModel.SelectedTiers.CODE_FAMILLE);
            qteUpdater.LotInfosUpdated += OnLotInfosUpdated;
            await PopupNavigation.Instance.PushAsync(qteUpdater);
        }

        private void OnLotInfosUpdated(object sender, LotInfosEventArgs e)
        {
            var item = sender as View_STK_STOCK;
            item.SelectedPrice = e.Price;
            item.SelectedQUANTITE = e.Quantity;
            MessagingCenter.Send(this, viewModel.CurrentStream, item);
        }

    }
}