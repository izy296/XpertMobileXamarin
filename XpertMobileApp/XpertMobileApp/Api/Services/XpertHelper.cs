using Acr.UserDialogs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using Xamarin.Forms;
using XpertMobileApp.DAL;
using XpertMobileApp.Services;
using XpertWebApi.Models;

namespace XpertMobileApp.Api.Services
{
    public static class XpertHelper
    {
        public static async Task<bool> PrintQrCode(string codeDoc, int qte)
        {
            return await PrintQrCode(codeDoc, qte, App.Settings.PrinterName, App.Settings.PrinterType);
        }
        
        public static async Task<bool> PrintQrCode(string codeDoc,int qte, string printerName, string printerType) 
        {
            bool result = false;
            if (printerType == Printer_Type.Bluetooth)
            {
                IBlueToothService _blueToothService = DependencyService.Get<IBlueToothService>();
                await _blueToothService.Print(printerName, printerName);
                result = true;
            }
            else if (printerType == Printer_Type.Wifi)
            {
                await UserDialogs.Instance.AlertAsync("L'impression pour les impémantes wifi n'est pas encor imlémenté", AppResources.alrt_msg_Alert,
AppResources.alrt_msg_Ok);
                return false;
            }
            else
            {
                result = await WebServiceClient.PrintQRProduit(codeDoc, 1, printerName);
            }
            return result;
        }

        public static void UpdateItemIndex<T>(List<T> items)
        {
            int i = 0;
            foreach (var item in items)
            {
                i += 1;
                (item as BASE_CLASS).Index = i;
            }
        }

        public static void CopyPropertiesTo<T, TU>(this T source, TU dest)
        {
            var sourceProps = typeof(T).GetProperties().Where(x => x.CanRead).ToList();
            var destProps = typeof(TU).GetProperties()
                    .Where(x => x.CanWrite)
                    .ToList();

            foreach (var sourceProp in sourceProps)
            {
                if (destProps.Any(x => x.Name == sourceProp.Name))
                {
                    var p = destProps.First(x => x.Name == sourceProp.Name);
                    if (p.CanWrite)
                    { // check if the property can be set or no.
                        p.SetValue(dest, sourceProp.GetValue(source, null), null);
                    }
                }

            }
        }


        public static T1 CloneObject<T1>(object oldObject)
        {
            if (oldObject == null) return default(T1);
            T1 newObject = Activator.CreateInstance<T1>();
            XpertHelper.CloneObject(oldObject, newObject);
            return newObject;
        }

        public static void CloneObject(object oldObject, object newObject)
        {
            XpertHelper.GetEmptyObject(newObject);
            PropertyInfo[] propCloned = oldObject.GetType().GetProperties();
            foreach (PropertyInfo pi in propCloned)
            {
                object value = pi.GetValue(oldObject, null);
                XpertHelper.SetValueInObject(newObject, pi.Name, value);
            }
        }

        public static object GetEmptyObject(object obj)
        {
            if (obj == null) return obj;
            PropertyInfo[] properties = obj.GetType().GetProperties();
            foreach (PropertyInfo pi in properties)
            {
                if (pi.CanWrite)
                {
                    pi.SetValue(obj, null, null);
                }
            }
            return obj;
        }

        public static void SetValueInObject(object obj, string key, object value)
        {
            if (obj == null) return;
            PropertyInfo[] properties = obj.GetType().GetProperties();
            foreach (PropertyInfo pi in properties)
            {
                if (pi.Name.Equals(key))
                {
                    if (pi.CanWrite)
                    {
                        value = XpertHelper.ConvertValue(pi.PropertyType, value);
                        pi.SetValue(obj, value, null);
                        return;
                    }
                }
            }
        }

        private static object ConvertValue(Type type, object value)
        {
            if (value == null) return value;
            if (value.GetType().Equals(type)) return value;
            System.TypeCode typeCode = Type.GetTypeCode(type);
            switch (typeCode)
            {
                case TypeCode.Boolean:
                    try
                    {
                        return Convert.ToBoolean(value);
                    }
                    catch (Exception ex)
                    {
                        throw ex;
                    }


                case TypeCode.String:
                    try
                    {
                        return Convert.ToString(value);
                    }
                    catch (Exception ex)
                    {
                        throw ex;
                    }
                case TypeCode.Int16:
                    try
                    {
                        return Convert.ToInt16(value);
                    }
                    catch (Exception ex)
                    {
                        throw ex;
                    }
                case TypeCode.Int32:
                    try
                    {
                        return Convert.ToInt32(value);
                    }
                    catch (Exception ex)
                    {

                        throw ex;
                    }
                case TypeCode.Int64:
                    try
                    {
                        return Convert.ToInt64(value);
                    }
                    catch (Exception ex)
                    {

                        throw ex;
                    }
                case TypeCode.Decimal:
                    try
                    {
                        return Convert.ToDecimal(value);
                    }
                    catch (Exception ex)
                    {

                        throw ex;
                    }
                case TypeCode.Char:
                    try
                    {
                        return Convert.ToChar(value);
                    }
                    catch (Exception ex)
                    {
                        throw ex;
                    }
                default:
                    try
                    {
                        return value;
                    }
                    catch (Exception ex)
                    {
                        throw ex;
                    }
            }
        }
    }
}
