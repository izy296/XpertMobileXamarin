using System;
using System.Collections.Generic;
using System.Text;
using Xamarin.Forms;
using XpertMobileApp.Api;
using XpertMobileApp.DAL;

namespace XpertMobileApp.Views._Templates.TemplateSelector
{
    public class ProduitDataSelectorTemplate : DataTemplateSelector
    {
        public DataTemplate LotPharm { get; set; }
        public DataTemplate LotComm { get; set; }
        protected override DataTemplate OnSelectTemplate(object item, BindableObject container)
        {
            return Constants.AppName == Apps.XCOM_Mob ? LotComm : LotPharm;
        }
    }
}
