using System;
using System.Globalization;
using Xamarin.Forms;
using XpertMobileApp.DAL;

namespace XpertMobileApp.Converters
{
    public class StatusToBoolForPriceConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            View_ACH_DOCUMENT obj = (View_ACH_DOCUMENT)value;
            if(obj == null) return false;
            if (obj.STATUS_DOC == DocStatus.Termine || obj.STATUS_DOC == DocStatus.EnCours)
            {
                return true;
            }
            else
            {
                return false;
            }
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotSupportedException("Only one way bindings are supported with this converter");
        }
    }
}
