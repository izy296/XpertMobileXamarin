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
    internal class SYS_MACHINE_CONFIG_Manager : CrudService<SYS_CONFIGURATION_MACHINE>
    {
        public SYS_MACHINE_CONFIG_Manager() :
            base(App.RestServiceUrl, "SYS_MachineOffLine")
        {

        }

        public async Task<bool> AddMachine(SYS_CONFIGURATION_MACHINE Machine)
        {
            try
            {
                string url = GetActionUrl("AddMachine");
                return await WSApi2.PostAauthorizedValueWithoutToken<bool, SYS_CONFIGURATION_MACHINE>(url, Machine);
            }
            catch (Exception ex)
            {
                await UserDialogs.Instance.AlertAsync(WSApi2.GetExceptionMessage(ex), AppResources.alrt_msg_Alert,
                    AppResources.alrt_msg_Ok);

                return false;
            }

        }

        public async Task<SYS_CONFIGURATION_MACHINE> GetPrefix(string Machine)
        {
            string url = GetActionUrl("GetPrefixe");
            url += WSApi2.AddParam(url, "Machine", Machine);
            return await WSApi2.RetrievAauthorizedValueWithoutToken<SYS_CONFIGURATION_MACHINE>(url);
        }
    }
}
