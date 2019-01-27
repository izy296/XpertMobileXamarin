using System;
using System.Diagnostics;
using System.Text;

namespace XpertMobileApp.Models
{
    public class XpertException : Exception
    {

        public string Module { get; set; }

        public int Code { get; set; }

        public object[] Args { get; set; }

        // Module list
        public static string MODULE_XPERTCOM = "XpertCom";

        public static string MODULE_XPERT_AUTHENTIFICATION = "Authentification";

        public static string MODULE_XPERT_PRIVILEGE = "Priviléges";

        
        // Error list
        public const int ERROR_XPERT_UNKNOWN = 0;

        public const int ERROR_XPERT_INCORRECTPASSWORD = 1;

        public const int ERROR_XPERT_PARAMS_EMAIL_EMPTY = 2;

        public const int ERROR_XPERT_PARAMS_WILLAYA_EMPTY = 3;

        public const int ERROR_XPERT_ROW_DUPLICATED = 4;

        public const int ERROR_XPERT_ACTION_CANCELED = 5;

        public const int ERROR_XPERT_STOCK_INSUFFICIENT = 6;

        public const int ERROR_XPERT_DATEPEREMPTION_ERROR = 7;

        public const int ERROR_XPERT_INVALIDELICENCE = 8;
        
        public const int ERROR_ACCESS_DENIED = 9;

        public const int ERROR_BAD_URL = 10;


        // error activation

        public const int ERROR_OPTION_DENIED = 20;

        public const int ERROR_PRODUCT_OR_CLIENT_NOTFOUND = 21;

        public const int ERROR_NBR_LICENCES_EXPRIRED = 22;

        public const int ERROR_ALREADY_REGISTRED = 23;



        public XpertException(string module, int code, string message, Exception e, object[] args)
            : base(message, e)
        {
            Module = module;
            Code = code;
            Args = args;

            if (e != null)
            {
                Debug.WriteLine(e.Message + " " + GetMessage());
            }
        }

        public XpertException(Exception e)
            : this(MODULE_XPERTCOM, ERROR_XPERT_UNKNOWN, "", e, null)
        {

        }

        public XpertException(string message)
            : this(MODULE_XPERTCOM, ERROR_XPERT_UNKNOWN, message, null, null)
        {

        }

        public XpertException(string message, Exception e)
            : this(MODULE_XPERTCOM, ERROR_XPERT_UNKNOWN, message, e, null)
        {

        }

        public XpertException(string message, int code)
            : this(MODULE_XPERTCOM, code, message, null, null)
        {

        }

        public string GetMessage()
        {
            StringBuilder buffer = new StringBuilder();

            buffer.Append("Error number ");
            buffer.Append(Code);
            buffer.Append(" in ");
            buffer.Append(this.Module);

            string message = base.Message;
            if (message != null)
            {
                buffer.Append(": ");
                if (this.Args == null)
                {
                    buffer.Append(message);
                }
                else
                {
                    try
                    {
                        buffer.Append(string.Format(message, this.Args));
                    }
                    catch
                    {
                        buffer.Append("Cannot format message [" + message + "] with args ");
                        for (int i = 0; i < this.Args.Length; i++)
                        {
                            if (i != 0)
                            {
                                buffer.Append(",");
                            }
                            buffer.Append(this.Args[i]);
                        }
                    }
                }
            }
            return buffer.ToString();
        }

        public string GetFullMessage()
        {
            StringBuilder buffer = new StringBuilder(GetMessage());
            buffer.Append("\n");
            buffer.Append(this.StackTrace);
            buffer.Append("\n");

            return buffer.ToString();
        }
    }
}
