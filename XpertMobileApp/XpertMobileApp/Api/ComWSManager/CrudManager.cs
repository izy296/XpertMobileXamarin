using Microcharts;
using System;
using System.Collections.Generic;
using System.Text;
using Xpert.Common.WSClient.Services;
using XpertMobileApp.Api.ComWSManager;
using XpertMobileApp.Api.Models;
using XpertMobileApp.DAL;
using XpertMobileApp.Models;
using XpertMobileApp.ViewModels;

namespace XpertMobileApp.Api.Managers
{
    public static class CrudManager
    {
        internal static ProductManager Products = new ProductManager();
        internal static StockManager Stock = new StockManager();
        internal static CommandesManager Commandes = new CommandesManager();


        //internal static CrudService<View_VTE_VENTE> Commandes = new CrudService<View_VTE_VENTE>(App.RestServiceUrl, "VTE_COMMANDE", App.User.Token);

        internal static CrudService<View_ACH_MANQUANTS> Manquant = new CrudService<View_ACH_MANQUANTS>(App.RestServiceUrl, "ACH_MANQUANTS", App.User.Token);

        internal static CrudService<View_TRS_ENCAISS> Encaiss = new CrudService<View_TRS_ENCAISS>(App.RestServiceUrl, ControllerNameSwitch.GetControllerName(ControllerNameItem.TRS_ENCAISS), App.User.Token);

        internal static CrudService<View_ACH_DOCUMENT> Achats = new CrudService<View_ACH_DOCUMENT>(App.RestServiceUrl, ControllerNameSwitch.GetControllerName(ControllerNameItem.ACH_ACHATS), App.User.Token);

        internal static CrudService<View_ACH_DOCUMENT> Reception = new CrudService<View_ACH_DOCUMENT>(App.RestServiceUrl, ControllerNameSwitch.GetControllerName(ControllerNameItem.ACH_RECEPTION), App.User.Token);

        internal static CrudService<View_BSE_MAGASIN> BSE_MAGASINS = new CrudService<View_BSE_MAGASIN>(App.RestServiceUrl, "BSE_MAGASINS", App.User.Token);

        internal static CrudService<BSE_TABLE> BSE_LIEUX = new CrudService<BSE_TABLE>(App.RestServiceUrl, "BSE_LIEUX", App.User.Token);

        internal static CrudService<Settings> MobileSettings = new CrudService<Settings>(App.RestServiceUrl, "MobileSettings", App.User.Token);

        internal static CrudService<View_BSE_COMPTE> BSE_COMPTE = new CrudService<View_BSE_COMPTE>(App.RestServiceUrl, "BSE_COMPTE", App.User.Token);

        internal static CrudService<View_VTE_VENTE> Ventes = new CrudService<View_VTE_VENTE>(App.RestServiceUrl, ControllerNameSwitch.GetControllerName(ControllerNameItem.VTE_VENTE), App.User.Token);

        internal static CrudService<View_WishList> WishList = new CrudService<View_WishList>(App.RestServiceUrl, "WishList", App.User.Token);

        internal static CrudService<View_PRD_AGRICULTURE> Productions = new CrudService<View_PRD_AGRICULTURE>(App.RestServiceUrl, "PRD_AGRICULTURE", App.User.Token);

        internal static TourneeManager Tournee = new TourneeManager();

        internal static TransfertStock_Manager StockTransfert = new TransfertStock_Manager();

        internal static CrudService<View_LIV_TOURNEE_DETAIL> TourneeDetails = new CrudService<View_LIV_TOURNEE_DETAIL>(App.RestServiceUrl, "LIV_TOURNEE_DETAIL", App.User.Token);

        internal static SYS_PARAMS_Manager SysParams = new SYS_PARAMS_Manager();

        internal static SessionsManager Sessions = new SessionsManager();

        internal static SYS_PERMISSIONS_Manager Permissions = new SYS_PERMISSIONS_Manager();

        internal static CrudService<View_TRS_TIERS> TiersService = new CrudService<View_TRS_TIERS>(App.RestServiceUrl, "TRS_TIERS", App.User.Token);

        internal static ProductionInfosManager ProductionInfosManager = new ProductionInfosManager();

        internal static TiersManager TiersManager = new TiersManager();
        internal static ReceptionManager ReceptionManager = new ReceptionManager();

        internal static CrudService<TDB_SIMPLE_INDICATORS> SimpleIndicatorsService = new CrudService<TDB_SIMPLE_INDICATORS>(App.RestServiceUrl, "TDB_SIMPLE_INDICATORS", App.User.Token);

        internal static PanierManager PanierManager = new PanierManager();

        public static void InitServices()
        {
            PanierManager = new PanierManager();
            WishList = new CrudService<View_WishList>(App.RestServiceUrl, "WishList", App.User.Token);

            SysParams = new SYS_PARAMS_Manager();
            Sessions = new SessionsManager();
            Permissions = new SYS_PERMISSIONS_Manager();
            Stock = new StockManager();
            ReceptionManager = new ReceptionManager();
            Products = new ProductManager();
            BSE_MAGASINS = new CrudService<View_BSE_MAGASIN>(App.RestServiceUrl, "BSE_MAGASINS", App.User.Token);
            BSE_COMPTE = new CrudService<View_BSE_COMPTE>(App.RestServiceUrl, "BSE_COMPTE", App.User.Token);
            BSE_LIEUX = new CrudService<BSE_TABLE>(App.RestServiceUrl, "BSE_LIEUX", App.User.Token);
            Commandes = new CommandesManager();
            Achats = new CrudService<View_ACH_DOCUMENT>(App.RestServiceUrl, ControllerNameSwitch.GetControllerName(ControllerNameItem.ACH_ACHATS), App.User.Token);
            Ventes = new CrudService<View_VTE_VENTE>(App.RestServiceUrl, "VTE_VENTE", App.User.Token);
            TiersService = new CrudService<View_TRS_TIERS>(App.RestServiceUrl, "TRS_TIERS", App.User.Token);

            ProductionInfosManager = new ProductionInfosManager();
            TiersManager = new TiersManager();

            Encaiss = new CrudService<View_TRS_ENCAISS>(App.RestServiceUrl, ControllerNameSwitch.GetControllerName(ControllerNameItem.TRS_ENCAISS), App.User.Token);
            Manquant = new CrudService<View_ACH_MANQUANTS>(App.RestServiceUrl, "ACH_MANQUANTS", App.User.Token);
            Productions = new CrudService<View_PRD_AGRICULTURE>(App.RestServiceUrl, "PRD_AGRICULTURE", App.User.Token);
            SimpleIndicatorsService = new CrudService<TDB_SIMPLE_INDICATORS>(App.RestServiceUrl, "TDB_SIMPLE_INDICATORS", App.User.Token);

            Tournee = new TourneeManager();
            TourneeDetails = new CrudService<View_LIV_TOURNEE_DETAIL>(App.RestServiceUrl, "LIV_TOURNEE_DETAIL", App.User.Token);
            MobileSettings = new CrudService<Settings>(App.RestServiceUrl, "MobileSettings", App.User.Token);
        }

        public static VentesManager GetVteBll(string typeVte)
        {
            string controlerName = "VTE_VENTE";

            if (typeVte == VentesTypes.Livraison && Constants.AppName == Apps.XPH_Mob)
            {
                controlerName = "VTE_LIVRAISON";
            }
            else if (typeVte == VentesTypes.Livraison && (Constants.AppName == Apps.XCOM_Mob || Constants.AppName == Apps.X_DISTRIBUTION))
            {
                controlerName = "VTE_LIVRAISON_XCOM";
            }
            else if (typeVte == VentesTypes.VenteComptoir && Constants.AppName == Apps.XPH_Mob)
            {
                controlerName = "VTE_COMPTOIR";
            }
            else if (typeVte == VentesTypes.VenteComptoir && Constants.AppName == Apps.XCOM_Mob)
            {
                controlerName = "VTE_COMPTOIR_XCOM";
            }

            var bll = new VentesManager(controlerName);
            return bll;
        }
    }
}
