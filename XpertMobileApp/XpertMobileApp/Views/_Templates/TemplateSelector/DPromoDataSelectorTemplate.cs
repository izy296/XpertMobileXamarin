using System;
using System.Collections.Generic;
using System.Text;
using Xamarin.Forms;

namespace XpertMobileApp.Views._Templates.TemplateSelector
{
    internal class DPromoDataSelectorTemplate : DataTemplateSelector
    {
        public DataTemplate DisplayPromo { get; set; }
        protected override DataTemplate OnSelectTemplate(object item, BindableObject container)
        {
            return DisplayPromo;
        }
    }
}
