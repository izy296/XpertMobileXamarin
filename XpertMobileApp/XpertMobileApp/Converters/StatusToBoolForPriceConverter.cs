using System;
using System.Globalization;
using System.Linq;
using Xamarin.Forms;
using XpertMobileApp.Api;
using XpertMobileApp.DAL;

namespace XpertMobileApp.Converters
{
    public class StatusToBoolForPriceConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (AppManager.HasAdmin) return true;

            View_ACH_DOCUMENT obj = (View_ACH_DOCUMENT)value;

            if (obj == null) return false;

            if ((obj.STATUS_DOC == DocStatus.EnAttente || obj.STATUS_DOC == DocStatus.EnCours || obj.STATUS_DOC == DocStatus.Termine) && HasPermission)
            {
                return true;
            }
            else
            {
                return false;
            }
        }

        public bool HasPermission
        {
            get
            {
                bool result = false;
                if (AppManager.permissions != null)
                {
                    var obj = AppManager.permissions.Where(x => x.CodeObjet == "ACH_UPDATE_PRIX_HT").FirstOrDefault();
                    result = obj != null && obj.AcUpdate > 0;
                }
                return result;
            }
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotSupportedException("Only one way bindings are supported with this converter");
        }
    }
}
