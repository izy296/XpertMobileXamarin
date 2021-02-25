using Acr.UserDialogs;
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
    internal class SYS_PARAMS_Manager : CrudService<SYS_MOBILE_PARAMETRE>
    {
        public SYS_PARAMS_Manager() : 
            base(App.RestServiceUrl, "SYS_PARAMETRE", App.User.Token)
        {

        }

        public async Task<SYS_MOBILE_PARAMETRE> GetParams()
        {
            try
            {
                string url = GetActionUrl("GetParams");
                var res = await WSApi2.RetrievAauthorizedValue<SYS_MOBILE_PARAMETRE>(url, this.Token.access_token);
                return res;
            }
            catch (Exception ex)
            {
                await UserDialogs.Instance.AlertAsync(WSApi2.GetExceptionMessage(ex), AppResources.alrt_msg_Alert,
                    AppResources.alrt_msg_Ok);

                return null;
            }
        }
    }
}
