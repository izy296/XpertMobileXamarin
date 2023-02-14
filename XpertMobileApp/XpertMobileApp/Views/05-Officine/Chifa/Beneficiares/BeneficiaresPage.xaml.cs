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
using XpertMobileApp.DAL;

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

        protected async override void OnAppearing()
        {
            base.OnAppearing();
            if (viewModel.Items.Count==0)
               await viewModel.ExecuteLoadItemsCommand();
        }

        private async void ExecutePullToRefresh(object sender, EventArgs e)
        {
            viewModel.Items.Clear();
            await viewModel.ExecuteLoadItemsCommand();
            viewModel.IsRefreshing = false;
        }

        private async void OnItemSelected(object sender, SelectionChangedEventArgs e)
        {
            View_CFA_MOBILE_DETAIL_FACTURE item = null;
            if (e.CurrentSelection.Count > 0)
                item = e.CurrentSelection[0] as View_CFA_MOBILE_DETAIL_FACTURE;
            if (item == null)
                return;

            await Navigation.PushAsync(new BeneficiaresDetailPage(item));

            // Manually deselect item.
            ItemsListView.SelectedItem = null;
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
            {
                viewModel.Items.Clear();
                viewModel.ExecuteLoadItemsCommand(); }
        }

        private void Order_By_Clicked(object sender, EventArgs e)
        {
            var text = (sender as ToolbarItem).Text;
            if (text == AppResources.pn_Beneficiaire_OrederBy_Num_ASC)
            {
                viewModel.OrderBy = 0;
            }
            else if (text == AppResources.pn_Beneficiaire_OrederBy_Num_DESC)
            {
                viewModel.OrderBy = 1;
            }            
            else if (text == AppResources.pn_Beneficiaire_OrederBy_Montant_ASC)
            {
                viewModel.OrderBy = 2;
            }   
            else if (text == AppResources.pn_Beneficiaire_OrederBy_Montant_DESC)
            {
                viewModel.OrderBy = 3;
            }
            else if (text == AppResources.pn_Beneficiaire_OrederBy_nbrFacts_ASC)
            {
                viewModel.OrderBy = 4;
            }
            else if (text == AppResources.pn_Beneficiaire_OrederBy_nbrFacts_DESC)
            {
                viewModel.OrderBy = 5;
            }

            viewModel.Items.Clear();
            viewModel.ExecuteLoadItemsCommand();

        }
    }
}