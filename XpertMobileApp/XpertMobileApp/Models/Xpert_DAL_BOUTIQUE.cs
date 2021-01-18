using System;
using System.Collections.Generic;
using System.Text;
using Xpert.Common.WSClient.Model;
using XpertMobileApp.DAL;

namespace XpertMobileApp.DAL
{

    #region mobile
    public partial class COMMANDES 
    {
        public string Title 
        { 
            get 
            { 
                return "Commande N° : " + ID; 
            } 
        }
        
        public string DESIGN_ETAT_COMMANDE
        {
            get
            {
                if(ETAT_COMMANDE == 1)
                    return "En cours";
                else 
                {
                    return "Livrée";
                }
            }
        }

    }


    public partial class View_PRODUITS_DETAILS
    {
        public bool IS_NEW { get; set; } = false;

        public decimal OLD_PRICE
        {
            get
            {
                decimal red = (PRIX_VENTE * Reduction) / 100;
                return PRIX_VENTE + red;
            }
        }

        public string Ratings { get; set; } = "1500 Votes";
        public int Reduction { get; set; } = 15;
        public decimal ReviewValue { get; set; } = (decimal)4.5;

        public string IMAGE_URL
        {
            get
            {
                return App.RestServiceUrl.Replace("api/", "") + string.Format("Images/GetImage?codeImage={0}", CODE_PRODUIT);
            }
        }
    }

    public partial class View_PRODUITS 
    {
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
        public bool IS_NEW { get; set; } = false;
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
                decimal red = (PRIX_VENTE * Reduction) / 100;
                return PRIX_VENTE + red;
            }
        }

        public string Ratings { get; set; } = "1500 Votes";
        public int Reduction { get; set; } = 15;
        public decimal ReviewValue { get; set; } = (decimal)4.5;

        public string CODE_DEFAULT_IMAGE { get; set; }
        public string IMAGE_URL
        {
            get
            {
                return App.RestServiceUrl.Replace("api/", "") + string.Format("Images/GetImage?codeImage={0}", CODE_DEFAULT_IMAGE);
            }
        }
    }

    public partial class View_WishList
    {
        public bool IS_NEW { get; set; } = false;

        public decimal OLD_PRICE
        {
            get
            {
                decimal red = (PRIX_VENTE * Reduction) / 100;
                return PRIX_VENTE + red;
            }
        }

        public string Ratings { get; set; } = "1500 Votes";
        public int Reduction { get; set; } = 15;
        public decimal ReviewValue { get; set; } = (decimal)4.5;

        public string CODE_DEFAULT_IMAGE { get; set; }
        public string IMAGE_URL
        {
            get
            {
                return App.RestServiceUrl.Replace("api/", "") + string.Format("Images/GetImage?codeImage={0}", CODE_DEFAULT_IMAGE);
            }
        }
    }
    
    #endregion

        public partial class PRODUITS : BASE_CLASS
    {
        public string CODE_PRODUIT { get; set; } // varchar(11)
        public string DESIGNATION { get; set; } // varchar(150)
        public string UNITE { get; set; } // varchar(50)
        public decimal TAUX_TVA { get; set; } // money(19,4)
        public int STOCK_MIN { get; set; } // int(10)
        public string TYPE_PRODUIT { get; set; } // smallint(5)
        public string DESCRIPTION { get; set; } // varchar(2000)
        public decimal MARGE { get; set; } // money(19,4)
        public bool ACTIF { get; set; } // tinyint(3)
        public string CODE_BARRE { get; set; } // nvarchar(250)
        public string REFERENCE { get; set; } // nvarchar(250)
        public string CODE_FAMILLE { get; set; } // nvarchar(50)
        public decimal PRIX_ACHAT_HT { get; set; } // money(19,4)
        public decimal PRIX_ACHAT_TTC { get; set; } // money(19,4)
        public decimal PRIX_VENTE_HT { get; set; } // money(19,4)
        public decimal PRIX_VENTE { get; set; } // money(19,4)
        public int? CODE_PRODUIT_CB { get; set; } // int(10)
        public bool IS_STOCKABLE { get; set; }
        public string CODE_UNITE_ACHAT { get; set; }
        public string CODE_UNITE_VENTE { get; set; }
        public bool SHOW_CATALOG { get; set; }
        public byte FLAG_FAVORIS { get; set; }
        public byte ORDRE_FAVORIS { get; set; }
        public string DESIGNATION_PRODUIT { get; set; } // varchar(404)
    }

    public partial class View_PRODUITS : BASE_CLASS//: PRODUITS
    {
        public string CODE_PRODUIT { get; set; } // varchar(11)
        public string DESIGNATION { get; set; } // varchar(150)
        public decimal QTE_STOCK { get; set; } // numeric(18,2)
        public decimal PRIX_VENTE { get; set; } // money(19,4)
        public byte[] IMAGES { get; set; } // varchar(50)
        public List<string> ImageList { get; set; }
        public decimal NOTE { get; set; }
        public decimal Quantity { get; set; } // numeric(18,2)
        public bool Wished { get; set; }
    }

    public partial class View_PRODUITS_DETAILS //: View_PRODUITS
    {
        public string CODE_PRODUIT { get; set; } // varchar(11)
        public string DESIGNATION { get; set; } // varchar(150)
        public decimal QTE_STOCK { get; set; } // numeric(18,2)
        public decimal PRIX_VENTE { get; set; } // money(19,4)
        public byte[] IMAGES { get; set; } // varchar(50)

        public string DESCRIPTION { get; set; } // varchar(2000)
 
    }

    public partial class PANIER
    {
        public string ID_PANIER { get; set; } // varchar(11)
        //public string ID { get; set; } // varchar(11)
        public string ID_USER { get; set; } // varchar(32)
        public DateTime? CREATED_ON { get; set; }
        public decimal QUANTITE { get; set; }
        public string CODE_PRODUIT { get; set; } // varchar(11)
    }

    public partial class View_PANIER //: PANIER
    {
        public string ID_PANIER { get; set; }
        public string ID_USER { get; set; }
        public DateTime? CREATED_ON { get; set; }
        public View_PRODUITS_DETAILS PRODUITS { get; set; }
        public string CODE_PRODUIT { get; set; }
        public string DESIGNATION { get; set; }
        public decimal QUANTITE { get; set; }
        public string IMAGE_URL { get; set; }
        //public PRODUITS PRODUITS { get; set; }
    }

    public partial class CartItem
    {
        public string ID_PANIER { get; set; } // varchar(11)
        public string ID_USER { get; set; } // varchar(32)
        public PRODUITS PRODUITS { get; set; }
        public decimal QUANTITE { get; set; }
        public string CODE_PRODUIT { get; set; } // varchar(11)
    }

    public partial class Wish_List
    {
        public string ID_WishList { get; set; } // varchar(11)
        public string CODE_PRODUIT { get; set; } // varchar(11)
        public string ID_USER { get; set; } // varchar(32)
        public DateTime? CREATED_ON { get; set; }
    }

    public partial class View_WishList : Wish_List
    {
        public decimal PRIX_VENTE { get; set; }
        public decimal QTE_STOCK { get; set; }

        public string DESIGNATION { get; set; } 
        public string DESCRIPTION { get; set; } 
        public decimal QUANTITE { get; set; }
        public byte[] IMAGES { get; set; } 
        public decimal NOTE { get; set; } 
        public string CODE_FAMILLE { get; set; } 
        public string DESIGNATION_FAMILLE { get; set; }
        public string CODE_TYPE { get; set; }
        public string DESIGNATION_TYPE { get; set; }
    }

    public partial class COMMANDES : BASE_CLASS
    {
        public string ID { get; set; }
        public DateTime? DATE_LIVRAISON { get; set; }
        public decimal TOTAL_HT { get; set; }
        public decimal TOTAL_TTC { get; set; }
        public string MODE_REGELEMENT { get; set; }
        public int ETAT_COMMANDE { get; set; } 
        public string ID_USER { get; set; } 
        public DateTime? CREATED_ON { get; set; }
        public string ADRESSE_LIVRAISON { get; set; } 

        public List<COMMANDES_DETAILS> details { get; set; }
    }

    #region envoi de commande

    public partial class addToCard
    {
        public string CODE_PRODUIT { get; set; }
        public decimal QUANTITE { get; set; }
        public string ID_USER { get; set; }
        public string ID_PANIER { get; set; }
        public decimal PRIX_VENTE { get; set; }
    }

    public partial class customer
    {
        public string address1 { get; set; }
        public string city { get; set; }
        public string firstname { get; set; }
        public string lastname { get; set; }
        public decimal phone { get; set; }
    }

    public partial class Order
    {
        public customer customer { get; set; }
        public DateTime? date { get; set; }
        public List<addToCard> items { get; set; }
        public decimal number { get; set; }
        public string paymentMethod { get; set; }
        public string shippingMethod { get; set; }
        public string status { get; set; }
        public decimal total { get; set; }
    }

    #endregion

    public partial class COMMANDES_DETAILS
    {
        public string ID { get; set; }
        public decimal QTE { get; set; }
        public decimal PRIX_HT { get; set; }
        public decimal MONTANT_HT { get; set; }
        public string ID_COMMANDE { get; set; }
        public string CODE_PRODUIT { get; set; }
        public string PHONE_NUMBER { get; set; } 
        public string NAME { get; set; }
    }

    public class ComandesStates
    {
        public static int Canceled = -1;
        public static int Waiting = 1;
        public static int In_progress = 2;
        public static int Delivered = 3;
        public static int Finished = 4;
    }
}