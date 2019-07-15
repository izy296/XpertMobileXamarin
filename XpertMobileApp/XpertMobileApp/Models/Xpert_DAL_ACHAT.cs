using System;
using Xpert.Common.WSClient.Model;

namespace XpertMobileApp.DAL
{

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
