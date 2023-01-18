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
        public async Task<bool> SyncCommandes(List<View_VTE_VENTE> vtes, string prefix, string CodeMagasin = "", string CodeCompte = "")
        {
            string url = GetActionUrl("SyncCommandes");
            url += WSApi2.AddParam(url, "prefix", prefix);
            url += WSApi2.AddParam(url, "CodeMagasin", CodeMagasin);
            url += WSApi2.AddParam(url, "CodeCompte", CodeCompte);
            return await WSApi2.PostAauthorizedValue<bool, List<View_VTE_VENTE>>(url, vtes, this.Token.access_token);
        }
    }
}
