using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Text;
using System.Threading.Tasks;
using Xpert.Common.WSClient.Helpers;
using Xpert.Common.WSClient.Model;
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
            catch (XpertWebException e)
            {
                throw new XpertWebException(e.Message, e.Code);
            }
        }

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



        #endregion

        #region Manquants

        internal static async Task<List<BSE_DOCUMENT_STATUS>> getManquantsTypes()
        {
            string url = WSApi2.CreateLink(App.RestServiceUrl, ServiceUrlDico.MANQUANTS_URL, ServiceUrlDico.MANQUANTS_TYPES_URL);

            return await RetrievAauthorizedData<BSE_DOCUMENT_STATUS>(url);
        }


        #endregion

        #region Encaissements

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

        public static async Task<List<View_VTE_JOURNAL_DETAIL>> GetVenteDetails(string codeVente)
        {
            string url = WSApi2.CreateLink(App.RestServiceUrl, ServiceUrlDico.VENTES_URL, ServiceUrlDico.VENTES_DETAILS_URL);
            url += WSApi2.AddParam(url, "codeVente", codeVente);

            return await RetrievAauthorizedData<View_VTE_JOURNAL_DETAIL>(url);
        }

        #endregion


        #region 
        public static async Task<List<View_VTE_VENTE_PRODUIT>> GetCommandeDetails(string codeCommande)
        {
            string url = WSApi2.CreateLink(App.RestServiceUrl, ServiceUrlDico.VTE_COMMANDE, ServiceUrlDico.COMMANDE_DETAILS_URL);
            url += WSApi2.AddParam(url, "codeCommande", codeCommande);

            return await RetrievAauthorizedData<View_VTE_VENTE_PRODUIT>(url);
        }
        #endregion

        public static async Task<List<View_ACH_DOCUMENT_DETAIL>> GetAchatsDetails(string codeDoc)
        {
            string url = WSApi2.CreateLink(App.RestServiceUrl, ServiceUrlDico.ACH_ACHATS, ServiceUrlDico.COMMANDE_DETAILS_URL);
            url += WSApi2.AddParam(url, "codeDoc", codeDoc);

            return await RetrievAauthorizedData<View_ACH_DOCUMENT_DETAIL>(url);
        }

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

        public static async Task<List<BSE_TABLE>> GetProduitFamilles()
        {
            string url = WSApi2.CreateLink(App.RestServiceUrl, ServiceUrlDico.PRODUITS_URL, ServiceUrlDico.PRODUITS_FAMILLES_URL);
            return await RetrievAauthorizedData<BSE_TABLE>(url);
        }
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
            return await WSApi2.PostAauthorizedValue< List<View_STK_STOCK_RFID >, List<string>>(url, RFIDs, App.User.Token.access_token);
        }
        internal static async Task<View_STK_INVENTAIRE> getCurentInventaire()
        {
            string url = WSApi2.CreateLink(App.RestServiceUrl, ServiceUrlDico.RFID_URL, ServiceUrlDico.RFID_GET_CURENT_INV);
            
            return await RetrievValAauthorizedData<View_STK_INVENTAIRE>(url);
        }
        internal static async Task<bool> UpdateCurentInventaire(List<View_STK_STOCK_RFID> lots,string modeValidate,string numInve,bool isOuvert)
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
    }
}
