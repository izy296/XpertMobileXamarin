using Rg.Plugins.Popup.Services;
using System;
using System.Linq;

using Xamarin.Forms;
using Xamarin.Forms.Xaml; 
using XpertMobileApp.DAL;
using XpertMobileApp.Helpers;
using XpertMobileApp.ViewModels;

namespace XpertMobileApp.Views
{
	[XamlCompilation(XamlCompilationOptions.Compile)]
	public partial class ManquantsPage : ContentPage
	{
        ManquantsViewModel viewModel;

        public ManquantsPage()
		{
			InitializeComponent ();

            itemSelector = new CentresSelector();

            BindingContext = viewModel = new ManquantsViewModel();


            MessagingCenter.Subscribe<TiersSelector, View_TRS_TIERS>(this, MCDico.ITEM_SELECTED, async (obj, selectedItem) =>
            {
               // viewModel.SelectedTiers = selectedItem;
               // ent_SelectedTiers.Text = selectedItem.NOM_TIERS1;
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

        async void AddItem_Clicked(object sender, EventArgs e)
        {
           // await Navigation.PushModalAsync(new NavigationPage(new NewEncaissementPage(null, viewModel.EncaissDisplayType)));
        }

        protected override void OnAppearing()
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

        private void btn_CancelFilter_Clicked(object sender, EventArgs e)
        {
            FilterPanel.IsVisible = false;
            viewModel.LoadItemsCommand.Execute(null);            
        }

        private CentresSelector itemSelector;
        private async void btn_Select_Clicked(object sender, EventArgs e)
        {
            await PopupNavigation.Instance.PushAsync(itemSelector);
        }
    }
}