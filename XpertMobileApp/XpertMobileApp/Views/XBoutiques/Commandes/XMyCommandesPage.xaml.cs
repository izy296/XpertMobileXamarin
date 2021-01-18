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
	public partial class XMyCommandesPage : ContentPage
	{
        XMyCommandesViewModel viewModel;
        public string CurrentStream = Guid.NewGuid().ToString();
        public XMyCommandesPage()
		{
			InitializeComponent ();

            itemSelector = new TiersSelector(CurrentStream);

            BindingContext = viewModel = new XMyCommandesViewModel();
        }

        async void OnItemSelected(object sender, SelectedItemChangedEventArgs args)
        {
            var item = args.SelectedItem as COMMANDES;
            if (item == null)
                return;

           await Navigation.PushAsync(new BtqCommandeDetailPage(item));

            // Manually deselect item.
            ItemsListView.SelectedItem = null;                        
        }

        async void AddItem_Clicked(object sender, EventArgs e)
        {
          //  await Navigation.PushModalAsync(new NavigationPage(new NewCommandePage(null)));
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
            await PopupNavigation.Instance.PushAsync(itemSelector);
        }
    }
}