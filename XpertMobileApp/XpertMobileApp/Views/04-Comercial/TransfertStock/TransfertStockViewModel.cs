using Acr.UserDialogs;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
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
using XpertMobileApp.Services;
using XpertMobileApp.SQLite_Managment;

namespace XpertMobileApp.ViewModels
{
    public class TransfertStockViewModel : CrudBaseViewModel2<STK_TRANSFERT, View_STK_TRANSFERT>
    {
        private ObservableCollection<View_STK_STOCK> itemStock;
        public ObservableCollection<View_STK_STOCK> ItemStock
        {
            get
            { return itemStock; }
            set
            {
                itemStock = value;
                OnPropertyChanged("ItemRows");
            }
        }

        public ItemRowsDetailViewModel<STK_STOCK, View_STK_STOCK> itemRowViewModel;
        string currentQB = null;
        public DateTime StartDate { get; set; }//DateTime.ParseExact("2020-03-21", "yyyy-MM-dd", System.Globalization.CultureInfo.InvariantCulture);
        public DateTime EndDate { get; set; } = DateTime.Now;

        public TransfertStockViewModel()
        {
            base.InitConstructor();
            StartDate = DateTime.Now;
        }

        protected override QueryInfos GetFilterParams()
        {
            base.GetFilterParams();

            this.AddCondition<View_STK_TRANSFERT, DateTime?>(e => e.DATE_TRANSEFRT, Operator.BETWEEN_DATE, StartDate, EndDate);
                this.AddConditionOperator(TypeConnector.AND, TypeParenthese.LEFT);
                this.AddCondition<View_STK_TRANSFERT, string>(e => e.MAGASIN_DESTINATION, App.CODE_MAGASIN);
                this.AddConditionOperator(TypeConnector.OR);
                this.AddCondition<View_STK_TRANSFERT, string>(e => e.MAGASIN_SOURCE, App.CODE_MAGASIN);
                this.AddConditionOperator(TypeParenthese.RIGHT);
            this.AddCondition<View_STK_TRANSFERT, bool>(e => e.IS_VALIDATE, false);
            this.AddOrderBy<View_STK_TRANSFERT, DateTime?>(e => e.CREATED_ON);
            return qb.QueryInfos;
        }
        public override async Task<List<View_STK_TRANSFERT>> SelectByPageFromSqlLite(QueryInfos filter)
        {
            var sqliteRes = await base.SelectByPageFromSqlLite(filter);
            if (StartDate == null)
                sqliteRes = sqliteRes.Where(e => StartDate.Date.CompareTo(((DateTime)e.DATE_TRANSEFRT).Date) <= 0 && EndDate.Date.CompareTo(((DateTime)e.DATE_TRANSEFRT).Date) >= 0).ToList();

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