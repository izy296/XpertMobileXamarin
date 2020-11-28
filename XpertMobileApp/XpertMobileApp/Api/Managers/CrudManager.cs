using Microcharts;
using System;
using System.Collections.Generic;
using System.Text;
using Xpert.Common.WSClient.Services;
using XpertMobileApp.DAL;
using XpertMobileApp.Models;
using XpertMobileApp.ViewModels;

namespace XpertMobileApp.Api.Managers
{
    public static class CrudManager
    {
        internal static ProductManager Products = new ProductManager();
        internal static StockManager Stock = new StockManager();
        
        internal static CrudService<View_VTE_VENTE> Commandes = new CrudService<View_VTE_VENTE>(App.RestServiceUrl, "VTE_COMMANDE", App.User.Token);

        internal static CrudService<View_ACH_DOCUMENT> Achats = new CrudService<View_ACH_DOCUMENT>(App.RestServiceUrl, "ACH_ACHATS", App.User.Token);

        internal static CrudService<View_BSE_MAGASIN> BSE_MAGASINS = new CrudService<View_BSE_MAGASIN>(App.RestServiceUrl, "BSE_MAGASINS", App.User.Token);
        internal static CrudService<BSE_TABLE> BSE_LIEUX = new CrudService<BSE_TABLE>(App.RestServiceUrl, "BSE_LIEUX", App.User.Token);
        

        internal static CrudService<Settings> MobileSettings = new CrudService<Settings>(App.RestServiceUrl, "MobileSettings", App.User.Token);

        internal static CrudService<View_BSE_COMPTE> BSE_COMPTE = new CrudService<View_BSE_COMPTE>(App.RestServiceUrl, "BSE_COMPTE", App.User.Token);

        internal static CrudService<View_VTE_VENTE> Ventes = new CrudService<View_VTE_VENTE>(App.RestServiceUrl, "VTE_VENTE", App.User.Token);

        internal static CrudService<View_PRD_AGRICULTURE> Productions = new CrudService<View_PRD_AGRICULTURE>(App.RestServiceUrl, "PRD_AGRICULTURE", App.User.Token);

        internal static SYS_PARAMS_Manager SysParams = new SYS_PARAMS_Manager();

        internal static SessionsManager Sessions = new SessionsManager();

        internal static SYS_PERMISSIONS_Manager Permissions = new SYS_PERMISSIONS_Manager();

        internal static CrudService<View_TRS_TIERS> TiersService = new CrudService<View_TRS_TIERS>(App.RestServiceUrl, "TRS_TIERS", App.User.Token);

        internal static ProductionInfosManager ProductionInfosManager = new ProductionInfosManager();

        internal static TiersManager TiersManager = new TiersManager();

        internal static CrudService<TDB_SIMPLE_INDICATORS> SimpleIndicatorsService = new CrudService<TDB_SIMPLE_INDICATORS>(App.RestServiceUrl, "TDB_SIMPLE_INDICATORS", App.User.Token);

        public static void InitServices()
        {
            SysParams = new SYS_PARAMS_Manager();
            Sessions = new SessionsManager();
            Permissions = new SYS_PERMISSIONS_Manager();
            Stock = new StockManager();

            Products = new ProductManager();
            BSE_MAGASINS = new CrudService<View_BSE_MAGASIN>(App.RestServiceUrl, "BSE_MAGASINS", App.User.Token);
            BSE_COMPTE = new CrudService<View_BSE_COMPTE>(App.RestServiceUrl, "BSE_COMPTE", App.User.Token);
            BSE_LIEUX = new CrudService<BSE_TABLE>(App.RestServiceUrl, "BSE_LIEUX", App.User.Token);

            Commandes = new CrudService<View_VTE_VENTE>(App.RestServiceUrl, "VTE_COMMANDE", App.User.Token);
            Achats = new CrudService<View_ACH_DOCUMENT>(App.RestServiceUrl, "ACH_ACHATS", App.User.Token);
            Ventes = new CrudService<View_VTE_VENTE>(App.RestServiceUrl, "VTE_VENTE", App.User.Token);
            TiersService = new CrudService<View_TRS_TIERS>(App.RestServiceUrl, "TRS_TIERS", App.User.Token);

            ProductionInfosManager = new ProductionInfosManager();
            TiersManager = new TiersManager();

            MobileSettings = new CrudService<Settings>(App.RestServiceUrl, "MobileSettings", App.User.Token);
        }

        public static VTE_VENTE_BLL GetVteBll(string typeVte) 
        {
            string controlerName = "VTE_VENTE";
            
            if (typeVte == VentesTypes.Livraison) 
            {
                controlerName = "XCOM_VTE_LIVRAISON";
            }
            else if (typeVte == VentesTypes.VenteComptoir)
            { 
                controlerName = "XCOM_VTE_COMPTOIR";
            }

            var bll = new VTE_VENTE_BLL(controlerName);
            return bll;
        }
    }
}
