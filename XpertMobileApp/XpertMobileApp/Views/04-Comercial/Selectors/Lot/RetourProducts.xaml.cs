using Acr.UserDialogs;
using Rg.Plugins.Popup.Pages;
using Rg.Plugins.Popup.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;
using XpertMobileApp.DAL;
using XpertMobileApp.Helpers;
using XpertMobileApp.Models;
using XpertMobileApp.SQLite_Managment;

namespace XpertMobileApp.Views
{
    //    [XamlCompilation(XamlCompilationOptions.Compile)]
    //    public partial class RetourProducts : ContentPage
    //    {
    //        public RetourProducts()
    //        {
    //            InitializeComponent();
    //        }
    //    }
    //}
    public partial class RetourProducts : PopupPage
    {

        public RetourProductViewModel viewModel;
        List<View_STK_STOCK> SelectedlistLot;
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

        public RetourProducts(string stream)
        {
            InitializeComponent();
            CurrentStream = stream;
            BindingContext = viewModel = new RetourProductViewModel();
        }

        protected override void OnAppearing()
        {
            base.OnAppearing();

            // if (viewModel.Items.Count == 0)
            viewModel.LoadItemsCommand.Execute(null);

            foreach (var item in viewModel.Items)
            {
                item.SelectedQUANTITE = 0;
            }

            //// recevoire les id stock des elements selectioné pour changer color de fond d'element
            //MessagingCenter.Subscribe<VenteFormLivraisonPage, List<int?>>(this, "SelectedList", async (obj, selectedItem) =>
            //{
            //    Device.BeginInvokeOnMainThread(() =>
            //    {
            //        foreach (var item in selectedItem)
            //        {
            //            foreach (var selectedListItem in viewModel.Items)
            //            {
            //                if (selectedListItem.CODE_PRODUIT == item)
            //                {
            //                    selectedListItem.Selected = true;
            //                }
            //            }
            //        }
            //    });
            //});
            //UpdateTotaux();
        }

        private async void OnClose(object sender, EventArgs e)
        {
            await PopupNavigation.Instance.PopAsync();
        }

        private void OnItemSelected(object sender, SelectedItemChangedEventArgs e)
        {
            // btnSelect.IsEnabled = true;
            // btnRemove.IsEnabled = viewModel.SelectedItem != null && viewModel.SelectedItem.SelectedQUANTITE > 0;

            // if (viewModel.SelectedItem != null)
            //  {
            //      viewModel.SelectedItem.SelectedQUANTITE += 1;
            //  }
        }

        private async void btn_Search_Clicked(object sender, EventArgs e)
        {
            if (App.Online)
            {
                viewModel.LoadItemsCommand.Execute(null);
            }
            else
            {
                var res = await SQLite_Manager.FilterProduits(viewModel.SearchedText);
                viewModel.Items.Clear();
                viewModel.Items.AddRange(res);
            }
        }

        private async void btnSelect_Clicked(object sender, EventArgs e)
        {
            await PopupNavigation.Instance.PopAsync();
            if (viewModel.SelectedItem != null)
            {
                MessagingCenter.Send(this, CurrentStream, SelectedlistLot);
                SelectedlistLot = null;
            }
        }

        private void btnRemove_Clicked(object sender, EventArgs e)
        {
            if (viewModel.SelectedItem != null)
            {
                // viewModel.SelectedItem.SelectedQUANTITE = 0; // -=
                // btnRemove.IsEnabled = viewModel.SelectedItem.SelectedQUANTITE > 0;

                // MessagingCenter.Send(this, CurrentStream, viewModel.SelectedItem);
                // App.MsgCenter.SendAction(this, CurrentStream, "REMOVE", MCDico.REMOVE_ITEM, viewModel.SelectedItem);
            }
        }

        private async void ItemsListView_ItemTapped(object sender, ItemTappedEventArgs e)
        {
            if (viewModel.SelectedItem != null)
            {
                if (SelectedlistLot == null)
                {
                    SelectedlistLot = new List<View_STK_STOCK>();
                }
                var res = await SQLite_Manager.getProductfromStock(viewModel.SelectedItem);
                QteUpdater = new QteUpdater(res);
                QteUpdater.LotInfosUpdated += OnLotInfosUpdated;
                await PopupNavigation.Instance.PushAsync(QteUpdater);
                ItemsListView.SelectedItem = null;
                //if (SelectedlistLot.Contains(res))
                //if (checkIfExist(res))
                //{
                //    try
                //    {
                //        //var element = SelectedlistLot.Where(x => x.ID_STOCK == res.ID_STOCK).FirstOrDefault();
                //        //element.SelectedQUANTITE += 1;
                //        viewModel.SelectedItem.SelectedQUANTITE += 1;
                //        SelectedlistLot.Where(x => x.ID_STOCK == res.ID_STOCK).FirstOrDefault().SelectedQUANTITE += 1;
                //    }
                //    catch
                //    {
                //    }
                //}
                //else
                //{
                //    viewModel.SelectedItem.SelectedQUANTITE += 1;
                //    res.SelectedQUANTITE += 1;
                //    SelectedlistLot.Add(res);
                //}
                //MessagingCenter.Send(this, CurrentStream, viewModel.SelectedItem);
                UpdateTotaux();
            }
        }

        private bool checkIfExist(View_STK_STOCK stockProduct)
        {
            var res = SelectedlistLot.Where(e => e.ID_STOCK == stockProduct.ID_STOCK).FirstOrDefault();
            return res != null;
        }

        private async void DelQte_Clicked(object sender, EventArgs e)
        {
            var lot = ((sender as Button).BindingContext as View_STK_PRODUITS);
            var res = await SQLite_Manager.getProductfromStock(lot);
            SelectedlistLot.Remove(SelectedlistLot.Where(x => x.ID_STOCK == res.ID_STOCK).FirstOrDefault());
            lot.SelectedQUANTITE = 0;
            App.MsgCenter.SendAction(this, CurrentStream, "REMOVE", MCDico.REMOVE_ITEM, viewModel.SelectedItem);
            UpdateTotaux();
        }

        private QteUpdater QteUpdater;
        private async void DelUpdate_Clicked(object sender, EventArgs e)
        {
            var lot = ((sender as Button).BindingContext as View_STK_PRODUITS);
            var res = await SQLite_Manager.getProductfromStock(lot);
            res.SelectedQUANTITE = lot.SelectedQUANTITE;
            res.OLD_QUANTITE = lot.QTE_STOCK;
            QteUpdater = new QteUpdater(res);
            QteUpdater.LotInfosUpdated += OnLotInfosUpdated;
            await PopupNavigation.Instance.PushAsync(QteUpdater);
        }

        private async void OnLotInfosUpdated(object sender, LotInfosEventArgs e)
        {
            var item = sender as View_STK_STOCK;
            item.SelectedPrice = e.Price;
            item.SelectedQUANTITE = e.Quantity;

            var res = await SQLite_Manager.getProductfromStock(viewModel.SelectedItem);

            if (checkIfExist(res))
            {
                viewModel.SelectedItem.SelectedQUANTITE = e.Quantity;
                SelectedlistLot.Where(x => x.ID_STOCK == res.ID_STOCK).FirstOrDefault().SelectedQUANTITE = e.Quantity;
            }


            UpdateTotaux();
            MessagingCenter.Send(this, CurrentStream, item);
        }

        private void UpdateTotaux()
        {
            viewModel.TotalSelected = viewModel.Items.Where(e => e.SelectedQUANTITE > 0).Sum(e => e.SelectedQUANTITE * e.PRIX_VENTE_HT);
        }
    }
}
