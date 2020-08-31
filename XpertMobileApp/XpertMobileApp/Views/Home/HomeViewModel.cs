using Acr.UserDialogs;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Diagnostics;
using System.Text;
using System.Threading.Tasks;
using Xamarin.Forms;
using Xpert.Common.DAO;
using XpertMobileApp.Api;
using XpertMobileApp.Api.Managers;
using XpertMobileApp.DAL;
using XpertMobileApp.Models;
using XpertMobileApp.Services;

namespace XpertMobileApp.ViewModels
{

    public class HomeViewModel : BaseViewModel
    {
        public ObservableCollection<TDB_SIMPLE_INDICATORS> Items { get; set; }
        public Command LoadItemsCommand { get; set; }

        public ObservableCollection<TRS_JOURNEES> Sessions { get; set; }
        public Command LoadSessionsCommand { get; set; }


        

        private TRS_JOURNEES currentSession;
        public TRS_JOURNEES CurrentSession {
            get { return currentSession; }
            set { SetProperty(ref currentSession, value); }
        }

        private decimal totalEncaiss;
        public decimal TotalEncaiss {
            get { return totalEncaiss; }
            set { SetProperty(ref totalEncaiss, value); }
        }

        private decimal totalDecaiss;
        public decimal TotalDecaiss {
            get { return totalDecaiss; }
            set { SetProperty(ref totalDecaiss, value); }
        }

        public HomeViewModel()
        {
            Title = AppResources.pn_home;

            Items = new ObservableCollection<TDB_SIMPLE_INDICATORS>();
            Sessions = new ObservableCollection<TRS_JOURNEES>();

            LoadItemsCommand = new Command(async () => await ExecuteLoadItemsCommand());

            LoadSessionsCommand = new Command(async () => await ExecuteLoadSessionsCommand());
        }

        async Task ExecuteLoadSessionsCommand()
        {
            try
            {
                /*
                Sessions.Clear();
                var items = await WebServiceClient.GetSessionInfos();

                foreach (var item in items)
                {
                    Sessions.Add(item);

                    if (App.User.UserName == item.USER_SESSION)
                    {
                        this.CurrentSession = item;
                    }
                    
                }
                */
            }
            catch (Exception ex)
            {
                await UserDialogs.Instance.AlertAsync(AppResources.err_msg_loadingDataError, AppResources.alrt_msg_Alert,
                    AppResources.alrt_msg_Ok);
            }
        }

        async Task ExecuteLoadItemsCommand()
        {
            try
            {
                if (IsBusy)
                    return;

                IsBusy = true;

                UserDialogs.Instance.ShowLoading(AppResources.txt_Loading);
                Items.Clear();
                var items = await CrudManager.SimpleIndicatorsService.SelectByPage(GetFilterParams(), 1,20);
                foreach (var item in items)
                {
                    Items.Add(item);
                }
                IsBusy = false;
            }
            catch (Exception ex)
            {
                IsBusy = false;
                await UserDialogs.Instance.AlertAsync(AppResources.err_msg_loadingDataError, AppResources.alrt_msg_Alert,
                    AppResources.alrt_msg_Ok);
            }
            finally
            {
                UserDialogs.Instance.HideLoading();
            }
        }

        protected QueryInfos GetFilterParams()
        {
            XpertSqlBuilder qb = new XpertSqlBuilder();
            qb.InitQuery();

            qb.AddCondition<TDB_SIMPLE_INDICATORS, string>(e => e.Profils, Operator.LIKE_ANY, App.User.UserGroup);

            qb.AddCondition<TDB_SIMPLE_INDICATORS, string>(e => e.AppNames, Operator.LIKE_ANY, Constants.AppName);

            qb.AddOrderBy<TDB_SIMPLE_INDICATORS, int>(e => e.ORDRE);

            return qb.QueryInfos;
        }
    }
}
