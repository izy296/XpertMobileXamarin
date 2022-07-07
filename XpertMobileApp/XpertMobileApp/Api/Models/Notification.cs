using System;
using System.Collections.Generic;
using System.Text;

namespace XpertMobileApp.Api.Models
{
    public class Notification
    {

        public Guid Index = Guid.NewGuid();
        public string Title { get; set; }
        public string Message { get; set; }
        public string Module { get; set; }

        public int ModuleId
        {
            get
            {
                Enum.TryParse(Module, out MenuItemType res);
                return Convert.ToInt32(res);
            }
        }
        public string User { get; set; }
        public DateTime TimeNotification { get; set; }

    }
}
