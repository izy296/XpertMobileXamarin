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
    public enum SenderType { LABO, THERAP, CONSOMMATION };

    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class CHIFA_ConsommationDetail : TabbedPage
    {
        private CHIFA_ConsommationViewModel viewModel;
        public SenderType SenderType { get; set; }
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
        public CHIFA_ConsommationDetail(View_CFA_MOBILE_DETAIL_FACTURE ConsommationItem, DateTime StartDate, DateTime EndDate, string type)
        {
            InitializeComponent();
            this.Item = ConsommationItem;
            this.startDate = StartDate;
            this.endDate = EndDate;
            DetailList = new ObservableCollection<View_CFA_MOBILE_DETAIL_FACTURE>();
            FactureList = new ObservableCollection<View_CFA_MOBILE_FACTURE>();
            BeneficiaireList = new ObservableCollection<View_CFA_MOBILE_DETAIL_FACTURE>();
            if (type == SenderType.CONSOMMATION.ToString())
            {
                if(!string.IsNullOrEmpty(ConsommationItem.DESIGNATION_PRODUIT))
                    designationProduit.Text = ConsommationItem.DESIGNATION_PRODUIT;
                else
                    designationProduit.Text = ConsommationItem.DESIGN_DCI;
            }
            else if (type == SenderType.LABO.ToString())
            {
                designationProduit.Text = ConsommationItem.DESIGNATION_LABO;
            }
            else if (type == SenderType.THERAP.ToString())
            {
                designationProduit.Text = ConsommationItem.DESIGNATION_FAMILLE;
            }
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
                DateTime startDate = this.startDate;
                DateTime endDate = this.endDate;
                string reference = "";
                if (Item.REFERENCE != null)
                {
                    reference = Item.REFERENCE.ToString();
                }

                if (Item.CODE_DCI != null)
                {
                    string codeDCI = Item.CODE_DCI.ToString();
                    string codeProduit = Item.CODE_PRODUIT.ToString();
                    var Items = await WebServiceClient.GetListFactDetailByDci(startDate, endDate, codeDCI, reference, codeProduit);

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

                DateTime startDate = this.startDate;
                DateTime endDate = this.endDate;
                string codeDCI = Item.CODE_DCI.ToString();
                string reference = String.Empty;
                if (Item.REFERENCE != null)
                {
                    reference = Item.REFERENCE.ToString();
                }

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
                if (title == "Bénéficiaires")
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

        private void ItemsListView_ItemSelected(object sender, SelectedItemChangedEventArgs e)
        {
            try
            {
                View_CFA_MOBILE_DETAIL_FACTURE cFAObject = (View_CFA_MOBILE_DETAIL_FACTURE)e.SelectedItem;
                //await Navigation.PushAsync(new CHIFA_FactureDetailsTemplate(cFAObject));
                ItemsListView.SelectedItem = null;
            }
            catch (Exception ex)
            {

                throw ex;
            }
        }
    }
}