using System;
using Windows.Security.ExchangeActiveSyncProvisioning;
using Windows.System.Profile;
using Xamarin.Forms;
using XpertMobileApp.Services;
using XpertMobileApp.UWP.Services;

[assembly: Dependency(typeof(DeviceInfos))]
namespace XpertMobileApp.UWP.Services
{
    public class DeviceInfos : IDeviceInfos
    {
        public string GetDeviceId()
        {
            return DeviceInfo();
        }

        public string GetImei()
        {
            throw new NotImplementedException();
        }

        public string GetSecureOsId()
        {
            throw new NotImplementedException();
        }

        public string GetSerial()
        {
            throw new NotImplementedException();
        }

        public string GetSimSerialNumber()
        {
            throw new NotImplementedException();
        }

        public string GetSubscriberId()
        {
            throw new NotImplementedException();
        }

        public bool HasPermission()
        {
            throw new NotImplementedException();
        }


        private string DeviceInfo()
        {
            string Id = GetId();
            var deviceInformation = new EasClientDeviceInformation();
            string Model = deviceInformation.SystemProductName;
            string Manufracturer = deviceInformation.SystemManufacturer;
            string Name = deviceInformation.FriendlyName;
            string OSName = deviceInformation.OperatingSystem;

            return string.Format("{0}-{1}-{2}-{3}", Id, Model, Manufracturer, Name, OSName);
        }

        private static string GetId()
        {
            if (Windows.Foundation.Metadata.ApiInformation.IsTypePresent("Windows.System.Profile.HardwareIdentification"))
            {
                var token = HardwareIdentification.GetPackageSpecificToken(null);
                var hardwareId = token.Id;
                var dataReader = Windows.Storage.Streams.DataReader.FromBuffer(hardwareId);

                byte[] bytes = new byte[hardwareId.Length];
                dataReader.ReadBytes(bytes);

                return BitConverter.ToString(bytes).Replace("-", "");
            }

            throw new Exception("NO API FOR DEVICE ID PRESENT!");
        }

        public void RequestPermissions()
        {
            throw new NotImplementedException();
        }
    }
}
