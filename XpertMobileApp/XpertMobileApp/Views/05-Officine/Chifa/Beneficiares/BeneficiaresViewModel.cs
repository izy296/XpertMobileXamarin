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
using Xamarin.Forms.Extended;

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

        public bool FactureLoadMore { get; set; } = true;
        public DateTime StartDate { get; set; } = DateTime.Now.AddMonths(-1);
        public DateTime EndDate { get; set; } = DateTime.Now;

        // -1 order by disabled
        // 0 order by num asc
        // 1 order by num desc
        // 2 order by nbr asc
        // 3 order by nbr desc
        // 4 order by montant asc
        // 5 order by montant desc
        private int orderBy { get; set; } = 0;
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
        private bool isRefreshing { get; set; } = false;
        public bool IsRefreshing
        {
            get
            {
                return isRefreshing;
            }
            set
            {
                isRefreshing = value;
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
            LoadItemsMoreCommand = new Command(async () => { await ExecuteLoadItemsCommand(); });
        }

        protected override QueryInfos GetSelectParams()
        {
            base.GetSelectParams();
            return qb.QueryInfos;
        }

        protected override QueryInfos GetFilterParams()
        {
            base.GetFilterParams();
            if (!string.IsNullOrEmpty(SearchText))
                this.AddCondition<View_CFA_MOBILE_DETAIL_FACTURE, string>(e => e.NOMC_TIERS, Operator.LIKE_ANY, SearchText);
            this.AddSelect("NUM_ASSURE,NOMC_TIERS,CODE_TIERS,RAND_AD,COUNT(NUM_ASSURE) TOTAL_FACTURES,SUM(MONT_FACTURE) MONTANT_FACTURES,DATE_FACTURE");
            this.AddCondition<View_CFA_MOBILE_DETAIL_FACTURE, DateTime>(e => e.DATE_FACTURE, Operator.BETWEEN_DATE, StartDate, EndDate);
            this.AddGroupBy("NUM_ASSURE,RAND_AD,NOMC_TIERS,DATE_FACTURE,CODE_TIERS");
            if (OrderBy == 0)
                this.AddOrderBy("NUM_ASSURE", Sort.ASC);
            else if (OrderBy ==1)
                this.AddOrderBy("NUM_ASSURE", Sort.DESC);
            else if (OrderBy == 2)
                this.AddOrderBy("MONTANT_FACTURES", Sort.ASC);
            else if (OrderBy == 3)
                this.AddOrderBy("MONTANT_FACTURES", Sort.DESC);
            else if (OrderBy == 4)
                this.AddOrderBy("TOTAL_FACTURES", Sort.ASC);
            else if (OrderBy == 5)
                this.AddOrderBy("TOTAL_FACTURES", Sort.DESC);
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
                var res = await WebServiceClient.GetCfa_Beneficaires_Summary(StartDate, EndDate);
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
                if (Items.Count == 0)
                {
                    FactureLoadMore = true;
                    await ExecuteLoadSummaries();
                    UserDialogs.Instance.ShowLoading();
                    Items = new InfiniteScrollCollection<View_CFA_MOBILE_DETAIL_FACTURE>(new InfiniteScrollCollection<View_CFA_MOBILE_DETAIL_FACTURE>(await WebServiceClient.SelectBeneficiares(search: SearchText, orderBy: OrderBy, startDate: StartDate, endDate: EndDate)));
                    var count = await WebServiceClient.SelectBeneficiaresCount(search: SearchText, orderBy: OrderBy, startDate: StartDate, endDate: EndDate);
                    if (Items.Count >= count)
                        FactureLoadMore = false;
                    UserDialogs.Instance.HideLoading();
                }
                else
                {
                    if (FactureLoadMore)
                    {
                        UserDialogs.Instance.ShowLoading();
                        var currnetPage = (int)(Math.Round((decimal)(Items.Count / 10)));
                        var page = currnetPage + 1;

                        var count = await WebServiceClient.SelectBeneficiaresCount(search: SearchText, orderBy: OrderBy, startDate: StartDate, endDate: EndDate, page: page);

                        var list = new InfiniteScrollCollection<View_CFA_MOBILE_DETAIL_FACTURE>(new InfiniteScrollCollection<View_CFA_MOBILE_DETAIL_FACTURE>(await WebServiceClient.SelectBeneficiares(search: SearchText,orderBy:OrderBy, startDate: StartDate, endDate: EndDate,page:page)));
                        Items.AddRange(list);

                        if (Items.Count >= count)
                            FactureLoadMore = false;
 
                        UserDialogs.Instance.HideLoading();
                    }

                }
                OnPropertyChanged("Items");
                IsBusy = false;
            }
            catch (Exception ex)
            {
                UserDialogs.Instance.HideLoading();
                CustomPopup AlertPopup = new CustomPopup(WSApi2.GetExceptionMessage(ex), trueMessage: AppResources.alrt_msg_Ok);
                await PopupNavigation.Instance.PushAsync(AlertPopup);
            }
        }

        public async Task ExecuteSearch(string SearchBarText)
        {
            SearchText = SearchBarText;
            Items.Clear();
            await ExecuteLoadItemsCommand();
        }

    }
}
