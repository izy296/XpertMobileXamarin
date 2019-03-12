using System;

namespace XpertMobileApp.DAL
{
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
