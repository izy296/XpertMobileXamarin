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

namespace XpertMobileApp.Views._04_Comercial.Templates
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class CHIFA_FactureDetailsTemplate : ContentPage
    {
        public ObservableCollection<View_CFA_MOBILE_DETAIL_FACTURE> FactureDetail { get; set; }
        private View_CFA_MOBILE_FACTURE cfaFacture { get; set; }
        public View_CFA_MOBILE_FACTURE CfaFacture
        {
            get
            {
                return cfaFacture;
            }
            set
            {
                cfaFacture = value;
                OnPropertyChanged("CfaFacture");
            }

        }

        private double prixTotal { get; set; }
        public double PrixTotal
        {
            get
            {
                return prixTotal;
            }
            set
            {
                prixTotal = value;
                OnPropertyChanged("PrixTotal");
            }
        }
        private string numBorderau { get; set; }
        public string Title { get; set; }
        public string NumBorderau
        {
            get
            {
                return numBorderau;
            }
            set
            {
                numBorderau = value;
                OnPropertyChanged("NumBorderau");
            }
        }
        private double quantiteVignette { get; set; }
        public double QuantiteVignette
        {
            get
            {
                return quantiteVignette;
            }
            set
            {
                quantiteVignette = value;
                OnPropertyChanged("QuantiteVignette");
            }
        }


        private string bENEFICIAIRE { get; set; }
        public string BENEFICIAIRE
        {
            get
            {
                return bENEFICIAIRE;
            }
            set
            {
                bENEFICIAIRE = value;
                OnPropertyChanged("BENEFICIAIRE");
            }
        }
        private double mtMajoration { get; set; }
        public double MtMajoration
        {
            get
            {
                return mtMajoration;
            }
            set
            {
                mtMajoration = value;
                OnPropertyChanged("MtMajoration");
            }
        }


        private double tarifTotal { get; set; }
        public double TarifTotal
        {
            get
            {
                return tarifTotal;
            }
            set
            {
                tarifTotal = value;
                OnPropertyChanged("tarifTotal");
            }
        }

        public CHIFA_FactureDetailsTemplate(View_CFA_MOBILE_FACTURE cFAObject)
        {
            InitializeComponent();
            FactureDetail = new ObservableCollection<View_CFA_MOBILE_DETAIL_FACTURE>();
            this.CfaFacture = cFAObject;
            BindingContext = this;
            BENEFICIAIRE = CfaFacture.NOMC_TIERS + " " + CfaFacture.NUM_ASSURE + "-" + CfaFacture.RAND_AD;
        }

        protected override void OnAppearing()
        {
            base.OnAppearing();
            if (CfaFacture != null)
            {
                GetDetailFacture(CfaFacture.NUM_FACTURE);
            }
        }

        private void OnItemSelected(object sender, SelectedItemChangedEventArgs e)
        {

        }

        public async void GetDetailFacture(string numeroFacture)
        {
            try
            {
                if (FactureDetail != null)
                {
                    FactureDetail.Clear();
                    var itemsDetails = await WebServiceClient.GetDetailsFacture(numeroFacture);
                    if (itemsDetails.Count() > 0)
                    {
                        NumBorderau = itemsDetails[0].NUM_BOURDEREAU;
                        foreach (var itemDetail in itemsDetails)
                        {
                            FactureDetail.Add(itemDetail);
                            PrixTotal += itemDetail.PRIX_VENTE * itemDetail.QUANTITE;
                            TarifTotal += itemDetail.TARIF;
                            QuantiteVignette += itemDetail.QUANTITE;
                            MtMajoration += itemDetail.MONT_MAJORATION;
                        }
                    }

                }
            }
            catch (Exception ex)
            {

                throw ex;
            }
        }
    }
}