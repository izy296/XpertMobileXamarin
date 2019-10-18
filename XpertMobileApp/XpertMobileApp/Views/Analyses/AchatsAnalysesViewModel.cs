using Acr.UserDialogs;
using SampleBrowser.SfChart;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Collections.Specialized;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xamarin.Forms;
using Xpert.Common.WSClient.Helpers;
using XpertMobileApp.DAL;
using XpertMobileApp.Helpers;
using XpertMobileApp.Services;

namespace XpertMobileApp.ViewModels.Analyses
{

    public class AchatsAnalysesViewModel : BaseViewModel
    {
        public ObservableCollection<ChartDataModel> Entries1 { get; set; }
        public ObservableCollection<ChartDataModel> Entries2 { get; set; }

        public StatsPeriode StartPeriodType = StatsPeriode.None;

        public ObservableCollection<STAT_ACHAT_AGRO> Items { get; set; }
        public Command LoadItemsCommand { get; set; }

        public AchatsAnalysesViewModel()
        {
            Title = AppResources.pn_Analyses;

            Entries1 = new ObservableCollection<ChartDataModel>();

            Entries2 = new ObservableCollection<ChartDataModel>();

            Items = new ObservableCollection<STAT_ACHAT_AGRO>();
            LoadItemsCommand = new Command(async () => await ExecuteLoadItemsCommand());
        }

        async Task ExecuteLoadItemsCommand()
        {
            if (IsBusy)
                return;

            IsBusy = true;
            UserDialogs.Instance.ShowLoading(AppResources.txt_Waiting);
            try
            {
                DateTime endDate = DateTime.Now;
                DateTime startDate = DateTime.Now;

                startDate = GetTheStartDate(startDate);

                Items.Clear();
                var items = await WebServiceClient.GetAchat(startDate, endDate);
                List<STAT_ACHAT_AGRO> result = items
                .GroupBy(l => l.CODE_PRODUIT)
                .Select(cl => new STAT_ACHAT_AGRO
                {
                    CODE_PRODUIT = cl.First().CODE_PRODUIT,
                    Montant = cl.Sum(c => c.Montant),
                    Qte_Net = cl.Sum(c => c.Qte_Net),
                }).ToList();
                foreach (var item in result)
                {
                    if(item.Montant != 0)
                    { 
                        Items.Add(item);
                    }
                }

                MessagingCenter.Send(this, MCDico.STATS_DATA_LOADED, Items);
            }
            catch (Exception ex)
            {
                await UserDialogs.Instance.AlertAsync(WSApi2.GetExceptionMessage(ex), AppResources.alrt_msg_Alert,
                    AppResources.alrt_msg_Ok);
            }
            finally
            {
                IsBusy = false;
                UserDialogs.Instance.HideLoading();
            }
        }

        private DateTime GetTheStartDate(DateTime startDate)
        {
            if (StartPeriodType == StatsPeriode.Day)
            {
                startDate = DateTime.Now;
            }
            else if (StartPeriodType == StatsPeriode.Week)
            {
                startDate = DateTime.Now.StartOfWeek(DayOfWeek.Sunday);
            }
            else if (StartPeriodType == StatsPeriode.Month)
            {
                startDate = new DateTime(startDate.Year, startDate.Month, 1);
            }
            else
            {
                startDate = new DateTime(startDate.Year, 1, 1);
            }

            return startDate;
        }
    }
}
