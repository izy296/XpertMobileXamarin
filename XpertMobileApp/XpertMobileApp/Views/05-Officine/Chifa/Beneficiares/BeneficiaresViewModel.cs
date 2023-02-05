using Acr.UserDialogs;
using Rg.Plugins.Popup.Services;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Timers;
using System.Threading.Tasks;
using Xamarin.Forms;
using Xpert.Common.DAO;
using Xpert.Common.WSClient.Helpers;
using XpertMobileApp.Api.ViewModels;
using XpertMobileApp.DAL;
using XpertMobileApp.Services;
using XpertMobileApp.ViewModels;

namespace XpertMobileApp.Views._05_Officine.Chifa.Beneficiares
{
    public class BeneficiaresViewModel : CrudBaseViewModel2<CFA_MOBILE_DETAIL_FACTURE, View_CFA_MOBILE_DETAIL_FACTURE>
    {
        public Timer timer;

        private TimeSpan _totalSeconds = new TimeSpan(0, 0, 0, 2);

        public TimeSpan TotalSeconds

        {

            get { return _totalSeconds; }

            set { _totalSeconds = value; }

        }
        public DateTime StartDate { get; set; } = DateTime.Now.AddMonths(-1);
        public DateTime EndDate { get; set; } = DateTime.Now;

        // -1 order by disabled
        // 0 order by nom asc
        // 1 order by nom desc
        // 2 order by num asc
        // 3 order by num desc
        private int orderBy { get; set; } = -1;
        public int OrderBy
        {
            get
            {
                return orderBy;
            }
            set
            {
                orderBy = value;
                OnPropertyChanged("OrderBy");
            }
        }

        private string title { get; set; }
        public string Title
        {
            get
            {
                return title;
            }
            set
            {
                title = value;
                OnPropertyChanged("Title");
            }
        }
        private View_CFA_MOBILE_FACTURE summary { get; set; }
        public View_CFA_MOBILE_FACTURE Summary
        {
            get
            {
                return summary;
            }
            set
            {
                summary = value;
                OnPropertyChanged("Summary");
            }
        }
        private bool isRefresing { get; set; } = false;
        public bool IsRefreshing
        {
            get
            {
                return isRefresing;
            }
            set
            {
                isRefresing = value;
                OnPropertyChanged("IsRefreshing");
            }
        }

        private string searchText { get; set; }
        public string SearchText
        {
            get { return searchText; }
            set { searchText = value; OnPropertyChanged("SearchText"); }
        }


        public Command LoadItemsMoreCommand { get; set; }

        public BeneficiaresViewModel()
        {
            Title = AppResources.pn_BordereauxChifa;
            LoadItemsMoreCommand = new Command(async () => { await ExecuteLoadMoreItemsCommand(); });
        }

        protected override QueryInfos GetSelectParams()
        {
            return base.GetSelectParams();
        }

        protected override QueryInfos GetFilterParams()
        {
            base.GetFilterParams();
            if (!string.IsNullOrEmpty(SearchText))
                this.AddCondition<View_CFA_MOBILE_DETAIL_FACTURE, string>(e => e.NOMC_TIERS, Operator.LIKE_ANY, SearchText);
            this.AddCondition<View_CFA_MOBILE_DETAIL_FACTURE, DateTime>(e => e.DATE_FACTURE, Operator.BETWEEN_DATE, StartDate, EndDate);
            this.AddSelect("NUM_ASSURE,NOMC_TIERS,RAND_AD,COUNT(NUM_ASSURE) TOTAL_FACTURES,SUM(MONT_FACTURE) MONTANT_FACTURES,DATE_FACTURE");
            this.AddGroupBy("NUM_ASSURE,RAND_AD,NOMC_TIERS,DATE_FACTURE");
            if (OrderBy == -1)
                this.AddOrderBy<View_CFA_MOBILE_DETAIL_FACTURE, string>(e => e.NUM_ASSURE, Sort.DESC);
            else if (OrderBy == 0)
                this.AddOrderBy<View_CFA_MOBILE_DETAIL_FACTURE, string>(e => e.NOMC_TIERS, Sort.ASC);
            else if (OrderBy ==1)
                this.AddOrderBy<View_CFA_MOBILE_DETAIL_FACTURE, string>(e => e.NOMC_TIERS, Sort.DESC);
            else if (OrderBy ==2)
                this.AddOrderBy<View_CFA_MOBILE_DETAIL_FACTURE, string>(e => e.NUM_ASSURE, Sort.ASC);
            else if (OrderBy ==3)
                this.AddOrderBy<View_CFA_MOBILE_DETAIL_FACTURE, string>(e => e.NUM_ASSURE, Sort.DESC);
            return qb.QueryInfos;
        }

        protected override void OnAfterLoadItems(IEnumerable<View_CFA_MOBILE_DETAIL_FACTURE> list)
        {
            base.OnAfterLoadItems(list);

            int i = 0;
            foreach (var item in list)
            {
                i += 1;
                (item as BASE_CLASS).Index = i;
            }
        }

        async Task ExecuteLoadSummaries()
        {
            try
            {
                UserDialogs.Instance.ShowLoading();
                var res = await WebServiceClient.GetCfa_Beneficaires_Summary(StartDate.ToString("MM/dd/yyyy HH:mm:ss"), EndDate.ToString("MM/dd/yyyy HH:mm:ss"));
                Summary = res[0];
                UserDialogs.Instance.HideLoading();
            }
            catch (Exception ex)
            {
                CustomPopup AlertPopup = new CustomPopup(WSApi2.GetExceptionMessage(ex), trueMessage: AppResources.alrt_msg_Ok);
                await PopupNavigation.Instance.PushAsync(AlertPopup);
            }
        }

        internal override async Task ExecuteLoadItemsCommand()
        {
            try
            {
                if (IsBusy)
                    return;
                IsBusy = true;
                Items.Clear();
                await ExecuteLoadSummaries();
                UserDialogs.Instance.ShowLoading();
                await Items.LoadMoreAsync();
                UserDialogs.Instance.HideLoading();
            }
            catch (Exception ex)
            {
                CustomPopup AlertPopup = new CustomPopup(WSApi2.GetExceptionMessage(ex), trueMessage: AppResources.alrt_msg_Ok);
                await PopupNavigation.Instance.PushAsync(AlertPopup);
            }
            finally
            {
                IsBusy = false;
            }
        }
        public async Task ExecuteLoadMoreItemsCommand()
        {
            try
            {
                if (IsBusy)
                    return;
                IsBusy = true;
                UserDialogs.Instance.ShowLoading();
                await Items.LoadMoreAsync();
                UserDialogs.Instance.HideLoading();
            }
            catch (Exception ex)
            {
                CustomPopup AlertPopup = new CustomPopup(WSApi2.GetExceptionMessage(ex), trueMessage: AppResources.alrt_msg_Ok);
                await PopupNavigation.Instance.PushAsync(AlertPopup);
            }
            finally
            {
                IsBusy = false;
            }
        }

        public async Task ExecuteSearch(string SearchBarText)
        {
            SearchText = SearchBarText;
            await ExecuteLoadItemsCommand();
        }

        public async void t_Tick(object sender, EventArgs e)
        {
            if (TotalSeconds == new TimeSpan(0, 0, 0, 0))
            {
                //do something after hitting 0, in this example it just stops/resets the timer
                await ExecuteLoadItemsCommand();
                if (timer != null)
                {
                    timer.Stop();
                    timer.Dispose();
                }
                TotalSeconds = new TimeSpan(0, 0, 0, 2);
            }
            else
            {
                if (TotalSeconds != (new TimeSpan(0, 0, 0, 0)))
                    TotalSeconds = TotalSeconds.Subtract(new TimeSpan(0, 0, 0, 2));

            }
        }

    }
}
