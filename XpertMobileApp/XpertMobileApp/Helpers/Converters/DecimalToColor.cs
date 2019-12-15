using System;
using System.Globalization;
using Xamarin.Forms;

namespace XpertMobileApp.Converters
{
    public class DecimalToColor : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            decimal val = (decimal)value;

            if (val > 0) 
                return Color.OrangeRed;
            else if(val < 0)
                return Color.Green;
            else
                return Color.Black;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotSupportedException("Only one way bindings are supported with this converter");
        }
    }
}
