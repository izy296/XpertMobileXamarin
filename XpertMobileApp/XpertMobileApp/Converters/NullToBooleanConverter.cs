using System;
using System.Globalization;
using System.Text.RegularExpressions;
using Xamarin.Forms;
using Xamarin.Forms.Internals;
using XpertMobileApp.Controls;

namespace XpertMobileApp.Converters
{
    /// <summary>
    /// This class have methods to convert the string values to boolean.     
    /// </summary>
    [Preserve(AllMembers = true)]
    public class NullToBooleanConverter : IValueConverter
    {
        /// <summary>
        /// This method is used to convert the object nullable to boolean.
        /// </summary>
        /// <param name="value">The value</param>
        /// <param name="targetType">The target</param>
        /// <param name="parameter">The parameter</param>
        /// <param name="culture">The culture</param>
        /// <returns>The result</returns>
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value == null)
            {
                return false;
            }
            else return true;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            return null;
        }
    }
}