using System;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;
using XpertMobileApp.DAL;
using XpertMobileApp.ViewModels;

namespace XpertMobileApp.Views
{
	[XamlCompilation(XamlCompilationOptions.Compile)]
	public partial class ProduitsPage : ContentPage
	{
        ProduitsViewModel viewModel;

        public ProduitsPage()
		{
			InitializeComponent();

            BindingContext = viewModel = new ProduitsViewModel();
        }

        async void OnItemSelected(object sender, SelectedItemChangedEventArgs args)
        {
            if (App.Online)
            {
                var item = args.SelectedItem as STK_PRODUITS;

                if (item == null)
                    return;

                await Navigation.PushAsync(new ProduitDetailPage(item));

                // Manually deselect item.
                ItemsListView.SelectedItem = null;
            }
        }

        async void AddItem_Clicked(object sender, EventArgs e)
        {
           // await Navigation.PushModalAsync(new NavigationPage(new NewEncaissementPage(null, viewModel.EncaissDisplayType)));
        }
         
        protected override void OnAppearing()
        {
            base.OnAppearing();

            if (viewModel.Items.Count == 0)
                LoadStats();

            if (viewModel.Familles.Count == 0)
                viewModel.LoadExtrasDataCommand.Execute(null);
        }

        private void TypeFilter_Clicked(object sender, EventArgs e)
        {
            LoadStats();
        }

        private void LoadStats()
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
            viewModel.ClearFilters();
            viewModel.LoadItemsCommand.Execute(null);
        }

        private void ComptePicker_SelectedIndexChanged(object sender, EventArgs e)
        {

        }
    }
}