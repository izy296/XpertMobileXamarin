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
using XpertMobileApp.Api;
using XpertMobileApp.Views.Helper;
using XpertMobileApp.Helpers;
using Syncfusion.SfNumericTextBox.XForms;

namespace XpertMobileApp.Views
{
    public partial class VteValidationPage : PopupPage
    {
        VteValidationViewModel viewModel;

        public VenteFormViewModel ParentviewModel { get; set; }
        public VenteFormLivraisonViewModel ParentLivraisonviewModel { get; set; }

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
        CancellationTokenSource cts;
        public VteValidationPage(string stream, View_VTE_VENTE item, View_TRS_TIERS tiers = null)
        {
            InitializeComponent();

            if (Constants.AppName == Apps.XCOM_Livraison)
                PrintTicketZoneLayout.IsVisible = true;

            if (Constants.AppName == Apps.X_DISTRIBUTION)
            {
                PointsFideliteLabel.IsVisible = false;
                PointsFidelite.IsVisible = false;
                if (item.TYPE_DOC == "CC")
                    TitleLabel.Text = AppResources.pn_NewCommande;
                else if (item.TYPE_DOC == "BR")
                    TitleLabel.Text = "Nouvelle Bon de Retour";
                else if (item.TYPE_DOC == "BL")
                    TitleLabel.Text = "Nouvelle de Bon Livraison";
            }


            //if (Constants.AppName != Apps.XCOM_Livraison)
            //    SfNE_MTRecu.IsEnabled = false;

            ParentStream = stream;
            BindingContext = viewModel = new VteValidationViewModel(item, "", tiers);

            if (tiers != null)
            {
                viewModel.SelectedTiers = tiers;
            }
            // Initialisation du montant reçu au reste a payer
            if (Constants.AppName != Apps.XCOM_Livraison)
            {
                viewModel.Item.TOTAL_RECU = item.TOTAL_RESTE;
                this.SfNE_MTRecu.Value = item.TOTAL_RESTE;
            }

            UpdateMontants(viewModel.Item.TOTAL_RECU);

            if (Apps.X_DISTRIBUTION == Constants.AppName)
            {
                var list = Container.Children;
                if (viewModel.Item.TYPE_DOC == "BR")
                {
                    SfNE_MTRecu.Minimum = null;
                    SfNE_MTRecu.Maximum = 0;
                    foreach (var element in list)
                    {
                        if (element.GetType() == typeof(SfNumericTextBox))
                        {
                            ((SfNumericTextBox)element).Minimum = null;
                            ((SfNumericTextBox)element).Maximum = 0;
                        }
                    }
                }
                else {
                    SfNE_MTRecu.Minimum = 0;
                    SfNE_MTRecu.Maximum = null;
                    foreach (var element in list)
                    {
                        if (element.GetType() == typeof(SfNumericTextBox))
                        {
                            ((SfNumericTextBox)element).Minimum = 0;
                            ((SfNumericTextBox)element).Maximum = null;
                        }
                    }
                }
            }


            MessagingCenter.Subscribe<TiersSelector, View_TRS_TIERS>(this, viewModel.CurrentStream, async (obj, selectedItem) =>
            {
                Device.BeginInvokeOnMainThread(() =>
                {
                    viewModel.SelectedTiers = selectedItem;
                    pts_Consumed.Value = 0;
                    CheckPointFidelParam();
                    //Trouver un moyen pour le bind le viewmodel apres modification                    
                    pts_Consumed.IsEnabled = viewModel.PointFideliteParam;
                });
            });
        }

        public void CheckPointFidelParam()
        {
            if (App.PARAM_FIDELITE_TIERS == 0)
            {
                viewModel.PointFideliteParam = false;
            }
            else if (viewModel.SelectedTiers.TOTAL_POINT_FIDELITE >= App.PARAM_FIDELITE_TIERS)
            {
                viewModel.PointFideliteParam = true;
            }
            else
            {
                viewModel.PointFideliteParam = false;
            }
        }
        protected override void OnAppearing()
        {
            base.OnAppearing();

            MessagingCenter.Subscribe<VenteFormLivraisonPage, string>(this, "TourneeVisit", async (obj, selectedItem) =>
            {
                if (Constants.AppName == Apps.X_DISTRIBUTION)
                {
                    btn_Search.IsVisible = false;
                    btn_Scan.IsVisible = false;
                }
            });
        }


        private async void btnCancel_Clicked(object sender, EventArgs e)
        {
            await PopupNavigation.Instance.PopAsync();
        }
        async Task SaveGPsLocationToVente(View_VTE_VENTE vente)
        {
            try
            {
                UserDialogs.Instance.ShowLoading(AppResources.txt_Waiting);

                var request = new GeolocationRequest(GeolocationAccuracy.Medium, TimeSpan.FromSeconds(10));
                cts = new CancellationTokenSource();
                var location = await Geolocation.GetLocationAsync(request, cts.Token);
                if (location != null)
                {
                    vente.GPS_LATITUDE = location.Latitude;
                    vente.GPS_LONGITUDE = location.Longitude;
                    Console.WriteLine($"Latitude: {location.Latitude}, Longitude: {location.Longitude}, Altitude: {location.Altitude}");
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
                if (Constants.AppName == Apps.X_DISTRIBUTION)
                {
                    Location location = await Manager.GetLocation();
                    if (location == null)
                    {
                        await UserDialogs.Instance.AlertAsync("Veuillez verifier la localisation", AppResources.alrt_msg_Alert, AppResources.alrt_msg_Ok);
                        location = new Location(0, 0);
                    }
                    viewModel.Item.GPS_LATITUDE = location.Latitude;
                    viewModel.Item.GPS_LONGITUDE = location.Longitude;
                    await SaveGPsLocationToVente(viewModel.Item);
                }

                if (App.Online)
                {
                    if (((decimal)SfNE_MTRecu.Value < (decimal)mt_Reste.Value) && viewModel.Item.CODE_TIERS == "CXPERTCOMPTOIR")
                    {
                        await UserDialogs.Instance.AlertAsync("Veuillez selectionner un client !", AppResources.alrt_msg_Alert, AppResources.alrt_msg_Ok);
                        return;
                    }

                    string res = await viewModel.ValidateVte(viewModel.Item);
                    viewModel.Item.NUM_VENTE = res;
                    if (!XpertHelper.IsNullOrEmpty(res))
                    {
                        await DisplayAlert(AppResources.alrt_msg_Info, AppResources.txt_actionsSucces, AppResources.alrt_msg_Ok);

                        //Impression ticket de caisse depuis l'appareil bluetooth
                        if (Constants.AppName == Apps.XCOM_Livraison)
                            if (viewModel.imprimerTecketCaiss)
                            {
                                await PrinterHelper.PrintBL(viewModel.Item);
                            }
                        if (Constants.AppName != Apps.X_DISTRIBUTION)
                            ParentviewModel.InitNewVentes();
                        else ParentLivraisonviewModel.InitNewVentes();

                        await PopupNavigation.Instance.PopAsync();
                    }
                }
                else
                {
                    try
                    {
                        if (viewModel.Item.Details != null || viewModel.Item.DetailsDistrib != null)
                        {
                            string res = await SQLite_Manager.AjoutVente(viewModel.Item);
                            if (!XpertHelper.IsNullOrEmpty(res))
                            {
                                await DisplayAlert(AppResources.alrt_msg_Info, AppResources.txt_actionsSucces, AppResources.alrt_msg_Ok);
                                if (viewModel.imprimerTecketCaiss)
                                {
                                    await PrinterHelper.PrintBL(viewModel.Item);
                                }
                                if (Constants.AppName != Apps.X_DISTRIBUTION)
                                    ParentviewModel.InitNewVentes();
                                else ParentLivraisonviewModel.InitNewVentes();

                                await PopupNavigation.Instance.PopAsync();
                            }
                            else
                            {
                                if (App.PrefixCodification == "" || App.PrefixCodification == null)
                                {
                                    await UserDialogs.Instance.AlertAsync("Veuillez Verifier le Prefix", AppResources.alrt_msg_Alert, AppResources.alrt_msg_Ok);
                                    await PopupNavigation.Instance.PopAsync();
                                }
                            }
                        }
                        else
                        {
                            await UserDialogs.Instance.AlertAsync("Veuillez selectionner au moins un produit!", AppResources.alrt_msg_Alert, AppResources.alrt_msg_Ok);
                        }
                    }
                    catch (Exception Ex)
                    {

                        throw Ex;
                    }

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
            itemSelector.SearchedType = "C";
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

        private async void POINTS_CONSUMED_Changed(object sender, Syncfusion.SfNumericTextBox.XForms.ValueEventArgs e)
        {
            if (Convert.ToDecimal(pts_Consumed.Value) > 0)
            {
                if (Convert.ToDecimal(pts_Consumed.Value) > viewModel.SelectedTiers.TOTAL_POINT_FIDELITE)
                {
                    pts_Consumed.Value = viewModel.SelectedTiers.TOTAL_POINT_FIDELITE;
                    return;
                }

                var vteBll = CrudManager.GetVteBll(null);
                var res = await vteBll.GetFideliteInfos(viewModel.SelectedTiers.CODE_CARTE_FIDELITE, Convert.ToDecimal(pts_Consumed.Value));

                mt_PointsUsed.Text = "(" + res.MT_POINTS_USED.ToString("N2") + "Da)";
                viewModel.Item.REMISE_GLOBALE = res.MT_POINTS_USED;
                mt_Reste.Value = viewModel.Item.TOTAL_TTC - viewModel.Item.REMISE_GLOBALE;
                SfNE_MTRecu.Value = mt_Reste.Value;
            }
            else
            {
                mt_PointsUsed.Text = "";
            }
        }

        private void CashbtnClicked(object sender, EventArgs e)
        {
            Item.TOTAL_RECU = Item.TOTAL_TTC;

            decimal Mt_Recu = Convert.ToDecimal(Item.TOTAL_RECU);
            UpdateMontants(Mt_Recu);
        }
    }
}
