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
        public static string XAGRI_Mob = "XAGRI_Mob";
        public static string XCOM_Livraison = "XCOM_Livraison";
        public static string XCOM_Abattoir = "XCOM_Abattoir";
    }

    public static class Mobile_Edition
    {
        public static int Lite = 1;
        public static int Standard = 2;
        public static int Pro = 3;

        public static string GetEditionTitle(int CodeEdition)
        {
            //if (CodeEdition == 3)
            return "Edition PRO ";
            //else if (CodeEdition == 2)
            //  return "Edition STANDARD ";
            //else 
            //  return "Edition LITE ";
        }
    }

    public static class Constants
    {
        public static string LOCAL_DB_NAME = "XpertLocalDb.db3";

#if Debug
                public static string AppName = Apps.XAGRI_Mob;
#endif

#if XMBL
                public static string AppName = Apps.XCOM_Livraison;

#endif
#if XMAG
                public static string AppName = Apps.XAGRI_Mob;

#endif
#if XMAB
                public static string AppName = Apps.XCOM_Abattoir;

#endif
        //
        //public static string AppName = Apps.XAGRI_Mob;

        public static TimeSpan ImageCashValidityTimeSpan => new TimeSpan(0, 50, 0);

        public static bool DebugMode = false;

    }
}
