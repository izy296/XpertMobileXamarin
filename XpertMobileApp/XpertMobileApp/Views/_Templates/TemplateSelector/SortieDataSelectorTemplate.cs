using System;
using System.Collections.Generic;
using System.Text;
using Xamarin.Forms;

namespace XpertMobileApp.Views._Templates.TemplateSelector
{
    public class SortieDataSelectorTemplate : DataTemplateSelector
    {
        public DataTemplate Sortie { set; get; }

        protected override DataTemplate OnSelectTemplate(object item, BindableObject container)
        {
            return Sortie;
        }
    }
}
