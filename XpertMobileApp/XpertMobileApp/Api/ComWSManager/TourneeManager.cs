using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using Xpert.Common.WSClient.Helpers;
using Xpert.Common.WSClient.Model;
using Xpert.Common.WSClient.Services;
using XpertMobileApp.Api.Models;
using XpertMobileApp.DAL;

namespace XpertMobileApp.Api.Managers
{
    public class TourneeManager : CrudService<View_LIV_TOURNEE>
    {
        public TourneeManager()
    : base(App.RestServiceUrl, 
          Constants.AppName != Apps.X_DISTRIBUTION ? ControllerNameSwitch.GetControllerName(ControllerNameItem.LIV_TOURNEE)
          : ControllerNameItem.LIV_TOURNEE.ToString(), App.User.Token)
        {
        }

        public async Task<string> SyncTournee(List<View_LIV_TOURNEE> tournees)
        {
            string url = GetActionUrl("SyncTournee");
            return await WSApi2.PostAauthorizedValue<string, List<View_LIV_TOURNEE>>(url, tournees, this.Token.access_token);
        }

    }
}
