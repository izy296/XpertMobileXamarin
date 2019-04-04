using System;
using System.Collections.Generic;
using System.Text;

namespace XpertMobileApp.Models
{
    public enum MenuItemType
    {
        Home,
        Encaissements,
        Ventes,
        Tiers,
        Produits,
        Items,
        rfid,
        invrfid,
        EncAnalyses,
        About
    }
    public class HomeMenuItem
    {
        public MenuItemType Id { get; set; }

        public string Title { get; set; }

        public string Image { get; set; }

        public Type TargetType { get; set; }
    }
}
