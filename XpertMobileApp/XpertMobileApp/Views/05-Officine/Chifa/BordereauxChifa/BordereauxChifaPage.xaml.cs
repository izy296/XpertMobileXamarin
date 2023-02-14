using Flex.Controls;
using Syncfusion.ListView.XForms;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.PancakeView;
using Xamarin.Forms.Xaml;
using XpertMobileApp.Api.ViewModels;
using XpertMobileApp.Base;
using XpertMobileApp.DAL;

namespace XpertMobileApp.Views
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class BordereauxChifaPage : XBasePage
    {
        public BordereauxChifaViewModel viewModel;

        private bool isScrolling { get; set; } = false;
        public bool IsScrolling { get; set; }
        private bool clicked { get; set; } = false;
        public bool Clicked
        {
            get
            {
                return clicked;
            }
            set
            {
                clicked = value;
                OnPropertyChanged("Clicked");
            }
        }
        public BordereauxChifaPage()
        {
            InitializeComponent();
            BindingContext = viewModel = new BordereauxChifaViewModel();
        }

        protected override async void OnAppearing()
        {
            base.OnAppearing();

            //await viewModel.ExecuteLoadItemsCommand();
            //await viewModel.ExecuteLoadLastBordereaux();
            //await viewModel.ExecuteLoadBordereauxInfo();
            //await viewModel.ExecuteLoadFacturesCommand();
            await viewModel.ExecuteLoadLastsBordereaux();
            expander.IsExpanded = true;
        }


        public override void SearchCommand()
        {
            base.SearchCommand();
            Device.BeginInvokeOnMainThread(async () =>
            {
                await viewModel.ExecuteSearch(SearchBarText);
            });
        }

        private void OnItemSelected(object sender, SelectionChangedEventArgs e)
        {

        }

        private async void ExecutePullToRefresh(object sender, EventArgs e)
        {
            await viewModel.ExecutePullToRefresh();
        }

        private async void ButtonClicked(object sender, EventArgs e)
        {
            if (Clicked)
                return;
            Clicked = true;
            var item = ((sender as PancakeView).BindingContext as View_CFA_BORDEREAUX_CHIFA);
            var elements = ((sender as PancakeView).Parent.Parent.Parent as VisualContainer).Children;
            foreach (var element in elements)
            {
                var x = (((Grid)((ContentView)element).Content).Children[0]);
                (x as PancakeView).BackgroundColor = Color.FromHex("#057665");
            }
                (sender as PancakeView).BackgroundColor = Color.FromHex("#068975");

            viewModel.Item = item;

            await viewModel.RefreshData();
            Clicked = false;
        }

        private async void CentrePicker_PropertyChanged(object sender, EventArgs e)
        {
            if (viewModel != null && viewModel.Item.NUM_BORDEREAU != null)
                await viewModel.ExecutePullToRefresh();
        }

        private void BordereauxListView_ScrollStateChanged(object sender, ScrollStateChangedEventArgs e)
        {
            //IsScrolling = e.ScrollState == ScrollState.Dragging ? true : false;
        }

    }
}