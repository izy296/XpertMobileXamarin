using System;
using System.Collections.Generic;
using System.Text;

namespace XpertMobileApp.Views._04_Comercial.Selectors.Settings
{
    class SettingsSelectorViewModel
    {
        public string Titre { get; set; } = "";
        public string Url { get; set; } = "";

        public SettingsSelectorViewModel(string titre = "", string url = "")
        {
            this.Titre = titre;
            this.Url = url;
        }
    }
}
