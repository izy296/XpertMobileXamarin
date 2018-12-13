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
            Token tokenInfos = await App.TokenDatabase.GetFirstItemAsync();
            return await WSApi.ExecuteGet<List<T>>(url, Token);
        }

        public static Token Login(string baseUrl, string username, string password)
        {
            try
            {
                using (WebClient client = new WebClient())
                {
                    // url de l'authentification et du recupération du token
                    string url = baseUrl + ServiceUrlDeco.TOKEN_URL;

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
            string url = WSApi.CreateLink(App.RestServiceUrl, ServiceUrlDeco.ENCAISSEMENT_URL);
            url += ServiceUrlDeco.ADD_ENCAISSEMENT_URL;
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
            string url = WSApi.CreateLink(App.RestServiceUrl, ServiceUrlDeco.SESSION_INFO_URL);

            return await RetrievAauthorizedData<TRS_JOURNEES>(url);
        }

        internal static async Task<List<View_VTE_Vente_Td>> GetMargeParVendeur(DateTime? startDate, DateTime? endDate)
        {
            string url = WSApi.CreateLink(App.RestServiceUrl, ServiceUrlDeco.DASHBOARD_URL, ServiceUrlDeco.MARGE_PAR_VENDEUR_URL);
            url += "?startDate=" + startDate?.ToString("yyyyMMddHHmmss");
            url += "&endDate=" + endDate?.ToString("yyyyMMddHHmmss");

            return await RetrievAauthorizedData<View_VTE_Vente_Td>(url);
        }

        internal static async Task<List<View_TRS_ENCAISS>> GetStatisticEncaiss(DateTime? startDate, DateTime? endDate)
        {
            string url = WSApi.CreateLink(App.RestServiceUrl, ServiceUrlDeco.ENCAISSEMENT_URL, ServiceUrlDeco.STATISTIC_URL);
            url += "?startDate=" + startDate?.ToString("yyyyMMddHHmmss");
            url += "&endDate=" + endDate?.ToString("yyyyMMddHHmmss");

            return await RetrievAauthorizedData<View_TRS_ENCAISS>(url);
        }

        public static async Task<List<View_TRS_ENCAISS>> GetEncaissements(string baseUrl, string type, string page, string idCaisse, string startDate, string endDate,
             string codeTiers, string codeMotif, string codeCompte)
        {
            string url = "";
            if (string.IsNullOrEmpty(startDate) && string.IsNullOrEmpty(endDate))
            {
                url += WSApi.CreateLink(baseUrl, ServiceUrlDeco.ENCAISSEMENT_PER_PAGE_URL, type, page, idCaisse);
            }
            else
            {
                url += WSApi.CreateLink(baseUrl, ServiceUrlDeco.ENCAISSEMENT_PER_PAGE_URL, type, page, idCaisse, startDate, endDate, codeTiers, codeMotif, codeMotif, codeCompte);
            }

            return await RetrievAauthorizedData<View_TRS_ENCAISS>(url);
        }

        internal static async Task<List<View_TRS_ENCAISS>> GetEncaissements(string type)
        {
            string url = WSApi.CreateLink(App.RestServiceUrl, type);
           
            return await RetrievAauthorizedData<View_TRS_ENCAISS>(url);
        }

        internal static async Task<List<View_BSE_COMPTE>> getComptes()
        {
            string url = WSApi.CreateLink(App.RestServiceUrl, ServiceUrlDeco.ENCAISSEMENT_URL, ServiceUrlDeco.COMPTES_URL);

            return await RetrievAauthorizedData<View_BSE_COMPTE>(url);
        }

        internal static async Task<List<BSE_ENCAISS_MOTIFS>> GetMotifs(string type= "ENC")
        {
            string url = WSApi.CreateLink(App.RestServiceUrl, ServiceUrlDeco.ENCAISSEMENT_URL, ServiceUrlDeco.MOTIFS_URL, type);
            return await RetrievAauthorizedData<BSE_ENCAISS_MOTIFS>(url);
        }

        internal static async Task<List<View_TRS_TIERS>> GetTiers(string type)
        {
            string url = WSApi.CreateLink(App.RestServiceUrl, ServiceUrlDeco.ENCAISSEMENT_URL, ServiceUrlDeco.TIERS_URL);
            return await RetrievAauthorizedData<View_TRS_TIERS>(url);
        }

        internal static async Task<List<View_BSE_COMPTE>> GetCaisses(string type)
        {
            string url = WSApi.CreateLink(App.RestServiceUrl, ServiceUrlDeco.ENCAISSEMENT_URL, ServiceUrlDeco.CAISSES_URL);
            return await RetrievAauthorizedData<View_BSE_COMPTE>(url);
        }

        internal static async Task<View_TRS_ENCAISS> AddEncaissement(View_TRS_ENCAISS encaiss)
        {
            string url = WSApi.CreateLink(App.RestServiceUrl, ServiceUrlDeco.ENCAISSEMENT_URL, ServiceUrlDeco.ADD_ENCAISSEMENT_URL);

            string strdata = JsonConvert.SerializeObject(encaiss);
            byte[] data = Encoding.UTF8.GetBytes(strdata);
            Token tokenInfos = await App.TokenDatabase.GetFirstItemAsync();
            byte[] resultData = await WSApi.ExecutePost(url, data, tokenInfos.access_token);
            string resposeData = Encoding.UTF8.GetString(resultData);
            return JsonConvert.DeserializeObject<View_TRS_ENCAISS>(resposeData);
        }

        internal static async Task<View_TRS_ENCAISS> UpdateEncaissement(View_TRS_ENCAISS encaiss)
        {
            string url = WSApi.CreateLink(App.RestServiceUrl, ServiceUrlDeco.ENCAISSEMENT_URL, ServiceUrlDeco.UPDATE_ENCAISSEMENT_URL);

            string strdata = JsonConvert.SerializeObject(encaiss);
            byte[] data = Encoding.UTF8.GetBytes(strdata);
            Token tokenInfos = await App.TokenDatabase.GetFirstItemAsync();
            byte[] resultData = await WSApi.ExecutePost(url, data, App.User.Token.access_token);
            string resposeData = Encoding.UTF8.GetString(resultData);
            return JsonConvert.DeserializeObject<View_TRS_ENCAISS>(resposeData);
        }

        internal static async Task<bool> DeleteEncaissement(View_TRS_ENCAISS encaiss)
        {
            string url = WSApi.CreateLink(App.RestServiceUrl, ServiceUrlDeco.ENCAISSEMENT_URL, ServiceUrlDeco.DELETE_ENCAISSEMENT_URL);

            string strdata = JsonConvert.SerializeObject(encaiss);
            byte[] data = Encoding.UTF8.GetBytes(strdata);            
            byte[] resultData = await WSApi.ExecutePost(url, data, App.User.Token.access_token);
            string resposeData = Encoding.UTF8.GetString(resultData);
            return JsonConvert.DeserializeObject<bool>(resposeData);
        }
    }
}
