using System;
using System.Collections.Generic;
using System.Text;
using Xamarin.Forms;
using XpertMobileApp.DAL;

namespace XpertMobileApp.Views._Templates.TemplateSelector
{
    public class VenteDataSelectorTemplate : DataTemplateSelector
    {
        public DataTemplate VenteNormal { get; set; }
        public DataTemplate VentePsychotrope { get; set; }
        protected override DataTemplate OnSelectTemplate(object item, BindableObject container)
        {
            return ((VTE_VENTE_DETAIL)item).PSYCHOTHROPE == 1 ? VentePsychotrope : VenteNormal;
        }
    }
}
