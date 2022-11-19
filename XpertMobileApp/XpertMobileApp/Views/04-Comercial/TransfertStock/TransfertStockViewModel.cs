using Acr.UserDialogs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xamarin.Forms;
using Xpert.Common.DAO;
using Xpert.Common.WSClient.Helpers;
using XpertMobileApp.Api.Services;
using XpertMobileApp.Api.ViewModels;
using XpertMobileApp.DAL;
using XpertMobileApp.Models;
using XpertMobileApp.SQLite_Managment;

namespace XpertMobileApp.ViewModels
{
    public class TransfertStockViewModel : CrudBaseViewModel2<STK_TRANSFERT, View_STK_TRANSFERT>
    {

        string currentQB = null;
        public DateTime StartDate { get; set; } = DateTime.Now;//DateTime.ParseExact("2020-03-21", "yyyy-MM-dd", System.Globalization.CultureInfo.InvariantCulture);
        public DateTime EndDate { get; set; } = DateTime.Now;


        public TransfertStockViewModel()
        {
            base.InitConstructor();
        }


        protected override QueryInfos GetFilterParams()
        {
            base.GetFilterParams();

            this.AddCondition<View_STK_TRANSFERT, DateTime?>(e => e.DATE_TRANSEFRT, Operator.BETWEEN_DATE, StartDate, EndDate);
            this.AddCondition<View_STK_TRANSFERT, string>(e => e.MAGASIN_DESTINATION, App.CODE_MAGASIN);
            this.AddCondition<View_STK_TRANSFERT, bool>(e => e.IS_VALIDATE, false);

            this.AddOrderBy<View_STK_TRANSFERT, DateTime?>(e => e.CREATED_ON);
            return qb.QueryInfos;
        }
        public override async Task<List<View_STK_TRANSFERT>> SelectByPageFromSqlLite(QueryInfos filter)
        {
            var sqliteRes = await base.SelectByPageFromSqlLite(filter);

            sqliteRes = sqliteRes.Where(e => e.DATE_TRANSEFRT >= StartDate && e.DATE_TRANSEFRT <= EndDate).ToList();

            return sqliteRes;
        }

        internal override async Task ExecuteLoadItemsCommand()
        {
            if (IsBusy)
                return;

            try
            {
                IsBusy = true;
                if (currentQB != null && currentQB != GetFilterParams().StringCondition)
                {
                    currentQB = GetFilterParams().StringCondition;
                    Items.Clear();
                }
                else
                {
                    if (Items.Count >= ElementsCount && Items.Count != 0)
                        return;
                    currentQB = GetFilterParams().StringCondition;
                }
                await Items.LoadMoreAsync();
                IsBusy = false;
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
        public Command PullTORefresh
        {
            get
            {
                return new Command(async () =>
                {
                    if (IsBusy)
                    {
                        return;
                    }
                    currentQB = "Empty";
                    await ExecuteLoadItemsCommand();
                    IsBusy = false;
                });
            }
        }
    }
}