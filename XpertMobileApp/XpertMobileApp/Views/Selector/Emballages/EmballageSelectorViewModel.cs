using Acr.UserDialogs;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Xamarin.Forms;
using Xamarin.Forms.Extended;
using Xpert.Common.WSClient.Helpers;
using XpertMobileApp.Api.ViewModels;
using XpertMobileApp.DAL;
using XpertMobileApp.Models;
using XpertMobileApp.Services;

namespace XpertMobileApp.ViewModels
{
    public class EmballageSelectorViewModel : CrudBaseViewModel<BSE_EMBALLAGE, View_BSE_EMBALLAGE>
    {

        public string SearchedText { get; set; } = "";

        public EmballageSelectorViewModel(string title= "" )
        {
            Title = title;
        }

        protected override void OnAfterLoadItems(IEnumerable<View_BSE_EMBALLAGE> list)
        {
            base.OnAfterLoadItems(list);

            View_BSE_EMBALLAGE fElem = new View_BSE_EMBALLAGE();
            fElem.DESIGNATION = "";
            fElem.CODE = "";
            Items.Insert(0, fElem);
        }

        protected override Dictionary<string, string> GetFilterParams()
        {
            Dictionary<string, string> result = base.GetFilterParams();

            result.Add("searchText", SearchedText);
            return result;
        }
    }
}
