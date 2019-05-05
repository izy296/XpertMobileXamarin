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
    public class ProductSelectorViewModel : CrudBaseViewModel<STK_PRODUITS, View_STK_PRODUITS>
    {

        public string SearchedText { get; set; } = "";

        public ProductSelectorViewModel(string title= "" )
        {
            Title = title;
        }

        protected override void OnAfterLoadItems(IEnumerable<View_STK_PRODUITS> list)
        {
            base.OnAfterLoadItems(list);
        }

        protected override Dictionary<string, string> GetFilterParams()
        {
            Dictionary<string, string> result = base.GetFilterParams();

            result.Add("searchText", SearchedText);
            return result;
        }
    }
}
