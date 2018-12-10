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
        }

        protected override void OnAppearing()
        {
            base.OnAppearing();

            if (viewModel.Items.Count == 0)
                 viewModel.LoadItemsCommand.Execute(null);

            MessagingCenter.Subscribe<Vte_AnalysesViewModel, ObservableCollection<View_VTE_Vente_Td>>(this, "StatsDataLoaded", async (obj, items) =>
            {
                entries1.Clear();
                foreach (var item in viewModel.Items)
                {
                    entries1.Add(
                        new Entry((float)item.Sum_TOTAL_VENTE)
                        {
                            Label = item.CREATED_BY,
                            ValueLabel = item.Sum_TOTAL_VENTE.ToString("F2"),
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
                            ValueLabel = item.Sum_MARGE.ToString("F2"),
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
            });




        }
    }
}