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
using XpertMobileApp.Services;

namespace XpertMobileApp.ViewModels
{
    public static class VentesTypes
    {
        public static string Vente { get { return "VTE"; } }
        public static string VentePSYCO { get { return "PSYCO"; } }
        public static string Livraison { get { return "Liv"; } }
    }

    public class VentesViewModel : CrudBaseViewModel<VTE_VENTE, View_VTE_VENTE>
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

        public ObservableCollection<View_BSE_COMPTE> Comptes { get; set; }
        public View_BSE_COMPTE SelectedCompte { get; set; }
        public Command LoadComptesCommand { get; set; }

        public ObservableCollection<View_BSE_COMPTE> Client { get; set; }
        public View_BSE_COMPTE SelectedClient { get; set; }
        public Command LoadClientsCommand { get; set; }

        public ObservableCollection<View_BSE_COMPTE> User { get; set; }
        public View_BSE_COMPTE SelectedUser { get; set; }
        public Command LoadUsersCommand { get; set; }

        protected override string ContoleurName
        {
            get
            {
                if (TypeVente == VentesTypes.VentePSYCO)
                {
                    return "VTE_PSYCHOTROP";
                }
                else
                {
                    return "VTE_VENTE";
                }
            }
        }        

        public VentesViewModel( string typeVente)
        {
            TypeVente = typeVente;
            if (typeVente == VentesTypes.Vente)
            {
                Title = AppResources.pn_Ventes;
            }
            else if(typeVente == VentesTypes.VentePSYCO)
            {
                Title = AppResources.pn_VtePsychotrop;
            }

            base.InitConstructor();

            if (typeVente == VentesTypes.Vente)
            { 
                LoadExtrasDataCommand = new Command(async () => await ExecuteLoadExtrasDataCommand());
            }
        }

        protected override Dictionary<string, string> GetFilterParams()
        {
            Dictionary<string, string> result = base.GetFilterParams();

            // result.Add("type", "all");
            // result.Add("idCaisse", "all");
            result.Add("startDate", WSApi2.GetStartDateQuery(StartDate));
            result.Add("endDate", WSApi2.GetEndDateQuery(EndDate));

            if (!string.IsNullOrEmpty(SelectedTiers?.CODE_TIERS))
                result.Add("codeClient", SelectedTiers?.CODE_TIERS);

            if (!string.IsNullOrEmpty(SelectedCompte?.CODE_COMPTE))
                result.Add("codeUser", SelectedCompte?.CODE_COMPTE);

            return result;
        }

        protected override void OnAfterLoadItems(IEnumerable<View_VTE_VENTE> list)
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
                STAT_VTE_BY_USER stat = await WebServiceClient.GetTotalMargeParVendeur(startDate, endDate);
                TotalTurnover = stat.MONTANT_VENTE;
                TotalMargin = stat.MONTANT_MARGE;

                // await ExecuteLoadFamillesCommand();
                // await ExecuteLoadTypesCommand();
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
