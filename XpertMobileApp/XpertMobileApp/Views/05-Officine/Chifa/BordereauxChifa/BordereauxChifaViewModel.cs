using Acr.UserDialogs;
using Rg.Plugins.Popup.Services;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xamarin.Forms;
using Xpert.Common.DAO;
using Xpert.Common.WSClient.Helpers;
using XpertMobileApp.Api.ViewModels;
using XpertMobileApp.DAL;
using XpertMobileApp.Services;
using XpertMobileApp.ViewModels;
using XpertMobileApp.Views;

namespace XpertMobileApp.Api.ViewModels
{
    public class BordereauxChifaViewModel : CrudBaseViewModel2<CFA_BORDEREAU, View_CFA_BORDEREAUX_CHIFA>
    {
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

        private View_CFA_BORDEREAUX_CHIFA item { get; set; }
        public View_CFA_BORDEREAUX_CHIFA Item
        {
            get
            {
                return item;
            }
            set {
                item = value;
                OnPropertyChanged("Item");
            }
        }

        private ObservableCollection<CFA_CENTRES> centres { get; set; }
        public ObservableCollection<CFA_CENTRES> Centres
        {
            get { return centres; }
            set
            {
                centres = value;
                OnPropertyChanged("Centres");
            }
        }

        private CFA_CENTRES selectedCentre { get; set; } = new CFA_CENTRES() { CODE="0",DESIGNATION=""};
        public CFA_CENTRES SelectedCentre { get { return selectedCentre; } set {
                selectedCentre = value;
                OnPropertyChanged("SelectedCentre");
            } 
        }

        private ObservableCollection<View_CONVENTION_FACTURE> chifaFacturesList { get; set; }
        public ObservableCollection<View_CONVENTION_FACTURE> ChifaFacturesList
        {
            get { return chifaFacturesList; }
            set
            {
                chifaFacturesList = value;
                OnPropertyChanged("ChifaFacturesList");
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

        private ObservableCollection<View_CFA_BORDEREAUX_CHIFA> bordereauxList { get; set; }
        public ObservableCollection<View_CFA_BORDEREAUX_CHIFA> BordereauxList { get { return bordereauxList; } set { bordereauxList = value; OnPropertyChanged("BordereauxList"); } }

        public bool FactureLoadMore { get; set; } = true;

        public Command LoadBillsCommand { get; set; }


        public BordereauxChifaViewModel()
        {
            Title = AppResources.pn_BordereauxChifa;

            Centres = new ObservableCollection<CFA_CENTRES>();

            Item = new View_CFA_BORDEREAUX_CHIFA();

            new Command(async () => {
                await ExecuteLoadLastBordereaux();
                await ExecuteLoadFacturesCommand();
            }).Execute(null);

            ChifaFacturesList = new ObservableCollection<View_CONVENTION_FACTURE>();

            LoadBillsCommand = new Command(async () => await ExecuteLoadFacturesCommand());
            BordereauxList = new ObservableCollection<View_CFA_BORDEREAUX_CHIFA>();
            Centres.Add(new CFA_CENTRES
            {
                CODE = "0",
                DESIGNATION = "",
            });
            Centres.Add(new CFA_CENTRES
            {
                CODE = "1",
                DESIGNATION = "CNAS",
            });
            Centres.Add(new CFA_CENTRES
            {
                CODE = "2",
                DESIGNATION = "CASNOS",
            });
            Centres.Add(new CFA_CENTRES
            {
                CODE = "9",
                DESIGNATION = "Hors CHIFA",
            });
        }

        public async Task RefreshData()
        {
                await ExecuteLoadBordereauxInfo();
                await ExecutePullToRefresh();
        }

        protected override QueryInfos GetSelectParams()
        {
            return base.GetSelectParams();
        }

        protected override QueryInfos GetFilterParams()
        {
            return base.GetFilterParams();
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

        public async Task ExecuteLoadBordereauxInfo()
        {
            try
            {
                if (IsBusy)
                    return;
                IsBusy = true;
                UserDialogs.Instance.ShowLoading(AppResources.txt_msg_BorderauxInfo);
                var res = await WebServiceClient.GetCFAByNumBordereaux(Item.NUM_BORDEREAU);
                if (res != null && res.Count > 0)
                    Item = res[0];
                UserDialogs.Instance.HideLoading();
            }
            catch (Exception ex)
            {
                UserDialogs.Instance.HideLoading();
                CustomPopup AlertPopup = new CustomPopup(WSApi2.GetExceptionMessage(ex), trueMessage: AppResources.alrt_msg_Ok);
                await PopupNavigation.Instance.PushAsync(AlertPopup);
            }
            finally
            {
                IsBusy = false;
            }
        }

        internal override async Task ExecuteLoadItemsCommand()
        {
            try
            {
                if (IsBusy)
                    return;
                IsBusy = true;
                await Items.LoadMoreAsync();
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

        public async Task ExecutePullToRefresh()
        {
            try
            {
                if (ChifaFacturesList.Count != 0)
                {
                    ChifaFacturesList.Clear();
                }
                FactureLoadMore = true;
                UserDialogs.Instance.ShowLoading(AppResources.txt_msg_RecuperationFactures);
                ChifaFacturesList = new ObservableCollection<View_CONVENTION_FACTURE>(await WebServiceClient.GetCFAFactsByNumBordereaux(Item.NUM_BORDEREAU, center: SelectedCentre.CODE));
                UserDialogs.Instance.HideLoading();
                IsRefreshing = false;
            }
            catch (Exception ex)
            {
                UserDialogs.Instance.HideLoading();
                CustomPopup AlertPopup = new CustomPopup(WSApi2.GetExceptionMessage(ex), trueMessage: AppResources.alrt_msg_Ok);
                await PopupNavigation.Instance.PushAsync(AlertPopup);
            }
            finally
            {
                IsRefreshing = false;
            }
        }
        public async Task ExecuteLoadLastBordereaux()
        {
            try
            {
                UserDialogs.Instance.ShowLoading(AppResources.txt_msg_BorderauxInfo);
                //queryBuilder.AddCondition<View_CFA_BORDEREAUX_CHIFA,DateTime?>(e=>e.CREATED_ON, Operator.BETWEEN_DATE,DateTime.Now);
                qb.AddCondition<View_CFA_BORDEREAUX_CHIFA, string>(e => e.NUM_BORDEREAU, Operator.NOT_EQUAL, "VIDE");
                qb.AddOrderBy<View_CFA_BORDEREAUX_CHIFA, string>(e => e.NUM_BORDEREAU, Sort.DESC);
                qb.AddPaging(1, 1);
                var result = await WebServiceClient.GetCFABordereaux(qb.QueryInfos);
                if (result != null)
                    Item = result[0];
                qb.InitQuery();
                UserDialogs.Instance.HideLoading();
            }
            catch (Exception ex)
            {
                qb.InitQuery();
                UserDialogs.Instance.HideLoading();
                CustomPopup AlertPopup = new CustomPopup(WSApi2.GetExceptionMessage(ex), trueMessage: AppResources.alrt_msg_Ok);
                PopupNavigation.Instance.PushAsync(AlertPopup);
            }
            finally
            {
                IsRefreshing = false;
            }
        }

        public async Task ExecuteLoadLastsBordereaux()
        {
            try
            {
                qb.InitQuery();
                //queryBuilder.AddCondition<View_CFA_BORDEREAUX_CHIFA,DateTime?>(e=>e.CREATED_ON, Operator.GREATER_DATE,DateTime.Now.AddMonths(-1));
                qb.AddCondition<View_CFA_BORDEREAUX_CHIFA, string>(e => e.NUM_BORDEREAU, Operator.NOT_EQUAL, "VIDE");
                qb.AddCondition<View_CFA_BORDEREAUX_CHIFA, decimal>(e => e.TOTAL_NB_CHIFA, Operator.GREATER, 10);
                qb.AddOrderBy<View_CFA_BORDEREAUX_CHIFA,string>(e => e.NUM_BORDEREAU, Sort.DESC);
                
                qb.AddPaging(1, 5);
                BordereauxList = new ObservableCollection<View_CFA_BORDEREAUX_CHIFA>(await WebServiceClient.GetCFABordereaux(qb.QueryInfos));
                qb.InitQuery();
            }
            catch (Exception ex)
            {
                qb.InitQuery();
                UserDialogs.Instance.HideLoading();
                CustomPopup AlertPopup = new CustomPopup(WSApi2.GetExceptionMessage(ex), trueMessage: AppResources.alrt_msg_Ok);
                await PopupNavigation.Instance.PushAsync(AlertPopup);
            }
            finally
            {
                IsRefreshing = false;
            }
        }

        public async Task ExecuteLoadFacturesCommand()
        {
            try
            {
                if (IsBusy)
                    return;
                IsBusy = true;
                if (ChifaFacturesList.Count == 0)
                {
                    UserDialogs.Instance.ShowLoading(AppResources.txt_msg_RecuperationFactures);
                    ChifaFacturesList = new ObservableCollection<View_CONVENTION_FACTURE>(await WebServiceClient.GetCFAFactsByNumBordereaux(Item.NUM_BORDEREAU, center: SelectedCentre.CODE));
                    UserDialogs.Instance.HideLoading();
                }
                else
                {
                    if (FactureLoadMore)
                    {
                        var stop = true;
                        var page = (int)(Math.Round((decimal)(ChifaFacturesList.Count / 10)) + 1);
                        var list = new ObservableCollection<View_CONVENTION_FACTURE>(await WebServiceClient.GetCFAFactsByNumBordereaux(Item.NUM_BORDEREAU, page: page,center:SelectedCentre.CODE));
                        foreach (var item in list)
                        {
                            if (!ChifaFacturesList.Any(e => e.NUM_FACTURE == item.NUM_FACTURE))
                            {
                                ChifaFacturesList.Add(item);
                                stop = false;
                            }
                        }
                        if (stop)
                        {
                            FactureLoadMore = false;
                        }
                    }

                }
            }
            catch (Exception ex)
            {
                UserDialogs.Instance.HideLoading();
                CustomPopup AlertPopup = new CustomPopup(WSApi2.GetExceptionMessage(ex), trueMessage: AppResources.alrt_msg_Ok);
                await PopupNavigation.Instance.PushAsync(AlertPopup);
            }
            finally
            {
                IsBusy = false;
            }
        }
    }
}
