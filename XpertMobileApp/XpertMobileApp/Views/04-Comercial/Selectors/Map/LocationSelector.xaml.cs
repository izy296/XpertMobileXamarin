using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Rg.Plugins.Popup.Pages;
using Rg.Plugins.Popup.Services;
using Xamarin.Essentials;
using Xamarin.Forms;
using Xamarin.Forms.GoogleMaps;
using XpertMobileApp.DAL;

namespace XpertMobileApp.Views
{
    public partial class LocationSelector : PopupPage
    {
        private bool pined = false;
        private View_TRS_TIERS tier;
        private Position clientPosition;
        private Position newClientPosition;

        private TaskCompletionSource<bool> taskCompletionSource;
        public Task<bool> PopupClosedTask { get { return taskCompletionSource.Task; } }

        public Position Result;
        public LocationSelector()
        {
            InitializeComponent();
            this.tier = null;
        }

        public LocationSelector(View_TRS_TIERS tier)
        {
            InitializeComponent();
            if (tier != null)
            {
                this.tier = tier;
                clientPosition = new Position(this.tier.GPS_LATITUDE, this.tier.GPS_LONGITUDE);
                if (this.tier.GPS_LATITUDE != 0 && this.tier.GPS_LONGITUDE != 0)
                {
                    Position position = new Position(this.tier.GPS_LATITUDE, this.tier.GPS_LONGITUDE);
                    MapSpan mapSpan = new MapSpan(position, 0.01, 0.01);
                    MyMap.MoveToRegion(mapSpan);

                    Pin pin = new Pin()
                    {
                        Address = tier.ADRESSE_TIERS,
                        Label = tier.NOM_TIERS,
                        Position = position,
                        IsDraggable = true,
                        Type = PinType.SavedPin
                    };


                    MyMap.Pins.Add(pin);
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
                Position position = new Position(location.Latitude, location.Longitude);
                MapSpan mapSpan = new MapSpan(position, 0.01, 0.01);
                MyMap.MoveToRegion(mapSpan);
                MyMap.MapClicked += OnMapClicked;
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

        private void OnMapClicked(object sender, MapClickedEventArgs e)
        {
            Position position = (Position)e.Point;
            if (position != newClientPosition)
            {
                newClientPosition = position;
            }
            if (tier == null)
            {
                Pin pin = new Pin()
                {
                    //Address = "Test",
                    Label = "Test",
                    Position = position,
                    IsDraggable = true,
                    Type = PinType.SavedPin
                };

                if (!pined)
                {
                    MyMap.Pins.Add(pin);
                    pined = true;
                }
                else
                {
                    if (MyMap.Pins[0].Position == position)
                    {
                        MyMap.Pins.RemoveAt(0);
                        pined = false;
                    }
                    else
                        MyMap.Pins[0].Position = position;
                }
            }
            else
            {
                Pin pin = new Pin()
                {
                    Address = tier.ADRESSE_TIERS,
                    Label = tier.NOM_TIERS,
                    Position = position,
                    IsDraggable = true,
                    Type = PinType.SavedPin
                };

                if (!pined)
                {
                    MyMap.Pins.Add(pin);
                    pined = true;
                }
                else
                {
                    if (MyMap.Pins[0].Position == position)
                    {
                        MyMap.Pins.RemoveAt(0);
                        pined = false;
                    }
                    else
                        MyMap.Pins[0].Position = position;
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
    }
}
