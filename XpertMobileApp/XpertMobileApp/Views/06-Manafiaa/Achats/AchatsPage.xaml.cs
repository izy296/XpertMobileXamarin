using Plugin.Connectivity;
using Rg.Plugins.Popup.Services;
using System;
using System.Collections.Generic;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;
using XpertMobileApp.Api;
using XpertMobileApp.DAL;
using XpertMobileApp.Models;
using XpertMobileApp.ViewModels;

namespace XpertMobileApp.Views
{
	[XamlCompilation(XamlCompilationOptions.Compile)]
	public partial class AchatsPage : ContentPage
	{
        public string CurrentStream = Guid.NewGuid().ToString();

        private string typeDoc = "LF";
        
        private string motifDoc = AchRecMotifs.PesageReception;
        
        AchatsViewModel viewModel;

        SYS_MOBILE_PARAMETRE parames;

        List<SYS_OBJET_PERMISSION> permissions;

        public AchatsPage()
		{
			InitializeComponent ();

            itemSelector = new TiersSelector(CurrentStream);

            BindingContext = viewModel = new AchatsViewModel(typeDoc);

            if (StatusPicker.ItemsSource != null && StatusPicker.ItemsSource.Count > 0)
            {
                StatusPicker.SelectedItem = StatusPicker.ItemsSource[1];
            }

            MessagingCenter.Subscribe<TiersSelector, View_TRS_TIERS>(this, CurrentStream, async (obj, selectedItem) =>
            {
                viewModel.SelectedTiers = selectedItem;
                ent_SelectedTiers.Text = selectedItem.NOM_TIERS1;
            });
        }

        protected override async void OnAppearing()
        {
            base.OnAppearing();

            var connectivity = CrossConnectivity.Current;
            if (connectivity.IsConnected)
            { 
                parames = await AppManager.GetSysParams();
                permissions = await AppManager.GetPermissions();

                LoadData();
            }

            if (!AppManager.HasAdmin)
            {
                ApplyVisibility();
            }
        }

        #region Interface actions

        async void OnItemSelected(object sender, SelectedItemChangedEventArgs args)
        {
            var item = args.SelectedItem as View_ACH_DOCUMENT;
            if (item == null)
                return;

            await Navigation.PushAsync(new AchatFormPage(item, typeDoc, motifDoc));

            // Manually deselect item.
            ItemsListView.SelectedItem = null;
        }

        async void AddItem_Clicked(object sender, EventArgs e)
        {
            await Navigation.PushAsync(new AchatFormPage(null, typeDoc, motifDoc));
        }

        private void ApplyVisibility()
        {
            btn_Additem.IsEnabled = viewModel.hasEditHeader;
        }

        private async void LoadData()
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

        #endregion
    }
}