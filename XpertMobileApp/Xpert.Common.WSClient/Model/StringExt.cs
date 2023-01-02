using System;
using System.Collections.Generic;
using System.Text;

namespace Xpert.Common.WSClient.Model
{
    public static class StringExt
    {
        public static string Truncate(this string value, int maxLength)
        {
            if (string.IsNullOrEmpty(value)) return value;
            return value.Trim().Length <= maxLength ? value.Trim() : (value.Trim().Substring(0, maxLength) + " ...");
        }
    }
}
