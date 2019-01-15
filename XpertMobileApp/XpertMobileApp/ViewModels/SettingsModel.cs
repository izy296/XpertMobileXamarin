using XpertMobileApp.Models;
using System;
using System.Collections.Generic;
using System.Text;
using System.Collections.ObjectModel;
using XpertMobileApp.Model;
using System.Linq;

namespace XpertMobileApp.ViewModels
{
    public class SettingsModel : BaseViewModel
    {

        public Settings Settings { get => App.Settings; set => App.Settings = value; }

        public ObservableCollection<Language> Languages { get; }

        public bool IsAdminUser
        {
            get
            {
                return App.User != null;
            }
        }

        public SettingsModel()
        {
            Title = "Configuration";

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
            App.SettingsDatabase.SaveItemAsync(Settings);
            this.Settings.isModified = false;
        }
    }
}

