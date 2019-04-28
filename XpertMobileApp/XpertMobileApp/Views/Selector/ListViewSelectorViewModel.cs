using Acr.UserDialogs;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Xamarin.Forms;
using Xamarin.Forms.Extended;
using Xpert.Common.WSClient.Helpers;
using XpertMobileApp.Api.ViewModels;
using XpertMobileApp.DAL;
using XpertMobileApp.Services;

namespace XpertMobileApp.ViewModels
{
    public class ListViewSelectorViewModel : CrudBaseViewModel<TRS_TIERS, View_TRS_TIERS>
    {

        public string SearchedText { get; set; } = "";

        public ListViewSelectorViewModel(string title= "" )
        {
            Title = title;
        }

        protected override void OnAfterLoadItems(IEnumerable<View_TRS_TIERS> list)
        {
            base.OnAfterLoadItems(list);

            View_TRS_TIERS fElem = new View_TRS_TIERS();
            fElem.NOM_TIERS1 = "";
            fElem.CODE_TIERS = "";
            Items.Insert(0, fElem);
        }

        protected override Dictionary<string, string> GetFilterParams()
        {
            Dictionary<string, string> result = base.GetFilterParams();

            result.Add("type", "C");
            result.Add("SearchText", SearchedText);
            return result;
        }
    }
}
