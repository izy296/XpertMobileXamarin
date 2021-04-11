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

namespace XpertMobileApp.ViewModels
{

    public class AchatsViewModel : CrudBaseViewModel2<ACH_DOCUMENT, View_ACH_DOCUMENT>
    {
        // Le droit d'éditer l'entête du document
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

        // Type de document d'achat d'olive 
        public string TypeDoc { get; set; } = "LF";

        protected override string ContoleurName
        {
            get
            {
                return "ACH_ACHATS";
            }
        }

        public AchatsViewModel(string typeDoc)
        {
            Title = AppResources.pn_Achats;
            TypeDoc = typeDoc;

            Status = new ObservableCollection<BSE_DOCUMENT_STATUS>();

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

            Status.Add(new BSE_DOCUMENT_STATUS
            {
                CODE_STATUS = "19",
                NAME = "En production",
            });

            if (AppManager.HasAdmin)
            {
                Status.Add(new BSE_DOCUMENT_STATUS
                {
                    CODE_STATUS = "22",
                    NAME = "Livré",
                });

                Status.Add(new BSE_DOCUMENT_STATUS
                {
                    CODE_STATUS = "19",
                    NAME = "Clôturée",
                });
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

            this.AddCondition<View_ACH_DOCUMENT, string>(e => e.CODE_MOTIF, "ES10");

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
            }
        }

        public override void ClearFilters()
        {
            base.ClearFilters();
            SelectedTiers = null;
            SelectedStatus = null;
            StartDate = DateTime.Now;
            EndDate = DateTime.Now;
            SelectedClient = null;
            SelectedUser = null;
        }

        #region Filter

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

        #endregion

    }

}
