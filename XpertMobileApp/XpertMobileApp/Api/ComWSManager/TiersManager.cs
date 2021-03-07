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

    }
}
