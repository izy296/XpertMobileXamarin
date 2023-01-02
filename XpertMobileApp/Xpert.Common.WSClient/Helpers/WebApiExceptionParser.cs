using Newtonsoft.Json;
using System.Net.Http;
using System.Threading.Tasks;
using Xpert.Common.WSClient.Model;

namespace Xpert.Common.WSClient.Helpers
{
    public static class HttpResponseMessageExtension
    {
        /// <summary>
        /// Extension de la class HttpResponseMessage pour la récupération des exceptions retournées par le webservice
        /// </summary>
        /// <param name="httpResponseMessage">La classe HttpResponseMessage retournée par le web service contenant un code de réponse different de 200</param>
        /// <returns></returns>
        public static async Task<ExceptionResponse> ExceptionResponse (this HttpResponseMessage httpResponseMessage)
        {
            string responseContent = await httpResponseMessage.Content.ReadAsStringAsync();
            ExceptionResponse exceptionResponse  =JsonConvert.DeserializeObject<ExceptionResponse>(responseContent);
            return exceptionResponse;
        }
    }
}
