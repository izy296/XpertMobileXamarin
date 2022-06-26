using Rg.Plugins.Popup.Services;
using System;
using System.Linq;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;
using XpertMobileApp.DAL;
using XpertMobileApp.Helpers;
using XpertMobileApp.ViewModels;
using XpertMobileApp.Views.Encaissement;

namespace XpertMobileApp.Views
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class BordereauFactsPage : ContentPage
    {
        BordereauFactsViewModel viewModel;
        public string CurrentStream = Guid.NewGuid().ToString();
        private bool opened = false;
        public BordereauFactsPage(View_CFA_BORDEREAU item)
        {
            InitializeComponent();

            itemSelector = new TiersSelector(CurrentStream);

            BindingContext = viewModel = new BordereauFactsViewModel(item);

            viewModel.LoadSummaries = false;
            filterLayout.TranslateTo(-270, 0);

            MessagingCenter.Subscribe<TiersSelector, View_TRS_TIERS>(this, CurrentStream, async (obj, selectedItem) =>
            {
                viewModel.SelectedTiers = selectedItem;
                //ent_SelectedTiers.Text = selectedItem.NOM_TIERS1;
            });
        }

        async void OnItemSelected(object sender, SelectedItemChangedEventArgs args)
        {
            var item = args.SelectedItem as View_VTE_VENTE;
            if (item == null)
                return;

            await Navigation.PushAsync(new VenteDetailPage(item));

            // Manually deselect item.
            ItemsListView.SelectedItem = null;
        }

        async void AddItem_Clicked(object sender, EventArgs e)
        {
            await Navigation.PushModalAsync(new NavigationPage(new NewEncaissementPage(null, viewModel.EncaissDisplayType)));
        }

        protected override void OnAppearing()
        {
            base.OnAppearing();

            if (viewModel.Items.Count == 0)
                LoadStats();

            viewModel.LoadExtrasDataCommand.Execute(null);
        }

        protected override void OnDisappearing()
        {
            base.OnDisappearing();
            filterLayout.TranslateTo(-270, 0);
        }

        private async void LoadStats()
        {
            viewModel.LoadItemsCommand.Execute(null);
        }

        private async void Filter_Clicked(object sender, EventArgs e)
        {
            //FilterPanel.IsVisible = !FilterPanel.IsVisible;
            filterLayout.TranslateTo(-270, 0);
        }

        private async void btn_ApplyFilter_Clicked(object sender, EventArgs e)
        {
            viewModel.LoadItemsCommand.Execute(null);
            opened = !opened;
            filterLayout.TranslateTo(-270, 0);
        }

        private void btn_CancelFilter_Clicked(object sender, EventArgs e)
        {
            viewModel.SelectedSTATUS = null;
            viewModel.SelectedTiers = null;
            //FilterPanel.IsVisible = false;
            filterLayout.TranslateTo(-270, 0);
            viewModel.LoadItemsCommand.Execute(null);
        }

        private void ComptePicker_SelectedIndexChanged(object sender, EventArgs e)
        {

        }
        private void showHideFilter(object sender, EventArgs e)
        {
            if (opened)
            {
                filterLayout.TranslateTo(-270, 0);
                opened = !opened;
            }
            else
            {
                filterLayout.TranslateTo(0, 0);
                opened = !opened;
            }
        }

        private TiersSelector itemSelector;
        private async void btn_Select_Clicked(object sender, EventArgs e)
        {
            await PopupNavigation.Instance.PushAsync(itemSelector);
        }
    }
}