

using Acr.UserDialogs;
using Newtonsoft.Json;
using Rg.Plugins.Popup.Services;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Xamarin.Essentials;
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
using XpertMobileApp.Views;

namespace XpertMobileApp.ViewModels
{
    public class ActivationViewModel : BaseViewModel
    {
        public IDeviceInfos DInfos = DependencyService.Get<IDeviceInfos>();

        public Client Client { get; set; }
        private bool phoneFormatCorrect { get; set; } = false;
        public bool PhoneFormatCorrect
        {
            get
            {
                return phoneFormatCorrect;
            }
            set
            {
                phoneFormatCorrect = value;
            }
        }
        private bool emailFormatCorrect { get; set; } = false;
        public bool EmailFormatCorrect
        {
            get
            {
                return emailFormatCorrect;
            }
            set
            {
                emailFormatCorrect = value;
            }
        }
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
                Client.Mobile_Version = VersionTracking.CurrentVersion;
                Client.LicenceTxt = String.Empty;
                LicenceInfos lInfos = await WebServiceClient.ActivateClient(Client);

                //Stocké les donnés du client ( deviceId, AppName, Mobile_version, license_info ) dans la table local Client ...
                await App.ClientDatabase.SaveItemAsync(Client);

                // set Settings Service url as json when activating the app at the first time
                UrlService url = new UrlService();
                if (!string.IsNullOrEmpty(lInfos.Mobile_Remote_URL))
                {
                    url = new UrlService
                    {
                        DisplayUrlService = lInfos.Mobile_Remote_URL,
                        Selected = true,
                        Title = Constants.AppName == Apps.XPH_Mob ? "Pharmacie" : "Entreprise"
                    };
                }
                List<UrlService> liste = new List<UrlService>();
                liste.Add(url);

                App.Settings.ServiceUrl = JsonConvert.SerializeObject(liste); ;
                App.Settings.ClientName = !string.IsNullOrEmpty(lInfos.ClientName) ? lInfos.ClientName : "-";
                App.Settings.ClientId = lInfos.ClientId;
                App.Settings.Mobile_Edition = lInfos.Mobile_Edition;

                // Sauvgarder les données ( ServiceUrl, ClientName, ClientId, Mobile_Edition ) dans l'objet App.Settings...
                await App.SettingsDatabase.SaveItemAsync(App.Settings);

                IsBusy = false;

                return Client;
            }
            catch (XpertWebException ex)
            {
                if (ex.Code == XpertWebException.ERROR_XPERT_UNKNOWN)
                {
                    CustomPopup AlertPopup = new CustomPopup(WSApi2.GetExceptionMessage(ex), trueMessage: AppResources.alrt_msg_Ok);
                    await PopupNavigation.Instance.PushAsync(AlertPopup);
                }
                else
                {
                    string msgKey = string.Format("Exception_errMsg_{0}", ex.Code);
                    CustomPopup AlertPopup = new CustomPopup(TranslateExtension.GetTranslation(msgKey), trueMessage: AppResources.alrt_msg_Ok);
                    await PopupNavigation.Instance.PushAsync(AlertPopup);
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