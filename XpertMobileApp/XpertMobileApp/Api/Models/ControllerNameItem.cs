using System;
using System.Collections.Generic;
using System.Text;

namespace XpertMobileApp.Api.Models
{
    public enum ControllerNameItem
    {
        VTE_COMMANDE,
        ACH_ACHATS,
        ACH_RECEPTION,
        BSE_MAGASINS,
        BSE_LIEUX,
        BSE_COMPTE,
        VTE_VENTE,
        PRD_AGRICULTURE,
        TRS_TIERS,
        TDB_SIMPLE_INDICATORS,
        VTE_LIVRAISON,
        VTE_COMPTOIR,
        TRS_ENCAISS,
        PANIER,
        STK_PRODUITS,
        TRS_JOURNEES,
        STK_STOCK,
        SYS_PARAMETRE,
        LIV_TOURNEE,
        STK_TRANSFERT
    }

    public static class ControllerNameSwitch
    {
        public static string GetControllerName(ControllerNameItem controllerName)
        {
            if (Constants.AppName == Apps.XCOM_Mob || Constants.AppName == Apps.X_DISTRIBUTION)
                    return controllerName.ToString() + "_XCOM";
            else
                return
                    controllerName.ToString();
        }
    }
}
