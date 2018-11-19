using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using XpertMobileApp.Models;

namespace XpertMobileApp.Services
{
    public class WebServiceClient
    {
        public async Task<byte[]> ExecutePost(string url, byte[] data = null, string token = null)
        {
            try
            {
                byte[] result;
                using (WebClient client = new WebClient())
                {
                    if (!string.IsNullOrEmpty(token))
                    {
                        client.Headers.Add(HttpRequestHeader.Authorization, "Bearer " + token);
                    }
                    client.Headers.Set("Content-Type", "text/json");
                    result = await client.UploadDataTaskAsync(new Uri(url), data);
                }
                return result;
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        public async Task<byte[]> ExecutePost(string url, NameValueCollection data = null, string token = null)
        {
            try
            {
                byte[] result;
                using (WebClient client = new WebClient())
                {
                    if (!string.IsNullOrEmpty(token))
                    {
                        client.Headers.Add(HttpRequestHeader.Authorization, "Bearer " + token);
                    }
                    client.Headers.Set("Content-Type", "text/json");
                    result = await client.UploadValuesTaskAsync(new Uri(url), data);
                }
                return result;
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        public byte[] ExecutePostForm(string url, NameValueCollection data = null, string token = null)
        {
            byte[] result;

            using (WebClient client = new WebClient())
            {
                // Ajout du token d'identification si il est présent
                if (!string.IsNullOrEmpty(token))
                {
                    client.Headers.Add(HttpRequestHeader.Authorization, "Bearer " + token);
                }

                // Type de contenu du header le la requete
                client.Headers.Set("Content-Type", "application/x-www-form-urlencoded");

                // Envoir de la requete et récupération de la réponse
                result = client.UploadValues(url, data);
            }
            return result;
        }

        public Token Login(string username, string password)
        {
            try
            {
                using (WebClient client = new WebClient())
                {
                    // url de l'authentification et du recupération du token
                    string url = App.RestServiceUrl + "Token";

                    // Preparation des paramettres
                    NameValueCollection values = new NameValueCollection();
                    values.Add("username", username);
                    values.Add("password", password);
                    values.Add("grant_type", "password");

                    byte[] result = this.ExecutePostForm(url, values);
                    Token tokenInfos = JsonConvert.DeserializeObject<Token>(Encoding.UTF8.GetString(result));
                    DateTime dt = new DateTime();
                    dt = DateTime.Today;
                    tokenInfos.expire_Date = dt.AddSeconds(tokenInfos.expires_in);
                    return tokenInfos;
                }
            }
            catch (WebException e)
            {
                throw new WebException("Un problème est survenu lors de la tentative de connexion : " + e.Message, e);
            }
        }

        public async Task<T> ExecuteGet<T>(string url, string token = null) where T : class
        {
            string jsonResult = "";
            T result = null;
            using (WebClient clt = new WebClient())
            {
                if (!string.IsNullOrEmpty(token))
                {
                    clt.Headers.Add(HttpRequestHeader.Authorization, "Bearer " + token);
                }
                try
                {
                    clt.Headers.Set("Content-Type", "text/json");
                    jsonResult = await clt.DownloadStringTaskAsync(new Uri(url));

                    result = JsonConvert.DeserializeObject<T>(jsonResult);
                }
                catch (Exception e)
                {
                    throw new Exception(e.Message);
                    // TODO : log error
                }
            }
            return result;
        }

    }
}
