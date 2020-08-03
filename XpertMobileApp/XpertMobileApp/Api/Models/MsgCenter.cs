using System;
using System.Collections.Generic;
using System.Text;
using Xamarin.Forms;

namespace XpertMobileApp.Models
{
    public class MsgCenter
    {
        public MsgCenter()
        {
        }

        public void SendAction(object sender, string stream, string prefix, string defaultMsg, object obj)
        {
            string msg = defaultMsg;
            if (!string.IsNullOrEmpty(stream))
            {
                msg = prefix + stream;
                MessagingCenter.Send(App.MsgCenter, msg, obj);
            }
        }
    }
}
