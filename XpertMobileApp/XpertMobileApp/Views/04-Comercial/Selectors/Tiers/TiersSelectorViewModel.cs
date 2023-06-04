using Acr.UserDialogs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Xamarin.Forms;
using Xamarin.Forms.Extended;
using Xpert.Common.DAO;
using Xpert.Common.WSClient.Helpers;
using XpertMobileApp.Api;
using XpertMobileApp.Api.ViewModels;
using XpertMobileApp.DAL;
using XpertMobileApp.Services;

namespace XpertMobileApp.ViewModels
{
    public class TiersSelectorViewModel : CrudBaseViewModel2<TRS_TIERS, View_TRS_TIERS>
    {

        public string SearchedText { get; set; } = "";

        public string SearchedType { get; set; } = "";

        public TiersSelectorViewModel(string title= "", string type = "")
        {
            Title = title;
            SearchedType = type;
        }

        protected override void OnAfterLoadItems(IEnumerable<View_TRS_TIERS> list)
        {
            base.OnAfterLoadItems(list);

            var nullItem = Items.Where(x => x.CODE_TIERS == "").FirstOrDefault();
            if(nullItem == null)
            { 
                View_TRS_TIERS fElem = new View_TRS_TIERS();
                fElem.NOM_TIERS1 = "";
                fElem.CODE_TIERS = "";
                Items.Insert(0, fElem);
            }
        }

        protected override QueryInfos GetFilterParams()
        {
            base.GetFilterParams();

            this.AddCondition<View_TRS_TIERS, short>(e => e.ACTIF_TIERS, 1);

            if (!string.IsNullOrEmpty(SearchedText)) 
            {
                this.AddConditionOperator(TypeConnector.AND, TypeParenthese.LEFT);
                this.AddCondition<View_TRS_TIERS, string>(e => e.NOM_TIERS1, Operator.LIKE_ANY, SearchedText);
                this.AddConditionOperator(TypeConnector.OR);
                this.AddCondition<View_TRS_TIERS, string>(e => e.NUM_TIERS, Operator.EQUAL, SearchedText);
                this.AddConditionOperator(TypeParenthese.RIGHT);
            }
            this.AddCondition<View_TRS_TIERS, string>(e => e.CODE_TYPE, SearchedType);

            qb.AddOrderBy<View_TRS_TIERS, string>(e => e.NOM_TIERS1);

            return qb.QueryInfos;
        }
    }
}
