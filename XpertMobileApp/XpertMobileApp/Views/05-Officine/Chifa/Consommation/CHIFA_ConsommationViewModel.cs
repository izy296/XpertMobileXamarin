using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Text;
using System.Threading.Tasks;
using Xamarin.Forms;
using Xpert.Common.DAO;
using XpertMobileApp.Api.ViewModels;
using XpertMobileApp.DAL;
using XpertMobileApp.Services;

namespace XpertMobileApp.Views._05_Officine.Chifa.Consommation
{
    public enum ConsommationDisplayType { DCI, NOMC };
    public class CHIFA_ConsommationViewModel : CrudBaseViewModel2<View_CFA_MOBILE_DETAIL_FACTURE, View_CFA_MOBILE_DETAIL_FACTURE>
    {
        public ConsommationDisplayType ConsommationDisplayType { get; set; }
        private DateTime startDate { get; set; } = DateTime.Now.AddYears(-1);
        public DateTime StartDate
        {
            get
            {
                return startDate;
            }
            set
            {
                startDate = value;
                OnPropertyChanged("StartDate");
            }
        }
        public DateTime endDate { get; set; } = DateTime.Now;
        public DateTime EndDate
        {
            get
            {
                return endDate;
            }
            set
            {
                endDate = value;
                OnPropertyChanged("EndDate");
            }
        }
        public ObservableCollection<View_CFA_MOBILE_DETAIL_FACTURE> ListeFactByDci { get; set; }
        public string searchedDCI { get; set; }
        public string searchedNomCommercial { get; set; }
        public bool orderByAlphabetic { get; set; }
        public bool orderByQuatity { get; set; }
        public bool orderByPrice { get; set; }
        public Command LoadListeFactByDciCommand { get; set; }
        public CHIFA_ConsommationViewModel()
        {
            ListeFactByDci = new ObservableCollection<View_CFA_MOBILE_DETAIL_FACTURE>();
            LoadListeFactByDciCommand = new Command(async () => await ExecuteLoadFactByDci());
            PageSize = 30;
        }

        private string GetCurrentDisplayType()
        {
            string type = "";
            switch (ConsommationDisplayType)
            {
                case ConsommationDisplayType.DCI:
                    type = "DCI";
                    break;
                case ConsommationDisplayType.NOMC:
                    type = "NOMC";
                    break;
            }
            return type;
        }

        public async Task InitAndReloadItemList()
        {
            try
            {
                ListeFactByDci.Clear();
                await ExecuteLoadFactByDci();
            }
            catch (Exception ex)
            {

                throw ex;
            }
        }

        protected override QueryInfos GetFilterParams()
        {
            base.GetFilterParams();
            if (!String.IsNullOrEmpty(searchedDCI))
                this.AddCondition<View_CFA_MOBILE_DETAIL_FACTURE, string>(e => e.DESIGN_DCI, Operator.LIKE_ANY, searchedDCI);

            if (!String.IsNullOrEmpty(searchedNomCommercial))
                this.AddCondition<View_CFA_MOBILE_DETAIL_FACTURE, string>(e => e.DESIGNATION_PRODUIT, Operator.LIKE_ANY, searchedNomCommercial);

            if (orderByAlphabetic)
            {
                if (GetCurrentDisplayType() == ConsommationDisplayType.DCI.ToString())
                {
                    this.AddOrderBy<View_CFA_MOBILE_DETAIL_FACTURE, string>(e => e.DESIGN_DCI);
                    this.AddGroupBy("DESIGN_DCI, REFERENCE");
                }
                else
                {
                    this.AddOrderBy<View_CFA_MOBILE_DETAIL_FACTURE, string>(e => e.DESIGNATION_PRODUIT);
                    this.AddGroupBy("DESIGNATION_PRODUIT, REFERENCE");
                }

            }
            else if (orderByPrice)
            {
                this.AddOrderBy("MONT_FACTURE");
                if (GetCurrentDisplayType() == ConsommationDisplayType.DCI.ToString())
                {
                    this.AddGroupBy("DESIGN_DCI, MONT_FACTURE, REFERENCE");
                }
                else
                {
                    this.AddGroupBy("DESIGNATION_PRODUIT, REFERENCE, ");
                }
            }
            else if (orderByQuatity)
            {
                this.AddOrderBy("QUANTITE");
                if (GetCurrentDisplayType() == ConsommationDisplayType.DCI.ToString())
                {
                    this.AddGroupBy("DESIGN_DCI, QUANTITE, REFERENCE");
                }
                else
                {
                    this.AddGroupBy("DESIGNATION_PRODUIT, QUANTITE, REFERENCE");
                }
            }

            return qb.QueryInfos;
        }

        async Task ExecuteLoadFactByDci()
        {
            try
            {
                if (IsBusy)
                    return;
                IsBusy = true;
                var page = (ListeFactByDci.Count / PageSize) + 1;

                var liste = await WebServiceClient.GetListFactureByDci(
                    StartDate.ToString("MM/dd/yyyy HH:MM:ss"),
                    EndDate.ToString("MM/dd/yyyy HH:MM:ss"),
                    GetCurrentDisplayType(),
                    GetFilterParams(),
                    page,
                    PageSize
                    );

                if (liste != null)
                {
                    foreach (var item in liste)
                    {
                        ListeFactByDci.Add(item);
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
    }
}
