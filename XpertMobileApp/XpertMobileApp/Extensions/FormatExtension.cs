using System;
using System.Collections.Generic;
using System.Text;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace XpertMobileApp.Helpers
{
    [ContentProperty("StaticResourceKey")]
    public class FormatExtension : IMarkupExtension
    {
        public string StringFormat { get; set; }
        public string StaticResourceKey { get; set; }

        public object ProvideValue(IServiceProvider serviceProvider)
        {
            string toReturn = null;
            if (serviceProvider == null)
                throw new ArgumentNullException(nameof(serviceProvider));

            if (StaticResourceKey != null)
            {
                var staticResourceExtension = new StaticResourceExtension { Key = StaticResourceKey };

                toReturn = (string)staticResourceExtension.ProvideValue(serviceProvider);
                if (!string.IsNullOrEmpty(StringFormat))
                    toReturn = string.Format(StringFormat, toReturn);
            }

            return toReturn;
        }
    }
}
