using Acr.UserDialogs;
using Rg.Plugins.Popup.Services;
using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Linq;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;
using Xpert.Common.WSClient.Helpers;
using XpertMobileApp.DAL;
using XpertMobileApp.Helpers;
using XpertMobileApp.Models;
using XpertMobileApp.Services;
using XpertMobileApp.ViewModels;
using XpertMobileApp.Views.Encaissement;

namespace XpertMobileApp.Views
{
	[XamlCompilation(XamlCompilationOptions.Compile)]
	public partial class AchatsOHPage : ContentPage
	{
        private string typeDoc = "LF";
        public string MotifDoc { get; set; }

        AchatsOHViewModel viewModel;

        SYS_MOBILE_PARAMETRE parames;
        List<SYS_OBJET_PERMISSION> permissions;

        public AchatsOHPage(string motifDoc)
		{
			InitializeComponent ();

            MotifDoc = motifDoc;

            itemSelector = new TiersSelector();

            BindingContext = viewModel = new AchatsOHViewModel(typeDoc, motifDoc);

            if (StatusPicker.ItemsSource != null && StatusPicker.ItemsSource.Count > 0)
            {
                StatusPicker.SelectedItem = StatusPicker.ItemsSource[1];
            }

            MessagingCenter.Subscribe<TiersSelector, View_TRS_TIERS>(this, MCDico.ITEM_SELECTED, async (obj, selectedItem) =>
            {
                viewModel.SelectedTiers = selectedItem;
                ent_SelectedTiers.Text = selectedItem.NOM_TIERS1;
            });

            viewModel.SelectedDocs.CollectionChanged += SlectedItempsChanged;
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
            decimal totalQte = viewModel.SelectedDocs.Sum(x => x.PESEE_BRUTE);
            int totalCount = viewModel.SelectedDocs.Count();

            txt_PoidsTotal.Text = "Quantité : "  + totalQte.ToString() ;
            txt_Count.Text = "Selection : " + "(" + totalCount.ToString() + ")";
        }

        async void OnItemSelected(object sender, SelectedItemChangedEventArgs args)
        {
            var item = args.SelectedItem as View_ACH_DOCUMENT;
            if (item == null)
                return;

            await Navigation.PushAsync(new AchatFormPage(item, typeDoc, MotifDoc));

            // Manually deselect item.
            ItemsListView.SelectedItem = null;                        
        }

        private void ItemsListView_ItemTapped(object sender, ItemTappedEventArgs e)
        {
            var selectedItem = e.Item as View_ACH_DOCUMENT;
            if (selectedItem == null)
                return;

            var item = viewModel.SelectedDocs.Where(x => x.CODE_DOC == selectedItem.CODE_DOC).SingleOrDefault();
            selectedItem.IsSelected = item == null;
            if (item != null)
            {
                viewModel.SelectedDocs.Remove(selectedItem);
            }
            else
            {
                viewModel.SelectedDocs.Add(selectedItem);
            }
        }

        async void AddItem_Clicked(object sender, EventArgs e)
        {
            await Navigation.PushAsync(new AchatFormPage(null, typeDoc, MotifDoc));
        }

        protected override async void OnAppearing()
        {
            base.OnAppearing();

            parames = await App.GetSysParams();
            permissions = await App.GetPermissions();

            if (!App.HasAdmin)
            {
                ApplyVisibility();
            }

            //if (viewModel.Items.Count == 0)
            LoadStats();
        }

        private void ApplyVisibility()
        {
            btn_Additem.IsEnabled = viewModel.hasEditHeader;
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

        private string GenerateProductionOrder() 
        {
            return "idDoc";
        }

        private async void btn_Production_Clicked(object sender, EventArgs e)
        {
            string  result = "";

            if (IsBusy)
                return;

            IsBusy = true;

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