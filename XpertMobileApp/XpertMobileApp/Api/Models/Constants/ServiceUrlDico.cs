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

        //Tiers
        public static string TIERS_URL = "TRS_TIERS";

        public static string TIERS_FAMILLES_URL = "GetTiersFamilles";

        public static string TIERS_TYPES_URL = "GetTiersTypes";

        public static string TIERS_GET_LIST_URL = "GetTiers";

        public static string STATISTIC_URL = "statistic";

        public static string SESSION_URL = "session";

        public static string DASHBOARD_URL = "Dashboard";

        public static string MARGE_PAR_VENDEUR_URL = "MarginByUser";

        public static string ACHAT_AGRO_INFOS_URL = "GetAchatsInfos";

        public static string TOTAL_MARGE_PAR_VENDEUR_URL = "GetTotalMargin";

        public static string ENCAISSEMENTS_COUNT = "GetEncaissementsCount";

        public static string TRS_TIER_GET_TIER = "GetTier";

        public static string WEBAPI_XML_URL = "http://xpertsoft.biz/XpertDecode/Update/XpertWebAPI/Production/XpertWebAPI.xml";
        public static string XOFFICINE_XML_URL = "http://xpertsoft.biz/XpertDecode/Update/XpertWebAPI/Production/XpertMobileOFFICINE.xml";
        public static string XCOMM_XML_URL = "http://xpertsoft.biz/XpertDecode/Update/XpertWebAPI/Production/XpertMobileCOMM.xml";
        public static string XDISTRIB_XML_URL = "http://xpertsoft.biz/XpertDecode/Update/XpertWebAPI/Production/XpertMobileDISTRIB.xml";

        // Activation

        public static string LICENCE_ACTIVATION_URL = "http://xpertsoft.noip.me/XpertDecodeWebAPI/api/prodactivationmobile/ActivateDeviceId";

        public static string LICENCE_ACTIVATION_URL_LOCAL = "http://192.168.171.193:99/XpertDecodeWebAPI/api/prodactivationmobile/ActivateDeviceId";

        public static string LICENCE_DEACTIVATION_URL = "http://xpertsoft.noip.me/XpertDecodeWebAPI/api/prodactivationmobile/DeActivateDeviceId";

        public static string New_Version_URL = "GetVersion";
        public static string Update_URL = "Update";
        public static string WebApiVersion = "WebApiVersion";
        public static string WebApiSelfHosted = "IsSelfHosted";
        public static string TunnelUrl = "http://xpertsoft.biz/XpertDecodeWebAPI/api/ProdActivationMobile/GetMobileUrl";

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
        public static string COMMANDE_URL = "GetCommande";

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

        public static string PRODUITS_UNITE_MESURE_URL_XCOM = "BSE_PRODUIT_AUTRE_UNITE_XCOM";
        public static string PRODUITS_UNITE_MESURE_URL = "BSE_PRODUIT_AUTRE_UNITE";
        public static string PRODUIT_GET_UNITE_MESURE = "GetAutreUniteByProduit";
        public static string PRODUITS_URL = "Produits";

        public static string PRODUITS_DETAILS_URL = "GetProduitDetails";

        public static string PRODUITS_REF_DETAILS_URL = "GetProduitRefDetails";

        public static string PRODUITS_FAMILLES_URL = "GetProduitsFamilles";

        public static string PRODUITS_TYPES_URL = "GetProduitsTypes";

        public static string PRODUITS_LOTS_URL = "GetLots";

        public static string PRODUITS_LOTS_REF_URL = "GetLotsFromRef";

        public static string STK_PRODUITS_URL = "STK_PRODUITS";
        public static string STK_PRODUITS_COM_URL = "STK_PRODUITS_XCOM";
        public static string STK_PRODUITS_CODE_BARRE_LOT = "GetByCodeBarreLot";
        public static string STK_PRODUITS_CODE_BARRE = "GetByCodeBarre";
        public static string ALL_PRODUCTS = "GetAllProduct";

        // Manquants
        public static string MANQUANTS_URL = "ACH_MANQUANTS";
        public static string MANQUANTS_TYPES_URL = "GetManquantsTypes";
        public static string Get_Qte_By_Produit = "GetQteStockByProduct";
        public static string Get_Qte_By_Reference = "GetQteStockByReference";
        public static string Find_Current_Non_CF_Manquants = "FindCurrent_Non_CF_Manquants";


        // Centres Suivi bordereaux 
        public static string BORDEREAUX_URL = "CFA_BORDEREAU";
        public static string BORDEREAUX_CENTRES_URL = "GetBordereauxCentres";
        public static string BORDEREAUX_ETAT_URL = "GetBordereauxStatus";

        public static string CFA_MOBILE_FACTURE_URL = "CFA_MOBILE_FACTURE";
        public static string CFA_MOBILE_FACTURE_SUMMARY_URL = "GetFactsSummary";
        public static string CFA_MOBILE_FACTURE_CHRONIC_URL = "GetFactChronic";
        public static string CFA_MOBILE_FACTURE_BENEFICIARE_URL = "SelectBeneficiares";
        public static string CFA_MOBILE_FACTURE_BENEFICIARE_COUNT_URL = "SelectBeneficiaresCount";


        // Centres bordereaux CHIFA
        public static string BORDEREAUX_CHIFA_URL = "CFA_BORDEREAUX_CHIFA";
        public static string BORDEREAUX_CHIFA_BY_NUM_URL = "GetCFAByNumBordereaux";
        public static string BORDEREAUX_CHIFA_SELECT_URL = "SelectBordereaux";
        public static string BORDEREAUX_CHIFA_SELECT_COUNT_URL = "SelectBordereauxCount";

        // Centres bordereaux         
        public static string CFA_FACTURE_CHIFA_URL = "FACTURE_CHIFA";
        public static string CFA_FACTURE_ETAT_URL = "GetCFAFactsStatus";
        public static string CFA_FACTURE_BORDEREAUX_URL = "GetCFAFactsByNumBordereaux";

        //Date des factures CHIFA
        public static string CFA_FACTURE_DETAIL_MOBILE_URL = "CFA_MOBILE_DETAIL_FACTURE";
        public static string CFA_FACTURE_DATE = "GetDateFactures";
        public static string CFA_FACTURE_COUNT_TODAY = "GetTodayCountFacture";
        public static string CFA_LABO_COUNT = "GetCountLabo";
        public static string CFA_FACTURE_ANALYSE= "AnalyseFactures";


        //Details facture chifa 
        public static string CFA_DETAIL_FACTURE_URL = "CFA_MOBILE_DETAIL_FACTURE";
        public static string CFA_DETAIL_SELECT_PATIENTS_URL = "SelectPatients";
        public static string CFA_DETAIL_SELECT_TRAITMENT_URL = "SelectTraitment";
        public static string CFA_DETAIL_SELECT_MEDICACTIONS_URL = "SelectMedications";
        public static string CFA_DETAIL_SELECT_PATIENTS_BY_MEDICATION_URL = "SelectPatientByMedication";
        public static string CFA_DETAIL_SELECT_BENEFICIARE_URL = "SelectBeneficiare";
        public static string CFA_DETAIL_SELECT_FACTS_URL = "SelectFactsByBeneficiare";
        public static string CFA_DETAIL_SELECT_FACT_URL = "SelectFactures";
        public static string CFA_DETAIL_SELECT_RELATIVES_URL = "SelectBeneficiareRelatives";
        public static string CFA_DETAIL_FACTURE = "GetCFAFactDetails";
        public static string CFA_TOTAUX_FACTURE = "GetTotauxFactureCHIFA";
        public static string CFA_LISTE_FACT_BY_DCI = "GetListFactByDci";
        public static string CFA_LISTE_CONSOMMATION_BY_DCI = "GetListFactDetailByDci";
        public static string CFA_LISTE_Facture_BY_REF = "GetFactureListByReference";
        public static string CFA_LISTE_BeneficiaireByDci = "GetBeneficiaireByDci";

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

        //TransfertStock
        public static string TRANSFERT_STOCK_URL = "STK_TRANSFERT";
        public static string TRANSFERT_STOCK_SELECT_TRANSFERT = "SelectTransfer";

        public static string TRANSFERT_STOCK_DETAIL_URL = "STK_TRANSFERT_DETAIL";
        public static string TRANSFERT_STOCK_GET_DETAIL= "GetTransferProducts";
        public static string TRANSFERT_STOCK_ADD = "InsertTransfert";

        //Inventaire
        public static string STK_INVENTAIRE_URL = "STK_INVENTAIRE";
        public static string STK_INVENTAIRE_OPEN = "GetOpenInventory";

        public static string STK_INVENTAIRE_DETAIL_URL = "STK_INVENTAIRE_DETAIL";
        public static string STK_INVENTAIRE_DETAIL_GET_LOT_INFO = "GetCodeBarreLotInventaireInfo";
        public static string STK_INVENTAIRE_DETAIL_UPDATE = "UpdateInventoryAndPlacement";
        
        public static string STK_AJUSTEMENT_DETAIL_URL = "STK_AJUSTEMENT_DETAIL";
        public static string STK_AJUSTEMENT_DETAIL_UPDATE_URL = "StockAdjustement";

        public static string STK_STOCK_URL = "STK_STOCK";
        public static string STK_STOCK_GET_LOT_INFO = "GetLotByCodeBarreAndMagasin";
    }

}
