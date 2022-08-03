using System;
using System.Collections.Generic;
using System.Text;
using Xamarin.Forms;

namespace XpertMobileApp.Views._Templates.TemplateSelector
{
    internal class CommandeDataSelectorTemplate : DataTemplateSelector
    {
        public DataTemplate Commande { get; set; }
        protected override DataTemplate OnSelectTemplate(object item, BindableObject container)
        {
            return Commande;
        }
    }
}
