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


        public static string TIERS_URL = "TRS_TIERS";

        public static string TIERS_FAMILLES_URL = "GetTiersFamilles";

        public static string TIERS_TYPES_URL = "GetTiersTypes";


        public static string STATISTIC_URL = "statistic";

        public static string SESSION_URL = "session";

        public static string DASHBOARD_URL = "Dashboard";

        public static string MARGE_PAR_VENDEUR_URL = "MarginByUser";

        public static string ACHAT_AGRO_INFOS_URL = "GetAchatsInfos";

        public static string TOTAL_MARGE_PAR_VENDEUR_URL = "GetTotalMargin";
        
        public static string ENCAISSEMENTS_COUNT = "GetEncaissementsCount";

        // Activation

        public static string LICENCE_ACTIVATION_URL = "http://xpertsoft.biz/XpertDecodeWebAPI/api/prodactivationmobile/ActivateDeviceId";

        public static string LICENCE_DEACTIVATION_URL = "http://xpertsoft.biz/XpertDecodeWebAPI/api/prodactivationmobile/DeActivateDeviceId";


        // Ventes

        public static string VENTES_URL = "VTE_VENTE";

        public static string VENTES_DETAILS_URL = "GetVenteDetails";

        public static string VENTES_TYPES_URL = "GetVenteTypes";

        // Commandes

        public static string VTE_COMMANDE = "VTE_COMMANDE";

        public static string COMMANDE_DETAILS_URL = "GetCommandesDetails";

        // Achats
        public static string ACH_ACHATS = "ACH_ACHATS";

        public static string ACHATS_DETAILS_URL = "GetAchatsDetails";
        public static string ACHATS_PESEE_URL = "GetPesee";
        public static string ACHATS_IMMATRICULATIONS_URL = "GetImmatriculations";

        // Production


        public static string ACH_PRODUCTION = "PRD_AGRICULTURE";
        public static string PRODUCTION_DETAILS_URL = "GetDetails";
        public static string PRODUCTION_GENERATE_PRODUCTION_URL = "GenerateProductionOrder";
        public static string PRODUCTION_SAVE_QTE_PRODUITE_URL = "SaveQteProduite";
        public static string PRODUCTION_UPDATE_STATUS_URL = "UpdateStatus";
        public static string PRODUCTION_SAVE_EMBALLAGES_URL = "SaveDetailsEmbalages";
        public static string PRODUCTION_PRINT_QR_CODE_URL = "PrintQRCodeRecept";
        public static string PRODUCTION_UPDATE_LIVRAISON_INFOS_URL = "SetInfoLivraison";

        // Produits

        public static string PRODUITS_URL = "Produits";

        public static string PRODUITS_DETAILS_URL = "GetProduitDetails";

        public static string PRODUITS_REF_DETAILS_URL = "GetProduitRefDetails";

        public static string PRODUITS_FAMILLES_URL = "GetProduitsFamilles";

        public static string PRODUITS_TYPES_URL = "GetProduitsTypes";

        public static string PRODUITS_LOTS_URL = "GetLots";

        public static string PRODUITS_LOTS_REF_URL = "GetLotsFromRef";

        // Manquants
        public static string MANQUANTS_URL = "ACH_MANQUANTS";
        public static string MANQUANTS_TYPES_URL = "GetManquantsTypes";

        // Centres bordereaux 
        public static string BORDEREAUX_URL = "CFA_BORDEREAU";
        public static string BORDEREAUX_CENTRES_URL = "GetBordereauxCentres";
        public static string BORDEREAUX_ETAT_URL = "GetBordereauxStatus";

        // RFID
        public static string RFID_URL = "RFID";
        public static string RFID_AddRFIDs_URL = "AddRfids";
        public static string RFID_GET_STOCK_FROM_IDSTOCK = "getStock";
        public static string RFID_GET_STOCK_FROM_RFIDs = "getStockFromRFIDs";
        
        public static string RFID_GET_CURENT_INV = "getcurentinv";
        public static string RFID_UPDATE_CURENT_INV = "updatecurentinv";

        public static string RFID_GET_STOCK_COED_BARRE = "getstockcodebarre";

    }

}
