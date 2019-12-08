using System;
using System.Collections.Generic;
using System.Text;
using Xpert.Common.WSClient.Services;
using XpertMobileApp.DAL;

namespace XpertMobileApp.Api.Managers
{
    public static class CrudManager
    {
        internal static ProductManager Products = new ProductManager();

        internal static CrudService<View_VTE_VENTE> Commandes = new CrudService<View_VTE_VENTE>(App.RestServiceUrl, "VTE_COMMANDE", App.User.Token);

        internal static CrudService<View_ACH_DOCUMENT> Achats = new CrudService<View_ACH_DOCUMENT>(App.RestServiceUrl, "ACH_ACHATS", App.User.Token);

        internal static CrudService<View_PRD_AGRICULTURE> Productions = new CrudService<View_PRD_AGRICULTURE>(App.RestServiceUrl, "PRD_AGRICULTURE", App.User.Token);

        internal static SYS_PARAMS_Manager SysParams = new SYS_PARAMS_Manager();

        internal static SYS_PERMISSIONS_Manager Permissions = new SYS_PERMISSIONS_Manager();

        internal static CrudService<View_TRS_TIERS> TiersService = new CrudService<View_TRS_TIERS>(App.RestServiceUrl, "TRS_TIERS", App.User.Token);

        internal static ProductionInfosManager ProductionInfosManager = new ProductionInfosManager();

        internal static TiersManager TiersManager = new TiersManager();
        
        public static void InitServices()
        {
            SysParams = new SYS_PARAMS_Manager();
            Permissions = new SYS_PERMISSIONS_Manager();

            Products = new ProductManager();
            Commandes = new CrudService<View_VTE_VENTE>(App.RestServiceUrl, "VTE_COMMANDE", App.User.Token);
            Achats = new CrudService<View_ACH_DOCUMENT>(App.RestServiceUrl, "ACH_ACHATS", App.User.Token);
            TiersService = new CrudService<View_TRS_TIERS>(App.RestServiceUrl, "TRS_TIERS", App.User.Token);

            ProductionInfosManager = new ProductionInfosManager();
            TiersManager = new TiersManager();
        }
    }
}
