using Android.App;
using Android.Content;
using Android.OS;
using Android.Runtime;
using Android.Views;
using Android.Widget;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Xamarin.Forms;
using XpertMobileApp.Api.Models.Interfaces;

[assembly: Xamarin.Forms.Dependency(typeof(XpertMobileApp.Droid.Services.StartSettings))]
namespace XpertMobileApp.Droid.Services
{
    public class StartSettings : ISettingsStart
    {
        void ISettingsStart.StartSettings()
        {
            var intent = new Intent(Android.Provider.Settings.ActionWifiSettings);
            Forms.Context.StartActivity(intent);
        }
    }
}