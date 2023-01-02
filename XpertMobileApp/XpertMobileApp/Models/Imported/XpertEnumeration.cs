namespace Xpert
{
    // Pour selection mode insert dans methode saveData dans mainBusiness
    public enum FLAG_INSERT { INSERT, INSERT_IGNORE, INSERT_REPLACE }


    // pour sélectionner le chams a modifier dans la liste des produit 
    // le code labo ou le code famille 
    public enum TypeModifProduit
    {
        ModifFamille,
        ModifLabo,
        ModifType
    }
    public enum TypeVenteTiersLookUP
    {
        FACTURATION,
        VC,
        FACTURATION_VC
    }
    public enum APPLICATION_NAME
    {
        NONE, XAS, XPH, XCM, XMOB, XPO, XCVM, XPOS
    }
    public enum APPLICATION_NAME_DESIGNATION
    {
        NONE, XpertAssist, XpertPharm, XpertMobile, XpertCVM
    }
    public enum TYPE_APPEL_LISTE_INSTANCE
    {
        NONE, XPH, XAS, CHIFA_MANNUEL
    }
    public enum TYPE_APPEL_LISTE_INSTANCE_CVM
    {
        NONE, LIST_INSTANCE, FORM_CVM
    }
    public enum TypePrimaryKey
    {
        AUTO_CODIF, MAX_ROW, DB_ID, NONE, GUID, MD5_ID,
        CONCAT, BD_NEW_ID
    }

    public enum XpertActions
    {
        None, AcInsert, AcUpdate, AcDelete, AcArchive, AcCloture, AcDecloture, AcPrint, AcSelect, AcImport,
        AcCustomData, AcColumns, AcSelectFromLookup
    }

    public enum XpertPacks
    {
        ACH, STK, VTE, BSE, TRS,  ADM, TDB
    }
    public enum XpertObjets
    {
        None
            ,
        SYS_OBLIGATION
            ,
        SYS_OBJET_PERMISSION
            ,
        SYS_OBJET_PWD
            ,
        SYS_TICKET
            ,
        SYS_USER
            ,
        SYS_GROUPE
            ,
        SYS_PARAMETRE
            ,
        TDB_ANALYSES
            ,
        TDB_ANALYSE_MVT_STK
            ,
        TDB_ANALYSE_MVT_STK_BY_PRODUIT
            ,
        TDB_ANALYSE_MENSUELLES_PRODUIT
            ,
        TDB_STAT_VTE_BY_USER
            ,
        TDB_STAT_NBR_VTE
            ,
        TDB_STAT_ACHAT_PAR_FOURNISSEUR
            ,
        SYS_FAVORIS_RUBBON
            ,
        SYS_OBJET_TRACE
            ,
        ACH_AVOIR
            ,
        ACH_COMMANDES
            ,
        ACH_FACTURE
            ,
        ACH_DOCUMENT
            ,
        ACH_RECEPTION
            ,
        ACH_RETOUR
            ,
        ACH_MANQUANTS
            ,
        ACH_RECLAMATIONS
            ,
        ACH_AUTORISER_RETOUR_FROM_STOCK
            ,
        ///////////XPERT_PRODUCTION /////////
        ACH_UPDATE_ENTETE
            ,
        ACH_UPDATE_DETAIL
            ,
        ACH_UPDATE_PRIX_HT
            ,
        ACH_RECEPTION_LISTE_AGRICULTEUR
            ,
        ACH_FACTURE_PSYCHOTHROPE
            ,
        ACH_COMMANDES_PSYCHOTHROPE
            ,
        PRD_PRODUCTION_AGRICULTURE,
        /////////////////////////////
        CFA_CENTRES
            ,
        View_TRS_ENCAISS_DETAIL
            ,
        STK_AJUSTEMENT_DETAIL
            ,
        STK_ENTREE
            ,
        ENTREES_WEB
            ,
        BSE_PRODUIT_FORMES
            ,
        STK_PRODUITS
            ,
        STK_LIST_PRODUIT_CAISSE
            ,
        STK_SORTIE
            ,
        STK_TRANSFERT
            ,
        STK_INVENTAIRE
            ,
        STK_STOCK
            ,
        STK_STOCK_BLOQUE
            ,
        STK_STOCK_UPDATE_QTT
            ,
        STK_SUIVI
            ,
        STK_ECHANGE
            ,
        BSE_PRODUIT_LABO
            ,
        BSE_PRODUIT_TYPE
            ,
        BSE_PRODUIT_DCI
            ,
        BSE_EMPLACEMENT
            ,
        View_BSE_MAGASIN
            ,
        BSE_COMPTE
            ,
        BSE_TIERS_FAMILLE
            ,
        BSE_TIERS_FAMILLE_SOLDE
            ,
        BSE_COMPTE_TYPE
            ,
        BSE_ENCAISS_MOTIFS
            ,
        BSE_TIERS_TYPE
            ,
        BSE_MEDECIN
            ,
        BSE_SPECIALITE_MEDECINE
            ,
        BSE_VALUES
            ,
        TRS_CREANCE
            ,
        View_TRS_CREDIT_PAIEMENT
            ,
        TRS_ENCAISS
            ,
        TRS_DECAISS
            ,
        TRS_JOURNEES
            ,
        TRS_JOURNEES_MAJ_MT_CLOTUR
            ,
        TRS_JOURNEES_MAJ_FOND_CAISSE
            ,
        TRS_JOURNEES_MAJ_MT_CLOTUR_ZERO
            ,
        TRS_TIERS
            ,
        TRS_CLIENT
            ,
        TRS_FOURNISSEUR
            ,
        TRS_CLIENT_FOURNISSEUR
            ,
        TRS_AUTRE
            ,
        TRS_TIERS_CHANGE_TYPE
            ,
        TRS_RESUME_SESSION
            ,
        TRS_DETAIL_SESSION
            ,
        TRS_ENCAISS_SANS_TIERS
            ,
        TRS_TIERS_FIDELITE
            ,
        View_TRS_VIREMENT
            ,
        VTE_AVOIR
            ,
        VTE_COMMANDE
            ,
        VTE_COMPTOIR
            ,
        VTE_FACTURE
            ,
        VTE_INSTANCE
            ,
        VTE_LIVRAISON
            ,
        VTE_PROFORMA
            ,
        VTE_RETOUR
            ,
        VTE_VENTE_CODEBARRE //prévilege pour Vente au comptoir uniquement avec code à barre
            ,
        View_VTE_BEST_CHOICE
            ,
        JOURNAL_VTE_COMMANDE_DETAIL
            ,
        View_VTE_JOURNAL_DETAIL
            ,
        JOURNAL_VTE_PROFORMA_DETAIL
            ,
        VTE_VENTE // idobjet pas dans les bll   
            ,
        VTE_ATTENTE //vente en attante pas relier au bll
            ,
        VTE_AUTORISER_RETOUR_FROM_STOCK
            ,
        VTE_USE_REMISE_WITH_PRIVILEGE
            ,
        VTE_ANNULER_VC
            ,
        VTE_SUPPRIMER_DETAIL_VC
            ,
        VTE_UPDATE_PRIX_VENTE
            ,
        VTE_COMPTOIR_TO_INSTANCE
            ,
        VTE_COMPTOIR_TO_CREDIT
            ,
        VTE_COMPTOIR_TO_CACHE
            ,
        VTE_BON_RETOUR
            ,
        VTE_COMPTOIR_TRACE
            ,
        VTE_ETAT_RECAP_VENTES_TAP
            ,
        VTE_ETAT_RECAP_VENTES_TAP_G50
            ,

        ACH_ASSIST_COMMANDES // assistance commande n'etulise pas une bll mais elle a un previlege pour le menue mainform
            ,
        MAJ_VERSION // evidament pas de bll
            ,
        MAJ_FICHE // meme remarque //, MAJ_A_PROPOS // meme remarque
            ,
        ETAT_CREANCE
            ,
        CFA_BENEFICIAIRE
            ,
        SYS_AUTHLIST
            ,
        SYS_FORM_CONFIG
            ,
        BSE_MOTIFS_VENTE_INSTANCE
            ,
        BSE_MOTIFS_ARCHIVE_INSTANCE
            ,
        SYS_CONFIGURATION_TABLEAUX_BORD
            ,
        BSE_PRODUIT_FAMILLE // enciennement FAMILLE
            ,
        View_STK_ECHANGE
            ,
        SYS_OPENTIROIRCAISSE // ajouter  pour le mainform
            ,
        TRS_TIROIRCAISSE_PREPAR_JOURNEE
            ,
        TRS_TIROIRCAISSE_CLOTURE_JOURNEE
            ,
        UP_PRODUIT
            ,
      
        STK_ARRIVAGE_INSTANCE
            ,
        ACH_DOCUMENT_DETAIL
            , // 
        ACH_COMMANDES_DETAILS
            ,
        
        STK_PRODUIT_FUSION
            ,
        SYS_EXPORT_XLS
            ,
        SYS_BASE_RESTORE
            ,
        SYS_BASE_CHANGE
            ,
        
        STK_STOCK_UPDATE_CB // Modification Code barre stock
            ,
        TDB_ANALYSES_CHIFR_VENDEUR
            ,
        TDB_ANALYSES_MARGE_VENDEUR
            ,
        TDB_CHIFFRE_DAFFAIRE_BY_MARGE
            ,
        TSB_ANALYSE_MENSUELLE_MVTS_STOCK
            ,
        TSB_ANALYSE_ACHAT_BY_FRNSSR_2
            ,
        STK_ETAT_STOCK_FROM_DATE
            ,
        SYS_PRINT_CB_USER
            ,
        TRS_CATRE_FOURNISSUER
            ,
        RPT_JRN_VENTE_PRD
            ,
        RPT_JRN_VENTE_PRD_ACHAT
            ,
        RPT_JRN_VENTE_PRD_PPA
            ,
        RPT_JRN_VENTE_PRD_PPA_ACHAT
            ,
        TRS_ANNULATION_TRANSACTION_VC
            ,
        TRS_VALIDATION_TRANSACTION_VC
            ,
        rv_ORD_ANNEX_13 // annex 13 psychotrope
            ,
        rv_ORD_ANNEX_12
            ,
        ENTREES_WEB_PSYCHOTHROPE
            ,
        ACH_PARAMETRAGE_FACTURE_WEB
            ,
        SYS_TOTAUX_LISTE
            ,
        REP_ETAT_DE_VENTE_JOURNALIERE
            ,
        TDB_ANALYSE_VENTE_TRANCHE_HEURE
            ,
        STK_PEREMPTION_DETECTOR
            ,
        VTE_UPDATE_NUM_FACTURE
            ,
        RPT_Bon_Vente_Comptoire
            ,
        VTE_UPDATE_VC_TO_BL
            ,
        SYS_PC_IGNORE_CLOTURE
            ,
        VTE_UPDATE_VC_TO_FV
            ,
        ACH_SHOW_PRIX_ACHAT_MOBILE
            ,
        VTE_UPDATE_PRIX_VENTE_MOBILE
            ,
        VTE_VENTE_DETAIL
            ,
        VTE_COMPTOIR_SAISIE
            ,
        BSE_PRODUIT_UNITE
            ,
        VTE_DETAIL_COMPTOIR_SAISIE
            ,
        VTE_VENTE_DETAIL_AFFICHER_MARGE
            ,
        STK_STOCK_UPDATE_TAUX_UG_VENTE
            ,
        VTE_COMMANDES_WEB
            ,
        ACH_RENDEZ_VOUS
            ,
        TRS_JOINTURE_TIERS_CARTE // affectaion de Carte Fidelité autiers
            ,
        VTE_VENTE_CARTE_FIDELITE // Vente au Comptoir avec Catre de Fidelité
            ,
        VTE_RE_VALIDER_VENTE_CARTE // Re-valider la Vente au Comptoir avec Catre de Fidelit
            ,
        View_ACH_DOCUMENT_DETAIL
            ,
        STK_PRODUIT_COMPOSANT
            ,
        BSE_PRODUIT_PRIX_VENTE
            ,
        BSE_EMPLACEMENT_PRODUIT
            ,
        BSE_PROMOTION_PRODUIT
            ,
        BSE_PRODUIT_LISTE_CB
            ,
        BSE_PRODUIT_CB_AUTRE_UNITE
            ,
        BSE_PRODUIT_AUTRE_UNITE
            ,
        VTE_SUPPRIMER_DETAIL_VTE
            ,
        ACH_SUPPRIMER_DETAIL_ACH
            ,
        CFA_FACTURE_CHIFA_TO_CREDIT
            ,
        VTE_COMPTOIR_VALIDER_CREDIT
            ,
        VTE_COMPTOIR_VALIDER_TPE
            ,
        TRS_TRACE_ENCAISS_DELETED
            ,
        TRS_ENCAISS_LIST_COMPTE
            ,
        TRS_ENCAISS_LIST_CAISSE
            ,
        CFA_DETAIL_FACTURE_CHIFA
 ,
        VTE_VENTE_SANS_TICKET
,
        ACH_UPDATE_DETAIL_UNITE_MESURE
,
//Web
        View_PANIER
            ,
        PRODUITS
            ,
        NOTES
            ,
        AVIS
            ,
        TRS_ZAKAT
    }

    public enum BSE_TABLE_NAME
    {
        BSE_PRODUIT_DCI, BSE_MOTIFS_VENTE_INSTANCE, BSE_PRODUIT_SPECIALITE, BSE_PRODUIT_FORMES, BSE_PRODUIT_FAMILLE,
        BSE_MOTIFS_ARCHIVE_INSTANCE, BSE_MAGASINS, BSE_ENCAISS_MOTIFS, CFA_CENTRES, BSE_PRODUIT_UNITE
        ,BSE_REGION
    }

    public enum TYPE_VALUES
    {
        TVA, SHP
    }


    public enum Edition_Features
    {
        none,  //for all
        CVM,   // Militaire  (for all Editions)

        //Extra Features   (for Advanced + XPERT Editions only)
        MOB,   // Inventaire Mobile
        CNXD,  // Conn. Distance
        OCAN,  // Outil Conf. Analyse
        PDATA, // Prévilege DATA
        XMOB,  // Xpert Mobile
        MSUS,  // Mult. Session User
        DIM,   // Détection des Interactions Médicamenteuses
        STCH,   // Suivi des Traitements Chroniques (TEMPO plus tards XPERT)

        // Features available in XPERT Edition (from Advanced Edition)
        SCOM,  // Suivi Commande
        ACOM,  // Assistant Commande
        IFEL,  // Imp. Fact. Elect.
        TFOND, // Transfert Fond
        VNAUT, // Vent. Auto
        SBCH,  // Suivi Bord. CHIFA
        XVIS,  // XAS Exp. Vent. inst.
        PCFCH, // Prev. Conf. Champ
        TATH,  // Trace Authentif.
        XMJR,  // XAS Majoration
        CARTF, // Carte Fidelité
        DTRP,  // Detection Risquede Peremption 

        // Features available in XPERT Edition (Free only with valid Support)
        XCMD,  // XAS Controle medical via CNAS -- Gratuit pour les clients sous support
        PSYC,  // PSYC Module psychotrope selon la nouvelle loi 03/11/2020 -- Gratuit pour les clients sous support

        //Feature for Start Edition only
        XASB,  // XpertAssist Basic
        VTE_COMMANDES_WEB
    }
    public enum TYPE_LOOKUP
    {
        STATIC,
        DYNAMIQUE,
        NON
    }
}

