using System;
using System.IO;
using UIKit;
using Xamarin.Forms;
using XpertMobileApp.Data;
using XpertMobileApp.Services;

[assembly: Dependency(typeof(XpertMobileApp.iOS.Services.DeviceInfos))]
namespace XpertMobileApp.iOS.Services
{
    public class DeviceInfos : IDeviceInfos
    {
        public string GetSecureOsId()
        {
            return UIDevice.CurrentDevice.IdentifierForVendor.AsString();
        }

        public bool HasPermission()
        {
            return true;
        }

        public void RequestPermissions()
        {
            throw new NotImplementedException();
        }
    }
}