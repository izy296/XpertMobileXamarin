using System;
using System.Globalization;
using Xamarin.Forms;

namespace XpertMobileApp.Converters
{
    public class MultipleColorConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            int index = (int)value;
            int moduloResult = index % 2;
            switch (moduloResult)
            {
                case 0: return Color.FromHex("#01B7AB");
                case 1: return Color.FromHex("#FEA901");
                case 2: return Color.FromHex("#5FAD01");//vert;
                default: return Color.FromHex("#01B7AB");
            }

        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotSupportedException("Only one way bindings are supported with this converter");
        }
    }
}
