using Acr.UserDialogs;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Xamarin.Forms;
using Xamarin.Forms.Extended;
using Xpert.Common.DAO;
using Xpert.Common.WSClient.Helpers;
using XpertMobileApp.Api;
using XpertMobileApp.Api.ViewModels;
using XpertMobileApp.DAL;
using XpertMobileApp.Models;
using XpertMobileApp.Services;

namespace XpertMobileApp.ViewModels
{
    public class ProductManaquantSelectorViewModel : CrudBaseViewModel2<STK_PRODUITS, View_STK_PRODUITS>
    {
        public string SearchedText { get; set; } = "";
        public string CodeTiers { get; set; } = "";
        public bool AutoriserReception { get; set; }
        public string SearchedType { get; set; } = "";
        public ProductManaquantSelectorViewModel(string title = "", string type = "")
        {
            Title = title;
            SearchedType = type;
        }
        protected override void OnAfterLoadItems(IEnumerable<View_STK_PRODUITS> list)
        {
            base.OnAfterLoadItems(list);            
        }
        protected override QueryInfos GetFilterParams()
        {
            base.GetFilterParams();
            this.AddCondition<View_STK_PRODUITS, string>(e => e.DESIGNATION_PRODUIT, Operator.LIKE_ANY, SearchedText);
            if (AutoriserReception)
            {
                this.AddCondition<View_STK_PRODUITS, bool>(e => e.AUTORISER_RECEPTIONS, 1) ;
            }
            this.AddOrderBy<View_STK_PRODUITS, string>(e => e.DESIGNATION_PRODUIT , Sort.ASC);
            return qb.QueryInfos;
        }
    }
}
