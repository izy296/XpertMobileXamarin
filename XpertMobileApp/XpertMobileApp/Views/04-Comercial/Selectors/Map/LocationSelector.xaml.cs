using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Rg.Plugins.Popup.Pages;
using Rg.Plugins.Popup.Services;
using Syncfusion.SfMaps.XForms;
using Xamarin.Essentials;
using Xamarin.Forms;
using Xamarin.Forms.GoogleMaps;
using XpertMobileApp.DAL;

namespace XpertMobileApp.Views
{
    public partial class LocationSelector : PopupPage
    {
        private bool pined = false;
        public View_TRS_TIERS tier;
        private Point clientPosition;
        private Point newClientPosition;

        private TaskCompletionSource<bool> taskCompletionSource;
        public Task<bool> PopupClosedTask { get { return taskCompletionSource.Task; } }

        public Point Result;
        public LocationSelector()
        {
            InitializeComponent();
            this.tier = null;
            BindingContext = this;
        }

        public LocationSelector(View_TRS_TIERS tier)
        {
            InitializeComponent();
            if (tier != null)
            {
                this.tier = tier;
                clientPosition = new Point(this.tier.GPS_LATITUDE, this.tier.GPS_LONGITUDE);
                if (this.tier.GPS_LATITUDE != 0 && this.tier.GPS_LONGITUDE != 0)
                {
                    //((ImageryLayer)MyMap.Layers[0]).GeoCoordinates = ((ImageryLayer)MyMap.Layers[0]).GetLatLonFromPoint(clientPosition);

                    MapMarker pin = new MapMarker()
                    {
                        Label = tier.NOM_TIERS,
                        Longitude= this.tier.GPS_LONGITUDE.ToString(),
                        Latitude = this.tier.GPS_LATITUDE.ToString(),
                    };


                    MyMap.Layers[0].Markers.Add(pin);
                    pined = true;
                }

            }

        }

        protected async override void OnAppearing()
        {
            base.OnAppearing();

            taskCompletionSource = new TaskCompletionSource<bool>();

            var location = await Geolocation.GetLocationAsync();

            if (location != null)
            {
                Console.WriteLine($"Latitude: {location.Latitude}, Longitude: {location.Longitude}, Altitude: {location.Altitude}");
                Point position = new Point(location.Latitude, location.Longitude);
                MyMap.Layers[0].GeopointToViewPoint(location.Latitude, location.Longitude);
            }
            else
            {
                if (tier == null || (tier.GPS_LATITUDE == 0 && tier.GPS_LONGITUDE == 0))
                {
                    await DisplayAlert(AppResources.txt_alert_message, "Le client n'a pas de position GPS", AppResources.alrt_msg_Ok);
                }
                await DisplayAlert(AppResources.txt_alert_message, "Localisation descativer", AppResources.alrt_msg_Ok);
            }

        }

        private void OnMapClicked(object sender, MapTappedEventArgs e)
        {
            Point position = e.Position;
            if (position != newClientPosition)
            {
                newClientPosition = MyMap.Layers[0].GetLatLonFromPoint(position);
            }
            if (tier == null)
            {
                MapMarker pin = new MapMarker()
                {
                    //Address = "Test",
                    Label = "Location",
                    Longitude = MyMap.Layers[0].GetLatLonFromPoint(position).Y.ToString(),
                    Latitude = MyMap.Layers[0].GetLatLonFromPoint(position).X.ToString(),
                };

                if (!pined)
                {
                    MyMap.Layers[0].Markers.Add(pin);
                    pined = true;
                }
                else
                {
                    if (new Point(double.Parse(MyMap.Layers[0].Markers[0].Latitude),
                        double.Parse(MyMap.Layers[0].Markers[0].Longitude)) == position)
                    {
                        MyMap.Layers[0].Markers.RemoveAt(0);
                        pined = false;
                    }
                    else
                    {
                        MyMap.Layers[0].Markers[0].Latitude = MyMap.Layers[0].GetLatLonFromPoint(position).Y.ToString();
                        MyMap.Layers[0].Markers[0].Longitude = MyMap.Layers[0].GetLatLonFromPoint(position).X.ToString();
                    }

                }
            }
            else
            {
                MapMarker pin = new MapMarker()
                {
                    //Address = "Test",
                    Label = tier.FULL_NOM_TIERS,
                    Longitude = MyMap.Layers[0].GetLatLonFromPoint(position).Y.ToString(),
                    Latitude = MyMap.Layers[0].GetLatLonFromPoint(position).X.ToString(),
                };

                if (!pined)
                {
                    MyMap.Layers[0].Markers.Add(pin);
                    pined = true;
                }
                else
                {
                    if (new Point(double.Parse(MyMap.Layers[0].Markers[0].Latitude),
                        double.Parse(MyMap.Layers[0].Markers[0].Longitude)) == MyMap.Layers[0].GetLatLonFromPoint(position))
                    {
                        MyMap.Layers[0].Markers.RemoveAt(0);
                        pined = false;
                    }
                    else
                    {
                        MyMap.Layers[0].Markers[0].Latitude = MyMap.Layers[0].GetLatLonFromPoint(position).Y.ToString();
                        MyMap.Layers[0].Markers[0].Longitude = MyMap.Layers[0].GetLatLonFromPoint(position).X.ToString();

                    }
                }
            }
            if (newClientPosition != clientPosition && newClientPosition != null)
            {
                ButtonsPannel.IsVisible = true;
            }

        }

        private async void SaveLocation_Clicked(object sender, EventArgs e)
        {
            if (newClientPosition != clientPosition && newClientPosition != null)
            {
                Result = newClientPosition;
            }
            var result = await this.DisplayAlert(AppResources.txt_alert_message, "Do you really want to Save?", AppResources.exit_Button_Yes, AppResources.exit_Button_No);
            if (result)
            {
                taskCompletionSource.SetResult(true);
                await PopupNavigation.Instance.PopAsync();
            }

        }

        private async void CancelLocation_Clicked(object sender, EventArgs e)
        {
            await PopupNavigation.Instance.PopAsync();
        }

        private void SfMaps_Tapped(object sender, MapTappedEventArgs e)
        {

        }
    }
}
