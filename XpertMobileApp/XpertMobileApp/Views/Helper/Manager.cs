using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Text;
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
    }
}
