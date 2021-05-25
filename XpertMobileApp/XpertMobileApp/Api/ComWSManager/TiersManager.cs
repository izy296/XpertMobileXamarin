using System.Collections.Generic;
using System.Threading.Tasks;
using Xpert.Common.WSClient.Helpers;
using Xpert.Common.WSClient.Services;
using XpertMobileApp.DAL;

namespace XpertMobileApp.Api
{
    internal class TiersManager : CrudService<View_TRS_TIERS>
    {
        public TiersManager() : 
            base(App.RestServiceUrl, "TRS_TIERS", App.User.Token)
        {

        }
        public async Task<string> SyncTiers(List<View_TRS_TIERS> Tiers)
        {
            string url = GetActionUrl("SendTiers");
            return await WSApi2.PostAauthorizedValue<string, List<View_TRS_TIERS>>(url, Tiers, this.Token.access_token);
        }

        public async Task<bool> saveGPSToTiers(View_TRS_TIERS Tiers)
        {
            string url = GetActionUrl("UpdateGPSTiers");
            return await WSApi2.PostAauthorizedValue<bool, View_TRS_TIERS>(url, Tiers, this.Token.access_token);
        }
         
    }
}
