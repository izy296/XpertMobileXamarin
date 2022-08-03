using System;
using System.Collections.Generic;
using System.Text;
using Xamarin.Forms;

namespace XpertMobileApp.Views._Templates.TemplateSelector
{

    class AchatDataSelectorTemplate : DataTemplateSelector
    {
        public DataTemplate Achat { get; set; }

        protected override DataTemplate OnSelectTemplate(object item, BindableObject container)
        {
            return Achat;
        }
    }
}
