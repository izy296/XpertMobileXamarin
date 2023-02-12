using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;
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
            var item = ((sender as Button).BindingContext as View_CFA_BORDEREAUX_CHIFA);
            viewModel.Item = item;
            await viewModel.RefreshData();
        }

        private async void CentrePicker_PropertyChanged(object sender, EventArgs e)
        {
            if (viewModel != null && viewModel.Item.NUM_BORDEREAU != null)
                await viewModel.ExecutePullToRefresh();
        }
    }
}