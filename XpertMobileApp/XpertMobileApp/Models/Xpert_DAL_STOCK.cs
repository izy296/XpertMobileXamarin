using System;
using XpertMobileApp.DAL;

namespace XpertMobileApp.Models
{
    public partial class STK_STOCK : BASE_CLASS
    {
        public int? ID_STOCK { get; set; } // int(10)
        public string CODE_PRODUIT { get; set; } // varchar(50)
        public string CODE_MAGASIN { get; set; } // varchar(50)
        public string LOT { get; set; } // varchar(50)
        public decimal VAL_STK { get; set; } // money(19,4)
        public decimal PRIX_VENTE { get; set; } // money(19,4)
        public decimal PRIX_FOURNISSEUR { get; set; } // money(19,4)
        public decimal PPA { get; set; } // money(19,4)
        public decimal SHP { get; set; } // money(19,4)
        public string CODE_ENTREE { get; set; } // varchar(50)
        public decimal QUANTITE { get; set; } // numeric(18,2)
        public DateTime? DATE_PEREMPTION { get; set; } // datetime(3)

        public string CODE_EMPLACEMENT { get; set; } // varchar(10)
        public string CODE_BARRE_LOT { get; set; } // nvarchar(50)
        public string CODE_DOC { get; set; } // varchar(50)
        public string NUM_DOC { get; set; } // varchar(20)
        public string TYPE_DOC { get; set; } // varchar(10)
        public bool IS_BLOCKED { get; set; } // bit
        public DateTime? CREATED_ON { get; set; } // datetime(3)
        public string CREATED_BY { get; set; } // varchar(200)
        public DateTime? MODIFIED_ON { get; set; } // datetime(3)
        public string MODIFIED_BY { get; set; } // varchar(200)
        public int? TYPE_OPERATION { get; set; } // smallint(5)
        public bool PENDING { get; set; } // bit
        public decimal PENDING_QUANTITY { get; set; } // numeric(18,2)
        public bool GENERATE_BL { get; set; } // {trace Arrivage en instance} :: generer un bon de reception .
        public bool GENERATE_DECAISS { get; set; } // {trace Arrivage en instance} :: generer un rembouresement client .
        public string IMAGE_URL
        {
            get
            {
                return App.RestServiceUrl.Replace("api/", "") + string.Format("Images/GetImage?codeProduit={0}", CODE_PRODUIT);
            }
        }
        public string Expiration
        {
            get
            {
                return DATE_PEREMPTION != null ? "Ex : " + DATE_PEREMPTION.Value.ToString("dd/MM/yyyy") : "";
            }
        }
    }

    public partial class View_STK_STOCK : STK_STOCK
    {
        public decimal OLD_QUANTITE { get; set; } // numeric(18,2)
        public decimal MARGE_VENTE { get; set; } // money(19,4)
        public string REF_PRODUIT { get; set; } // nvarchar(250)
        public string CODE_DCI { get; set; } // varchar(20)
        public short TYPE_PRODUIT { get; set; } // smallint(5)
        public decimal TAUX_TVA { get; set; } // money(19,4)
        public decimal MARGE { get; set; } // money(19,4)
        public string CODE_BARRE { get; set; } // nvarchar(250)
        public string DESIGNATION_PRODUIT { get; set; } // varchar(404)
        public string DESIGN_MAGASIN { get; set; } // varchar(100)
        public string DESIGN_DCI { get; set; } // varchar(500)
        public string DOSAGE { get; set; } // varchar(50)
        public string CODE_FORME { get; set; } // varchar(20)
        public string CODE_CNAS { get; set; } // varchar(11)
        public decimal TARIF { get; set; } // money(19,4)
        public bool AUTORISER_VENTE { get; set; } // bit
        public bool PSYCHOTHROPE { get; set; } // tinyint(3)
        public string DESIGN_EMPLACEMENT { get; set; } // varchar(100)
        public string CODE_LABO { get; set; } // smallint(5)
        public string DESIGN_FORME { get; set; } // varchar(150)
        public string UNITE { get; set; } // varchar(50)
        public string CONDIT { get; set; } // varchar(50)
        public bool IS_STOCKABLE { get; set; } // bit
        public string DESIGN_LABO { get; set; } // varchar(2500)
        public string FACT_BL_LOT { get; set; } // varchar(2500)
        public decimal TARIF_CVM { get; set; } // varchar(2500)
        public bool REMB_MILITAIRE { get; set; } // money(19,4)
        public decimal QTE_STOCK_PRODUIT { get; set; } // numeric(18,2)
        public string CODE_FAMILLE { get; set; } // varchar()
        public string DESIGN_FAMILLE { get; set; } // varchar()
        public decimal COUT_ACHAT { get; set; } // varchar(2500)
    }

    public partial class View_STK_STOCK
    {
        //---- utiliser en cas retour d'une vente 
        public string CODE_MVT_DETAIL { get; set; } // varchar(32)
        public decimal QTE_STOCK { get; set; } // varchar(32)

        public DateTime? DATE_VENTE { get; set; } // varchar(32)
        //--------------------------
        public bool LOT_BLOQUE
        {
            get
            {
                return this.IS_BLOCKED != false;
            }
            set { }
        }

        public decimal QTE_RESTE_RETOUR { get; set; }
        public decimal QTE_RETOUR { get; set; }
        public decimal QTE_CALC { get; set; }
        public bool IS_VALID { get; set; }
        public string NUM_VENTE { get; set; }
        public decimal MT_VENTE
        {
            get
            {
                return PRIX_VENTE * QUANTITE;
            }
        }
        public decimal TOTAL_PPA
        {
            get
            {
                return PPA * QUANTITE;
            }
        }
        public decimal TOTAL_SHP
        {
            get
            {
                return SHP * QUANTITE;
            }
        }
        public decimal COUT_ACHAT_CALC
        {
            get
            {
                if (QUANTITE > 0 && VAL_STK != 0)
                {
                    return Math.Round((VAL_STK / QUANTITE), 2);
                }
                return PRIX_FOURNISSEUR;
            }
        }
        public bool IS_SERVICE
        {
            get
            {
                return !this.IS_STOCKABLE;
            }
            set
            {
                this.IS_STOCKABLE = !value;
            }
        }
        
    
    }
    public partial class STK_STOCK_RFID
    {
        public string EPC { get; set; }
        public int? ID_STOCK { get; set; }
        public DateTime? CREATED_ON { get; set; } // datetime(3)
        public string CREATED_BY { get; set; } // varchar(200)
        public DateTime? MODIFIED_ON { get; set; } // datetime(3)
        public string MODIFIED_BY { get; set; } // varchar(200)
        public string MACHINE_USER { get; set; }
    }


    public partial class View_STK_STOCK_RFID : STK_STOCK_RFID
    {
        public string DESIGNATION_PRODUIT { get; set; } // varchar(404)
        public string DESIGN_MAGASIN { get; set; }
        public string DESIGN_EMPLACEMENT { get; set; }
        public int QTE_INVENTAIRE { get; set; }
        public string CODE_PRODUIT { get; set; }
        public string CODE_MAGASIN { get; set; }
        public string LOT { get; set; }
        public string NUM_ENTREE { get; set; }
        public decimal PRIX_HT { get; set; }
        public decimal SHP { get; set; }
        public decimal PPA { get; set; }

    }

    public partial class View_STK_INVENTAIRE
    {
        public string NUM_INVENT { get; set; } // varchar(32)
        public string DESIGN_INVENT { get; set; } // 
        public string CODE_MAGASIN { get; set; } // varchar(10)
        public string TYPE_PRODUIT { get; set; } // varchar(1)
        public short ETAT_INVENT { get; set; } // smallint(5)
        public DateTime? DATE_INVENT { get; set; } // 
        public DateTime? DATE_CLOTURE { get; set; } // 
        public decimal TOTAL_HT { get; set; } // 
        public decimal TOTAL_PPA { get; set; } // 
        public decimal TOTAL_SHP { get; set; } // 
        public decimal TOTAL_ECART_HT { get; set; } // 
        public decimal TOTAL_ECART_SHP { get; set; } // 
        public decimal TOTAL_ECART_PPA { get; set; } // 
        public bool IS_OUVERT { get; set; } // bit
        public bool QTE_INV_STOCK { get; set; } // bit
        public bool QTE_NULL_NEG { get; set; } // bit
        public bool MAZ_NON_MODIFIE { get; set; } // bit
        public string EXERCICE { get; set; } // char(4)
        public string DESIGN_MAGASIN { get; set; } // 
        public string MACHINE_USER { get; set; } // varchar(300)
        public decimal TOTAL_ECART_HT_MAZ { get; set; } // 
        public decimal TOTAL_ECART_SHP_MAZ { get; set; } // 
        public decimal TOTAL_ECART_PPA_MAZ { get; set; } // 
        public decimal ECART_GLOBAL_PPA { get; set; }
        public decimal ECART_GLOBAL_SHP { get; set; }
        public decimal ECART_GLOBAL_HT { get; set; }
        public string TYPE_PRODUIT_2 { get; set; } // varchar(MAX)
    }
    public partial class SCANED_RFID
    {
        public string EPC { get; set; }
        public int COUNT { get; set; }
        public string RSSI { get; set; }
    }

    #region Not user from desktop
    public partial class ACH_DOCUMENT_DETAIL_EMBALLAGE11
    {
        public string CODE_DETAIL { get; set; }
        public string CODE_EMBALLAGE { get; set; }
        public string CODE_DETAIL_ACHAT { get; set; }
        public decimal QUANTITE_ENTREE { get; set; }
        public decimal QUANTITE_SORTIE { get; set; }
    }

    public partial class View_ACH_DOCUMENT_DETAIL_EMBALLAGE11 : ACH_DOCUMENT_DETAIL_EMBALLAGE11
    {
        public string DESIGNATION_EMBALLAGE { get; set; }
        public string DESIGNATION_UNITE { get; set; }
        public string CODE_UNITE { get; set; }
        public decimal QUANTITE_UNITE { get; set; }
        public decimal QTE_DEFF { get; set; }
        public bool IS_PRINCIPAL { get; set; }
        public bool IS_INTERNE { get; set; }
    }

    public partial class BSE_EMBALLAGE11
    {

        public string CODE { get; set; }
        public string DESIGNATION { get; set; }
        public string CODE_PRODUIT { get; set; }
        public decimal QUANTITE_UNITE { get; set; }
        public string CODE_UNITE { get; set; }
        public bool IS_INTERNE { get; set; }
    }

    public partial class View_BSE_EMBALLAGE11 : BSE_EMBALLAGE11
    {
        public string DESIGNATION_PRODUIT { get; set; }
        public string DESIGNATION_UNITE { get; set; }
    }
    #endregion

    public partial class BSE_EMBALLAGE : BASE_CLASS
    {
        public string CODE { get; set; }
        public string CODE_EMBALLAGE { get; set; }

        public string DESIGNATION { get; set; }
        public string CODE_PRODUIT { get; set; }
        public decimal QUANTITE_UNITE { get; set; }
        public string CODE_UNITE { get; set; }
        public bool IS_INTERNE { get; set; }
    }

    public partial class View_BSE_EMBALLAGE : BSE_EMBALLAGE
    {
        public string DESIGNATION_PRODUIT { get; set; }
        public string DESIGNATION_UNITE { get; set; }

        public decimal QUANTITE_ENTREE_REEL { get; set; }

        private decimal qUANTITE_ENTREE { get; set; }
        public decimal QUANTITE_ENTREE
        {
            get
            {
                return qUANTITE_ENTREE;
            }
            set
            {
                qUANTITE_ENTREE = value;
                OnPropertyChanged("QUANTITE_ENTREE");
            }
        }

        private decimal qUANTITE_SORTIE { get; set; }
        public decimal QUANTITE_SORTIE
        {
            get
            {
                return qUANTITE_SORTIE;
            }
            set
            {
                qUANTITE_SORTIE = value;
                OnPropertyChanged("QUANTITE_SORTIE");
            }
        }


        private decimal qUANTITE_VIDE { get; set; }
        public decimal QUANTITE_VIDE
        {
            get
            {
                return qUANTITE_VIDE;
            }
            set
            {
                qUANTITE_VIDE = value;
                OnPropertyChanged("QUANTITE_VIDE");
            }
        }

        public decimal QTE_DEFF
        {
            get
            {
                return QUANTITE_ENTREE - QUANTITE_SORTIE;
            }
        }
    }

    public class BSE_CHAUFFEUR
    {
        public string CODE_CHAUFFEUR { get; set; } //[char](20) NOT NULL,
        public string NOM_CHAUFFEUR { get; set; } //[varchar](100 ) NOT NULL,
        public string NUM_PERMIS_CONDUIRE { get; set; }
    }


    public partial class ACH_INFO_ANEX
    {
        public string ID_LIGNE { get; set; }
        public string CODE_DOC { get; set; }
        public string NOM_TIERS { get; set; }
        public decimal QUANTITE_APPORT { get; set; }

        public DateTime? CREATED_ON { get; set; } // datetime(3)
        public string CREATED_BY { get; set; } // varchar(200)
        public DateTime? MODIFIED_ON { get; set; } // datetime(3)
        public string MODIFIED_BY { get; set; } // varchar(200)

    }

    public partial class VIEW_ACH_INFO_ANEX : ACH_INFO_ANEX
    {
        public decimal PRIX_PRODUIT { get; set; }
        public decimal MT_PRODUIT { get; set; }
        public decimal QUANTITE_PRODUITE { get; set; }

        public decimal MT_ANNEX
        {
            get
            {
                return PRIX_PRODUIT * QUANTITE_APPORT;
            }
        }
    }
}
