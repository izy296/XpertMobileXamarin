using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using Xpert.Common.WSClient.Helpers;
using XpertMobileApp.DAL;
using XpertMobileApp.Helpers;

namespace XpertMobileApp.Api
{
    public static class BoutiqueManager
    {
        public static string Token
        {
            get { return App.User.Token.access_token; }
        }

        public static List<View_PANIER> PanierElem { get; set; }

        public static async Task<List<BSE_TABLE_TYPE>> GetProduitTypes()
        {
            string url = WSApi2.CreateLink(App.RestServiceUrl, "Produits", "GetProduitsTypes");
            return await WSApi2.RetrievAauthorizedData<BSE_TABLE_TYPE>(url, Token);
        }

        public static async Task<View_PRODUITS_DETAILS> GetProduitDetail(string codeprod)
        {
            string url = WSApi2.CreateLink(App.RestServiceUrl, "Produits", "GetProductDetails");
            url += WSApi2.AddParam(url, "CODE_PRODUCT", codeprod);
            url += WSApi2.AddParam(url, "ID_USER", App.User.Id);
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
            url += WSApi2.AddParam(url, "codeUser", App.User.Id);
            return await WSApi2.RetrievAauthorizedData<View_PANIER>(url, Token);
        }

        internal static async Task<string> AddCartItem(CartItem item)
        {
            string url = WSApi2.CreateLink(App.RestServiceUrl, "PANIER", "Ajout_Produit");
            var result = await WSApi2.PostAauthorizedValue<string, CartItem>(url, item, Token);
            return result;
        }

        internal static async Task<string> RemoveCartItem(string codeProd)
        {
            string url = WSApi2.CreateLink(App.RestServiceUrl, "PANIER", "REMOVE_Produit");
            url += WSApi2.AddParam(url, "codeDoc", codeProd);
            return await WSApi2.PostAauthorizedValue<string, string>(url, codeProd, Token);
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

        internal static async Task<List<COMMANDES_DETAILS>> GetCommandeDetails(string codeOrder)
        {
            string url = WSApi2.CreateLink(App.RestServiceUrl, "COMMANDES", "COMMANDE_DETAIL");
            url += WSApi2.AddParam(url, "codeDoc", codeOrder);
            return await WSApi2.RetrievAauthorizedData<COMMANDES_DETAILS>(url, Token);
        }

    }
}
