using System;
using System.Collections.Generic;
using System.Text;

namespace XpertMobileApp.Api
{
    public static class Apps
    {
        public static string XCOM_Mob = "XCOM_Mob";
        public static string XAGRI_Mob = "XAGRI_Mob";
    }

    public static class Constants
    {
        public static string LOCAL_DB_NAME = "XpertLocalDb.db3";

        public static string AppName = Apps.XCOM_Mob;

        public static TimeSpan ImageCashValidityTimeSpan => new TimeSpan(0, 50, 0);
    }
}
