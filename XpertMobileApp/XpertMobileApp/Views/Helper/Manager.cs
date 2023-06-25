using Acr.UserDialogs;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xamarin.Essentials;
using XpertMobileApp.Api.Models;
using XpertMobileApp.Models;

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
        /// check if a url service ending with "/" else append one at the end and return it
        /// </summary>
        /// <param name="urlService"></param>
        /// <returns></returns>
        public static string UrlServiceFormatter(string urlService)
        {
            string urlServiceFormatted = urlService;
            if (urlServiceFormatted != null)
            {
                if (urlServiceFormatted.Last() != '/')
                {
                    urlServiceFormatted = string.Concat(urlServiceFormatted, '/');
                }
            }
            else throw new Exception("urlService is NUll");

            return urlServiceFormatted;
        }

        /// <summary>
        /// fonction pour la récupération de cordonnée actuelle
        /// </summary>
        /// <returns></returns>
        public static async Task<Location> GetLocation()
        {
            try
            {
                GeolocationRequest geolocationRequest = new GeolocationRequest(accuracy: GeolocationAccuracy.Medium, timeout: TimeSpan.FromSeconds(3));
                return await Geolocation.GetLocationAsync(geolocationRequest);  //(request, cts.Token);
            }
            catch (Exception Ex)
            {
                return null;
            }
        }
        #region Unite de Mesure
        /// <summary>
        /// fonction pour calculer la quantite d'une unite de mesure
        /// </summary>
        /// <param name="unite"></param>
        /// <returns></returns>
        public static decimal QuantiteUnitetoQuantite(View_BSE_PRODUIT_AUTRE_UNITE unite)
        {
            return unite.SelectedQUANTITE * unite.COEFFICIENT;
        }

        /// <summary>
        /// fonction pour calculer la quantite d'une liste des unites de mesures
        /// </summary>
        /// <param name="list"></param>
        /// <returns></returns>
        public static decimal TotalQuantiteUnite(List<View_BSE_PRODUIT_AUTRE_UNITE> list, bool multiplication = true)
        {
            if (list != null)
            {
                decimal total = 0;
                foreach (var item in list)
                    if (multiplication)
                        total += QuantiteUnitetoQuantite(item);
                    else
                        total += item.SelectedQUANTITE;
                return total;
            }
            else
            {
                return 0;
            }
        }

        /// <summary>
        /// fonction pour donner la quantite d'une liste des unites de mesure sous forme de string pour l'affichage dans la page
        /// </summary>
        /// <param name="list"></param>
        /// <returns></returns>
        public static string TotalQuantiteUniteString(List<View_BSE_PRODUIT_AUTRE_UNITE> list)
        {
            string totalstring = "";
            if (list != null)
            {
                foreach (var item in list)
                {
                    if (totalstring != "")
                        totalstring += "\n";
                    totalstring += item.SelectedQUANTITE + " " + item.DESIGNATION_UNITE + "( x" + item.COEFFICIENT + " )";
                }
                return totalstring;
            }
            else
            {
                return totalstring;
            }
        }

        /// <summary>
        /// fonction pour vider la quantite dans une liste des unites de mesure
        /// </summary>
        /// <param name="list"></param>
        public static void ClearUnitesList(List<View_BSE_PRODUIT_AUTRE_UNITE> list)
        {
            foreach (var item in list)
            {
                item.SelectedQUANTITE = 0;
            }
        }
        #endregion
    }
}
