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
    public partial class BordereauxPage : ContentPage
    {
        BordereauxViewModel viewModel;
        public string CurrentStream = Guid.NewGuid().ToString();
        private TiersSelector itemSelector;
        private bool opened = false;
        public BordereauxPage()
        {
            InitializeComponent();

            itemSelector = new TiersSelector(CurrentStream);

            BindingContext = viewModel = new BordereauxViewModel();

            MessagingCenter.Subscribe<TiersSelector, View_TRS_TIERS>(this, CurrentStream, async (obj, selectedItem) =>
            {
                // viewModel.SelectedTiers = selectedItem;
                // ent_SelectedTiers.Text = selectedItem.NOM_TIERS1;
            });

            filterLayout.TranslateTo(-270, 0);
        }

        async void OnItemSelected(object sender, SelectedItemChangedEventArgs args)
        {
            var item = args.SelectedItem as View_CFA_BORDEREAU;
            if (item == null)
                return;

            await Navigation.PushAsync(new BordereauFactsPage(item));

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

            if (viewModel.Centres.Count == 0)
                viewModel.LoadExtrasDataCommand.Execute(null);
        }
        protected override void OnDisappearing()
        {
            base.OnDisappearing();
            filterLayout.TranslateTo(-270, 0);
        }

        private void Filter_Clicked(object sender, EventArgs e)
        {
            //FilterPanel.IsVisible = !FilterPanel.IsVisible;
        }

        private void btn_ApplyFilter_Clicked(object sender, EventArgs e)
        {
            viewModel.LoadItemsCommand.Execute(null);
            opened = !opened;
            filterLayout.TranslateTo(-270, 0);
        }

        private void btn_CancelFilter_Clicked(object sender, EventArgs e)
        {
            filterLayout.TranslateTo(-270, 0);
            opened = !opened;
            viewModel.ClearFilters();
            viewModel.LoadItemsCommand.Execute(null);
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
        private async void btn_Select_Clicked(object sender, EventArgs e)
        {
            await PopupNavigation.Instance.PushAsync(itemSelector);
        }
    }
}