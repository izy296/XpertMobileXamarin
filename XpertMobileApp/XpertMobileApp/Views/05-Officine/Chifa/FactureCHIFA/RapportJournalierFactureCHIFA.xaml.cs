using Rg.Plugins.Popup.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;
using XpertMobileApp.DAL;
using XpertMobileApp.Views._04_Comercial.Templates;

namespace XpertMobileApp.Views._05_Officine.Chifa.FactureCHIFA
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class RapportJournalierFactureCHIFA : ContentPage
    {
        RapportJournalierFactureCHIFAViewModel viewModel;
        public RapportJournalierFactureCHIFA()
        {
            InitializeComponent();
            BindingContext = this.viewModel = new RapportJournalierFactureCHIFAViewModel();
        }
        protected override void OnAppearing()
        {
            base.OnAppearing();

            if (viewModel.DateTimeListe.Count <= 0)
                viewModel.LoadExtrasDataCommand.Execute(null);

            if (viewModel.Items.Count <= 0)
                viewModel.LoadFactureDataCommand.Execute(null);

            if (viewModel.Totaux.Count <= 0)
            {
                viewModel.LoadTotauxFactureCommand.Execute(null);
            }
        }

        private void selectDate(object sender, EventArgs e)
        {
            try
            {
                viewModel.SearchedNumFacture = String.Empty;
                viewModel.SelectedDate = ((sender as Button).BindingContext as DateTimeList).Date;
                viewModel.LoadTotauxFactureCommand.Execute(null);
                viewModel.LoadFactureDataCommand.Execute(null);

            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        private async void OnItemSelected(object sender, SelectedItemChangedEventArgs e)
        {
            if (e.SelectedItem != null)
            {

                View_CFA_MOBILE_FACTURE cFAObject = (View_CFA_MOBILE_FACTURE)e.SelectedItem;
                //await Navigation.PushAsync(new CHIFA_FactureDetailsTemplate(cFAObject));
                await Navigation.PushAsync(new CHIFA_FactureCarousel(viewModel));
                ItemsListView.SelectedItem = null;
            }

        }

        private async void ShowHideFilter(object sender, EventArgs e)
        {
            CHIFARapportJournalierFacturePopupFilter filter = new CHIFARapportJournalierFacturePopupFilter(viewModel);
            //Load data for the first time ...
            await PopupNavigation.Instance.PushAsync(filter);
        }
    }
}