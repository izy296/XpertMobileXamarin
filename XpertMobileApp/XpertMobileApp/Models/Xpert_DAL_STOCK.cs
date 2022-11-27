using SQLite;
using System;
using System.Collections.Generic;
using Xpert.Common.WSClient.Model;
using XpertMobileApp.DAL;
using System.IO;
using Xamarin.Forms;

namespace XpertMobileApp.Models
{
    public partial class STK_STOCK : BASE_CLASS
    {
        [PrimaryKey, AutoIncrement]
        public int ID { get; set; }
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
        public byte[] IMAGE { get; set; }
        private string iMAGE_URL { get; set; }
        private ImageSource image_source { get; set; }
        public ImageSource IMAGE_SOURCE
        {
            get
            {
                if (image_source==null)
                {
                    if (!App.Online)
                    {
                        if (IMAGE != null)
                            image_source = ImageSource.FromStream(() => new MemoryStream(IMAGE));
                        else image_source = null;
                    }
                    else
                    {
                        image_source = ImageSource.FromUri(new Uri(App.RestServiceUrl.Replace("api/", "") + string.Format("Images/GetImage?codeProduit={0}", CODE_PRODUIT)));
                    }
                }
                return image_source;
            }
            set
            {
                image_source = value;
            }
        }
        public string IMAGE_URL
        {
            get
            {
                if (App.Online)
                {
                    return App.RestServiceUrl.Replace("api/", "") + string.Format("Images/GetImage?codeProduit={0}", CODE_PRODUIT);
                }
                else
                {
                    if (IMAGE != null)
                        if (iMAGE_URL == null)
                        {
                            iMAGE_URL = BitConverter.ToString(IMAGE);
                            return iMAGE_URL;
                        }
                        else
                        {
                            return iMAGE_URL;
                        }

                    else return null;
                }
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
        public string TRUNCATED_DESIGNATION
        {
            get
            {
                return DESIGNATION_PRODUIT.Truncate(40);
            }
        }
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
        public bool HAS_NEW_ID_STOCK { get; set; } = false; // varchar(2500)
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

        private decimal selectedPrice = -1;
        public decimal SelectedPrice
        {
            get
            {
                if (selectedPrice == -1)
                    return PRIX_VENTE;
                else
                    return selectedPrice;
            }
            set
            {
                selectedPrice = value;
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
        [Ignore]
        public List<View_BSE_PRODUIT_AUTRE_UNITE> UnitesList { get; set; }

        public string CODE_UNITE_ACHAT { get; set; }
        public string CODE_UNITE_VENTE { get; set; }
        public string CODE_UNITE { get; set; }
        public decimal COEFFICIENT { get; set; }
        public decimal PRIX_VENTE_COLLISAGE { get; set; }
        public string DESIGNATION_UNITE { get; set; }
        public decimal PRIX_VENTE_FAMILLE { get; set; }
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

    public partial class STK_SORTIE : BASE_CLASS
    {
        public string CODE_SORTIE { get; set; } // varchar(50)
        public string NUM_SORTIE { get; set; } // varchar(20)
        public DateTime? DATE_SORTIE { get; set; } // datetime(3)
        public string CODE_TIERS { get; set; } // varchar(32)
        [NotNull]
        public string TYPE_SORTIE { get; set; } // varchar(4)
        public string NOTE_SORTIE { get; set; } // text(2147483647)
        public DateTime? CREATED_ON { get; set; } // datetime(3)
        public string CREATED_BY { get; set; } // varchar(200)
        public DateTime? MODIFIED_ON { get; set; } // datetime(3)
        public string MODIFIED_BY { get; set; } // varchar(200)
        public int SENS_DOC { get; set; } // int(10)
        public string EXERCICE { get; set; } // char(4)
        public decimal TOTAL_ACHAT { get; set; } // money(19,4)
        public decimal TOTAL_PPA { get; set; } // money(19,4)
        public decimal TOTAL_SHP { get; set; } // money(19,4)
        public decimal TOTAL_VENTE { get; set; } // money(19,4)   
        public decimal TOTAL_SORTIE { get; set; } // money(19,4)  
        public decimal TOTAL_PAYE { get; set; } // money(19,4)        
        public string TYPE_PAIEMENT { get; set; } // varchar(4)        
        public bool SOLVABLE { get; set; } // bit 
        public string STATUS_DOC { get; set; } // varchar(10)
        public DateTime? CLOTURE_ON { get; set; } // datetime(3)
        public string CLOTURED_BY { get; set; } // varchar(200)
    }


    public partial class STK_SORTIE_DETAIL : BASE_CLASS
    {
        public string CODE_SORTIE_DETAIL { get; set; } // varchar(32)
        public string CODE_SORTIE { get; set; } // varchar(50)
        public int ID_STOCK { get; set; } // int(10)
        public decimal QUANTITE { get; set; } // numeric(19,2)
        public decimal QTE_SORTIE { get; set; } // numeric(19,2)
        public int ORDRE { get; set; } // int(10)
        public DateTime? CREATED_ON { get; set; } // datetime(3)
        public string CREATED_BY { get; set; } // varchar(200)
        public DateTime? MODIFIED_ON { get; set; } // datetime(3)
        public string MODIFIED_BY { get; set; } // varchar(200)
        public decimal MT_ACHAT { get; set; } // money(19,4)
        public decimal MT_PPA { get; set; } // money(19,4)
        public decimal MT_SHP { get; set; } // money(19,4)
        public decimal MT_VENTE { get; set; } // money(19,4)
        public decimal COUT_ACHAT { get; set; } // money(19,4)
        public String CODE_UNITE { get; set; } // varchar(15)
    }

    // View

    public partial class View_STK_SORTIE : STK_SORTIE
    {
        public string TIERS_NomC { get; set; } // varchar(501)
        public string DESIGNATION_TYPE { get; set; } // varchar(200)
    }

    // View
    public partial class View_STK_SORTIE_DETAIL : STK_SORTIE_DETAIL
    {
        public string CODE_PRODUIT { get; set; } // varchar(50)
        public DateTime? DATE_PEREMPTION { get; set; }
        public decimal PRIX_VENTE { get; set; } // 
        public decimal PRIX_HT { get; set; } // 
        public decimal PPA { get; set; } // 
        public decimal SHP { get; set; } // 
        public decimal TAUX_TVA { get; set; } //
        public decimal COUT_ACHAT_STOCK { get; set; } // money(19,4)
        public string NUM_SORTIE { get; set; } // varchar(20)
        public DateTime? DATE_SORTIE { get; set; } // datetime(3)
        public string CODE_TIERS { get; set; } // varchar(32)
        public string TYPE_SORTIE { get; set; } // varchar(4)
        public string TIERS_NOMC { get; set; } // varchar(501)
        public string DESIGNATION_PRODUIT { get; set; } // varchar(404)
        public decimal QTE_STOCK { get; set; } // numeric(18,2)
        public string LOT { get; set; } // varchar(50)
                                        // char(8)
        public short TYPE_PRODUIT { get; set; } // smallint(5)
        public decimal TARIF { get; set; } // money(19,4)
        public string CODE_LABO { get; set; } // smallint(5)
        public string CODE_BARRE { get; set; } // nvarchar(250)
        public string CODE_BARRE_LOT { get; set; } // nvarchar(250)
        public string CODE_FORME { get; set; } // varchar(20)
        public string UNITE { get; set; } // varchar(50)
        public string CONDIT { get; set; } // varchar(50)
        public string DOSAGE { get; set; } // varchar(50)
        public string DESIGN_LABO { get; set; } // varchar(2500)
        public string DESIGN_FORME { get; set; } // varchar(100)
        public string DESIGN_MAGASIN { get; set; } // varchar(100)
        public string DESIGNATION_TYPE_SORTIE { get; set; } // varchar(100)
        public string DESIGNATION_EMPLACEMENT { get; set; }
        public string DESIGNATION_UNITE { get; set; }
    }

    public partial class STK_ENTREE : BASE_CLASS
    {
        public string CODE_ENTREE { get; set; } // varchar(50)
        public string NUM_ENTREE { get; set; } // varchar(20)
        public DateTime? DATE_ENTREE { get; set; } // datetime(3)
        public string CODE_TIERS { get; set; } // varchar(32)
        public string NUM_COMMANDE { get; set; } // varchar(20)
        public string TYPE_ENTREE { get; set; } // varchar(4)
        public string CODE_MAGASIN { get; set; } // varchar(10)
        public string MODE_REGLEMENT { get; set; } // varchar(1)
        public string FACT_BL { get; set; } // varchar(5)
        public string NUM_DOC_TIER { get; set; } // varchar(50)
        public decimal TOTAL_HT { get; set; } // money(19,4)
        public decimal TOTAL_TVA { get; set; } // money(19,4)
        public decimal TOTAL_PPA { get; set; } // money(19,4)
        public decimal TOTAL_SHP { get; set; } // money(19,4)
        public decimal TOTAL_RISTOURNE { get; set; } // money(19,4)
        public decimal RISTOUNE_FACT { get; set; } // money(19,4)
        public decimal TOTAL_MARGE { get; set; } // money(19,4)
        public decimal MT_TIMBRE { get; set; } // money(19,4)
        public decimal TOTAL_TTC { get; set; } // money(19,4)
        public decimal TOTAL_VENTE { get; set; } // money(19,4)
        public decimal TOTAL_UG { get; set; } // money(19,4)
        public decimal TOTAL_PAYE { get; set; } // money(19,4)
        public DateTime? CREATED_ON { get; set; } // datetime(3)
        public string CREATED_BY { get; set; } // varchar(200)
        public DateTime? MODIFIED_ON { get; set; } // datetime(3)
        public string MODIFIED_BY { get; set; } // varchar(200)
        public string EXERCICE { get; set; } // char(4)
        public decimal TOTAL_PPA_UG { get; set; } // money(19,4)
        public decimal TOTAL_SHP_UG { get; set; } // money(19,4)
        public DateTime? DATE_ECHEANCE { get; set; } // datetime(3)
        public string NOTE_ENTREE { get; set; } // text
        public int SENS_DOC { get; set; } // int(1)
        public decimal TOTAL_FOURNISSEUR { get; set; } // money(19,4)
        public string STATUS { get; set; } // varchar(5)  
        public DateTime? DATE_CLOTURE { get; set; }  // datetime(3)
        public string CLOTURED_BY { get; set; } // varchar(200)
        public decimal TOTAL_TTC_CLOTURE { get; set; } // money(19,4)
        public bool PPA_BY_PRIXVENTE { get; set; }// calucle ppa par prix vente c'est la valeur true snn le contraire
    }

    public partial class STK_ENTREE_DETAIL : BASE_CLASS
    {
        public string CODE_ENTREE_DETAIL { get; set; } // varchar(32)        
        public string CODE_ENTREE { get; set; } // varchar(50)
        public int? ID_STOCK { get; set; } // int(10)
        public string CODE_PRODUIT { get; set; } // varchar(32)
        public string LOT { get; set; } // varchar(50)
        public string CODE_BARRE_LOT { get; set; } // nvarchar(250)
        public DateTime? DATE_PEREMPTION { get; set; } // datetime(3)
        public decimal QUANTITE { get; set; } // numeric(19,2)
        public decimal QTE_BONUS { get; set; } // numeric(19,2)
        public decimal QTE_RECUE { get; set; } // numeric(19,2)
        public decimal COUT_ACHAT { get; set; } // money(19,4)
        public decimal PRIX_UNITAIRE { get; set; } // money(19,4)
        public decimal PRIX_TTC { get; set; } // money(19,4)
        public decimal PRIX_VENTE { get; set; } // money(19,4)
        public decimal SHP { get; set; } // money(19,4)
        public decimal TAUX_MARGE { get; set; } // money(19,4)
        public decimal TAUX_BONUS { get; set; } // money(19,4)
        public decimal PPA { get; set; } // money(19,4)
        public decimal TAUX_RISTOURNE { get; set; } // money(19,4)
        public decimal MT_TVA { get; set; } // money(19,4)
        public decimal MT_PPA { get; set; } // money(19,4)
        public decimal MT_PPA_UG { get; set; } // money(19,4)
        public decimal MT_SHP { get; set; } // money(19,4)
        public decimal MT_SHP_UG { get; set; } // money(19,4)
        public decimal MT_HT { get; set; } // money(19,4)
        public decimal MT_RISTOURNE { get; set; } // money(19,4)
        public decimal TAUX_TVA { get; set; } // money(19,4)
        public decimal MT_RISTOURNE_FACT { get; set; } // money(19,4)
        public decimal MT_UG { get; set; } // money(19,4)
        public decimal MT_TTC { get; set; } // money(19,4)
        public decimal MT_VENTE { get; set; } // money(19,4)    
        public string CODE_MAGASIN { get; set; } // varchar(20)
        public string CODE_EMPLACEMENT { get; set; } // varchar(10)
        public short ETAT { get; set; } // smallint(5)
        public int ORDRE { get; set; } // int(10)
        public DateTime? CREATED_ON { get; set; } // datetime(3)
        public string CREATED_BY { get; set; } // varchar(200)
        public DateTime? MODIFIED_ON { get; set; } // datetime(3)
        public string MODIFIED_BY { get; set; } // varchar(200)
        public string CODE_UNITE { get; set; }// varchar(15)
    }

    // View
    public partial class View_STK_ENTREE : STK_ENTREE
    {
        public decimal TAUX_MARGE { get; set; } // money(19,4)
        public decimal TOTAL_RESTE { get; set; } // money(19,4)
        public string TIERS_NomC { get; set; } // varchar(501)
        public string DESIGNATION_TYPE { get; set; } // varchar(200)
        public string DESIGN_MODE_REG { get; set; } // varchar(300)
        public string DESIGNATION_MAGASIN { get; set; } // varchar(100)
        public string CATEGORIE { get; set; } // varchar(7)
        public int LITIGES { get; set; }
    }

    // View
    public partial class View_STK_ENTREE_DETAIL : STK_ENTREE_DETAIL
    {
        public string NUM_DOC_TIER { get; set; } // varchar(50)
        public string NUM_ENTREE { get; set; } // varchar(20)
        public DateTime? DATE_ENTREE { get; set; } // datetime(3)
        public string CODE_TIERS { get; set; } // varchar(32)
        public string TYPE_ENTREE { get; set; } // varchar(4)
        public string TIERS_NOMC { get; set; } // varchar(501)
        public string DESIGNATION_PRODUIT { get; set; } // varchar(404)
        public decimal QTE_STOCK { get; set; } // numeric(18,2)
        public short TYPE_PRODUIT { get; set; } // smallint(5)
        public decimal TARIF { get; set; } // money(19,4)
        public string CODE_LABO { get; set; } // smallint(5)
        public string CODE_BARRE { get; set; } // nvarchar(250)
        public string CODE_FORME { get; set; } // varchar(20)
        public string UNITE { get; set; } // varchar(50)
        public string CONDIT { get; set; } // varchar(50)
        public string DOSAGE { get; set; } // varchar(50)
        public string DESIGN_LABO { get; set; } // varchar(2500)
        public string DESIGN_FORME { get; set; } // varchar(100)
        public string DESIGN_MAGASIN { get; set; } // varchar(100)       
        public string DESIGN_EMPLACEMENT { get; set; } // varchar(100)       
        public string FACT_BL { get; set; } // varchar(5)
        public decimal QTE_MANQUANTS { get; set; } // numeric(19,2)
        public string DESIGNATION_TYPE_ENTREE { get; set; }
        public string OLD_CB { get; set; }
        public int LITIGES { get; set; }
        public string DESIGNATION_UNITE { get; set; }//varchar(100)
    }

    public partial class BSE_SORTIE_TYPE : BASE_CLASS
    {
        public string CODE_TYPE { get; set; }
        public string DESIGNATION_TYPE { get; set; }

    }

    public partial class BSE_PRODUIT_TAG : BASE_CLASS
    {
        public string CODE { get; set; } //[char](20) NOT NULL,
        public string DESIGNATION { get; set; } //[varchar](100 ) NOT NULL,

    }

    public partial class BSE_PRODUIT_LABO : BASE_CLASS
    {
        public string CODE { get; set; } //smallint
        public string DESIGNATION { get; set; } // varchar(2500)
        public string ADRESSE { get; set; } // varchar(2500)
        public string PAYE { get; set; } // varchar(250)
        public string TEL { get; set; } // varchar(50)
        public string FAX { get; set; } // varchar(50)
        public string E_MAIL { get; set; } // varchar(50)
        public string SITE_WEB { get; set; }// varchar(250)
    }


    public partial class BSE_PRODUIT_UNITE : BASE_CLASS
    {
        public string CODE { get; set; } //[char](20) NOT NULL,
        public string DESIGNATION { get; set; } //[varchar](100 ) NOT NULL,
        public bool UNITE_PRODUIT_PUBLIC { get; set; }
        public bool SYNCHRONISE { get; set; }
        public DateTime? DATE_SYNCHRONISATION { get; set; }//[datetime] NULL

    }
    public partial class STK_ECHANGE : BASE_CLASS
    {
        public decimal TOTAL_ECHANGE_SYS { get; set; }
        public decimal TOTAL_ECHANGE { get; set; }
        public decimal TOTAL_ECHANGE_DOC { get; set; }
        public decimal TOTAL_HT { get; set; }
        public decimal TOTAL_PPA { get; set; }
        public decimal TOTAL_SHP { get; set; }
        public decimal TOTAL_TVA { get; set; }
        public string MODE_CAL_MT_ECHANGE { get; set; }
        public int SENS_DOC { get; set; } // int(10)
        public string CODE_MOTIF { get; set; } // string 
        public string DESIGNATION_MOTIF { get; set; } // string 
        public string DESIGNATION_MODE_CAL { get; set; } // string 
    }
    public partial class View_STK_ECHANGE : View_TRS_CREDIT_PAIEMENT
    {
        public decimal TOTAL_ECHANGE_SYS { get; set; }
        public decimal TOTAL_ECHANGE { get; set; }
        public decimal TOTAL_ECHANGE_DOC { get; set; }
        public decimal TOTAL_HT { get; set; }
        public decimal TOTAL_PPA { get; set; }
        public decimal TOTAL_SHP { get; set; }
        public decimal TOTAL_TVA { get; set; }
        public string MODE_CAL_MT_ECHANGE { get; set; }
        public int SENS_DOC { get; set; } // int(10)
        public string CODE_MOTIF { get; set; } // string 
        public string DESIGNATION_MOTIF { get; set; } // string 
        public string DESIGNATION_MODE_CAL { get; set; } // string 
    }

    public partial class View_STK_ECHANGE_DETAIL
    {
        public string DESIGNATION_PRODUIT { get; set; }
        public string CODE_BARRE_LOT { get; set; }
        public string LOT { get; set; }
        public DateTime? DATE_PEREMPTION { get; set; }
        public double QTE_ECHANGE { get; set; }
        public decimal MT_HT { get; set; } // money(19,4)
        public decimal MT_SHP { get; set; } // money(19,4)
        public decimal MT_PPA { get; set; } // money(19,4)

    }

    public partial class View_STK_MOTIF_ECHANGE
    {
        public string CODE_MOTIF { get; set; } // string 
        public string DESIGNATION_MOTIF { get; set; } // string 
    }

    public class RotationDesProduits : BASE_CLASS
    {
        public string CODE_PRODUIT { get; set; } // varchar(11)
        public decimal PRIX_UNITAIRE { get; set; } // varchar(11)
        public string DESIGNATION { get; set; } // varchar(11)

        public decimal QTE_ENTREE { get; set; } // varchar(11)

        public decimal QTE_SORTIE { get; set; } // varchar(11)

        public decimal VAL_SORTIE { get; set; } // varchar(11)
        public decimal VAL_ENTREE { get; set; } // varchar(11)

        public decimal QuantiteToDisplay
        {
            get
            {
                if (QTE_ENTREE == 0)
                {
                    return QTE_SORTIE;
                }
                else if (QTE_SORTIE == 0)
                {
                    return QTE_ENTREE;
                }
                else return 0;
            }
        }
    }

    public class RotationDesProduitsDetails : RotationDesProduits
    {
        public string CREATED_BY { get; set; } // varchar(11)
        public string REF { get; set; } // varchar(11)
        public string CODE_DOC { get; set; } // varchar(11)
        public DateTime? DATE_MVT { get; set; } // varchar(11)
        public string TYPE_MVT { get; set; } // varchar(11)
        public string TIERS { get; set; } // varchar(11)
        public string TYPE_DOC { get; set; } // varchar(11)

        public decimal ValeurToDisplay
        {
            get
            {
                if (VAL_SORTIE == 0)
                {
                    return VAL_ENTREE;
                }
                else if (VAL_ENTREE == 0)
                {
                    return VAL_SORTIE;
                }
                else return 0;
            }
        }
    }

    public partial class STK_TRANSFERT : BASE_CLASS
    {
        public string CODE_TRANSFERT { get; set; } // varchar(32)
        public string NUM_TRANSFERT { get; set; } // varchar(32)
        public DateTime? DATE_TRANSEFRT { get; set; } // datetime(3)
        public string MAGASIN_SOURCE { get; set; } // varchar(32)
        public string MAGASIN_DESTINATION { get; set; } // varchar(32)
        public DateTime? CREATED_ON { get; set; } // datetime(3)
        public string CREATED_BY { get; set; } // varchar(200)
        public DateTime? MODIFIED_ON { get; set; } // datetime(3)
        public string MODIFIED_BY { get; set; } // varchar(200)
        public decimal TOTAL_ACHAT { get; set; } // money(19,4)
        public decimal TOTAL_PPA { get; set; } // money(19,4)
        public decimal TOTAL_SHP { get; set; } // money(19,4)
        public string NOTE_TRANSFERT { get; set; } // varchar(max)
        public bool IS_VALIDATE { get; set; } // varchar(max)
        public string VALIDATE_BY { get; set; } // varchar(max)
    }

    public partial class View_STK_TRANSFERT : STK_TRANSFERT
    {

        public string DESIGN_MAGASIN_SOURCE { get; set; } // varchar(32)
        public string DESIGN_MAGASIN_DESTINATION { get; set; } // varchar(32)
        public string RESPONSABLE_MAGASIN_SOURCE { get; set; } // varchar(200)
        public string RESPONSABLE_MAGASIN_DESTINATION { get; set; } // varchar(200)

    }

    public partial class STK_TRANSFERT_DETAIL : BASE_CLASS
    {
        public string CODE_DETAIL { get; set; } // varchar(32)
        public short ORDER_ROW { get; set; } // smallint(5)
        public string CODE_TRANSFERT { get; set; } // varchar(32)
        public int? ID_STOCK_SOURCE { get; set; } // int(10)
        public int? ID_STOCK_DESTINATION { get; set; } // int(10)
        public decimal QUANTITE { get; set; } // numeric(19,2)
        public decimal COUT_ACHAT { get; set; } // money(19,4)
        public decimal MONTANT_ACHAT { get; set; } // money(19,4)
        public decimal MONTANT_PPA { get; set; } // money(19,4)
        public decimal MONTANT_SHP { get; set; } // money(19,4)
        public DateTime? CREATED_ON { get; set; } // datetime(3)
        public string CREATED_BY { get; set; } // varchar(200)
        public DateTime? MODIFIED_ON { get; set; } // datetime(3)
        public string MODIFIED_BY { get; set; } // varchar(200)
        public string CODE_EMPLACEMENT_DESTINIATION { get; set; } // varchar(200)
    }

    public partial class View_STK_TRANSFERT_DETAIL : STK_TRANSFERT_DETAIL
    {
        public string NUM_TRANSFERT { get; set; } // varchar(32)
        public string CODE_PRODUIT { get; set; } // varchar(20)
        public string DESIGNATION { get; set; } // varchar(404)
        public string MAGASIN_SOURCE { get; set; } // varchar(10)
        public string MAGASIN_DESTINATION { get; set; } // varchar(10)
        public DateTime? DATE_PEREMPTION { get; set; } // datetime(3)
        public string CODE_BARRE { get; set; } // nvarchar(250)
        public string CODE_BARRE_LOT { get; set; } // nvarchar(1500)
        public string CODE_EMPLACEMENT { get; set; } // varchar(10)
        public string LOT { get; set; } // varchar(50)
        public decimal QTE_STOCK { get; set; } // numeric(18,2)
        public decimal PPA { get; set; } // money(19,4)
        public decimal SHP { get; set; } // money(19,4)
        public string REF_PRODUIT { get; set; } // nvarchar(250)
        public decimal PRIX_ACH_HT { get; set; } // money(19,4)
        public decimal PRIX_HT { get; set; } // money(19,4)
        public decimal PRIX_VENTE { get; set; } // money(19,4)
        public short TYPE_PRODUIT { get; set; } // smallint(5)
        public decimal TARIF { get; set; } // money(19,4)
        public string CODE_LABO { get; set; } // smallint(5)
        public string CODE_FORME { get; set; } // varchar(20)
        public string DESIGN_FORME { get; set; } // varchar(100)
        public string UNITE { get; set; } // varchar(50)
        public string CONDIT { get; set; } // varchar(50)
        public string DOSAGE { get; set; } // varchar(50)
        public DateTime? DATE_TRANSEFRT { get; set; } // datetime(3)
        public string DESIGNATION_MAGASIN_SOURCE { get; set; }
        public string DESIGNATION_MAGASIN_DESTINATION { get; set; }
        public decimal TAUX_TVA { get; set; }
        public string DESIGNATION_EMPLACEMENT_SOURCE { get; set; }
        public string DESIGNATION_EMPLACEMENT_DESTINATION { get; set; }

    }

    public class BSE_PRODUIT_FAMILLE
    {
        public string CODE { get; set; } // varchar(10)
        public string DESIGNATION { get; set; } // varchar(50)
    }

    public class BSE_PRODUIT_TYPE
    {
        public string CODE_TYPE { get; set; }
        public string DESIGNATION_TYPE { get; set; }
    }
    public partial class View_STK_PRODUITS_PRIX_UNITE : BASE_CLASS
    {
        public string lot { get; set; }
        public DateTime? DATE_PEREMPTION { get; set; }
        public string CODE_PRODUIT { get; set; }
        public string CODE_UNITE_ACHAT { get; set; }
        public string CODE_UNITE_VENTE { get; set; }
        public string DESIGNATION_PRODUIT { get; set; }
        public decimal PRIX_VENTE_PRODUIT { get; set; }
        public decimal QTE_STOCK { get; set; }
        public string CODE_UNITE { get; set; }
        public decimal COEFFICIENT { get; set; }
        public decimal PRIX_VENTE_COLLISAGE { get; set; }
        public string DESIGNATION_UNITE { get; set; }
        public decimal PRIX_VENTE_FAMILLE { get; set; }
        public string DESIGN_FAMILLE { get; set; }
        public string CODE_FAMILLE { get; set; }
    }
    public partial class STK_PRODUITS_IMAGES_XCOM
    {
        // implementer pour la synchronisaton
    }

    public partial class STK_PRODUITS_IMAGES
    {
        public string CODE_IMAGE { get; set; } // varchar(11)
        public string CODE_PRODUIT { get; set; } // varchar(150)
        public byte[] IMAGE { get; set; } // varchar(50)
        public bool DEFAULT_IMAGE { get; set; } // varchar(50)
    }

}
