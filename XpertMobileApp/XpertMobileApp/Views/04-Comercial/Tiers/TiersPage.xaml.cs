using Rg.Plugins.Popup.Services;
using Syncfusion.XForms.Buttons;
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
    public partial class TiersPage : ContentPage
    {
        TiersViewModel viewModel;
        public string CurrentStream = Guid.NewGuid().ToString();
        public TiersPage()
        {
            InitializeComponent();

            itemSelector = new TiersSelector(CurrentStream);

            BindingContext = viewModel = new TiersViewModel();


            MessagingCenter.Subscribe<TiersSelector, View_TRS_TIERS>(this, CurrentStream, async (obj, selectedItem) =>
            {
                // viewModel.SelectedTiers = selectedItem;
                // ent_SelectedTiers.Text = selectedItem.NOM_TIERS1;
            });

        }

        async void OnItemSelected(object sender, SelectedItemChangedEventArgs args)
        {
            /*
            var item = args.SelectedItem as View_TRS_TIERS;
            if (item == null)
                return;

            await Navigation.PushAsync(new VenteDetailPage(item));

            // Manually deselect item.
            ItemsListView.SelectedItem = null;                        
           */
        }
        async void AddItem_Clicked(object sender, EventArgs e)
        {
            NewTiersPage form = new NewTiersPage(null);
            await Navigation.PushModalAsync(new NavigationPage(form));
        }

        protected override void OnAppearing()
        {
            base.OnAppearing();


            if (viewModel.Items.Count == 0)
                viewModel.LoadItemsCommand.Execute(null);

            if (viewModel.Familles.Count == 0)
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
            soldeG.IsChecked = false;
            soldeE.IsChecked = false;
            soldeL.IsChecked = false;
            viewModel.ClearFilters();
            viewModel.LoadItemsCommand.Execute(null);
        }

        private TiersSelector itemSelector;
        private async void btn_Select_Clicked(object sender, EventArgs e)
        {
            await PopupNavigation.Instance.PushAsync(itemSelector);
        }

        private void rg_solde_StateChanged(object sender, Syncfusion.XForms.Buttons.StateChangedEventArgs e)
        {
            if (e.IsChecked.HasValue && e.IsChecked.Value)
            {
                viewModel.SoldOperator = (sender as SfRadioButton).ClassId;
            }
        }
    }
}