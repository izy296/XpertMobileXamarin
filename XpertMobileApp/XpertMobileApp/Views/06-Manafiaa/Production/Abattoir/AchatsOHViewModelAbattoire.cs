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
using XpertMobileApp.Api.Services;
using XpertMobileApp.Api.ViewModels;
using XpertMobileApp.DAL;

namespace XpertMobileApp.ViewModels
{
    public class AchatsOHViewModelAbattoire : CrudBaseViewModel2<ACH_DOCUMENT, View_ACH_DOCUMENT>
    {
        public ObservableCollection<View_ACH_DOCUMENT> SelectedDocs { get; set; }

        public string TypeDoc { get; set; } = "LF";
        public string MotifDoc { get; set; } = AchRecMotifs.PesageForProduction;

        public string SelectedIdentifiant { get; set; }

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
                if (AppManager.HasAdmin && Constants.AppName != Apps.XCOM_Abattoir) return true;

                bool result = false;
                if (AppManager.permissions != null)
                {
                    var obj = AppManager.permissions.Where(x => x.CodeObjet == "ACH_UPDATE_ENTETE").FirstOrDefault();
                    result = obj != null && obj.AcUpdate > 0;
                }
                return result;
            }
        }

        public bool hasInsertHeader
        {
            get
            {
                if (AppManager.HasAdmin && Constants.AppName != Apps.XCOM_Abattoir) return true;

                bool result = false;
                if (AppManager.permissions != null)
                {
                    var obj = AppManager.permissions.Where(x => x.CodeObjet == "ACH_UPDATE_ENTETE").FirstOrDefault();
                    result = obj != null && obj.AcInsert > 0;
                }
                return result;
            }
        }



        public View_TRS_TIERS SelectedTiers { get; set; }

        public EncaissDisplayType EncaissDisplayType { get; set; }
        public DateTime StartDate { get; set; } = DateTime.Now.Subtract(TimeSpan.FromDays(1));
        public DateTime EndDate { get; set; } = DateTime.Now;

        public ObservableCollection<BSE_DOCUMENT_STATUS> Status { get; set; }
        public BSE_DOCUMENT_STATUS SelectedStatus { get; set; }

        public bool hasDocumentPermission
        {
            get
            {
                bool result = false;
                if (AppManager.permissions != null)
                {
                    var obj = AppManager.permissions.Where(x => x.CodeObjet == "ACH_RECEPTION_LISTE_AGRICULTEUR").FirstOrDefault();
                    result = obj != null && obj.AcSelect > 0;
                }
                return result;
            }
        }

        public string[] getPermitedDocument
        {
            get
            {
                string[] result = { };
                if (AppManager.permissions != null)
                {
                    var obj = AppManager.permissions.Where(x => x.CodeObjet == "ACH_RECEPTION_LISTE_AGRICULTEUR").FirstOrDefault();
                    if (obj != null)
                        obj.AcCustomData = obj.AcCustomData.Replace("'", "");
                    result = obj != null && !string.IsNullOrEmpty(obj.AcCustomData) ? obj.AcCustomData.Split(',') : result;
                }
                return result;
            }
        }

        public bool hasExaminationVeterinaire
        {
            get
            {
                bool result = false;
                if (AppManager.permissions != null)
                {
                    var obj = AppManager.permissions.Where(x => x.CodeObjet == "ACH_VALIDATE_VETERINAIRE").FirstOrDefault();
                    result = obj != null && obj.AcSelect > 0;
                }
                return result;
            }
        }

        public ObservableCollection<View_BSE_COMPTE> Client { get; set; }
        public View_BSE_COMPTE SelectedClient { get; set; }
        public Command LoadClientsCommand { get; set; }

        public ObservableCollection<View_BSE_COMPTE> User { get; set; }
        public View_BSE_COMPTE SelectedUser { get; set; }
        public Command LoadUsersCommand { get; set; }

        public AchatsOHViewModelAbattoire(string typeDoc, string motifDoc)
        {
            Title = AppResources.pn_AchatsProduction;
            MotifDoc = motifDoc;
            TypeDoc = typeDoc;

            Status = new ObservableCollection<BSE_DOCUMENT_STATUS>();
            SelectedDocs = new ObservableCollection<View_ACH_DOCUMENT>();

            Status.Add(new BSE_DOCUMENT_STATUS()
            {
                CODE_STATUS = "",
                NAME = "",
            });



            Type type = typeof(DocStatus);
            var res = type.GetProperties(System.Reflection.BindingFlags.Static | System.Reflection.BindingFlags.Public);
            foreach (var status in res)
            {
                Status.Add(new BSE_DOCUMENT_STATUS()
                {
                    CODE_STATUS = status.GetValue(null).ToString(),
                    NAME = status.Name,
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
            if (hasDocumentPermission && Constants.AppName == Apps.XCOM_Abattoir)
            {
                var statusDoc = getPermitedDocument;
                if (!XpertHelper.IsNullOrEmpty(statusDoc))
                {
                    this.AddCondition<View_ACH_DOCUMENT, string>(e => e.STATUS_DOC, Operator.IN, statusDoc);
                }
                else this.AddCondition<View_ACH_DOCUMENT, string>(e => e.STATUS_DOC, "BLOCK");
            }

            if (!string.IsNullOrEmpty(SelectedStatus?.CODE_STATUS))
                this.AddCondition<View_ACH_DOCUMENT, string>(e => e.STATUS_DOC, SelectedStatus?.CODE_STATUS);

            if (!string.IsNullOrEmpty(SelectedIdentifiant))
                this.AddCondition<View_ACH_DOCUMENT, string>(e => e.IMMATRICULATION, Operator.LIKE_ANY, SelectedIdentifiant);

            var res = getPermitedDocument.Where(e => e == DocStatus.Accepter).Any();

            if (!hasExaminationVeterinaire && res == true)
                this.AddCondition<View_ACH_DOCUMENT, int>(e => e.VALIDATE, 1);

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
                item.DESIGNATION_STATUS = item.DESIGNATION_STATUS.Replace("Achat ", "");
                if (SelectedDocs.Count > 0 && SelectedDocs.Where(x => x.CODE_DOC == item.CODE_DOC).Count() > 0)
                {
                    item.IsSelected = true;
                }

            }

            MessagingCenter.Send(this, "ChangingGroupingList", list);
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
