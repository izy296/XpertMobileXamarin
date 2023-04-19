using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using Xpert.Common.WSClient.Helpers;
using Xpert.Common.WSClient.Services;
using XpertMobileApp.DAL;

namespace XpertMobileApp.Api.ComWSManager
{
    public class ReceptionManager : CrudService<View_TRS_ENCAISS>
    {
        public ReceptionManager() : base(App.RestServiceUrl,
                Constants.AppName == Apps.XCOM_Mob || Constants.AppName == Apps.X_DISTRIBUTION ? "ACH_RECEPTION_XCOM" : "ACH_RECEPTION", App.User.Token)
        {
        }

        public async Task<bool> SyncAchats(List<View_ACH_DOCUMENT> purchases)
        {
            string url = GetActionUrl("SyncAchats");
            return await WSApi2.PostAauthorizedValue<bool, List<View_ACH_DOCUMENT>>(url, purchases, this.Token.access_token);
        }
    }
}
