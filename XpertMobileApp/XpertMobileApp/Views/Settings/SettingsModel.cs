using XpertMobileApp.Models;
using System;
using System.Collections.Generic;
using System.Text;
using System.Collections.ObjectModel;
using XpertMobileApp.Model;
using System.Linq;
using System.Threading.Tasks;
using XpertMobileApp.Services;
using Acr.UserDialogs;
using XpertMobileApp.Helpers;
using Xpert.Common.WSClient.Model;
using Xpert.Common.WSClient.Helpers;

namespace XpertMobileApp.ViewModels
{
    public class SettingsModel : BaseViewModel
    {

        public Settings Settings { get => App.Settings; set => App.Settings = value; }

        public ObservableCollection<Language> Languages { get; }

        public bool IsConnected
        {
            get
            {
                return App.User != null;
            }
        }

        public bool IsAdminUser
        {
            get
            {
                return App.User != null;
            }
        }

        public SettingsModel()
        {
            Title = AppResources.pn_Settings;

            Languages = new ObservableCollection<Language>()
            {
                new Language { DisplayName =  "Device language", ShortName = "" },
                new Language { DisplayName =  "عربي", ShortName = "ar" },
                new Language { DisplayName =  "English", ShortName = "en" },
                new Language { DisplayName =  "Français - French", ShortName = "fr" },
                new Language { DisplayName =  "中文 - Chinese (simplified)", ShortName = "zh-Hans" }
            };

            LoadSettings();
        }

        public Language GetLanguageElem(string language)
        {
            Language result = Languages[0];
            if (Languages.Where( e => e.ShortName == language).Count() > 0)
            {
                result = Languages.Where(e => e.ShortName == language).ToList()[0];
            }
            return result;
        }


        public void LoadSettings()
        {
            this.Settings = App.SettingsDatabase.GetFirstItemAsync().Result;
            if (this.Settings == null)
            {
                this.Settings = new Settings();
                // App.Settings = this.Settings;
            }
            this.Settings.isModified = false;           
        }

        public void SaveSettings()
        {
            if (Settings.SubscribedToFBNotifications)
            {
                FireBaseHelper.RegisterUserForDefaultTopics(App.User, App.User.ClientId);
            }
            else
            {
                FireBaseHelper.UnsubscribeFromAllTopics();
            }

            App.SettingsDatabase.SaveItemAsync(Settings);
            this.Settings.isModified = false;
        }

        internal async Task<bool> DeactivateClient()
        {
            try
            {
                if (IsBusy)
                    return false;

                IsBusy = true;

                Client client = App.ClientDatabase.GetFirstItemAsync().Result;

                if (client == null) return false;

                bool result = await WebServiceClient.DeactivateClient(client);
                if (result)
                { 
                    await App.ClientDatabase.DeleteItemAsync(client);
                }

                IsBusy = false;

                return true;
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
                return false;
            }
            finally
            {
                IsBusy = false;
            }
        }
    }
}

