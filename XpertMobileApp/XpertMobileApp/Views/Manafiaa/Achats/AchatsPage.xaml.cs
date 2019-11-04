using Rg.Plugins.Popup.Services;
using System;
using System.Collections.Generic;
using System.Linq;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;
using XpertMobileApp.DAL;
using XpertMobileApp.Helpers;
using XpertMobileApp.Models;
using XpertMobileApp.ViewModels;
using XpertMobileApp.Views.Encaissement;

namespace XpertMobileApp.Views
{
	[XamlCompilation(XamlCompilationOptions.Compile)]
	public partial class AchatsPage : ContentPage
	{
        private string typeDoc = "LF";
        private string motifDoc = AchRecMotifs.PesageReception;
        AchatsViewModel viewModel;

        SYS_MOBILE_PARAMETRE parames;
        List<SYS_OBJET_PERMISSION> permissions;

        public AchatsPage()
		{
			InitializeComponent ();

            itemSelector = new TiersSelector();

            BindingContext = viewModel = new AchatsViewModel(typeDoc);

            if (StatusPicker.ItemsSource != null && StatusPicker.ItemsSource.Count > 0)
            {
                StatusPicker.SelectedItem = StatusPicker.ItemsSource[1];
            }

            MessagingCenter.Subscribe<TiersSelector, View_TRS_TIERS>(this, MCDico.ITEM_SELECTED, async (obj, selectedItem) =>
            {
                viewModel.SelectedTiers = selectedItem;
                ent_SelectedTiers.Text = selectedItem.NOM_TIERS1;
            });
        }

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
    }
}