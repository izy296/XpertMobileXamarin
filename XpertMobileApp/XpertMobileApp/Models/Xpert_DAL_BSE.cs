using SQLite;
using System;
using System.Collections.Generic;
using System.Text;

namespace XpertMobileApp.Models
{
    public partial class BSE_MAGASINS
    {
        public string CODE { get; set; }
        public string DESIGNATION { get; set; }
        public bool AUTORISER_VENTE { get; set; } // bit
    }
    public partial class View_BSE_MAGASIN : BSE_MAGASINS
    {
        public string CODE_MAGASIN { get; set; } // varchar(10)
        public string DESIGN_MAGASIN { get; set; } // varchar(100)
    }

    public partial class BSE_TIERS_TYPE
    {
        public string CODE_TYPE { get; set; }

        public string DESIGNATION_TYPE { get; set; }
    }

    public partial class BSE_PRODUIT_AUTRE_UNITE_XCOM
    {
        // implementer pour l'appele api SyncProduiteUnite
    }

    public partial class BSE_PRODUIT_AUTRE_UNITE
    {
        public string CODE_DETAIL { get; set; }
        public string CODE_PRODUIT { get; set; }
        public string CODE_UNITE { get; set; }
        public decimal COEFFICIENT { get; set; }
        public decimal PRIX_VENTE { get; set; }
        public decimal PRIX_ACHAT { get; set; }
        public string CODE_BARRE_UNITE { get; set; }
        public bool DEFAULT_FAVORIS { get; set; }
        public int CPT_CB_UNITE { get; set; }// int auto inc+1 
        public bool AUTRE_UNITE_PRODUIT_PUBLIC { get; set; }
        public bool SYNCHRONISE { get; set; }
        public DateTime? DATE_SYNCHRONISATION { get; set; }//[datetime] NULL

    }

    public partial class View_BSE_PRODUIT_AUTRE_UNITE : BSE_PRODUIT_AUTRE_UNITE
    {
        public string DESIGNATION_UNITE { get; set; }
        public string DESIGNATION_PRODUIT { get; set; }
        private decimal selectedQUANTITE { get; set; }
        [Ignore]
        public Decimal SelectedQUANTITE
        {
            get
            {
                return SelectedQUANTITE;
            }
            set
            {
                selectedQUANTITE = value;
            }
        }
    }

    public partial class View_BSE_PRODUIT_UNITE_COEFFICIENT
    {
        public string CODE_DETAIL { get; set; }
        public string CODE_UNITE_1 { get; set; } //[varchar](100 ) NOT NULL,
        public string CODE_UNITE_2 { get; set; }
        public decimal COEFFICIENT { get; set; }
        public string DESIGNATION_UNITE_1 { get; set; }
        public string DESIGNATION_UNITE_2 { get; set; }
    }

    public partial class BSE_PRODUIT_PRIX_VENTE
    {
        // declaration de type BSE_PRODUIT_PRIX_VENTE pour la sycnhronisation sqlite
    }

    public partial class View_BSE_PRODUIT_PRIX_VENTE
    {
        public string CODE_DETAIL { get; set; }
        public string CODE_PRODUIT { get; set; }
        public string CODE_FAMILLE { get; set; }
        public int TYPE_PRIX { get; set; }
        public decimal VALEUR { get; set; }
        public decimal VALEUR_ACHAT { get; set; }
        public bool PRODUIT_PRIX_VENTE_PUBLIC { get; set; }
        public bool SYNCHRONISE { get; set; }
        public DateTime? DATE_SYNCHRONISATION { get; set; }
        public string DESIGN_FAMILLE { get; set; }
        public string DESIGNATION_TYPE_PRIX { get; set; }
    }
}

