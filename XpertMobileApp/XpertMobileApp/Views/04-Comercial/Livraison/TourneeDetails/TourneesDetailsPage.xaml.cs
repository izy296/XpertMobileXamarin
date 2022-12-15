using Acr.UserDialogs;
using Rg.Plugins.Popup.Services;
using System;
using System.Linq;
using System.Threading.Tasks;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;
using Xpert.Common.DAO;
using Xpert.Common.WSClient.Helpers;
using XpertMobileApp.Api;
using XpertMobileApp.Api.Managers;
using XpertMobileApp.Api.Services;
using XpertMobileApp.DAL;
using XpertMobileApp.SQLite_Managment;
using XpertMobileApp.ViewModels;
using ZXing.Net.Mobile.Forms;
using Xamarin.Essentials;
using System.Threading;
using XpertMobileApp.SQLite_Managment;
using System.Collections.Generic;
using Syncfusion.SfMaps.XForms;

namespace XpertMobileApp.Views
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class TourneesDetailsPage : ContentPage
    {
        TourneesDetailsViewModel viewModel;
        CancellationTokenSource cts;
        View_LIV_TOURNEE Item;

        bool autoLodData = true;
        public TourneesDetailsPage(string codeTournee, View_LIV_TOURNEE item = null)
        {
            InitializeComponent();
            if (item != null)
            {
                Item = item;
            }
            BindingContext = viewModel = new TourneesDetailsViewModel(codeTournee);
            viewModel.MyMapPage = this;

        }

        async void OnItemSelected(object sender, SelectedItemChangedEventArgs args)
        {
            var item = args.SelectedItem as View_LIV_TOURNEE_DETAIL;
            if (item == null)
                return;
            if (Item.ETAT_TOURNEE != TourneeStatus.Closed)
                await Navigation.PushAsync(new TourneeVisitPage(item));
            else await Navigation.PushAsync(new TourneeVisitPage(item, true));

            // Manually deselect item.
            listView.SelectedItem = null;
        }

        async void AddItem_Clicked(object sender, EventArgs e)
        {
            // await Navigation.PushModalAsync(new NavigationPage(new NewEncaissementPage(null, viewModel.EncaissDisplayType)));
        }

        protected override async void OnAppearing()
        {
            base.OnAppearing();
            if (Item != null)
            {
                //if (Item.ETAT_TOURNEE)
                //{
                //    await UserDialogs.Instance.AlertAsync("", AppResources.alrt_msg_Alert, AppResources.alrt_msg_Ok);

                //}
            }
            LoadData();
            //   if (viewModel.Familles.Count == 0)
            //       viewModel.LoadExtrasDataCommand.Execute(null);
        }

        public void RefreshMap(IEnumerable<View_LIV_TOURNEE_DETAIL> items)
        {
            MyMap.Layers[0].MarkerSettings.LabelSize = 15;
            MyMap.Layers[0].MarkerSettings.IconColor = Color.Red;
            MyMap.Layers[0].MarkerSettings.IconSize = 15;
            MyMap.Layers[0].MarkerSettings.MarkerIcon = MapMarkerIcon.Diamond;
            MyMap.Layers[0].Markers.Clear();

            foreach (var item in items)
            {
                if (item.GPS_LATITUDE_CLIENT != 0 && item.GPS_LONGITUDE_CLIENT != 0)
                {
                    MapMarker pin = new MapMarker()
                    {
                        Label = item.FULL_NOM_TIERS,
                        Latitude = item.GPS_LATITUDE_CLIENT.ToString(),
                        Longitude = item.GPS_LONGITUDE_CLIENT.ToString()
                    };

                    MyMap.Layers[0].Markers.Add(pin);
                }

            }
            if (MyMap.Layers[0].Markers.Count > 0)
                ((ImageryLayer)MyMap.Layers[0]).GeoCoordinates = new Point(double.Parse(MyMap.Layers[0].Markers[0].Latitude), double.Parse(MyMap.Layers[0].Markers[0].Longitude));
            MyMap.ZoomLevel = 25;
        }

        private void TypeFilter_Clicked(object sender, EventArgs e)
        {
            LoadData();
        }

        private void LoadData()
        {
            viewModel.LoadItemsCommand.Execute(null);

        }

        private void Filter_Clicked(object sender, EventArgs e)
        {
            //FilterPanel.IsVisible = !FilterPanel.IsVisible;
        }

        private async void btn_ApplyFilter_Clicked(object sender, EventArgs e)
        {
            if (App.Online)
            {
                viewModel.LoadItemsCommand.Execute(null);
            }
            else
            {
                var res = await SQLite_Manager.FilterTournee(viewModel.SearchedText);
                viewModel.Items.Clear();
                viewModel.Items.AddRange(res);
            }
        }

        private void btn_CancelFilter_Clicked(object sender, EventArgs e)
        {
            //FilterPanel.IsVisible = false;
            viewModel.ClearFilters();
            viewModel.LoadItemsCommand.Execute(null);
        }

        private void ComptePicker_SelectedIndexChanged(object sender, EventArgs e)
        {

        }

        private async void OnVisiteSwipeItemInvoked(object sender, EventArgs e)
        {
            var scaner = new ZXingScannerPage();
            Navigation.PushAsync(scaner);
            autoLodData = false;
            scaner.OnScanResult += (result) =>
            {
                scaner.IsScanning = false;
                Device.BeginInvokeOnMainThread(async () =>
                {
                    try
                    {
                        await Navigation.PopAsync();
                        var tr = (sender as SwipeItem).Parent.Parent.Parent.BindingContext as View_LIV_TOURNEE_DETAIL;
                        View_TRS_TIERS tiers = null;
                        if (App.Online)
                        {
                            tiers = await SelectScanedTiers(result.Text);
                            if (tiers != null && tiers.CODE_TIERS == tr.CODE_TIERS)
                            {
                                tr.CODE_ETAT_VISITE = TourneeStatus.Visited;
                                tr.ETAT_COLOR = "#FFA500";
                                var res = await viewModel.UpdateItem(tr);
                                await SaveGPsLocationToTiers(tiers);
                            }
                        }
                        else
                        {
                            tiers = await SQLite_Manager.SelectScanedTiers(result.Text);
                            if (tiers != null && tiers.CODE_TIERS == tr.CODE_TIERS)
                            {
                                var res = await SQLite_Manager.UpdateTourneeItemVisited(tr);
                                await SaveGPsLocationToTiers(tiers);
                            }
                        }
                        LoadData();
                    }
                    catch (Exception ex)
                    {
                        await UserDialogs.Instance.AlertAsync(ex.Message, AppResources.alrt_msg_Alert, AppResources.alrt_msg_Ok);
                    }
                    autoLodData = true;
                    /*
                    ClassId = string.Format("pb_{0}", vteLot.ID);
                    var pbruteElem2 = ItemsListView.Children.Where(x => x.ClassId == ClassId).FirstOrDefault() as SfNumericTextBox;
                    pbruteElem2.Focus();
                    */
                });
            };
        }


        async Task SaveGPsLocationToTiers(View_TRS_TIERS tiers)
        {
            bool getGPsSucces = false;
            try
            {

                UserDialogs.Instance.ShowLoading(AppResources.txt_Waiting);
                var request = new GeolocationRequest(GeolocationAccuracy.Medium, TimeSpan.FromSeconds(10));
                cts = new CancellationTokenSource();
                var location = await Geolocation.GetLocationAsync(request, cts.Token);
                if (location != null)
                {
                    tiers.GPS_LATITUDE = location.Latitude;
                    tiers.GPS_LONGITUDE = location.Longitude;
                    Console.WriteLine($"Latitude: {location.Latitude}, Longitude: {location.Longitude}, Altitude: {location.Altitude}");
                    getGPsSucces = true;
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
            if (getGPsSucces)
            {
                await SaveGpsToTiers(tiers);
            }
        }

        private static async Task SaveGpsToTiers(View_TRS_TIERS tiers)
        {
            try
            {
                UserDialogs.Instance.ShowLoading(AppResources.txt_Waiting);
                bool seucces = false;
                //await App.IsConected();
                if (App.Online)
                {
                    seucces = await CrudManager.TiersManager.saveGPSToTiers(tiers);
                }
                else
                {
                    seucces = await SQLite_Manager.saveGPSToTiers(tiers);
                }
                if (seucces)
                {
                    await UserDialogs.Instance.AlertAsync("Sauvgarde des localisation GPS echouées !", AppResources.alrt_msg_Alert, AppResources.alrt_msg_Ok);
                }
                else
                {
                    await UserDialogs.Instance.AlertAsync("Sauvgarde des localisation GPS avec succes !", AppResources.alrt_msg_Alert, AppResources.alrt_msg_Ok);
                }
            }
            catch (Exception ex)
            {
                await UserDialogs.Instance.AlertAsync(ex.Message, AppResources.alrt_msg_Alert, AppResources.alrt_msg_Ok);
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
        internal async Task<View_TRS_TIERS> SelectScanedTiers(string cb_tiers)
        {
            try
            {
                // Récupérer le lot depuis le serveur
                XpertSqlBuilder qb = new XpertSqlBuilder();
                qb.AddCondition<View_TRS_TIERS, string>(x => x.CODE_TIERS, cb_tiers);
                qb.AddOrderBy<View_TRS_TIERS, string>(x => x.CODE_TIERS);
                var tiers = await CrudManager.TiersManager.SelectByPage(qb.QueryInfos, 1, 2);
                if (tiers == null)
                    return null;

                XpertHelper.PeepScan();

                if (tiers.Count() > 1)
                {
                    await UserDialogs.Instance.AlertAsync("Plusieurs tiers pour ce code barre!", AppResources.alrt_msg_Alert, AppResources.alrt_msg_Ok);
                    return null;
                }
                else if (tiers.Count() == 0)
                {
                    await UserDialogs.Instance.AlertAsync("Aucun tiers pour ce code barre!", AppResources.alrt_msg_Alert, AppResources.alrt_msg_Ok);
                    return null;
                }
                else
                {
                    return tiers.FirstOrDefault();
                }
            }
            catch (Exception ex)
            {
                await UserDialogs.Instance.AlertAsync(WSApi2.GetExceptionMessage(ex), AppResources.alrt_msg_Alert,
                    AppResources.alrt_msg_Ok);
                return null;
            }
        }

        private async void OnDelevrySwipeItemInvoked(object sender, EventArgs e)
        {
            var item = viewModel.SelectedItem;
            var tr = (sender as SwipeItem).Parent.Parent.Parent.BindingContext as View_LIV_TOURNEE_DETAIL;
            if (App.Online == true)
            {
                var session = await CrudManager.Sessions.GetCurrentSession();
                if (session == null)
                {
                    var res = await DisplayAlert(AppResources.msg_Confirmation, AppResources.msg_ShouldPrepaireSession, AppResources.alrt_msg_Ok, AppResources.alrt_msg_Cancel);
                    if (res)
                    {
                        OpenSessionPage openPage = new OpenSessionPage(viewModel.CurrentStream);
                        await PopupNavigation.Instance.PushAsync(openPage);
                    }
                }
                else
                {
                    var trs = new View_TRS_TIERS();
                    trs.CODE_TIERS = tr.CODE_TIERS;
                    trs.NOM_TIERS1 = tr.FULL_NOM_TIERS;
                    VenteFormLivraisonPage form = new VenteFormLivraisonPage(null, VentesTypes.Livraison, trs, tr.CODE_DETAIL);

                    await Navigation.PushAsync(form);
                }
            }
            else
            {
                var session = await SQLite_Manager.GetInstance().Table<TRS_JOURNEES>().FirstOrDefaultAsync();
                if (session == null)
                {
                    var res = await DisplayAlert(AppResources.msg_Confirmation, AppResources.msg_ShouldPrepaireSession, AppResources.alrt_msg_Ok, AppResources.alrt_msg_Cancel);
                    if (res)
                    {
                        OpenSessionPage openPage = new OpenSessionPage(viewModel.CurrentStream);
                        await PopupNavigation.Instance.PushAsync(openPage);
                    }
                }
                else
                {
                    var trs = new View_TRS_TIERS();
                    trs.CODE_TIERS = tr.CODE_TIERS;
                    trs.NOM_TIERS1 = tr.FULL_NOM_TIERS;
                    VenteFormLivraisonPage form = new VenteFormLivraisonPage(null, VentesTypes.Livraison, trs, tr.CODE_DETAIL);

                    await Navigation.PushAsync(form);
                }
            }

        }
    }
}