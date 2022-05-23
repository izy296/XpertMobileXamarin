

using Acr.UserDialogs;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Xamarin.Forms;
using Xpert.Common.WSClient.Helpers;
using Xpert.Common.WSClient.Model;
using XpertMobileApp.Api;
using XpertMobileApp.Api.Models;
using XpertMobileApp.Api.Services;
using XpertMobileApp.Data;
using XpertMobileApp.Helpers;
using XpertMobileApp.Models;
using XpertMobileApp.Services;

namespace XpertMobileApp.ViewModels
{
    public class ActivationViewModel : BaseViewModel
    {
        public IDeviceInfos DInfos = DependencyService.Get<IDeviceInfos>();

        public Client Client { get; set; }

        public ActivationViewModel()
        {
            Client = new Client();
            Client.DeviceId = DInfos.GetSecureOsId();
        }

        internal async Task<Client> ActivateClient()
        {
            try
            {
                if (IsBusy)
                    return null;

                IsBusy = true;

                Client.DeviceId = DInfos.GetSecureOsId();
                /*
                string val = DInfos.GetSecureOsId();
                val = DInfos.GetSerial();
                val = DInfos.GetImei();
                val = DInfos.GetSimSerialNumber();
                */
                Client.AppName = Constants.AppName;
                LicenceInfos lInfos = await WebServiceClient.ActivateClient(Client);

                await App.ClientDatabase.SaveItemAsync(Client);
                // set Settings Service url as json when activating the app at the first time
                string value = lInfos.Mobile_Remote_URL;
                UrlService url = new UrlService();
                if (!string.IsNullOrEmpty(value))
                {
                    url = new UrlService
                    {
                        DisplayurlService = lInfos.Mobile_Remote_URL,
                        Selected = true
                    };
                }
                List<UrlService> liste = new List<UrlService>();
                liste.Add(url);

                App.Settings.ServiceUrl = JsonConvert.SerializeObject(liste); ;
                App.Settings.ClientName = !string.IsNullOrEmpty(lInfos.ClientName) ? lInfos.ClientName : "-";
                App.Settings.ClientId = lInfos.ClientId;
                App.Settings.Mobile_Edition = lInfos.Mobile_Edition;
                await App.SettingsDatabase.SaveItemAsync(App.Settings);

                IsBusy = false;

                return Client;
            }
            catch (XpertWebException ex)
            {
                if (ex.Code == XpertWebException.ERROR_XPERT_UNKNOWN)
                {
                    await UserDialogs.Instance.AlertAsync(WSApi2.GetExceptionMessage(ex), AppResources.alrt_msg_Alert,
                        AppResources.alrt_msg_Ok);
                }
                else
                {
                    string msgKey = string.Format("Exception_errMsg_{0}", ex.Code);
                    await UserDialogs.Instance.AlertAsync(TranslateExtension.GetTranslation(msgKey), AppResources.alrt_msg_Alert,
                    AppResources.alrt_msg_Ok);
                }
                return null;
            }
            finally
            {
                IsBusy = false;
            }
        }

        internal async Task<DateTime> GetEndDate(string licenceTxt)
        {
            return await LicActivator.GetLicenceEndDate(licenceTxt);
        }
    }
}