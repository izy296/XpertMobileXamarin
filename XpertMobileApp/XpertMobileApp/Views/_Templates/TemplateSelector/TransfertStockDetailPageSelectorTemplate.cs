using System;
using System.Collections.Generic;
using System.Text;
using Xamarin.Forms;
using XpertMobileApp.Api;
using XpertMobileApp.DAL;

namespace XpertMobileApp.Views._Templates.TemplateSelector
{
    public class TransfertStockDetailPageSelectorTemplate : DataTemplateSelector
    {
        public DataTemplate TransfertDetailPharm { get; set; }
        public DataTemplate TransfertDetailComm { get; set; }
        protected override DataTemplate OnSelectTemplate(object item, BindableObject container)
        {
            return Constants.AppName == Apps.XCOM_Mob ? TransfertDetailComm : TransfertDetailPharm;
        }
    }
}
