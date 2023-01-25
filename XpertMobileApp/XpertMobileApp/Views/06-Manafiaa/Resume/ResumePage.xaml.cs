using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;
using XpertMobileApp.DAL;
using XpertMobileApp.ViewModels;
using XpertMobileApp.Views;
using XpertMobileApp;
using XpertMobileApp.Models;

namespace XpertMobileAppManafiaa.Views._06_Manafiaa.Resume
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class ResumePage : ContentPage
    {
        ResumeViewModel viewModel;

        public ResumePage()
        {
            InitializeComponent();

            BindingContext = viewModel = new ResumeViewModel();
        }

        async void OnItemSelected(object sender, SelectedItemChangedEventArgs args)
        {
            //if (App.Online)
            //{
                var item = args.SelectedItem as View_STK_STATISTICS;

                if (item == null)
                    return;

                //await Navigation.PushAsync(new ProduitDetailPage(item));

                // Manually deselect item.
                ItemsListView.SelectedItem = null;
            //}
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
    }
}