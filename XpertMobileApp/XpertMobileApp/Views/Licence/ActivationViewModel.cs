

using Acr.UserDialogs;
using System;
using System.Threading.Tasks;
using Xamarin.Forms;
using XpertMobileApp.Api;
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
            Client.DeviceId = DInfos.GetDeviceId();
        }

        internal async Task<Client> ActivateClient()
        {
            try
            {
                if (IsBusy)
                    return null;

                IsBusy = true;

                Client.DeviceId = DInfos.GetDeviceId();
                LicenceInfos lInfos = await WebServiceClient.ActivateClient(Client);

                await App.ClientDatabase.SaveItemAsync(Client);

                App.Settings.ServiceUrl = lInfos.Mobile_Remote_URL;
                await App.SettingsDatabase.SaveItemAsync(App.Settings);

                IsBusy = false;

                return Client;
            }
            catch (XpertException ex)
            {
                if(ex.Code == XpertException.ERROR_XPERT_UNKNOWN)
                {
                    await UserDialogs.Instance.AlertAsync(ex.Message, AppResources.alrt_msg_Alert, AppResources.alrt_msg_Ok);
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