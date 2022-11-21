using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using Xamarin.Essentials;
using XpertMobileApp.Api.Models;

namespace XpertMobileApp.Views.Helper
{
    public class Manager
    {
        /// <summary>
        /// check if filed of type UrlService is Json 
        /// </summary>
        /// <param name="str"></param>
        /// <returns></returns>
        public static bool isJson(string str)
        {
            try
            {
                var Jobject = JsonConvert.DeserializeObject<List<UrlService>>(str);
                return true;
            }
            catch (Exception e)
            {
                return false;
            }
        }

        /// <summary>
        /// fonction pour la récupération de cordonnée actuelle
        /// </summary>
        /// <returns></returns>
        public static async Task<Location> GetLocation()
        {
            try
            {
                return await Geolocation.GetLocationAsync();  //(request, cts.Token);
            }
            catch (Exception Ex)
            {
                return null;
            }
        }
    }
}
