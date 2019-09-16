using System;
using System.Globalization;
using Xamarin.Forms;

namespace XpertMobileApp.Converters
{
    public class BordereauStatusToColor : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            bool val = (bool)value;

            if (val) 
                return Color.FromHex("#ecfbd5"); // #2ecc71
            else
                return Color.White;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotSupportedException("Only one way bindings are supported with this converter");
        }
    }
}
