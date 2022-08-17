using System;
using System.Collections.Generic;
using System.Text;
using Xamarin.Forms;

namespace XpertMobileApp.Api.Models
{
    public class Notification
    {

        public Guid Index = Guid.NewGuid();
        public string Title { get; set; }
        public string Message { get; set; }
        public string Module { get; set; }
        public object Extras { get; set; }

        public int ModuleId
        {
            get
            {
                if (Enum.TryParse(Module, out MenuItemType res))
                    return Convert.ToInt32(res);
                else return 0;
            }
        }

        public string ColorM
        {
            get
            {

                if (Application.Current.Resources.TryGetValue($"cl_{ModuleId}", out var result))
                {
                    Color c = (Color)result;
                    return c.ToHex();
                }
                else
                {
                    var c = (Color)Application.Current.Resources["cl_0"];
                    return c.ToHex();
                }
            }
        }

        public string User { get; set; }
        public DateTime TimeNotification { get; set; }

        public bool IsUnRead
        {
            get; set;
        } = true;

    }
}
