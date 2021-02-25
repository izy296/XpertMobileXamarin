using System;
using System.Collections.Generic;
using System.Text;

namespace XpertMobileApp.Helpers
{
    public static class DALHelper
    {
        public static string GetTypeDesignation(string typeVente)
        {
            if (typeVente == "FV") return "Facture de Vente";
            else if (typeVente == "CC") return "Commande Client";
            else if (typeVente == "BL") return "Bon de Livraison";
            else if (typeVente == "VC") return "Vente au Comptoir";
            else if (typeVente == "PR") return "Proforma Client";
            else if (typeVente == "BR") return "Retour Livraison";
            else if (typeVente == "RC") return "Retour Comptoir";
            else if (typeVente == "FA") return "Avoir Client";
            else if (typeVente == "ES") return "Entrée en Stock";
            else if (typeVente == "ES0") return "Fournisseur /Solde Initial";
            else if (typeVente == "ES1") return "Entrée / Achat";
            else if (typeVente == "ES2") return "Entrée / Échange";
            else if (typeVente == "ES3") return "Entrée / Préparation";
            else if (typeVente == "ES4") return "Entrée / Dépôt de vente";
            else if (typeVente == "ES5") return "Entrée / Régularisation";
            else if (typeVente == "ES6") return "Entrée / Liste démarrage";
            else if (typeVente == "ES7") return "Entrée / Bonus";
            else if (typeVente == "ES8") return "Entrée / Autre";
            else if (typeVente == "SS4") return "Sortie / Échange";
            else if (typeVente == "SS8") return "Sortie / Dépôt de Vente";
            else if (typeVente == "FF") return "Facture Fournisseur";
            else if (typeVente == "AF") return "Avoir Fournisseur";
            else if (typeVente == "RF") return "Retour Fournisseur";
            else if (typeVente == "LF") return "Bon de réception";
            else if (typeVente == "FC1") return "Tarif CHIFA";
            else if (typeVente == "FC") return "Facture CHIFA";
            else if (typeVente == "FS1") return "Tarif CASNOS";
            else if (typeVente == "FC2") return "Crédit CHIFA";
            else if (typeVente == "FS2") return "Crédit CASNOS";
            else if (typeVente == "VI") return "Vente Instance";
            else if (typeVente == "AJ") return "Ajustement";
            else if (typeVente == "ARC") return "Archivage Instance";
            else if (typeVente == "VM") return "Vente CVM";
            else return "";
        }
    }
}
