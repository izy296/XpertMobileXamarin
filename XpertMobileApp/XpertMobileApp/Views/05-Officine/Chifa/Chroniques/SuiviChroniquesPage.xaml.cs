using Syncfusion.XForms.Expander;
using Syncfusion.XForms.TabView;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;
using XpertMobileApp.Api.ViewModels;
using XpertMobileApp.DAL;
using XpertMobileApp.Services;
using XpertMobileApp.Views._05_Officine.Chifa.FactureCHIFA;
using SelectionChangedEventArgs = Xamarin.Forms.SelectionChangedEventArgs;

namespace XpertMobileApp.Views
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class SuiviChroniquesPage : ContentPage
    {
        public SuiviChroniquesViewModel viewModel;

        public SuiviChroniquesPage()
        {
            InitializeComponent();
            BindingContext = viewModel = new SuiviChroniquesViewModel();

        }

        protected override async void OnAppearing()
        {
            base.OnAppearing();
            if (viewModel.Items.Count == 0)
            {
                await viewModel.ExecuteLoadItemsCommand();
            }

        }

        private void btn_ExportCSV_Clicked(object sender, EventArgs e)
        {

        }

        private void CentrePicker_PropertyChanged(object sender, EventArgs e)
        {

        }

        private void OnItemSelected(object sender, SelectionChangedEventArgs e)
        {

        }

        private async void expander_Expanding(object sender, Syncfusion.XForms.Expander.ExpandingAndCollapsingEventArgs e)
        {
            var expander = ((SfExpander)sender);
            var parent = ((View)sender).Parent.BindingContext as View_CFA_MOBILE_DETAIL_FACTURE;
            if (parent.Traitments == null)
                if (viewModel.selectedTab == SuiviChroniquesViewModel.tabType.MALADES)
                    parent.Traitments = new ObservableCollection<View_CFA_MOBILE_DETAIL_FACTURE>(await WebServiceClient.SelectTraitment(parent.NUM_FACTURE));
                else
                    parent.Malades = new ObservableCollection<View_CFA_MOBILE_DETAIL_FACTURE>(await WebServiceClient.SelectPatientByMedication(parent.DESIGNATION_PRODUIT));
            //(expander.Content as StackLayout).HeightRequest = ((expander.Content as StackLayout).Children[0] as CollectionView).Height;

            //((expander.Content as StackLayout).Children[0] as CollectionView).HeightRequest = 50;
        }

        private void tabView_SelectionChanged(object sender, Syncfusion.XForms.TabView.SelectionChangedEventArgs e)
        {
            var v = (sender as SfTabView);
            viewModel.Items.Clear();
            if (v.SelectedIndex == 0)
                viewModel.selectedTab = SuiviChroniquesViewModel.tabType.MALADES;
            else viewModel.selectedTab = SuiviChroniquesViewModel.tabType.MEDICAMENTS;
        }

        private void Order_By_Date(object sender, EventArgs e)
        {
            var text = (sender as ToolbarItem).Text;
            if (text == "Trier par Trier Date Facture")
            {
                viewModel.SelectedDateFilter = "DF";
            }
            else if (text == "")
            {
                viewModel.SelectedDateFilter = "DS";
            }

            viewModel.ExecuteLoadItemsCommand();
        }

        private async void FactureTapped(object sender, EventArgs e)
        {
            var obj = (sender as Label).Text.Split(' ');
            RapportJournalierFactureCHIFAViewModel viewModelFacture = new RapportJournalierFactureCHIFAViewModel();
            viewModelFacture.Items.Add(new View_CFA_MOBILE_FACTURE()
            {
                NUM_FACTURE = obj[0]
            });
            await Navigation.PushAsync(new CHIFA_FactureCarousel(viewModelFacture));
        }

        private async void NumAssureTapped(object sender, EventArgs e)
        {
            var obj = (sender as Label).Text.Split('-');
            View_CFA_MOBILE_DETAIL_FACTURE beneficiare = new View_CFA_MOBILE_DETAIL_FACTURE()
            {
                RAND_AD = obj[1].Replace(" &#xf35a;", ""),
                NUM_ASSURE = obj[0]
            };
            await Navigation.PushAsync(new BeneficiaresDetailPage(beneficiare,false));
        }
    }
}