using XpertMobileApp.Services;
using Xamarin.Forms;
using System;
using System.Runtime.InteropServices;
using Foundation;

[assembly: Dependency(typeof(XpertMobileApp.IOS.Services.DeviceInfos))]
namespace XpertMobileApp.IOS.Services
{
    public class DeviceInfos : IDeviceInfos
    {
        [DllImport("/System/Library/Frameworks/IOKit.framework/IOKit")]
        private static extern uint IOServiceGetMatchingService(uint masterPort, IntPtr matching);

        [DllImport("/System/Library/Frameworks/IOKit.framework/IOKit")]
        private static extern IntPtr IOServiceMatching(string s);

        [DllImport("/System/Library/Frameworks/IOKit.framework/IOKit")]
        private static extern IntPtr IORegistryEntryCreateCFProperty(uint entry, IntPtr key, IntPtr allocator, uint options);

        [DllImport("/System/Library/Frameworks/IOKit.framework/IOKit")]
        private static extern int IOObjectRelease(uint o);


        public string GetDeviceId()
        {
            return GetSecureOsId();
        }

        public string GetImei()
        {
            throw new System.NotImplementedException();
        }

        public string GetSecureOsId()
        {
            string serial = string.Empty;
            uint platformExpert = IOServiceGetMatchingService(0, IOServiceMatching("IOPlatformExpertDevice"));
            if (platformExpert != 0)
            {
                NSString key = (NSString)"IOPlatformSerialNumber";
                IntPtr serialNumber = IORegistryEntryCreateCFProperty(platformExpert, key.Handle, IntPtr.Zero, 0);
                if (serialNumber != IntPtr.Zero)
                {
                    serial = NSString.FromHandle(serialNumber);
                }

                IOObjectRelease(platformExpert);
            }

            return serial;
        }

        public string GetSerial()
        {
            throw new System.NotImplementedException();
        }

        public string GetSimSerialNumber()
        {
            throw new System.NotImplementedException();
        }

        public string GetSubscriberId()
        {
            throw new System.NotImplementedException();
        }

        public bool HasPermission()
        {
            throw new System.NotImplementedException();
        }
    }
}