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
	public partial class ProductionsPage : ContentPage
	{
        private string typeDoc = "LF";
        public string MotifDoc { get; set; }

        ProductionsViewModel viewModel;

        SYS_MOBILE_PARAMETRE parames;
        List<SYS_OBJET_PERMISSION> permissions;

        public ProductionsPage(string motifDoc)
		{
			InitializeComponent ();

            MotifDoc = motifDoc;

            itemSelector = new TiersSelector();

            BindingContext = viewModel = new ProductionsViewModel(typeDoc, motifDoc);

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
        }

        async void OnItemSelected(object sender, SelectedItemChangedEventArgs args)
        {
            var item = args.SelectedItem as View_PRD_AGRICULTURE;
            if (item == null)
                return;

            await Navigation.PushAsync(new ProductionFormPage(item, typeDoc, MotifDoc));

            // Manually deselect item.
            ItemsListView.SelectedItem = null;                        
        }

        async void AddItem_Clicked(object sender, EventArgs e)
        {
            await Navigation.PushAsync(new ProductionFormPage(null, typeDoc, MotifDoc));
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