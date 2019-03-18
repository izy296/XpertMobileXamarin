using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using XpertMobileApp.Api.Services;
using XpertMobileApp.DAL;
using XpertMobileApp.Helpers;
using XpertMobileApp.Models;

namespace XpertMobileApp.Services
{
    public class WebServiceClient
    {

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
            catch (XpertException e)
            {
                throw new XpertException(e.Message, e.Code);
            }
        }

        public static async Task<List<TRS_JOURNEES>> GetSessionInfos()
        {
            string url = WSApi2.CreateLink(App.RestServiceUrl, ServiceUrlDico.SESSION_INFO_URL);

            return await RetrievAauthorizedData<TRS_JOURNEES>(url);
        }

        #region statistiques

        internal static async Task<List<View_VTE_Vente_Td>> GetMargeParVendeur(DateTime? startDate, DateTime? endDate)
        {
            string url = WSApi2.CreateLink(App.RestServiceUrl, ServiceUrlDico.DASHBOARD_URL, ServiceUrlDico.MARGE_PAR_VENDEUR_URL);
            url += "?startDate=" + startDate?.ToString("yyyyMMddHHmmss");
            url += "&endDate=" + endDate?.ToString("yyyyMMddHHmmss");

            return await RetrievAauthorizedData<View_VTE_Vente_Td>(url);
        }

        internal static async Task<List<View_TRS_ENCAISS>> GetStatisticEncaiss(DateTime? startDate, DateTime? endDate)
        {
            string url = WSApi2.CreateLink(App.RestServiceUrl, ServiceUrlDico.ENCAISSEMENT_URL, ServiceUrlDico.STATISTIC_URL);
            url += "?startDate=" + startDate?.ToString("yyyyMMddHHmmss");
            url += "&endDate=" + endDate?.ToString("yyyyMMddHHmmss");

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

        internal static async Task<List<View_TRS_TIERS>> GetTiers(int page = 1, int nbrRows = 10, string type = "", string famille= "", string searchText = "")
        {
            string url = WSApi2.CreateLink(App.RestServiceUrl, ServiceUrlDico.ENCAISSEMENT_URL, ServiceUrlDico.TIERS_URL);

            url += WSApi2.AddParam(url, "page", Convert.ToString(page));
            url += WSApi2.AddParam(url, "nbrRows", Convert.ToString(nbrRows));

            if (!string.IsNullOrEmpty(type))
                url += WSApi2.AddParam(url, "type", type);
            if (!string.IsNullOrEmpty(famille))
                url += WSApi2.AddParam(url, "famille", famille);

            if (!string.IsNullOrEmpty(searchText))
                url += WSApi2.AddParam(url, "searchText", searchText);

            return await RetrievAauthorizedData<View_TRS_TIERS>(url);
        }

        public static async Task<int> GetTiersCount(string type = "", string famille = "", string searchText= "")
        {
            string url = WSApi2.CreateLink(App.RestServiceUrl, ServiceUrlDico.ENCAISSEMENT_URL, ServiceUrlDico.TIERS_COUNT_URL);

            if (!string.IsNullOrEmpty(type))
                url += WSApi2.AddParam(url, "type", type);

            if (!string.IsNullOrEmpty(famille))
                url += WSApi2.AddParam(url, "famille", famille);

            if (!string.IsNullOrEmpty(searchText))
                url += WSApi2.AddParam(url, "searchText", searchText);

            return await RetrievValAauthorizedData<int>(url);
        }

        #endregion

        #region Encaissements

        public static async Task<List<View_TRS_ENCAISS>> GetEncaissements(string baseUrl, string type, string page, string idCaisse, DateTime? startDate, DateTime? endDate, string codeTiers, string codeMotif, string codeCompte)
        {
            string url = WSApi2.CreateLink(baseUrl, ServiceUrlDico.ENCAISSEMENT_URL, ServiceUrlDico.ENCAISSEMENT_PER_PAGE_URL);

            url += WSApi2.AddParam(url, "type", type);
            url += WSApi2.AddParam(url, "page", page);
            url += WSApi2.AddParam(url, "id_caisse", idCaisse);
            url += WSApi2.AddParam(url, "startDate", startDate?.ToString("yyyyMMddHHmmss"));
            url += WSApi2.AddParam(url, "endDate", endDate?.ToString("yyyyMMddHHmmss"));
            if (!string.IsNullOrEmpty(codeTiers))
                url += WSApi2.AddParam(url, "codeTiers", codeTiers);
            if (!string.IsNullOrEmpty(codeCompte))
                url += WSApi2.AddParam(url, "codeCompte", codeCompte);
            if (!string.IsNullOrEmpty(codeMotif))
                url += WSApi2.AddParam(url, "codeMotif", codeMotif);

            return await RetrievAauthorizedData<View_TRS_ENCAISS>(url);
        }

        public static async Task<int> GetEncaissementsCount(string baseUrl, string type, string idCaisse, DateTime? startDate, DateTime? endDate, string codeTiers, string codeMotif, string codeCompte)
        {
            string url = WSApi2.CreateLink(baseUrl, ServiceUrlDico.ENCAISSEMENT_URL, ServiceUrlDico.ENCAISSEMENTS_COUNT);

            url += WSApi2.AddParam(url, "type", type);
            url += WSApi2.AddParam(url, "id_caisse", idCaisse);
            url += WSApi2.AddParam(url, "startDate", startDate?.ToString("yyyyMMddHHmmss"));
            url += WSApi2.AddParam(url, "endDate", endDate?.ToString("yyyyMMddHHmmss"));
            if (!string.IsNullOrEmpty(codeTiers))
                url += WSApi2.AddParam(url, "codeTiers", codeTiers);
            if (!string.IsNullOrEmpty(codeCompte))
                url += WSApi2.AddParam(url, "codeCompte", codeCompte);
            if (!string.IsNullOrEmpty(codeMotif))
                url += WSApi2.AddParam(url, "codeMotif", codeMotif);

            return await RetrievValAauthorizedData<int>(url);
        }

        internal static async Task<List<View_TRS_ENCAISS>> GetEncaissements(string type)
        {
            string url = WSApi2.CreateLink(App.RestServiceUrl, type);

            return await RetrievAauthorizedData<View_TRS_ENCAISS>(url);
        }

        internal static async Task<View_TRS_ENCAISS> AddEncaissement(View_TRS_ENCAISS encaiss)
        {
            string url = WSApi2.CreateLink(App.RestServiceUrl, ServiceUrlDico.ENCAISSEMENT_URL, ServiceUrlDico.ADD_ENCAISSEMENT_URL);

            return await WSApi2.PostAauthorizedValue<View_TRS_ENCAISS, View_TRS_ENCAISS>(url, encaiss, App.User.Token.access_token);
        }

        internal static async Task<View_TRS_ENCAISS> UpdateEncaissement(View_TRS_ENCAISS encaiss)
        {
            string url = WSApi2.CreateLink(App.RestServiceUrl, ServiceUrlDico.ENCAISSEMENT_URL, ServiceUrlDico.UPDATE_ENCAISSEMENT_URL);

            string strdata = JsonConvert.SerializeObject(encaiss);
            byte[] data = Encoding.UTF8.GetBytes(strdata);
            byte[] resultData = await WSApi.ExecutePost(url, data, App.User.Token.access_token);
            string resposeData = Encoding.UTF8.GetString(resultData);
            return JsonConvert.DeserializeObject<View_TRS_ENCAISS>(resposeData);
        }

        internal static async Task<View_TRS_ENCAISS> SaveEncaissements(View_TRS_ENCAISS item)
        {
            string url = WSApi2.CreateLink(App.RestServiceUrl, ServiceUrlDico.ENCAISSEMENT_URL);
            url += ServiceUrlDico.ADD_ENCAISSEMENT_URL;
            View_TRS_ENCAISS result = null;
            string strdata = JsonConvert.SerializeObject(item);
            byte[] data = Encoding.UTF8.GetBytes(strdata);
            byte[] resultData = await WSApi.ExecutePost(url, data, Token);
            string resposeData = Encoding.UTF8.GetString(resultData);
            result = JsonConvert.DeserializeObject<View_TRS_ENCAISS>(resposeData);

            return result;
        }

        internal static async Task<bool> DeleteEncaissement(View_TRS_ENCAISS encaiss)
        {
            string url = WSApi2.CreateLink(App.RestServiceUrl, ServiceUrlDico.ENCAISSEMENT_URL, ServiceUrlDico.DELETE_ENCAISSEMENT_URL);

            string strdata = JsonConvert.SerializeObject(encaiss);
            byte[] data = Encoding.UTF8.GetBytes(strdata);            
            byte[] resultData = await WSApi.ExecutePost(url, data, App.User.Token.access_token);
            string resposeData = Encoding.UTF8.GetString(resultData);
            return JsonConvert.DeserializeObject<bool>(resposeData);
        }

        internal static async Task<List<BSE_ENCAISS_MOTIFS>> GetMotifs(string type = "ENC")
        {
            string url = WSApi2.CreateLink(App.RestServiceUrl, ServiceUrlDico.ENCAISSEMENT_URL, ServiceUrlDico.MOTIFS_URL, type);
            return await RetrievAauthorizedData<BSE_ENCAISS_MOTIFS>(url);
        }

        internal static async Task<List<View_BSE_COMPTE>> getComptes()
        {
            string url = WSApi2.CreateLink(App.RestServiceUrl, ServiceUrlDico.ENCAISSEMENT_URL, ServiceUrlDico.COMPTES_URL);

            return await RetrievAauthorizedData<View_BSE_COMPTE>(url);
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

        #endregion

        
        #region Ventes

        public static async Task<int> GetVentesCount(string type, string idCaisse, DateTime? startDate, DateTime? endDate, string codeClient, string codeMotif, string codeUser)
        {
            string url = WSApi2.CreateLink(App.RestServiceUrl, ServiceUrlDico.VENTES_URL, ServiceUrlDico.VENTES_COUNT);

            url += WSApi2.AddParam(url, "type", type);
            url += WSApi2.AddParam(url, "id_caisse", idCaisse);
            url += WSApi2.AddParam(url, "startDate", startDate?.ToString("yyyyMMddHHmmss"));
            url += WSApi2.AddParam(url, "endDate", endDate?.ToString("yyyyMMddHHmmss"));
            if (!string.IsNullOrEmpty(codeUser))
                url += WSApi2.AddParam(url, "codeUser", codeUser);
            if (!string.IsNullOrEmpty(codeClient))
                url += WSApi2.AddParam(url, "codeClient", codeClient);
            if (!string.IsNullOrEmpty(codeMotif))
                url += WSApi2.AddParam(url, "codeMotif", codeMotif);

            return await RetrievValAauthorizedData<int>(url);
        }

        public static async Task<List<View_VTE_VENTE>> GetVentes(string type, string page, string idCaisse, DateTime? startDate, DateTime? endDate, string codeClient, string codeMotif, string codeUser)
        {
            string url = WSApi2.CreateLink(App.RestServiceUrl, ServiceUrlDico.VENTES_URL, ServiceUrlDico.VENTES_PER_PAGE_URL);

            url += WSApi2.AddParam(url, "type", type);
            url += WSApi2.AddParam(url, "page", page);
            url += WSApi2.AddParam(url, "id_caisse", idCaisse);
            url += WSApi2.AddParam(url, "startDate", startDate?.ToString("yyyyMMddHHmmss"));
            url += WSApi2.AddParam(url, "endDate", endDate?.ToString("yyyyMMddHHmmss"));
            if (!string.IsNullOrEmpty(codeUser))
                url += WSApi2.AddParam(url, "codeUser", codeUser);
            if (!string.IsNullOrEmpty(codeClient))
                url += WSApi2.AddParam(url, "codeClient", codeClient);
            if (!string.IsNullOrEmpty(codeMotif))
                url += WSApi2.AddParam(url, "codeMotif", codeMotif);

            return await RetrievAauthorizedData<View_VTE_VENTE>(url);
        }

        public static async Task<List<View_VTE_VENTE_LOT>> GetVenteDetails(string codeVente)
        {
            string url = WSApi2.CreateLink(App.RestServiceUrl, ServiceUrlDico.VENTES_URL, ServiceUrlDico.VENTES_DETAILS_URL);
            url += WSApi2.AddParam(url, "codeVente", codeVente);

            return await RetrievAauthorizedData<View_VTE_VENTE_LOT>(url);
        }

        #endregion

        #region Produits

        public static async Task<int> GetProduitsCount(string type = "", string codeFamille = "", string searchText = "")
        {
            string url = WSApi2.CreateLink(App.RestServiceUrl, ServiceUrlDico.PRODUITS_URL, ServiceUrlDico.PRODUITS_COUNT);

            if (!string.IsNullOrEmpty(type))
                url += WSApi2.AddParam(url, "type", type);
            if (!string.IsNullOrEmpty(codeFamille))
                url += WSApi2.AddParam(url, "codeFamille", codeFamille);
            if (!string.IsNullOrEmpty(searchText))
                url += WSApi2.AddParam(url, "searchText", searchText);

            return await RetrievValAauthorizedData<int>(url);
        }

        public static async Task<List<STK_PRODUITS>> GetProduits(int page = 1, int nbrRows = 10, string type = "", 
                 string famille = "", string searchText = "")
        {
            string url = WSApi2.CreateLink(App.RestServiceUrl, ServiceUrlDico.PRODUITS_URL, ServiceUrlDico.PRODUITS_PER_PAGE_URL);

            url += WSApi2.AddParam(url, "page", Convert.ToString(page));
            url += WSApi2.AddParam(url, "nbrRows", Convert.ToString(nbrRows));

            if (!string.IsNullOrEmpty(type))
                url += WSApi2.AddParam(url, "type", type);
            if (!string.IsNullOrEmpty(famille))
                url += WSApi2.AddParam(url, "famille", famille);
            if (!string.IsNullOrEmpty(searchText))
                url += WSApi2.AddParam(url, "searchText", searchText);

            return await RetrievAauthorizedData<STK_PRODUITS>(url);
        }

        public static async Task<View_AssistantCommandes> GetProduitDetails(string codeProduit)
        {
            string url = WSApi2.CreateLink(App.RestServiceUrl, ServiceUrlDico.PRODUITS_URL, ServiceUrlDico.PRODUITS_DETAILS_URL);
            url += WSApi2.AddParam(url, "codeProduit", codeProduit);

            return await RetrievValAauthorizedData<View_AssistantCommandes>(url);
        }

        public static async Task<List<View_STK_STOCK>> GetLots(string codeProduit)
        {
            string url = WSApi2.CreateLink(App.RestServiceUrl, ServiceUrlDico.PRODUITS_URL, ServiceUrlDico.PRODUITS_LOTS_URL);
            url += WSApi2.AddParam(url, "codeProduit", codeProduit);

            return await RetrievAauthorizedData<View_STK_STOCK>(url);
        }

        public static async Task<List<BSE_TABLE_TYPE>> GetProduitTypes()
        {
            string url = WSApi2.CreateLink(App.RestServiceUrl, ServiceUrlDico.PRODUITS_URL, ServiceUrlDico.PRODUITS_TYPES_URL);
            return await RetrievAauthorizedData<BSE_TABLE_TYPE>(url);
        }

        public static async Task<List<BSE_TABLE>> GetProduitFamilles()
        {
            string url = WSApi2.CreateLink(App.RestServiceUrl, ServiceUrlDico.PRODUITS_URL, ServiceUrlDico.PRODUITS_FAMILLES_URL);
            return await RetrievAauthorizedData<BSE_TABLE>(url);
        }
        #endregion
    }
}
