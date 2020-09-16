using System;
using System.Collections.Generic;
using System.Text;

namespace XpertMobileApp.Models
{
    public enum MenuItemType
    {
        Home,
        Encaissements,
        Tresorerie,
        Ventes,
        Livraison,
        Tournee,
        VenteComptoir,
        Psychotrop,
        Achats,
        AchatsProduction,
        OrdresProduction,
        Reception,
        Catalogues,
        MyCommandes,
        Commandes,
        Sessions,
        Tiers,
        Bordereaux,
        Produits,
        Manquants,
        Items,
        rfid,
        invrfid,
        EncAnalyses,
        SimpleIndicators,
        AchatAgroAnalyses,
        Settings,
        About
    }
    public class HomeMenuItem
    {
        public MenuItemType Id { get; set; }

        public string Title { get; set; }

        public string Image { get; set; }

        public int Mobile_Edition { get; set; }

        public Type TargetType { get; set; }
    }
}
