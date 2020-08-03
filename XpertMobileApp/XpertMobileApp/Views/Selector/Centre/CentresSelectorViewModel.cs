using Acr.UserDialogs;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Xamarin.Forms;
using Xamarin.Forms.Extended;
using Xpert.Common.DAO;
using Xpert.Common.WSClient.Helpers;
using XpertMobileApp.Api.ViewModels;
using XpertMobileApp.DAL;
using XpertMobileApp.Services;

namespace XpertMobileApp.ViewModels
{
    public class CentresSelectorViewModel : CrudBaseViewModel2<TRS_TIERS, View_TRS_TIERS>
    {

        public string SearchedText { get; set; } = "";

        public string SearchedType { get; set; } = "";

        public CentresSelectorViewModel(string title= "", string type = "")
        {
            Title = title;
            SearchedType = type;
        }

        protected override void OnAfterLoadItems(IEnumerable<View_TRS_TIERS> list)
        {
            base.OnAfterLoadItems(list);

            View_TRS_TIERS fElem = new View_TRS_TIERS();
            fElem.NOM_TIERS1 = "";
            fElem.CODE_TIERS = "";
            Items.Insert(0, fElem);
        }

        protected override QueryInfos GetFilterParams()
        {
            base.GetFilterParams();

            this.AddCondition<View_TRS_TIERS, string>(e => e.CODE_TYPE, SearchedType);
            this.AddCondition<View_TRS_TIERS, string>(e => e.NOM_TIERS1, SearchedText);
            this.AddOrderBy<View_TRS_TIERS, string>(e => e.NOM_TIERS1);

            return qb.QueryInfos;
        }
    }
}
