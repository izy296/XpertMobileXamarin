using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Xpert.Common.DAO;
using XpertMobileApp.Api.ViewModels;
using XpertMobileApp.DAL;
using XpertMobileApp.Models;

namespace XpertMobileApp.Views._04_Comercial.Selectors.Lot
{
    class RetourProductViewModel : CrudBaseViewModel2<STK_STOCK, View_STK_STOCK>
    {

        public string SearchedText { get; set; } = "";

        public string CodeTiers { get; set; } = "";

        public string AutoriserReception { get; set; } = "";

        private decimal totalSelected;
        public decimal TotalSelected
        {
            get
            {
                return totalSelected;
            }
            set
            {
                totalSelected = value;
                OnPropertyChanged("TotalSelected");
                OnPropertyChanged("SelectionsInfos");
            }
        }

        public string SelectionsInfos
        {
            get
            {
                return "Total : " + TotalSelected.ToString("N2") + "\r\n" + "Total Qte : " + Items.Sum(e => e.SelectedQUANTITE);
            }
        }

        public RetourProductViewModel(string title = "")
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

            qb.QueryInfos.Param1 = CodeTiers;

            return qb.QueryInfos;
        }
    }
}

