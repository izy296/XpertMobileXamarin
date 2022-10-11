using System;
using System.Collections.Generic;
using System.Text;

namespace XpertMobileApp.Api.Models
{
    public enum MenuItemType
    {
        None,
        Home,
        Sessions,
        Encaissements,
        TransfertDeFond,
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
        About,
        XBoutiqueHome,
        XBoutique,
        XMyCommandes,
        XWishList,
        XPurchased,
        XProfile,
        Export,
        Import,
        Sortie,
        Notification,
        Echange,
        RotationDesProduits,
        TransfertStock
    }

    public enum MenuItemGroup
    {
        Home,
        Achats,
        Ventes,
        Stock,
        Psychotrope,
        Tresorerie,
        CHIFA,
        CVM,
        Analyses,
        Parametres,
    }
}
