using System;
using System.Collections.Generic;
using System.Text;
using Xamarin.Forms;
using XpertMobileApp.Api;

namespace XpertMobileApp.Views._Templates.TemplateSelector
{

    class AchatDetailsSelectorTemplate : DataTemplateSelector
    {
        public DataTemplate AchatDetailsPharm { get; set; }
        public DataTemplate AchatDetailsComm { get; set; }

        protected override DataTemplate OnSelectTemplate(object item, BindableObject container)
        {
            return Constants.AppName == Apps.XPH_Mob ? AchatDetailsPharm : AchatDetailsComm;
        }
    }
}
