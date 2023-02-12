using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;
using XpertMobileApp.DAL;
using XpertMobileApp.Services;
using XpertMobileApp.Views._05_Officine.Chifa.FactureCHIFA;

namespace XpertMobileApp.Views._05_Officine.Chifa.Consommation
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class CHIFA_ConsommationDetail : TabbedPage
    {
        private CHIFA_ConsommationViewModel viewModel;
        public View_CFA_MOBILE_DETAIL_FACTURE Item { get; set; }
        public ObservableCollection<View_CFA_MOBILE_DETAIL_FACTURE> DetailList { get; set; }
        public ObservableCollection<View_CFA_MOBILE_FACTURE> FactureList { get; set; }
        public ObservableCollection<View_CFA_MOBILE_DETAIL_FACTURE> BeneficiaireList { get; set; }

        public DateTime startDate { get; set; }
        public DateTime endDate { get; set; }

        private int countFacture { get; set; }
        public int CountFacture
        {
            get
            {
                return countFacture;
            }
            set
            {
                countFacture = value;
                OnPropertyChanged("CountFacture");
            }
        }
        public CHIFA_ConsommationDetail(CHIFA_ConsommationViewModel viewModel, View_CFA_MOBILE_DETAIL_FACTURE ConsommationItem, DateTime StartDate, DateTime EndDate)
        {
            InitializeComponent();
            this.viewModel = viewModel;
            this.Item = ConsommationItem;
            this.startDate = StartDate;
            this.endDate = EndDate;
            DetailList = new ObservableCollection<View_CFA_MOBILE_DETAIL_FACTURE>();
            FactureList = new ObservableCollection<View_CFA_MOBILE_FACTURE>();
            BeneficiaireList = new ObservableCollection<View_CFA_MOBILE_DETAIL_FACTURE>();
            BindingContext = this;
        }

        protected override async void OnAppearing()
        {
            base.OnAppearing();
            await GetConsommationDetails();
        }

        private async Task GetConsommationDetails()
        {
            try
            {
                if (IsBusy)
                {
                    return;
                }
                IsBusy = true;
                DateTime startDate = viewModel.StartDate;
                DateTime endDate = viewModel.EndDate;
                string reference = Item.REFERENCE.ToString();
                if (Item.CODE_DCI != null)
                {
                    string codeDCI = Item.CODE_DCI.ToString();
                    var Items = await WebServiceClient.GetListFactDetailByDci(startDate, endDate, codeDCI, reference);

                    if (Items != null)
                    {
                        foreach (var item in Items)
                        {
                            DetailList.Add(item);
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
            finally
            {
                IsBusy = false;
            }
        }

        async Task GetFactureListByReference()
        {
            try
            {
                if (IsBusy)
                {
                    return;
                }
                IsBusy = true;

                string startDate = viewModel.StartDate.ToString("MM/dd/yyyy HH:MM:ss");
                string endDate = viewModel.EndDate.ToString("MM/dd/yyyy HH:MM:ss");
                string codeDCI = Item.CODE_DCI.ToString();
                string reference = Item.REFERENCE.ToString();

                var FactureList = await WebServiceClient.GetFactureListByReference(codeDCI, reference, startDate, endDate);
                CountFacture = FactureList.Count();
                if (FactureList != null)
                {
                    foreach (var item in FactureList)
                    {
                        this.FactureList.Add(item);
                    }
                }
            }
            catch (Exception ex)
            {

                throw ex;
            }
            finally
            {
                IsBusy = false;
            }
        }

        async Task GetBeneficiaireByCodeDci()
        {
            try
            {
                if (IsBusy)
                {
                    return;
                }
                IsBusy = true;

                string codeDCI = Item.CODE_DCI.ToString();

                var BeneficiaireListe = await WebServiceClient.GetBeneficiaireByDci(codeDCI);
                if (FactureList != null)
                {
                    foreach (var item in BeneficiaireListe)
                    {
                        this.BeneficiaireList.Add(item);
                    }
                }
            }
            catch (Exception ex)
            {

                throw ex;
            }
            finally
            {
                IsBusy = false;
            }
        }

        private async void TabbedPage_CurrentPageChanged(object sender, EventArgs e)
        {
            try
            {
                string title = ((TabbedPage)sender).CurrentPage.Title;
                if (title == "Factures")
                {
                    Title = "Liste des factures";
                    await GetFactureListByReference();
                }
                if(title == "Bénéficiaires")
                {
                    Title = "Liste des bénéficiaires";
                    await GetBeneficiaireByCodeDci();
                }
                else
                {
                    Title = "Liste des consommations";
                }
            }
            catch (Exception ex)
            {

                throw ex;
            }
        }
    }
}