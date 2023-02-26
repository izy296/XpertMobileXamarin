using Acr.UserDialogs;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using Xamarin.Forms;
using Xamarin.Forms.Extended;
using Xpert.Common.DAO;
using XpertMobileApp.Api.ViewModels;
using XpertMobileApp.DAL;
using XpertMobileApp.Services;

namespace XpertMobileApp.Views._05_Officine.Chifa.ClassificationLabo
{
    public enum ClassificationDisplayType { LABORATOIRES, THERAP };
    public class CHIFA_ClassificaitonByLaboViewModel : CrudBaseViewModel2<CFA_MOBILE_DETAIL_FACTURE, View_CFA_MOBILE_DETAIL_FACTURE>
    {
        public ClassificationDisplayType ClassificationDisplayType { get; set; }
        public Command LoadListLaboratory { get; set; }

        private bool laboPageIsVisible{ get; set; }
        public bool LaboPageIsVisible
        {
            get
            {
                return laboPageIsVisible;
            }
            set
            {
                laboPageIsVisible = value;
                OnPropertyChanged("LaboPageIsVisible");
            }
        }
        private bool therapPageIsVisible { get; set; }
        public bool TherapPageIsVisible
        {
            get
            {
                return therapPageIsVisible;
            }
            set
            {
                therapPageIsVisible = value;
                OnPropertyChanged("TherapPageIsVisible");
            }
        }
        private string searchLaboratory { get; set; }
        public string SearchLaboratory
        {
            get
            {
                return searchLaboratory;
            }
            set
            {
                searchLaboratory = value;
                OnPropertyChanged("SearchLaboratory");
            }
        }
        private string searchClassTherap { get; set; }
        public string SearchClassTherap
        {
            get
            {
                return searchClassTherap;
            }
            set
            {
                searchClassTherap = value;
                OnPropertyChanged("SearchClassTherap");
            }
        }
        public DateTime SelectedStartDate { get; set; } = DateTime.Now.AddYears(-1);
        public DateTime SelectedEndDate { get; set; } = DateTime.Now;
        private int ElementsCount { get; set; }
        public CHIFA_ClassificaitonByLaboViewModel()
        {
            LoadListLaboratory = new Command(async () => await ExecuteLoadLaboratoryList());
            Items = new InfiniteScrollCollection<View_CFA_MOBILE_DETAIL_FACTURE>
            {
                OnLoadMore = async () =>
                {
                    try
                    {

                        if (App.Online)
                        {

                            IsBusy = true;

                            ElementsCount = await WebServiceClient.GetCountLabo(GetFilterParams());

                            // load the next page
                            var page = (Items.Count / PageSize) + 1;

                            var items = await service.SelectByPage(GetSelectParams(), page, PageSize);

                            OnAfterLoadItems(items);

                            IsBusy = false;

                            // return the items that need to be added
                            return items;
                        }
                        return new List<View_CFA_MOBILE_DETAIL_FACTURE>();
                    }
                    catch (Exception ex)
                    {
                        await UserDialogs.Instance.AlertAsync(ex.Message, AppResources.alrt_msg_Alert, AppResources.alrt_msg_Ok);
                        return new List<View_CFA_MOBILE_DETAIL_FACTURE>();
                    }
                },
                OnCanLoadMore = () =>
                {
                    return Items.Count < ElementsCount;
                }

            };
        }

        async Task ExecuteLoadLaboratoryList()
        {
            try
            {
                if (IsBusy) return;
                IsBusy = true;
                Items.Clear();
                await Items.LoadMoreAsync();
            }
            catch (Exception ex)
            {
                throw ex;
            }
            finally
            {
                IsBusy = false;
            }

        }

        public string GetCurrentDisplayType()
        {
            string type = "";
            switch (ClassificationDisplayType)
            {
                case ClassificationDisplayType.LABORATOIRES:
                    type = "LABORATOIRES";
                    break;
                case ClassificationDisplayType.THERAP:
                    type = "THERAP";
                    break;
            }
            return type;
        }

        protected override QueryInfos GetFilterParams()
        {
            base.GetFilterParams();
            string typeDisplay = GetCurrentDisplayType();
            this.AddCondition<View_CFA_MOBILE_DETAIL_FACTURE, DateTime>(e => e.DATE_FACTURE, Operator.BETWEEN_DATE, SelectedStartDate, SelectedEndDate);

            if (typeDisplay == ClassificationDisplayType.LABORATOIRES.ToString())
            {
                if (!String.IsNullOrEmpty(SearchLaboratory))
                {
                    this.AddCondition<View_CFA_MOBILE_DETAIL_FACTURE, string>(e => e.DESIGNATION_LABO, Operator.EQUAL, searchLaboratory);
                }
                this.AddGroupBy("DESIGNATION_LABO, PAYE, DESIGN_DCI, CODE_DCI, CODE_PRODUIT");
                this.AddOrderBy<View_CFA_MOBILE_DETAIL_FACTURE, string>(e => e.DESIGNATION_LABO);
            }
            else
            {
                if (!String.IsNullOrEmpty(SearchClassTherap))
                {
                    this.AddCondition<View_CFA_MOBILE_DETAIL_FACTURE, string>(e => e.DESIGNATION_FAMILLE, Operator.EQUAL, searchClassTherap);
                }
                this.AddGroupBy("PAYE, DESIGN_DCI, CODE_DCI, DESIGNATION_FAMILLE, CODE_PRODUIT");
                this.AddOrderBy<View_CFA_MOBILE_DETAIL_FACTURE, string>(e => e.DESIGNATION_FAMILLE);
            }

            return qb.QueryInfos;
        }
        protected override QueryInfos GetSelectParams()
        {
            base.GetSelectParams();
            string typeDisplay = GetCurrentDisplayType();

            this.AddSelect("Sum(QUANTITE) QUANTITE");
            this.AddSelect("SUM(MONT_FACTURE) MONT_FACTURE");
            this.AddSelect<View_CFA_MOBILE_DETAIL_FACTURE, string>(e => e.PAYE);
            this.AddSelect<View_CFA_MOBILE_DETAIL_FACTURE, string>(e => e.DESIGN_DCI);
            this.AddSelect<View_CFA_MOBILE_DETAIL_FACTURE, string>(e => e.CODE_DCI);
            this.AddSelect<View_CFA_MOBILE_DETAIL_FACTURE, string>(e => e.CODE_PRODUIT);
            if (typeDisplay == ClassificationDisplayType.LABORATOIRES.ToString())
            {
                this.AddSelect<View_CFA_MOBILE_DETAIL_FACTURE, string>(e => e.DESIGNATION_LABO);
            }
            else
            {
                this.AddSelect<View_CFA_MOBILE_DETAIL_FACTURE, string>(e => e.DESIGNATION_FAMILLE);
            }

            return qb.QueryInfos;
        }
    }
}
