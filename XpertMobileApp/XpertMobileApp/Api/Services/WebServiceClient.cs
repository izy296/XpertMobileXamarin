using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Text;
using System.Threading.Tasks;
using Xpert.Common.DAO;
using Xpert.Common.WSClient.Helpers;
using Xpert.Common.WSClient.Model;
using XpertMobileApp.Api;
using XpertMobileApp.Api.Services;
using XpertMobileApp.DAL;
using XpertMobileApp.Helpers;
using XpertMobileApp.Models;
using XpertWebApi.Models;

namespace XpertMobileApp.Services
{
    public class WebServiceClient
    {
        #region LOGIN 
        public static string Token
        {
            get { return App.User.Token.access_token; }
        }

        public static async Task<List<T>> RetrievAauthorizedData<T>(string url)
        {
            return await WSApi2.RetrievAauthorizedData<T>(url, Token);
        }

        public static async Task<T> RetrievValAauthorizedData<T>(string url)
        {
            return await WSApi2.RetrievAauthorizedValue<T>(url, Token);
        }

        public static async Task<Token> Login(string baseUrl, string username, string password)
        {
            try
            {
                string url = baseUrl + "Token";
                return await WSApi2.Log_in(url, username, password);
            }
            catch (XpertWebException e)
            {
                throw new XpertWebException(e.Message, e.Code);
            }
        }

        #endregion

        public static async Task<List<TRS_JOURNEES>> GetSessionInfos()
        {
            string url = WSApi2.CreateLink(App.RestServiceUrl, ServiceUrlDico.SESSION_INFO_URL);

            return await RetrievAauthorizedData<TRS_JOURNEES>(url);
        }

        #region statistiques

        internal static async Task<List<STAT_VTE_BY_USER>> GetMargeParVendeur(DateTime? startDate, DateTime? endDate)
        {
            string url = WSApi2.CreateLink(App.RestServiceUrl, ServiceUrlDico.DASHBOARD_URL, ServiceUrlDico.MARGE_PAR_VENDEUR_URL);
            url += "?startDate=" + WSApi2.GetStartDateQuery(startDate);
            url += "&endDate=" + WSApi2.GetEndDateQuery(endDate);

            return await RetrievAauthorizedData<STAT_VTE_BY_USER>(url);
        }

        internal static async Task<List<STAT_ACHAT_AGRO>> GetAchat(DateTime? startDate, DateTime? endDate)
        {
            string url = WSApi2.CreateLink(App.RestServiceUrl, ServiceUrlDico.DASHBOARD_URL, ServiceUrlDico.ACHAT_AGRO_INFOS_URL);
            url += "?startDate=" + WSApi2.GetStartDateQuery(startDate);
            url += "&endDate=" + WSApi2.GetEndDateQuery(endDate);

            return await RetrievAauthorizedData<STAT_ACHAT_AGRO>(url);
        }
        internal static async Task<STAT_VTE_BY_USER> GetTotalMargeParVendeur(DateTime? startDate, DateTime? endDate)
        {
            string url = WSApi2.CreateLink(App.RestServiceUrl, ServiceUrlDico.DASHBOARD_URL, ServiceUrlDico.TOTAL_MARGE_PAR_VENDEUR_URL);
            url += "?startDate=" + WSApi2.GetStartDateQuery(startDate);
            url += "&endDate=" + WSApi2.GetEndDateQuery(endDate);

            return await RetrievValAauthorizedData<STAT_VTE_BY_USER>(url);
        }

        internal static async Task<List<View_TRS_ENCAISS>> GetStatisticEncaiss(DateTime? startDate, DateTime? endDate)
        {
            string url = WSApi2.CreateLink(App.RestServiceUrl, ServiceUrlDico.ENCAISSEMENT_URL, ServiceUrlDico.STATISTIC_URL);
            url += "?startDate=" + WSApi2.GetStartDateQuery(startDate);
            url += "&endDate=" + WSApi2.GetEndDateQuery(endDate);

            return await RetrievAauthorizedData<View_TRS_ENCAISS>(url);
        }

        #endregion

        #region Tiers

        internal static async Task<List<View_BSE_TIERS_FAMILLE>> getTiersFamilles()
        {
            string url = WSApi2.CreateLink(App.RestServiceUrl, ServiceUrlDico.TIERS_URL, ServiceUrlDico.TIERS_FAMILLES_URL);

            return await RetrievAauthorizedData<View_BSE_TIERS_FAMILLE>(url);
        }

        internal static async Task<List<BSE_TABLE_TYPE>> getTiersTypes()
        {
            string url = WSApi2.CreateLink(App.RestServiceUrl, ServiceUrlDico.TIERS_URL, ServiceUrlDico.TIERS_TYPES_URL);

            return await RetrievAauthorizedData<BSE_TABLE_TYPE>(url);
        }

        internal static async Task<List<View_TRS_TIERS>> GetClients(string codeTournee)
        {
            string url = WSApi2.CreateLink(App.RestServiceUrl, ServiceUrlDico.TIERS_URL, ServiceUrlDico.TIERS_GET_LIST_URL);
            url += "?codeTournee=" + codeTournee;
            return await RetrievAauthorizedData<View_TRS_TIERS>(url);
        }

        #endregion

        #region Manquants

        internal static async Task<List<BSE_DOCUMENT_STATUS>> getManquantsTypes()
        {
            string url = WSApi2.CreateLink(App.RestServiceUrl, ServiceUrlDico.MANQUANTS_URL, ServiceUrlDico.MANQUANTS_TYPES_URL);

            return await RetrievAauthorizedData<BSE_DOCUMENT_STATUS>(url);
        }

        internal static async Task<List<CFA_CENTRES>> getBordereauxCentresTypes()
        {
            string url = WSApi2.CreateLink(App.RestServiceUrl, ServiceUrlDico.BORDEREAUX_URL, ServiceUrlDico.BORDEREAUX_CENTRES_URL);

            return await RetrievAauthorizedData<CFA_CENTRES>(url);
        }

        internal static async Task<List<CFA_ETAT>> getBordereauxSTATUS()
        {
            string url = WSApi2.CreateLink(App.RestServiceUrl, ServiceUrlDico.BORDEREAUX_URL, ServiceUrlDico.BORDEREAUX_ETAT_URL);

            return await RetrievAauthorizedData<CFA_ETAT>(url);
        }

        internal static async Task<List<View_CFA_MOBILE_FACTURE>> GetCFAFactsByNumBordereaux(string numBorderaux, string center = "0", string codeTier = "", string search = "", int page = 1, int count = 10)
        {
            string url = WSApi2.CreateLink(App.RestServiceUrl, ServiceUrlDico.CFA_FACTURE_CHIFA_URL, ServiceUrlDico.CFA_FACTURE_BORDEREAUX_URL);
            url += "?numBordereau=" + numBorderaux;
            url += "&page=" + page;
            url += "&count=" + count;
            url += "&center=" + center;
            url += "&codeTier=" + codeTier;
            url += "&search=" + search;

            return await RetrievAauthorizedData<View_CFA_MOBILE_FACTURE>(url);
        } 
        
        internal static async Task<List<View_CFA_MOBILE_FACTURE>> AnalyseFactures(string numBorderaux)
        {
            string url = WSApi2.CreateLink(App.RestServiceUrl, ServiceUrlDico.CFA_MOBILE_FACTURE_URL, ServiceUrlDico.CFA_FACTURE_ANALYSE);
            url += "?numBordereau=" + numBorderaux;

            return await RetrievAauthorizedData<View_CFA_MOBILE_FACTURE>(url);
        }

        internal static async Task<List<View_CFA_MOBILE_DETAIL_FACTURE>> SelectBeneficiares(DateTime startDate, DateTime endDate, string search = "",int orderBy=0, int page = 1, int count = 10)
        {
            string url = WSApi2.CreateLink(App.RestServiceUrl, ServiceUrlDico.CFA_DETAIL_FACTURE_URL, ServiceUrlDico.CFA_MOBILE_FACTURE_BENEFICIARE_URL);
            url += "?page=" + page;
            url += "&count=" + count;
            url += "&SearchText=" + search;
            url += "&StartDate=" + WSApi2.GetStartDateQuery(startDate);
            url += "&EndDate=" + WSApi2.GetEndDateQuery(endDate);
            url += "&orderBy=" + orderBy.ToString();

            return await RetrievAauthorizedData<View_CFA_MOBILE_DETAIL_FACTURE>(url);
        } 
        
        internal static async Task<int> SelectBeneficiaresCount(DateTime startDate, DateTime endDate, string search = "", int orderBy = 0, int page = 1, int count = 10)
        {
            string url = WSApi2.CreateLink(App.RestServiceUrl, ServiceUrlDico.CFA_DETAIL_FACTURE_URL, ServiceUrlDico.CFA_MOBILE_FACTURE_BENEFICIARE_COUNT_URL);
            url += WSApi2.AddParam(url, "page", page.ToString());
            url += WSApi2.AddParam(url, "count", count.ToString());
            url += WSApi2.AddParam(url, "orderBy", orderBy.ToString());
            url += WSApi2.AddParam(url, "SearchText", search);
            url += WSApi2.AddParam(url, "EndDate", WSApi2.GetStartDateQuery(endDate));
            url += WSApi2.AddParam(url, "StartDate", WSApi2.GetStartDateQuery(startDate));

            return await WSApi2.PostAauthorizedValue<int, string>(url,"",Token);
        }

        internal static async Task<List<View_CFA_MOBILE_FACTURE>> GetFactChronic(string numAssure)
        {
            string url = WSApi2.CreateLink(App.RestServiceUrl, ServiceUrlDico.CFA_MOBILE_FACTURE_URL, ServiceUrlDico.CFA_MOBILE_FACTURE_CHRONIC_URL);
            url += "?numAssure=" + numAssure;

            return await RetrievAauthorizedData<View_CFA_MOBILE_FACTURE>(url);
        }

        internal static async Task<List<View_CFA_MOBILE_FACTURE>> GetCfa_Beneficaires_Summary(DateTime startDate, DateTime endDate)
        {
            string url = WSApi2.CreateLink(App.RestServiceUrl, ServiceUrlDico.CFA_MOBILE_FACTURE_URL, ServiceUrlDico.CFA_MOBILE_FACTURE_SUMMARY_URL);
            url += "?startDate=" + WSApi2.GetStartDateQuery(startDate);
            url += "&endDate=" + WSApi2.GetEndDateQuery(endDate);
            return await RetrievAauthorizedData<View_CFA_MOBILE_FACTURE>(url);
        }

        internal static async Task<List<View_CFA_BORDEREAUX_CHIFA>> GetCFAByNumBordereaux(string numBorderaux)
        {
            string url = WSApi2.CreateLink(App.RestServiceUrl, ServiceUrlDico.BORDEREAUX_CHIFA_URL, ServiceUrlDico.BORDEREAUX_CHIFA_BY_NUM_URL);
            url += "?numBordereau=" + numBorderaux;
            return await RetrievAauthorizedData<View_CFA_BORDEREAUX_CHIFA>(url);
        }

        internal static async Task<List<View_CFA_BORDEREAUX_CHIFA>> GetCFABordereaux(QueryInfos queryInfos)
        {
            string url = WSApi2.CreateLink(App.RestServiceUrl, ServiceUrlDico.BORDEREAUX_CHIFA_URL, ServiceUrlDico.BORDEREAUX_CHIFA_SELECT_URL);
            return await WSApi2.PostAauthorizedValue<List<View_CFA_BORDEREAUX_CHIFA>, QueryInfos>(url, queryInfos, Token);
            
        } 
        
        internal static async Task<int> GetCFABordereauxCount(QueryInfos queryInfos)
        {
            string url = WSApi2.CreateLink(App.RestServiceUrl, ServiceUrlDico.BORDEREAUX_CHIFA_URL, ServiceUrlDico.BORDEREAUX_CHIFA_SELECT_COUNT_URL);
            return await WSApi2.PostAauthorizedValue<int, QueryInfos>(url, queryInfos, Token);
            
        }

        internal static async Task<List<Get_Print_ds_ViewTrsEncaiss>> GetDataTecketCaisseEncaisse(string cODE_ENCAISS)
        {
            string url = WSApi2.CreateLink(App.RestServiceUrl, ServiceUrlDico.ENCAISSEMENT_URL, ServiceUrlDico.TIKET_CAISS_PRINT_ENCAISSE_URL);
            url += "?codeEncaiss=" + cODE_ENCAISS;
            return await RetrievAauthorizedData<Get_Print_ds_ViewTrsEncaiss>(url);
        }

        internal static async Task<List<CFA_ETAT>> get_CFA_Fact_STATUS()
        {
            string url = WSApi2.CreateLink(App.RestServiceUrl, ServiceUrlDico.CFA_FACTURE_CHIFA_URL, ServiceUrlDico.CFA_FACTURE_ETAT_URL);

            return await RetrievAauthorizedData<CFA_ETAT>(url);
        }
        internal static async Task<List<DateTime>> get_CFA_Fact_Dates()
        {
            string url = WSApi2.CreateLink(App.RestServiceUrl, ServiceUrlDico.CFA_FACTURE_CHIFA_URL, ServiceUrlDico.CFA_FACTURE_DATE);

            return await RetrievAauthorizedData<DateTime>(url);
        }

        internal static async Task<List<View_CFA_MOBILE_DETAIL_FACTURE>> GetDetailsFacture(string codeFacture)
        {
            string url = WSApi2.CreateLink(App.RestServiceUrl, ServiceUrlDico.CFA_FACTURE_CHIFA_URL, ServiceUrlDico.CFA_DETAIL_FACTURE);
            url += "?NumFacture=" + codeFacture;
            return await RetrievValAauthorizedData<List<View_CFA_MOBILE_DETAIL_FACTURE>>(url);
        }

        internal static async Task<List<View_CFA_MOBILE_FACTURE>> GetTotauxFactureCHIFA(DateTime date)
        {
            string url = WSApi2.CreateLink(App.RestServiceUrl, ServiceUrlDico.CFA_FACTURE_CHIFA_URL, ServiceUrlDico.CFA_TOTAUX_FACTURE);
            url += "?date=" + WSApi2.GetStartDateQuery(date);
            return await RetrievValAauthorizedData<List<View_CFA_MOBILE_FACTURE>>(url);
        }

        internal static async Task<int> GetCountLabo(QueryInfos queryInfos)
        {
            string url = WSApi2.CreateLink(App.RestServiceUrl, ServiceUrlDico.CFA_FACTURE_DETAIL_MOBILE_URL, ServiceUrlDico.CFA_LABO_COUNT);
            return  await WSApi2.PostAauthorizedValue<int, QueryInfos>(url, queryInfos, Token);
        }

        internal static async Task<List<View_CFA_MOBILE_DETAIL_FACTURE>> GetListFactureByDci(DateTime startDate, DateTime endDate, string displayType, QueryInfos filterParams,int page, int pageSize)
        {
            string url = WSApi2.CreateLink(App.RestServiceUrl, ServiceUrlDico.CFA_FACTURE_DETAIL_MOBILE_URL, ServiceUrlDico.CFA_LISTE_FACT_BY_DCI);
            url += WSApi2.AddParam(url, "displayType", displayType);
            url += WSApi2.AddParam(url, "endDate", WSApi2.GetStartDateQuery(endDate));
            url += WSApi2.AddParam(url, "startDate", WSApi2.GetStartDateQuery(startDate));
            url += WSApi2.AddParam(url, "page", page.ToString());
            url += WSApi2.AddParam(url, "pageSize", pageSize.ToString());
            return await WSApi2.PostAauthorizedValue<List<View_CFA_MOBILE_DETAIL_FACTURE>, QueryInfos>(url, filterParams, Token);
        }
        
        internal static async Task<List<View_CFA_MOBILE_DETAIL_FACTURE>> GetListFactDetailByDci(DateTime startDate, DateTime endDate, string codeDCI, string reference, string codeProduit)
        {
            string url = WSApi2.CreateLink(App.RestServiceUrl, ServiceUrlDico.CFA_FACTURE_DETAIL_MOBILE_URL, ServiceUrlDico.CFA_LISTE_CONSOMMATION_BY_DCI);
            
            url += "?startDate=" + WSApi2.GetStartDateQuery(startDate);
            url += "&endDate=" + WSApi2.GetEndDateQuery(endDate);
            url += "&codeDCI=" + codeDCI;
            url += "&reference=" + reference;
            url += "&codeProduit=" + codeProduit;

            return await RetrievValAauthorizedData<List<View_CFA_MOBILE_DETAIL_FACTURE>>(url);
        }

        internal static async Task<List<View_CFA_MOBILE_FACTURE>> GetFactureListByReference(string codeDCI, string reference, DateTime startDate, DateTime endDate)
        {
            string url = WSApi2.CreateLink(App.RestServiceUrl, ServiceUrlDico.CFA_FACTURE_DETAIL_MOBILE_URL, ServiceUrlDico.CFA_LISTE_Facture_BY_REF);
            url += "?startDate=" + WSApi2.GetStartDateQuery(startDate);
            url += "&endDate=" + WSApi2.GetEndDateQuery(endDate);
            url += "&codeDCI=" + codeDCI;
            url += "&reference=" + reference;
            return await RetrievValAauthorizedData<List<View_CFA_MOBILE_FACTURE>>(url);
        }

        internal static async Task<List<View_CFA_MOBILE_DETAIL_FACTURE>> GetBeneficiaireByDci(string codeDCI)
        {
            string url = WSApi2.CreateLink(App.RestServiceUrl, ServiceUrlDico.CFA_FACTURE_DETAIL_MOBILE_URL, ServiceUrlDico.CFA_LISTE_BeneficiaireByDci);
            url += "?codeDCI=" + codeDCI;
            return await RetrievValAauthorizedData<List<View_CFA_MOBILE_DETAIL_FACTURE>>(url);

        }
        internal static async Task<View_CFA_MOBILE_FACTURE> GetTodayCountFacture()
        {
            string url = WSApi2.CreateLink(App.RestServiceUrl, ServiceUrlDico.CFA_MOBILE_FACTURE_URL, ServiceUrlDico.CFA_FACTURE_COUNT_TODAY);
            return await RetrievValAauthorizedData<View_CFA_MOBILE_FACTURE>(url);
        }

        internal static async Task<List<Get_Print_VTE_TiketCaisse>> GetDataTecketCaisseVente(string res)
        {
            string url = WSApi2.CreateLink(App.RestServiceUrl, ServiceUrlDico.VENTES_URL, ServiceUrlDico.TIKET_CAISS_PRINT_VENTE_URL);
            url += "?codeVente=" + res;
            return await RetrievAauthorizedData<Get_Print_VTE_TiketCaisse>(url);
        }
        public static async Task<decimal> GetQteStockByProdeuct(string res)
        {
            string url = WSApi2.CreateLink(App.RestServiceUrl, ServiceUrlDico.MANQUANTS_URL, ServiceUrlDico.Get_Qte_By_Produit);
            url += "?codeProduit=" + res;
            return await RetrievValAauthorizedData<decimal>(url);

        }

        public static async Task<decimal> GetQteStockByReference(string res)
        {
            string url = WSApi2.CreateLink(App.RestServiceUrl, ServiceUrlDico.MANQUANTS_URL, ServiceUrlDico.Get_Qte_By_Reference);
            url += "?reference=" + res;
            return await RetrievValAauthorizedData<decimal>(url);
        }
        public static async Task<List<View_ACH_MANQUANTS>> FindCurrent_Non_CF_Manquants(string codeProduit)
        {
            string url = WSApi2.CreateLink(App.RestServiceUrl, ServiceUrlDico.MANQUANTS_URL, ServiceUrlDico.Find_Current_Non_CF_Manquants);
            url += WSApi2.AddParam(url, "codeProduit", codeProduit);
            return await WSApi2.PostAauthorizedValue<List<View_ACH_MANQUANTS>, string>(url, codeProduit, Token);
        }

        #endregion

        #region Encaissements

        internal static async Task<List<BSE_ENCAISS_MOTIFS>> GetMotifs(string type = "ENC")
        {
            string url = WSApi2.CreateLink(App.RestServiceUrl, ServiceUrlDico.ENCAISSEMENT_URL, ServiceUrlDico.MOTIFS_URL, type);
            return await RetrievAauthorizedData<BSE_ENCAISS_MOTIFS>(url);
        }

        internal static async Task<List<BSE_ENCAISS_MOTIFS>> GetAllMotifs()
        {
            string url = WSApi2.CreateLink(App.RestServiceUrl, ServiceUrlDico.ENCAISSEMENT_URL, ServiceUrlDico.MOTIFS_URL);
            return await RetrievAauthorizedData<BSE_ENCAISS_MOTIFS>(url);
        }

        internal static async Task<List<View_BSE_COMPTE>> getComptes()
        {
            string url = WSApi2.CreateLink(App.RestServiceUrl, ServiceUrlDico.ENCAISSEMENT_URL, ServiceUrlDico.COMPTES_URL);

            return await RetrievAauthorizedData<View_BSE_COMPTE>(url);
        }

        internal static async Task<List<BSE_MODE_REG>> GetMODE_REGs()
        {
            string url = WSApi2.CreateLink(App.RestServiceUrl, ServiceUrlDico.VIREMENT_URL, ServiceUrlDico.MODE_REG_URL);
            return await RetrievAauthorizedData<BSE_MODE_REG>(url);
        }

        #endregion

        #region Activation

        internal static async Task<LicenceInfos> ActivateClient(Client client)
        {
            string url = ServiceUrlDico.LICENCE_ACTIVATION_URL;
            string result = await WSApi2.PostValue<Client>(url, client);
            client.LicenceTxt = result;
            LicenceInfos licInfos = LicActivator.DecryptLicence(result);
            return licInfos;
        }

        internal static async Task<bool> DeactivateClient(Client client)
        {
            string url = ServiceUrlDico.LICENCE_DEACTIVATION_URL;
            string result = await WSApi2.PutValue<Client>(url, client);

            return !string.IsNullOrEmpty(result);
        }

        internal static async Task<string> GetWebApiVersion()
        {
            string url = WSApi2.CreateLink(App.RestServiceUrl, ServiceUrlDico.WebApiVersion, ServiceUrlDico.New_Version_URL);
            return await RetrievValAauthorizedData<string>(url);
        }

        internal static async Task<string> GetNewWebApiVersion()
        {
            string url = WSApi2.CreateLink(ServiceUrlDico.WEBAPI_XML_URL);
            return await RetrievValAauthorizedData<string>(url);
        }

        internal static async Task<string> GetNewVersion()
        {
            string url = WSApi2.CreateLink(ServiceUrlDico.WEBAPI_XML_URL);
            if (Constants.AppName == Apps.XPH_Mob)
                url = WSApi2.CreateLink(ServiceUrlDico.XOFFICINE_XML_URL);
            else if (Constants.AppName == Apps.XCOM_Mob)
                url = WSApi2.CreateLink(ServiceUrlDico.XCOMM_XML_URL);
            else if (Constants.AppName == Apps.X_DISTRIBUTION)
                url = WSApi2.CreateLink(ServiceUrlDico.XDISTRIB_XML_URL);
            return await RetrievValAauthorizedData<string>(url);
        }
        public static async Task<string> UpdateVersion(string Version)
        {
            string url = WSApi2.CreateLink(App.RestServiceUrl, ServiceUrlDico.WebApiVersion, ServiceUrlDico.Update_URL);
            url += WSApi2.AddParam(url, "versionToCompare", Version);
            return await WSApi2.PostAauthorizedValue<string, string>(url, Version, Token);
        }
        #endregion

        #region Ventes

        public static async Task<List<View_VTE_JOURNAL_DETAIL>> GetVenteDetails(string codeVente)
        {
            string url = WSApi2.CreateLink(App.RestServiceUrl, ServiceUrlDico.VENTES_URL, ServiceUrlDico.VENTES_DETAILS_URL);
            url += WSApi2.AddParam(url, "codeVente", codeVente);

            return await RetrievAauthorizedData<View_VTE_JOURNAL_DETAIL>(url);
        }

        public static async Task<List<View_VTE_VENTE_LOT>> GetVenteLotDetails(string codeVente)
        {
            string url = WSApi2.CreateLink(App.RestServiceUrl, ServiceUrlDico.VENTES_URL, ServiceUrlDico.VENTES_DETAILS_URL);
            url += WSApi2.AddParam(url, "codeVente", codeVente);

            return await RetrievAauthorizedData<View_VTE_VENTE_LOT>(url);
        }

        public static async Task<List<View_VTE_VENTE_LIVRAISON>> GetVenteLotLivraisonDetails(string codeVente)
        {
            string url = WSApi2.CreateLink(App.RestServiceUrl, ServiceUrlDico.VENTES_URL, ServiceUrlDico.VENTES_DETAILS_URL);
            url += WSApi2.AddParam(url, "codeVente", codeVente);

            return await RetrievAauthorizedData<View_VTE_VENTE_LIVRAISON>(url);
        }

        public static async Task<List<BSE_DOCUMENTS_TYPE>> GetVenteTypes()
        {
            string url = WSApi2.CreateLink(App.RestServiceUrl, ServiceUrlDico.VENTES_URL, ServiceUrlDico.VENTES_TYPES_URL);

            return await RetrievAauthorizedData<BSE_DOCUMENTS_TYPE>(url);
        }

        public static async Task<List<BSE_DOCUMENT_STATUS>> GetStatusCommande()
        {
            string url = WSApi2.CreateLink(App.RestServiceUrl, ServiceUrlDico.VTE_COMMANDE, ServiceUrlDico.STATUS_COMMANDE_URL);

            return await RetrievAauthorizedData<BSE_DOCUMENT_STATUS>(url);
        }

        public static async Task<List<View_VTE_VENTE_LOT>> GetCommandeDetails(string codeCommande)
        {
            string url = WSApi2.CreateLink(App.RestServiceUrl, ServiceUrlDico.VTE_COMMANDE, ServiceUrlDico.COMMANDE_DETAILS_URL);
            url += WSApi2.AddParam(url, "codeCommande", codeCommande);

            return await RetrievAauthorizedData<View_VTE_VENTE_LOT>(url);
        }

        public static async Task<List<View_VTE_COMMANDE>> GetCommande(string codeCommande)
        {
            string url = WSApi2.CreateLink(App.RestServiceUrl, ServiceUrlDico.VTE_COMMANDE, ServiceUrlDico.COMMANDE_URL);
            url += WSApi2.AddParam(url, "codeCommande", codeCommande);

            return await RetrievAauthorizedData<View_VTE_COMMANDE>(url);
        }

        public static async Task<List<View_VTE_VENTE>> GetVente(string codeVente)
        {
            string url = WSApi2.CreateLink(App.RestServiceUrl, ServiceUrlDico.VENTES_URL, ServiceUrlDico.VENTES_GET_VENTE_URL);
            url += WSApi2.AddParam(url, "codeVente", codeVente);

            return await RetrievAauthorizedData<View_VTE_VENTE>(url);
        }
        #endregion

        #region Achats
        public static async Task<List<View_ACH_DOCUMENT_DETAIL>> GetAchatsDetails(string codeDoc)
        {
            string url = WSApi2.CreateLink(App.RestServiceUrl, ServiceUrlDico.ACH_ACHATS, ServiceUrlDico.ACHATS_DETAILS_URL);
            url += WSApi2.AddParam(url, "codeDoc", codeDoc);

            return await RetrievAauthorizedData<View_ACH_DOCUMENT_DETAIL>(url);
        }

        #endregion

        #region Manafiaa
        public static async Task<List<View_PRD_AGRICULTURE_DETAIL>> GetProductionDetails(string codeDoc)
        {
            string url = WSApi2.CreateLink(App.RestServiceUrl, ServiceUrlDico.ACH_PRODUCTION, ServiceUrlDico.PRODUCTION_DETAILS_URL);
            url += WSApi2.AddParam(url, "codeDoc", codeDoc);

            return await RetrievAauthorizedData<View_PRD_AGRICULTURE_DETAIL>(url);
        }

        public static async Task<List<View_PRD_AGRICULTURE_DETAIL>> GetProductionInfos(string codeDoc)
        {
            string url = WSApi2.CreateLink(App.RestServiceUrl, ServiceUrlDico.ACH_PRODUCTION, ServiceUrlDico.PRODUCTION_DETAILS_URL);
            url += WSApi2.AddParam(url, "codeDoc", codeDoc);

            return await RetrievAauthorizedData<View_PRD_AGRICULTURE_DETAIL>(url);
        }

        public static async Task<decimal> GetPesse()
        {
            string url = WSApi2.CreateLink(App.RestServiceUrl, ServiceUrlDico.ACHATS_PESEE_URL);
            decimal result = await RetrievValAauthorizedData<decimal>(url);
            return result;
        }

        public static async Task<List<XPrinter>> GetPrintersList()
        {
            string url = WSApi2.CreateLink(App.RestServiceUrl, ServiceUrlDico.PRINTERS_LIST_URL);
            List<XPrinter> result = await RetrievAauthorizedData<XPrinter>(url);
            return result;
        }

        public static async Task<List<string>> GetImmatriculations(string str)
        {
            string url = WSApi2.CreateLink(App.RestServiceUrl, ServiceUrlDico.ACH_ACHATS, ServiceUrlDico.ACHATS_IMMATRICULATIONS_URL);
            url += WSApi2.AddParam(url, "str", str);

            return await RetrievAauthorizedData<string>(url);
        }

        public static async Task<string> GenerateProductionOrder(string[] strs)
        {
            string url = WSApi2.CreateLink(App.RestServiceUrl, ServiceUrlDico.ACH_PRODUCTION, ServiceUrlDico.PRODUCTION_GENERATE_PRODUCTION_URL);
            url += WSApi2.AddParam(url, "param01", "param01");

            return await WSApi2.PostAauthorizedValue<string, string[]>(url, strs, Token);
        }

        public static async Task<bool> SaveQteProduite(string codeDoc, decimal Qte)
        {
            string url = WSApi2.CreateLink(App.RestServiceUrl, ServiceUrlDico.ACH_PRODUCTION, ServiceUrlDico.PRODUCTION_SAVE_QTE_PRODUITE_URL);
            url += WSApi2.AddParam(url, "param02", "param02");

            Dictionary<string, decimal> values = new Dictionary<string, decimal>();
            values.Add(codeDoc, Qte);

            return await WSApi2.PostAauthorizedValue<bool, Dictionary<string, decimal>>(url, values, Token);
        }

        public static async Task<bool> PrintQRProduit(string codeDoc, int Qte, string printerName)
        {
            string url = WSApi2.CreateLink(App.RestServiceUrl, ServiceUrlDico.ACH_PRODUCTION, ServiceUrlDico.PRODUCTION_PRINT_QR_CODE_URL);
            url += WSApi2.AddParam(url, "param05", "param05");
            url += WSApi2.AddParam(url, "printerName", printerName);

            Dictionary<string, int> values = new Dictionary<string, int>();
            values.Add(codeDoc, Qte);

            return await WSApi2.PostAauthorizedValue<bool, Dictionary<string, int>>(url, values, Token);
        }

        public static async Task<bool> LivrerProduction(string codeDoc, string codeDocDetail)
        {
            string url = WSApi2.CreateLink(App.RestServiceUrl, ServiceUrlDico.ACH_PRODUCTION, ServiceUrlDico.PRODUCTION_UPDATE_LIVRAISON_INFOS_URL);
            url += WSApi2.AddParam(url, "param06", "param06");

            Dictionary<string, string> values = new Dictionary<string, string>();
            values.Add(codeDoc, codeDocDetail);

            return await WSApi2.PostAauthorizedValue<bool, Dictionary<string, string>>(url, values, Token);
        }

        public static async Task<bool> UpdateStatus(string codeDoc, string status)
        {
            string url = WSApi2.CreateLink(App.RestServiceUrl, ServiceUrlDico.ACH_PRODUCTION, ServiceUrlDico.PRODUCTION_UPDATE_STATUS_URL);
            url += WSApi2.AddParam(url, "param04", "param04");

            Dictionary<string, string> values = new Dictionary<string, string>();
            values.Add(codeDoc, status);

            return await WSApi2.PostAauthorizedValue<bool, Dictionary<string, string>>(url, values, Token);
        }

        public static async Task<bool> SaveProdEmballages(List<View_BSE_EMBALLAGE> embalagges, string codeDetail)
        {
            string url = WSApi2.CreateLink(App.RestServiceUrl, ServiceUrlDico.ACH_PRODUCTION, ServiceUrlDico.PRODUCTION_SAVE_EMBALLAGES_URL);
            url += WSApi2.AddParam(url, "codeDetail", codeDetail);
            url += WSApi2.AddParam(url, "param03", "param03");


            return await WSApi2.PostAauthorizedValue<bool, List<View_BSE_EMBALLAGE>>(url, embalagges, Token);
        }
        #endregion

        #region Produits

        public static async Task<View_AssistantCommandes> GetProduitDetails(string codeProduit)
        {
            string url = WSApi2.CreateLink(App.RestServiceUrl, ServiceUrlDico.PRODUITS_URL, ServiceUrlDico.PRODUITS_DETAILS_URL);
            url += WSApi2.AddParam(url, "codeProduit", codeProduit);

            return await RetrievValAauthorizedData<View_AssistantCommandes>(url);
        }

        public static async Task<View_AssistantCommandes> GetProduitRefDetails(string refProduit)
        {
            string url = WSApi2.CreateLink(App.RestServiceUrl, ServiceUrlDico.PRODUITS_URL, ServiceUrlDico.PRODUITS_REF_DETAILS_URL);
            url += WSApi2.AddParam(url, "refProduit", refProduit);

            return await RetrievValAauthorizedData<View_AssistantCommandes>(url);
        }

        public static async Task<List<View_STK_STOCK>> GetLots(string codeProduit)
        {
            string url = WSApi2.CreateLink(App.RestServiceUrl, ServiceUrlDico.PRODUITS_URL, ServiceUrlDico.PRODUITS_LOTS_URL);
            url += WSApi2.AddParam(url, "codeProduit", codeProduit);

            return await RetrievAauthorizedData<View_STK_STOCK>(url);
        }

        public static async Task<List<View_STK_STOCK>> GetLotsFromRef(string refProduit)
        {
            string url = WSApi2.CreateLink(App.RestServiceUrl, ServiceUrlDico.PRODUITS_URL, ServiceUrlDico.PRODUITS_LOTS_REF_URL);
            url += WSApi2.AddParam(url, "refProduit", refProduit);

            return await RetrievAauthorizedData<View_STK_STOCK>(url);
        }

        public static async Task<List<BSE_TABLE_TYPE>> GetProduitTypes()
        {
            string url = WSApi2.CreateLink(App.RestServiceUrl, ServiceUrlDico.PRODUITS_URL, ServiceUrlDico.PRODUITS_TYPES_URL);
            return await RetrievAauthorizedData<BSE_TABLE_TYPE>(url);
        }

        public static async Task<List<RotationDesProduitsDetails>> GetRotationProduitDetails(string codeProduit, string startDate, string endDate, string domain)
        {
            string url = WSApi2.CreateLink(App.RestServiceUrl, ServiceUrlDico.ROTATION_URL, ServiceUrlDico.ROTATION_SELECT_PRODUCT);
            url += WSApi2.AddParam(url, "codeProduit", codeProduit);
            url += WSApi2.AddParam(url, "startDate", startDate);
            url += WSApi2.AddParam(url, "endDate", endDate);
            url += WSApi2.AddParam(url, "domain", domain);

            return await RetrievValAauthorizedData<List<RotationDesProduitsDetails>>(url);
        }

        public static async Task<List<BSE_TABLE>> GetProduitFamilles()
        {
            string url = WSApi2.CreateLink(App.RestServiceUrl, ServiceUrlDico.PRODUITS_URL, ServiceUrlDico.PRODUITS_FAMILLES_URL);
            return await RetrievAauthorizedData<BSE_TABLE>(url);
        }

        public static async Task<List<BSE_PRODUIT_TAG>> GetProduitTags()
        {
            string url = WSApi2.CreateLink(App.RestServiceUrl, ServiceUrlDico.PRODUIT_TAG_URL, ServiceUrlDico.PRODUIT_GET_TAG_URL);
            return await RetrievAauthorizedData<BSE_PRODUIT_TAG>(url);
        }

        public static async Task<List<BSE_PRODUIT_LABO>> GetProduitLabos()
        {
            string url = WSApi2.CreateLink(App.RestServiceUrl, ServiceUrlDico.PRODUIT_LABO_URL, ServiceUrlDico.PRODUIT_GET_LABO_URL);
            return await RetrievAauthorizedData<BSE_PRODUIT_LABO>(url);
        }

        public static async Task<List<BSE_PRODUIT_UNITE>> GetProduitUnite()
        {
            string url = WSApi2.CreateLink(App.RestServiceUrl, ServiceUrlDico.PRODUIT_UNITE_URL, ServiceUrlDico.PRODUIT_GET_UNITE_URL);
            return await RetrievAauthorizedData<BSE_PRODUIT_UNITE>(url);
        }

        public static async Task<List<View_STK_PRODUITS>> GetAllProduct()
        {

            string url = WSApi2.CreateLink(App.RestServiceUrl, ServiceUrlDico.STK_PRODUITS_COM_URL, ServiceUrlDico.ALL_PRODUCTS);
            return await RetrievAauthorizedData<View_STK_PRODUITS>(url);
        }
        /// <summary>
        /// fonction pour récupérer le produit qui a le code-barres scanné dans le domaine officine
        /// </summary>
        /// <param name="codeBarre"></param>
        /// <returns></returns>
        public static async Task<List<View_STK_PRODUITS>> GetProductByCodeBarre(string codeBarre)
        {
            string url = WSApi2.CreateLink(App.RestServiceUrl, ServiceUrlDico.STK_PRODUITS_URL, ServiceUrlDico.STK_PRODUITS_CODE_BARRE);
            url += WSApi2.AddParam(url, "codeBarre", codeBarre);
            return await RetrievAauthorizedData<View_STK_PRODUITS>(url);
        }

        //internal static async Task<List<View_STK_PRODUITS>> getProducts()
        //{
        //    string url = WSApi2.CreateLink(App.RestServiceUrl, ServiceUrlDico.PROD_URL, ServiceUrlDico.PROD_URL_GET_ALL);

        //    return await RetrievAauthorizedData<View_STK_PRODUITS>(url);
        //}
        #endregion

        #region  RFID
        internal static async Task<int> AddRfids(List<STK_STOCK_RFID> rfids)
        {
            string url = WSApi2.CreateLink(App.RestServiceUrl, ServiceUrlDico.RFID_URL, ServiceUrlDico.RFID_AddRFIDs_URL);

            return await WSApi2.PostAauthorizedValue<int, List<STK_STOCK_RFID>>(url, rfids, App.User.Token.access_token);
        }
        internal static async Task<List<View_STK_STOCK>> getStckFroIdStock(int IDStock)
        {
            string url = WSApi2.CreateLink(App.RestServiceUrl, ServiceUrlDico.RFID_URL, ServiceUrlDico.RFID_GET_STOCK_FROM_IDSTOCK);
            url += WSApi2.AddParam(url, "IdStock", Convert.ToString(IDStock));
            return await RetrievAauthorizedData<View_STK_STOCK>(url);
        }
        internal static async Task<List<View_STK_STOCK_RFID>> getStockFromRFIDs(List<string> RFIDs)
        {
            string url = WSApi2.CreateLink(App.RestServiceUrl, ServiceUrlDico.RFID_URL, ServiceUrlDico.RFID_GET_STOCK_FROM_RFIDs);
            return await WSApi2.PostAauthorizedValue<List<View_STK_STOCK_RFID>, List<string>>(url, RFIDs, App.User.Token.access_token);
        }
        internal static async Task<View_STK_INVENTAIRE> getCurentInventaire()
        {
            string url = WSApi2.CreateLink(App.RestServiceUrl, ServiceUrlDico.RFID_URL, ServiceUrlDico.RFID_GET_CURENT_INV);

            return await RetrievValAauthorizedData<View_STK_INVENTAIRE>(url);
        }
        internal static async Task<bool> UpdateCurentInventaire(List<View_STK_STOCK_RFID> lots, string modeValidate, string numInve, bool isOuvert)
        {
            string url = WSApi2.CreateLink(App.RestServiceUrl, ServiceUrlDico.RFID_URL, ServiceUrlDico.RFID_UPDATE_CURENT_INV);
            url += WSApi2.AddParam(url, "modvalidate", modeValidate);
            url += WSApi2.AddParam(url, "numInv", numInve);
            url += WSApi2.AddParam(url, "isOuvert", isOuvert.ToString());
            return await WSApi2.PostAauthorizedValue<bool, List<View_STK_STOCK_RFID>>(url, lots, App.User.Token.access_token);
        }
        internal static async Task<List<View_STK_STOCK>> getStckFromCodeBarre(string cODE_BARRE_LOT)
        {
            string url = WSApi2.CreateLink(App.RestServiceUrl, ServiceUrlDico.RFID_URL, ServiceUrlDico.RFID_GET_STOCK_COED_BARRE);
            url += WSApi2.AddParam(url, "codebarre", cODE_BARRE_LOT);
            return await RetrievAauthorizedData<View_STK_STOCK>(url);
        }
        #endregion

        #region Stock
        internal static async Task<List<View_STK_SORTIE_DETAIL>> getSortieDetails(string code)
        {
            string url = WSApi2.CreateLink(App.RestServiceUrl, ServiceUrlDico.SORTIE_URL, ServiceUrlDico.SORTIE_GET_STOCK_SORTIE);
            url += WSApi2.AddParam(url, "code", code);
            return await RetrievAauthorizedData<View_STK_SORTIE_DETAIL>(url);
        }

        internal static async Task<List<BSE_SORTIE_TYPE>> getSortieMotifs()
        {
            string url = WSApi2.CreateLink(App.RestServiceUrl, ServiceUrlDico.SORTIE_TYPE_URL, ServiceUrlDico.SORTIE_GET_TYPE);
            return await RetrievAauthorizedData<BSE_SORTIE_TYPE>(url);
        }
        /// <summary>
        /// pour la recupérarion des types des sortie de stock
        /// </summary>
        /// <returns></returns>
        internal static async Task<List<BSE_DOCUMENT_STATUS>> getSortieTypes()
        {
            string url = WSApi2.CreateLink(App.RestServiceUrl, ServiceUrlDico.SORTIE_URL, ServiceUrlDico.SORTIE_GET_STOCK_TYPES);
            return await RetrievAauthorizedData<BSE_DOCUMENT_STATUS>(url);
        }

        internal static async Task<List<View_STK_MOTIF_ECHANGE>> GetMotifEchange()
        {
            string url = WSApi2.CreateLink(App.RestServiceUrl, ServiceUrlDico.ECHANGE_URL, ServiceUrlDico.ECHANGE_GET_MOTIFS);
            return await RetrievAauthorizedData<View_STK_MOTIF_ECHANGE>(url);
        }

        internal static async Task<List<BSE_TIERS_TYPE>> GetTypeTiers()
        {
            string url = WSApi2.CreateLink(App.RestServiceUrl, ServiceUrlDico.ECHANGE_URL, ServiceUrlDico.ECHANGE_GET_TYPE_TIERS);
            return await RetrievAauthorizedData<BSE_TIERS_TYPE>(url);
        }
        internal static async Task<List<View_TRS_TIERS>> GetListeTiers(string typeTiers, string isPharmacien)
        {
            string url = WSApi2.CreateLink(App.RestServiceUrl, ServiceUrlDico.ECHANGE_URL, ServiceUrlDico.ECHANGE_GET_TIERS);
            url += WSApi2.AddParam(url, "typeTiers", typeTiers);
            url += WSApi2.AddParam(url, "isPharmacien", isPharmacien);
            return await RetrievAauthorizedData<View_TRS_TIERS>(url);
        }
        internal static async Task<List<View_TRS_TIERS>> GetTier(string codeTier)
        {
            string url = WSApi2.CreateLink(App.RestServiceUrl, ServiceUrlDico.TIERS_URL, ServiceUrlDico.TRS_TIER_GET_TIER);
            url += WSApi2.AddParam(url, "codeTier", codeTier);
            return await RetrievAauthorizedData<View_TRS_TIERS>(url);
        }


        internal static async Task<List<View_BSE_MAGASIN>> GetListeMagasin()
        {
            string url = WSApi2.CreateLink(App.RestServiceUrl, ServiceUrlDico.ECHANGE_URL, ServiceUrlDico.ECHANGE_GET_MAGASIN);
            return await RetrievAauthorizedData<View_BSE_MAGASIN>(url);
        }
        internal static async Task<List<View_VTE_VENTE_LOT>> GetEchangeDetail(string codeDocument, string typeDoc)
        {
            string url = WSApi2.CreateLink(App.RestServiceUrl, ServiceUrlDico.ECHANGE_URL, ServiceUrlDico.ECHANGE_GET_DETAIL);
            url += WSApi2.AddParam(url, "codeDocument", codeDocument);
            url += WSApi2.AddParam(url, "typeDoc", typeDoc);
            return await RetrievAauthorizedData<View_VTE_VENTE_LOT>(url);
        }


        internal static async Task<List<View_STK_TRANSFERT>> GetTransfertHeader(string codeTransfer)
        {
            string url = WSApi2.CreateLink(App.RestServiceUrl, ServiceUrlDico.TRANSFERT_STOCK_URL, ServiceUrlDico.TRANSFERT_STOCK_SELECT_TRANSFERT);
            url += WSApi2.AddParam(url, "codeTransfer", codeTransfer);
            return await RetrievAauthorizedData<View_STK_TRANSFERT>(url);
        }

        internal static async Task<List<View_STK_TRANSFERT_DETAIL>> GetTransfertDetail(string codeTransfer)
        {
            string url = WSApi2.CreateLink(App.RestServiceUrl, ServiceUrlDico.TRANSFERT_STOCK_DETAIL_URL, ServiceUrlDico.TRANSFERT_STOCK_GET_DETAIL);
            url += WSApi2.AddParam(url, "codeTransfer", codeTransfer);
            return await RetrievAauthorizedData<View_STK_TRANSFERT_DETAIL>(url);
        }
        //internal static async Task<View_STK_INVENTAIRE_DETAIL> GetInventaireDetail(string numIvent, string codeBarre)
        //{
        //    string url = WSApi2.CreateLink(App.RestServiceUrl, ServiceUrlDico.Print_Url);
        //    url += WSApi2.AddParam(url, "printerName", numIvent);
        //    url += WSApi2.AddParam(url, "codeVente", codeBarre);
        //    return await WSApi2.PostAauthorizedValue<View_STK_INVENTAIRE_DETAIL>(url);

        //}

        #endregion

        #region User

        internal static async Task<List<View_SYS_USER>> getUserIDs()
        {
            string url = WSApi2.CreateLink(App.RestServiceUrl, ServiceUrlDico.USER_URL, ServiceUrlDico.USER_GET_IDS);
            return await RetrievAauthorizedData<View_SYS_USER>(url);
        }
        #endregion
        internal static async Task<bool> printTicket(string codeVent, string printerName)
        {
            string url = WSApi2.CreateLink(App.RestServiceUrl, ServiceUrlDico.Print_Url);
            url += WSApi2.AddParam(url, "printerName", printerName);
            url += WSApi2.AddParam(url, "codeVente", codeVent);
            return await WSApi2.PostAauthorizedValue<bool, string>(url, printerName, Token);

        }

        #region CHIFA
        internal static async Task<List<View_CFA_MOBILE_DETAIL_FACTURE>> SelectPatients(int period=0,string dateName="DF")
        {
            string url = WSApi2.CreateLink(App.RestServiceUrl, ServiceUrlDico.CFA_DETAIL_FACTURE_URL, ServiceUrlDico.CFA_DETAIL_SELECT_PATIENTS_URL);
            url += "?period=" + period;
            url += "&dateName=" + dateName;
            return await RetrievAauthorizedData<View_CFA_MOBILE_DETAIL_FACTURE>(url);

        }

        internal static async Task<List<View_CFA_MOBILE_DETAIL_FACTURE>> SelectTraitment(string numFacture)
        {
            string url = WSApi2.CreateLink(App.RestServiceUrl, ServiceUrlDico.CFA_DETAIL_FACTURE_URL, ServiceUrlDico.CFA_DETAIL_SELECT_TRAITMENT_URL);
            url += "?numFacture=" + numFacture;
            return await RetrievAauthorizedData<View_CFA_MOBILE_DETAIL_FACTURE>(url);

        } 
        internal static async Task<List<View_CFA_MOBILE_DETAIL_FACTURE>> SelectMedications(int period=0,string dateName="DF")
        {
            string url = WSApi2.CreateLink(App.RestServiceUrl, ServiceUrlDico.CFA_DETAIL_FACTURE_URL, ServiceUrlDico.CFA_DETAIL_SELECT_MEDICACTIONS_URL);
            url += "?period=" + period;
            url += "&dateName=" + dateName;
            return await RetrievAauthorizedData<View_CFA_MOBILE_DETAIL_FACTURE>(url);

        }  
        internal static async Task<List<View_CFA_MOBILE_DETAIL_FACTURE>> SelectPatientByMedication(string codeProduit)
        {
            string url = WSApi2.CreateLink(App.RestServiceUrl, ServiceUrlDico.CFA_DETAIL_FACTURE_URL, ServiceUrlDico.CFA_DETAIL_SELECT_PATIENTS_BY_MEDICATION_URL);
            url += "?codeProduit=" + codeProduit;
            return await RetrievAauthorizedData<View_CFA_MOBILE_DETAIL_FACTURE>(url);

        }
        
        internal static async Task<List<View_CFA_MOBILE_DETAIL_FACTURE>> SelectBeneficiare(string numBeneficiare)
        {
            string url = WSApi2.CreateLink(App.RestServiceUrl, ServiceUrlDico.CFA_DETAIL_FACTURE_URL, ServiceUrlDico.CFA_DETAIL_SELECT_BENEFICIARE_URL);
            url += "?numBeneficiare=" + numBeneficiare;
            return await RetrievAauthorizedData<View_CFA_MOBILE_DETAIL_FACTURE>(url);

        }
        
        internal static async Task<List<View_CFA_MOBILE_DETAIL_FACTURE>> SelectFactsByBeneficiare(string numAssure, string rand)
        {
            string url = WSApi2.CreateLink(App.RestServiceUrl, ServiceUrlDico.CFA_DETAIL_FACTURE_URL, ServiceUrlDico.CFA_DETAIL_SELECT_FACTS_URL);
            url += "?numAssure=" + numAssure;
            url += "&rand=" + rand;
            return await RetrievAauthorizedData<View_CFA_MOBILE_DETAIL_FACTURE>(url);

        }
        internal static async Task<List<View_CFA_MOBILE_DETAIL_FACTURE>> SelectBeneficiareRelatives(string numAssure)
        {
            string url = WSApi2.CreateLink(App.RestServiceUrl, ServiceUrlDico.CFA_DETAIL_FACTURE_URL, ServiceUrlDico.CFA_DETAIL_SELECT_RELATIVES_URL);
            url += "?numAssure=" + numAssure;
            return await RetrievAauthorizedData<View_CFA_MOBILE_DETAIL_FACTURE>(url);

        }
        #endregion


    }
}