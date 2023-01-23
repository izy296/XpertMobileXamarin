using Android.App;
using Android.Content;
using Android.OS;
using Android.Runtime;
using Android.Views;
using Android.Widget;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using Xamarin.Forms;
using XpertMobileApp.Droid;
using XpertMobileApp.Droid.Services;
using XpertMobileAppManafiaa.Api.Models.Interfaces;

[assembly: Xamarin.Forms.Dependency(typeof(XpertMobileApp.Droid.Services.StartSettings_Implementation))]
namespace XpertMobileApp.Droid.Services
{
    public class StartSettings_Implementation : ISettingsStart
    {
        public void StartSettings()
        {
            var intent = new Intent(Android.Provider.Settings.ActionWifiSettings);
            Forms.Context.StartActivity(intent);
        }
    }
}