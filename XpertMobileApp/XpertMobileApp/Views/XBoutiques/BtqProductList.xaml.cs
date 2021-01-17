using System.Collections.Generic;
using Xamarin.Forms.Xaml;

using Xamarin.Forms.Internals;
using XpertMobileApp.Views;
using XpertMobileApp.Api.Managers;
using XpertMobileApp.DAL;
using XpertMobileApp;
using Syncfusion.SfDataGrid.XForms.DataPager;
using System;
using XpertMobileApp.Helpers;
using Xamarin.Forms;
using XpertMobileApp.Models;


namespace XpertMobileApp.Views
{
    [Preserve(AllMembers = true)]

    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class BtqProductList : BaseView
    {
        BtqProductViewModel viewModel;

        public BtqProductList()
        {
            BindingContext = viewModel = new BtqProductViewModel();
            InitializeComponent();

        }

        protected async override void OnAppearing()
        {
            base.OnAppearing();

            var i = await viewModel.GetCount();
            viewModel.PageCount = i / viewModel.PagesSize;


            if (viewModel.Items.Count == 0)
            { 
              //  viewModel.LoadItemsCommand.Execute(null);                
            }

           // if (viewModel.Familles.Count == 0)
           //     viewModel.LoadExtrasDataCommand.Execute(null);

            MessagingCenter.Subscribe<MsgCenter, int>(this, MCDico.DATA_COUNT_LOADED, async (obj, count) =>
            {
                // dataPager.PageCount = ;
              //  viewModel.PageCount = count;
            });
        }


        private async void dataPager_OnDemandLoading(object sender, OnDemandLoadingEventArgs e)
        {
            // TODO : Permettre l'affectation du numéro de page dans le nouveau curdviewmodel (présent dans BasePagerViewModel)
            // viewModel.PageIndex = e.StartIndex;
           // viewModel.LoadItemsCommand.Execute(null);
            await viewModel.LoadItems(e.StartIndex, e.PageSize);
        }

        private async void listView_SelectionChanged(object sender, Syncfusion.ListView.XForms.ItemSelectionChangedEventArgs e)
        {
            var item = e.AddedItems[0] as View_PRODUITS;
            if (item == null)
                return;

           // await Navigation.PushAsync(new BtqProductDetailPage(item));
            // await Navigation.PushAsync(new ProduitCDetailPage(item));

            // Manually deselect item.
            // ItemsListView.SelectedItem = null;
        }

        private void Filter_Clicked(object sender, EventArgs e)
        {
            FilterPanel.IsVisible = !FilterPanel.IsVisible;
        }

        private async void btn_ApplyFilter_Clicked(object sender, EventArgs e)
        {
            // TODO : Permettre l'affectation du numéro de page dans le nouveau curdviewmodel
            //  dataPager.PageIndex = 0;
            //  viewModel.PageIndex = dataPager.PageIndex;
            //  viewModel.LoadItemsCommand.Execute(null);
            await viewModel.LoadItems(1, 8);
        }

        private void btn_CancelFilter_Clicked(object sender, EventArgs e)
        {
            FilterPanel.IsVisible = false;         
        }
    }
}