using Acr.UserDialogs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xamarin.Forms.Internals;
using Xpert.Common.DAO;
using Xpert.Common.WSClient.Helpers;
using XpertMobileApp.Api;
using XpertMobileApp.Api.Managers;
using XpertMobileApp.Api.Services;
using XpertMobileApp.DAL;
using XpertMobileApp.Models;
using XpertMobileApp.SQLite_Managment;
using XpertMobileApp.ViewModels;

namespace XpertMobileApp.Views
{
    public class RSFormViewModel : ItemRowsDetailViewModel<View_STK_ENTREE, View_STK_ENTREE_DETAIL>
    {


        public RSFormViewModel(View_STK_ENTREE obj, string itemId) : base(obj, itemId)
        {

        }


        internal void InitNewEntree()
        {
            ItemRows.Clear();

            var entree = new View_STK_ENTREE();
            entree.ID_Random = XpertHelper.RandomString(7);
            entree.DATE_ENTREE = DateTime.Now.Date;
            Item = entree;

            SelectedTiers = new View_TRS_TIERS()
            {
                CODE_TIERS = "CXPERTCOMPTOIR",
                NOM_TIERS1 = "COMPTOIR"
            };
        }
    }
}
