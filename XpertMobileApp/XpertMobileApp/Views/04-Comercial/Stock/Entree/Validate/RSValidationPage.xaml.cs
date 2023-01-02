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
using XpertMobileApp.Api.Managers;
using XpertMobileApp.SQLite_Managment;
using Xamarin.Essentials;
using System.Threading.Tasks;
using System.Threading;

namespace XpertMobileApp.Views
{
    public partial class RSValidationPage : PopupPage
    {
        RSValidationViewModel viewModel;

        public RSFormViewModel ParentviewModel { get; set; }
        public string ParentStream { get; set; }
        public View_STK_ENTREE Item
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
        CancellationTokenSource cts;
        public RSValidationPage(string stream, View_STK_ENTREE item, View_TRS_TIERS tiers = null)
        {
            InitializeComponent();

            ParentStream = stream;
            BindingContext = viewModel = new RSValidationViewModel(item, "", tiers);

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
        async Task SaveGPsLocationToVente(View_STK_ENTREE entree)
        {
            try
            {
                UserDialogs.Instance.ShowLoading(AppResources.txt_Waiting);

                //var request = new GeolocationRequest(GeolocationAccuracy.Medium, TimeSpan.FromSeconds(10));
                //cts = new CancellationTokenSource();
                //var location = await Geolocation.GetLocationAsync(request, cts.Token);
                //if (location != null)
                {
                    //entree.GPS_LATITUDE = location.Latitude;
                    //entree.GPS_LONGITUDE = location.Longitude;
                    //Console.WriteLine($"Latitude: {location.Latitude}, Longitude: {location.Longitude}, Altitude: {location.Altitude}");
                }
            }
            catch (FeatureNotSupportedException fnsEx)
            {
                await UserDialogs.Instance.AlertAsync("Géolocalisation non pris en charge sur le périphérique!", AppResources.alrt_msg_Alert, AppResources.alrt_msg_Ok);
                // Handle not supported on device exception
            }
            catch (FeatureNotEnabledException fneEx)
            {
                await UserDialogs.Instance.AlertAsync("Géolocalisation non activé sur le périphérique!", AppResources.alrt_msg_Alert, AppResources.alrt_msg_Ok); // Handle not supported on device exception
                // Handle not enabled on device exception
            }
            catch (PermissionException pEx)
            {
                await UserDialogs.Instance.AlertAsync("Géolocalisation Problème d'autorisation!", AppResources.alrt_msg_Alert, AppResources.alrt_msg_Ok); // Handle not supported on device exception
                // Handle permission exception
            }
            catch (Exception ex)
            {
                await UserDialogs.Instance.AlertAsync("Impossible d'obtenir l'emplacement!", AppResources.alrt_msg_Alert, AppResources.alrt_msg_Ok); // Handle not supported on device exception
                // Unable to get location
            }
            finally
            {
                UserDialogs.Instance.HideLoading();
            }
        }

        protected override void OnDisappearing()
        {
            if (cts != null && !cts.IsCancellationRequested)
                cts.Cancel();
            base.OnDisappearing();
        }
        private async void btnValidate_Clicked(object sender, EventArgs e)
        {
            try
            {
                //viewModel.Item.GPS_LATITUDE = 0;
                //viewModel.Item.GPS_LONGITUDE = 0;
                //await SaveGPsLocationToVente(viewModel.Item);
                if (viewModel.Item.Details != null)
                {
                    string res = await UpdateDatabase.AjoutEntree(viewModel.Item);
                    if (!XpertHelper.IsNullOrEmpty(res))
                    {
                        await DisplayAlert(AppResources.alrt_msg_Info, AppResources.txt_actionsSucces, AppResources.alrt_msg_Ok);
                        if (viewModel.imprimerTecketCaiss)
                        {
                            PrinterHelper.PrintEntree(viewModel.Item);
                        }
                        ParentviewModel.InitNewEntree();
                        await PopupNavigation.Instance.PopAsync();
                    }
                }
                else
                {
                    await UserDialogs.Instance.AlertAsync("Veuillez selectionner au moins un produit!", AppResources.alrt_msg_Alert, AppResources.alrt_msg_Ok);
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


        
        }

        private void SfNumericTextBox_ValueChanged(object sender, Syncfusion.SfNumericTextBox.XForms.ValueEventArgs e)
        {

        }

        private void TOTAL_RECU_ValueChanged(object sender, Syncfusion.SfNumericTextBox.XForms.ValueEventArgs e)
        {
           
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
