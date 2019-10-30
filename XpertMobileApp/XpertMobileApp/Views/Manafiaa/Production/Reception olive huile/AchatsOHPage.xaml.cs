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
        AchatsOHViewModel viewModel;

        SYS_MOBILE_PARAMETRE parames;
        List<SYS_OBJET_PERMISSION> permissions;

        public AchatsOHPage()
		{
			InitializeComponent ();

            itemSelector = new TiersSelector();

            BindingContext = viewModel = new AchatsOHViewModel(typeDoc);

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

        private void SlectedItempsChanged(object sender, NotifyCollectionChangedEventArgs e)
        {
            decimal totalQte = viewModel.SelectedDocs.Sum(x => x.PESEE_BRUTE);
            int totalCount = viewModel.SelectedDocs.Count();

            txt_PoidsTotal.Text = totalQte.ToString();
            txt_Count.Text = totalCount.ToString();
        }

        async void OnItemSelected(object sender, SelectedItemChangedEventArgs args)
        {
            var item = args.SelectedItem as View_ACH_DOCUMENT;
            if (item == null)
                return;

            await Navigation.PushAsync(new AchatFormPage(item, typeDoc));

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
            await Navigation.PushAsync(new AchatFormPage(null, typeDoc));
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