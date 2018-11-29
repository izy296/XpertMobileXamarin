using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;
using Xpert.Pharm.DAL;
using XpertMobileApp.Helpers;
using XpertMobileApp.Models;

namespace XpertMobileApp.Views
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class HomePage : ContentPage
    {
        public HomePage()
        {
            InitializeComponent();
        }

        async Task ConnectUserAsync(object sender, EventArgs e)
        {
            await GetSessionInfos();
        }

        internal async Task<List<TRS_JOURNEES>> GetSessionInfos()
        {
            string url = App.RestServiceUrl + ServiceUrlDeco.SESSION_INFO_URL;

            Token tokenInfos = await App.TokenDatabase.GetFirstItemAsync();
            
            List<TRS_JOURNEES> resultData = await WSApi.ExecuteGet<List<TRS_JOURNEES>>(url, App.User.Token.access_token);
            //string resposeData = Encoding.UTF8.GetString(resultData);
            //result = JsonConvert.DeserializeObject<VteServiceResponse>(resposeData);

            return resultData;
        }

    }
}