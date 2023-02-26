using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using SQLite;
using XpertMobileApp.Api.Services;
using XpertMobileApp.Models;

namespace XpertMobileApp.DAL
{
    public class View_VTE_Vente_Td22
    {
        public decimal Sum_TOTAL_VENTE { get; set; }
        public decimal Sum_TOTAL_ACHAT { get; set; }
        public decimal Sum_MARGE { get; set; }
        public string CREATED_BY { get; set; }
        public string Exercice { get; set; }
    }

    public partial class STAT_VTE_BY_USER
    {
        public DateTime DATE_DEBUT { get; set; }
        public DateTime DATE_FIN { get; set; }
        public Int16 CODE_TYPE_PRODUIT { get; set; }
        public string TYPE_VENTE { get; set; }
        public string UTILISATEUR { get; set; }
        public decimal MONTANT_ACHAT { get; set; }
        public decimal MONTANT_PPA { get; set; }
        public decimal MONTANT_SHP { get; set; }
        public decimal MONTANT_VENTE { get; set; }
        public decimal MONTANT_MARGE { get; set; }
        public decimal MARGE_TAUX { get; set; }
        public int NOMBRE_OPERATION { get; set; }
        public int NOMBRE_PRODUIT { get; set; }
        public string TOTAL_PAYE { get; set; }
    }

    public partial class VTE_VENTE : BASE_CLASS
    {
        [PrimaryKey, AutoIncrement]
        public int? ID { get; set; }
        public string CODE_VENTE { get; set; } // varchar(32)
        public string NUM_VENTE { get; set; } // varchar(32)
        public string REF_CLIENT { get; set; } // varchar(32)
        public DateTime? DATE_VENTE { get; set; } // datetime(3)
        public string CODE_TIERS { get; set; } // varchar(32)

        public string TYPE_VENTE { get; set; } // char(2)
        public string ETAT_VENTE { get; set; } // varchar(2)
        public string CODE_MODE { get; set; } // varchar(1)
        public decimal MT_TIMBRE { get; set; } // money(19,4)
        public decimal TAUX_REMISE { get; set; } // money(19,4)

        private decimal tOTAL_HT;
        public decimal TOTAL_HT
        {
            get
            {
                return tOTAL_HT;
            }
            set
            {
                tOTAL_HT = value;
                OnPropertyChanged("TOTAL_HT");
            }
        }

        private decimal tOTAL_TTC;
        public decimal TOTAL_TTC
        {
            get
            {
                return tOTAL_TTC;
            }
            set
            {
                if (tOTAL_TTC != value)
                {
                    tOTAL_TTC = value;
                    OnPropertyChanged("TOTAL_TTC");
                }
            }
        }

        public decimal TOTAL_PPA { get; set; } // money(19,4)
        public decimal TOTAL_SHP { get; set; } // money(19,4)
        public decimal TOTAL_REMISE { get; set; } // money(19,4)
        public decimal TOTAL_TVA { get; set; } // money(19,4)
        public decimal REMISE_GLOBALE { get; set; } // money(19,4)
        public decimal TOTAL_RTA { get; set; } // money(19,4)
        public bool APPLIQUER_TVA { get; set; } // bit
        public string EXERCICE { get; set; } // char(4)
        public bool AUTO_CREATE { get; set; } // bit
        public string CODE_FACTURE { get; set; } // varchar(32)
        //public decimal   TOTAL_PAYE      { get; set; } // money(19,4)
        public DateTime? DATE_ECHEANCE { get; set; } // datetime(3)

        private decimal tOTAL_RECU;
        public decimal TOTAL_RECU
        {
            get
            {
                return tOTAL_RECU;
            }
            set
            {
                if (tOTAL_RECU != value)
                {
                    SetProperty(ref tOTAL_RECU, value);
                }
            }
        }

        public string ID_CAISSE { get; set; } // varchar(50)
        public string CODE_ORIGINE { get; set; } // varchar(32)
        public int TYPE_VALIDATION { get; set; } // int(10)
        public short SENS_DOC { get; set; } // smallint(5)
        public string STATUS_DOC { get; set; } //varchar(20)
        public int TYPE_PRIX_VENTE { get; set; } // int(10)
        public DateTime? CREATED_ON { get; set; } // datetime(3)
        public string CREATED_BY { get; set; } // varchar(200)
        public DateTime? MODIFIED_ON { get; set; } // datetime(3)
        public string MODIFIED_BY { get; set; } // varchar(200)
        public string NOTE_VENTE { get; set; } // nvarchar(-1)
        public string CODE_MOTIF { get; set; }
        public string CODE_BARRE { get; set; }
        public string N_ORDRE_PSYCHO { get; set; }
        public decimal TOTAL_ARCHIVE { get; set; } // money(19,4)
        public string CODE_MEDECIN { get; set; }
        public int DUREE { get; set; }
        public bool EXON_TIMBRE { get; set; }
        public bool EXON_TVA { get; set; }
        public string CODE_MOTIF_LIVRAISON { get; set; }
        public string MODE_CAL_MT_ECHANGE { get; set; }
        // CVM
        public decimal TOTAL_TARIF_CVM { get; set; } // money(19,4)
        public bool IS_CVM_COMPLETE { get; set; }
        public string CREATED_POSTE { get; set; } // varchar(300)
        public DateTime? DATE_SOINS { get; set; }
        public string NUM_TERMINAL { get; set; } // varchar(300)
        public string NUM_COMMERCANT { get; set; } // varchar(300)
        public string IS_DIM { get; set; } // varchar(200)
        public string NOTE_PIED { get; set; } // nvarchar(MAX)
        public string NOM_PORTEUR_ORD { get; set; }
        public string N_CARTE_PORTEUR_ORD { get; set; }
        public string ADRESS_LIVRAISON_COMMANDE { get; set; }
        public int NBR_COLIS { get; set; }
        public DateTime? CLOTURE_ON { get; set; } // datetime(3)
        public string CLOTURED_BY { get; set; } // varchar(200)
        public bool SYNCHRONISE { get; set; }
        public decimal MT_VERSEMENT { get; set; } // money(19,4)
        // XBOUTIK
        public int ORIGINE_COMMANDE { get; set; }
        public string ID_CMD_WEB { get; set; }
        public string DESIGN_MOTIF_ANNULATION { get; set; }

        public string TYPE_ORIGINE { get; set; } // char(2)

        //public decimal   TOTAL_PAYE      { get; set; } // money(19,4)
    }

    public partial class View_VTE_VENTE : VTE_VENTE
    {
        public string NOM_MEDECIN { get; set; }
        public string DESIGNATION_WILAYA_DELIVRE { get; set; }
        public string DATE_DELIVRE_CARTE { get; set; }
        public string DATE_DELIVRE_CARTE_TITLE
        {
            get
            {
                return !string.IsNullOrEmpty(DATE_DELIVRE_CARTE) && DATE_DELIVRE_CARTE.ToString().Length >= 10 ? DATE_DELIVRE_CARTE.ToString().Substring(0, 10) : "";
            }
        }
        public string N_CARTE { get; set; }

        public decimal TOTAL_PAYE { get; set; } // money(19,4)
        public decimal TOTAL_RESTE { get; set; } // money(19,4)
        public decimal MT_VERSEMENT { get; set; } // money(19,4)

        private string nOM_TIERS;
        public string NOM_TIERS
        {
            get
            {
                return nOM_TIERS;
            }
            set
            {
                nOM_TIERS = value;
                OnPropertyChanged("NOM_TIERS");
            }
        }
        public string TITRE_VENTE { get; set; } // varchar(300)
        public string TYPE_DOC { get; set; }

        public override string ToString()
        {
            return "N° " + NUM_VENTE;

        }

        public string CODE_CARTE_FIDELITE { get; set; }

        private decimal pOINTS_CONSUMED;
        public decimal POINTS_CONSUMED
        {
            get
            {
                return pOINTS_CONSUMED;
            }
            set
            {
                pOINTS_CONSUMED = value;
                OnPropertyChanged("POINTS_CONSUMED");
            }
        }
        public string CODE_TOURNEE { get; set; }

        #region Validation vente
        [Ignore]
        public List<View_VTE_VENTE_LOT> Details { get; set; }
        [Ignore]
        public List<View_VTE_VENTE_LIVRAISON> DetailsDistrib { get; set; }
        public string ID_Random { get; internal set; }
        public string MBL_MODE_PAIMENT { get; internal set; }
        private string _MBL_NUM_CARTE_FEDILITE;
        public string MBL_NUM_CARTE_FEDILITE
        {
            get { return _MBL_NUM_CARTE_FEDILITE; }
            set { SetProperty(ref _MBL_NUM_CARTE_FEDILITE, value); }
        }

        private decimal _MBL_MT_VERCEMENT;
        public decimal MBL_MT_VERCEMENT
        {
            get { return _MBL_MT_VERCEMENT; }
            set
            {
                if (SetProperty(ref _MBL_MT_VERCEMENT, value))
                {
                    if (_MBL_MT_VERCEMENT > this.TOTAL_TTC)
                    {
                        MBL_MT_RENDU = this.TOTAL_TTC - _MBL_MT_VERCEMENT;
                    }
                }
            }
        }

        private decimal _MBL_MT_RENDU;

        public decimal MBL_MT_RENDU
        {
            get { return _MBL_MT_RENDU; }
            set
            {
                SetProperty(ref _MBL_MT_RENDU, value);
            }
        }
        public string MBL_CODE_TOURNEE_DETAIL { get; set; }
        public double GPS_LATITUDE { get; set; }
        public double GPS_LONGITUDE { get; set; }
        public string NameTableVente { get; set; }
        public string NameTableVenteDetail { get; set; }
        public string TauxRemiseGlobale
        {
            get
            {
                decimal dd = this.TOTAL_TTC + this.REMISE_GLOBALE;
                if (dd == 0) return "0 %";
                return XpertHelper.RoundPrix(100 * this.REMISE_GLOBALE / dd) + " %";
            }
        }
        public string DESIGN_TYPE
        {
            get
            {
                if ("VC".Equals(this.TYPE_VENTE))
                    return "Vente au Comptoir";
                else if ("RC".Equals(this.TYPE_VENTE))
                    return "Retour Client";
                return "";
            }
        }
        [Ignore]
        public string NUM_FACTURE
        {
            get
            {
                return this.CODE_FACTURE;
            }
        }
        [Ignore]
        public string ETAT_IMPORT
        {
            get
            {
                switch (this.ETAT_VENTE)
                {
                    case "CC":
                        return "Commandé";
                    case "BL":
                        return "Livré";
                    case "FV":
                        return "Facturé";
                    default:
                        return "";
                }
            }
        }
        // date sois sera dans la table pas dans la view
        //public DateTime? DATE_SOINS { get; set; }
        [Ignore]
        public decimal TOTAL_ASSURE
        {
            get
            {
                return this.TOTAL_TTC - TOTAL_TARIF_CVM;
            }
        }
        //informations CVM
        public string NUM_DECOMPTE { get; set; }
        public string ORDRE { get; set; }
        public string ID_CVM_TYPE_DECOMPTE { get; set; }
        public DateTime? DATE_SOINS_CVM { get; set; }
        public string CODE_PATHOLOGIE { get; set; }
        public string NUM_FACTURE_CVM { get; set; }
        public string NUM_BORDEREAU_CVM { get; set; }
        public string CVM_ID_FACTURE { get; set; }
        public string DESIGNATION_DECOMPTE { get; set; }
        public string DESIGNATION_FACTURE { get; set; }
        public string DESIGNATION_PHATOLOGIE { get; set; }
        public DateTime? DATE_FACTURE_CVM { get; set; }
        public DateTime? DATE_DECOMPTE_CVM { get; set; }
        public string NOM_ASSURE { get; set; }
        [Ignore]
        public bool IS_NOT_CVM_COMPLETE

        {
            get
            {
                return !IS_CVM_COMPLETE;
            }
        }
        public decimal QTE_TOTAL_VENTE { get; set; }
        public decimal QTE_TOTAL_IMPORTER { get; set; }
        public decimal QTE_REST_FACTURER { get; set; }
        public string NUM_TICKET { get; set; }
        [Ignore]
        public decimal MontantVerseRestant
        {
            get
            {
                return this.TOTAL_TTC - TOTAL_RECU;
            }
        }
        public string OWNER { get; set; } // varchar(200)
        public string ADRESSE_TIERS { get; set; } // varchar(500)
        public string NUM_ASSURE { get; set; } // varchar(12)
        public string TEL1_TIERS { get; set; } // varchar(50)
        public string TEL2_TIERS { get; set; } // varchar(50)
        public string DESIGN_MODE { get; set; } // varchar(300)
        public string DESIGN_MOTIF { get; set; } // nvarchar(250)
        public decimal SOLDE_TIERS_AT_VALIDAT { get; set; } // money(19,4)
        public decimal TOTAL_ENCAISS_AT_VALIDAT { get; set; } // money(19,4)
        public DateTime? DATE_MAX_ARCHIVE { get; set; }
        public string ARCHIVER_PAR { get; set; }
        [Obsolete("Warning :: TYPE_DOC2 is empty in all sales except vente chifa")]
        public string TYPE_DOC2 { get; set; }
        // utiliser juste  pour Séparer entre vente CHIFA cash & vente CHIFA crédit
        // Warning :: TYPE_DOC2 is empty in all sales except vente chifa  ()        
        public decimal TOTAL_HT_REEL { get { return TOTAL_HT * SENS_DOC; } }

        public decimal TOTAL_PPA_REEL { get { return TOTAL_PPA * SENS_DOC; } }

        public decimal TOTAL_SHP_REEL { get { return TOTAL_SHP * SENS_DOC; } }

        public decimal TOTAL_REMISE_REEL { get { return TOTAL_REMISE * SENS_DOC; } }

        public decimal TOTAL_TVA_REEL { get { return TOTAL_TVA * SENS_DOC; } }

        public decimal TOTAL_TTC_REEL { get { return TOTAL_TTC * SENS_DOC; } }

        public decimal TOTAL_PAYE_REEL { get { return TOTAL_PAYE * SENS_DOC; } }

        public decimal TOTAL_RESTE_REEL { get { return TOTAL_RESTE * SENS_DOC; } }

        public decimal TOTAL_TTC_INSTANCE { get; set; }
        public string DESIGN_MOTIF_LIVRAISON { get; set; }
        public string TYPE_TIERS { get; set; }
        public decimal TOTAL_TTC_SANS_REMISE { get; set; }
        public DateTime? DATE_PAYE_CREANCE { get; set; }
        public string DESIGNATION_UNITE { get; set; }
        public string ETAT_ENCAISS { get; set; }
        // pour indique que la vente est importee a partir chifa 
        //se qui signefie que il faut pas modifier la date sois et la date vente dans la fenetre psychothrope
        public bool IS_IMPORTED { get; set; }
        #region Validation vente from Mobile

        #endregion

        public string TITRE_VENTE_SPECIAL
        {
            get
            {
                if ("4".Equals(CODE_MODE) && "VC".Equals(TYPE_VENTE))
                {
                    return "Vente TPE";
                }
                else
                {
                    return this.TITRE_VENTE;
                }
            }
        }
        public string ID_USER_TARGET { get; set; }
        public string DESIGNATION_STATUS { get; set; }
        public string DESIGNATION_ORIGINE { get; set; }
        #endregion
    }

    public class VIEW_FIDELITE_INFOS
    {
        public string CODE_CARD { get; set; }
        public decimal POINTS_USED { get; set; }
        public decimal MT_POINTS_USED { get; set; }
        public decimal MAX_MT_POINTS { get; set; }
    }

    public class VTE_COMMANDE
    {
        //Cet Objet est créer pour etre passé comme paramétre dans la fonction SyncData (sqlite) 
    }
    public partial class View_VTE_COMMANDE : View_VTE_VENTE
    {
        public string DESIGNATION_STATUS { get; set; }
        public string DESIGNATION_ORIGINE { get; set; }
    }

    public partial class View_VTE_PSYCHOTROP : View_VTE_VENTE
    {
    }

    public partial class VTE_VENTE_DETAIL : BASE_CLASS
    {
        [PrimaryKey, AutoIncrement]
        public int ID { get; set; }
        //[Ignore] 
        // public View_VTE_VENTE Parent_Doc { get; set; }
        public string CODE_DETAIL { get; set; } // varchar(32)
        public string CODE_VENTE { get; set; } // varchar(32)
        public int? ID_STOCK { get; set; } // int(10)
        public string CODE_PRODUIT { get; set; } // varchar(32)
        public string CODE_BARRE_PRODUIT { get; set; } // varchar(32)
        public string CLEANED_CODE_DOC
        {
            get
            {
                return CODE_VENTE?.Replace("/", "").Replace(" ", "");
            }
        }


        // public decimal PRIX_VTE_TTC { get; set; } // money(19,4)

        private decimal _PRIX_VTE_TTC;
        public decimal PRIX_VTE_TTC
        {
            get
            {
                return _PRIX_VTE_TTC;
            }
            set
            {
                // qUANTITE = value;
                if (_PRIX_VTE_TTC != value)
                {
                    MT_TTC = value * QUANTITE;
                    MT_HT = value * QUANTITE;

                    SetProperty(ref _PRIX_VTE_TTC, value);
                    OnPropertyChanged("MT_TTC");
                    OnPropertyChanged("MT_HT");
                }
            }
        }

        public decimal MT_TTC { get; set; } // money(19,4)

        public decimal QTE_LIVREE { get; set; } // numeric(19,2)
        public decimal QTE_BONUS { get; set; } // numeric(19,2)
        public decimal PRIX_VTE_HT { get; set; } // money(19,4)
        public decimal MT_HT { get; set; } // money(19,4)
        public decimal TAUX_REMISE { get; set; } // money(19,4)
        public decimal TAUX_TVA { get; set; } // money(19,4)
        public decimal MT_TVA { get; set; } // money(19,4)
        public decimal MT_REMISE { get; set; } // money(19,4)
        public decimal REMISE_GLOBALE { get; set; } // money(19,4)
        public decimal RTA { get; set; } // money(19,4)
        public string EXERCICE { get; set; } // char(4)
        public decimal REMISE { get; set; } // money(19,4)
        public string CODE_DETAIL_ORIGINE { get; set; } // varchar(32)
        public int? ID_BEST { get; set; }
        public short TYPE_BEST { get; set; }
        public bool IS_BEST { get; set; }
        public DateTime? CREATED_ON { get; set; } // datetime(3)
        public string CREATED_BY { get; set; } // varchar(200)
        public DateTime? MODIFIED_ON { get; set; } // datetime(3)
        public string MODIFIED_BY { get; set; } // varchar(200)
        public decimal COUT_ACHAT { get; set; } // money(19,4)
        public decimal MT_ACHAT { get; set; } // money(19,4)
        public string NUM_SERIE { get; set; } // varchar(500) - numero de serie 
        public string ID_ARRIVAGE_INSTANCE { get; set; }
        public short PSYCHOTHROPE { get; set; }
        // CVM
        public decimal TARIF_CVM { get; set; } // money(19,4)

        // Binding
        private decimal qUANTITE;
        public decimal QUANTITE
        {
            get
            {
                return qUANTITE;
            }
            set
            {
                // qUANTITE = value;
                if (qUANTITE != value)
                {
                    MT_TTC = value * PRIX_VTE_TTC;
                    MT_HT = value * PRIX_VTE_HT;

                    SetProperty(ref qUANTITE, value);
                    OnPropertyChanged("MT_TTC");
                    OnPropertyChanged("MT_HT");
                }
            }
        }
        [Ignore]
        public bool IsRetour { get; set; }
    }

    public partial class View_VTE_VENTE_LOT : VTE_VENTE_DETAIL
    {
        public string IMAGE_URL { get; set; }
        public string NUM_VENTE { get; set; } // varchar(32)
        public DateTime? DATE_VENTE { get; set; } // datetime(3)
        public string NOM_TIERS { get; set; } // varchar(501)
        public string CODE_TIERS { get; set; } // varchar(32)
        public string TYPE_VENTE { get; set; } // char(2)
        public decimal QTE_RETOUR { get; set; }
        public decimal QTE_RESTE_RETOUR { get; set; }
        public string CODE_ORIGINE { get; set; } // varchar(32)
        public decimal TAUX_MARGE { get; set; } // money(19,4)
        public string OWNER { get; set; } // varchar(200)
        public string DESIGNATION_PRODUIT { get; set; } // varchar(754)
        public string CODE_MAGASIN { get; set; } // varchar(10)
        public DateTime? DATE_PEREMPTION { get; set; }
        public string CODE_BARRE { get; set; } // nvarchar(250)
        public string CODE_BARRE_LOT { get; set; } // nvarchar(1500)
        public int TYPE_VALIDATION { get; set; } // int(10)
        public string CODE_EMPLACEMENT { get; set; } // varchar(10)
        public string DESIGN_EMPLACEMENT { get; set; } // varchar(10)
        public string LOT { get; set; } // varchar(50)
        public decimal QTE_STOCK { get; set; } // numeric(18,2)
        public decimal PPA { get; set; } // money(19,4)
        public decimal SHP { get; set; } // money(19,4)
        public string DESIGN_MAGASIN { get; set; } // varchar(100)
        public string REF_PRODUIT { get; set; } // nvarchar(250)
        public decimal PRIX_ACH_HT { get; set; } // money(19,4)
        public decimal PRIX_VENTE { get; set; } // money(19,4)
        public short TYPE_PRODUIT { get; set; } // smallint(5)
        public decimal TARIF { get; set; } // money(19,4)
        public string CODE_LABO { get; set; } // smallint(5)
        public string CODE_FORME { get; set; } // varchar(20)
        public string DESIGN_LABO { get; set; } // varchar(2500)
        public string DESIGN_FORME { get; set; } // varchar(100)
        public string UNITE { get; set; } // varchar(50)
        public string CONDIT { get; set; } // varchar(50)
        public string DOSAGE { get; set; } // varchar(50)
        public string CODE_CNAS { get; set; }
        public bool PENDING { get; set; }
        public decimal PENDING_QUANTITY { get; set; }
        public bool EXON_TVA { get; set; }
        public decimal QTE_STOCK_PRODUIT { get; set; } // numeric(18,2)
        public string DESIGNATION_UNITE { get; set; }
        public string DESIGNATION_UNITE2 { get; set; }
        public string UNITE_AFFICHER { get; set; }
        public decimal QTE_AFFICHER { get; set; }
        public decimal PU_AFFICHER { get; set; }

        // pour que le detail facture chifa indique esque la ligne destocké ou  non 
        // elle a la valeur inverse de la valeur ignoree destockage dans la fenaitre improtation chifa
        public bool DESTOCKER { get; set; }
        public string DESIGNATION_CVM { get; set; }
        public string CODE_MEDIC { get; set; }
        public string N_ORDRE_PSYCHO_ENTET { get; set; }
        public DateTime? DATE_SOINS { get; set; }
        public string CODE_MEDECIN { get; set; }

        public string TYPE_DOC { get; set; }
        public string ID { get; internal set; }
        public string VenteID { get; internal set; }
        public bool HAS_NEW_ID_STOCK { get; set; } = false; // varchar(2500)
    }
    public partial class View_VTE_JOURNAL_DETAIL : VTE_VENTE_DETAIL
    {

        public string NUM_VENTE { get; set; } // varchar(32)
        public DateTime? DATE_VENTE { get; set; } // datetime(3)
        public string NOM_TIERS { get; set; } // varchar(501)
        public string CODE_TIERS { get; set; } // varchar(32)
        public string TYPE_VENTE { get; set; } // varchar(2)
        public string CODE_EMPLACEMENT { get; set; } // varchar()
        public string DESIGNATION_EMPLACEMENT { get; set; } // varchar()
        public string TITLE_VENTE { get; set; } // varchar(300)
        public string DESIGNATION { get; set; } // varchar(404)


        public decimal PRIX_VENTE { get; set; } // money(19,4)

        public decimal MT_VENTE { get; set; } // money(19,4)

        public decimal MT_PPA { get; set; } // numeric(38,6)
        public decimal MT_SHP { get; set; } // numeric(38,6)
        public string CODE_MAGASIN { get; set; } // varchar(10)
        // char(8)
        public string CODE_BARRE { get; set; } // nvarchar(250)
        public string CODE_BARRE_LOT { get; set; } // nvarchar(1500)
        public string LOT { get; set; } // varchar(50)
        public decimal PPA { get; set; } // money(19,4)
        public decimal SHP { get; set; } // money(19,4)
        public short TYPE_PRODUIT { get; set; } // smallint(5)
        public int SENS_DOC { get; set; } // int(10)
        public string VENTE_USER { get; set; } // varchar(50)
        public string TITRE_VENTE { get; set; } // varchar(300)
        public string TYPE_DOC { get; set; }
        public string CODE_LABO { get; set; }
        public string CODE_FAMILLE { get; set; } // varchar(10)
        public string DESIGNATION_LABO { get; set; }
        public string DESIGNATION_FAMIL { get; set; }
        public string TYPE_PRODUIT_TITLE { get; set; }
        public decimal PPA_REEL { get; set; }
        public decimal SHP_REEL { get; set; }
        public decimal PRIX_VENTE_REEL { get; set; }
        public decimal COUT_ACHAT_REEL { get; set; }
        public decimal MT_VENTE_REEL { get; set; }
        public decimal MT_ACHAT_REEL { get; set; }
        public decimal MT_PPA_REEL { get; set; }
        public decimal MT_SHP_REEL { get; set; }

        public decimal TOTAL_RESTE { get; set; }
        public decimal TOTAL_PAYE { get; set; }
        public decimal TOTAL_TTC { get; set; }
        public string TYPE_DOC_ACHAT { get; set; }

        public decimal TOTAL_RESTE_REEL { get; set; }
        public decimal TOTAL_PAYE_REEL { get; set; }
        public decimal TOTAL_TTC_REEL { get; set; }

        public string HEUR_VENTE
        {
            get
            {
                return DATE_VENTE.Value.TimeOfDay.ToString();
            }
        }
        public string DATE_VENTE1
        {
            get
            {
                return DATE_VENTE.Value.ToShortDateString();
            }
        }
    }

    public class COM_DOC : BASE_CLASS
    {
        public string Code_doc { get; set; }
        public string Title { get; set; }
        public DateTime? Date_doc { get; set; }
        public string State { get; set; } // smallint(5)
        public string Creator { get; set; }
        public string Third { get; set; }

        public DateTime? CREATED_ON { get; set; } // datetime(3)
        public string CREATED_BY { get; set; } // varchar(200)
        public DateTime? MODIFIED_ON { get; set; } // datetime(3)
        public string MODIFIED_BY { get; set; } // varchar(200)

        public decimal TOTAL_DOC { get; set; }
        public decimal TOTAL_Payed { get; set; }
        public decimal TOTAL_Rest
        {
            get
            {
                return TOTAL_DOC - TOTAL_Payed;
            }
        }
    }

    public class COM_DOC_DETAIL : BASE_CLASS
    {
        public string Code_doc { get; set; }
        public string Title { get; set; }
        public decimal Quantity { get; set; }
        public decimal Price { get; set; }
        public decimal Amount
        {
            get
            {
                return Quantity * Price;
            }
        }
    }

    public partial class BSE_DOCUMENTS_TYPE
    {
        public string CODE_TYPE { get; set; }
        public string DESIGNATION_TYPE { get; set; }
        public string TYPE_DATA { get; set; }
        public string TYPE_DOC { get; set; }
    }

    public partial class FACTURE_CHIFA : BASE_CLASS
    {
        public string CODE_TIERS { get; set; }
        public string NUM_BOURDEREAU { get; set; }
        public string CREATED_BY { get; set; }
        public string ETAT_FACT { get; set; }
        public string ID_CAISSE { get; set; }
        public string NUM_FACTURE { get; set; }
        public DateTime DATE_FACTURE { get; set; }
        public decimal MONT_OFFICINE { get; set; }
        public decimal MONT_ASSURE { get; set; }
        public decimal MONT_FACTURE { get; set; }
        public string CODE_CLIENT { get; set; }
        public string RAND_AD { get; set; }
    }

    public partial class View_CFA_MOBILE_FACTURE : FACTURE_CHIFA
    {
        public string NOMC_TIERS { get; set; }
        public string NUM_ASSURE { get; set; }
        public string CENTRE{ get; set; }
        public decimal MONT_ACHAT { get; set; }
        public decimal MONT_TOTAL { get; set; }
        public string DESIGN_ETAT { get; set; }
        public double MONT_MAJORATION { get; set; }
        public double TOTAL_CHIFA { get; set; }
        public double TOTAL_CASNOS { get; set; }
        public double TOTAL_HORS_CHIFA { get; set; }
        public string NUMASSUR_RAND
        {
            get
            {
                return NUM_ASSURE.ToString() + "-" + RAND_AD.ToString();
            }
            set
            {
                NUMASSUR_RAND = value;
            }
        }
        //for Mobile only 
        public string NavigationBar_Title
        {
            get
            {
                return $"Facture N° {NUM_FACTURE} {String.Format($"Du :{DATE_FACTURE:dd/MM/yyyy}")}";
            }
            set
            {
                NavigationBar_Title = value;
            }
        }
        public decimal TOTAL_ASSURE { get; set; }
        public decimal TOTAL_AYD { get; set; }
        public int DUREE_TRAIT { get; set; }
        public DateTime DATE_SOIN { get; set; }
        public DateTime DATE_PREVUE { get { return DATE_SOIN.AddDays(DUREE_TRAIT); } }
        public int TOTAL_NB_FACTURE { get; set; }
        public int GROUPE_ANALYSE_FACTURE { get; set; }

    }
    public partial class View_CONVENTION_FACTURE : FACTURE_CHIFA
    {
        public string NOM_TIERS { get; set; }
        public string PRENOM_TIERS { get; set; }
        public string NOMC_TIERS { get; set; }
        public string TEL1_TIERS { get; set; } // varchar(50)
        public byte ETAT { get; set; } // tinyint(3)
        public string DESIGN_ETAT { get; set; }
        public int Chifa_Pharmenos { get; set; } // int(10)        
        public string CREATED_BY { get; set; }
        public decimal TOTAL_CONV_TO_PAY { get; set; } // money(19,4)
        public decimal NUM_ASSURE { get; set; } // money(19,4)
        public DateTime? DATE_SOIN { get; set; }

        // Extension not in database
        public int importSate { get; set; }
        public string ETAT_ENCAISS { get; set; }
        public decimal MONT_ASSURE_SANS_REMISE { get; set; }
        public string NUM_ASSURE_RAND
        {
            get
            {
                return NUM_ASSURE + "-" + RAND_AD;
            }
        }
        public string CENTRE { get; set; } // char(5)
    }

    #region Livraison
    public partial class LIV_SECTEUR : BASE_CLASS
    {
        public string CODE_SECTEUR { get; set; }
        public string NUM_SECTEUR { get; set; }
        public string TYPE_DOC { get; set; }
        public string NOM { get; set; }
        public string DESCRIPTION { get; set; }
        public string COMMUNE { get; set; }
        public string WILAYA { get; set; }

        public string CREATED_BY { get; set; }
        public DateTime? CREATED_ON { get; set; }
        public DateTime? MODIFIED_ON { get; set; }
        public string MODIFIED_BY { get; set; } // varchar(200)
    }
    public partial class LIV_SECTEUR_DETAIL
    {
        public string CODE_DETAIL { get; set; }
        public string CODE_TIERS { get; set; }
        public string CODE_SECTEUR { get; set; }

        public string CREATED_BY { get; set; }
        public DateTime? CREATED_ON { get; set; }
        public DateTime? MODIFIED_ON { get; set; }
        public string MODIFIED_BY { get; set; } // varchar(200)
    }
    public partial class View_LIV_SECTEUR_DETAIL : LIV_SECTEUR_DETAIL
    {
        public string NOM_TIERS { get; set; }
        public decimal SOLDE { get; set; }

    }
    public partial class LIV_TOURNEE : BASE_CLASS
    {
        [PrimaryKey, AutoIncrement]
        public int ID { get; set; }
        public string CODE_TOURNEE { get; set; }
        public string NUM_TOURNEE { get; set; }
        public DateTime DATE_TOURNEE { get; set; }
        public string NOTE { get; set; }
        public string CODE_SECTEUR { get; set; }
        public string CODE_MAGASIN { get; set; }
        public string CODE_VENDEUR { get; set; }
        public string CREATED_BY { get; set; }
        public DateTime? CREATED_ON { get; set; }
        public DateTime? MODIFIED_ON { get; set; }
        public string MODIFIED_BY { get; set; } // varchar(200)
        public TourneeStatus ETAT_TOURNEE { get; set; }
        public DateTime? DATE_DEPART_TOURNEE { get; set; }
        public DateTime? DATE_RETOUR_TOURNEE { get; set; }
        public DateTime? DATE_LAST_SYNC_TOURNEE { get; set; }
    }
    public partial class View_LIV_TOURNEE : LIV_TOURNEE
    {
        public string NUM_SECTEUR { get; set; }
        public string NOM_SECTEUR { get; set; }
        public string DESIGNATION_MAGASIN { get; set; }
        public string NOM_VENDEUR { get; set; }
        [Ignore]
        public string ETAT_COLOR
        {
            get
            {
                if (ETAT_TOURNEE == TourneeStatus.Planned)
                {
                    return "#ffffff";
                }
                else if (ETAT_TOURNEE == TourneeStatus.EnRoute)
                {
                    return "#7EC384";
                }
                else if (ETAT_TOURNEE == TourneeStatus.Visited)
                {
                    return "#7EC384";
                }
                else if (ETAT_TOURNEE == TourneeStatus.Delivered)
                {
                    return "#009B72";
                }
                else if (ETAT_TOURNEE == TourneeStatus.Canceled)
                {
                    return "#F26430";
                }
                else if (ETAT_TOURNEE == TourneeStatus.VisitedNotDelivered)
                {
                    return "#6761A8";
                }
                else if (ETAT_TOURNEE == TourneeStatus.NotVisited)
                {
                    return "#6761A8";
                }
                else if (ETAT_TOURNEE == TourneeStatus.Started)
                {
                    return "#6fc2e3";
                }
                else if (ETAT_TOURNEE == TourneeStatus.Closed)
                {
                    return "#009B72";
                }
                else
                {
                    return "#2A2D34";
                }
            }
            set
            {
                ETAT_COLOR = value;
                OnPropertyChanged("ETAT_COLOR");
            }
        }
        public string ACOMPILCHEMENT_PERCENT
        {
            get
            {
                if (NBR_CIENTS == 0) return "0 %";

                decimal res = ((NBR_EN_VISITED + NBR_EN_DELEVRED) * 100) / NBR_CIENTS;

                return "Accompli : " + res.ToString("N2") + " %";
            }
        }
        public int NBR_CIENTS { get; set; }
        public int NBR_EN_WAITING { get; set; }
        private int nbr_en_visited { get; set; }
        public int NBR_EN_VISITED
        {
            get { return nbr_en_visited; }
            set
            {
                nbr_en_visited = value;
                OnPropertyChanged("NBR_EN_VISITED");
            }
        }
        private int nbr_en_delevred { get; set; }
        public int NBR_EN_DELEVRED
        {
            get { return nbr_en_delevred; }
            set
            {
                nbr_en_delevred = value;
                OnPropertyChanged("NBR_EN_DELEVRED");
            }
        }

        [Ignore]
        public List<View_LIV_TOURNEE_DETAIL> Details { get; set; }
    }

    public partial class LIV_TOURNEE_DETAIL : BASE_CLASS
    {
        [PrimaryKey, AutoIncrement]
        public int ID { get; set; }
        public string CODE_DETAIL { get; set; }
        public string CODE_TOURNEE { get; set; }
        public string CODE_TIERS { get; set; }
        public string CODE_VENTE { get; set; }
        private string code_etat { get; set; }
        public string CODE_ETAT
        {
            get { return code_etat; }
            set
            {
                code_etat = value;
                OnPropertyChanged("CODE_ETAT");
                OnPropertyChanged("DESIGNATION_ETAT");
                OnPropertyChanged("ETAT_COLOR");

            }
        }
        public string CREATED_BY { get; set; }
        public DateTime? CREATED_ON { get; set; }
        public DateTime? MODIFIED_ON { get; set; }
        public string MODIFIED_BY { get; set; }
        public double GPS_LATITUDE { get; set; }
        public double GPS_LONGITUDE { get; set; }
        public DateTime? HEURE_DEPART_VISITE { get; set; }
        public DateTime? HEURE_RETOUR_VISITE { get; set; }
        public int MOTIF_NON_LIVRAISON { get; set; }
        public string PROOF_NON_LIVRAISON { get; set; }
        public int VISITE_CATEGORIE { get; set; }
        private TourneeStatus codeEtatVisitie { get; set; }
        public TourneeStatus CODE_ETAT_VISITE
        {
            get
            {
                return codeEtatVisitie;
            }
            set
            {
                codeEtatVisitie = value;
                OnPropertyChanged("CODE_ETAT_VISITE");
                OnPropertyChanged("DESIGNATION_ETAT");
                OnPropertyChanged("ETAT_COLOR");
            }
        }
    }
    public enum TourneeStatus : byte
    {
        Planned = 15,
        Visited = 16,
        Delivered = 17,
        Canceled = 18,
        VisitedNotDelivered = 19,
        NotVisited = 20,
        EnRoute = 21,
        Started = 22,
        Closed = 30
    }
    public partial class View_LIV_TOURNEE_DETAIL : LIV_TOURNEE_DETAIL
    {
        public string FULL_NOM_TIERS { get; set; }
        public decimal SOLDE_TIERS { get; set; }
        public decimal MT_VERSEMENT { get; set; }
        public decimal TOTAL_TTC { get; set; }
        [Ignore]
        public double GPS_LATITUDE_CLIENT { get; set; }
        [Ignore]
        public double GPS_LONGITUDE_CLIENT { get; set; }
        [Ignore]
        public string DESIGNATION_ETAT
        {
            get
            {
                if (CODE_ETAT_VISITE == TourneeStatus.Planned)
                {
                    return AppResources.tourneeStatusPlaned;
                }
                else if (CODE_ETAT_VISITE == TourneeStatus.EnRoute)
                {
                    return AppResources.tourneeStatusInRoute;
                }
                else if (CODE_ETAT_VISITE == TourneeStatus.Visited)
                {
                    return AppResources.tourneeStatusVisited;
                }
                else if (CODE_ETAT_VISITE == TourneeStatus.Delivered)
                {
                    return AppResources.tourneeStatusDelivered;
                }
                else if (CODE_ETAT_VISITE == TourneeStatus.Canceled)
                {
                    return AppResources.tourneeStatusCanceled;
                }
                else if (CODE_ETAT_VISITE == TourneeStatus.VisitedNotDelivered)
                {
                    return AppResources.tourneeStatusVisited;
                }
                else if (CODE_ETAT_VISITE == TourneeStatus.NotVisited)
                {
                    return AppResources.tourneeStatusNotVisited;
                }
                else if (CODE_ETAT_VISITE == TourneeStatus.Started)
                {
                    return AppResources.tourneePopUpStart;
                }
                else if (CODE_ETAT_VISITE == TourneeStatus.Closed)
                {
                    return AppResources.tourneePopUpClose;
                }
                else
                {
                    return "Planifié";
                }
            }
            set
            {
                DESIGNATION_ETAT = value;
                OnPropertyChanged("DESIGNATION_ETAT");
            }

        }
        [Ignore]
        public string ETAT_COLOR
        {
            get
            {
                if (CODE_ETAT_VISITE == TourneeStatus.Planned)
                {
                    return "#2A2D34";
                }
                else if (CODE_ETAT_VISITE == TourneeStatus.EnRoute)
                {
                    return "#7EC384";
                }
                else if (CODE_ETAT_VISITE == TourneeStatus.Visited)
                {
                    return "#7EC384";
                }
                else if (CODE_ETAT_VISITE == TourneeStatus.Delivered)
                {
                    return "#009B72";
                }
                else if (CODE_ETAT_VISITE == TourneeStatus.Canceled)
                {
                    return "#F26430";
                }
                else if (CODE_ETAT_VISITE == TourneeStatus.VisitedNotDelivered)
                {
                    return "#6761A8";
                }
                else if (CODE_ETAT_VISITE == TourneeStatus.NotVisited)
                {
                    return "#6761A8";
                }
                else if (CODE_ETAT_VISITE == TourneeStatus.Started)
                {
                    return "#7EC384";
                }
                else if (CODE_ETAT_VISITE == TourneeStatus.Closed)
                {
                    return "#009B72";
                }
                else
                {
                    return "#2A2D34";
                }
            }
            set
            {
                ETAT_COLOR = value;
                OnPropertyChanged("ETAT_COLOR");
            }
        }
    }

    public partial class View_VTE_VENTE_LIVRAISON : VTE_VENTE_DETAIL
    {
        /// <summary>
        /// Le contenu ancien de View_VTE_VENTE_LIVRAISON
        /// </summary>
        /// 
        //public decimal TOTAL_PAYE { get; set; } // money(19,4)
        //public decimal TOTAL_RESTE { get; set; } // money(19,4)
        //public string OWNER { get; set; } // varchar(200)
        //public string NOM_TIERS { get; set; } // varchar(501)
        //public string ADRESSE_TIERS { get; set; } // varchar(500)
        //public string NUM_ASSURE { get; set; } // varchar(12)
        //public string TEL1_TIERS { get; set; } // varchar(50)
        //public string TEL2_TIERS { get; set; } // varchar(50)
        //public string DESIGN_MODE { get; set; } // varchar(300)      
        //public decimal SOLDE_TIERS_AT_VALIDAT { get; set; } // money(19,4)
        //public decimal TOTAL_ENCAISS_AT_VALIDAT { get; set; } // money(19,4)
        //public DateTime? DATE_MAX_ARCHIVE { get; set; }
        //public string NOM_MEDECIN { get; set; }
        //public string DESIGNATION_WILAYA_DELIVRE { get; set; }
        //public string DATE_DELIVRE_CARTE { get; set; }
        //public string N_CARTE { get; set; }
        //public string TITRE_VENTE { get; set; } // varchar(300)
        //public string TYPE_DOC { get; set; }
        //public decimal TOTAL_HT_REEL { get; set; }
        //public decimal TOTAL_PPA_REEL { get; set; }
        //public decimal TOTAL_SHP_REEL { get; set; }
        //public decimal TOTAL_REMISE_REEL { get; set; }
        //public decimal TOTAL_TVA_REEL { get; set; }
        //public decimal TOTAL_TTC_REEL { get; set; }
        //public decimal TOTAL_PAYE_REEL { get; set; }
        //public decimal TOTAL_RESTE_REEL { get; set; }
        //public string DESIGN_MOTIF_LIVRAISON { get; set; }
        //public string TYPE_TIERS { get; set; }
        //public decimal TOTAL_TTC_SANS_REMISE { get; set; }
        //public DateTime? DATE_PAYE_CREANCE { get; set; }
        //public string DESIGNATION_UNITE { get; set; }
        //public decimal QTE_TOTAL_VENTE { get; set; }
        //public decimal QTE_TOTAL_IMPORTER { get; set; }
        //public decimal QTE_REST_FACTURER { get; set; }
        //// bon de livraison facturée
        //public bool LIVRAISON_FACTUREE { get; set; }

        public string IMAGE_URL { get; set; }
        public string NUM_VENTE { get; set; } // varchar(32)
        public DateTime? DATE_VENTE { get; set; } // datetime(3)
        public string NOM_TIERS { get; set; } // varchar(501)
        public string CODE_TIERS { get; set; } // varchar(32)
        public string TYPE_VENTE { get; set; } // char(2)
        public decimal QTE_RETOUR { get; set; }
        public decimal QTE_RESTE_RETOUR { get; set; }
        public string CODE_ORIGINE { get; set; } // varchar(32)
        public decimal TAUX_MARGE { get; set; } // money(19,4)
        public string OWNER { get; set; } // varchar(200)
        public string DESIGNATION_PRODUIT { get; set; } // varchar(754)
        public string CODE_MAGASIN { get; set; } // varchar(10)
        public DateTime? DATE_PEREMPTION { get; set; }
        public string CODE_BARRE { get; set; } // nvarchar(250)
        public string CODE_BARRE_LOT { get; set; } // nvarchar(1500)
        public int TYPE_VALIDATION { get; set; } // int(10)
        public string CODE_EMPLACEMENT { get; set; } // varchar(10)
        public string DESIGN_EMPLACEMENT { get; set; } // varchar(10)
        public string LOT { get; set; } // varchar(50)
        public decimal QTE_STOCK { get; set; } // numeric(18,2)
        public decimal PPA { get; set; } // money(19,4)
        public decimal SHP { get; set; } // money(19,4)
        public string DESIGN_MAGASIN { get; set; } // varchar(100)
        public string REF_PRODUIT { get; set; } // nvarchar(250)
        public decimal PRIX_ACH_HT { get; set; } // money(19,4)
        public decimal PRIX_VENTE { get; set; } // money(19,4)
        public short TYPE_PRODUIT { get; set; } // smallint(5)
        public decimal TARIF { get; set; } // money(19,4)
        public string CODE_LABO { get; set; } // smallint(5)
        public string CODE_FORME { get; set; } // varchar(20)
        public string DESIGN_LABO { get; set; } // varchar(2500)
        public string DESIGN_FORME { get; set; } // varchar(100)
        public string UNITE { get; set; } // varchar(50)
        public string CONDIT { get; set; } // varchar(50)
        public string DOSAGE { get; set; } // varchar(50)
        public string CODE_CNAS { get; set; }
        public bool PENDING { get; set; }
        public decimal PENDING_QUANTITY { get; set; }
        public bool EXON_TVA { get; set; }
        public decimal QTE_STOCK_PRODUIT { get; set; } // numeric(18,2)
        public string DESIGNATION_UNITE { get; set; }
        public string DESIGNATION_UNITE2 { get; set; }
        public string UNITE_AFFICHER { get; set; }
        public decimal QTE_AFFICHER { get; set; }
        public decimal PU_AFFICHER { get; set; }

        // pour que le detail facture chifa indique esque la ligne destocké ou  non 
        // elle a la valeur inverse de la valeur ignoree destockage dans la fenaitre improtation chifa
        public bool DESTOCKER { get; set; }
        public string DESIGNATION_CVM { get; set; }
        public string CODE_MEDIC { get; set; }
        public string N_ORDRE_PSYCHO_ENTET { get; set; }
        public DateTime? DATE_SOINS { get; set; }
        public string CODE_MEDECIN { get; set; }

        public string TYPE_DOC { get; set; }
        public string ID { get; internal set; }
        public string VenteID { get; internal set; }
        public bool HAS_NEW_ID_STOCK { get; set; } = false; // varchar(2500)

        private decimal mt_ttc { get; set; }
        public decimal MT_TTC
        {
            get
            {
                return mt_ttc;
            }
            set
            {
                mt_ttc = value;
                OnPropertyChanged("MT_TTC");
            }
        }

        private List<View_BSE_PRODUIT_AUTRE_UNITE> unitesList { get; set; }
        [Ignore]
        public List<View_BSE_PRODUIT_AUTRE_UNITE> UnitesList
        {
            get
            {
                return unitesList;
            }
            set
            {
                unitesList = value;
                OnPropertyChanged("UnitesList");
            }
        }

    }

    #endregion
}