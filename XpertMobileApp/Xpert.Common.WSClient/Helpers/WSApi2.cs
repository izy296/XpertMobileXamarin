using System;
using System.Net;
using System.Threading.Tasks;
using System.Net.Http;
using System.Net.Http.Headers;
using Newtonsoft.Json;
using System.Text;
using System.Collections.Generic;
using System.IO;
using Xpert.Common.WSClient.Model;

namespace Xpert.Common.WSClient.Helpers
{
    /// <summary>
    /// Class contenant les méthodes d'acces de base au web service
    /// </summary>
    public static class WSApi2
    {
        #region Méthode utilitaire de base

        /// <summary>
        /// Méthode exécutant une méthode get de la web api et retourne une liste d'objets de type T
        /// </summary>
        /// <typeparam name="T"> Le type d'objet retournée</typeparam>
        /// <param name="url">l'url de l'action</param>
        /// <param name="token">le token identifant le client de la webapi</param>
        /// <returns>Une liste d'objets de type T</returns>
        public static async Task<List<T>> RetrievAauthorizedData<T>(string url, string token)
        {
            HttpResponseMessage response = await GetResponse(url, token);
            List<T> result;
            if (response.StatusCode == HttpStatusCode.OK)
            {
                string responseStr = await response.Content.ReadAsStringAsync();
                result = JsonConvert.DeserializeObject<List<T>>(responseStr);
                return result;
            }
            else
            {
                throw GetAppropriateException(response);
            }
        }

        /// <summary>
        /// Méthode exécutant une méthode get de la web api et retourne un objet de type T
        /// </summary>
        /// <typeparam name="T"> Le type d'objet retournée</typeparam>
        /// <param name="url">l'url de l'action</param>
        /// <param name="token">Token identifant le client de la webapi</param>
        /// <returns>Objet de type T</returns>
        public static async Task<T> RetrievAauthorizedValue<T>(string url, string token)
        {
            HttpResponseMessage response = await GetResponse(url, token);
            T result;
            if (response.StatusCode == HttpStatusCode.OK)
            {
                string responseStr = await response.Content.ReadAsStringAsync();
                result = JsonConvert.DeserializeObject<T>(responseStr);
                return result;
            }
            else
            {
                throw GetAppropriateException(response);
            }
        }

        /// <summary>
        /// get method without token 
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="url"></param>
        /// <param name="token"></param>
        /// <returns></returns>
        public static async Task<T> RetrievAauthorizedValueWithoutToken<T>(string url)
        {
            HttpResponseMessage response = await GetResponse(url);
            T result;
            if (response.StatusCode == HttpStatusCode.OK)
            {
                string responseStr = await response.Content.ReadAsStringAsync();
                result = JsonConvert.DeserializeObject<T>(responseStr);
                return result;
            }
            else
            {
                throw GetAppropriateException(response);
            }
        }

        /// <summary>
        /// Méthode exécutant une méthode SET de la web api, envoi un objet de type T1 et retourne un objet de type T
        /// </summary>
        /// <typeparam name="T">Type d'objet retourné</typeparam>
        /// <typeparam name="T1">Type d'objet envoyé</typeparam>
        /// <param name="url">Url de l'action</param>
        /// <param name="data">Objet envoyé</param>
        /// <param name="token">Token identifant le client de la webapi</param>
        /// <returns>Objet de type T</returns>
        public static async Task<T> PostAauthorizedValue<T, T1>(string url, T1 data, string token)
        {
            string strdata = JsonConvert.SerializeObject(data);

            HttpResponseMessage response = await PostResponse(url, strdata, token);

            if (response.StatusCode == HttpStatusCode.OK)
            {
                string responseStr = await response.Content.ReadAsStringAsync();
                T result = JsonConvert.DeserializeObject<T>(responseStr);
                return result;
            }            
            else
            {
                throw GetAppropriateException(response);
            }
        }
        /// <summary>
        /// post without token 
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <typeparam name="T1"></typeparam>
        /// <param name="url"></param>
        /// <param name="data"></param>
        /// <returns></returns>
        public static async Task<T> PostAauthorizedValueWithoutToken<T, T1>(string url, T1 data)
        {
            string strdata = JsonConvert.SerializeObject(data);

            HttpResponseMessage response = await PostResponse(url, strdata);

            if (response.StatusCode == HttpStatusCode.OK)
            {
                string responseStr = await response.Content.ReadAsStringAsync();
                T result = JsonConvert.DeserializeObject<T>(responseStr);
                return result;
            }
            else
            {
                throw GetAppropriateException(response);
            }
        }

        /// <summary>
        /// Méthode exécutant une méthode SET de la web api, envoi un objet de type T comme paramettre
        /// </summary>
        /// <typeparam name="T">Type d'objet envoyé</typeparam>
        /// <param name="url">Url de l'action</param>
        /// <param name="data">Objet envoyé</param>
        /// <param name="token">Token identifant le client de la webapi</param>
        public static async void PostAauthorizedValue<T>(string url, T data, string token)
        {
            string strdata = JsonConvert.SerializeObject(data);

            HttpResponseMessage response = await PostResponse(url, strdata, token);

            if (response.StatusCode != HttpStatusCode.OK)
            {
                throw GetAppropriateException(response);
            }
        }

        /// <summary>
        /// Méthode exécutant une méthode PUT pour la modification d'objet, envoi un objet de type T1 et retourne un objet de type T
        /// </summary>
        /// <typeparam name="T">Type d'objet retourné</typeparam>
        /// <typeparam name="T1">Type d'objet envoyé</typeparam>
        /// <param name="url">Url de l'action</param>
        /// <param name="data">Objet envoyé</param>
        /// <param name="token">Token identifant le client de la webapi</param>
        /// <returns>Objet de type T</returns>
        public static async Task<T> UpdateAauthorizedValue<T, T1>(string url, T1 data, string token)
        {
            string strdata = JsonConvert.SerializeObject(data);

            HttpResponseMessage response = await PutResponse(url, strdata, token);

            if (response.StatusCode == HttpStatusCode.OK)
            {
                string responseStr = await response.Content.ReadAsStringAsync();
                T result = JsonConvert.DeserializeObject<T>(responseStr);
                return result;
            }
            else
            {
                throw GetAppropriateException(response);
            }
        }

        /// <summary>
        /// Méthode exécutant une méthode DELETE pour la suppression d'objet et retourne un objet de type T
        /// </summary>
        /// <typeparam name="T">Type d'objet retourné</typeparam>
        /// <param name="url">Url de l'action</param>
        /// <param name="token">Token identifant le client de la webapi</param>
        /// <returns>Objet de type T</returns>
        public static async Task<T> DeleteAauthorizedValue<T>(string url, string token)
        {
            HttpResponseMessage response = await DeleteResponse(url, token);

            if (response.StatusCode == HttpStatusCode.OK)
            {
                string responseStr = await response.Content.ReadAsStringAsync();
                T result = JsonConvert.DeserializeObject<T>(responseStr);
                return result;
            }
            else
            {
                throw GetAppropriateException(response);
            }
        }

        #endregion

        #region Méthode web service de base

        public static async Task<HttpResponseMessage> GetResponse(string requestUri, string token = null)
        {
            var handler = new HttpClientHandler();
            HttpResponseMessage resp = null;

            try
            {
                using (HttpClient httpClient = new HttpClient(handler))
                {
                    if (!string.IsNullOrEmpty(token))
                    {
                        httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);
                    }
                    httpClient.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));

                    resp = await httpClient.GetAsync(requestUri);

                    if(resp.StatusCode == HttpStatusCode.NotFound) 
                    { 
                       throw new XpertWebException("Url de l'action non trouvée! : " + requestUri, XpertWebException.ERROR_XPERT_UNKNOWN);
                    }
                }
            }
            catch (Exception e)
            {
                throw new XpertWebException(e.Message, XpertWebException.ERROR_XPERT_UNKNOWN);
            }

            return resp;
        }

        public async static Task<HttpResponseMessage> PostResponse(string requestUri, string data, string token = null)
        {
            var handler = new HttpClientHandler();

            HttpResponseMessage resp = null;
            string strResult = string.Empty;

            try
            {
                using (HttpClient httpClient = new HttpClient(handler))
                {
                    if (!string.IsNullOrEmpty(token))
                    {
                        httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);
                    }
                    httpClient.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
                    resp = await httpClient.PostAsync(requestUri, new StringContent(data, Encoding.UTF8, "application/json"));

                    if (resp.StatusCode == HttpStatusCode.NotFound)
                    {
                        throw new XpertWebException("Url de l'action non trouvée! : " + requestUri, XpertWebException.ERROR_XPERT_UNKNOWN);
                    }
                }
            }
            catch (Exception e)
            {
                throw new XpertWebException(e.Message, XpertWebException.ERROR_XPERT_UNKNOWN);
            }
            return resp;
        }

        public async static Task<HttpResponseMessage> PutResponse(string requestUri, string data, string token = null)
        {
            var handler = new HttpClientHandler();

            HttpResponseMessage resp = null;
            string strResult = string.Empty;

            try
            {
                using (HttpClient httpClient = new HttpClient(handler))
                {
                    if (!string.IsNullOrEmpty(token))
                    {
                        httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);
                    }
                    httpClient.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
                    resp = await httpClient.PutAsync(requestUri, new StringContent(data, Encoding.UTF8, "application/json"));

                    if (resp.StatusCode == HttpStatusCode.NotFound)
                    {
                        throw new XpertWebException("Url de l'action non trouvée! : " + requestUri, XpertWebException.ERROR_XPERT_UNKNOWN);
                    }
                }
            }
            catch (Exception e)
            {
                throw new XpertWebException(e.Message, e);
            }
            return resp;
        }

        public async static Task<HttpResponseMessage> DeleteResponse(string requestUri, string token = null)
        {
            var handler = new HttpClientHandler();

            HttpResponseMessage resp = null;
            string strResult = string.Empty;

            try
            {
                using (HttpClient httpClient = new HttpClient(handler))
                {
                    if (!string.IsNullOrEmpty(token))
                    {
                        httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);
                    }
                    httpClient.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
                    resp = await httpClient.DeleteAsync(requestUri);

                    if (resp.StatusCode == HttpStatusCode.NotFound)
                    {
                        throw new XpertWebException("Url de l'action non trouvée! : " + requestUri, XpertWebException.ERROR_XPERT_UNKNOWN);
                    }
                }
            }
            catch (Exception e)
            {
                throw new XpertWebException(e.Message, e);
            }
            return resp;
        }

        private static XpertWebException GetAppropriateException(HttpResponseMessage response)
        {
            string msg;
            try
            {
                ExceptionResponse exceptionResponse = response.ExceptionResponse().Result;
                msg = exceptionResponse.ExceptionMessage;
                return new XpertWebException(msg, exceptionResponse.Code);
            }
            catch
            {
                msg = response.ReasonPhrase;
                return new XpertWebException(msg, XpertWebException.ERROR_XPERT_UNKNOWN);
            }
        }

        public static string GetExceptionMessage(Exception e)
        {
            if (e is XpertWebException)
            {
                XpertWebException ex = (e as XpertWebException);
                return ex.GetMessage();
            }
            else
            {
                return e.Message;
            }
        }

        #endregion

        #region Login

        public async static Task<HttpResponseMessage> ExecuteHttpPostForm(string url, string data = null, string token = null)
        {

            var handler = new HttpClientHandler();

            HttpResponseMessage resp = null;
            string strResult = string.Empty;

            try
            {
                using (HttpClient httpClient = new HttpClient(handler))
                {
                    if (!string.IsNullOrEmpty(token))
                    {
                        httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);
                    }
                    httpClient.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/x-www-form-urlencoded"));
                    resp = await httpClient.PostAsync(url, new StringContent(data));
                }
            }
            catch (HttpRequestException e)
            {
                // return new HttpResponseMessage(HttpStatusCode.ServiceUnavailable);
                throw new WebException(e.InnerException.Message, e);
            }
            return resp;
        }

        public async static Task<Token> Log_in(string url, string username, string password)
        {
            Token token = null;
            try
            {
                using (var client = new HttpClient())
                {
                    var form = new Dictionary<string, string>
                   {
                       {"grant_type", "password"},
                       {"username", username},
                       {"password", password},
                   };

                    HttpResponseMessage tokenResponse = await client.PostAsync(url, new FormUrlEncodedContent(form));

                    if (tokenResponse.StatusCode == HttpStatusCode.OK)
                    {
                        string result = await tokenResponse.Content.ReadAsStringAsync();
                        token = JsonConvert.DeserializeObject<Token>(result);
                        var accessExpiration = DateTime.Now.AddSeconds(token.expires_in);
                        token.expire_Date = accessExpiration;
                        if (string.IsNullOrEmpty(token.error_Descriptjion))
                        {
                            return token;
                        }
                        else
                        {
                            throw new XpertWebException(token.error_Descriptjion, XpertWebException.ERROR_XPERT_INCORRECTPASSWORD);
                        }
                    }
                    else
                    {
                        throw new XpertWebException("BadRequest", XpertWebException.ERROR_XPERT_INCORRECTPASSWORD);
                    }
                }
            }
            catch (Exception e)
            {
                if(e is XpertWebException)
                   throw new XpertWebException(e.Message, XpertWebException.ERROR_XPERT_INCORRECTPASSWORD);
                else if(e is UriFormatException)
                    throw new XpertWebException(e.Message, XpertWebException.ERROR_BAD_URL);
                else
                    throw new XpertWebException(e.Message, XpertWebException.ERROR_XPERT_UNKNOWN);
            }
        }
        
        #endregion

        #region Méthode utilitaire pour la génération d'url

        public static string CreateLink(string BaseUrl, params string[] linkFragment)
        {
            string url = BaseUrl;
            foreach (var item in linkFragment)
            {
                if (!string.IsNullOrEmpty(item))
                { }
                url += item + "/";

            }

            return url;
        }

        public static string GetStartDateQuery(DateTime? date)
        {
            return date?.ToString("yyyyMMdd") + "000000";
        }

        public static string GetEndDateQuery(DateTime? date)
        {
            return date?.ToString("yyyyMMdd") + "235959";
        }

        public static string AddParam(string BaseUrl, string name, string value)
        {
            string param = "";
            string separator = BaseUrl.Contains("?") ? "&" : "?";
            param = string.Format("{0}{1}={2}", separator, name, value);
            return param;
        }
        
        #endregion

        #region Used for activation

        public static async Task<string> PutValue<T>(string url, T data)
        {
            string strdata = JsonConvert.SerializeObject(data);

            HttpResponseMessage response = await PutResponse(url, strdata, null);

            if (response.StatusCode == HttpStatusCode.OK)
            {
                string responseStr = await response.Content.ReadAsStringAsync();
                return responseStr;
            }
            else if (response.StatusCode == HttpStatusCode.BadRequest)
            {
                //  var responseStr = response.Content.ReadAsStringAsync().Result;

                var stream = response.Content.ReadAsStreamAsync().Result;
                StreamReader reader = new StreamReader(stream);
                string text = reader.ReadToEnd();

                // string des = JsonConvert.DeserializeObject(content);

                if (string.IsNullOrEmpty(text) || text.Split('-').Length < 2)
                {
                    throw new XpertWebException(response.ReasonPhrase, XpertWebException.ERROR_XPERT_UNKNOWN);
                }
                string[] str = text.Split('-');
                throw new XpertWebException(str[1], Convert.ToInt32(str[0]) + 20);
            }
            else
            {
                throw new XpertWebException(response.ReasonPhrase, XpertWebException.ERROR_XPERT_UNKNOWN);
            }
        }

        public static async Task<string> PostValue<T>(string url, T data)
        {
            string strdata = JsonConvert.SerializeObject(data);

            HttpResponseMessage response = await PostResponse(url, strdata, null);

            if (response.StatusCode == HttpStatusCode.OK)
            {
                string responseStr = await response.Content.ReadAsStringAsync();
                return responseStr;
            }
            else if (response.StatusCode == HttpStatusCode.BadRequest)
            {
                //  var responseStr = response.Content.ReadAsStringAsync().Result;

                var stream = response.Content.ReadAsStreamAsync().Result;
                StreamReader reader = new StreamReader(stream);
                string text = reader.ReadToEnd();

                // string des = JsonConvert.DeserializeObject(content);

                if (string.IsNullOrEmpty(text) || text.Split('-').Length < 2)
                {
                    throw new XpertWebException(text, XpertWebException.ERROR_XPERT_UNKNOWN);
                }
                string[] str = text.Split('-');
                throw new XpertWebException(str[1], Convert.ToInt32(str[0]) + 30); 
            }
            else
            {
                throw new XpertWebException(response.ReasonPhrase, XpertWebException.ERROR_XPERT_UNKNOWN);
            }
        }
        #endregion
    }
}
