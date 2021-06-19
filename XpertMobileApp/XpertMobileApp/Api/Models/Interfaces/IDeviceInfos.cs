using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace XpertMobileApp.Services
{
    public interface IDeviceInfos
    {
        bool HasPermission();

        void RequestPermissions();

        string GetImei();

        string GetSubscriberId();

        string GetSerial();

        string GetSimSerialNumber();

        string GetDeviceId();

        string GetSecureOsId();
    }
}
