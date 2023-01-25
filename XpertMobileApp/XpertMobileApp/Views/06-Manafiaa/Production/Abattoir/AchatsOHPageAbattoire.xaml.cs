using Acr.UserDialogs;
using Rg.Plugins.Popup.Services;
using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Linq;
using System.Threading.Tasks;
using Xamarin.Forms;
using Xamarin.Forms.Extended;
using Xamarin.Forms.Xaml;
using Xpert.Common.WSClient.Helpers;
using XpertMobileApp.Api;
using XpertMobileApp.Api.Managers;
using XpertMobileApp.DAL;
using XpertMobileApp.Models;
using XpertMobileApp.Services;
using XpertMobileApp.ViewModels;
using ZXing.Net.Mobile.Forms;

namespace XpertMobileApp.Views
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class AchatsOHPageAbattoire : ContentPage
    {
        private string typeDoc = "LF";
        public string MotifDoc { get; set; }
        public string CurrentStream = Guid.NewGuid().ToString();
        AchatsOHViewModelAbattoire viewModel;

        SYS_MOBILE_PARAMETRE parames;
        List<SYS_OBJET_PERMISSION> permissions;
        string veterinaryNote;

        public AchatsOHPageAbattoire(string motifDoc)
        {
            InitializeComponent();

            MotifDoc = motifDoc;

            itemSelector = new TiersSelector(CurrentStream);

            if (Constants.AppName == Apps.XCOM_Abattoir)
            {
                headerGrid.IsVisible = false;
            }

            BindingContext = viewModel = new AchatsOHViewModelAbattoire(typeDoc, motifDoc);

            if (StatusPicker.ItemsSource != null && StatusPicker.ItemsSource.Count > 0)
            {
                StatusPicker.SelectedItem = StatusPicker.ItemsSource[0];
            }

            MessagingCenter.Subscribe<TiersSelector, View_TRS_TIERS>(this, CurrentStream, async (obj, selectedItem) =>
            {
                viewModel.SelectedTiers = selectedItem;
                ent_SelectedTiers.Text = selectedItem.NOM_TIERS1;
            });

            MessagingCenter.Subscribe<VeterinaryPopup, string>(this, "VeterinaryPopup", async (obj, selectedItem) =>
            {
                veterinaryNote = selectedItem;
            });

            viewModel.SelectedDocs.CollectionChanged += SlectedItempsChanged;

            viewModel.Items.OnAfterLoadMore = () =>
            {
                UserDialogs.Instance.ShowLoading();
                var itemsList = viewModel.Items;
                var EnAttentEncours = itemsList.Where(e => e.STATUS_DOC == DocStatus.EnAttente || e.STATUS_DOC == DocStatus.EnCours || e.STATUS_DOC == DocStatus.EnAttente)
                                        //.OrderBy(e => e.STATUS_DOC)
                                        .OrderByDescending(e => e.DATE_DOC)
                                        .GroupBy(e => e.STATUS_DOC)
                                        .ToList();
                var OtehrItems = itemsList.Where(e => e.STATUS_DOC != DocStatus.EnAttente && e.STATUS_DOC != DocStatus.EnCours && e.STATUS_DOC != DocStatus.EnAttente)
                                        //.OrderBy(e => e.STATUS_DOC)
                                        .OrderByDescending(e => e.DATE_DOC)
                                        .GroupBy(e => e.STATUS_DOC)
                                        .ToList();
                List<View_ACH_DOCUMENT> resList = new List<View_ACH_DOCUMENT>();
                foreach (var groupedList in EnAttentEncours)
                {
                    resList.AddRange(groupedList);
                }
                foreach (var groupedList in OtehrItems)
                {
                    resList.AddRange(groupedList);
                }
                //resList.Reverse();
                viewModel.Items.Clear();
                viewModel.Items.AddRange(new InfiniteScrollCollection<View_ACH_DOCUMENT>(resList));
                OnPropertyChanged("Items");
                UserDialogs.Instance.HideLoading();
            };
        }

        #region Selections 

        private void selectionCancelImage_Tapped(object sender, EventArgs e)
        {
            for (int i = 0; i < viewModel.SelectedDocs.Count; i++)
            {
                var item = viewModel.SelectedDocs[i] as View_ACH_DOCUMENT;
                item.IsSelected = false;
            }
            this.viewModel.SelectedDocs.Clear();

            UpdateSelectionTempate();
        }

        private void selectionEditImage_Tapped(object sender, EventArgs e)
        {
            UpdateSelectionTempate();
        }

        public void UpdateSelectionTempate()
        {
            if (viewModel.SelectedDocs.Count > 0 || ItemsListView.SelectionMode == ListViewSelectionMode.Single)
            {
                ItemsListView.SelectionMode = ListViewSelectionMode.None;
                editImageParent.IsVisible = false;
                cancelImageParent.IsVisible = true;
                GenerateOrdreProd.IsVisible = true;
            }
            else
            {
                ItemsListView.SelectionMode = ListViewSelectionMode.Single;
                editImageParent.IsVisible = true;
                cancelImageParent.IsVisible = false;
                GenerateOrdreProd.IsVisible = false;
            }

            /*
            if (ListView.SelectedItems.Count > 0 || selectionEditImageParent.IsVisible)
            {
                ListView.SelectionMode = SelectionMode.Multiple;
                ListView.SelectionBackgroundColor = Color.Transparent;
                ListView.SelectedItems.Clear();
                SelectionViewModel.HeaderInfo = ListView.SelectedItems.Count + " selected";
                SelectionViewModel.TitleInfo = "";
                SelectionViewModel.IsVisible = true;
                selectionEditImageParent.IsVisible = false;
            }
            else
            {
                ListView.SelectionMode = SelectionMode.Single;
                ListView.SelectionBackgroundColor = Color.FromRgb(228, 228, 228);
                SelectionViewModel.HeaderInfo = "";
                SelectionViewModel.TitleInfo = "Music Library";
                SelectionViewModel.IsVisible = false;
                selectionEditImageParent.IsVisible = true;
            }
            */
        }
        #endregion Selections

        private void SlectedItempsChanged(object sender, NotifyCollectionChangedEventArgs e)
        {
            decimal totalQte = viewModel.SelectedDocs.Sum(x => x.QTE_NET);
            int totalCount = viewModel.SelectedDocs.Count();

            txt_PoidsTotal.Text = "Quantité : " + totalQte.ToString() + " Kg";
            txt_Count.Text = "Selection : " + "(" + totalCount.ToString() + ")";
        }

        async void OnItemSelected(object sender, SelectedItemChangedEventArgs args)
        {
            var item = args.SelectedItem as View_ACH_DOCUMENT;
            if (item == null)
                return;

            if (item.STATUS_DOC == DocStatus.EnAttente && !viewModel.hasInsertHeader)
            {
                var bll = CrudManager.Achats;
                VeterinaryPopup popup = new VeterinaryPopup("Voulez vous accepter cet article ?", "Rejeté", "Valider");
                CustomPopup confirmationPopup = new CustomPopup("êtes-vous sûrs ?", "Annuller", "Ok");
                await PopupNavigation.Instance.PushAsync(popup);
                if (await popup.PopupClosedTask)
                {
                    if (popup.Result)
                    {
                        Device.BeginInvokeOnMainThread(async () =>
                        {
                            try
                            {
                                await PopupNavigation.Instance.PushAsync(confirmationPopup);
                                if (await confirmationPopup.PopupClosedTask)
                                {
                                    if (confirmationPopup.Result)
                                    {
                                        item.STATUS_DOC = DocStatus.EnCours;
                                        item.NOTE_DOC = veterinaryNote;
                                        UserDialogs.Instance.ShowLoading();
                                        await bll.UpdateItemAsync(item);
                                        await viewModel.ExecuteLoadItemsCommand();
                                        UserDialogs.Instance.HideLoading();
                                    }
                                }
                            }
                            catch (Exception ex)
                            {
                                UserDialogs.Instance.HideLoading();
                            }
                        });
                    }
                    else if (!popup.Result)
                    {
                        Device.BeginInvokeOnMainThread(async () =>
                        {
                            try
                            {
                                await PopupNavigation.Instance.PushAsync(confirmationPopup);
                                if (await confirmationPopup.PopupClosedTask)
                                {
                                    if (confirmationPopup.Result)
                                    {
                                        item.STATUS_DOC = DocStatus.Rejeter;
                                        item.NOTE_DOC = veterinaryNote;
                                        UserDialogs.Instance.ShowLoading();
                                        await bll.UpdateItemAsync(item);
                                        await viewModel.ExecuteLoadItemsCommand();
                                        UserDialogs.Instance.HideLoading();
                                    }
                                }
                            }
                            catch (Exception ex)
                            {
                                UserDialogs.Instance.HideLoading();
                            }

                        });
                    }
                }
            }
            else
            {
                if (!viewModel.hasInsertHeader && item.STATUS_DOC != DocStatus.Terminer)
                    await Navigation.PushAsync(new AchatFormPageAbattoire(item, typeDoc, MotifDoc));
            }
            // Manually deselect item.
            ItemsListView.SelectedItem = null;
        }

        private void ItemsListView_ItemTapped(object sender, ItemTappedEventArgs e)
        {
            var selectedItem = e.Item as View_ACH_DOCUMENT;
            if (selectedItem == null)
                return;
            if (ItemsListView.SelectionMode == ListViewSelectionMode.None)
            {
                var item = viewModel.SelectedDocs.Where(x => x.CODE_DOC == selectedItem.CODE_DOC).SingleOrDefault();
                selectedItem.IsSelected = item == null;
                if (item != null)
                {
                    viewModel.SelectedDocs.Remove(item);
                }
                else
                {
                    viewModel.SelectedDocs.Add(selectedItem);
                }
            }
        }

        async void AddItem_Clicked(object sender, EventArgs e)
        {
            await Navigation.PushAsync(new AchatFormPageAbattoire(null, typeDoc, MotifDoc));
        }

        protected override async void OnAppearing()
        {
            base.OnAppearing();

            parames = await AppManager.GetSysParams();
            permissions = await AppManager.GetPermissions();

            UserDialogs.Instance.ShowLoading();
            if (!AppManager.HasAdmin)
            {
                ApplyVisibility();
            }

            //if (viewModel.Items.Count == 0)
            LoadStats();
            UserDialogs.Instance.HideLoading();
        }

        private void ApplyVisibility()
        {
            btn_Additem.IsEnabled = viewModel.hasInsertHeader;
        }

        private async void LoadStats()
        {
            viewModel.LoadItemsCommand.Execute(null);
        }

        private void Filter_Clicked(object sender, EventArgs e)
        {
            FilterPanel.IsVisible = !FilterPanel.IsVisible;
        }

        private void btn_ApplyFilter_Clicked(object sender, EventArgs e)
        {
            viewModel.LoadItemsCommand.Execute(null);
        }

        private void btn_CancelFilter_Clicked(object sender, EventArgs e)
        {
            FilterPanel.IsVisible = false;
            ent_SelectedIdentifiant.Text = "";
            ent_SelectedTiers= null;
            StatusPicker.SelectedItem = null;
            viewModel.LoadItemsCommand.Execute(null);
        }

        private void ComptePicker_SelectedIndexChanged(object sender, EventArgs e)
        {

        }

        private TiersSelector itemSelector;
        private async void btn_Select_Clicked(object sender, EventArgs e)
        {
            itemSelector.SearchedType = "CF";
            await PopupNavigation.Instance.PushAsync(itemSelector);
        }

        private async void btn_Scan_QRCode(object sender, EventArgs e)
        {
            try
            {
                var scanner = new ZXing.Mobile.MobileBarcodeScanner();
                var result = await scanner.Scan();
                if (result != null)
                {
                    ent_SelectedIdentifiant.Text = result.Text;
                    OnPropertyChanged("SelectedIdentifiant");
                }
            }
            catch (Exception ex)
            {
                await UserDialogs.Instance.AlertAsync(ex.Message, "Alerte", "Ok");
            }
        }

        private async void btn_Production_Clicked(object sender, EventArgs e)
        {
            string result = "";

            if (IsBusy)
                return;

            IsBusy = true;

            /*
            int indivdualCount = viewModel.SelectedDocs.Where(x => x.IS_INDIVIDUAL == true).Count();
            if(viewModel.SelectedDocs.Count > indivdualCount && indivdualCount > 0)
            {
                await UserDialogs.Instance.AlertAsync("Vous avez choisit des receptions non individuelles avec d'autres qui sont individuelles!", AppResources.alrt_msg_Alert, AppResources.alrt_msg_Ok);
                return;
            }
            */

            int Net0Count = viewModel.SelectedDocs.Where(x => x.QTE_NET <= 0).Count();
            if (Net0Count > 0)
            {
                await UserDialogs.Instance.AlertAsync("Vous avez choisit des receptions avec des quantiés null!", AppResources.alrt_msg_Alert, AppResources.alrt_msg_Ok);
                return;
            }

            try
            {
                UserDialogs.Instance.ShowLoading(AppResources.txt_Loading);
                string[] docs = viewModel.SelectedDocs.Select(x => x.CODE_DOC).ToArray();
                result = await WebServiceClient.GenerateProductionOrder(docs);
                if (!string.IsNullOrEmpty(result))
                {
                    await UserDialogs.Instance.AlertAsync("L'ordre de production a été correctement généré!", AppResources.alrt_msg_Alert, AppResources.alrt_msg_Ok);
                    viewModel.SelectedDocs.Clear();
                    viewModel.LoadItemsCommand.Execute(null);
                }
            }
            catch (Exception ex)
            {
                await UserDialogs.Instance.AlertAsync(WSApi2.GetExceptionMessage(ex), AppResources.alrt_msg_Alert,
                    AppResources.alrt_msg_Ok);
            }
            finally
            {
                UserDialogs.Instance.HideLoading();
                IsBusy = false;
            }
        }


    }
}