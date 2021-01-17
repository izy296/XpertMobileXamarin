using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using Xpert.Common.WSClient.Helpers;
using Xpert.Common.WSClient.Model;
using Xpert.Common.WSClient.Services;
using XpertMobileApp.DAL;
using XpertMobileApp.Models;

namespace XpertMobileApp.Api
{
    internal class PanierManager : CrudService<View_PANIER>
    {
        public PanierManager() : 
            base(App.RestServiceUrl, "PANIER", App.User.Token)
        {

        }

    }
}
