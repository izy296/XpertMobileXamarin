using Acr.UserDialogs;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
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
    public static class VentesTypes
    {
        public static string Vente { get { return ""; } }
        public static string VentePSYCO { get { return "PSYCO"; } }

        public static string VenteComptoir { get { return "VC"; } }
        public static string Livraison { get { return "BL"; } }
    }

    public class VentesViewModel : CrudBaseViewModel2<VTE_VENTE, View_VTE_VENTE>
    {
        public string TypeVente = VentesTypes.Vente;

        public bool IsVtesList { get; set; } = false;

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
        public BSE_DOCUMENTS_TYPE SelectedType { get; set; }

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
                /*
                else if (TypeVente == VentesTypes.VenteComptoir)
                {
                    return "XCOM_VTE_COMPTOIR";
                }
                else if (TypeVente == VentesTypes.Livraison)
                {
                    return "XCOM_VTE_LIVRAISON";
                }*/
                else
                {
                    return "VTE_VENTE";
                }
            }
        }        

        public VentesViewModel( string typeVente)
        {
            TypeVente = typeVente;

            Types = new ObservableCollection<BSE_DOCUMENTS_TYPE>();
            if (typeVente == VentesTypes.Vente)
            {
                Title = AppResources.pn_Ventes;
                IsVtesList = true;
            }
            else if(typeVente == VentesTypes.VentePSYCO)
            {
                Title = AppResources.pn_VtePsychotrop;
            }
            else if (typeVente == VentesTypes.Livraison)
            {
                Title = AppResources.pn_Livraison;
            }
            else if (typeVente == VentesTypes.VenteComptoir)
            {
                Title = AppResources.pn_VteComptoir;
            }
            base.InitConstructor();

            if (typeVente == VentesTypes.Vente)
            { 
                LoadExtrasDataCommand = new Command(async () => await ExecuteLoadExtrasDataCommand());
            }
        }

        protected override QueryInfos GetFilterParams()
        {
            base.GetFilterParams();

            if (TypeVente == VentesTypes.VentePSYCO)
            {
                this.AddCondition<View_VTE_PSYCHOTROP, DateTime?>(e => e.DATE_VENTE, Operator.BETWEEN_DATE, StartDate, EndDate);
                if (!string.IsNullOrEmpty(SelectedTiers?.CODE_TIERS))
                    this.AddCondition<View_VTE_PSYCHOTROP, string>(e => e.CODE_TIERS, SelectedTiers?.CODE_TIERS);
                if (!string.IsNullOrEmpty(SelectedType?.CODE_TYPE))
                    this.AddCondition<View_VTE_PSYCHOTROP, string>(e => e.TYPE_DOC, SelectedType?.CODE_TYPE);
                this.AddOrderBy<View_VTE_PSYCHOTROP, DateTime?>(e => e.CREATED_ON, Sort.DESC);
            } 
            else
            {
                this.AddCondition<View_VTE_VENTE, DateTime?>(e => e.DATE_VENTE, Operator.BETWEEN_DATE, StartDate, EndDate);
                if (!string.IsNullOrEmpty(SelectedTiers?.CODE_TIERS))
                    this.AddCondition<View_VTE_VENTE, string>(e => e.CODE_TIERS, SelectedTiers?.CODE_TIERS);

                if (!string.IsNullOrEmpty(TypeVente)) 
                {
                    this.AddCondition<View_VTE_VENTE, string>(e => e.TYPE_VENTE, TypeVente);

                    if(Constants.AppName == Apps.XCOM_Livraison) 
                    {
                        this.AddCondition<View_VTE_VENTE, string>(e => e.CREATED_BY, App.User.UserName);
                    }
                }

                if (!string.IsNullOrEmpty(SelectedType?.CODE_TYPE))
                    this.AddCondition<View_VTE_VENTE, string>(e => e.TYPE_DOC, SelectedType?.CODE_TYPE);
                this.AddOrderBy<View_VTE_VENTE, DateTime?>(e => e.CREATED_ON, Sort.DESC);
            }
            return qb.QueryInfos;
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

                await ExecuteLoadTypesCommand();

                TotalTurnover = stat.MONTANT_VENTE;
                TotalMargin = stat.MONTANT_MARGE;

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
                empty.DESIGNATION_TYPE = AppResources.txt_All;
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
    }

}
