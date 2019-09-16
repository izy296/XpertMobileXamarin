using Acr.UserDialogs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Xamarin.Forms;
using Xamarin.Forms.Extended;
using Xpert.Common.WSClient.Helpers;
using XpertMobileApp.Api.ViewModels;
using XpertMobileApp.DAL;
using XpertMobileApp.Services;

namespace XpertMobileApp.ViewModels
{
    public class TiersSelectorViewModel : CrudBaseViewModel<TRS_TIERS, View_TRS_TIERS>
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

        protected override Dictionary<string, string> GetFilterParams()
        {
            Dictionary<string, string> result = base.GetFilterParams();

            result.Add("type", SearchedType);
            result.Add("searchText", SearchedText);
            return result;
        }
    }
}
