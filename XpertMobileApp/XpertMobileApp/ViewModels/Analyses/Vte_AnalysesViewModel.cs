using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Collections.Specialized;
using System.Diagnostics;
using System.Text;
using System.Threading.Tasks;
using Xamarin.Forms;
using XpertMobileApp.DAL;
using XpertMobileApp.Services;

namespace XpertMobileApp.ViewModels.Analyses
{
    public class Vte_AnalysesViewModel : BaseViewModel
    {
        public ObservableCollection<View_VTE_Vente_Td> Items { get; set; }
        public Command LoadItemsCommand { get; set; }

        public Vte_AnalysesViewModel()
        {
            Title = AppResources.pn_Analyses;

            Items = new ObservableCollection<View_VTE_Vente_Td>();
            LoadItemsCommand = new Command(async () => await ExecuteLoadItemsCommand());
        }

        async Task ExecuteLoadItemsCommand()
        {
            if (IsBusy)
                return;

            IsBusy = true;

            try
            {
                Items.Clear();
                DateTime endDate  = DateTime.Now;
                DateTime startDate = DateTime.Now.AddDays(-300);

                var items = await WebServiceClient.GetMargeParVendeur(startDate, endDate);
                foreach (var item in items)
                {
                    Items.Add(item);
                }

                MessagingCenter.Send(this, "StatsDataLoaded", Items);
            }
            catch (Exception ex)
            {
                Debug.WriteLine(ex);
            }
            finally
            {
                IsBusy = false;
            }
        }
    }

}
