using System;
using System.Collections.Generic;
using System.Text;
using Xamarin.Forms;
using XpertMobileApp.Api;

namespace XpertMobileApp.Views._Templates.TemplateSelector
{

    class RotationDataSelectorTemplate : DataTemplateSelector
    {
        public DataTemplate RotationPharm { get; set; }
        public DataTemplate RotationComm { get; set; }

        protected override DataTemplate OnSelectTemplate(object item, BindableObject container)
        {
            return Constants.AppName == Apps.XCOM_Mob ? RotationComm : RotationPharm;
        }
    }
}
