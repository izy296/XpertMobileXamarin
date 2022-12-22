using System;
using System.Collections.Generic;
using System.Text;
using Xamarin.Forms;
using XpertMobileApp.Api;
using XpertMobileApp.DAL;

namespace XpertMobileApp.Views._Templates.TemplateSelector
{
    public class TransfertStockPageSelectorTemplate : DataTemplateSelector
    {
        public DataTemplate TransfertPharm { get; set; }
        public DataTemplate TransfertComm { get; set; }
        protected override DataTemplate OnSelectTemplate(object item, BindableObject container)
        {
            return Constants.AppName == Apps.XCOM_Mob || Constants.AppName == Apps.X_DISTRIBUTION ? TransfertComm : TransfertPharm;
        }
    }
}
