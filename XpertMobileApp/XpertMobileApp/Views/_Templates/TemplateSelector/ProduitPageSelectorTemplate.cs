using System;
using System.Collections.Generic;
using System.Text;
using Xamarin.Forms;
using XpertMobileApp.Api;
using XpertMobileApp.DAL;

namespace XpertMobileApp.Views._Templates.TemplateSelector
{
    public class ProduitPageSelectorTemplate : DataTemplateSelector
    {
        public DataTemplate ProduitPharm { get; set; }
        public DataTemplate ProduitComm { get; set; }
        protected override DataTemplate OnSelectTemplate(object item, BindableObject container)
        {
            return Constants.AppName == Apps.XCOM_Mob || Constants.AppName == Apps.X_DISTRIBUTION ? ProduitComm : ProduitPharm;
        }
    }
}
