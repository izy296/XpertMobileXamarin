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
        public static string XCOM_Mob = "XCOM_Mob"; //XpertMobile COM
        public static string XPH_Mob = "XCPH_Mob";  //XpertMobile OFFICINE
        public static string XAGRI_Mob = "XAGRI_Mob";  // XpertMobile AGRO
        public static string XACATALOG_Mob = "XACATALOG_Mob";  // XpertMobile CATALOG
        public static string XCOM_Vente = "XCOM_Vente";  // XpertMobile VENTE
        public static string XCOM_Livraison = "XCOM_Livraison";  // XpertMobile Livraison Manafiaa
        public static string X_BOUTIQUE = "X_BOUTIQUE";  //Boutique Web
        public static string X_DISTRIBUTION = "X_DIST"; //XpertMobile Distibution.
        public static string XM_D_PRICE = "X_DISPLAY_PRICE"; //XpertMobile Distibution.
    }

    public static class Mobile_Edition
    {
        public static int Lite = 1;
        public static int Standard = 2;
        public static int Pro = 3;

        public static string GetEditionTitle(int CodeEdition)
        {
            if (CodeEdition == 3)
                return "Edition PRO ";
            else if (CodeEdition == 2)
                return "Edition STANDARD ";
                        else
                return "Edition LITE ";
        }
    
    }

    public static class Constants
    {
        public static string LOCAL_DB_NAME = "XpertLocalDb.db3";

        public static string AppName = Apps.XPH_Mob;

        public static string AppFullName = GetAppFullName(AppName);

        private static string GetAppFullName(string appName)
        {
            if (AppName == "X_BOUTIQUE")
                return "Xpert BOUTIK";
#if XM_AGRO
            AppName = Apps.XAGRI_Mob;
            return "XpertMobile AGRO";
#endif

#if XM_CATALOG
            AppName = Apps.XACATALOG_Mob;
            return "XpertMobile CATALOG";
#endif

#if XM_LIVRAISON
            AppName = Apps.XCOM_Livraison;
            return "XpertMobile LIVRAISON";
#endif

#if XM_OFFICINE
            AppName = Apps.XPH_Mob;
            return "XpertMobile OFFICINE";
#endif

#if XM_VENTE
            AppName = Apps.XCOM_Vente;
            return "XpertMobile VENTE";
#endif

#if XM_COMM
            AppName = Apps.XCOM_Mob;
            return "XpertMobile COMM";
#endif
#if XM_DIST
            AppName = Apps.X_DISTRIBUTION;
            return "XpertMobile DISTRIBUTION";
#endif
#if XM_D_PRICE
            AppName = Apps.XM_D_PRICE;
            return "XpertMobile Afficheur De Prix";
#else
            AppName = Apps.XPH_Mob;
            return "XpertMobile OFFICINE";
#endif

        }
        public static TimeSpan ImageCashValidityTimeSpan => new TimeSpan(0, 50, 0);
#if release
        public static bool DebugMode = false;
#else
        public static bool DebugMode = true;
#endif

    }
}
