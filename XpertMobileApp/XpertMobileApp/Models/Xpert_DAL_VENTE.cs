using System;
using System.Collections.Generic;

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
                tOTAL_TTC = value;
                OnPropertyChanged("TOTAL_TTC");
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
        public decimal TOTAL_RECU { get; set; } // money(19,4)
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
        public string N_ORDRE_PSYCHO { get; set; }
    }

    public partial class View_VTE_VENTE : VTE_VENTE
    {
        public decimal TOTAL_PAYE { get; set; } // money(19,4)
        public decimal TOTAL_RESTE { get; set; } // money(19,4)
        public string NOM_TIERS { get; set; } // varchar(501)
        public string N_CARTE { get; set; }
        public string TITRE_VENTE { get; set; } // varchar(300)
        public string TYPE_DOC { get; set; }

        public override string ToString()
        {
            return "N° " + NUM_VENTE;
        }

        public List<View_VTE_VENTE_PRODUIT> Details { get; set; }
    }

    public partial class VTE_VENTE_DETAIL : BASE_CLASS
    {
        public string CODE_DETAIL { get; set; } // varchar(32)
        public string CODE_VENTE { get; set; } // varchar(32)
        public int? ID_STOCK { get; set; } // int(10)
        public string CODE_PRODUIT { get; set; } // varchar(32)
        public string CODE_BARRE_PRODUIT { get; set; } // varchar(32)
        

        public decimal PRIX_VTE_TTC { get; set; } // money(19,4)
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
        public bool PSYCHOTHROPE { get; set; }
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
                qUANTITE = value;

                MT_TTC = qUANTITE * PRIX_VTE_TTC;

                OnPropertyChanged("QUANTITE");
                OnPropertyChanged("MT_TTC");
            }
        }
    }

    public partial class View_VTE_VENTE_PRODUIT : VTE_VENTE_DETAIL
    {
        public string DESIGNATION_PRODUIT { get; set; } 
        public string IMAGE_URL { get; set; }  
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
}