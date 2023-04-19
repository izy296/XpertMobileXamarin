using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xpert.Common.WSClient.Helpers;
using Xpert.Common.WSClient.Model;
using Xpert.Common.WSClient.Services;
using XpertMobileApp.Api.Models;
using XpertMobileApp.DAL;
using XpertMobileApp.Models;
using XpertMobileApp.Services;

namespace XpertMobileApp.Api
{
    internal class ProductManager : CrudService<View_STK_PRODUITS>
    {
        public ProductManager() :
            base(App.RestServiceUrl, ControllerNameSwitch.GetControllerName(ControllerNameItem.STK_PRODUITS), App.User?.Token)
        {

        }

        //function pour la recuperation d'un produit depuis le barrecode peu import
        public async Task<List<View_STK_PRODUITS>> SelectProduitByCodeBarre(string codeBarre)
        {
            
            string url = GetActionUrl("GetByCodeBarre");
            url += WSApi2.AddParam(url, "codebarre", codeBarre);
            return await WSApi2.RetrievAauthorizedData<View_STK_PRODUITS>(url, this.Token.access_token);
        }
        public async Task<View_STK_PRODUITS> GetProductByCodeBarre(string codeBarre)
        {
            string url = GetActionUrl("GetProductByCodeBarre");
            url += WSApi2.AddParam(url, "codebarre", codeBarre);
            return await WSApi2.RetrievAauthorizedValue<View_STK_PRODUITS>(url, this.Token.access_token);
        }

        public async Task<List<View_STK_PRODUITS>> SelectByCodeBarre(string vparam1)
        {
            string url = GetActionUrl("ValidateVente");
            url += WSApi2.AddParam(url, "vparam1", vparam1);

            return await WSApi2.RetrievAauthorizedData<View_STK_PRODUITS>(url, this.Token.access_token);
        }

        public async Task<List<View_BSE_PRODUIT_PRIX_VENTE_BY_QUANTITY>> SelectListePrix()
        {
            string url = GetActionUrl("GeListPriceByQuantity");
            return await WSApi2.RetrievAauthorizedData<View_BSE_PRODUIT_PRIX_VENTE_BY_QUANTITY>(url, this.Token.access_token);
        }

        public async Task<List<View_STK_PRODUITS>> GetProduitFromMagasin(string CodeMagasin)
        {
            string url = GetActionUrl("GetProduitFromMagasin");
            url += WSApi2.AddParam(url, "CodeMagasin", CodeMagasin);
            return await WSApi2.RetrievAauthorizedData<View_STK_PRODUITS>(url, this.Token.access_token);
        }

        public async Task<List<View_STK_PRODUITS>> GetProduitUniteByCode(string CodeMagasin)
        {
            string url = GetActionUrl("GetProduitUniteByCode");
            url += WSApi2.AddParam(url, "codeUnite", CodeMagasin);
            return await WSApi2.RetrievAauthorizedData<View_STK_PRODUITS>(url, this.Token.access_token);
        }

        public async Task<List<BSE_PRODUIT_FAMILLE>> GetProduitFamille()
        {
            try
            {
                var produitFamille = await WebServiceClient.GetProduitFamilles();
                var listeFamilleProduits = produitFamille.Cast<BSE_PRODUIT_FAMILLE>().ToList();
                return listeFamilleProduits;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
    }
}
