using System;

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
        public decimal TOTAL_HT { get; set; } // money(19,4)
        public decimal TOTAL_PPA { get; set; } // money(19,4)
        public decimal TOTAL_SHP { get; set; } // money(19,4)
        public decimal TOTAL_REMISE { get; set; } // money(19,4)
        public decimal TOTAL_TVA { get; set; } // money(19,4)
        public decimal TOTAL_TTC { get; set; } // money(19,4)
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
    }
    
    public partial class VTE_VENTE_DETAIL : BASE_CLASS
    {
        public string CODE_DETAIL { get; set; } // varchar(32)
        public string CODE_VENTE { get; set; } // varchar(32)
        public int? ID_STOCK { get; set; } // int(10)
        public string CODE_PRODUIT { get; set; } // varchar(32)
        public decimal QUANTITE { get; set; } // numeric(19,2)
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

    }

    public partial class View_VTE_VENTE_LOT : VTE_VENTE_DETAIL
    {
        public string DESIGNATION_PRODUIT { get; set; } // varchar(754)
        public string CODE_MAGASIN { get; set; } // varchar(10)

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

        public DateTime? DATE_PEREMPTION { get; set; }
        public string CODE_BARRE { get; set; } // nvarchar(250)
        public string CODE_BARRE_LOT { get; set; } // nvarchar(1500)
        public int TYPE_VALIDATION { get; set; } // int(10)
        public string CODE_EMPLACEMENT { get; set; } // varchar(10)
        public string DESIGNE_EMPLACEMENT { get; set; } // varchar(10)
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
    }
}
