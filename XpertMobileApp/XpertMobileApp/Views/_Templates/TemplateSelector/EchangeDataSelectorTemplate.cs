using System;
using System.Collections.Generic;
using System.Text;
using Xamarin.Forms;
using XpertMobileApp.Api;

namespace XpertMobileApp.Views._Templates.TemplateSelector
{

    class EchangeDataSelectorTemplate : DataTemplateSelector
    {
        public DataTemplate EchangePharm { get; set; }
        public DataTemplate EchangeComm { get; set; }

        protected override DataTemplate OnSelectTemplate(object item, BindableObject container)
        {
            return Constants.AppName == Apps.XCOM_Mob ? EchangePharm : EchangePharm;
        }
    }
}
