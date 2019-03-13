using System;

namespace XpertMobileApp.DAL
{

    public partial class STK_PRODUITS : BASE_CLASS
    {
        public string CODE_PRODUIT { get; set; } // varchar(11)
        public string DESIGNATION { get; set; } // varchar(150)
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
        public char REMB { get; set; } // char(1)
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

        public string DESIGNATION_PRODUIT { get; set; } // varchar(404)
        public string DESIGN_DCI { get; set; } // nvarchar(50)
        public decimal QTE_STOCK { get; set; } // numeric(18,2)
        public string DESIGN_FORME { get; set; } // 
        public string DESIGN_LABO { get; set; } // 
        public string DESIGN_TYPE { get; set; }
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

    }
}
