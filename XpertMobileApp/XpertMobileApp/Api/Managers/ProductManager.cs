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
            base(App.RestServiceUrl, "STK_PRODUITS", App.User.Token)
        {

        }

        public async Task<List<View_STK_PRODUITS>> SelectByCodeBarre(string codeBarre)
        {
            string url = GetActionUrl("GetByCodeBarre");
            url += WSApi2.AddParam(url, "codeBarre", codeBarre);

            return await WSApi2.RetrievAauthorizedData<View_STK_PRODUITS>(url, this.Token.access_token);
        }
    }
}
