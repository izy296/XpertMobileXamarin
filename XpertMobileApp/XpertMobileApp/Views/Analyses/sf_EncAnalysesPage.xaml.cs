using SampleBrowser.SfChart;
using Syncfusion.SfChart.XForms;
using System;
using System.Collections.ObjectModel;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;
using XpertMobileApp.DAL;
using XpertMobileApp.Helpers;
using XpertMobileApp.ViewModels.Analyses;

namespace XpertMobileApp.Views
{
	[XamlCompilation(XamlCompilationOptions.Compile)]
	public partial class sf_EncAnalysesPage : ContentPage
	{

        SF_Vte_AnalysesViewModel viewModel;

        public sf_EncAnalysesPage()
		{
			InitializeComponent ();

            BindingContext = viewModel = new SF_Vte_AnalysesViewModel();

            MessagingCenter.Subscribe<SF_Vte_AnalysesViewModel, ObservableCollection<View_VTE_Vente_Td>>(this, MCDico.STATS_DATA_LOADED, (obj, items) =>
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

            LoadStats(btn_Year);
        }

        private void StatFilter_Clicked(object sender, EventArgs e)
        {
            LoadStats((Button)sender);
        }

        private void DisplayStats(ObservableCollection<View_VTE_Vente_Td>  items)
        {
            viewModel.Entries1.Clear();
            foreach (var item in viewModel.Items)
            {
                viewModel.Entries1.Add(new ChartDataModel(item.CREATED_BY, (double)item.Sum_TOTAL_VENTE));
            }

            viewModel.Entries2.Clear();
            foreach (var item in viewModel.Items)
            {
                viewModel.Entries2.Add(new ChartDataModel(item.CREATED_BY, (double)item.Sum_MARGE));
            }
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