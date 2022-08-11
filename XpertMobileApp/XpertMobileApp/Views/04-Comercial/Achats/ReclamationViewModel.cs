using Acr.UserDialogs;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Threading.Tasks;
using Xamarin.Forms;
using Xamarin.Forms.Extended;
using Xpert.Common.DAO;
using Xpert.Common.WSClient.Helpers;
using XpertMobileApp.Api;
using XpertMobileApp.Api.ViewModels;
using XpertMobileApp.DAL;
using XpertMobileApp.Services;
using XpertMobileApp.SQLite_Managment;

namespace XpertMobileApp.ViewModels
{

    public class ReclamationViewModel : CrudBaseViewModel2<ACH_RECLAMATIONS, View_ACH_RECLAMATIONS>
    {
        public string bla;
        string CODE_ENTREE_DETAIL;
        public ReclamationViewModel(string CODE_ENTREE_DETAIL)
        {
            base.InitConstructor();
            this.CODE_ENTREE_DETAIL = CODE_ENTREE_DETAIL;
        }
        protected override string ContoleurName
        {
            get
            {
                return "ACH_RECLAMATIONS";
            }
        }

        protected override QueryInfos GetFilterParams()
        {
            base.GetFilterParams();
            this.AddOrderBy<View_ACH_RECLAMATIONS, string>(e => e.CODE_FACTURE);
            this.AddCondition<View_ACH_RECLAMATIONS, string>(e => e.CODE_ENTREE_DETAIL, CODE_ENTREE_DETAIL);
            return qb.QueryInfos;
        }

        protected override QueryInfos GetSelectParams()
        {
            base.GetSelectParams();
            return qb.QueryInfos;
        }
    }

}
