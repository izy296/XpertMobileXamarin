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
using XpertMobileApp.Api.ViewModels;
using XpertMobileApp.DAL;
using XpertMobileApp.Services;

namespace XpertMobileApp.ViewModels
{

    public class AchatsOHViewModel : CrudBaseViewModel2<ACH_DOCUMENT, View_ACH_DOCUMENT>
    {

        public ObservableCollection<View_ACH_DOCUMENT> SelectedDocs { get; set; }

        public string TypeDoc { get; set; } = "LF";
        public string MotifDoc { get; set; } = AchRecMotifs.PesageForProduction;

        private bool selectionMode;
        public bool SelectionMode
        {
            get { return selectionMode; }
            set { SetProperty(ref selectionMode, value); }
        }

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
                if (App.HasAdmin) return true;

                bool result = false;
                if (App.permissions != null)
                {
                    var obj = App.permissions.Where(x => x.CodeObjet == "ACH_UPDATE_ENTETE").FirstOrDefault();
                    result = obj != null && obj.AcUpdate > 0;
                }
                return result;
            }
        }

        public View_TRS_TIERS SelectedTiers { get; set; }

        public EncaissDisplayType EncaissDisplayType { get; set; }
        public DateTime StartDate { get; set; } = DateTime.Now;
        public DateTime EndDate { get; set; } = DateTime.Now;

        public ObservableCollection<BSE_DOCUMENT_STATUS> Status { get; set; }
        public BSE_DOCUMENT_STATUS SelectedStatus { get; set; }
        

        public ObservableCollection<View_BSE_COMPTE> Client { get; set; }
        public View_BSE_COMPTE SelectedClient { get; set; }
        public Command LoadClientsCommand { get; set; }

        public ObservableCollection<View_BSE_COMPTE> User { get; set; }
        public View_BSE_COMPTE SelectedUser { get; set; }
        public Command LoadUsersCommand { get; set; }

        public AchatsOHViewModel(string typeDoc, string motifDoc)
        {
            Title = AppResources.pn_AchatsProduction;
            MotifDoc = motifDoc;
            TypeDoc = typeDoc;

            Status = new ObservableCollection<BSE_DOCUMENT_STATUS>();
            SelectedDocs = new ObservableCollection<View_ACH_DOCUMENT>();

            Status.Add(new BSE_DOCUMENT_STATUS
            {
                CODE_STATUS = "",
                NAME = "",
            });

            Status.Add(new BSE_DOCUMENT_STATUS
            {
                CODE_STATUS = "16",
                NAME = "En attente",
            });

            Status.Add(new BSE_DOCUMENT_STATUS
            {
                CODE_STATUS = "17",
                NAME = "En cours",
            });

            Status.Add(new BSE_DOCUMENT_STATUS
            {
                CODE_STATUS = "18",
                NAME = "Terminé",
            });

            if (App.HasAdmin)
            {
                Status.Add(new BSE_DOCUMENT_STATUS
                {
                    CODE_STATUS = "19",
                    NAME = "Clôturée",
                });
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

            if (!string.IsNullOrEmpty(SelectedStatus?.CODE_STATUS))
                this.AddCondition<View_ACH_DOCUMENT, string>(e => e.STATUS_DOC, SelectedStatus?.CODE_STATUS);

            this.AddCondition<View_ACH_DOCUMENT, string>(e => e.CODE_MOTIF, MotifDoc);

            this.AddCondition<View_ACH_DOCUMENT, string>(e => e.TYPE_DOC, TypeDoc);

            this.AddOrderBy<View_ACH_DOCUMENT, DateTime?>(e => e.CREATED_ON, Sort.DESC);

            return qb.QueryInfos;
        }

        protected override void OnAfterLoadItems(IEnumerable<View_ACH_DOCUMENT> list)
        {
            base.OnAfterLoadItems(list);
            
            int i = 0;
            foreach (var item in list)
            {
                i += 1;
                (item as BASE_CLASS).Index = i;

                if(SelectedDocs.Count > 0 && SelectedDocs.Where(x => x.CODE_DOC == item.CODE_DOC).Count() > 0)
                {
                    item.IsSelected = true;
                }

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
