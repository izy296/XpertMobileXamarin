using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace XpertWebApi.Models
{
    public static class Printer_Type
    {
        public static string Network { get { return "Network"; } }
        public static string Wifi { get { return "Wifi"; } }
        public static string Bluetooth { get { return "Bluetooth"; } }
    }

    public class XPrinter
    {
        public string Name { get; set; }
        public string Type { get; set; }
        public bool IsDefault { get; set; }
        public bool IsNetwork { get; set; }
    }
    public partial class Get_Print_VTE_TiketCaisse
    {
        public string CODE_VENTE { get; set; }
        public DateTime? DATE_VENTE { get; set; }
        public string NUM_VENTE { get; set; }
        public string TYPE_VENTE { get; set; }
        public decimal TOTAL_TTC { get; set; }
        public DateTime? DATE_ECHEANCE { get; set; }
        public string CREATED_BY { get; set; }
        public string NOM_TIERS { get; set; }
        public string DESIGN_MOTIF { get; set; }
        public string TYPE_DOC { get; set; }
        public string DESIGNATION_VTE { get; set; }
        public decimal REMISE_GLOBALE { get; set; }
        public string CODE_TIERS { get; set; }
        public int TYPE_VALIDATION { get; set; }
        public decimal QUANTITE { get; set; }
        public decimal PRIX_VTE_TTC { get; set; }
        public string UNITE_AFFICHER { get; set; }
        public decimal MT_TTC { get; set; }
        public string DESIGNATION_PRODUIT { get; set; }
        public decimal MT_RECU { get; set; }
        public decimal TOTAL_ENCAISS_REAL { get; set; }
        public string NOM_PHARM { get; set; }
        public string ADRESSE_PHARM { get; set; }
        public string TEL_PHARM { get; set; }
        public string PIED_TICKET { get; set; }
        public bool INCLUDE_NAME_VENDEUR { get; set; }
        public Int16 AFFICHE_MONNAIE { get; set; }
        public decimal Rendu { get; set; }
        public bool AFFICHER_NUM_VENTE_TICKET { get; set; }
        public decimal SOLDE_TIERS { get; set; }
        public decimal POINTS { get; set; }
        public string MoneyWords { get; set; }
    }
    public partial class Get_Print_ds_ViewTrsEncaiss
    {
        public string NUM_ENCAISS { get; set; }
        public DateTime?  DATE_ENCAISS { get; set; }
        public string NOM_TIERS { get; set; }
        public string CREATED_BY { get; set; }
        public decimal TOTAL_ENCAISS { get; set; }
        public string ADRESSE_PHARM { get; set; }
        public string NOM_PHARM { get; set; }
        public string  CODE_TYPE { get; set; }
        public decimal SOLDE_ANTERIEUR { get; set; }
        public decimal SOLDE_ACTUEL { get; set; }
        public int AFFICHE_SOLDE { get; set; }
        public string MoneyWords { get; set; }
    }
        
}