using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using Xpert.Common.WSClient.Helpers;
using Xpert.Common.WSClient.Model;
using Xpert.Common.WSClient.Services;
using XpertMobileApp.DAL;

namespace XpertMobileApp.Api.Managers
{
    public class VTE_VENTE_BLL : CrudService<View_VTE_VENTE>
    {
        public VTE_VENTE_BLL(string controleurName) 
            : base(App.RestServiceUrl, controleurName, App.User.Token)
        {
        }

        public async Task<bool> ValidateVente(View_VTE_VENTE vte, string vparam1)
        {
            string url = GetActionUrl("ValidateVente");
            url += WSApi2.AddParam(url, "vparam1", vparam1);
            return await WSApi2.PostAauthorizedValue<bool, View_VTE_VENTE>(url, vte, this.Token.access_token);
        }
    }
}
