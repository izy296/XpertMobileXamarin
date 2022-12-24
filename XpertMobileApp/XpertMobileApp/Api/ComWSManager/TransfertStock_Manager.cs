using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using Xpert.Common.WSClient.Helpers;
using Xpert.Common.WSClient.Model;
using Xpert.Common.WSClient.Services;
using XpertMobileApp.Api.Models;
using XpertMobileApp.DAL;
using XpertMobileApp.Models;

namespace XpertMobileApp.Api
{
    internal class TransfertStock_Manager : CrudService<View_STK_STOCK>
    {
        public TransfertStock_Manager() :
            base(App.RestServiceUrl, ControllerNameSwitch.GetControllerName(ControllerNameItem.STK_TRANSFERT), App.User.Token)
        {

        }

        public async Task<List<View_STK_STOCK>> SelectByCodeBarreLot(string codeBarre, string tiers, string CodeMagasin = "")
        {
            string url = GetActionUrl("GetByCodeBarreLot");
            url += WSApi2.AddParam(url, "codebarre", codeBarre);
            url += WSApi2.AddParam(url, "tiers", tiers);
            url += WSApi2.AddParam(url, "CodeMagasin", CodeMagasin);

            return await WSApi2.RetrievAauthorizedData<View_STK_STOCK>(url, this.Token.access_token);
        }

        public async Task<string> TestBetterLot(int? idStock)
        {
            string url = GetActionUrl("TestBetterLot");
            url += WSApi2.AddParam(url, "idStock", idStock.ToString());
            return await WSApi2.RetrievAauthorizedValue<string>(url, this.Token.access_token);
        }

        public async Task<bool> InsertTransfert(View_STK_TRANSFERT transfers)
        {
            string url = GetActionUrl("InsertTransfert");
            return await WSApi2.PostAauthorizedValue<bool, View_STK_TRANSFERT>(url, transfers, this.Token.access_token);
        }
    }
}
