using Acr.UserDialogs;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Threading.Tasks;
using Xamarin.Forms;
using Xamarin.Forms.Extended;
using Xpert.Common.DAO;
using Xpert.Common.WSClient.Helpers;
using XpertMobileApp.Api;
using XpertMobileApp.Api.ViewModels;
using XpertMobileApp.DAL;
using XpertMobileApp.Services;
using XpertMobileApp.SQLite_Managment;

namespace XpertMobileApp.ViewModels
{

    public class AchatsListViewModel : CrudBaseViewModel2<ACH_DOCUMENT, View_ACH_DOCUMENT>
    {


        public string TypeDoc { get; set; } = "LF";
        public bool InclureEchange { get; set; } = false;
        public string RefDocum { get; set; } // Reference du document original (fournisseur)

        decimal totalTurnover;
        public decimal TotalTurnover
        {
            get { return totalTurnover; }
            set { SetProperty(ref totalTurnover, value); }
        }

        decimal totalMargin;
        public decimal TotalMargin
        {
            get { return totalMargin; }
            set { SetProperty(ref totalMargin, value); }
        }

        public bool hasEditHeader
        {
            get
            {
                if (AppManager.HasAdmin) return true;

                bool result = false;
                if (AppManager.permissions != null)
                {
                    var obj = AppManager.permissions.Where(x => x.CodeObjet == "ACH_UPDATE_ENTETE").FirstOrDefault();
                    result = obj != null && obj.AcUpdate > 0;
                }
                return result;
            }
        }

        public View_TRS_TIERS SelectedTiers { get; set; }

        public EncaissDisplayType EncaissDisplayType { get; set; }
        public DateTime StartDate { get; set; } = DateTime.ParseExact("2020-03-21", "yyyy-MM-dd", System.Globalization.CultureInfo.InvariantCulture);
        public DateTime EndDate { get; set; } = DateTime.Now;


        public ObservableCollection<BSE_DOCUMENT_STATUS> Status { get; set; }
        public BSE_DOCUMENT_STATUS SelectedStatus { get; set; }


        public ObservableCollection<View_BSE_COMPTE> Client { get; set; }
        public View_BSE_COMPTE SelectedClient { get; set; }
        public Command LoadClientsCommand { get; set; }


        public ObservableCollection<View_BSE_COMPTE> User { get; set; }
        public View_BSE_COMPTE SelectedUser { get; set; }
        public Command LoadUsersCommand { get; set; }



        public AchatsListViewModel(string typeDoc)
        {
            Title = AppResources.pn_Achats;
            TypeDoc = typeDoc;
            Status = new ObservableCollection<BSE_DOCUMENT_STATUS>();
            base.InitConstructor();
            LoadExtrasDataCommand = new Command(async () => await ExecuteLoadExtrasDataCommand());
            LoadSummaries = true;
        }

        async Task ExecuteLoadExtrasDataCommand()
        {
            if (IsLoadExtrasBusy)
                return;

            try
            {
                IsLoadExtrasBusy = true;
                //await autres filtres
                await ExecuteLoadStatusDocAchats();
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

        async Task ExecuteLoadStatusDocAchats()
        {
            if (App.Online)
            {
                try
                {
                    Status.Clear();
                    var itemsC = await WebServiceClient.getManquantsTypes();

                    BSE_DOCUMENT_STATUS allElem = new BSE_DOCUMENT_STATUS();
                    allElem.CODE_STATUS = "";
                    allElem.NAME = "";
                    Status.Add(allElem);
                    foreach (var itemC in itemsC)
                    {
                        Status.Add(itemC);
                    }
                }
                catch (Exception ex)
                {
                    await UserDialogs.Instance.AlertAsync(WSApi2.GetExceptionMessage(ex), AppResources.alrt_msg_Alert,
                        AppResources.alrt_msg_Ok);
                }
            }
            else
            {

            }

        }

        protected override string ContoleurName
        {
            get
            {
                return "ACH_ACHATS";
            }
        }

        protected override QueryInfos GetFilterParams()
        {
            base.GetFilterParams();

            this.AddCondition<View_ACH_DOCUMENT, DateTime?>(e => e.DATE_DOC, Operator.BETWEEN_DATE, StartDate, EndDate);

            if (!string.IsNullOrEmpty(SelectedTiers?.CODE_TIERS))
                this.AddCondition<View_ACH_DOCUMENT, string>(e => e.CODE_TIERS, SelectedTiers?.CODE_TIERS);

            if (!string.IsNullOrEmpty(RefDocum))
                this.AddCondition<View_ACH_DOCUMENT, string>(e => e.REF_TIERS, RefDocum);

            if (!InclureEchange)
            {
                this.AddCondition<View_ACH_DOCUMENT, string>(e => e.CODE_MOTIF, Operator.NOT_EQUAL, "ES02");
                this.AddCondition<View_ACH_DOCUMENT, string>(e => e.CODE_MOTIF, Operator.NOT_EQUAL, "ES05");
            }

            //if (!string.IsNullOrEmpty(SelectedStatus?.CODE_STATUS))
            //   this.AddCondition<View_ACH_DOCUMENT, string>(e => e.STATUS_DOC, SelectedStatus?.CODE_STATUS);

            //this.AddCondition<View_ACH_DOCUMENT, string>(e => e.CODE_MOTIF, "ES10");

            this.AddOrderBy<View_ACH_DOCUMENT, DateTime?>(e => e.DATE_DOC, Sort.DESC);

            return qb.QueryInfos;
        }

        protected override QueryInfos GetSelectParams()
        {
            base.GetSelectParams();

            this.AddSelect<View_ACH_DOCUMENT, string>(e => e.CODE_DOC);
            this.AddSelect<View_ACH_DOCUMENT, string>(e => e.TYPE_DOC);
            this.AddSelect<View_ACH_DOCUMENT, string>(e => e.CODE_TIERS);
            this.AddSelect<View_ACH_DOCUMENT, string>(e => e.TIERS_NomC);
            this.AddSelect<View_ACH_DOCUMENT, string>(e => e.CODE_TIERS);
            this.AddSelect<View_ACH_DOCUMENT, DateTime?>(e => e.CREATED_ON);
            this.AddSelect<View_ACH_DOCUMENT, DateTime?>(e => e.DATE_DOC);
            this.AddSelect<View_ACH_DOCUMENT, string>(e => e.CREATED_BY);
            this.AddSelect<View_ACH_DOCUMENT, string>(e => e.DESIGNATION_STATUS);
            this.AddSelect<View_ACH_DOCUMENT, string>(e => e.REF_TIERS);
            this.AddSelect<View_ACH_DOCUMENT, decimal>(e => e.TOTAL_TTC_REEL);
            this.AddSelect<View_ACH_DOCUMENT, decimal>(e => e.TOTAL_RESTE_REEL);
            this.AddSelect<View_ACH_DOCUMENT, decimal>(e => e.TOTAL_RISTOURNE_REEL);
            this.AddSelect<View_ACH_DOCUMENT, decimal>(e => e.TOTAL_MARGE);
            this.AddSelect<View_ACH_DOCUMENT, decimal>(e => e.TAUX_MARGE);
            this.AddSelect<View_ACH_DOCUMENT, decimal>(e => e.TOTAL_PPA);
            this.AddSelect<View_ACH_DOCUMENT, decimal>(e => e.TOTAL_SHP);
            this.AddSelect<View_ACH_DOCUMENT, decimal>(e => e.TOTAL_HT);
            this.AddSelect<View_ACH_DOCUMENT, decimal>(e => e.TOTAL_TVA);
            this.AddSelect<View_ACH_DOCUMENT, decimal>(e => e.TOTAL_TTC);

            return qb.QueryInfos;
        }

        protected override void OnAfterLoadItems(IEnumerable<View_ACH_DOCUMENT> list)
        {
            base.OnAfterLoadItems(list);

            if (Summaries.Count > 0)
            {
                Summaries.Add(Summaries[2]);
                Summaries.RemoveAt(2);

                if (!this.HasAdmin)
                {
                    var Tot_Achat = Summaries[1];
                    Summaries.Clear();
                    Summaries.Add(Tot_Achat);
                }
            }

            int i = 0;
            foreach (var item in list)
            {
                i += 1;
                (item as BASE_CLASS).Index = i;
            }
        }

        async Task ExecuteLoadItemsCommand()
        {
            if (IsBusy)
                return;

            try
            {
                Items.Clear();

                // liste des ventes
                await Items.LoadMoreAsync();
            }
            catch (Exception ex)
            {
                await UserDialogs.Instance.AlertAsync(WSApi2.GetExceptionMessage(ex), AppResources.alrt_msg_Alert,
                    AppResources.alrt_msg_Ok);
            }
            finally
            {
                IsBusy = false;
            }
        }
    }

}
