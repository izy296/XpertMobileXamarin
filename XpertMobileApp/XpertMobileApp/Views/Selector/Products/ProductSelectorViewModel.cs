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

        public string CodeTiers { get; set; } = "";
        

        public ProductSelectorViewModel(string title= "" )
        {
            Title = title;
        }

        protected override void OnAfterLoadItems(IEnumerable<View_STK_PRODUITS> list)
        {
            base.OnAfterLoadItems(list);

            int i = 0;
            foreach (var item in list)
            {
                i += 1;
                item.IMAGE_URL = App.RestServiceUrl.Replace("api/", "") + string.Format("Images/GetImage?codeProduit={0}", item.CODE_PRODUIT);
                (item as BASE_CLASS).Index = i;
            }
        }

        protected override Dictionary<string, string> GetFilterParams()
        {
            Dictionary<string, string> result = base.GetFilterParams();
            
            result.Add("codeTiers", CodeTiers);
            result.Add("searchText", SearchedText);
            return result;
        }
    }
}
