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

        internal static async Task<List<Get_Print_VTE_TiketCaisse>> GetDataTecketCaisseVente(string res)
        {
            string url = WSApi2.CreateLink(App.RestServiceUrl, ServiceUrlDico.VENTES_URL, ServiceUrlDico.TIKET_CAISS_PRINT_VENTE_URL);
            url += "?codeVente=" + res;
            return await RetrievAauthorizedData<Get_Print_VTE_TiketCaisse>(url);
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

        public static async Task<List<View_VTE_VENTE_LOT>> GetVenteLotDetails(string codeVente)
        {
            string url = WSApi2.CreateLink(App.RestServiceUrl, ServiceUrlDico.VENTES_URL, ServiceUrlDico.VENTES_DETAILS_URL);
            url += WSApi2.AddParam(url, "codeVente", codeVente);

            return await RetrievAauthorizedData<View_VTE_VENTE_LOT>(url);
        }

        public static async Task<List<BSE_DOCUMENTS_TYPE>> GetVenteTypes()
        {
            string url = WSApi2.CreateLink(App.RestServiceUrl, ServiceUrlDico.VENTES_URL, ServiceUrlDico.VENTES_TYPES_URL);

            return await RetrievAauthorizedData<BSE_DOCUMENTS_TYPE>(url);
        }

        public static async Task<List<View_VTE_VENTE_LOT>> GetCommandeDetails(string codeCommande)
        {
            string url = WSApi2.CreateLink(App.RestServiceUrl, ServiceUrlDico.VTE_COMMANDE, ServiceUrlDico.COMMANDE_DETAILS_URL);
            url += WSApi2.AddParam(url, "codeCommande", codeCommande);

            return await RetrievAauthorizedData<View_VTE_VENTE_LOT>(url);
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
        /// <summary>
        /// pour la recupérarion des détails d'un sortie de stock
        /// </summary>
        /// <param name="code"></param>
        /// <returns></returns>
        internal static async Task<List<View_STK_SORTIE_DETAIL>> getSortieDetails(string code)
        {
            string url = WSApi2.CreateLink(App.RestServiceUrl, ServiceUrlDico.STOCK_URL, ServiceUrlDico.STOCK_GET_STOCK_SORTIE);
            url += WSApi2.AddParam(url, "code", code);
            return await RetrievAauthorizedData<View_STK_SORTIE_DETAIL>(url);
        }
        /// <summary>
        /// pour la recupérarion des Motifs d'un sortie de stock
        /// </summary>
        /// <returns></returns>
        internal static async Task<List<View_STK_SORTIE>> getSortieMotifs()
        {
            string url = WSApi2.CreateLink(App.RestServiceUrl, ServiceUrlDico.STOCK_URL, ServiceUrlDico.STOCK_GET_STOCK_MOTIFS);
            return await RetrievAauthorizedData<View_STK_SORTIE>(url);
        }
        #endregion

        #region User
        /// <summary>
        /// pour la recupérarion des IDs de utilisateur d'un sortie de stock
        /// </summary>
        /// <returns></returns>
        internal static async Task<List<View_SYS_USER>> getUserIDs()
        {
            string url = WSApi2.CreateLink(App.RestServiceUrl, ServiceUrlDico.USER_URL, ServiceUrlDico.USER_GET_IDS);
            return await RetrievAauthorizedData<View_SYS_USER>(url);

        #endregion
        }

    }
}