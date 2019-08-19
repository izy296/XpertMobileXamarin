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
    internal class SYS_PERMISSIONS_Manager : CrudService<SYS_OBJET_PERMISSION>
    {
        public SYS_PERMISSIONS_Manager() : 
            base(App.RestServiceUrl, "SYS_OBJET_PERMISSION", App.User.Token)
        {

        }

        public async Task<List<SYS_OBJET_PERMISSION>> GetPermissions(string idGroup)
        {
            string url = GetActionUrl("GetPermissions");
            url += WSApi2.AddParam(url, "idGroup", idGroup);
            var result = await WSApi2.RetrievAauthorizedData<SYS_OBJET_PERMISSION>(url, this.Token.access_token);
            return result;
        }
    }
}
