using System;
using System.Collections.Generic;
using System.Text;

namespace XpertMobileApp.Helpers
{
    public static class ServiceUrlDico
    {
        public static string BASE_URL = "api/";

        // Authentification url
        public static string TOKEN_URL = "token";

        // Authentification url
        public static string SESSION_INFO_URL = "session";

        public static string ENCAISSEMENT_URL = "Encaissements";

        public static string ENCAISSEMENT_PER_PAGE_URL = "GetEncaissements";

        public static string ADD_ENCAISSEMENT_URL = "addEncaissement";

        public static string UPDATE_ENCAISSEMENT_URL = "updateEncaissement";

        public static string DELETE_ENCAISSEMENT_URL = "deleteEncaissement";

        public static string MOTIFS_URL = "motifs";

        public static string COMPTES_URL = "comptes";

        public static string TIERS_URL = "tiers";

        public static string STATISTIC_URL = "statistic";

        public static string SESSION_URL = "session";

        public static string DASHBOARD_URL = "Dashboard";

        public static string MARGE_PAR_VENDEUR_URL = "MargeParVendeur";

        public static string ENCAISSEMENTS_COUNT = "GetEncaissementsCount";

    }

}
