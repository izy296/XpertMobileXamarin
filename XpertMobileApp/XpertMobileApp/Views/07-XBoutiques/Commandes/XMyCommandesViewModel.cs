using Acr.UserDialogs;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Threading.Tasks;
using Xamarin.Forms;
using Xamarin.Forms.Extended;
using Xpert.Common.DAO;
using Xpert.Common.WSClient.Helpers;
using XpertMobileApp.Api.ViewModels;
using XpertMobileApp.DAL;
using XpertMobileApp.Services;

namespace XpertMobileApp.ViewModels
{

    public class XMyCommandesViewModel : CrudBaseViewModel2<COMMANDES, COMMANDES>
    {
        #region Filtres

        public DateTime StartDate { get; set; } = DateTime.Now.AddDays(-60);
        public DateTime EndDate { get; set; } = DateTime.Now;

        #endregion

        public XMyCommandesViewModel()
        {
            Title = AppResources.pn_MyCommandes;
        }

        protected override string ContoleurName
        {
            get
            {
                return "COMMANDES";
            }
        }

        protected override QueryInfos GetFilterParams()
        {
            base.GetFilterParams();

            this.AddCondition<COMMANDES, DateTime?>(e => e.CREATED_ON, Operator.BETWEEN_DATE, StartDate, EndDate);


           // this.AddCondition<COMMANDES, string>(e => e.ID_USER, App.User.Token.userID);

            this.AddOrderBy<COMMANDES, DateTime?>(e => e.CREATED_ON, Sort.DESC);

            return qb.QueryInfos;
        }

        protected override void OnAfterLoadItems(IEnumerable<COMMANDES> list)
        {
            base.OnAfterLoadItems(list);

            int i = 0;
            foreach (var item in list)
            {
                i += 1;
                (item as BASE_CLASS).Index = i;
            }
        }

        async Task ExecuteLoadItemsCommand()
        {
            if (IsBusy)
                return;

            try
            {
                Items.Clear();

                // liste des ventes
                await Items.LoadMoreAsync();
            }
            catch (Exception ex)
            {
                await UserDialogs.Instance.AlertAsync(WSApi2.GetExceptionMessage(ex), AppResources.alrt_msg_Alert,
                    AppResources.alrt_msg_Ok);
            }
            finally
            {
                IsBusy = false;
            }
        }
    }

}
