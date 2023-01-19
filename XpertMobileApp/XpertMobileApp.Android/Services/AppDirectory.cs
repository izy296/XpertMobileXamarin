using Java.Lang;
using System;
using Xamarin.Forms;

[assembly: Dependency(typeof(XpertMobileApp.Droid.Services.AppDirectory))]
namespace XpertMobileApp.Droid.Services
{
    public class AppDirectory : XpertMobileApp.Services.IAppDirectory
    {
        public string GetPublicDirectroy()
        {
            var res = Android.OS.Environment.ExternalStorageDirectory.AbsolutePath;
            return res;
        }
    }
}