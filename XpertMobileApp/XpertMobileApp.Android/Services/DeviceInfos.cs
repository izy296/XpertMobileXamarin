using Android.OS;
using Android.Telephony;
using Xamarin.Forms;
using static Android.Provider.Settings;
using Android;
using Android.Content.PM;
using Android.Support.V4.App;
using Android.Util;
using Exception = Java.Lang.Exception;

[assembly: Dependency(typeof(XpertMobileApp.Droid.Services.DeviceInfos))]
namespace XpertMobileApp.Droid.Services
{
    public class DeviceInfos : XpertMobileApp.Services.IDeviceInfos
    {
        TelephonyManager tm = (Android.Telephony.TelephonyManager) MainActivity.Instance.GetSystemService(Android.Content.Context.TelephonyService);

        public bool HasPermission()
        {
            return ActivityCompat.CheckSelfPermission(MainActivity.Instance, Manifest.Permission.ReadPhoneState) != (int)Permission.Granted;
        }

        public string GetImei()
        {
            try
            {
                return tm.Imei;
            }
            catch(Exception e)
            {
                return e.Message;
            }
 
        }

        public string GetSubscriberId()
        {
            try
            {
                return tm.SubscriberId;
            }
            catch (Exception e)
            {
                return e.Message;
            }

        }

        public string GetSerial()
        {
            try
            {            
                return Build.Serial;
            }
            catch (Exception e)
            {
                return e.Message;
            }
        }

        public string GetSimSerialNumber()
        {
            try
            {
                return tm.SimSerialNumber;
            }
            catch (Exception e)
            {
                return e.Message;
            }
        }

        public string GetDeviceId()
        {
            string deviceId ="-";


                // Camera permissions is already available, show the camera preview.
                Log.Info("DeviceInfos", "CAMERA permission has already been granted. Displaying camera preview.");

                try
                {

                    if (Build.VERSION.SdkInt >= Android.OS.BuildVersionCodes.O)
                    {
                        deviceId = tm.Imei;
                    }
                    else
                    {
                        deviceId = tm.DeviceId;
                    }

                    if (deviceId != null)
                    {
                        return deviceId;
                    }
                    else
                    {
                        deviceId =  Android.OS.Build.Serial;
                    }
                }
                catch (Exception e)
                {
                    deviceId = e.Message;
                }
            return deviceId;
        }

        public string GetSecureOsId()
        {
            try
            { 
                return  Secure.GetString(MainActivity.Instance.ContentResolver, Secure.AndroidId);
            }
            catch (Exception e)
            {
                return e.Message;
            }
        }

    }
}