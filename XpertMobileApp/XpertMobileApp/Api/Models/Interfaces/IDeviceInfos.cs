using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace XpertMobileApp.Services
{
    public interface IDeviceInfos
    {
        bool HasPermission();

        void RequestPermissions();

        string GetSecureOsId();
    }
}
