using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using Xpert.Common.WSClient.Helpers;
using Xpert.Common.WSClient.Model;
using Xpert.Common.WSClient.Services;
using XpertMobileApp.DAL;

namespace XpertMobileApp.Api.Managers
{
    public class VentesManager : CrudService<View_VTE_VENTE>
    {
        public VentesManager(string controleurName) 
            : base(App.RestServiceUrl, controleurName, App.User.Token)
        {
        }

        public async Task<string> ValidateVente(View_VTE_VENTE vte, string vparam1)
        {
            string url = GetActionUrl("ValidateVente");
            url += WSApi2.AddParam(url, "vparam1", vparam1);
            return await WSApi2.PostAauthorizedValue<string, View_VTE_VENTE>(url, vte, this.Token.access_token);
        }

        public async Task<string> SyncVentes(List<View_VTE_VENTE> vtes)
        {
            string url = GetActionUrl("SynchronisationVente");
            return await WSApi2.PostAauthorizedValue<string, List<View_VTE_VENTE>>(url, vtes , this.Token.access_token);
        }

        public async Task<VIEW_FIDELITE_INFOS> GetFideliteInfos(string CodeCard, decimal PointUsed)
        {
            string url = GetActionUrl("GetFideliteInfos");
            url += WSApi2.AddParam(url, "paramv", "1");
            VIEW_FIDELITE_INFOS obj = new VIEW_FIDELITE_INFOS();
            obj.CODE_CARD = CodeCard;
            obj.POINTS_USED = PointUsed;
            obj = await WSApi2.PostAauthorizedValue<VIEW_FIDELITE_INFOS, VIEW_FIDELITE_INFOS>(url, obj, this.Token.access_token);

            return obj;
        }
    }
}
