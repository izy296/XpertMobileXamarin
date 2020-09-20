using System;
using Rg.Plugins.Popup.Pages;
using Xamarin.Forms;
using Rg.Plugins.Popup.Services;
using XpertMobileApp.ViewModels;
using XpertMobileApp.DAL;
using XpertMobileApp.Api.Services;
using XpertMobileApp.Models;
using Xpert.Common.WSClient.Helpers;
using Acr.UserDialogs;
using ZXing.Net.Mobile.Forms;
using Rg.Plugins.Popup.Extensions;
using XpertWebApi.Models;
using System.Collections.Generic;
using XpertMobileApp.Services;
using XpertMobileSettingsPage.Helpers.Services;

namespace XpertMobileApp.Views
{
    public partial class VteValidationPage : PopupPage
    {
        VteValidationViewModel viewModel;

        public VenteFormViewModel ParentviewModel { get; set; }
        public string ParentStream { get; set; }
        public View_VTE_VENTE Item 
        { 
            get 
            { 
                return viewModel.Item; 
            } 
            internal set 
            { 
                viewModel.Item = value; 
            } 
        }

        public VteValidationPage(string stream, View_VTE_VENTE item)
        {
            InitializeComponent();
 
            ParentStream = stream;
            BindingContext = viewModel = new VteValidationViewModel(item);

            // Initialisation du montant reçu au reste a payer
            viewModel.Item.TOTAL_RECU = item.TOTAL_RESTE;
            // this.SfNE_MTRecu.Value = item.TOTAL_RESTE;
            UpdateMontants(viewModel.Item.TOTAL_RECU);

            MessagingCenter.Subscribe<TiersSelector, View_TRS_TIERS>(this, viewModel.CurrentStream, async (obj, selectedItem) =>
            {
                viewModel.SelectedTiers = selectedItem;
            });
           
        }

        protected override void OnAppearing()
        {
            base.OnAppearing();
        }


        private async void btnCancel_Clicked(object sender, EventArgs e)
        {
            await PopupNavigation.Instance.PopAsync();
        }
      
        private async void btnValidate_Clicked(object sender, EventArgs e)
        {
            try
            {
                string res = await viewModel.ValidateVte(viewModel.Item);

                if (!XpertHelper.IsNullOrEmpty(res) )
                {
                    await DisplayAlert(AppResources.alrt_msg_Info, AppResources.txt_actionsSucces, AppResources.alrt_msg_Ok);
                    if (viewModel.imprimerTecketCaiss)
                    {
                        if(SettingsPage.printerLocal!=null && SettingsPage.printerLocal.isConnected())
                        {
                            var tecketData = await WebServiceClient.GetDataTecketCaisseVente(res);
                            if (tecketData == null) return;
                            if (tecketData.Count == 0)
                            {
                                await DisplayAlert(AppResources.alrt_msg_Info, "Pas de données à imprimer !", AppResources.alrt_msg_Ok);
                            }
                            else
                            {
                                PrinterHelper.PrintBL(tecketData);
                            }
                        }
                        else
                        {
                            await UserDialogs.Instance.AlertAsync(AppResources.alrt_msg_Info, "Imprimante mobile non connectée !", AppResources.alrt_msg_Ok);
                        }

                    }
                    ParentviewModel.InitNewVentes();
                    await PopupNavigation.Instance.PopAsync(); 
                }
            }
            catch (Exception ex) 
            {
                await UserDialogs.Instance.AlertAsync(WSApi2.GetExceptionMessage(ex), AppResources.alrt_msg_Alert, AppResources.alrt_msg_Ok);
            }
        }

        private TiersSelector itemSelector;
        private async void btn_Select_Clicked(object sender, EventArgs e)
        {
            itemSelector = new TiersSelector(viewModel.CurrentStream);
            await PopupNavigation.Instance.PushAsync(itemSelector);
        }

        private void btn_Scan_Clicked(object sender, EventArgs e)
        {
            _scanView.IsVisible = !_scanView.IsVisible;
            _scanView.IsScanning = !_scanView.IsScanning;


            /*
            var scaner = new ZXingScannerPage();
            Navigation.PushModalAsync(scaner);
            scaner.OnScanResult += (result) =>
            {
                scaner.IsScanning = false;
                Device.BeginInvokeOnMainThread(async () =>
                {
                    await Navigation.PopModalAsync();
                    await viewModel.SelectScanedTiers(result.Text);
                });
            };
            */
        }

        private void SfNumericTextBox_ValueChanged(object sender, Syncfusion.SfNumericTextBox.XForms.ValueEventArgs e)
        {

        }

        private void TOTAL_RECU_ValueChanged(object sender, Syncfusion.SfNumericTextBox.XForms.ValueEventArgs e)
        {
            decimal Mt_Recu = Convert.ToDecimal(e.Value);
            UpdateMontants(Mt_Recu);
        }

        private void UpdateMontants(decimal mt_Recu) 
        {

            if (mt_Recu >= viewModel.Item.TOTAL_RESTE)
            {
                viewModel.Item.MBL_MT_RENDU = mt_Recu - viewModel.Item.TOTAL_RESTE;
                viewModel.Item.MBL_MT_VERCEMENT = viewModel.Item.TOTAL_RESTE;
            }
            else
            {
                viewModel.Item.MBL_MT_RENDU = 0;
                viewModel.Item.MBL_MT_VERCEMENT = mt_Recu;
            }
        }

        private async void Handle_OnScanResult(ZXing.Result result)
        {
            try
            {
                string cb = result.Text;
                Device.BeginInvokeOnMainThread(async () =>
                {
                    _scanView.IsVisible = false;
                    _scanView.IsScanning = false;
                    await viewModel.SelectScanedTiers(cb);
                });
            }
            catch (Exception ex) 
            {
                await UserDialogs.Instance.AlertAsync(WSApi2.GetExceptionMessage(ex), AppResources.alrt_msg_Alert, AppResources.alrt_msg_Ok);
            }
        }
    }
}
