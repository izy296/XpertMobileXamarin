using Acr.UserDialogs;
using Rg.Plugins.Popup.Services;
using Syncfusion.SfMaps.XForms;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web;
using Xamarin.Essentials;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;
using XpertMobileApp.Api.Managers;
using XpertMobileApp.DAL;
using XpertMobileApp.Services;
using XpertMobileApp.SQLite_Managment;
using XpertMobileApp.ViewModels;

namespace XpertMobileApp.Views
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class TourneeVisitPage : ContentPage
    {
        TourneeVisitViewModel viewModel;
        View_TRS_TIERS tier;
        private View_LIV_TOURNEE_DETAIL livTournee;
        private bool isOpen = false;
        public TourneeVisitPage()
        {
            InitializeComponent();
            BindingContext = viewModel = new TourneeVisitViewModel();
        }

        public TourneeVisitPage(View_LIV_TOURNEE_DETAIL item)
        {
            InitializeComponent();
            livTournee = item;
            BindingContext = viewModel = new TourneeVisitViewModel(item);
            new Command(async () =>
            {
                if (App.Online)
                {
                    var result = await WebServiceClient.GetTier(item.CODE_TIERS);
                    if (result.Count != 0)
                        tier = result.First();
                }
                else
                {
                    var result = await SQLite_Manager.GetClient(item.CODE_TIERS);
                    if (result != null)
                        tier = result;

                }

            }).Execute(null);
        }

        protected async override void OnAppearing()
        {
            base.OnAppearing();
            UserDialogs.Instance.ShowLoading();
            if (livTournee != null)
            {
                var currentClient = await SQLite_Manager.GetClient(livTournee.CODE_TIERS);
                soldeTierLabel.Text = string.Format("{0:N2} DA", currentClient.SOLDE_TIERS); ;
            }
            new Command(async () =>
            {
                await viewModel.ExecuteLoadActiviteCommand();
            }).Execute(null);
            UserDialogs.Instance.HideLoading();
        }

        public async void SaveNewLocation(Point point)
        {
            if (point != null)
            {
                try
                {
                    var postition = point;
                    tier.GPS_LATITUDE = postition.Y;
                    tier.GPS_LONGITUDE = postition.X;
                    if (App.Online)
                    {
                        UserDialogs.Instance.ShowLoading();
                        await CrudManager.TiersManager.UpdateItemAsync(tier);
                        await UserDialogs.Instance.AlertAsync("Mise a jour avec succee", AppResources.alrt_msg_Alert, AppResources.alrt_msg_Ok);
                        UserDialogs.Instance.HideLoading();
                    }
                    else
                    {
                        UserDialogs.Instance.ShowLoading();
                        await SQLite_Manager.UpdateTiers(tier);

                        await UserDialogs.Instance.AlertAsync("Mise a jour avec succee", AppResources.alrt_msg_Alert, AppResources.alrt_msg_Ok);
                        UserDialogs.Instance.HideLoading();
                    }

                }
                catch (Exception)
                {
                    throw;
                }
                finally
                {
                    UserDialogs.Instance.HideLoading();
                }

            }

        }

        private async void Location(object sender, EventArgs e)
        {
            if (tier != null)
            {
                if (tier.GPS_LATITUDE == 0 && tier.GPS_LONGITUDE == 0)
                {
                    var result = await UserDialogs.Instance.ConfirmAsync("Location est Vide, Voulez vous la mise a jour avec votre position", "Confirm Selection", "Oui", "Non");
                    if (!result)
                    {
                        LocationSelector locationSelector = new LocationSelector(tier);
                        await PopupNavigation.Instance.PushAsync(locationSelector);
                        if (await locationSelector.PopupClosedTask)
                        {
                            SaveNewLocation(locationSelector.Result);
                        }
                    }
                    else
                    {
                        try
                        {
                            UserDialogs.Instance.ShowLoading();
                            var location = await Geolocation.GetLocationAsync();
                            tier.GPS_LATITUDE = location.Latitude;
                            tier.GPS_LONGITUDE = location.Longitude;
                            if (App.Online)
                            {
                                await CrudManager.TiersManager.UpdateItemAsync(tier);
                            }
                            else
                            {
                                await SQLite_Manager.UpdateTiers(tier);
                            }
                            await UserDialogs.Instance.AlertAsync("Mise a jour avec succee", AppResources.alrt_msg_Alert, AppResources.alrt_msg_Ok);
                            UserDialogs.Instance.ShowLoading();

                        }
                        catch (Exception ex)
                        {
                            throw ex;
                        }
                        finally
                        {
                            UserDialogs.Instance.HideLoading();
                        }

                    }
                }
                else
                {
                    LocationSelector locationSelector = new LocationSelector(tier);
                    await PopupNavigation.Instance.PushAsync(locationSelector);
                    if (await locationSelector.PopupClosedTask)
                    {
                        SaveNewLocation(locationSelector.Result);
                    }
                }
            }
        }

        private async void RouteClicked(object sender, EventArgs e)
        {
            string destinationCordinates = HttpUtility.UrlEncode(tier.GPS_LATITUDE.ToString().Replace(",", ".") +
                                                           "," + tier.GPS_LONGITUDE.ToString().Replace(",", "."), Encoding.UTF8);
            Location location = null;
            try
            {
                UserDialogs.Instance.ShowLoading();
                location = await Geolocation.GetLocationAsync();
            }
            catch (Exception ex)
            {
                await UserDialogs.Instance.AlertAsync(ex.Message, AppResources.alrt_msg_Alert, AppResources.alrt_msg_Ok);

            }
            finally
            {
                UserDialogs.Instance.HideLoading();
            }

            if (location != null)
            {
                string startCordinates = HttpUtility.UrlEncode(location.Latitude.ToString().Replace(",", ".") +
                                         "," + location.Longitude.ToString().Replace(",", "."), Encoding.UTF8);
                if (Device.RuntimePlatform == Device.iOS)
                {
                    // https://developer.apple.com/library/ios/featuredarticles/iPhoneURLScheme_Reference/MapLinks/MapLinks.html
                    await Launcher.OpenAsync("http://maps.apple.com/?daddr=San+Francisco,+CA&saddr=cupertino");
                }
                else if (Device.RuntimePlatform == Device.Android)
                {
                    // opens the 'task chooser' so the user can pick Maps, Chrome or other mapping app
                    await Launcher.OpenAsync("http://maps.google.com/?saddr=" + startCordinates + "&daddr=" + destinationCordinates);
                }
            }
            else
            {
                await UserDialogs.Instance.AlertAsync("Activer la location et ressayer", AppResources.alrt_msg_Alert, AppResources.alrt_msg_Ok);
            }
        }

        private void Appele(object sender, EventArgs e)
        {

        }

        private void Info(object sender, EventArgs e)
        {

        }

        private async void VisitDemarer(object sender, EventArgs e)
        {
            viewModel.Item.CODE_ETAT = TourneeStatus.EnRoute;
            await CrudManager.TourneeDetails.UpdateItemAsync(viewModel.Item);
        }

        private void RouteClicked()
        {

        }

        /// <summary>
        /// Open and close the floating expandable button ...
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private async void OpenCloseButtons(object sender, EventArgs e)
        {
            try
            {
                await ((Frame)sender).ScaleTo(0.5, 50, Easing.Linear);
                await ((Frame)sender).ScaleTo(1, 50, Easing.Linear);

                if (isOpen)
                {
                    isOpen = !isOpen;
                    await closeIcon.RotateTo(0);
                    FloatMenuItem1.IsVisible = false;
                    await Task.Delay(70);
                    FloatMenuItem2.IsVisible = false;
                    await Task.Delay(70);
                    FloatMenuItem3.IsVisible = false;
                    await Task.Delay(70);
                    FloatMenuItem4.IsVisible = false;

                }
                else
                {
                    isOpen = !isOpen;
                    await closeIcon.RotateTo(-45);
                    FloatMenuItem1.IsVisible = true;
                    await Task.Delay(70);
                    FloatMenuItem2.IsVisible = true;
                    await Task.Delay(70);
                    FloatMenuItem3.IsVisible = true;
                    await Task.Delay(70);
                    FloatMenuItem4.IsVisible = true;
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        private async void AddNewCommand(object sender, EventArgs e)
        {
            try
            {
                await ((Frame)sender).ScaleTo(0.75, 50, Easing.Linear);
                await ((Frame)sender).ScaleTo(1, 50, Easing.Linear);
                await Navigation.PushAsync(new NavigationPage(new NewCommandePage(null, tier)));
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        private async void AddNewRetour(object sender, EventArgs e)
        {
            try
            {
                await ((Frame)sender).ScaleTo(0.75, 50, Easing.Linear);
                await ((Frame)sender).ScaleTo(1, 50, Easing.Linear);

                await Navigation.PushAsync(new VenteFormPage(null, "BR", tier));
            }
            catch (Exception ex)
            {

                throw ex;
            }
        }

        private async void AddNewVersement(object sender, EventArgs e)
        {
            try
            {
                await ((Frame)sender).ScaleTo(0.75, 50, Easing.Linear);
                await ((Frame)sender).ScaleTo(1, 50, Easing.Linear);

                await Navigation.PushAsync(new NewEncaissementPage(null, EncaissDisplayType.ENC, tier));
            }
            catch (Exception ex)
            {

                throw ex;
            }
        }


        private async void AddNewBl(object sender, EventArgs e)
        {
            try
            {
                await ((Frame)sender).ScaleTo(0.75, 50, Easing.Linear);
                await ((Frame)sender).ScaleTo(1, 50, Easing.Linear);

                await Navigation.PushAsync(new VenteFormLivraisonPage(null, "BL", tier, viewModel.Item.CODE_DETAIL));

            }
            catch (Exception ex)
            {
                throw ex;
            }

        }
    }
}