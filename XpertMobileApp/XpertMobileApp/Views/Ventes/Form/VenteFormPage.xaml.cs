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
using XpertMobileApp.Api.Managers;
using XpertMobileApp.Api.Services;
using XpertMobileApp.DAL;
using XpertMobileApp.Helpers;
using XpertMobileApp.Models;
using XpertMobileApp.Services;
using XpertMobileApp.Views.Achats;
using ZXing.Net.Mobile.Forms;

namespace XpertMobileApp.Views
{
	[XamlCompilation(XamlCompilationOptions.Compile)]
	public partial class VenteFormPage : ContentPage
	{
        private VenteFormViewModel viewModel;


        SYS_MOBILE_PARAMETRE parames;

        List<SYS_OBJET_PERMISSION> permissions;

        public Command AddItemCommand { get; set; }



        public VenteFormPage(View_VTE_VENTE vente, string typeDoc)
        {


            InitializeComponent();

            var vte = vente == null ? new View_VTE_VENTE() : vente;
            if(vente == null)
            {
                vte.ID = XpertHelper.RandomString(7);
                vte.TYPE_DOC = typeDoc;
                vte.TYPE_VENTE = typeDoc;
                vte.DATE_VENTE = DateTime.Now.Date;

                vte.PropertyChanged += Vte_PropertyChanged;
            }

            BindingContext = this.viewModel = new VenteFormViewModel(vte, vte?.CODE_VENTE);
            viewModel.TypeDoc = typeDoc;

            itemSelector = new LotSelector(viewModel.CurrentStream);
            TiersSelector = new TiersSelector(viewModel.CurrentStream);

            // jobFieldAutoComplete.BindingContext = viewModel;

            this.viewModel.Title = string.IsNullOrEmpty(vte.CODE_VENTE) ? AppResources.pn_NewSales : vte?.ToString();

            this.viewModel.LoadRowsCommand = new Command(async () => await ExecuteLoadRowsCommand());

           // viewModel.ItemRows.CollectionChanged += ItemsRowsChanged;

            MessagingCenter.Subscribe<LotSelector, View_STK_STOCK>(this, viewModel.CurrentStream, async (obj, selectedItem) =>
            {
                Device.BeginInvokeOnMainThread(() =>
                {
                    viewModel.AddNewRow(selectedItem);
                });
            });
            
            MessagingCenter.Subscribe<LotSelector, View_STK_PRODUITS>(this, "REMOVE" + viewModel.CurrentStream, async (obj, selectedItem) =>
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
        private void Vte_PropertyChanged(object sender, System.ComponentModel.PropertyChangedEventArgs e)
        {

        }

        protected override async void OnAppearing()
        {
            base.OnAppearing();

            parames = await App.GetSysParams();
            permissions = await App.GetPermissions();

            // viewModel.ImmatriculationList = await GetImmatriculations("");

            if (!App.HasAdmin)
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
                var itemsC = await WebServiceClient.GetVenteLotDetails(this.viewModel.Item.CODE_VENTE);

                foreach (var itemC in itemsC)
                {
                    itemC.Parent_Doc = viewModel.Item;
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

        private LotSelector itemSelector;
        private async void RowSelect_Clicked(object sender, EventArgs e)
        {
            /*
            if (string.IsNullOrEmpty(viewModel?.Item?.CODE_VENTE))
            {
                await UserDialogs.Instance.AlertAsync("Vous devez valider l'en-têtes avant de pouvoir ajouter des produits !", AppResources.alrt_msg_Alert, AppResources.alrt_msg_Ok);

                return;
            }
            */
            itemSelector.CodeTiers = viewModel?.Item?.CODE_TIERS;
            //itemSelector.AutoriserReception = "1";
            await PopupNavigation.Instance.PushAsync(itemSelector);
        }



        public static View_ACH_DOCUMENT_DETAIL currentRow;



        private void btn_SelectImmat_Clicked(object sender, EventArgs e)
        {

        }
        #endregion 

        #region Events

        private void RowScan_Clicked(object sender, EventArgs e)
        {
            var scaner = new ZXingScannerPage();
            Navigation.PushAsync(scaner);
            scaner.OnScanResult += (result) =>
            {
                scaner.IsScanning = false;
                Device.BeginInvokeOnMainThread(async () =>
                {
                    await Navigation.PopAsync();

                    var vteLot = await viewModel.AddScanedProduct(result.Text);
                    /*
                    ClassId = string.Format("pb_{0}", vteLot.ID);
                    var pbruteElem2 = ItemsListView.Children.Where(x => x.ClassId == ClassId).FirstOrDefault() as SfNumericTextBox;
                    pbruteElem2.Focus();
                    */
                });
            };
        }

        private void HeaderSettings_Clicked(object sender, EventArgs e)
        {
           // pnl_Header.IsVisible = !pnl_Header.IsVisible;
        }

        private async void Btn_Delete_Clicked(object sender, EventArgs e)
        {
            View_VTE_VENTE_LOT vteD = (sender as Button).BindingContext as View_VTE_VENTE_LOT;

            if (vteD != null)
            {
                if (await UserDialogs.Instance.ConfirmAsync(AppResources.txt_ConfimDelProductCmd, AppResources.msg_Confirmation, AppResources.alrt_msg_Ok, AppResources.alrt_msg_Cancel))
                {
                    int index = viewModel.ItemRows.IndexOf(vteD);
                    viewModel.ItemRows.Remove(vteD);
                    if(viewModel.Item?.Details?.Count - 1 >= index)
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

        private VteValidationPage VteValidationPage;
        private async void cmd_Buy_Clicked(object sender, EventArgs e)
        {
            VteValidationPage = new VteValidationPage(viewModel.CurrentStream, viewModel.Item);
            VteValidationPage.ParentviewModel = viewModel;
            await PopupNavigation.Instance.PushAsync(VteValidationPage);
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
                viewModel.ItemRows.RemoveAt(itemIndex);
            this.listView.ResetSwipe();
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


    }
}