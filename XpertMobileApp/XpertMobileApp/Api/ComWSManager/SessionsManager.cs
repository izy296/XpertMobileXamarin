using System.Collections.Generic;
using System.Threading.Tasks;
using Xpert.Common.WSClient.Helpers;
using Xpert.Common.WSClient.Services;
using XpertMobileApp.DAL;
using XpertMobileApp.Models;
using XpertMobileApp.Api.Models;

namespace XpertMobileApp.Api.Managers
{
    
    class SessionsManager : CrudService<TRS_JOURNEES>
    {
        public SessionsManager() :
            base(App.RestServiceUrl, ControllerNameSwitch.GetControllerName(ControllerNameItem.TRS_JOURNEES), App.User.Token)
        {
            
        }

        public async Task<TRS_JOURNEES> GetCurrentSession() 
        {
            string url = GetActionUrl("GetCurrentSession");
 
            return await WSApi2.RetrievAauthorizedValue<TRS_JOURNEES>(url, this.Token.access_token);
        }

        public async Task<bool> OpenSession(SESSION_INFO infosSession) 
        {
            string url = GetActionUrl("OpenSession");
            // url += WSApi2.AddParam(url, "vparam1", vparam1);
            return await WSApi2.PostAauthorizedValue<bool, SESSION_INFO>(url, infosSession, this.Token.access_token);
        }

        public bool CloseSession(TRS_JOURNEES session) 
        {
            return false;
        }

    }
}


