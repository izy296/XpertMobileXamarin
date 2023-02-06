using Acr.UserDialogs;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Globalization;
using System.Text;
using System.Threading.Tasks;
using Xamarin.Forms;
using Xpert.Common.DAO;
using Xpert.Common.WSClient.Helpers;
using XpertMobileApp.Api.ViewModels;
using XpertMobileApp.DAL;
using XpertMobileApp.Services;

namespace XpertMobileApp.Views._05_Officine.Chifa.FactureCHIFA
{
    public class DateTimeList
    {
        public DateTime Date { get; set; }
        public string DesignationDate { get; set; }
    }
    public class RapportJournalierFactureCHIFAViewModel : CrudBaseViewModel2<FACTURE_CHIFA, View_CFA_MOBILE_FACTURE>
    {
        private string etatFiltre { get; set; } = "#2196F3";
        public string EtatFiltre
        {
            get
            {
                return etatFiltre;
            }
            set
            {
                etatFiltre = value;
                OnPropertyChanged("EtatFiltre");
            }
        }

        public string SearchedNumFacture { get; set; }
        public Command LoadFactureDataCommand { get; set; }
        public Command LoadTotauxFactureCommand { get; set; }
        public ObservableCollection<DateTimeList> DateTimeListe { get; set; }
        public ObservableCollection<View_CFA_MOBILE_FACTURE> Totaux { get; set; }
        private View_CFA_MOBILE_FACTURE totauxFactures { get; set; }
        public View_CFA_MOBILE_FACTURE TotauxFactures
        {
            get
            {
                return totauxFactures;
            }
            set
            {
                totauxFactures = value;
                OnPropertyChanged("TotauxFactures");
            }
        }
        public string infoFacture { get; set; }
        public string InfoFacture
        {
            get
            {
                return infoFacture;
            }
            set
            {
                infoFacture = value;
                OnPropertyChanged("InfoFacture"); 
            }
        }
        public string numAssure { get; set; }
        public string NumAssure
        {
            get
            {
                return numAssure;
            }
            set
            {
                numAssure = value;
            }
        }

        public string rand { get; set; }
        public string Rand
        {
            get
            {
                return rand;
            }
            set
            {
                rand = value;
            }
        }
        public DateTime SelectedDate { get; set; } = DateTime.Now;

        public RapportJournalierFactureCHIFAViewModel()
        {
            DateTimeListe = new ObservableCollection<DateTimeList>();
            Totaux = new ObservableCollection<View_CFA_MOBILE_FACTURE>();
            TotauxFactures = new View_CFA_MOBILE_FACTURE();
            LoadExtrasDataCommand = new Command(async () => await ExecuteLoadCfaFactDateCommand());
            LoadTotauxFactureCommand = new Command(async () => await ExecuteLoadTotauxFactureCHIFA());
            LoadFactureDataCommand = new Command(async () => await ExecuteLoadFactureByDate());
        }

        protected override QueryInfos GetFilterParams()
        {
            base.GetFilterParams();
            this.AddCondition<View_CFA_MOBILE_FACTURE, DateTime>(e => e.DATE_FACTURE, Operator.EQUAL, SelectedDate);

            if (!String.IsNullOrEmpty(SearchedNumFacture))
                this.AddCondition<View_CFA_MOBILE_FACTURE, string>(e => e.NUM_FACTURE, SearchedNumFacture);

            this.AddOrderBy<View_CFA_MOBILE_FACTURE, DateTime>(e => e.DATE_FACTURE);
            return qb.QueryInfos;
        }

        protected override QueryInfos GetSelectParams()
        {
            base.GetSelectParams();
            this.AddSelect<View_CFA_MOBILE_FACTURE, string>(e => e.NOMC_TIERS);
            this.AddSelect<View_CFA_MOBILE_FACTURE, string>(e => e.NUM_FACTURE);
            this.AddSelect<View_CFA_MOBILE_FACTURE, string>(e => e.DESIGN_ETAT);
            this.AddSelect<View_CFA_MOBILE_FACTURE, decimal>(e => e.MONT_FACTURE);
            this.AddSelect<View_CFA_MOBILE_FACTURE, DateTime>(e => e.DATE_FACTURE);
            this.AddSelect<View_CFA_MOBILE_FACTURE, string>(e => e.NUM_ASSURE);
            this.AddSelect<View_CFA_MOBILE_FACTURE, string>(e => e.RAND_AD);
            this.AddSelect<View_CFA_MOBILE_FACTURE, string>(e => e.CREATED_BY);

            return qb.QueryInfos;
        }


        public async Task ExecuteLoadFactureByDate()
        {
            try
            {
                if (IsBusy) return;
                Items.Clear();
                await Items.LoadMoreAsync();   // liste des ventes
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
        public async Task ExecuteLoadFactureByDateWithoutClear()
        {
            try
            {
                if (IsBusy) return;
                await Items.LoadMoreAsync();   // liste des ventes
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
        async Task ExecuteLoadTotauxFactureCHIFA()
        {
            try
            {
                Totaux.Clear();
                var itemsT = await WebServiceClient.GetTotauxFactureCHIFA(SelectedDate);

                if (itemsT.Count > 0)
                {
                    TotauxFactures = itemsT[0];
                    InfoFacture = $"{TotauxFactures.TOTAL_CHIFA + TotauxFactures.TOTAL_CASNOS} Factures (CNAS: {TotauxFactures.TOTAL_CHIFA} | CASNOS: {TotauxFactures.TOTAL_CASNOS})";
                }
            }
            catch (Exception ex)
            {
                await UserDialogs.Instance.AlertAsync(WSApi2.GetExceptionMessage(ex), AppResources.alrt_msg_Alert,
                    AppResources.alrt_msg_Ok);
            }
        }

        async Task ExecuteLoadCfaFactDateCommand()
        {
            try
            {

                var itemsD = await WebServiceClient.get_CFA_Fact_Dates();
                DateTimeListe.Add(new DateTimeList
                {
                    Date = DateTime.Now,
                    DesignationDate = "Aujourd'hui"
                });
                foreach (var itemD in itemsD)
                {
                    DateTimeListe.Add(new DateTimeList
                    {
                        Date = itemD,
                        DesignationDate = itemD.ToString("dd/MM/yy", CultureInfo.InvariantCulture),
                    });
                }


            }
            catch (Exception ex)
            {
                await UserDialogs.Instance.AlertAsync(WSApi2.GetExceptionMessage(ex), AppResources.alrt_msg_Alert,
                    AppResources.alrt_msg_Ok);
            }
        }
    }
}
