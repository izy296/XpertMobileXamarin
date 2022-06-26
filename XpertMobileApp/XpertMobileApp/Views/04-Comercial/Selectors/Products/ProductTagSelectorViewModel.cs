using System;
using System.Collections.Generic;
using System.Text;
using Xamarin.Forms;
using Xpert.Common.DAO;
using XpertMobileApp.Api.ViewModels;
using XpertMobileApp.DAL;
using XpertMobileApp.Helpers;
using XpertMobileApp.Models;

namespace XpertMobileApp.ViewModels
{
    public class ProductTagSelectorViewModel : CrudBaseViewModel2<BSE_PRODUIT_TAG, BSE_PRODUIT_TAG>
    {
        public List<BSE_PRODUIT_TAG> SelectedItem { get; set; }

        public ProductTagSelectorViewModel()
        {

        }

        protected override QueryInfos GetFilterParams()
        {
            base.GetFilterParams();
            this.AddOrderBy<BSE_PRODUIT_TAG, string>(e => e.CODE);
            return qb.QueryInfos;
        }

        protected override QueryInfos GetSelectParams()
        {
            base.GetSelectParams();
            return qb.QueryInfos;
        }
    }
}
