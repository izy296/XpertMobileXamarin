using Android.App;
using Android.Content.PM;
using Java.IO;
using Java.Lang;
using System;
using Xamarin.Forms;

[assembly: Dependency(typeof(XpertMobileApp.Droid.Services.OrientationService))]
namespace XpertMobileApp.Droid.Services
{
    public class OrientationService : XpertMobileApp.Services.IOrientaionService
    {

        public void Landscape()
        {
            ((Activity)Forms.Context).RequestedOrientation = ScreenOrientation.Landscape;
        }

        public void Portrait()
        {
            ((Activity)Forms.Context).RequestedOrientation = ScreenOrientation.Portrait;
        }

        public void ReverseLandscape()
        {
            ((Activity)Forms.Context).RequestedOrientation = ScreenOrientation.ReverseLandscape;
        }

        public void ReversePortrait()
        {
            ((Activity)Forms.Context).RequestedOrientation = ScreenOrientation.ReversePortrait;
        }
    }
}