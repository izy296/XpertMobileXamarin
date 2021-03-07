using SampleBrowser.SfChart;
using Syncfusion.SfChart.XForms;
using System;
using System.Collections.ObjectModel;
using System.Linq;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;
using XpertMobileApp.DAL;
using XpertMobileApp.Helpers;
using XpertMobileApp.ViewModels.Analyses;

namespace XpertMobileApp.Views
{
	[XamlCompilation(XamlCompilationOptions.Compile)]
	public partial class AchatsAnalysesPage : ContentPage
	{

        AchatsAnalysesViewModel viewModel;

        public AchatsAnalysesPage()
		{
			InitializeComponent ();

            BindingContext = viewModel = new AchatsAnalysesViewModel();

            MessagingCenter.Subscribe<AchatsAnalysesViewModel, ObservableCollection<STAT_ACHAT_AGRO>>(this, MCDico.STATS_DATA_LOADED, (obj, items) =>
            {
                DisplayStats(items);
            });

            if (Device.RuntimePlatform == Device.macOS || Device.RuntimePlatform == Device.UWP)
            {
                Chart_Marges.Legend.OverflowMode = ChartLegendOverflowMode.Scroll;
                Chart_Ventes.Legend.OverflowMode = ChartLegendOverflowMode.Scroll;
            }
        }

        protected override void OnSizeAllocated(double width, double height)
        {
            base.OnSizeAllocated(width, height);
            if ((Device.RuntimePlatform == Device.Android || Device.RuntimePlatform == Device.iOS))
            {
                if (height > 0 && width > 0)
                {
                    if (height > width)
                    {
                        Chart_Marges.Legend.DockPosition = LegendPlacement.Bottom;
                        Chart_Ventes.Legend.DockPosition = LegendPlacement.Bottom;
                    }
                    else
                    {
                        Chart_Marges.Legend.DockPosition = LegendPlacement.Right;
                        Chart_Ventes.Legend.DockPosition = LegendPlacement.Right;
                    }
                }
            }
        }

        protected override void OnAppearing()
        {
            base.OnAppearing();

            LoadStats(btn_Week);
        }

        private void StatFilter_Clicked(object sender, EventArgs e)
        {
            LoadStats((Button)sender);
        }

        private void DisplayStats(ObservableCollection<STAT_ACHAT_AGRO>  items)
        {
            /*
Chart_AchatDetails.Height = chartBounds + (chartArea * parseInt(id));
chart.redraw();

Chart_AchatDetails.Series.Add()

Entries3.Clear();
string[] prods = items.Select(x => x.CODE_PRODUIT).Distinct().ToArray();
foreach (var prod in prods)
{
    StackingBarSeries serie = new StackingBarSeries();
    foreach (var item in items)
    {
        if(item.CODE_PRODUIT == prod) 
        { 
            double val = (double)item.Qte_Net;
            DateTime dte = item.DATE_DOC ?? DateTime.Now;
            Entries3.Add(new ChartDataModel(dte.ToString("ddd, d MMM "), val));
        }
    }
    serie.ItemsSource =
}
*/
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
                viewModel.StartDate = viewModel.GetTheStartDate();
                viewModel.LoadItemsCommand.Execute(null);
            }
        }

        private void btn_CancelFilter_Clicked(object sender, EventArgs e)
        {

        }

        private void btn_ApplyFilter_Clicked(object sender, EventArgs e)
        {
            viewModel.LoadItemsCommand.Execute(null);
        }

        private void Filter_Clicked(object sender, EventArgs e)
        {
            FilterPanel.IsVisible = !FilterPanel.IsVisible;
        }
    }
}