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
        public DataTemplate ProduitVerticalComm { get; set; }
        protected override DataTemplate OnSelectTemplate(object item, BindableObject container)
        {
            if (Constants.AppName == Apps.XCOM_Mob || Constants.AppName == Apps.X_DISTRIBUTION)
            {
                return ProduitsPage.displayGrid ? ProduitVerticalComm : ProduitComm;
            }
            else
            {
                return ProduitPharm;
            }
        }
    }
}
