using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Timers;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;
using XpertMobileApp.Base;
using XpertMobileApp.Views._05_Officine.Chifa.Beneficiares;

namespace XpertMobileApp.Views
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class BeneficiaresPage : XBasePage
    {
        public BeneficiaresViewModel viewModel;
        public BeneficiaresPage()
        {
            InitializeComponent();
            BindingContext = viewModel = new BeneficiaresViewModel();
            viewModel.Title = AppResources.pn_Beneficiaire;
        }

        protected override void OnAppearing()
        {
            base.OnAppearing();
            viewModel.ExecuteLoadItemsCommand();
        }

        private void ExecutePullToRefresh(object sender, EventArgs e)
        {
            viewModel.ExecuteLoadItemsCommand();
        }

        private void OnItemSelected(object sender, SelectionChangedEventArgs e)
        {

        }


        public override void SearchCommand()
        {
            base.SearchCommand();
            Device.BeginInvokeOnMainThread(async () =>
            {
                await viewModel.ExecuteSearch(SearchBarText);
            });
        }


        private void DatePicker_PropertyChanged(object sender, DateChangedEventArgs e)
        {
            if (viewModel != null)
                viewModel.ExecuteLoadItemsCommand();
        }

        private void Order_By_Clicked(object sender, EventArgs e)
        {
            Device.BeginInvokeOnMainThread(async () =>
            {
                viewModel.OrderBy++;
                if (viewModel.OrderBy == 4)
                    viewModel.OrderBy = -1;
                switch (viewModel.OrderBy)
                {
                    case -1:
                        btn_Order.IconImageSource = "SortBy.png";
                        break;
                    case 0:
                        btn_Order.IconImageSource = "SortByAlphabetAsc.png";
                        break;
                    case 1:
                        btn_Order.IconImageSource = "SortByAlphabetDesc.png";
                        break;
                    case 2:
                        btn_Order.IconImageSource = "SortByNumberAsc.png";
                        break;
                    case 3:
                        btn_Order.IconImageSource = "SortByNumberDesc.png";
                        break;
                }

                if (viewModel.timer != null)
                {
                    viewModel.timer.Stop();
                    viewModel.timer.Dispose();
                }
                viewModel.timer = new Timer();
                viewModel.timer.Interval = 1000;
                viewModel.timer.Elapsed += viewModel.t_Tick;
                viewModel.timer.Start();
                TotalSeconds = new TimeSpan(0, 0, 0, 2);

            });


        }
    }
}