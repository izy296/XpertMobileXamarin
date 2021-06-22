using System;
using Rg.Plugins.Popup.Pages;
using Xamarin.Forms;
using Rg.Plugins.Popup.Services;
using XpertMobileApp.ViewModels;
using XpertMobileApp.Helpers;
using XpertMobileApp.Models;
using System.Linq;
using XpertMobileApp.DAL;
using XpertMobileApp.Api.Services;

namespace XpertMobileApp.Views
{
    public partial class LotSelector : PopupPage
    {

        LotSelectorViewModel viewModel;
        
        ProduitDetailPage prodViwer;

        private QteUpdater QteUpdater;

        public string CurrentStream { get; set; }

        public string CodeTiers
        {
            get
            {
                return viewModel.CodeTiers;
            }
            set
            {
                viewModel.CodeTiers = value;
            }
        }

        public string AutoriserReception
        {
            get
            {
                return viewModel.AutoriserReception;
            }
            set
            {
                viewModel.AutoriserReception = value;
            }
        }

        public LotSelector(string stream)
        {
            InitializeComponent();
            CurrentStream = stream;
            BindingContext = viewModel = new LotSelectorViewModel();
        }

        protected override void OnAppearing()
        {
            base.OnAppearing();

           // if (viewModel.Items.Count == 0)
            //viewModel.LoadItemsCommand.Execute(null);

            viewModel.LoadMoreItemsCommand.Execute(listView);

            foreach (var item in viewModel.Items)
            {
                item.SelectedQUANTITE = 0;
            }
        }

        private async void OnClose(object sender, EventArgs e)
        {
            await PopupNavigation.Instance.PopAsync();
        }

        private async void btn_Search_Clicked(object sender, EventArgs e)
        {
            viewModel.LoadItemsCommand.Execute(null);
        }

        private async void btnSelect_Clicked(object sender, EventArgs e)
        {
            await PopupNavigation.Instance.PopAsync();
            if (viewModel.SelectedItem != null)
            {
                MessagingCenter.Send(this, CurrentStream, viewModel.SelectedItem);
            }
        }

        private void DelQte_Clicked(object sender, EventArgs e)
        {
            var lot = ((sender as Button).BindingContext as View_STK_STOCK);
            lot.SelectedQUANTITE = 0;
            MessagingCenter.Send(this, "REMOVE" + CurrentStream, viewModel.SelectedItem);
            UpdateTotaux();
        }

        private async void DelUpdate_Clicked(object sender, EventArgs e)
        {
            var lot = ((sender as Button).BindingContext as View_STK_STOCK);
            QteUpdater = new QteUpdater(lot);
            QteUpdater.LotInfosUpdated += OnLotInfosUpdated;
            await PopupNavigation.Instance.PushAsync(QteUpdater);
        }

        private void OnLotInfosUpdated(object sender, LotInfosEventArgs e)
        {
            var item = sender as View_STK_STOCK;
            item.SelectedPrice = e.Price;
            item.SelectedQUANTITE = e.Quantity;

            UpdateTotaux();
            MessagingCenter.Send(this, CurrentStream, item);
        }

        private void UpdateTotaux() 
        {
            viewModel.TotalSelected = viewModel.Items.Where(e=>e.SelectedQUANTITE > 0).Sum(e => e.SelectedQUANTITE * e.SelectedPrice);
        }

        private void ItemsListView_ItemTapped(object sender, Syncfusion.ListView.XForms.ItemTappedEventArgs e)
        {
            View_STK_STOCK selected = e.ItemData as View_STK_STOCK;
            if(selected != null) 
            {
                selected.SelectedQUANTITE += 1;
                MessagingCenter.Send(this, CurrentStream, selected);
                UpdateTotaux();
            }
        }

        // Long click sur un element

        private async void listView_ItemHolding(object sender, Syncfusion.ListView.XForms.ItemHoldingEventArgs e)
        {
            View_STK_STOCK selected = e.ItemData as View_STK_STOCK;
            STK_PRODUITS prod = XpertHelper.CloneObject<STK_PRODUITS>(selected);

            if (prod == null)
                return;

            prodViwer = new ProduitDetailPage(prod);

            await Navigation.PushModalAsync(prodViwer);

            await PopupNavigation.Instance.PopAsync();
        }

        void listView_SelectionChanged(System.Object sender, Syncfusion.ListView.XForms.ItemSelectionChangedEventArgs e)
        {
        }
    }
}
