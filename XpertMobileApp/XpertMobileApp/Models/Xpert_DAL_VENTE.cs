using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using SQLite;
using SampleBrowser.SfChart;

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
        public int ID { get; set; }
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
                    SetProperty(ref tOTAL_TTC, value);
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
        public int TYPE_PRIX_VENTE { get; set; } // int(10)
        public DateTime? CREATED_ON { get; set; } // datetime(3)
        public string CREATED_BY { get; set; } // varchar(200)
        public DateTime? MODIFIED_ON { get; set; } // datetime(3)
        public string MODIFIED_BY { get; set; } // varchar(200)
        public string NOTE_VENTE { get; set; } // nvarchar(-1)
        public string CODE_MOTIF { get; set; }
        public string CODE_BARRE { get; set; }
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

        #region Validation vente
        [Ignore]
        public List<View_VTE_VENTE_LOT> Details { get; set; }
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
        public decimal OLD_SOLDE { get; set; }
        public decimal NEW_SOLDE { get; set; }

        #endregion
    }

    public class VIEW_FIDELITE_INFOS
    {
        public string CODE_CARD { get; set; }
        public decimal POINTS_USED { get; set; }
        public decimal MT_POINTS_USED { get; set; }
        public decimal MAX_MT_POINTS { get; set; }
    }

    public partial class View_VTE_COMMANDE : View_VTE_VENTE
    {
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
    }

    public partial class View_CFA_MOBILE_FACTURE : FACTURE_CHIFA
    {
        public string NOMC_TIERS { get; set; }
        public string NUM_ASSURE { get; set; }
        public decimal MONT_ACHAT { get; set; }
        public decimal MONT_TOTAL { get; set; }
        public string DESIGN_ETAT { get; set; }
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
    }
    public partial class View_LIV_TOURNEE : LIV_TOURNEE
    {
        public string NUM_SECTEUR { get; set; }
        public string NOM_SECTEUR { get; set; }
        public string DESIGNATION_MAGASIN { get; set; }
        public string NOM_VENDEUR { get; set; }
        public string ETAT_COLOR { get; set; }
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
        public int NBR_EN_VISITED { get; set; }
        public int NBR_EN_DELEVRED { get; set; }


        public decimal TOTAL_PaiementCredit { get; set; } // money(19,4)
        public decimal TOTAL_Vente { get; set; } // money(19,4)
        public decimal TOTAL_TOURNEE { get; set; } // money(19,4)
        public decimal Total_Credit_Journee { get; set; } // money(19,4)

        public decimal TOTAL_CREDIT_TIERS { get; set; } // money(19,4)
        public decimal TOTAL_STOCK_AVANT { get; set; } // money(19,4)
        public decimal TOTAL_STOCK_APRES { get; set; } // money(19,4)
        public decimal TOTAL_RETOUR_STOCK { get; set; } // money(19,4)



        private decimal GetPercent(decimal val, decimal total)
        {
            if (total == 0) return 0;

            return (val * 100) / total;
        }

        public List<ChartDataModel> Data1
        {
            get
            {
                decimal total = TOTAL_RETOUR_STOCK + TOTAL_Vente + TOTAL_PaiementCredit ;

                var data = new List<ChartDataModel>();
                data.Add(new ChartDataModel("Ventes Cash", Convert.ToDouble(GetPercent(TOTAL_Vente, total))));
                data.Add(new ChartDataModel("Paiement crédit", Convert.ToDouble(GetPercent(TOTAL_PaiementCredit, total))));
                data.Add(new ChartDataModel("Retour", Convert.ToDouble(GetPercent(TOTAL_RETOUR_STOCK, total))));
                return data;
            }
        }
    }

    public partial class LIV_TOURNEE_DETAIL : BASE_CLASS
    {
        [PrimaryKey, AutoIncrement]
        public int ID { get; set; }
        public string CODE_DETAIL { get; set; }
        public string CODE_TOURNEE { get; set; }
        public string CODE_TIERS { get; set; }
        public string CODE_VENTE { get; set; }
        public string CODE_ETAT { get; set; }
        public string CREATED_BY { get; set; }
        public DateTime? CREATED_ON { get; set; }
        public DateTime? MODIFIED_ON { get; set; }
        public string MODIFIED_BY { get; set; }
        public double GPS_LATITUDE { get; set; }
        public double GPS_LONGITUDE { get; set; }

    }
    public static class TourneeStatus
    {
        public static string EnAttente { get { return "15"; } }
        public static string Visited { get { return "16"; } }
        public static string Delevred { get { return "17"; } }
    }
    public partial class View_LIV_TOURNEE_DETAIL : LIV_TOURNEE_DETAIL
    {
        public string NOM_TIERS { get; set; }
        public decimal SOLDE_TIERS { get; set; }
        public string DESIGNATION_ETAT { get; set; }

        public string ETAT_COLOR { get; set; }
    }
    #endregion
}