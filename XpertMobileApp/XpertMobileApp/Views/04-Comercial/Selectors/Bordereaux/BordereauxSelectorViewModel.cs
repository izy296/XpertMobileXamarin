using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Text;
using System.Threading.Tasks;
using Xamarin.Forms;
using Xamarin.Forms.Extended;
using Xpert.Common.DAO;
using XpertMobileApp.Api.ViewModels;
using XpertMobileApp.DAL;
using XpertMobileApp.Services;

namespace XpertMobileApp.Views
{
    public class BordereauxSelectorViewModel : CrudBaseViewModel2<CFA_BORDEREAU, View_CFA_BORDEREAUX_CHIFA>
    {
        public string SearchedText { get; set; } = "";

        public string CodeTiers { get; set; } = "";

        public bool AutoriserReception { get; set; }
        public bool CanLoadMore { get; set; } = true;
        public BordereauxSelectorViewModel(string title = "")
        {
            Title = title;
            LoadItemsCommand = new Command(async async => { await ExecuteLoadItemsCommand(); });
        }

        protected override void OnAfterLoadItems(IEnumerable<View_CFA_BORDEREAUX_CHIFA> list)
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
            this.AddOrderBy<View_CFA_BORDEREAUX_CHIFA, string>(e => e.NUM_BORDEREAU);
            return qb.QueryInfos;
        }

        internal override async Task ExecuteLoadItemsCommand()
        {
            qb.InitQuery();
            if (SearchedText != "")
                qb.AddCondition<View_CFA_BORDEREAUX_CHIFA, string>(e => e.NUM_BORDEREAU,Operator.LIKE_ANY, SearchedText);

            qb.AddCondition<View_CFA_BORDEREAUX_CHIFA, string>(e => e.NUM_BORDEREAU, Operator.NOT_EQUAL, "VIDE");
            qb.AddOrderBy<View_CFA_BORDEREAUX_CHIFA, string>(e => e.NUM_BORDEREAU);
            if (Items.Count == 0)
            {
                var totalCount = await WebServiceClient.GetCFABordereauxCount(qb.QueryInfos);
                CanLoadMore = true;
                qb.AddPaging(1, 10);
                Items = new InfiniteScrollCollection<View_CFA_BORDEREAUX_CHIFA>(await WebServiceClient.GetCFABordereaux(qb.QueryInfos));
                if (Items.Count >= totalCount)
                {
                    CanLoadMore = false;
                }
            }
            else
            {
                if (CanLoadMore)
                {
                    var totalCount = await WebServiceClient.GetCFABordereauxCount(qb.QueryInfos);
                    var page = ((int)Math.Round((decimal)Items.Count / 10));
                    page += 1;
                    qb.AddPaging(page, 10);
                    Items.AddRange(new InfiniteScrollCollection<View_CFA_BORDEREAUX_CHIFA>(await WebServiceClient.GetCFABordereaux(qb.QueryInfos)));

                    if (Items.Count >= totalCount)
                    {
                        CanLoadMore = false;
                    }
                }
            }
            OnPropertyChanged("Items");
        }

    }
}
