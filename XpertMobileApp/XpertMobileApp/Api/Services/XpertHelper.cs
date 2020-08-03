using Acr.UserDialogs;
using Plugin.SimpleAudioPlayer;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using Xamarin.Essentials;
using Xamarin.Forms;
using XpertMobileApp.DAL;
using XpertMobileApp.Services;
using XpertWebApi.Models;

namespace XpertMobileApp.Api.Services
{
    public static class XpertHelper
    {
        public static void PeepScan() 
        {
            try
            {
                ISimpleAudioPlayer player = Plugin.SimpleAudioPlayer.CrossSimpleAudioPlayer.Current;
                player.Load(GetStreamFromFile("beep07.mp3"));
                player.Play();
            }
            catch { }
        }

        public static string GetMachineName() 
        {
            string machineName = DeviceInfo.Name;
            if (DeviceInfo.Idiom != null)
            {
                machineName += "-" + DeviceInfo.Idiom.ToString();
            }
            machineName += "-" + DeviceInfo.Manufacturer;

            return machineName;
        }

        static Stream GetStreamFromFile(string filename)
        {
            var assembly = typeof(App).GetTypeInfo().Assembly;
            var stream = assembly.GetManifestResourceStream("XpertMobileApp." + filename);
            return stream;
        }

        private static Random random = new Random();
        public static string RandomString(int length)
        {
            const string chars = "ABCDEFGHIJKLMNOPQRSTUVWXYZ0123456789";
            return new string(Enumerable.Repeat(chars, length)
              .Select(s => s[random.Next(s.Length)]).ToArray());
        }
        public static async Task<bool> PrintQrCode(View_PRD_AGRICULTURE doc, int qte)
        {
            return await PrintQrCode(doc.CODE_TIERS, doc.NOM_TIERS, doc.CODE_DOC, doc.DATE_DOC, qte, App.Settings.PrinterName, App.Settings.PrinterType);
        }

        public static async Task<bool> PrintQrCode(View_ACH_DOCUMENT doc, int qte)
        {
            return await PrintQrCode(doc.CODE_TIERS, doc.TIERS_NomC, doc.CODE_DOC, doc.DATE_DOC, qte, App.Settings.PrinterName, App.Settings.PrinterType);
        }

        public static async Task<bool> PrintQrCode(string codeTiers, string NomTiers, string codeDoc, DateTime? dateDoc,
            int qte, string printerName, string printerType)
        {
            bool result = false;
            if (printerType == Printer_Type.Bluetooth)
            {
                //IBlueToothService _blueToothService = DependencyService.Get<IBlueToothService>();
                //await _blueToothService.Print(printerName, printerName);

                string adr = GodexManager.GetPrinterAdress(printerName);
                GodexManager.PrintQrCode(adr, 1, codeTiers, NomTiers, codeDoc, dateDoc);

                result = true;
            }
            else if (printerType == Printer_Type.Wifi)
            {
                await UserDialogs.Instance.AlertAsync("L'impression pour les impémantes wifi n'est pas encor imlémentée", AppResources.alrt_msg_Alert,
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

        public static bool IsNullOrEmpty(object value)
        {
            if (value is System.DBNull) return true;
            else if (value == null) return true;
            else if (value is IList)
            {
                return (value as IList).Count == 0;
            }
            else if (string.IsNullOrEmpty(value.ToString())) return true;
            else if (value.Equals("<Tous>")) return true;
            else if (value.Equals("<Auto>")) return true;
            return false;
        }
        public static bool IsNotNullAndNotEmpty(object value)
        {
            return !IsNullOrEmpty(value);
        }
        public static bool IsNullOrWhiteSpace(object value)
        {
            if (value is System.DBNull) return true;
            else if (value == null) return true;
            else if (string.IsNullOrWhiteSpace(value.ToString())) return true;
            return false;
        }

        public static string GetSQLDate(object value)
        {
            return GetSQLDate(value, "yyyyMMdd");
        }
        public static string GetSQLDate(object value, string format)
        {
            if (XpertHelper.IsNullOrWhiteSpace(value)) return null;

            if (value is string) return XpertHelper.GetDateTime(value.ToString()).Value.ToString(format);
            else if (value is DateTime)
            {
                return ((DateTime)value).ToString(format, CultureInfo.InvariantCulture);
            }
            return null;
        }
        public static string GetSQLDateTime(object value, string format)
        {
            if (XpertHelper.IsNullOrEmpty(value)) return null;
            return ((DateTime)value).ToString(format);
        }
        public static object GetSqlValue(object value)
        {
            if (value is DateTime) return GetSQLDate(value);
            if (value == null) return "";
            if ((value is decimal) || (value is float) || (value is double))
            {
                decimal val = Math.Round(Convert.ToDecimal(value), 2);
                return val.ToString("0.00").Replace(",", ".");
            }
            return value;
        }

        public static bool IsNull(object value)
        {
            if (value is System.DBNull) return true;
            else if (value == null) return true;
            return false;
        }

        public static bool IsNotNull(object value)
        {
            return !IsNull(value);
        }

        public static bool IsEmptyCode(object code)
        {
            return XpertHelper.IsNullOrEmpty(code) || "<Auto>".Equals(code);
        }

        public static string GetPropertyName<TModel, TProperty>(Expression<Func<TModel, TProperty>> property)
        {
            MemberExpression memberExpression = (MemberExpression)property.Body;
            return memberExpression.Member.Name;
        }

        public static DateTime? GetDateTime(object date)
        {
            return GetDateTime(date, "yyyyMMdd");
        }
        public static DateTime? GetDateTime(object date, string format)
        {
            try
            {
                if (date == null) return null;

                if ((date is DateTime?) || (date is DateTime)) return Convert.ToDateTime(date);
                if (date is string)
                {
                    string ff = date.ToString().Trim();
                    if (string.IsNullOrWhiteSpace(ff)) return null;
                    try
                    {
                        return Convert.ToDateTime(date);
                    }
                    catch
                    {
                        try
                        {
                            return DateTime.ParseExact(ff, format, CultureInfo.InvariantCulture);
                        }
                        catch
                        {
                            return null;
                        }
                    }


                }
                return null;
            }
            catch (Exception ex)
            {

                return null;
            }
        }

        public static string GetValues(object valueField, char separator)
        {
            return GetValues(valueField, separator, true);
        }
        public static string GetValues(object valueField, char separator, bool quote)
        {
            string res = "";
            bool first = true;
            if (valueField is IEnumerable<string>)
            {
                var enumerator = ((IEnumerable<string>)valueField).GetEnumerator();
                while (enumerator.MoveNext())
                {
                    string value = enumerator.Current;
                    if (!XpertHelper.IsNullOrEmpty(value))
                    {
                        if (first)
                        {
                            if (quote) res = "'" + value + "'";
                            else res = value;
                            first = false;
                        }
                        else
                        {
                            if (quote) res += separator + "'" + value + "'";
                            else res += separator + value;

                        }
                    }
                }
            }
            else if (valueField is string)
            {
                res = XpertHelper.GetString(valueField);
            }
            return res;
        }
        public static string GetString(object value)
        {
            if (value == null) return "";
            if ((value is decimal) || (value is float) || (value is double))
            {
                decimal val = Math.Round(Convert.ToDecimal(value), 2);
                return val.ToString("n2");
            }
            return Convert.ToString(value);
        }

        public static string GetTableName<T>()
        {
            return typeof(T).Name;
        }
    }
}
