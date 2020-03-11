using Acr.UserDialogs;

using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Threading.Tasks;
using Xamarin.Forms;
using Xamarin.Forms.Extended;
using Xpert.Common.WSClient.Helpers;
using XpertMobileApp.Api.ViewModels;
using XpertMobileApp.DAL;
using XpertMobileApp.Helpers;
using XpertMobileApp.Services;

namespace XpertMobileApp.ViewModels
{
    public class BordereauFactsViewModel : CrudBaseViewModel<FACTURE_CHIFA, View_CFA_MOBILE_FACTURE>
    {
        public string TypeVente = VentesTypes.Vente;

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

        public View_TRS_TIERS SelectedTiers { get; set; }

        public EncaissDisplayType EncaissDisplayType { get; set; }
        public DateTime StartDate { get; set; } = DateTime.Now;
        public DateTime EndDate { get; set; } = DateTime.Now;

        public ObservableCollection<BSE_DOCUMENTS_TYPE> Types { get; set; }
        public ObservableCollection<CFA_ETAT> FactStatus { get; set; }
        CFA_ETAT selectedSTATUS;
        public CFA_ETAT SelectedSTATUS
        {
            get { return selectedSTATUS; }
            set { SetProperty(ref selectedSTATUS, value); }
        }

        public ObservableCollection<View_BSE_COMPTE> User { get; set; }
        public View_BSE_COMPTE SelectedUser { get; set; }
        public Command LoadUsersCommand { get; set; }

        public View_CFA_BORDEREAU CURRENT_BORDEREAU { get; set; }

        public BordereauFactsViewModel(View_CFA_BORDEREAU item)
        {
            CURRENT_BORDEREAU = item;


            Types = new ObservableCollection<BSE_DOCUMENTS_TYPE>();
            FactStatus = new ObservableCollection<CFA_ETAT>();
            Title = "Num " + item.NUM_BORDEREAU;

            base.InitConstructor();

            this.LoadSaumuaries(item);

            LoadExtrasDataCommand = new Command(async () => await ExecuteLoadExtrasDataCommand());
        }

        private void LoadSaumuaries(View_CFA_BORDEREAU item)
        {
            Summaries.Add(new Models.SAMMUARY()
            {
                key = TranslateExtension.GetTranslation("NBR_FACTS"),
                Value = item.NBR_FACTS
            });

            Summaries.Add(new Models.SAMMUARY()
            {
                key = TranslateExtension.GetTranslation("MONT_BORD"),
                Value = item.TOTAL_FACTURES
            });

            Summaries.Add(new Models.SAMMUARY()
            {
                key = TranslateExtension.GetTranslation("TOTAL_AJUSTEMENT"),
                Value = item.TOTAL_AJUSTEMENT
            });

            Summaries.Add(new Models.SAMMUARY()
            {
                key = TranslateExtension.GetTranslation("TOTAL_EXCLUDED"),
                Value = item.TOTAL_EXCLUDED
            });

            Summaries.Add(new Models.SAMMUARY()
            {
                key = TranslateExtension.GetTranslation("TOTAL_TO_PAY"),
                Value = item.TOTAL_TO_PAY
            });

            Summaries.Add(new Models.SAMMUARY()
            {
                key = TranslateExtension.GetTranslation("TOTAL_OFFICINE_PAYE"),
                Value = item.TOTAL_OFFICINE_PAYE
            });
        }

        protected override Dictionary<string, string> GetFilterParams()
        {
            Dictionary<string, string> result = base.GetFilterParams();

            // result.Add("type", "all");
            // result.Add("idCaisse", "all");
            
            // result.Add("startDate", WSApi2.GetStartDateQuery(StartDate));
            // result.Add("endDate", WSApi2.GetEndDateQuery(EndDate));

            if (!string.IsNullOrEmpty(SelectedTiers?.CODE_TIERS))
                result.Add("codeClient", SelectedTiers?.CODE_TIERS);

            if (!string.IsNullOrEmpty(CURRENT_BORDEREAU?.NUM_BORDEREAU))
                result.Add("numBordereau", CURRENT_BORDEREAU?.NUM_BORDEREAU);            

            if (!string.IsNullOrEmpty(SelectedSTATUS?.CODE_ETAT))
                result.Add("etat", SelectedSTATUS?.CODE_ETAT);

            return result;
        }

        protected override void OnAfterLoadItems(IEnumerable<View_CFA_MOBILE_FACTURE> list)
        {
            base.OnAfterLoadItems(list);

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

        async Task ExecuteLoadExtrasDataCommand()
        {

            if (IsLoadExtrasBusy)
                return;

            try
            {
                IsLoadExtrasBusy = true;

                // Infos de la journée
                DateTime endDate = DateTime.Now;
                DateTime startDate = DateTime.Now;
                // STAT_VTE_BY_USER stat = await WebServiceClient.GetTotalMargeParVendeur(startDate, endDate);

                await ExecuteLoadTypesProduitCommand();

                // TotalTurnover = stat.MONTANT_VENTE;
                // TotalMargin = stat.MONTANT_MARGE;

                // await ExecuteLoadFamillesCommand();

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

        async Task ExecuteLoadTypesCommand()
        {
            try
            {
              //  UserDialogs.Instance.ShowLoading(AppResources.txt_Loading);

                Types.Clear();
                var itemsC = await WebServiceClient.GetVenteTypes();

                foreach (var itemC in itemsC)
                {
                    Types.Add(itemC);
                }

                BSE_DOCUMENTS_TYPE empty = new BSE_DOCUMENTS_TYPE();
                empty.CODE_TYPE = "";
                Types.Insert(0, empty);

              //  UserDialogs.Instance.HideLoading();
            }
            catch (Exception ex)
            {
              //  UserDialogs.Instance.HideLoading();
                await UserDialogs.Instance.AlertAsync(WSApi2.GetExceptionMessage(ex), AppResources.alrt_msg_Alert,
                    AppResources.alrt_msg_Ok);
            }
            finally
            {
                IsBusy = false;
            }
        }

        async Task ExecuteLoadTypesProduitCommand()
        {

            try
            {                
                FactStatus.Clear();
                var itemsC = await WebServiceClient.get_CFA_Fact_STATUS();
                
                foreach (var itemC in itemsC)
                {
                    FactStatus.Add(itemC);
                }

                CFA_ETAT allElem = new CFA_ETAT();
                allElem.CODE_ETAT = "";
                allElem.DESIGN_ETAT = AppResources.txt_All;
                itemsC.Add(allElem);

            }
            catch (Exception ex)
            {
                await UserDialogs.Instance.AlertAsync(WSApi2.GetExceptionMessage(ex), AppResources.alrt_msg_Alert,
                    AppResources.alrt_msg_Ok);
            }

        }
    }

}
