using Android.App;
using Android.Content.PM;
using Android.Views;
using Java.IO;
using Java.Lang;
using System;
using Xamarin.Forms;

[assembly: Dependency(typeof(XpertMobileApp.Droid.Services.StatusBarService))]
namespace XpertMobileApp.Droid.Services
{
    public class StatusBarService : XpertMobileApp.Services.IStatusBar
    {
        WindowManagerFlags _originalFlags;
        public void HideStatusBar()
        {
            var activity = (Activity)Forms.Context;
            var attrs = activity.Window.Attributes;
            _originalFlags = attrs.Flags;
            attrs.Flags |= Android.Views.WindowManagerFlags.Fullscreen;
            activity.Window.Attributes = attrs;
        }

        public void ShowStatusBar()
        {
            var activity = (Activity)Forms.Context;
            var attrs = activity.Window.Attributes;
            attrs.Flags = _originalFlags;
            activity.Window.Attributes = attrs;
        }
    }
}