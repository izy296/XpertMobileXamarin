using System;
using System.Globalization;
using System.Linq;
using Xamarin.Forms;
using XpertMobileApp.Api;
using XpertMobileApp.DAL;

namespace XpertMobileApp.Converters
{
    public class StatusToColor : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {

            Color res;
            if (value.ToString() == DocStatus.EnAttente)
                res = Color.OrangeRed;
            else if (value.ToString() == DocStatus.EnCours)
                res = Color.Blue;
            else if (value.ToString() == DocStatus.Accepter)
                res = Color.Green;
            else if (value.ToString() == DocStatus.Rejeter)
                res = Color.Red;
            else res = Color.Black;

            return res;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotSupportedException("Only one way bindings are supported with this converter");
        }
    }
}
