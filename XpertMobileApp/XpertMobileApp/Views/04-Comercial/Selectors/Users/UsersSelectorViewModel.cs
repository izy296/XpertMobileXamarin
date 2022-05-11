using Acr.UserDialogs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Xamarin.Forms;
using Xamarin.Forms.Extended;
using Xpert.Common.DAO;
using Xpert.Common.WSClient.Helpers;
using XpertMobileApp.Api.ViewModels;
using XpertMobileApp.DAL;
using XpertMobileApp.Models;
using XpertMobileApp.Services;

namespace XpertMobileApp.ViewModels
{
    public class UserSelectorViewModel : CrudBaseViewModel2<SYS_USER, View_SYS_USER>
    {

        public string SearchedText { get; set; } = "";

        public string SearchedType { get; set; } = "";

        public UserSelectorViewModel(string title= "", string type = "")
        {
            Title = title;
            SearchedType = type;
        }

        protected override void OnAfterLoadItems(IEnumerable<View_SYS_USER> list)
        {
            base.OnAfterLoadItems(list);

            var nullItem = Items.Where(x => x.ID_USER == "").FirstOrDefault();
            if(nullItem == null)
            {
                View_SYS_USER fElem = new View_SYS_USER();
                fElem.ID_USER= "";
                fElem.CODE_TIERS = "";
                Items.Insert(0, fElem);
            }
        }

        protected override QueryInfos GetFilterParams()
        {
            base.GetFilterParams();

            this.AddCondition<SYS_USER, string>(e => e.ID_USER, Operator.LIKE_ANY, SearchedText);

            return qb.QueryInfos;
        }

        protected override QueryInfos GetSelectParams()
        {
            base.GetSelectParams();

            return qb.QueryInfos;
        }
    }
}
