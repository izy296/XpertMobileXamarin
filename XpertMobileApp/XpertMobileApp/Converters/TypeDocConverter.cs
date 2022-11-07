using System;
using System.Collections.Generic;
using System.Globalization;
using System.Text;
using Xamarin.Forms;

namespace XpertMobileApp.Converters
{
    internal class TypeDocConverter : IValueConverter
    {

        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if ((string)value == "ENC")
            {
                return "Encaissement";
            }
            if ((string)value == "BL")
            {
                return "Bon de Livraison";
            }
            if ((string)value == "BR")
            {
                return "Bon de Retour";
            }
            if ((string)value == "CC")
            {
                return "Commande";
            }
            else return "";
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if ((string)value == "Encaissement")
            {
                return "ENC";
            }
            if ((string)value == "Bon de Livraison")
            {
                return "BL";
            }
            if ((string)value == "Bon de Retour")
            {
                return "BR";
            }
            if ((string)value == "Commande")
            {
                return "CC";
            }
            else return "";
        }
    }
}
