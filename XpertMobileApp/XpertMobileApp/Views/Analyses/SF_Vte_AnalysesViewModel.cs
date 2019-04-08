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
using XpertMobileApp.DAL;
using XpertMobileApp.Helpers;
using XpertMobileApp.Services;

namespace XpertMobileApp.ViewModels.Analyses
{

    public class SF_Vte_AnalysesViewModel : BaseViewModel
    {
        public ObservableCollection<ChartDataModel> Entries1 { get; set; }
        public ObservableCollection<ChartDataModel> Entries2 { get; set; }

        public StatsPeriode StartPeriodType = StatsPeriode.None;

        public ObservableCollection<STAT_VTE_BY_USER> Items { get; set; }
        public Command LoadItemsCommand { get; set; }

        public SF_Vte_AnalysesViewModel()
        {
            Title = AppResources.pn_Analyses;

            Entries1 = new ObservableCollection<ChartDataModel>();

            Entries2 = new ObservableCollection<ChartDataModel>();

            Items = new ObservableCollection<STAT_VTE_BY_USER>();
            LoadItemsCommand = new Command(async () => await ExecuteLoadItemsCommand());
        }

        async Task ExecuteLoadItemsCommand()
        {
            if (IsBusy)
                return;

            IsBusy = true;

            try
            {
                DateTime endDate = DateTime.Now;
                DateTime startDate = DateTime.Now;

                startDate = GetTheStartDate(startDate);

                Items.Clear();
                var items = await WebServiceClient.GetMargeParVendeur(startDate, endDate);
                List<STAT_VTE_BY_USER> result = items
                .GroupBy(l => l.UTILISATEUR)
                .Select(cl => new STAT_VTE_BY_USER
                {
                    UTILISATEUR = cl.First().UTILISATEUR,
                    MONTANT_VENTE = cl.Sum(c => c.MONTANT_VENTE),
                    MONTANT_MARGE = cl.Sum(c => c.MONTANT_MARGE),
                }).ToList();
                foreach (var item in result)
                {
                    if(item.MONTANT_VENTE != 0)
                    { 
                        Items.Add(item);
                    }
                }

                MessagingCenter.Send(this, MCDico.STATS_DATA_LOADED, Items);
            }
            catch (Exception ex)
            {
                await UserDialogs.Instance.AlertAsync(ex.Message, AppResources.alrt_msg_Alert,
                    AppResources.alrt_msg_Ok);
            }
            finally
            {
                IsBusy = false;
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
