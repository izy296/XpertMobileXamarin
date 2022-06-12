using Acr.UserDialogs;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Diagnostics;
using System.Text;
using System.Threading.Tasks;
using Xamarin.Forms;
using Xpert.Common.DAO;
using Xpert.Common.WSClient.Helpers;
using XpertMobileApp.Api.ViewModels;
using XpertMobileApp.DAL;
using XpertMobileApp.Helpers;
using XpertMobileApp.Models;
using XpertMobileApp.Services;
using XpertMobileApp.Views._04_Comercial.TransfertDeFond;

namespace XpertMobileApp.ViewModels
{
    public class TransfertDeFondPageViewModel : CrudBaseViewModel2<TRS_VIREMENT, View_TRS_VIREMENT>
    {
        public string TypeDoc { get; set; }

        public TransfertDeFondPageViewModel()
        {
            Title = AppResources.pn_TransfertDeFond;
            Comptes = new ObservableCollection<View_BSE_COMPTE>();
            LoadExtrasDataCommand = new Command(async () => await ExecuteLoadExtrasDataCommand());
        }
        #region filters data 
        public DateTime StartDate { get; set; } = DateTime.ParseExact("2022-01-01", "yyyy-MM-dd", System.Globalization.CultureInfo.InvariantCulture);
        public DateTime EndDate { get; set; } = DateTime.Now;
        public ObservableCollection<View_BSE_COMPTE> Comptes { get; set; }
        public View_BSE_COMPTE SelectedCompteSrc { get; set; }
        public View_BSE_COMPTE SelectedCompteDst { get; set; }

        private bool transfertCloture;
        public bool TransfertCloture
        {
            get { return transfertCloture; }
            set { SetProperty(ref transfertCloture, value); }
        }
        #endregion

        /// <summary>
        /// Avoir les parametre de filtre 
        /// </summary>
        /// <returns></returns>
        protected override QueryInfos GetFilterParams()
        {
            base.GetFilterParams();

            this.AddCondition<View_TRS_VIREMENT, DateTime?>(e => e.CREATED_ON, Operator.BETWEEN_DATE, StartDate, EndDate);

            if (!string.IsNullOrEmpty(SelectedCompteSrc?.DESIGN_COMPTE))
                this.AddCondition<View_TRS_VIREMENT, string>(e => e.DESIGN_COMPTE_SRC, SelectedCompteSrc?.DESIGN_COMPTE);

            if (!string.IsNullOrEmpty(SelectedCompteDst?.DESIGN_COMPTE))
                this.AddCondition<View_TRS_VIREMENT, string>(e => e.DESIGN_COMPTE_DEST, SelectedCompteDst?.DESIGN_COMPTE);

            if (transfertCloture != true)
                this.AddCondition<View_TRS_VIREMENT, string>(e => e.CODE_MOTIF, Operator.NOT_EQUAL, "TMC");

            this.AddOrderBy<View_TRS_VIREMENT, DateTime?>(e => e.CREATED_ON, Sort.DESC);

            return qb.QueryInfos;
        }

        protected override void OnAfterLoadItems(IEnumerable<View_TRS_VIREMENT> list)
        {
            base.OnAfterLoadItems(list);
            int i = 0;
            foreach (var item in list)
            {
                i += 1;
                (item as BASE_CLASS).Index = i;
            }
        }

        /// <summary>
        /// Load Design of source accounte
        /// </summary>
        /// <returns></returns>
        /// 
        async Task ExecuteLoadExtrasDataCommand()
        {
            if (IsLoadExtrasBusy)
                return;
            try
            {
                await ExecuteLoadComptes();
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
        /// <summary>
        /// Get designtaion accounts and insert them in the picker 
        /// </summary>
        /// <returns></returns>
        async Task ExecuteLoadComptes()
        {
            try
            {
                IsLoadExtrasBusy = true;
                Comptes.Clear();

                var itemsC = await WebServiceClient.getComptes();
                itemsC.Insert(0, new View_BSE_COMPTE()
                {
                    DESIGNATION_TYPE = "",
                    DESIGN_COMPTE = "",
                    CODE_TYPE = ""
                });
                foreach (var itemC in itemsC)
                {
                    Comptes.Add(itemC);
                }
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
    }
}
