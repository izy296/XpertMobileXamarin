using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using Xpert.Common.WSClient.Helpers;
using XpertMobileApp.DAL;
using XpertMobileApp.Helpers;
using XpertMobileApp.Models;

namespace XpertMobileApp.Api
{
    public static class BoutiqueManager
    {
        public static string Token
        {
            get { return App.User?.Token?.access_token; }
        }

        public static List<View_PANIER> PanierElem { get; set; } = new List<View_PANIER>();

        public static async Task<List<BSE_TABLE_TYPE>> GetProduitTypes()
        {
            string url = WSApi2.CreateLink(App.RestServiceUrl, "Produits", "GetProduitsTypes");
            return await WSApi2.RetrievAauthorizedData<BSE_TABLE_TYPE>(url, Token);
        }

        public static async Task<View_PRODUITS_DETAILS> GetProduitDetail(string codeprod)
        {
            string url = WSApi2.CreateLink(App.RestServiceUrl, "Produits", "GetProductDetails");
            url += WSApi2.AddParam(url, "CODE_PRODUCT", codeprod);
            url += WSApi2.AddParam(url, "ID_USER", App.User.Token.userID);
            return await WSApi2.RetrievAauthorizedValue<View_PRODUITS_DETAILS>(url, Token);
        }

        public static async Task<List<BSE_TABLE>> GetProduitFamilles()
        {
            string url = WSApi2.CreateLink(App.RestServiceUrl, "Produits", "GetProduitsFamilles");
            return await WSApi2.RetrievAauthorizedData<BSE_TABLE>(url, Token);
        }

        internal static async Task<List<View_PANIER>> GetPanier()
        {
            string url = WSApi2.CreateLink(App.RestServiceUrl, "PANIER", "Afficher_Panier");
            url += WSApi2.AddParam(url, "codeUser", App.User.Token.userID);
            return await WSApi2.RetrievAauthorizedData<View_PANIER>(url, Token);
        }

        internal static async Task<string> AddCartItem(addToCard item)
        {
            string url = WSApi2.CreateLink(App.RestServiceUrl, "PANIER", "Ajout_Produit");
            url += WSApi2.AddParam(url, "operation", "0");
            var result = await WSApi2.PostAauthorizedValue<string, addToCard>(url, item, Token);
            return result;
        }

        internal static async Task<string> RemoveCartItem(string codeProduit)
        {
            string url = WSApi2.CreateLink(App.RestServiceUrl, "PANIER", "REMOVE_Produit");
            url += WSApi2.AddParam(url, "codeUser", App.User.Token.userID);
            url += WSApi2.AddParam(url, "codeProduit", codeProduit);
            return await WSApi2.PostAauthorizedValue<string, string>(url, codeProduit, Token);
        }

        internal static async Task<string> AddOrder(Order order)
        {
            string url = WSApi2.CreateLink(App.RestServiceUrl, "COMMANDES", "ADD_ORDER");
            return await WSApi2.PostAauthorizedValue<string, Order > (url, order, Token);
        }

        internal static async Task<string> RemoveOrder(string codeOrder)
        {
            string url = WSApi2.CreateLink(App.RestServiceUrl, "COMMANDES", "REMOVE_ORDER");
            url += WSApi2.AddParam(url, "codeDoc", codeOrder);
            return await WSApi2.PostAauthorizedValue<string, string>(url, codeOrder, Token);
        }

        internal static async Task<List<View_COMMANDES_DETAILS>> GetCommandeDetails(string codeOrder)
        {
            string url = WSApi2.CreateLink(App.RestServiceUrl, "COMMANDES", "GetCmdDetails");
            url += WSApi2.AddParam(url, "codeDoc", codeOrder);
            return await WSApi2.RetrievAauthorizedData<View_COMMANDES_DETAILS>(url, Token);
        }

        internal static async Task<bool> SetProducEval(Evaluation note)
        {
            string url = WSApi2.CreateLink(App.RestServiceUrl, "EVALUATION", "ADD_COMMENT_NOTE");
            //url += WSApi2.AddParam(url, "codeDoc", codeOrder);
            return await WSApi2.PostAauthorizedValue<bool, Evaluation>(url, note, Token);
        }

        internal static async Task<HOME_INFOS> GetHomeProducts()
        {
            string url = WSApi2.CreateLink(App.RestServiceUrl, "Produits", "GetHomeProducts");
            url += WSApi2.AddParam(url, "codeUser", App.User?.Token?.userID);
            return await WSApi2.RetrievAauthorizedValue<HOME_INFOS>(url, Token);
        }

        internal static async Task<object> Subscribe(RegisterBindingModel registerInfos)
        {
            string url = WSApi2.CreateLink(App.RestServiceUrl, "Account", "Register");
            //url += WSApi2.AddParam(url, "codeDoc", codeOrder);
            object res =  await WSApi2.PostAauthorizedValue<object, RegisterBindingModel>(url, registerInfos, null);
            return res;
        }

        internal static async Task<Product> LoadProdDetails (string codeProd) 
        {
            var pDetails = await GetProduitDetail(codeProd);

            Product p = new Product()
            {
                Id = pDetails.CODE_PRODUIT,
                Name = pDetails.DESIGNATION,
                Category = pDetails.DESIGNATION_FAMILLE,
                Price = pDetails.PRIX_VENTE,
                Description = pDetails.DESCRIPTION,
                ReviewValue = pDetails.NOTE,
                UserReviewValue = pDetails.NOTE_USER,
                IMAGE_URL = pDetails.IMAGE_URL
            };

            List<string> listImgurl = new List<string>();

            // Création des urls des images du produit
            if (pDetails.ImageList != null)
            {
                foreach (var str in pDetails.ImageList)
                {
                    string val = App.RestServiceUrl.Replace("api/", "") + string.Format("Images/GetImage?codeImage={0}", str);
                    listImgurl.Add(val);
                }
            }
            p.ImageList = listImgurl;

            return p;
        }
    }
}
