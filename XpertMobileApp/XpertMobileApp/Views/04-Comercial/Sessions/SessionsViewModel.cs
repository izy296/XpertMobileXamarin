using Acr.UserDialogs;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Threading.Tasks;
using Xamarin.Forms;
using Xpert.Common.DAO;
using Xpert.Common.WSClient.Helpers;
using XpertMobileApp.Api.Models;
using XpertMobileApp.Api.ViewModels;
using XpertMobileApp.DAL;
using XpertMobileApp.Services;

namespace XpertMobileApp.ViewModels
{

    public class SessionsViewModel : CrudBaseViewModel2<TRS_JOURNEES, TRS_JOURNEES>
    {

        EncaissDisplayType encaissDisplayType;
        public EncaissDisplayType EncaissDisplayType
        {
            get { return encaissDisplayType; }
            set { SetProperty(ref encaissDisplayType, value); }
        }

        DateTime startDate = DateTime.Now.AddDays(-15);
        public DateTime StartDate
        {
            get { return startDate; }
            set { SetProperty(ref startDate, value); }
        }

        DateTime endDate = DateTime.Now;
        public DateTime EndDate
        {
            get { return endDate; }
            set { SetProperty(ref endDate, value); }
        }

        public ObservableCollection<View_BSE_COMPTE> Comptes { get; set; }
        View_BSE_COMPTE selectedCompte;
        public View_BSE_COMPTE SelectedCompte
        {
            get { return selectedCompte; }
            set { SetProperty(ref selectedCompte, value); }
        }

        public override void ClearFilters()
        {
            base.ClearFilters();
            StartDate = DateTime.Now.AddDays(-15);
            EndDate = DateTime.Now;
            SelectedCompte = null;
        }

        public SessionsViewModel()
        {
            Title = AppResources.pn_session;            
            Comptes = new ObservableCollection<View_BSE_COMPTE>();
            LoadExtrasDataCommand = new Command(async () => await ExecuteLoadExtrasDataCommand());
        }

        protected override QueryInfos GetFilterParams()
        {
            base.GetFilterParams();

            this.AddCondition<TRS_JOURNEES, DateTime>(e => e.DATE_JOURNEE, Operator.BETWEEN_DATE, StartDate, EndDate);

            this.AddCondition<TRS_JOURNEES, bool>(e => e.JOURNEE_CLOTUREE, true);

            if (!string.IsNullOrEmpty(SelectedCompte?.CODE_COMPTE))
                this.AddCondition<TRS_JOURNEES, string>(e => e.CODE_COMPTE, SelectedCompte?.CODE_COMPTE);

            this.AddOrderBy<TRS_JOURNEES, DateTime>(e => e.DATE_JOURNEE);

            return qb.QueryInfos;
        }

        protected override QueryInfos GetSelectParams()
        {
            base.GetSelectParams();
            this.AddSelect<TRS_JOURNEES, string>(e => e.ID_CAISSE);
            this.AddSelect<TRS_JOURNEES, string>(e => e.DEBUTEE_PAR);
            this.AddSelect<TRS_JOURNEES, string>(e => e.POSTE_DEBUT);
            //this.AddSelect<TRS_JOURNEES, decimal>(e => e.MONT_CLOTURE_PH_virtual);
            this.AddSelect<TRS_JOURNEES, decimal>(e => e.MONT_ECART);
            this.AddSelect<TRS_JOURNEES, DateTime?>(e => e.DATE_DEBUT);
            this.AddSelect<TRS_JOURNEES, string>(e => e.CLOTUREE_PAR);
            this.AddSelect<TRS_JOURNEES, DateTime?>(e => e.DATE_CLOTURE);
            this.AddSelect<TRS_JOURNEES, bool>(e => e.JOURNEE_CLOTUREE);

            return qb.QueryInfos;
        }

        protected override void OnAfterLoadItems(IEnumerable<TRS_JOURNEES> list)
        {
            base.OnAfterLoadItems(list);

            int i = 0;
            foreach (var item in list)
            {
                i += 1;
                (item as BASE_CLASS).Index = i;
            }
        }

        private string GetCurrentType()
        {
            string type = "";
            switch (EncaissDisplayType)
            {
                case EncaissDisplayType.All:
                    type = "all";
                    break;
                case EncaissDisplayType.ENC:
                    type = "ENC";
                    break;
                case EncaissDisplayType.DEC:
                    type = "DEC";
                    break;
            }

            return type;
        }

        async Task ExecuteLoadExtrasDataCommand()
        {
            
            if (IsLoadExtrasBusy)
                return;
            
            try
            {
                IsLoadExtrasBusy = true;
                Comptes.Clear();
                var itemsC = await WebServiceClient.getComptes();
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

        /// <summary>
        /// 
        /// </summary>
        /// <param name="idCaisse"></param>
        /// <returns></returns>
        public async Task<TRS_JOURNEES> GetItemById(string idCaisse)
        {
            try
            {
                UserDialogs.Instance.ShowLoading(AppResources.txt_Waiting);
                XpertSqlBuilder qbOneQuery = new XpertSqlBuilder();
                qbOneQuery.AddCondition<TRS_JOURNEES, string>(e => e.ID_CAISSE, idCaisse);
                qbOneQuery.AddOrderBy<TRS_JOURNEES, DateTime>(e => e.DATE_JOURNEE);

                qbOneQuery.AddSelect<TRS_JOURNEES, string>(e => e.DEBUTEE_PAR);
                qbOneQuery.AddSelect<TRS_JOURNEES, string>(e => e.POSTE_DEBUT);
                //this.AddSelect<TRS_JOURNEES, decimal>(e => e.MONT_CLOTURE_PH_virtual);
                qbOneQuery.AddSelect<TRS_JOURNEES, decimal>(e => e.MONT_ECART);
                qbOneQuery.AddSelect<TRS_JOURNEES, DateTime?>(e => e.DATE_DEBUT);
                qbOneQuery.AddSelect<TRS_JOURNEES, string>(e => e.CLOTUREE_PAR);
                qbOneQuery.AddSelect<TRS_JOURNEES, DateTime?>(e => e.DATE_CLOTURE);
                qbOneQuery.AddSelect<TRS_JOURNEES, bool>(e => e.JOURNEE_CLOTUREE);

                List<TRS_JOURNEES> items = (List<TRS_JOURNEES>) await service.SelectByPage(qbOneQuery.QueryInfos, 1, 1);
                UserDialogs.Instance.HideLoading();
                if (items.Count != 0)
                    return items[0];
                else return null;

            }
            catch (Exception ex)
            {
                UserDialogs.Instance.HideLoading();
                await UserDialogs.Instance.AlertAsync(ex.Message, AppResources.alrt_msg_Alert, AppResources.alrt_msg_Ok);
                return null;
            }
        }
    }

}
