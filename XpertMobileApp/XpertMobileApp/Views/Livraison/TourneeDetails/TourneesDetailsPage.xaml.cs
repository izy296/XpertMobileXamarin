using System;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;
using XpertMobileApp.DAL;
using XpertMobileApp.ViewModels;

namespace XpertMobileApp.Views
{
	[XamlCompilation(XamlCompilationOptions.Compile)]
	public partial class TourneesDetailsPage : ContentPage
	{
        TourneesDetailsViewModel viewModel;

        public TourneesDetailsPage(string codeTournee)
		{
			InitializeComponent();

            BindingContext = viewModel = new TourneesDetailsViewModel(codeTournee);
        }

        async void OnItemSelected(object sender, SelectedItemChangedEventArgs args)
        {
            var item = args.SelectedItem as View_LIV_TOURNEE_DETAIL;
            if (item == null)
                return;

            //await Navigation.PushAsync(new TourneesDetailsPage(item.CODE_TOURNEE));

            // Manually deselect item.
            listView.SelectedItem = null;
        }

        async void AddItem_Clicked(object sender, EventArgs e)
        {
           // await Navigation.PushModalAsync(new NavigationPage(new NewEncaissementPage(null, viewModel.EncaissDisplayType)));
        }
         
        protected override void OnAppearing()
        {
            base.OnAppearing();

            if (viewModel.Items.Count == 0)
                LoadData();

         //   if (viewModel.Familles.Count == 0)
         //       viewModel.LoadExtrasDataCommand.Execute(null);
        }

        private void TypeFilter_Clicked(object sender, EventArgs e)
        {
            LoadData();
        }

        private void LoadData()
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

        private void OnFavoriteSwipeItemInvoked(object sender, EventArgs e)
        {
            var item = viewModel.SelectedItem;
            var tr = (sender as SwipeItem).Parent.Parent.Parent.BindingContext as View_LIV_TOURNEE_DETAIL;
        }

        private void OnDeleteSwipeItemInvoked(object sender, EventArgs e)
        {
            var item = viewModel.SelectedItem;
            var tr = (sender as SwipeItem).Parent.Parent.Parent.BindingContext as View_LIV_TOURNEE_DETAIL;
        }
    }
}