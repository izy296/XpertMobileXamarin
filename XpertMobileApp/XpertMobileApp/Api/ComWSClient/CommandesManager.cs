using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using Xpert.Common.WSClient.Helpers;
using Xpert.Common.WSClient.Model;
using Xpert.Common.WSClient.Services;
using XpertMobileApp.DAL;

namespace XpertMobileApp.Api
{
    internal class CommandesManager : CrudService<View_VTE_VENTE>
    {
        public CommandesManager() : 
            base(App.RestServiceUrl, "VTE_COMMANDE", App.User.Token)
        {

        }

    }
}
