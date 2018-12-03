using System;
using System.Linq;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;
using Xpert.Pharm.DAL;
using XpertMobileApp.ViewModels;

namespace XpertMobileApp.Views
{
	[XamlCompilation(XamlCompilationOptions.Compile)]
	public partial class EncaissementsPage : ContentPage
	{
        EncaissementsViewModel viewModel;

        public EncaissementsPage ()
		{
			InitializeComponent ();
            BindingContext = viewModel = new EncaissementsViewModel();
        }

        async void OnItemSelected(object sender, SelectedItemChangedEventArgs args)
        {
            var item = args.SelectedItem as TRS_ENCAISS;
            if (item == null)
                return;

      //      await Navigation.PushAsync(new ItemDetailPage(new ItemDetailViewModel(item)));

            // Manually deselect item.
            ItemsListView.SelectedItem = null;
        }

        async void AddItem_Clicked(object sender, EventArgs e)
        {
            await Navigation.PushModalAsync(new NavigationPage(new NewItemPage()));
        }

        protected override void OnAppearing()
        {
            base.OnAppearing();

            if (viewModel.Items.Count == 0)
              viewModel.LoadItemsCommand.Execute(null);
        }
    }
}