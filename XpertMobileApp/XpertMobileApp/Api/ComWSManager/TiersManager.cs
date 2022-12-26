using System.Collections.Generic;
using System.Threading.Tasks;
using Xpert.Common.WSClient.Helpers;
using Xpert.Common.WSClient.Services;
using XpertMobileApp.Api.Models;
using XpertMobileApp.DAL;

namespace XpertMobileApp.Api
{
    internal class TiersManager : CrudService<View_TRS_TIERS>
    {
        public TiersManager() : 
            base(App.RestServiceUrl, ControllerNameSwitch.GetControllerName(ControllerNameItem.TRS_TIERS), App.User.Token)
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

        public async Task<decimal> getPointfidelite(string NumCardFDtiers = "")
        {
            string url = GetActionUrl("GetFDParamPoints");
            url += WSApi2.AddParam(url, "NumCardFDtiers", NumCardFDtiers);
            return await WSApi2.RetrievAauthorizedValue<decimal>(url, this.Token.access_token);
        }

    }
}
