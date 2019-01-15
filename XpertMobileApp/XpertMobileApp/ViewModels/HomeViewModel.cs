using Acr.UserDialogs;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Diagnostics;
using System.Text;
using System.Threading.Tasks;
using Xamarin.Forms;
using Xpert.Pharm.DAL;
using XpertMobileApp.Services;

namespace XpertMobileApp.ViewModels
{

    public class HomeViewModel : BaseViewModel
    {
        public ObservableCollection<View_BSE_COMPTE> Items { get; set; }
        public Command LoadItemsCommand { get; set; }

        public ObservableCollection<TRS_JOURNEES> Sessions { get; set; }
        public Command LoadSessionsCommand { get; set; }

        public ObservableCollection<View_TRS_ENCAISS> Encaissements { get; set; }
        public Command LoadEncaissStatCommand { get; set; }
        

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

            Items = new ObservableCollection<View_BSE_COMPTE>();
            Sessions = new ObservableCollection<TRS_JOURNEES>();
            Encaissements = new ObservableCollection<View_TRS_ENCAISS>();

            LoadItemsCommand = new Command(async () => await ExecuteLoadItemsCommand());

            LoadSessionsCommand = new Command(async () => await ExecuteLoadSessionsCommand());

            LoadEncaissStatCommand = new Command(async () => await ExecuteLoadEncaissStatCommand());
        }

        async Task ExecuteLoadEncaissStatCommand()
        {
            try
            {
                Encaissements.Clear();
                var items = await WebServiceClient.GetStatisticEncaiss(DateTime.Now.AddYears(-1), DateTime.Now);
                if (items.Count > 0)
                { 
                    foreach (var item in items)
                    {
                        Encaissements.Add(item);
                    }
                    this.TotalEncaiss = items[0].TOTAL_ENCAISS;
                    this.TotalDecaiss = items[1].TOTAL_ENCAISS;
                }
            }
            catch (Exception ex)
            {
                await UserDialogs.Instance.AlertAsync(AppResources.err_msg_loadingDataError, AppResources.alrt_msg_Alert,
                    AppResources.alrt_msg_Ok);
            }
        }

        async Task ExecuteLoadSessionsCommand()
        {
            try
            {
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
                Items.Clear();
                var items = await WebServiceClient.getComptes();
                foreach (var item in items)
                {
                    Items.Add(item);
                }
            }
            catch (Exception ex)
            {
                await UserDialogs.Instance.AlertAsync(AppResources.err_msg_loadingDataError, AppResources.alrt_msg_Alert,
                    AppResources.alrt_msg_Ok);
            }
        }
    }
}
