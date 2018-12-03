using Newtonsoft.Json;
using System;
using System.Collections.Specialized;
using System.Net;
using System.Threading.Tasks;

namespace XpertMobileApp.Helpers
{
    public static class WSApi
    {
        public static async Task<byte[]> ExecutePost(string url, byte[] data = null, string token = null)
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

        public static async Task<byte[]> ExecutePost(string url, NameValueCollection data = null, string token = null)
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

        public static byte[] ExecutePostForm(string url, NameValueCollection data = null, string token = null)
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

        public static async Task<T> ExecuteGet<T>(string url, string token = null) where T : class
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
    }
}
