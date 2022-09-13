using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace XpertMobileApp.CustomControls
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class ExpandableView : ContentView
    {

        private TapGestureRecognizer _tapRecogniser;
        private StackLayout _summary;
        private StackLayout _details;
        private bool animating = false;
        private bool firstTime = false;
        

        public ExpandableView()
        {
            InitializeComponent();
            InitializeGuestureRecognizer();
            SubscribeToGuestureHandler();
        }

        private void InitializeGuestureRecognizer()
        {
            _tapRecogniser = new TapGestureRecognizer();
            SummaryRegion.GestureRecognizers.Add(_tapRecogniser);
            Arrow.GestureRecognizers.Add(_tapRecogniser);
        }

        private void SubscribeToGuestureHandler()
        {
            _tapRecogniser.Tapped += TapRecogniser_Tapped;
        }

        public virtual StackLayout Summary
        {
            get { return _summary; }
            set
            {
                _summary = value;
                SummaryRegion.Children.Add(_summary);
                OnPropertyChanged();
            }
        }

        public virtual StackLayout Details
        {
            get { return _details; }
            set
            {
                _details = value;
                DetailsRegion.Children.Add(_details);
                OnPropertyChanged();
            }
        }

        public void changeVisible()
        {
            if (DetailsRegion.IsVisible)
            {
                DetailsRegion.IsVisible = false;
                Arrow.RotateTo(360,180, easing: Easing.Linear);
            }
            else
            {
                DetailsRegion.IsVisible = true;
                Arrow.RotateTo(180, 180, easing: Easing.Linear);
            }
        }

        private void TapRecogniser_Tapped(object sender, EventArgs e)
        {
            if (animating)
                return;

            var wait = 200;
            if (DetailsRegion.IsVisible)
            {
                animating= true;
                var animate = new Animation(d => Layout.HeightRequest = d, SummaryRegion.Height + DetailsRegion.Height, SummaryRegion.Height, Easing.CubicInOut);
                animate.Commit(Layout, "BarGraph", 16, (uint)wait);
                Task.Run(() =>
                {
                    System.Threading.Thread.Sleep(wait);
                    Device.BeginInvokeOnMainThread(() =>
                    {
                        changeVisible();
                        animating = false;
                    });
                });

            }
            else
            {
                animating = true;
                Task.Run(() =>
                {
                    Device.BeginInvokeOnMainThread(() =>
                    {
                        changeVisible();
                    });
                });
                var animate = new Animation(d => Layout.HeightRequest = d, SummaryRegion.Height, SummaryRegion.Height + DetailsRegion.Height, Easing.CubicInOut);
                animate.Commit(Layout, "BarGraph", 16, (uint)wait);
                if (DetailsRegion.Height==-1)
                {
                    Task.Run(() =>
                    {
                        System.Threading.Thread.Sleep(wait);
                        Device.BeginInvokeOnMainThread(() =>
                        {
                            animate = new Animation(d => Layout.HeightRequest = d, SummaryRegion.Height, SummaryRegion.Height + DetailsRegion.Height, Easing.CubicInOut);
                            animate.Commit(Layout, "BarGraph", 16, (uint)wait);
                        });
                    });
                }
                animating = false;
            }
        }
    }
}