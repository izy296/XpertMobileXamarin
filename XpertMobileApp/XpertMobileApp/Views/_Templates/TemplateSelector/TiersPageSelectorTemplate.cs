using System;
using System.Collections.Generic;
using System.Text;
using Xamarin.Forms;
using XpertMobileApp.Api;

namespace XpertMobileApp.Views._Templates.TemplateSelector
{

    class TiersPageSelectorTemplate : DataTemplateSelector
    {
        public DataTemplate Tiers { get; set; }

        protected override DataTemplate OnSelectTemplate(object item, BindableObject container)
        {
            return Tiers;
        }
    }
}
