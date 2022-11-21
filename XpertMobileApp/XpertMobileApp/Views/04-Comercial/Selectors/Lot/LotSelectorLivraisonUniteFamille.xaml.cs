using System;
using Rg.Plugins.Popup.Pages;
using Xamarin.Forms;
using Rg.Plugins.Popup.Services;
using XpertMobileApp.ViewModels;
using XpertMobileApp.Helpers;
using XpertMobileApp.Models;
using System.Linq;
using XpertMobileApp.DAL;
using System.Collections.Generic;
using Acr.UserDialogs;

namespace XpertMobileApp.Views
{
    public partial class LotSelectorLivraisonUniteFamille : PopupPage
    {

        public LotSelectorLivraisonViewModel viewModel;
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

        public LotSelectorLivraisonUniteFamille(string stream)
        {
            InitializeComponent();
            CurrentStream = stream;
            BindingContext = viewModel = new LotSelectorLivraisonViewModel();
            
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
            viewModel.LoadItemsCommand.Execute(null);
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
                if (viewModel.SelectedItem.OLD_QUANTITE > 0)
                {
                    if (viewModel.SelectedItem.SelectedQUANTITE < viewModel.SelectedItem.OLD_QUANTITE)
                    {
                        if (SelectedlistLot.Contains(viewModel.SelectedItem))
                        {
                            try
                            {
                                SelectedlistLot.Where(x => x.ID_STOCK == viewModel.SelectedItem.ID_STOCK).FirstOrDefault().SelectedQUANTITE += 1;
                            }
                            catch
                            {
                            }
                        }
                        else
                        {
                            viewModel.SelectedItem.SelectedQUANTITE += 1;
                            SelectedlistLot.Add(viewModel.SelectedItem);
                        }
                        //MessagingCenter.Send(this, CurrentStream, viewModel.SelectedItem);
                        UpdateTotaux();
                    }
                    else
                    {
                        await UserDialogs.Instance.AlertAsync(" Quantité stock insuffisante ! \n La quantité stock = " + viewModel.SelectedItem.OLD_QUANTITE, AppResources.alrt_msg_Alert, AppResources.alrt_msg_Ok);
                    }
                }
                else
                {
                    await UserDialogs.Instance.AlertAsync(" Quantité stock insuffisante ! \n La quantité stock = " + viewModel.SelectedItem.OLD_QUANTITE, AppResources.alrt_msg_Alert, AppResources.alrt_msg_Ok);
                }
            }
        }

        private void DelQte_Clicked(object sender, EventArgs e)
        {
            var lot = ((sender as Button).BindingContext as View_STK_STOCK);
            SelectedlistLot.Remove(lot);
            lot.SelectedQUANTITE = 0;
            App.MsgCenter.SendAction(this, CurrentStream, "REMOVE", MCDico.REMOVE_ITEM, viewModel.SelectedItem);
            UpdateTotaux();
        }

        private QteUpdater QteUpdater;
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
            viewModel.TotalSelected = viewModel.Items.Where(e => e.SelectedQUANTITE > 0).Sum(e => e.SelectedQUANTITE * e.SelectedPrice);
        }
    }
}
