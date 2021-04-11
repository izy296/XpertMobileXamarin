using Acr.UserDialogs;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Threading.Tasks;
using Xamarin.Forms;
using Xpert.Common.DAO;
using Xpert.Common.WSClient.Helpers;
using XpertMobileApp.Api;
using XpertMobileApp.Api.ViewModels;
using XpertMobileApp.DAL;

namespace XpertMobileApp.ViewModels
{
    public class ProductionsViewModel : CrudBaseViewModel2<PRD_AGRICULTURE, View_PRD_AGRICULTURE>
    {

        public ObservableCollection<View_PRD_AGRICULTURE> SelectedDocs { get; set; }

        public string TypeDoc { get; set; } = "LF";

        public string MotifDoc { get; set; } = AchRecMotifs.PesageForProduction;

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

        private bool selectionMode;
        public bool SelectionMode
        {
            get { return selectionMode; }
            set { SetProperty(ref selectionMode, value); }
        }

        public ProductionsViewModel(string typeDoc, string motifDoc)
        {
            Title = AppResources.pn_OrdresProduction; ;
            MotifDoc = motifDoc;
            TypeDoc = typeDoc;

            Status = new ObservableCollection<BSE_DOCUMENT_STATUS>();
            SelectedDocs = new ObservableCollection<View_PRD_AGRICULTURE>();

            Status.Add(new BSE_DOCUMENT_STATUS
            {
                CODE_STATUS = "",
                NAME = "",
            });

            Status.Add(new BSE_DOCUMENT_STATUS
            {
                CODE_STATUS = "30",
                NAME = "En attente",
            });

            Status.Add(new BSE_DOCUMENT_STATUS
            {
                CODE_STATUS = "31",
                NAME = "En cours",
            });

            Status.Add(new BSE_DOCUMENT_STATUS
            {
                CODE_STATUS = "32",
                NAME = "Terminé",
            });
        }

        protected override QueryInfos GetFilterParams()
        {
            base.GetFilterParams();

            this.AddCondition<View_PRD_AGRICULTURE, DateTime?>(e => e.DATE_DOC, Operator.BETWEEN_DATE, StartDate, EndDate);

            // this.AddCondition<View_PRD_AGRICULTURE, string>(e => e., "ES10");
            // this.AddCondition<View_PRD_AGRICULTURE, string>(e => e.CODE_MOTIF, MotifDoc);

            if (!string.IsNullOrEmpty(SelectedTiers?.CODE_TIERS))
                this.AddCondition<View_PRD_AGRICULTURE, string>(e => e.CODE_TIERS, SelectedTiers?.CODE_TIERS);

            if (!string.IsNullOrEmpty(SelectedStatus?.CODE_STATUS))
                this.AddCondition<View_PRD_AGRICULTURE, string>(e => e.STATUS_DOC, SelectedStatus?.CODE_STATUS);

            this.AddOrderBy<View_PRD_AGRICULTURE, DateTime?>(e => e.CREATED_ON);

            return qb.QueryInfos;
        }

        protected override void OnAfterLoadItems(IEnumerable<View_PRD_AGRICULTURE> list)
        {
            base.OnAfterLoadItems(list);

            int i = 0;
            foreach (var item in list)
            {
                i += 1;
                (item as BASE_CLASS).Index = i;
            }

            // GetItemsSum();
        }

        #region Filter

        public DateTime StartDate { get; set; } = DateTime.Now;
        public DateTime EndDate { get; set; } = DateTime.Now;

        public View_TRS_TIERS SelectedTiers { get; set; }

        public ObservableCollection<BSE_DOCUMENT_STATUS> Status { get; set; }
        public BSE_DOCUMENT_STATUS SelectedStatus { get; set; }


        public ObservableCollection<View_BSE_COMPTE> Client { get; set; }
        public View_BSE_COMPTE SelectedClient { get; set; }
        public Command LoadClientsCommand { get; set; }

        public ObservableCollection<View_BSE_COMPTE> User { get; set; }
        public View_BSE_COMPTE SelectedUser { get; set; }
        public Command LoadUsersCommand { get; set; }

        #endregion
    }

}
