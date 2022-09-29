using System;
using System.Collections.Generic;
using System.Text;
using Xamarin.Forms;
using XpertMobileApp.Api;

namespace XpertMobileApp.Views._Templates.TemplateSelector
{
    public class SortieDataSelectorTemplate : DataTemplateSelector
    {
        public DataTemplate SortiePharm { set; get; }
        public DataTemplate SortieComm { set; get; }
        protected override DataTemplate OnSelectTemplate(object item, BindableObject container)
        {
            return Constants.AppName == Apps.XCOM_Mob ? SortieComm : SortiePharm;
        }
    }
}
