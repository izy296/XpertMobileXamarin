using System;
using System.Linq;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;
using Xpert.Pharm.DAL;
using XpertMobileApp.ViewModels;
using XpertMobileApp.Views.Encaissement;

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
            var item = args.SelectedItem as View_TRS_ENCAISS;
            if (item == null)
                return;

            await Navigation.PushAsync(new EncaissementDetailPage(new ItemDetailViewModel<View_TRS_ENCAISS>(item)));

            // Manually deselect item.
            ItemsListView.SelectedItem = null;
        }

        async void AddItem_Clicked(object sender, EventArgs e)
        {
            await Navigation.PushModalAsync(new NavigationPage(new NewEncaissementPage()));
        }

        protected override void OnAppearing()
        {
            base.OnAppearing();

            if (viewModel.Items.Count == 0)
                LoadStats(btn_All);
        }

        private void TypeFilter_Clicked(object sender, EventArgs e)
        {
            LoadStats((Button)sender);
        }

        private void LoadStats(Button btn)
        {
            EncaissDisplayType selectedType = viewModel.EncaissDisplayType;

            btn_All.BackgroundColor = Color.FromHex("#2196F3");
            btn_Decaiss.BackgroundColor = Color.FromHex("#2196F3");
            btn_Encaiss.BackgroundColor = Color.FromHex("#2196F3");

            btn.BackgroundColor = Color.FromHex("#51adf6");
            switch (btn.ClassId)
            {
                case "all":
                    selectedType = EncaissDisplayType.All;
                    break;
                case "Encaiss":
                    selectedType = EncaissDisplayType.Encaiss;
                    break;
                case "Decaiss":
                    selectedType = EncaissDisplayType.Decaiss;
                    break; 
            }

            if (viewModel.EncaissDisplayType != selectedType)
            {
                viewModel.EncaissDisplayType = selectedType;
                viewModel.LoadItemsCommand.Execute(null);
            }
        }
    }
}