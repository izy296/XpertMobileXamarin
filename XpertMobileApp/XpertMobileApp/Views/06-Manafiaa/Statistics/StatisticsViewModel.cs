using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Xpert.Common.DAO;
using XpertMobileApp.Api.ViewModels;
using XpertMobileApp.DAL;
using XpertMobileApp.Models;
using XpertMobileAppManafiaa.SQLite_Managment;

namespace XpertMobileApp.ViewModels
{

    public class StatisticsViewModel : CrudBaseViewModel2<View_STK_STATISTICS, View_STK_STATISTICS>
    {
        public StatisticsViewModel()
        {
            Title = AppResources.pn_Statistics;
        }

        protected override QueryInfos GetFilterParams()
        {
            base.GetFilterParams();

            if (!string.IsNullOrEmpty(searchedRef))
                this.AddCondition<View_STK_STATISTICS, string>(e => e.DESIGNATION, searchedRef);

            this.AddOrderBy<View_STK_STATISTICS, string>(e => e.DESIGNATION);
            return qb.QueryInfos;
        }

        public override async Task<List<View_STK_STATISTICS>> SelectByPageFromSqlLite(QueryInfos filter)
        {
            var res = await SyncManager.GetStatistics();
            if (!string.IsNullOrEmpty(searchedRef))
                res = res.Where(e => e.DESIGNATION.ToLower().StartsWith(searchedRef.ToLower()) 
                || e.DESIGNATION.ToLower().Equals(searchedRef.ToLower()) 
                || e.DESIGNATION.ToLower().Contains(searchedRef.ToLower()) 
                || e.DESIGNATION.ToLower().EndsWith(searchedRef.ToLower())).ToList();
            return res;
        }

        protected override void OnAfterLoadItems(IEnumerable<View_STK_STATISTICS> list)
        {
            base.OnAfterLoadItems(list);

            int i = 0;
            foreach (var item in list)
            {
                i += 1;
                (item as BASE_CLASS).Index = i;
            }
        }

        #region filters data

        private string searchedRef;
        public string SearchedRef
        {
            get { return searchedRef; }
            set { SetProperty(ref searchedRef, value); }
        }

        public override void ClearFilters()
        {
            base.ClearFilters();
            searchedRef = "";
        }

        #endregion
    }

}
