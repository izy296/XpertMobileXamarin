using Microcharts;
using SkiaSharp;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;
using XpertMobileApp.DAL;
using XpertMobileApp.Helpers;
using XpertMobileApp.ViewModels.Analyses;
using Entry = Microcharts.Entry;

namespace XpertMobileApp.Views
{
	[XamlCompilation(XamlCompilationOptions.Compile)]
	public partial class EncAnalysesPage : ContentPage
	{

        Vte_AnalysesViewModel viewModel;

        List<Entry> entries1 = new List<Entry>();
        List<Entry> entries2 = new List<Entry>();

        public EncAnalysesPage ()
		{
			InitializeComponent ();

            BindingContext = viewModel = new Vte_AnalysesViewModel();

            MessagingCenter.Subscribe<Vte_AnalysesViewModel, ObservableCollection<View_VTE_Vente_Td>>(this, MCDico.STATS_DATA_LOADED, (obj, items) =>
            {
                DisplayStats(items);
            });
        }

        protected override void OnAppearing()
        {
            base.OnAppearing();

            LoadStats(btn_Year);
        }

        private void StatFilter_Clicked(object sender, EventArgs e)
        {
            LoadStats((Button)sender);
        }

        private void DisplayStats(ObservableCollection<View_VTE_Vente_Td>  items)
        {
            entries1.Clear();
            foreach (var item in viewModel.Items)
            {
                entries1.Add(
                    new Entry((float)item.Sum_TOTAL_VENTE)
                    {
                        Label = item.CREATED_BY,
                        ValueLabel = item.Sum_TOTAL_VENTE.ToString("N2"),
                        Color = SKColor.Parse("#68B9C0")
                    });
            }
            Chart1.Chart = new BarChart() { Entries = entries1, LabelTextSize = 22F };

            entries2.Clear();
            foreach (var item in viewModel.Items)
            {
                entries2.Add(
                    new Entry((float)item.Sum_MARGE)
                    {
                        Label = item.CREATED_BY,
                        ValueLabel = item.Sum_MARGE.ToString("N2"),
                        Color = SKColor.Parse("#68B9C0")
                    });
            }
            Chart2.Chart = new BarChart() { Entries = entries2, LabelTextSize = 22F };

            //Chart1.Chart = new DonutChart() { Entries = entries };                
            // or: var chart = new PointChart() { Entries = entries };
            // or: var chart = new LineChart() { Entries = entries };
            // or: var chart = new DonutChart() { Entries = entries };
            // or: var chart = new RadialGaugeChart() { Entries = entries };
            // or: var chart = new RadarChart() { Entries = entries };
        }

        private void LoadStats(Button btn)
        {
            StatsPeriode selectedType = viewModel.StartPeriodType;

            btn_Day.BackgroundColor   = Color.FromHex("#2196F3");
            btn_Week.BackgroundColor  = Color.FromHex("#2196F3");
            btn_Month.BackgroundColor = Color.FromHex("#2196F3");
            btn_Year.BackgroundColor  = Color.FromHex("#2196F3");

            btn.BackgroundColor = Color.FromHex("#51adf6");
            switch (btn.ClassId)
            {
                case "btn_Day":
                    selectedType = StatsPeriode.Day;

                    break;
                case "btn_Week":
                    selectedType = StatsPeriode.Week;
                    break;
                case "btn_Month":
                    selectedType = StatsPeriode.Month;
                    break;
                case "btn_Year":
                    selectedType = StatsPeriode.Year;
                    break;
            }

            if (viewModel.StartPeriodType != selectedType)
            {
                viewModel.StartPeriodType = selectedType;
                viewModel.LoadItemsCommand.Execute(null);
            }
        }
    }
}