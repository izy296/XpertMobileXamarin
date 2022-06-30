using System;
using System.Collections.Generic;
using System.Text;

namespace XpertMobileApp.Api.Models
{
    public class Notification
    {
        public string Title { get; set; }   
        public string Message { get; set; }
        public string Module { get; set; }
        public string User { get; set; }
        public DateTime TimeNotification { get; set; }  

    }
}
