using Acr.UserDialogs;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Threading.Tasks;
using Xamarin.Forms;
using Xamarin.Forms.Extended;
using Xpert.Common.WSClient.Helpers;
using XpertMobileApp.Api.ViewModels;
using XpertMobileApp.DAL;
using XpertMobileApp.Services;

namespace XpertMobileApp.ViewModels
{

    public class ProductionsViewModel : CrudBaseViewModel<PRD_AGRICULTURE, View_PRD_AGRICULTURE>
    {

        public ObservableCollection<View_PRD_AGRICULTURE> SelectedDocs { get; set; }

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

        protected override Dictionary<string, string> GetFilterParams()
        {
            Dictionary<string, string> result = base.GetFilterParams();

            result.Add("typeDoc", TypeDoc);
            result.Add("motifDoc", MotifDoc);
            // result.Add("idCaisse", "all");
            result.Add("startDate", WSApi2.GetStartDateQuery(StartDate));
            result.Add("endDate", WSApi2.GetEndDateQuery(EndDate));

            if (!string.IsNullOrEmpty(SelectedTiers?.CODE_TIERS))
                result.Add("codeClient", SelectedTiers?.CODE_TIERS);

            if (!string.IsNullOrEmpty(SelectedStatus?.CODE_STATUS))
                result.Add("statusDoc", SelectedStatus?.CODE_STATUS);

            return result;
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

            GetItemsSum();
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
