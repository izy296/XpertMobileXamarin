using Acr.UserDialogs;
using Rg.Plugins.Popup.Services;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;
using Xpert.Common.WSClient.Helpers;
using XpertMobileApp.Api;
using XpertMobileApp.Api.Managers;
using XpertMobileApp.Api.Services;
using XpertMobileApp.Helpers;
using XpertMobileApp.Models;
using XpertMobileApp.Services;
using XpertMobileApp.ViewModels;

namespace XpertMobileApp.Views._04_Comercial.TransfertStock
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class NewTransfertStock : ContentPage
    {
        public LotSelectorLivraisonUniteFamille productSelector;
        public NewTransfertStockViewModel viewModel;
        private bool isStillBusy = false;
        public NewTransfertStock()
        {
            InitializeComponent();
            BindingContext = viewModel = new NewTransfertStockViewModel();
        }
        protected async override void OnAppearing()
        {
            base.OnAppearing();
            if (App.Online)
            {
                if (viewModel.Magasin.Count <= 0)
                {
                    await viewModel.ExecuteLoadMagasinsCommand();

                }
                if (viewModel.AllProductInStock.Count <= 0)
                {
                    await viewModel.ExecuteLoadStockCommand();
                }
                GetSelectedMagasin();
            }
        }

        public void GetSelectedMagasin()
        {
            try
            {
                if (!string.IsNullOrEmpty(App.CODE_MAGASIN) && viewModel.Magasin.Count() > 0)
                {
                    string CodeMagasin = App.CODE_MAGASIN;
                    viewModel.SelectedMagasinSource = viewModel.Magasin.Where(magasin => magasin.CODE == CodeMagasin).First();
                    magasinSourcePicker.SelectedItem = viewModel.SelectedMagasinSource;
                    magasinSourcePicker.IsEnabled = false;
                }

            }
            catch (Exception ex)
            {

                throw ex;
            }
        }
        /// <summary>
        /// Close the popup page 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private async void Cancel_Clicked(object sender, EventArgs e)
        {
            await Navigation.PopModalAsync();
        }

        private async void Save_Clicked(object sender, EventArgs e)
        {
            try
            {

                // check if the destination magasin is set ...
                if (viewModel.SelectedMagasinDest == null || viewModel.SelectedMagasinSource == null)
                {
                    CustomPopup customPopup = new CustomPopup("Veuillez Selectionner Un magasin Source !", trueMessage: AppResources.alrt_msg_Ok);
                    await PopupNavigation.Instance.PushAsync(customPopup);
                    if (await customPopup.PopupClosedTask)
                    {
                        if (customPopup.Result)
                        {
                            return;
                        }
                    }
                }
                else
                {
                    View_STK_TRANSFERT transfert = new View_STK_TRANSFERT();

                    if (viewModel.SelectedMagasinSource != null)
                        transfert.MAGASIN_SOURCE = viewModel.SelectedMagasinSource.CODE;

                    if (viewModel.SelectedMagasinDest != null)
                        transfert.MAGASIN_DESTINATION = viewModel.SelectedMagasinDest.CODE;

                    transfert.DATE_TRANSEFRT = DateTime.Now;
                    transfert.TOTAL_ACHAT = viewModel.MontantTotal;
                    transfert.DETAILS = new List<View_STK_TRANSFERT_DETAIL>();
                    foreach (var item in viewModel.AllProductInStock)
                    {
                        if (item.DECHARGE_SELECTED)
                        {
                            transfert.DETAILS.Add(new View_STK_TRANSFERT_DETAIL
                            {
                                CODE_PRODUIT = item.CODE_PRODUIT,
                                DESIGNATION = item.DESIGNATION_PRODUIT,
                                QUANTITE = item.QUANTITE,
                                COUT_ACHAT = item.COUT_ACHAT,
                                CODE_EMPLACEMENT_DESTINIATION = viewModel.SelectedMagasinDest.CODE,
                                ID_STOCK_SOURCE = item.ID_STOCK,
                                MAGASIN_DESTINATION = item.CODE_MAGASIN,
                                NUM_TRANSFERT = item.NUM_DOC,
                            });
                        }
                    }

                    if (transfert.DETAILS.Count > 0)
                    {
                        if (App.Online)
                        {
                            bool result = await CrudManager.StockTransfert.InsertTransfert(transfert);
                            if (result)
                            {
                                CustomPopup customPopup = new CustomPopup("Déchargement fait avec succée", trueMessage: AppResources.alrt_msg_Ok);
                                await PopupNavigation.Instance.PushAsync(customPopup);
                                await Navigation.PopModalAsync();
                            }
                        }
                    }
                }

            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        private void SelectedItem(object sender, SelectionChangedEventArgs e)
        {
            try
            {
                if (e.CurrentSelection != null && e.CurrentSelection.Count > 0)
                {
                    (e.CurrentSelection.Last() as View_STK_STOCK).DECHARGE_SELECTED = !(e.CurrentSelection.Last() as View_STK_STOCK).DECHARGE_SELECTED;
                    if ((e.CurrentSelection.Last() as View_STK_STOCK).DECHARGE_SELECTED)
                    {
                        viewModel.NumOfProductSelected += (int)(e.CurrentSelection.Last() as View_STK_STOCK).QUANTITE;
                        viewModel.MontantTotal += (e.CurrentSelection.Last() as View_STK_STOCK).QUANTITE * (e.CurrentSelection.Last() as View_STK_STOCK).PRIX_VENTE;
                    }
                    else
                    {
                        viewModel.NumOfProductSelected -= (int)(e.CurrentSelection.Last() as View_STK_STOCK).QUANTITE;
                        viewModel.MontantTotal -= (e.CurrentSelection.Last() as View_STK_STOCK).QUANTITE * (e.CurrentSelection.Last() as View_STK_STOCK).PRIX_VENTE;
                    }

                    collectionView.SelectedItem = null;
                }

            }
            catch (Exception ex)
            {

                throw ex;
            }
        }

        private void SelectAll(object sender, Syncfusion.XForms.Buttons.StateChangedEventArgs e)
        {
            try
            {

                var listeOfProducts = viewModel.AllProductInStock;
                isStillBusy = true;
                if (e.IsChecked.HasValue && e.IsChecked.Value)
                {
                    //here checked state...
                    viewModel.NumOfProductSelected = 0;
                    viewModel.MontantTotal = 0;
                    foreach (var item in listeOfProducts)
                    {
                        item.DECHARGE_SELECTED = true;
                        viewModel.NumOfProductSelected += (int)item.QUANTITE;
                        viewModel.MontantTotal += item.PRIX_VENTE * item.QUANTITE;
                    }
                }
                else if (e.IsChecked.HasValue && !e.IsChecked.Value)
                {
                    // Unchecked state...
                    viewModel.NumOfProductSelected = 0;
                    viewModel.MontantTotal = 0;
                    foreach (var item in listeOfProducts)
                    {
                        item.DECHARGE_SELECTED = false;
                    }
                }
                else
                {
                    // Inditeremined state...
                    viewModel.NumOfProductSelected = 0;
                    viewModel.MontantTotal = 0;
                    foreach (var item in listeOfProducts)
                    {
                        item.DECHARGE_SELECTED = false;
                    }
                }
                isStillBusy = false;
            }
            catch (Exception ex)
            {

                throw ex;
            }
        }

        private void SelectProduct(object sender, Syncfusion.XForms.Buttons.StateChangedEventArgs e)
        {
            try
            {
                if (isStillBusy == false)
                {
                    View_STK_STOCK item = (View_STK_STOCK)((Syncfusion.XForms.Buttons.SfCheckBox)sender).BindingContext;

                    if (item != null)
                    {
                        if (e.IsChecked.HasValue && e.IsChecked.Value)
                        {
                            viewModel.NumOfProductSelected += (int)(item.QUANTITE);
                            viewModel.MontantTotal += item.QUANTITE * item.PRIX_VENTE;
                        }
                        else if (e.IsChecked.HasValue && !e.IsChecked.Value)
                        {
                            viewModel.NumOfProductSelected -= (int)(item.QUANTITE);
                            viewModel.MontantTotal -= item.QUANTITE * item.PRIX_VENTE;
                        }
                    }
                }


            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
    }
}