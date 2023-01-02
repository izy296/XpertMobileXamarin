using SQLite;
using System.Collections.Generic;
using System.Threading.Tasks;
using Xpert.Common.DAO;
using Xpert.Common.WSClient.Helpers;
using Xpert.Common.WSClient.Model;

namespace Xpert.Common.WSClient.Services
{
    /// <summary>
    /// Classe contenant les actions Webservice de base
    /// </summary>
    /// <typeparam name="T">La classe échangé entre le mobile et le WebService, dans la plus part des cas, son nom  indique le nom du controleur correspendant</typeparam>
    public class CrudService<T> : ICurdService<T>
    {
        protected Token Token;
        private string ControleurName;
        private string BaseUrl;
         
        // Nom des méthodes de base du controleur de base BaseControler 
        private const string AN_ADD = "AddItem";
        private const string AN_UPDATE = "UpdateItem";
        private const string AN_SELECT_ALL = "SelectAll";
        private const string AN_SELECT_BY_ID = "SelectObject";
        private const string AN_SELECT_SUM = "SelectSum";
        private const string AN_SELECT_SUMS = "SelectSumuaries";
        private const string AN_SELECT_BY_PAGES = "SelectByPages";
        private const string AN_SELECT_COUNTS = "SelectCounts";

        public CrudService(string baseUrl, string controleurName, Token token)
        {
            BaseUrl = baseUrl;
            Token = token;
            ControleurName = controleurName;
        }
        /// <summary>
        /// crud service without token we need it if we have to get from server without authentification
        /// </summary>
        /// <param name="baseUrl"></param>
        /// <param name="controleurName"></param>
        public CrudService(string baseUrl, string controleurName)
        {
            BaseUrl = baseUrl;
            ControleurName = controleurName;
        }

        /// <summary>
        /// Méthode qui génere l'url complete pour une méthode du web service donnée
        /// </summary>
        /// <param name="actionName">Nom de la méthode WebService</param>
        /// <returns></returns>
        public string GetActionUrl(string actionName)
        {
            return WSApi2.CreateLink(BaseUrl, ControleurName, actionName);
        }

        /// <summary>
        /// Méthode qui récupère le nombre d'objets remplissant une lise de conditions QueryInfos
        /// </summary>
        /// <param name="filter">Conditions a remplir pour la liste d'objets retournée</param>
        /// <returns></returns>
        public async Task<int> ItemsCount(QueryInfos filter)
        {
            string url = WSApi2.CreateLink(BaseUrl, ControleurName, AN_SELECT_COUNTS);
            url += WSApi2.AddParam(url, "param11", "1");
            return await WSApi2.PostAauthorizedValue<int, QueryInfos>(url, filter, Token?.access_token);
        }

        /// <summary>
        /// Sélection de tous les objets du de la class T
        /// </summary>
        /// <returns></returns>
        /// 
        public async Task<IEnumerable<T>> GetItemsAsync()
        {
            string url = WSApi2.CreateLink(BaseUrl, ControleurName, AN_SELECT_ALL);
            return await WSApi2.RetrievAauthorizedData<T>(url, Token?.access_token);
        }

        public async Task<IEnumerable<T>> GetItemsAsync(QueryInfos filter)
        {
            string url = WSApi2.CreateLink(BaseUrl, ControleurName, AN_SELECT_ALL);
            return await WSApi2.RetrievAauthorizedData<T>(url, Token?.access_token);
        }

        public async Task<IEnumerable<T>> GetItemsAsyncWithUrl(string MethodName,string param)
        {
            string url = WSApi2.CreateLink(BaseUrl, ControleurName, MethodName);
            if (!(param == ""))
            {
                url = url.Remove(url.Length - 1);
                url = url + "?" + param;
            }
            return await WSApi2.RetrievAauthorizedData<T>(url, Token?.access_token);
        }

        /// <summary>
        /// Méthode qui récupère l'objet de la class T ayant la valeur id
        /// </summary>
        /// <param name="id">Valeur de la clé de l'objet</param>
        /// <returns></returns>
        public async Task<T> GetItemAsync(string id)
        {
            string url = WSApi2.CreateLink(BaseUrl, ControleurName, AN_SELECT_BY_ID);
            url += WSApi2.AddParam(url, "codeObject", id);
            return await WSApi2.RetrievAauthorizedValue<T>(url, Token?.access_token);
        }

        /// <summary>
        /// Méthode qui récupère une liste d'objets remplissant une lise de conditions QueryInfos
        /// </summary>
        /// <param name="filter">Conditions a remplir pour la liste d'objets retournée</param>
        /// <param name="page">Numéro de page</param>
        /// <param name="count">Nombre d'élément par page</param>
        /// <returns></returns>
        public async Task<IEnumerable<T>> SelectByPage(QueryInfos filter, int page, int count)
        {
            string url = WSApi2.CreateLink(BaseUrl, ControleurName, AN_SELECT_BY_PAGES);
            url += WSApi2.AddParam(url, "page", page.ToString());
            url += WSApi2.AddParam(url, "count", count.ToString());
            url += WSApi2.AddParam(url, "param12", "1");
            return await WSApi2.PostAauthorizedValue<IEnumerable<T>, QueryInfos>(url, filter, Token?.access_token);
        }

      



        public async Task<decimal> ItemsSum(QueryInfos filter)
        {
            string url = WSApi2.CreateLink(BaseUrl, ControleurName, AN_SELECT_SUM);
            url += WSApi2.AddParam(url, "paramS", "1");
            return await WSApi2.PostAauthorizedValue<decimal, QueryInfos>(url, filter, Token?.access_token);
        }
        
        /// <summary>
        /// Méthode qui récupère une liste de totaux d'objets remplissant une lise de conditions QueryInfos
        /// </summary>
        /// <param name="filter">Conditions a remplir pour la liste d'objets retournée</param>
        /// <returns></returns>
        public async Task<SortedDictionary<string, decimal>> ItemsSums(QueryInfos filter)
        {
            string url = WSApi2.CreateLink(BaseUrl, ControleurName, AN_SELECT_SUMS);
            url += WSApi2.AddParam(url, "paramQIS", "1");
            return await WSApi2.PostAauthorizedValue<SortedDictionary<string, decimal>, QueryInfos>(url, filter, Token?.access_token);
        }

        public async Task<string> AddItemAsync(T item)
        {
            string url = WSApi2.CreateLink(BaseUrl, ControleurName, AN_ADD);
            return await WSApi2.PostAauthorizedValue<string, T>(url, item, Token?.access_token);
        }

        public async Task<bool> UpdateItemAsync(T item)
        {
            string url = WSApi2.CreateLink(BaseUrl, ControleurName, AN_UPDATE);
            return await WSApi2.UpdateAauthorizedValue<bool, T>(url, item, Token?.access_token);
        }

        public async Task<bool> DeleteItemAsync(string id)
        {
            string url = WSApi2.CreateLink(BaseUrl, ControleurName, "Remove");
            url += WSApi2.AddParam(url, "codeObject", id);
            return await WSApi2.PostAauthorizedValue<bool, string>(url, id, Token?.access_token);
        }

       
    }
}
