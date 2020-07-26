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
using XpertMobileApp.Models;
using XpertMobileApp.Services;

namespace XpertMobileApp.ViewModels
{
    public class LotSelectorViewModel : CrudBaseViewModel2<STK_STOCK, View_STK_STOCK>
    {

        public string SearchedText { get; set; } = "";

        public string CodeTiers { get; set; } = "";

        public string AutoriserReception { get; set; } = "";
        
        public LotSelectorViewModel(string title= "" )
        {
            Title = title;
        }

        protected override void OnAfterLoadItems(IEnumerable<View_STK_STOCK> list)
        {
            base.OnAfterLoadItems(list);

            int i = 0;
            foreach (var item in list)
            {
                i += 1;
                (item as BASE_CLASS).Index = i;
            }
        }

        protected override QueryInfos GetFilterParams()
        {
            base.GetFilterParams();
            this.AddCondition<View_STK_STOCK, string>(e => e.DESIGNATION_PRODUIT, Operator.LIKE_ANY, SearchedText);
            if (!string.IsNullOrEmpty(App.Settings.DefaultMagasinVente)) 
            { 
                this.AddCondition<View_STK_STOCK, string>(e => e.CODE_MAGASIN, App.Settings.DefaultMagasinVente);
            }
            this.AddCondition<View_STK_STOCK, bool>(e => e.IS_BLOCKED, 0);
            this.AddCondition<View_STK_STOCK, decimal>(e => e.QUANTITE, Operator.GREATER, 0);
            
           // this.AddCondition<View_STK_STOCK, bool>(e => e.IS_VALID, 1);

            this.AddOrderBy<View_STK_STOCK, string>(e => e.DESIGNATION_PRODUIT);
            return qb.QueryInfos;
        }           
    }
}
