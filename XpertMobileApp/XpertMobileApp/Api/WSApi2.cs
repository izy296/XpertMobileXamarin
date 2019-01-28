using System;
using System.Collections.Specialized;
using System.Net;
using System.Threading.Tasks;
using System.Net.Http;
using System.Net.Http.Headers;
using Newtonsoft.Json;
using System.Text;
using System.Collections.Generic;
using XpertMobileApp.Models;
using System.IO;

namespace XpertMobileApp.Helpers
{
    public static class WSApi2
    {
        // Generic methods
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
                throw new Exception(response.ReasonPhrase);
            }
        }

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
                throw new Exception(response.ReasonPhrase);
            }
        }

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
                throw new XpertException(response.ReasonPhrase, XpertException.ERROR_XPERT_UNKNOWN);
            }
        }

        public static async void PostAauthorizedValue<T>(string url, T data, string token)
        {
            string strdata = JsonConvert.SerializeObject(data);

            HttpResponseMessage response = await PostResponse(url, strdata, token);

            if (response.StatusCode != HttpStatusCode.OK)
            {
                throw new Exception(response.ReasonPhrase);
            }
        }

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
                throw new Exception(response.ReasonPhrase);
            }
        }

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
                throw new Exception(response.ReasonPhrase);
            }
        }

        // Base Methodes
        public static async Task<HttpResponseMessage> GetResponse(string requestUri, string token = null)
        {
            var handler = new HttpClientHandler();

            HttpResponseMessage resp = null;
            using (HttpClient httpClient = new HttpClient(handler))
            {
                if (!string.IsNullOrEmpty(token))
                {
                    httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);
                }
                httpClient.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));

                resp = await httpClient.GetAsync(requestUri);
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
                }
            }
            catch (Exception e)
            {
                // return new HttpResponseMessage(HttpStatusCode.ServiceUnavailable);
                throw new WebException(e.InnerException.Message, e);
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
                }
            }
            catch (Exception e)
            {
                // return new HttpResponseMessage(HttpStatusCode.ServiceUnavailable);
                throw new WebException(e.InnerException.Message, e);
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
                }
            }
            catch (Exception e)
            {
                // return new HttpResponseMessage(HttpStatusCode.ServiceUnavailable);
                throw new WebException(e.InnerException.Message, e);
            }
            return resp;
        }

        // Login
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

                        if (string.IsNullOrEmpty(token.error_Descriptjion))
                        {
                            return token;
                        }
                        else
                        {
                            throw new XpertException(token.error_Descriptjion, XpertException.ERROR_XPERT_INCORRECTPASSWORD);
                        }
                    }
                    else
                    {
                        throw new XpertException("BadRequest", XpertException.ERROR_XPERT_INCORRECTPASSWORD);
                    }
                }
            }
            catch (Exception e)
            {
                if(e is XpertException)
                   throw new XpertException(e.Message, XpertException.ERROR_XPERT_INCORRECTPASSWORD);
                else if(e is UriFormatException)
                    throw new XpertException(e.Message, XpertException.ERROR_BAD_URL);
                else
                    throw new XpertException(e.Message, XpertException.ERROR_XPERT_UNKNOWN);
            }
        }

        // Url generator
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

        public static string AddParam(string BaseUrl, string name, string value)
        {
            string param = "";
            string separator = BaseUrl.Contains("?") ? "&" : "?";
            param = string.Format("{0}{1}={2}", separator, name, value);
            return param;
        }

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
                    throw new XpertException(response.ReasonPhrase, XpertException.ERROR_XPERT_UNKNOWN);
                }
                string[] str = text.Split('-');
                throw new XpertException(str[1], Convert.ToInt32(str[0]) + 20);
            }
            else
            {
                throw new XpertException(response.ReasonPhrase, XpertException.ERROR_XPERT_UNKNOWN);
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
                    throw new XpertException(response.ReasonPhrase, XpertException.ERROR_XPERT_UNKNOWN);
                }
                string[] str = text.Split('-');
                throw new XpertException(str[1], Convert.ToInt32(str[0]) + 30);
            }
            else
            {
                throw new XpertException(response.ReasonPhrase, XpertException.ERROR_XPERT_UNKNOWN);
            }
        }
        #endregion
    }
}
