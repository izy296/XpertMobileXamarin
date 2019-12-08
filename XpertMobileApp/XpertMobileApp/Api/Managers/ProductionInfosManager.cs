using System.Threading.Tasks;
using Xpert.Common.WSClient.Helpers;
using Xpert.Common.WSClient.Services;
using XpertMobileApp.DAL;

namespace XpertMobileApp.Api
{
    internal class ProductionInfosManager : CrudService<View_PRD_AGRICULTURE_DETAIL_INFO>
    {
        public ProductionInfosManager() : 
            base(App.RestServiceUrl, "PRD_AGRICULTURE_DETAIL_INFO", App.User.Token)
        {

        }


        public async Task<View_PRD_AGRICULTURE> GetProductionFromCodeReception(string codeReception)
        {
            string url = GetActionUrl("GetProductionFromCodeReception");
            url += WSApi2.AddParam(url, "codeReception", codeReception);

            return await WSApi2.RetrievAauthorizedValue<View_PRD_AGRICULTURE>(url, this.Token.access_token);
        }

    }
}
