using System;
using System.Collections.Generic;
using System.Linq;
using Xpert.Common.WSClient.Model;
using XpertMobileApp.Helpers;
using XpertMobileApp.Models;

namespace XpertMobileApp.DAL
{

    public static class StausAchRecDoc
    {
        public static string EnAttente { get { return "16"; } }
        public static string EnCours { get { return "17"; } }
        public static string Termine { get { return "18"; } }
        public static string Cloture { get { return "19"; } }
    }

    public partial class ACH_DOCUMENT : BASE_CLASS
    {
        public string CODE_DOC { get; set; } // varchar(50)
        public string NUM_DOC { get; set; } // varchar(20)
        public DateTime? DATE_DOC { get; set; } // datetime(3)
        public string CODE_TIERS { get; set; } // varchar(32)
        public string CODE_COMMANDE { get; set; } // varchar(50)
        public string CODE_FACTURE { get; set; } // varchar(50)
        public string CODE_AVOIR { get; set; } // varchar(50)
        public string TYPE_DOC { get; set; } // varchar(4)
        public string CODE_MAGASIN { get; set; } // varchar(10)
        public string MODE_REGLEMENT { get; set; } // varchar(1)
        public string REF_TIERS { get; set; } // varchar(50)
        public decimal TOTAL_HT { get; set; } // money(19,4)
        public decimal TOTAL_TVA { get; set; } // money(19,4)
        public decimal TOTAL_PPA { get; set; } // money(19,4)
        public decimal TOTAL_SHP { get; set; } // money(19,4)
        public decimal TOTAL_RISTOURNE { get; set; } // money(19,4)
        public decimal TOTAL_MARGE { get; set; } // money(19,4)
        public decimal MT_TIMBRE { get; set; } // money(19,4)

        //public decimal TOTAL_PAYE { get; set; } // money(19,4)
        public decimal TOTAL_PPA_UG { get; set; } // money(19,4)
        public decimal TOTAL_SHP_UG { get; set; } // money(19,4)
        public decimal TOTAL_FOURN_HT { get; set; } // money(19,4)
        public decimal TOTAL_FOURN_TVA { get; set; } // money(19,4)
        public decimal TOTAL_FOURN_TTC { get; set; } // money(19,4)
        public decimal TOTAL_FOURN_SHP { get; set; } // money(19,4)
        public decimal TOTAL_FOURN_PPA { get; set; } // money(19,4)
        public decimal TOTAL_FOURN_HT_REMISE { get; set; } // money(19,4)
        public DateTime? DATE_ECHEANCE { get; set; } // datetime(3)
        public string NOTE_DOC { get; set; } // nvarchar(-1)
        public int SENS_DOC { get; set; } // int(10)
        public decimal RISTOUNE_FACT { get; set; } // money(19,4)
        public decimal TOTAL_VENTE { get; set; } // money(19,4)
        public string STATUS_DOC { get; set; } // varchar(10)
        public DateTime? CLOTURE_ON { get; set; } // datetime(3)
        public string CLOTURED_BY { get; set; } // varchar(200)
        public DateTime? CREATED_ON { get; set; } // datetime(3)
        public string CREATED_BY { get; set; } // varchar(200)
        public DateTime? MODIFIED_ON { get; set; } // datetime(3)
        public string MODIFIED_BY { get; set; } // varchar(200)
        public string EXERCICE { get; set; } // varchar(4)
        public bool UPDATE_STOCK { get; set; }
        public string CODE_MOTIF { get; set; }
        public bool INCLUDE_UG { get; set; }
        public bool PPA_BY_PRIXVENTE { get; set; }
        public string MODE_CAL_MT_ECHANGE { get; set; }

        public decimal PESEE_ENTREE { get; set; }
        public decimal PESEE_SORTIE { get; set; }

        public string IMMATRICULATION { get; set; }
        public DateTime? DATE_PESEE_ENTREE { get; set; }
        public DateTime? DATE_PESEE_SORTIE { get; set; }
        public string CODE_CHAUFFEUR { get; set; }
        public string CODE_UNITE { get; set; }

        private decimal tOTAL_TTC { get; set; }
        public decimal TOTAL_TTC {
            get
            {
                return tOTAL_TTC;
            }
            set
            {
                tOTAL_TTC = value;
                OnPropertyChanged("TOTAL_TTC");
            }
        }
    }

    public partial class View_ACH_DOCUMENT : ACH_DOCUMENT
    {
        private string tIERS_NomC;
        public string TIERS_NomC
        {
            get
            {
                return tIERS_NomC;
            }
            set
            {
                tIERS_NomC = value;
                OnPropertyChanged("TIERS_NomC");
            }
        }

        public string DESIGN_MODE_REG { get; set; } // varchar(300)
        public string DESIGNATION_MAGASIN { get; set; } // varchar(100)
        public decimal TOTAL_RESTE { get; set; }
        public int LITIGES { get; set; }
        public string DESIGN_MOTIF { get; set; }
        public decimal TOTAL_PAYE { get; set; }
        public string CODE_FAMILLE_TIERS { get; set; }

        // Totaux avec Valeurs réels
        public decimal TOTAL_TTC_REEL { get; set; }
        public decimal TOTAL_RESTE_REEL { get; set; }
        public decimal TOTAL_HT_REEL { get; set; }
        public decimal TOTAL_MARGE_REEL { get; set; }
        public decimal TOTAL_PPA_REEL { get; set; }
        public decimal TOTAL_PPA_UG_REEL { get; set; }
        public decimal TOTAL_RISTOURNE_REEL { get; set; }
        public decimal TOTAL_SHP_REEL { get; set; }
        public decimal TOTAL_SHP_UG_REEL { get; set; }
        public decimal TOTAL_TVA_REEL { get; set; }
        public decimal TOTAL_VENTE_REEL { get; set; }
        public string DESIGN_FAMILLE_TIERS { get; set; }
        public string DESIGNATION_STATUS { get; set; }

        // Mobile extension
        private string nOM_CHAUFFEUR;
        public string NOM_CHAUFFEUR
        {
            get
            {
                return nOM_CHAUFFEUR;
            }
            set
            {
                nOM_CHAUFFEUR = value;
                OnPropertyChanged("NOM_CHAUFFEUR");
            }
        }

        public override string ToString()
        {
            return "N° " + NUM_DOC;
        }

        public string TITLE_DOCUMENT
        {
            get
            {
                return DALHelper.GetTypeDesignation(this.TYPE_DOC);
            }
        }

        private bool pSEE_UPDATED;
        public bool PSEE_UPDATED
        {
            get
            {
                return pSEE_UPDATED;
            }
            set
            {
                pSEE_UPDATED = value;

                OnPropertyChanged("PESEE_BRUTE");
                OnPropertyChanged("TOTAL_TTC");
            }
        }

        public decimal PESEE_BRUTE
        {
            get
            {
                return PESEE_ENTREE - PESEE_SORTIE;
            }
        }

        public List<View_ACH_DOCUMENT_DETAIL> Details;

    }

    public partial class ACH_DOCUMENT_DETAIL : BASE_CLASS
    {
        public string CODE_DOC_DETAIL { get; set; } // varchar(32)
        public string CODE_ORIGINE_DETAIL { get; set; } // varchar(32)
        public string CODE_DOC { get; set; } // varchar(50)
        public int? ID_STOCK { get; set; } // int(10)
        public string CODE_PRODUIT { get; set; } // varchar(32)
        public string LOT { get; set; } // varchar(50)
        public DateTime? DATE_PEREMPTION { get; set; } // datetime(3)
        public string CODE_MAGASIN { get; set; } // varchar(20)
        public string CODE_EMPLACEMENT { get; set; } // varchar(50)

        public decimal QTE_BONUS { get; set; } // numeric(19,2)
        public decimal QTE_RECUE { get; set; } // numeric(19,2)
        public decimal PRIX_UNITAIRE { get; set; } // money(19,4)
        public decimal PPA { get; set; } // money(19,4)
        public decimal SHP { get; set; } // money(19,4)
        public decimal PRIX_VENTE { get; set; } // money(19,4)
        public decimal TAUX_RISTOURNE { get; set; } // money(19,4)
        public decimal TAUX_BONUS { get; set; } // money(19,4)
        public decimal TAUX_TVA { get; set; } // money(19,4)
        public decimal COUT_ACHAT { get; set; } // money(19,4)
        public decimal TAUX_MARGE { get; set; } // money(19,4)
        public decimal PRIX_TTC { get; set; } // money(19,4)
        public decimal MT_HT { get; set; } // money(19,4)
        public decimal MT_RISTOURNE { get; set; } // money(19,4)
        public decimal MT_TVA { get; set; } // money(19,4)
        public decimal MT_RISTOURNE_FACT { get; set; } // money(19,4)

        public decimal MT_VENTE { get; set; } // money(19,4)
        public decimal MT_PPA { get; set; } // money(19,4)
        public decimal MT_PPA_UG { get; set; } // money(19,4)
        public decimal MT_SHP { get; set; } // money(19,4)
        public decimal MT_SHP_UG { get; set; } // money(19,4)
        public string CODE_BARRE_LOT { get; set; } // varchar(200)
        public int ORDRE { get; set; } // int(10)
        public DateTime? CREATED_ON { get; set; } // datetime(3)
        public string CREATED_BY { get; set; } // varchar(200)
        public DateTime? MODIFIED_ON { get; set; } // datetime(3)
        public string MODIFIED_BY { get; set; } // varchar(200)
        public decimal TAUX_DECHET { get; set; }

        private decimal qUANTITE_DECHETS;
        public decimal QUANTITE_DECHETS
        {
            get
            {
                return qUANTITE_DECHETS;
            }
            set
            {
                qUANTITE_DECHETS = value;
                OnPropertyChanged("QUANTITE_DECHETS");
            }
        }

        // Mobile extension

        public decimal qUANTITE_NET_PRIMAIRE { get; set; }

        public decimal QUANTITE_NET_PRIMAIRE
        {
            get
            {
                return qUANTITE_NET_PRIMAIRE;
            }
            set
            {
                qUANTITE_NET_PRIMAIRE = value;
                OnPropertyChanged("QUANTITE_NET_PRIMAIRE");
            }
        }

        public decimal qUANTITE { get; set; }

        public decimal QUANTITE
        {
            get
            {
                return qUANTITE;
            }
            set
            {
                qUANTITE = value;
                OnPropertyChanged("QUANTITE");
            }
        }

        public bool iS_PRINCIPAL { get; set; }

        public bool IS_PRINCIPAL
        {
            get
            {
                return iS_PRINCIPAL;
            }
            set
            {
                iS_PRINCIPAL = value;
                OnPropertyChanged("IS_PRINCIPAL");
            }
        }

        public string IMAGE_URL
        {
            get
            {
                return App.RestServiceUrl.Replace("api/", "") + string.Format("Images/GetImage?codeProduit={0}", CODE_PRODUIT);
            }
        }

        // public View_ACH_DOCUMENT ParentDoc;

        private decimal mT_TTC;
        public decimal MT_TTC
        {
            get
            {
                return mT_TTC;
            }
            set
            {
                if (mT_TTC != value)
                {
                    mT_TTC = value;
                    OnPropertyChanged("MT_TTC");
                }
            }
        } 

        internal decimal qTE_BRUTE;
        public decimal QTE_BRUTE
        {
            get
            {
                return qTE_BRUTE;
            }
            set
            {
                if (qTE_BRUTE != value)
                {
                    qTE_BRUTE = value;

                    OnPropertyChanged("QTE_BRUTE");
                    OnPropertyChanged("MT_TTC");

                    // ParentDoc.PSEE_UPDATED = true;
                }
            }
        }

    }

    public partial class View_ACH_DOCUMENT_DETAIL : ACH_DOCUMENT_DETAIL
    {
        public string NUM_DOC { get; set; } // varchar(20)
        public DateTime? DATE_DOC { get; set; } // datetime(3)
        public string CODE_TIERS { get; set; } // varchar(32)
        public string TYPE_DOC { get; set; } // varchar(4)
        public string REF_TIERS { get; set; } // varchar(50)
        public string TIERS_NOMC { get; set; } // varchar(501)
        public string DESIGNATION_PRODUIT { get; set; } // varchar(404)
        public decimal QTE_STOCK { get; set; } // numeric(18,2)
        public short TYPE_PRODUIT { get; set; } // smallint(5)
        public decimal TARIF { get; set; } // money(19,4)
        public string CODE_LABO { get; set; } // varchar(32)
        public string CODE_BARRE { get; set; } // nvarchar(250)
        public string CODE_FORME { get; set; } // varchar(20)
        public string UNITE { get; set; } // varchar(50)
        public string CONDIT { get; set; } // varchar(50)
        public string DOSAGE { get; set; } // varchar(50)
        public string DESIGN_LABO { get; set; } // varchar(2500)
        public string DESIGN_FORME { get; set; } // varchar(100)
        public string DESIGN_MAGASIN { get; set; } // varchar(100)
        public string DESIGN_EMPLACEMENT { get; set; } // varchar(100)
        public decimal QTE_MANQUANTS { get; set; } // numeric(38,2)
        public int LITIGES { get; set; } // int(10)
        public string OLD_CB { get; set; }
        public bool UPDATE_STOCK { get; set; }
        public bool INCLUDE_UG { get; set; }
        public bool PPA_BY_PRIXVENTE { get; set; }
        public string CODE_MOTIF { get; set; } // varchar(100)
        public string DESIGNATION_MOTIF { get; set; } // varchar(100)
        public string CODE_FAMILLE_TIERS { get; set; }
        public string DESIGN_FAMILLE_TIERS { get; set; }
        public decimal MT_ACHAT { get; set; }
        public bool PSYCHOTHROPE { get; set; } // tinyint(3)

        // Extension mobile
        private List<View_BSE_EMBALLAGE> embalages;
        public List<View_BSE_EMBALLAGE> Embalages
        {
            get
            {
                return embalages;
            }
            set
            {
                embalages = value;
                // ParentDoc.PSEE_UPDATED = true;

                Nbr_Caisses = Embalages.Sum(x => x.QUANTITE_ENTREE);
                Poids_Caisses = Embalages.Sum(x => x.QUANTITE_ENTREE * x.QUANTITE_UNITE);

                OnPropertyChanged("Nbr_Caisses");
                OnPropertyChanged("Poids_Caisses");
                OnPropertyChanged("MT_TTC");
            }
        }

        public decimal Nbr_Caisses { get; set; }
        private decimal Poids_Caisses { get; set; }

        internal void SetPeseeBrute(decimal v)
        {
            if(this.qTE_BRUTE != v)
            { 
                this.qTE_BRUTE = v;
                OnPropertyChanged("QTE_BRUTE");
            }
        }
    }

    public partial class BSE_TABLE
    {
        public string CODE { get; set; } // varchar(32)
        public string DESIGNATION { get; set; } // char(8) 
        public bool? REINTEGRER_STOCK { get; set; }
        public bool? GENERER_VENTE_COMPTOIR { get; set; }
        public bool? GENERER_VENTE_CHIFA { get; set; }
    }

    public partial class STK_PRODUITS : BASE_CLASS
    {
        public string CODE_PRODUIT { get; set; } // varchar(11)
        public string DESIGNATION { get; set; } // varchar(150)
        public string SHORT_DESIGNATION {
            get
            {
                return DESIGNATION.Truncate(40);
            }
        } 
        public string DOSAGE { get; set; } // varchar(50)
        public string UNITE { get; set; } // varchar(50)
        public string CONDIT { get; set; } // varchar(50)
        public string CODE_DCI { get; set; } // varchar(20)
        public string CODE_FORME { get; set; } // varchar(20)
        public decimal TAUX_TVA { get; set; } // money(19,4)
        public int STOCK_MIN { get; set; } // int(10)
        public int QTE_REAPPRO { get; set; } // int(10)
        public string TYPE_PRODUIT { get; set; } // smallint(5)
        public decimal SHP { get; set; } // money(19,4)
        public decimal TARIF { get; set; } // money(19,4)
        public string CODE_CNAS { get; set; } // varchar(11)
        public string REMB { get; set; } // char(1)
        public string DATE_REMB { get; set; } // varchar(8)
        public string D_NON_REMB { get; set; } // varchar(8)
        public string DESCRIPTION { get; set; } // varchar(2000)
        public decimal MARGE { get; set; } // money(19,4)
        public string SPECIALITE { get; set; } // varchar(350)
        public bool PSYCHOTHROPE { get; set; } // tinyint(3)
        public bool ACTIF { get; set; } // tinyint(3)
        public string CODE_CASNOS { get; set; } // nvarchar(13)
        public string CODE_BARRE { get; set; } // nvarchar(250)
        public string CODE_SOUS_FAMILLE { get; set; } // nvarchar(10)
        public string REFERENCE { get; set; } // nvarchar(250)
        public string CODE_LABO { get; set; } // smallint(5)
        public byte CODE_SPECIALITE { get; set; } // tinyint(3)
        public bool REMB_MILITAIRE { get; set; } // bit
        public string CODE_FAMILLE { get; set; } // nvarchar(50)
        public decimal RTA { get; set; } // money(19,4)
        public string CODE_FOURNISSEUR { get; set; } // varchar(20)
        public decimal PRIX_ACHAT_HT { get; set; } // money(19,4)
        public decimal PRIX_ACHAT_TTC { get; set; } // money(19,4)
        public decimal PRIX_VENTE_HT { get; set; } // money(19,4)
        public decimal PRIX_VENTE_TTC { get; set; } // money(19,4)
        public int? CODE_PRODUIT_CB { get; set; } // int(10)
        public bool IS_STOCKABLE { get; set; }
        public bool RUPTURE { get; set; } // 
        public bool IS_PRINCEPS { get; set; }
        public decimal TARIF_CVM { get; set; } // money(19,4)

        public bool IS_NEW { get; set; }
        public bool SHOW_CATALOG { get; set; }
        public string IMAGE_URL { get; set; }

    }

    public partial class View_STK_PRODUITS : STK_PRODUITS
    {
        public string DESIGNATION_PRODUIT { get; set; } // varchar(404)
        public string DESIGN_DCI { get; set; } // nvarchar(50)
        public decimal QTE_STOCK { get; set; } // numeric(18,2)
        public string DESIGN_FORME { get; set; } // 
        public string DESIGN_LABO { get; set; } // 
        public string DESIGN_TYPE { get; set; }
        public string DESIGN_FAMILLE { get; set; } // nvarchar(50)
        public decimal TAUX_DECHET { get; set; } // numeric(18,2)
        

        private decimal selectedQUANTITE;
        public decimal SelectedQUANTITE
        {
            get
            {
                return selectedQUANTITE;
            }
            set
            {
                selectedQUANTITE = value;
                OnPropertyChanged("SelectedQUANTITE");
                OnPropertyChanged("HasSelectedQUANTITE");
            }
        }

        private bool hasSelectedQUANTITE;
        public bool HasSelectedQUANTITE
        {
            get
            {
                return SelectedQUANTITE > 0;
            }
        }


        public string TRUNCATED_DESIGNATION
        {
            get
            {
                return DESIGNATION.Truncate(40);
            }
        }

        public decimal OLD_PRICE
        {
            get
            {
                decimal red = (PRIX_VENTE_HT * Reduction) / 100;
                return PRIX_VENTE_HT + red;
            }
        }

        public string Ratings { get; set; } = "1500 Votes";
        public int Reduction { get; set; } = 15;
        public decimal ReviewValue { get; set; } = (decimal)4.5;
    }

    public class View_AssistantCommandes : BASE_CLASS
    {
        public decimal Quantity { get; set; }    // int(10)
        public string CODE_PRODUIT { get; set; } // varchar(11)
        public string REFERENCE { get; set; }    // nvarchar(250)
        public string PRODUIT { get; set; }      // varchar(1)
        public decimal STOCK_MIN { get; set; }   // int(10)
        public decimal QTE_STOCK { get; set; }   // numeric(38,2)
        public decimal QTE_VENTECOMPT { get; set; } // numeric(38,2)
        public decimal QTE_CHIFFA { get; set; }     // money(19,4)
        public decimal QTE_PHARM { get; set; }      // numeric(38,2)
        public decimal QTE_MILITAIRE { get; set; }  // numeric(38,2)
        public decimal QTE_INSTANCE { get; set; }   // numeric(38,2)
        public decimal QTE_VENTE { get; set; }      // numeric(38,2)
        public decimal QTE_CONSMOY { get; set; }    // numeric(38,6)
        public decimal QTE_ARRIVAGE { get; set; }   // int(10)
        public decimal QTE_MANQUANTS { get; set; }  // int(10)
        public decimal QTE_ROTATION { get; set; }   // int(10)
        public decimal PRIX_UNITAIRE { get; set; }  // money(19,4)
        public bool Commande { get; set; }
        public decimal UG { get; set; }
        public decimal Ristoune { get; set; }
        public bool RUPTURE { get; set; }
        public DateTime? DATE_MAX_FIRST_MANQUANT { get; set; } // datetime(3)
        public decimal QTE_RESERVED { get; set; }

        private decimal? qTE_COMMANDE = null;
        public decimal? QTE_COMMANDE
        {
            get
            {
                if (qTE_COMMANDE == null)
                {
                    decimal QteDiffStock = this.STOCK_MIN - (this.QTE_STOCK + this.QTE_ARRIVAGE);
                    decimal qteRotation = this.QTE_ROTATION;
                    decimal qteManquants = this.QTE_MANQUANTS;

                    decimal maxValue = qteRotation > QteDiffStock ? qteRotation : QteDiffStock;
                    maxValue = maxValue > qteManquants ? maxValue : qteManquants;
                    qTE_COMMANDE = maxValue;
                }

                return qTE_COMMANDE;
            }
            set
            {
                qTE_COMMANDE = value;
            }
        }

        private decimal? qTE_COMMANDE_REF = null;
        public decimal? QTE_COMMANDE_REF
        {
            get
            {
                if (qTE_COMMANDE_REF == null)
                {
                    decimal QteDiffStock = this.STOCK_MIN - (this.QTE_STOCK + this.QTE_ARRIVAGE);
                    decimal qteRotation = this.QTE_ROTATION;
                    decimal qteManquants = this.QTE_MANQUANTS;

                    decimal maxValue = qteRotation > QteDiffStock ? qteRotation : QteDiffStock;
                    maxValue = maxValue > qteManquants ? maxValue : qteManquants;
                    qTE_COMMANDE_REF = maxValue;
                }

                return qTE_COMMANDE_REF;
            }
            set
            {
                qTE_COMMANDE_REF = value;
            }
        }

        /// <summary>
        /// Nombre de jours couverts par le stock actuel
        /// </summary>
        public decimal CovredDays
        {
            get
            {
                if (QTE_CONSMOY != 0)
                    return QTE_STOCK / QTE_CONSMOY;
                else
                    return 0;
            }
        }

        private DateTime? peremption = null;
        public DateTime? Peremption
        {
            get
            {
                return peremption;
            }

            set
            {
                peremption = value;
            }
        }

        public override string ToString()
        {
            return "N° " + CODE_PRODUIT;
        }
    }

    public partial class BSE_DOCUMENT_STATUS
    {
        public string CODE_STATUS { get; set; } // int(10)
        public string NAME { get; set; } // varchar(50)
        public string DESCRIPTION { get; set; } // varchar(500)
        public string TYPE { get; set; } // varchar(50)
    }

    public partial class ACH_MANQUANTS : BASE_CLASS
    {
        public int? CODE_MANQUANT { get; set; } // int(10)
        public string CODE_TYPE { get; set; } // varchar(30)
        public string CODE_PRODUIT { get; set; } // varchar(30)
        public decimal QUANTITE { get; set; } // numeric(19,2)
        public DateTime? DATE_LIVRAISON_MAX { get; set; } // datetime(3)
        public string NOTE { get; set; } // varchar(300)
        public string EXERCICE { get; set; } // char(4)
        public bool TREATED { get; set; } // bit
        public DateTime? CREATED_ON { get; set; } // datetime(3)
        public string CREATED_BY { get; set; } // varchar(200)
        public DateTime? MODIFIED_ON { get; set; } // datetime(3)
        public string MODIFIED_BY { get; set; } // varchar(200)
        public string Etat { get; set; } // char(3)
        public bool AUTOCREATED { get; set; } // bit
    }

    public partial class View_ACH_MANQUANTS : ACH_MANQUANTS
    {
        public string PRODUIT_REFERENCE { get; set; } // varchar(201)
        public string NOM_USER { get; set; } // varchar(201)
        public string TYPE_NAME { get; set; } // varchar(50)
        public string TYPE_DESCRIPTION { get; set; } // varchar(500)
        public string DESIGNATION_PRODUIT { get; set; } // varchar(303)
        public decimal ARRIVAGE { get; set; } // numeric(19,2)
        public DateTime? DATE_FIRST_ARRIVAGE { get; set; } // datetime(3)
        public decimal QTE_STOCK { get; set; } // numeric(19,2)
        public decimal QTE_REFERENCE { get; set; } // numeric(19,2)
        public short TYPE_PRODUIT { get; set; } // smallint(5)
        public string DESIGNATION_ETAT { get; set; } // char(3)
        public decimal STOCK_MIN { get; set; }
        public string DESIGNATION_TYPE_PRODUIT { get; set; }
    }

}
