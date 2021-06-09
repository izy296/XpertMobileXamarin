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
    internal class EncaissManager : CrudService<View_TRS_ENCAISS>
    {
        public EncaissManager() : 
            base(App.RestServiceUrl, "TRS_ENCAISS", App.User.Token)
        {

        }

        public async Task<string> SyncEncaiss(List<View_TRS_ENCAISS> encaiss)
        {
            string url = GetActionUrl("SyncEncaiss");
            return await WSApi2.PostAauthorizedValue<string, List<View_TRS_ENCAISS>>(url, encaiss, this.Token.access_token);
        }
    }
}
