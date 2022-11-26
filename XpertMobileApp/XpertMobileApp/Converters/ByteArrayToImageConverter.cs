using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using Xamarin.Forms;

namespace XpertMobileApp.Converters
{
    public class ByteArrayToImageConverter : IValueConverter
    {
        public View ConverterParameter { get; set; }
        public static byte[] StringToByteArray(string hex)
        {
            return Enumerable.Range(0, hex.Length)
                             .Where(x => x % 2 == 0)
                             .Select(x => System.Convert.ToByte(hex.Substring(x, 2), 16))
                             .ToArray();
        }
        public object Convert(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            if (value == null)
                return null;

            string stringValue = (string)value;
            try
            {
                if (!stringValue.Contains("GetImage"))
                {
                    var imageAsBytes = StringToByteArray(stringValue.Replace("-",""));
                    return ImageSource.FromStream(() => new MemoryStream(imageAsBytes));
                }
                else return value;
            }
            catch
            {
                return value;
            }
        }

        public object ConvertBack(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture) =>
                throw new NotImplementedException();
    }
}
