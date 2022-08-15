using System;
using System.Collections.Generic;
using System.Text;
using Xamarin.Forms;
using XpertMobileApp.Api;

namespace XpertMobileApp.Views._Templates.TemplateSelector
{
    public class AchatEnteteDataSelectorTemplate : DataTemplateSelector
    {
        public DataTemplate AchatEntetePharm { get; set; }
        public DataTemplate AchatEnteteComm { get; set; }
        protected override DataTemplate OnSelectTemplate(object item, BindableObject container)
        {
            return Constants.AppName == Apps.XCOM_Mob ? AchatEnteteComm : AchatEntetePharm;
        }
    }
}
