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
    internal class ProductManager : CrudService<View_STK_PRODUITS>
    {
        public ProductManager() : 
            base(App.RestServiceUrl, "STK_PRODUITS", App.User?.Token)
        {

        }

        public async Task<List<View_STK_PRODUITS>> SelectByCodeBarre(string vparam1)
        {
            string url = GetActionUrl("ValidateVente");
            url += WSApi2.AddParam(url, "vparam1", vparam1);

            return await WSApi2.RetrievAauthorizedData<View_STK_PRODUITS>(url, this.Token.access_token);
        }
    }
}
