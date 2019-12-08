using System;
using System.Collections.Generic;
using System.Text;
using Xpert.Common.WSClient.Model;
using Xpert.Common.WSClient.Services;
using XpertMobileApp.DAL;

namespace XpertMobileApp.Api
{
    public static class Apps
    {
        public static string XCOM_Mob = "XCOM_Mob";
        public static string XPH_Mob = "XCPH_Mob";
        public static string XAGRI_Mob = "XAGRI_Mob";
        public static string XACATALOG_Mob = "XACATALOG_Mob";
    }

    public static class Constants
    {
        public static string LOCAL_DB_NAME = "XpertLocalDb.db3";

        public static string AppName = Apps.XPH_Mob;

        public static TimeSpan ImageCashValidityTimeSpan => new TimeSpan(0, 50, 0);

    }
}
