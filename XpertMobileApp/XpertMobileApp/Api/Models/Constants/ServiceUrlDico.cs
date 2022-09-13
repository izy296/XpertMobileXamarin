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

        public static string VIREMENT_URL = "TRS_VIREMENT";

        public static string ENCAISSEMENT_GET_PRINT_ENCAISSE = "GetPrintTiketCaisseEncaisse";

        public static string ENCAISSEMENT_PER_PAGE_URL = "GetEncaissements";

        public static string ADD_ENCAISSEMENT_URL = "addEncaissement";

        public static string UPDATE_ENCAISSEMENT_URL = "updateEncaissement";

        public static string DELETE_ENCAISSEMENT_URL = "deleteEncaissement";

        public static string MOTIFS_URL = "motifs";

        public static string COMPTES_URL = "comptes";

        public static string MODE_REG_URL = "GetModeReg";


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

        public static string WEBAPI_XML_URL = "http://xpertsoft.biz/XpertDecode/Update/XpertWebAPI/Production/XpertWebAPI.xml";

        // Activation

        public static string LICENCE_ACTIVATION_URL = "http://xpertsoft.noip.me/XpertDecodeWebAPI/api/prodactivationmobile/ActivateDeviceId";

        public static string LICENCE_ACTIVATION_URL_LOCAL = "http://192.168.171.193:99/XpertDecodeWebAPI/api/prodactivationmobile/ActivateDeviceId";

        public static string LICENCE_DEACTIVATION_URL = "http://xpertsoft.noip.me/XpertDecodeWebAPI/api/prodactivationmobile/DeActivateDeviceId";

        public static string New_Version_URL = "GetVersion";
        public static string Update_URL = "Update";
        public static string WebApiVersion = "WebApiVersion";

        // Printer

        public static string Print_Url = "PrintVente";

        // Ventes

        public static string VENTES_URL = "VTE_VENTE";

        public static string VENTES_DETAILS_URL = "GetVenteDetails";

        public static string VENTES_TYPES_URL = "GetVenteTypes";

        public static string STATUS_COMMANDE_URL = "GetStatus";

        public static string TIKET_CAISS_PRINT_ENCAISSE_URL = "GetPrintTiketCaisseEncaisse";

        public static string TIKET_CAISS_PRINT_VENTE_URL = "getPrintTiketCaisseVente";

        public static string VENTES_GET_VENTE_URL = "GetVente";

        // Commandes

        public static string VTE_COMMANDE = "VTE_COMMANDE";

        public static string COMMANDE_DETAILS_URL = "GetCommandesDetails";

        // Achats
        public static string ACH_ACHATS = "ACH_ACHATS";

        public static string ACHATS_DETAILS_URL = "GetAchatsDetails";
        public static string ACHATS_PESEE_URL = "GetPesee";
        public static string PRINTERS_LIST_URL = "GetPrintersList";
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

        public static string STK_PRODUITS_URL = "STK_PRODUITS";
        public static string STK_PRODUITS_CODE_BARRE_LOT = "GetByCodeBarreLot";
        public static string STK_PRODUITS_CODE_BARRE = "GetByCodeBarre";


        // Manquants
        public static string MANQUANTS_URL = "ACH_MANQUANTS";
        public static string MANQUANTS_TYPES_URL = "GetManquantsTypes";
        public static string Get_Qte_By_Produit = "GetQteStockByProduct";
        public static string Get_Qte_By_Reference = "GetQteStockByReference";
        public static string Find_Current_Non_CF_Manquants = "FindCurrent_Non_CF_Manquants";


        // Centres bordereaux 
        public static string BORDEREAUX_URL = "CFA_BORDEREAU";
        public static string BORDEREAUX_CENTRES_URL = "GetBordereauxCentres";
        public static string BORDEREAUX_ETAT_URL = "GetBordereauxStatus";

        // Centres bordereaux         
        public static string CFA_FACTURE_CHIFA_URL = "FACTURE_CHIFA";
        public static string CFA_FACTURE_ETAT_URL = "GetCFAFactsStatus";

        // RFID
        public static string RFID_URL = "RFID";
        public static string RFID_AddRFIDs_URL = "AddRfids";
        public static string RFID_GET_STOCK_FROM_IDSTOCK = "getStock";
        public static string RFID_GET_STOCK_FROM_RFIDs = "getStockFromRFIDs";

        public static string RFID_GET_CURENT_INV = "getcurentinv";
        public static string RFID_UPDATE_CURENT_INV = "updatecurentinv";

        public static string RFID_GET_STOCK_COED_BARRE = "getstockcodebarre";

        //Sortie
        public static string SORTIE_URL = "STK_SORTIE";
        public static string SORTIE_GET_STOCK_SORTIE = "GetDetailsByCode";
        public static string SORTIE_GET_STOCK_MOTIFS = "GetMotifs";
        public static string SORTIE_GET_STOCK_TYPES = "GetSortieTypes";

        //User
        public static string USER_URL = "SYS_USER";
        public static string USER_GET_IDS = "GetUsersID";

        //
        public static string SORTIE_TYPE_URL = "BSE_SORTIE_TYPE";
        public static string SORTIE_GET_TYPE = "GetDesignationType";

        //BSE_PRODUIT_TAG
        public static string PRODUIT_TAG_URL = "BSE_PRODUIT_TAG";
        public static string PRODUIT_GET_TAG_URL = "SelectAll";

        //BSE_PRODUIT_TAG
        public static string PRODUIT_LABO_URL = "BSE_PRODUIT_LABO";
        public static string PRODUIT_GET_LABO_URL = "SelectAll";

        //BSE_PRODUIT_UNITE
        public static string PRODUIT_UNITE_URL = "BSE_PRODUIT_UNITE";
        public static string PRODUIT_GET_UNITE_URL = "SelectAll";

        //ECHANGE
        public static string ECHANGE_URL = "STK_ECHANGE";
        public static string ECHANGE_GET_MOTIFS = "GetMotif";
        public static string ECHANGE_GET_TYPE_TIERS = "GetTiersType";
        public static string ECHANGE_GET_TIERS = "GetListTiers";
        public static string ECHANGE_GET_DETAIL = "GetEchangeDetail";
        public static string ECHANGE_GET_MAGASIN = "GetMagasin";
        //RotationDesProduits
        public static string ROTATION_URL = "RotationDesProduits";
        public static string ROTATION_SELECT_PRODUCT = "SelectProduct";


    }

}
