using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using XpertMobileApp.Helpers;
using XpertMobileApp.Models;

namespace XpertMobileApp.Services
{
    public class WebServiceClient
    {
        public Token Login(string username, string password)
        {
            try
            {
                using (WebClient client = new WebClient())
                {
                    // url de l'authentification et du recupération du token
                    string url = App.RestServiceUrl + ServiceUrlDeco.TOKEN_URL;

                    // Preparation des paramettres
                    NameValueCollection values = new NameValueCollection();
                    values.Add("username", username);
                    values.Add("password", password);
                    values.Add("grant_type", "password");

                    byte[] result = WSApi.ExecutePostForm(url, values);
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


    }
}
