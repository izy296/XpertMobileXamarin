using Plugin.Connectivity;
using Rg.Plugins.Popup.Services;
using System;
using System.Collections.Generic;
using System.Linq;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;
using XpertMobileApp.Api;
using XpertMobileApp.DAL;
using XpertMobileApp.Helpers;
using XpertMobileApp.Models;
using XpertMobileApp.ViewModels;
using XpertMobileApp.Views._04_Comercial.Manquants;
using XpertMobileApp.Views.Templates;

namespace XpertMobileApp.Views
{
	[XamlCompilation(XamlCompilationOptions.Compile)]
	public partial class ManquantsPage : ContentPage
	{
        ManquantsViewModel viewModel;      
        public string CurrentStream = Guid.NewGuid().ToString();       
        public View_SYS_USER SelectedTiers { get; set; }
        private UserSelector itemSelector;       
        public ManquantsPage()
		{
			InitializeComponent ();
            itemSelector = new UserSelector(CurrentStream);
            BindingContext = viewModel = new ManquantsViewModel();
            MessagingCenter.Subscribe<UserSelector, View_SYS_USER>(this, CurrentStream, async (obj, selectedItem) =>
            {
               viewModel.SelectedTiers = selectedItem;
               ent_SelectedTiers.Text = selectedItem.ID_USER;
            });           
        }
        async void OnItemSelected(object sender, SelectedItemChangedEventArgs args)
        {
            var item = args.SelectedItem as View_ACH_MANQUANTS;
            if (item == null)
                return;
            await Navigation.PushAsync(new ProduitDetailPage(item.CODE_PRODUIT));
            // Manually deselect item.
            ItemsListView.SelectedItem = null;
        }       
        protected override async void OnAppearing()
        {
            base.OnAppearing();
            if (viewModel.Items.Count == 0)
                viewModel.LoadItemsCommand.Execute(null);
            if (viewModel.Types.Count == 0)
                viewModel.LoadExtrasDataCommand.Execute(null);
        }
        private void Filter_Clicked(object sender, EventArgs e)
        {
            FilterPanel.IsVisible = !FilterPanel.IsVisible;
        }
        private void btn_ApplyFilter_Clicked(object sender, EventArgs e)
        {        
            viewModel.LoadItemsCommand.Execute(null);
        }
        async void AjoutItem_Clicked(object sender, EventArgs e)
        {
            NewManquantPopupPage form = new NewManquantPopupPage();
            await PopupNavigation.Instance.PushAsync(form);
        }
        private void btn_CancelFilter_Clicked(object sender, EventArgs e)
        {
            FilterPanel.IsVisible = false;
            viewModel.LoadItemsCommand.Execute(null);            
        }        
        private async void btn_Select_Clicked(object sender, EventArgs e)
        {
            itemSelector.SearchedType = "";
            await PopupNavigation.Instance.PushAsync(itemSelector);
        }
    }
}