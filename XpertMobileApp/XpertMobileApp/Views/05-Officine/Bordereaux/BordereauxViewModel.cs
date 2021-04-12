using Acr.UserDialogs;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Threading.Tasks;
using Xamarin.Forms;
using Xamarin.Forms.Extended;
using Xpert.Common.DAO;
using Xpert.Common.WSClient.Helpers;
using XpertMobileApp.Api.Services;
using XpertMobileApp.Api.ViewModels;
using XpertMobileApp.DAL;
using XpertMobileApp.Services;

namespace XpertMobileApp.ViewModels
{
    public class BordereauxViewModel : CrudBaseViewModel2<CFA_BORDEREAU, View_CFA_BORDEREAU>
    {

        public BordereauxViewModel()
        {
            Title = AppResources.pn_Bordereaux;

            Centres = new ObservableCollection<CFA_CENTRES>();

            brdStatus = new ObservableCollection<CFA_ETAT>();

            LoadExtrasDataCommand = new Command(async () => await ExecuteLoadExtrasDataCommand());
        }

        protected override QueryInfos GetFilterParams()
        {
            base.GetFilterParams();

            // result.Add("searchText", SearchedText);

            if (!string.IsNullOrEmpty(SelectedCentre?.CODE))
                this.AddCondition<View_CFA_BORDEREAU, string>(e => e.ID_CENTRE, SelectedCentre?.CODE); 

            if (!string.IsNullOrEmpty(SelectedSTATUS?.CODE_ETAT))
                this.AddCondition<View_CFA_BORDEREAU, string>(e => e.CODE_ETAT, SelectedSTATUS?.CODE_ETAT);

            this.AddOrderBy<View_CFA_BORDEREAU, string>(e => e.CODE_ETAT);

            return qb.QueryInfos;
        }

        #region filters data

        public DateTime StartDate { get; set; } = DateTime.Now.AddMonths(-3);
        public DateTime EndDate { get; set; } = DateTime.Now;

        private string searchedText;
        public string SearchedText
        {
            get { return searchedText; }
            set { SetProperty(ref searchedText, value); }
        }

        public ObservableCollection<CFA_CENTRES> Centres { get; set; }
        CFA_CENTRES selectedCentre;
        public CFA_CENTRES SelectedCentre
        {
            get { return selectedCentre; }
            set { SetProperty(ref selectedCentre, value); }
        }

        public ObservableCollection<CFA_ETAT> brdStatus { get; set; }
        CFA_ETAT selectedSTATUS;
        public CFA_ETAT SelectedSTATUS
        {
            get { return selectedSTATUS; }
            set { SetProperty(ref selectedSTATUS, value); }
        }

        async Task ExecuteLoadExtrasDataCommand()
        {

            if (IsLoadExtrasBusy)
                return;

            try
            {
                IsLoadExtrasBusy = true;
                await ExecuteLoadTypesProduitCommand();
                await ExecuteLoadTypesCommand();
            }
            catch (Exception ex)
            {
                await UserDialogs.Instance.AlertAsync(WSApi2.GetExceptionMessage(ex), AppResources.alrt_msg_Alert,
                    AppResources.alrt_msg_Ok);
            }
            finally
            {
                IsLoadExtrasBusy = false;
            }
        }

        async Task ExecuteLoadTypesProduitCommand()
        {

            try
            {
                brdStatus.Clear();
                var itemsC = await WebServiceClient.getBordereauxSTATUS();

                CFA_ETAT allElem = new CFA_ETAT();
                allElem.CODE_ETAT = "";
                allElem.DESIGN_ETAT = AppResources.txt_All;
                itemsC.Add(allElem);

                foreach (var itemC in itemsC)
                {
                    brdStatus.Add(itemC);
                }
            }
            catch (Exception ex)
            {
                await UserDialogs.Instance.AlertAsync(WSApi2.GetExceptionMessage(ex), AppResources.alrt_msg_Alert,
                    AppResources.alrt_msg_Ok);
            }

        }

        async Task ExecuteLoadTypesCommand()
        {

            try
            {
                Centres.Clear();
                var itemsC = await WebServiceClient.getBordereauxCentresTypes();

                CFA_CENTRES allElem = new CFA_CENTRES();
                allElem.CODE = "";
                allElem.DESIGNATION = "";
                Centres.Add(allElem);

                foreach (var itemC in itemsC)
                {
                    Centres.Add(itemC);
                }
            }
            catch (Exception ex)
            {
                await UserDialogs.Instance.AlertAsync(WSApi2.GetExceptionMessage(ex), AppResources.alrt_msg_Alert,
                    AppResources.alrt_msg_Ok);
            }
        }

        public override void ClearFilters()
        {
            base.ClearFilters();
            SearchedText = "";
            SelectedCentre = null;
            SelectedSTATUS = null;
        }
        #endregion
    }
}
