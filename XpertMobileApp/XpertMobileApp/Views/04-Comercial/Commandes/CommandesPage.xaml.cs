using Rg.Plugins.Popup.Services;
using System;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;
using XpertMobileApp.DAL;
using XpertMobileApp.ViewModels;

namespace XpertMobileApp.Views
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class CommandesPage : ContentPage
    {
        CommandesViewModel viewModel;
        public string CurrentStream = Guid.NewGuid().ToString();
        public CommandesPage()
        {
            InitializeComponent();

            itemSelector = new TiersSelector(CurrentStream);

            BindingContext = viewModel = new CommandesViewModel();


            MessagingCenter.Subscribe<TiersSelector, View_TRS_TIERS>(this, CurrentStream, async (obj, selectedItem) =>
            {
                viewModel.SelectedTiers = selectedItem;
                ent_SelectedTiers.Text = selectedItem.NOM_TIERS1;
            });

        }

        async void OnItemSelected(object sender, SelectedItemChangedEventArgs args)
        {
            var item = args.SelectedItem as View_VTE_VENTE;
            if (item == null)
                return;

            await Navigation.PushAsync(new CommandeDetailPage(item));

            // Manually deselect item.
            ItemsListView.SelectedItem = null;
        }

        async void AddItem_Clicked(object sender, EventArgs e)
        {
            await Navigation.PushModalAsync(new NavigationPage(new NewCommandePage(null)));
        }

        protected override void OnAppearing()
        {
            base.OnAppearing();


            if (viewModel.Items.Count == 0)
                LoadStats();
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
            viewModel.SelectedCompte = null;
            FilterPanel.IsVisible = false;
            viewModel.LoadItemsCommand.Execute(null);
        }

        private void ComptePicker_SelectedIndexChanged(object sender, EventArgs e)
        {

        }

        private TiersSelector itemSelector;
        private async void btn_Select_Clicked(object sender, EventArgs e)
        {
            itemSelector.pargentPage = this;
            await PopupNavigation.Instance.PushAsync(itemSelector);
        }
    }
}