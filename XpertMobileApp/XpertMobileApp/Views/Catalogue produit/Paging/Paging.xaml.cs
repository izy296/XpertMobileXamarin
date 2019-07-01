using System.Collections.Generic;
using Xamarin.Forms.Xaml;

using Xamarin.Forms.Internals;
using XpertMobileApp.Views;
using XpertMobileApp.Api.Managers;
using XpertMobileApp.DAL;
using XpertMobileApp;
using Syncfusion.SfDataGrid.XForms.DataPager;
using System;

namespace SampleBrowser.SfListView
{
    [Preserve(AllMembers = true)]

    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class Paging : BaseView
    {
        PagingViewModel viewModel;

        public Paging()
        {
            InitializeComponent();

            BindingContext = viewModel = new PagingViewModel();
        }

        protected override void OnAppearing()
        {
            base.OnAppearing();

            if (viewModel.Items.Count == 0)
                viewModel.LoadItemsCommand.Execute(null);

            if (viewModel.Familles.Count == 0)
                viewModel.LoadExtrasDataCommand.Execute(null);
        }


        private void dataPager_OnDemandLoading(object sender, OnDemandLoadingEventArgs e)
        {
            viewModel.PageIndex = e.StartIndex;
            viewModel.LoadItemsCommand.Execute(null);
        }

        private async void listView_SelectionChanged(object sender, Syncfusion.ListView.XForms.ItemSelectionChangedEventArgs e)
        {
            var item = e.AddedItems[0] as View_STK_PRODUITS;
            if (item == null)
                return;

            await Navigation.PushAsync(new ProduitCDetailPage(item));

            // Manually deselect item.
            // ItemsListView.SelectedItem = null;
        }

        private void Filter_Clicked(object sender, EventArgs e)
        {
            FilterPanel.IsVisible = !FilterPanel.IsVisible;
        }

        private void btn_ApplyFilter_Clicked(object sender, EventArgs e)
        {
            dataPager.PageIndex = 0;
            viewModel.PageIndex = dataPager.PageIndex;
            viewModel.LoadItemsCommand.Execute(null);
        }

        private void btn_CancelFilter_Clicked(object sender, EventArgs e)
        {
            FilterPanel.IsVisible = false;         
        }
    }
}