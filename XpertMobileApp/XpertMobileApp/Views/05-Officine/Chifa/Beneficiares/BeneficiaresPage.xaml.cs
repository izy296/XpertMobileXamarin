using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
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
        }


        private void DatePicker_PropertyChanged(object sender, DateChangedEventArgs e)
        {
            if (viewModel != null)
                viewModel.ExecuteLoadItemsCommand();
        }
    }
}