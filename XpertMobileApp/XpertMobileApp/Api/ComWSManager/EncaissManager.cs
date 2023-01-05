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
            base(App.RestServiceUrl,
                Constants.AppName == Apps.XCOM_Mob || Constants.AppName == Apps.X_DISTRIBUTION ? "TRS_ENCAISS_XCOM" : "TRS_ENCAISS", 
                App.User.Token)
        {

        }

        public async Task<bool> SyncEncaiss(List<View_TRS_ENCAISS> encaiss)
        {
            string url = GetActionUrl("SyncEncaiss");
            return await WSApi2.PostAauthorizedValue<bool, List<View_TRS_ENCAISS>>(url, encaiss, this.Token.access_token);
        }
    }
}
