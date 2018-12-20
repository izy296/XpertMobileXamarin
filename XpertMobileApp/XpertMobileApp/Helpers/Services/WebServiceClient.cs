using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using System.Web;
using Xpert.Pharm.DAL;
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
            return await WSApi.ExecuteGet<List<T>>(url, Token);
        }

        public static async Task<T> RetrievValAauthorizedData<T>(string url)
        {
            return await WSApi.ExecuteGet<T>(url, Token);
        }

        public static Token Login(string baseUrl, string username, string password)
        {
            try
            {
                using (WebClient client = new WebClient())
                {
                    // url de l'authentification et du recupération du token
                    string url = baseUrl + ServiceUrlDico.TOKEN_URL;

                    // Preparation des paramettres
                    NameValueCollection values = new NameValueCollection();
                    values.Add("username", username);
                    values.Add("password", password);
                    values.Add("grant_type", "password");

                    byte[] result = WSApi.ExecutePostForm(url, values);
                    Token tokenInfos = JsonConvert.DeserializeObject<Token>(Encoding.UTF8.GetString(result));
                    DateTime dt = new DateTime();
                    dt = DateTime.Now;
                    tokenInfos.expire_Date = dt.AddSeconds(tokenInfos.expires_in);
                    return tokenInfos;
                }
            }
            catch (WebException e)
            {
                throw new WebException("Un problème est survenu lors de la tentative de connexion : " + e.Message, e);
            }
        }

        internal static async Task<View_TRS_ENCAISS> SaveEncaissements(View_TRS_ENCAISS item)
        {
            string url = WSApi.CreateLink(App.RestServiceUrl, ServiceUrlDico.ENCAISSEMENT_URL);
            url += ServiceUrlDico.ADD_ENCAISSEMENT_URL;
            View_TRS_ENCAISS result = null;
            string strdata = JsonConvert.SerializeObject(item);
            byte[] data = Encoding.UTF8.GetBytes(strdata);
            byte[] resultData = await WSApi.ExecutePost(url, data, Token);
            string resposeData = Encoding.UTF8.GetString(resultData);
            result = JsonConvert.DeserializeObject<View_TRS_ENCAISS>(resposeData);

            return result;
        }

        public static async Task<List<TRS_JOURNEES>> GetSessionInfos()
        {
            string url = WSApi.CreateLink(App.RestServiceUrl, ServiceUrlDico.SESSION_INFO_URL);

            return await RetrievAauthorizedData<TRS_JOURNEES>(url);
        }

        internal static async Task<List<View_VTE_Vente_Td>> GetMargeParVendeur(DateTime? startDate, DateTime? endDate)
        {
            string url = WSApi.CreateLink(App.RestServiceUrl, ServiceUrlDico.DASHBOARD_URL, ServiceUrlDico.MARGE_PAR_VENDEUR_URL);
            url += "?startDate=" + startDate?.ToString("yyyyMMddHHmmss");
            url += "&endDate=" + endDate?.ToString("yyyyMMddHHmmss");

            return await RetrievAauthorizedData<View_VTE_Vente_Td>(url);
        }

        internal static async Task<List<View_TRS_ENCAISS>> GetStatisticEncaiss(DateTime? startDate, DateTime? endDate)
        {
            string url = WSApi.CreateLink(App.RestServiceUrl, ServiceUrlDico.ENCAISSEMENT_URL, ServiceUrlDico.STATISTIC_URL);
            url += "?startDate=" + startDate?.ToString("yyyyMMddHHmmss");
            url += "&endDate=" + endDate?.ToString("yyyyMMddHHmmss");

            return await RetrievAauthorizedData<View_TRS_ENCAISS>(url);
        }

        public static async Task<List<View_TRS_ENCAISS>> GetEncaissements(string baseUrl, string type, string page, string idCaisse, DateTime? startDate, DateTime? endDate, string codeTiers, string codeMotif, string codeCompte)
        {
            string url = WSApi.CreateLink(baseUrl, ServiceUrlDico.ENCAISSEMENT_URL, ServiceUrlDico.ENCAISSEMENT_PER_PAGE_URL);

            url += WSApi.AddParam(url, "type", type);
            url += WSApi.AddParam(url, "page", page);
            url += WSApi.AddParam(url, "id_caisse", idCaisse);
            url += WSApi.AddParam(url, "startDate", startDate?.ToString("yyyyMMddHHmmss"));
            url += WSApi.AddParam(url, "endDate", endDate?.ToString("yyyyMMddHHmmss"));
            if (!string.IsNullOrEmpty(codeTiers))
                url += WSApi.AddParam(url, "codeTiers", codeTiers);
            if (!string.IsNullOrEmpty(codeCompte))
                url += WSApi.AddParam(url, "codeCompte", codeCompte);
            if(!string.IsNullOrEmpty(codeMotif))
                url += WSApi.AddParam(url, "codeMotif", codeMotif);
            
            return await RetrievAauthorizedData<View_TRS_ENCAISS>(url);
        }

        public static async Task<int> GetEncaissementsCount(string baseUrl, string type, string idCaisse, DateTime? startDate, DateTime? endDate, string codeTiers, string codeMotif, string codeCompte)
        {
            string url = WSApi.CreateLink(baseUrl, ServiceUrlDico.ENCAISSEMENT_URL, ServiceUrlDico.ENCAISSEMENTS_COUNT);

            url += WSApi.AddParam(url, "type", type);
            url += WSApi.AddParam(url, "id_caisse", idCaisse);
            url += WSApi.AddParam(url, "startDate", startDate?.ToString("yyyyMMddHHmmss"));
            url += WSApi.AddParam(url, "endDate", endDate?.ToString("yyyyMMddHHmmss"));
            if (!string.IsNullOrEmpty(codeTiers))
                url += WSApi.AddParam(url, "codeTiers", codeTiers);
            if (!string.IsNullOrEmpty(codeCompte))
                url += WSApi.AddParam(url, "codeCompte", codeCompte);
            if (!string.IsNullOrEmpty(codeMotif))
                url += WSApi.AddParam(url, "codeMotif", codeMotif);

            return await RetrievValAauthorizedData<int>(url);
        }

        internal static async Task<List<View_TRS_ENCAISS>> GetEncaissements(string type)
        {
            string url = WSApi.CreateLink(App.RestServiceUrl, type);
           
            return await RetrievAauthorizedData<View_TRS_ENCAISS>(url);
        }

        internal static async Task<List<View_BSE_COMPTE>> getComptes()
        {
            string url = WSApi.CreateLink(App.RestServiceUrl, ServiceUrlDico.ENCAISSEMENT_URL, ServiceUrlDico.COMPTES_URL);

            return await RetrievAauthorizedData<View_BSE_COMPTE>(url);
        }

        internal static async Task<List<BSE_ENCAISS_MOTIFS>> GetMotifs(string type= "ENC")
        {
            string url = WSApi.CreateLink(App.RestServiceUrl, ServiceUrlDico.ENCAISSEMENT_URL, ServiceUrlDico.MOTIFS_URL, type);
            return await RetrievAauthorizedData<BSE_ENCAISS_MOTIFS>(url);
        }

        internal static async Task<List<View_TRS_TIERS>> GetTiers(string type)
        {
            string url = WSApi.CreateLink(App.RestServiceUrl, ServiceUrlDico.ENCAISSEMENT_URL, ServiceUrlDico.TIERS_URL);
            return await RetrievAauthorizedData<View_TRS_TIERS>(url);
        }

        internal static async Task<List<View_BSE_COMPTE>> GetCaisses(string type)
        {
            string url = WSApi.CreateLink(App.RestServiceUrl, ServiceUrlDico.ENCAISSEMENT_URL, ServiceUrlDico.CAISSES_URL);
            return await RetrievAauthorizedData<View_BSE_COMPTE>(url);
        }

        internal static async Task<View_TRS_ENCAISS> AddEncaissement(View_TRS_ENCAISS encaiss)
        {
            string url = WSApi.CreateLink(App.RestServiceUrl, ServiceUrlDico.ENCAISSEMENT_URL, ServiceUrlDico.ADD_ENCAISSEMENT_URL);

            string strdata = JsonConvert.SerializeObject(encaiss);
            byte[] data = Encoding.UTF8.GetBytes(strdata);
            Token tokenInfos = await App.TokenDatabase.GetFirstItemAsync();
            byte[] resultData = await WSApi.ExecutePost(url, data, tokenInfos.access_token);
            string resposeData = Encoding.UTF8.GetString(resultData);
            return JsonConvert.DeserializeObject<View_TRS_ENCAISS>(resposeData);
        }

        internal static async Task<View_TRS_ENCAISS> UpdateEncaissement(View_TRS_ENCAISS encaiss)
        {
            string url = WSApi.CreateLink(App.RestServiceUrl, ServiceUrlDico.ENCAISSEMENT_URL, ServiceUrlDico.UPDATE_ENCAISSEMENT_URL);

            string strdata = JsonConvert.SerializeObject(encaiss);
            byte[] data = Encoding.UTF8.GetBytes(strdata);
            byte[] resultData = await WSApi.ExecutePost(url, data, App.User.Token.access_token);
            string resposeData = Encoding.UTF8.GetString(resultData);
            return JsonConvert.DeserializeObject<View_TRS_ENCAISS>(resposeData);
        }

        internal static async Task<bool> DeleteEncaissement(View_TRS_ENCAISS encaiss)
        {
            string url = WSApi.CreateLink(App.RestServiceUrl, ServiceUrlDico.ENCAISSEMENT_URL, ServiceUrlDico.DELETE_ENCAISSEMENT_URL);

            string strdata = JsonConvert.SerializeObject(encaiss);
            byte[] data = Encoding.UTF8.GetBytes(strdata);            
            byte[] resultData = await WSApi.ExecutePost(url, data, App.User.Token.access_token);
            string resposeData = Encoding.UTF8.GetString(resultData);
            return JsonConvert.DeserializeObject<bool>(resposeData);
        }
    }
}
