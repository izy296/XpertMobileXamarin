using Acr.UserDialogs;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Globalization;
using System.Linq;
using System.Threading.Tasks;
using Xamarin.Forms;
using Xamarin.Forms.Extended;
using Xpert.Common.DAO;
using Xpert.Common.WSClient.Helpers;
using XpertMobileApp.Api;
using XpertMobileApp.Api.Services;
using XpertMobileApp.Api.ViewModels;
using XpertMobileApp.DAL;
using XpertMobileApp.Helpers;
using XpertMobileApp.Services;
using XpertMobileApp.SQLite_Managment;
using XpertMobileApp.Views.Encaissement;

namespace XpertMobileApp.ViewModels
{
    public enum EncaissDisplayType { None, All, ENC, DEC };

    public class EncaissementsViewModel : CrudBaseViewModel2<TRS_ENCAISS, View_TRS_ENCAISS>
    {

        public EncaissDisplayType EncaissDisplayType { get; set; }


        private DateTime startDate;
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

        public ObservableCollection<BSE_ENCAISS_MOTIFS> Motifs { get; set; }
        BSE_ENCAISS_MOTIFS selectedMotif;
        public BSE_ENCAISS_MOTIFS SelectedMotif
        {
            get { return selectedMotif; }
            set { SetProperty(ref selectedMotif, value); }
        }
        public View_TRS_TIERS SelectedTiers { get; set; }
        public bool CheckBoxTransfertDeFond
        {
            get;
            set;
        } = false;
        public EncaissementsViewModel()
        {
            Title = AppResources.pn_encaissement;

            Comptes = new ObservableCollection<View_BSE_COMPTE>();
            Motifs = new ObservableCollection<BSE_ENCAISS_MOTIFS>();
            LoadExtrasDataCommand = new Command(async () => await ExecuteLoadExtrasDataCommand());
            if (App.Online)
                StartDate = DateTime.Now;
        }
        protected override string ContoleurName
        {
            get
            {
                return Constants.AppName == Apps.XCOM_Mob || Constants.AppName == Apps.X_DISTRIBUTION ? "TRS_ENCAISS_XCOM" : "TRS_ENCAISS";
            }
        }

        protected override QueryInfos GetFilterParams()
        {

            base.GetFilterParams();
            string type = GetCurrentType();

            this.AddCondition<View_TRS_ENCAISS, DateTime?>(e => e.DATE_ENCAISS, Operator.BETWEEN_DATE, StartDate, EndDate);

            if (!CheckBoxTransfertDeFond)
            {
                this.AddCondition<View_TRS_ENCAISS, string>(e => e.CODE_MOTIF, Operator.NOT_EQUAL, "3");
            }

            if (!string.IsNullOrEmpty(type))
                this.AddCondition<View_TRS_ENCAISS, string>(e => e.CODE_TYPE, type);

            if (!string.IsNullOrEmpty(SelectedCompte?.CODE_COMPTE))
                this.AddCondition<View_TRS_ENCAISS, string>(e => e.CODE_COMPTE, SelectedCompte?.CODE_COMPTE);

            if (!string.IsNullOrEmpty(SelectedMotif?.CODE_MOTIF))
                this.AddCondition<View_TRS_ENCAISS, string>(e => e.CODE_MOTIF, SelectedMotif?.CODE_MOTIF);

            if (!string.IsNullOrEmpty(SelectedTiers?.CODE_TIERS))
                this.AddCondition<View_TRS_ENCAISS, string>(e => e.CODE_TIERS, SelectedTiers?.CODE_TIERS);

            this.AddOrderBy<View_TRS_ENCAISS, DateTime?>(e => e.DATE_ENCAISS, Sort.DESC);

            return qb.QueryInfos;
        }

        protected override void OnAfterLoadItems(IEnumerable<View_TRS_ENCAISS> list)
        {
            base.OnAfterLoadItems(list);

            int i = 0;
            foreach (var item in list)
            {
                i += 1;
                (item as BASE_CLASS).Index = i;
            }
        }

        public override async Task<List<View_TRS_ENCAISS>> SelectByPageFromSqlLite(QueryInfos filter)
        {
            var res = await base.SelectByPageFromSqlLite(filter);
            if (EncaissDisplayType.ToString() != "" && EncaissDisplayType.ToString() != EncaissDisplayType.All.ToString())
            {
                res = res.Where(x => x.CODE_TYPE == EncaissDisplayType.ToString()).ToList();
                return res;
            }

            if (!string.IsNullOrEmpty(SelectedTiers?.CODE_TIERS))
                res = res.Where(e => e.CODE_TIERS == SelectedTiers?.CODE_TIERS).ToList();

            if (StartDate == null)
            {
                res = res.Where(e => StartDate.Date.CompareTo(((DateTime)e.DATE_ENCAISS).Date) <= 0 && EndDate.Date.CompareTo(((DateTime)e.DATE_ENCAISS).Date) >= 0).ToList();
            }

            if (selectedCompte != null)
                res = res.Where(e => e.CODE_COMPTE == selectedCompte.CODE_COMPTE).ToList();

            if (selectedMotif != null)
                res = res.Where(e => e.CODE_MOTIF == selectedMotif.CODE_MOTIF).ToList();

            return res;
        }

        private string GetCurrentType()
        {
            string type = "";
            switch (EncaissDisplayType)
            {
                case EncaissDisplayType.All:
                    type = "";
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

        public async Task ExecuteLoadExtrasDataCommand()
        {

            if (IsLoadExtrasBusy)
                return;
            if (App.Online)
            {
                try
                {
                    IsLoadExtrasBusy = true;
                    Comptes.Clear();
                    Motifs.Clear();

                    var itemsC = await WebServiceClient.getComptes();
                    itemsC.Insert(0, new View_BSE_COMPTE()
                    {
                        DESIGNATION_TYPE = AppResources.txt_All,
                        DESIGN_COMPTE = AppResources.txt_All,
                        CODE_TYPE = ""
                    });

                    var itemsM = await WebServiceClient.GetMotifs(GetCurrentType());
                    itemsM.Insert(0, new BSE_ENCAISS_MOTIFS()
                    {
                        DESIGN_MOTIF = AppResources.txt_All,
                        CODE_MOTIF = ""
                    });

                    foreach (var itemC in itemsC)
                    {
                        Comptes.Add(itemC);
                    }

                    foreach (var itemM in itemsM)
                    {
                        Motifs.Add(itemM);
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
            else
            {
                try
                {
                    IsLoadExtrasBusy = true;
                    Comptes.Clear();
                    Motifs.Clear();
                    //Obtenir les comptes de la table bse_compte...
                    var itemsC = await SQLite_Manager.getComptes();

                    //Obtenir les motif de la table BSE_ENCAISS_MOTIFS...
                    var itemsM = await SQLite_Manager.getMotifs();

                    foreach (var itemC in itemsC)
                    {
                        Comptes.Add(itemC);
                    }

                    foreach (var itemM in itemsM)
                    {
                        Motifs.Add(itemM);
                    }
                }
                catch (Exception ex)
                {
                    await UserDialogs.Instance.AlertAsync("veuillez synchroniser svp !!", AppResources.alrt_msg_Alert, AppResources.alrt_msg_Ok);
                }
                finally
                {
                    IsLoadExtrasBusy = false;
                }
            }
        }

        public override void ClearFilters()
        {
            base.ClearFilters();
            StartDate = DateTime.Now;
            EndDate = DateTime.Now;
            SelectedCompte = null;
            selectedMotif = null;
        }

    }

}
