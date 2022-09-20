using Rg.Plugins.Popup.Services;
using System;
using System.Collections;
using System.Collections.ObjectModel;
using System.Timers;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;
using XpertMobileApp.Base;
using XpertMobileApp.Models;
using XpertMobileApp.ViewModels;

namespace XpertMobileApp.Views
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class RotationDesProduitsPage : XBasePage
    {
        RotationDesProduitsViewModel viewModel;

        public RotationDesProduitsPage()
        {
            InitializeComponent();

            BindingContext = viewModel = new RotationDesProduitsViewModel();
        }

        private void FilterScroll_Focused(object sender, FocusEventArgs e)
        {
            ItemsListView.Opacity = 0;
        }

        async void OnItemSelected(object sender, SelectedItemChangedEventArgs args)
        {
            if (App.Online)
            {
                var item = args.SelectedItem as RotationDesProduits;

                if (item == null)
                    return;

                await Navigation.PushAsync(new RotationProduitDetailPage(item, viewModel.BeginDate, viewModel.EndDate));

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
            Label label = new Label();
            if (viewModel.Items.Count == 0)
                LoadStats();

        }

        private void TypeFilter_Clicked(object sender, EventArgs e)
        {
            LoadStats();
        }

        private void LoadStats()
        {
            viewModel.LoadItemsCommand.Execute(null);
        }

        private void btn_ApplyFilter_Clicked(object sender, EventArgs e)
        {
            viewModel.LoadItemsCommand.Execute(null);
        }

        private async void ShowHideFilter(object sender, EventArgs e)
        {
            RotationPopupFilter filter = new RotationPopupFilter(viewModel);
            //Load data for the first time ...
            await PopupNavigation.Instance.PushAsync(filter);
        }

        public override void SearchCommand()
        {
            while (IsBusy)
            {
                System.Threading.Thread.Sleep(1000);
            }
            viewModel.LoadItemsCommand.Execute(null);
        }

        public override void HandleSearchBarTextChanged(object sender, string searchBarText)
        {
            base.HandleSearchBarTextChanged(sender,searchBarText);
            if (searchBarText != "")
            {
                viewModel.SearchedText = searchBarText;
            }
            else
            {
                viewModel.SearchedText = "";
                SearchCommand();
            }
        }
    }
}