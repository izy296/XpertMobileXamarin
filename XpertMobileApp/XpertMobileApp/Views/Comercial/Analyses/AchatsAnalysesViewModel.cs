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
        public DateTime StartDate { get; set; } = DateTime.Now.StartOfWeek(DayOfWeek.Sunday);
        public DateTime EndDate { get; set; } = DateTime.Now;

        public ObservableCollection<ChartDataModel> Entries1 { get; set; }
        public ObservableCollection<ChartDataModel> Entries2 { get; set; }
        public ObservableCollection<ChartDataModel> Entries3 { get; set; }
        
        public StatsPeriode StartPeriodType = StatsPeriode.None;

        public ObservableCollection<STAT_ACHAT_AGRO> Items { get; set; }
        public Command LoadItemsCommand { get; set; }

        public AchatsAnalysesViewModel()
        {
            Title = AppResources.pn_Analyses;

            Entries1 = new ObservableCollection<ChartDataModel>();

            Entries2 = new ObservableCollection<ChartDataModel>();

            Entries3 = new ObservableCollection<ChartDataModel>();

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
                DateTime endDate = this.EndDate ;
                DateTime startDate = this.StartDate;

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
                    if (item.Montant != 0)
                    {
                        Items.Add(item);
                    }
                }

                Entries1.Clear();
                foreach (var item in Items)
                {
                    double val = (double)item.Montant;
                    Entries1.Add(new ChartDataModel(item.CODE_PRODUIT, val));
                }

                Entries2.Clear();
                foreach (var item in Items)
                {
                    double val = (double)item.Qte_Net;
                    Entries2.Add(new ChartDataModel(item.CODE_PRODUIT, val));
                }

                Entries3.Clear();
                foreach (var item in items)
                {                    
                    double val = (double)item.Qte_Net;
                    DateTime dte = item.DATE_DOC ?? DateTime.Now;
                    int prod = Convert.ToInt32(item.CODE_PRODUIT);
                    Entries3.Add(new ChartDataModel("P" + prod + " - "+ dte.ToString("ddd, d MMM "), val));
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

        internal DateTime GetTheStartDate()
        {
            if (StartPeriodType == StatsPeriode.Day)
            {
                return DateTime.Now;
            }
            else if (StartPeriodType == StatsPeriode.Week)
            {
                return DateTime.Now.StartOfWeek(DayOfWeek.Sunday);
            }
            else if (StartPeriodType == StatsPeriode.Month)
            {
                return new DateTime(DateTime.Now.Year, DateTime.Now.Month, 1);
            }
            else
            {
                return new DateTime(DateTime.Now.Year, 1, 1);
            }
        }
    }
}
