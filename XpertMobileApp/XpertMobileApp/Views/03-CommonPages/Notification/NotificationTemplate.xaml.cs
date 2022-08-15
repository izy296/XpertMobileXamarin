using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;
using XpertMobileApp.ViewModels;

namespace XpertMobileApp.Views.Templates
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class NotificationTemplate : ContentView
    {

        public NotificationTemplate()
        {
            InitializeComponent();
            MessagingCenter.Subscribe<NotificationViewModel, string>(this,"RefreshImage", (o,s) =>
            {
                if (NotifTitle.Text==AppResources.Update_Notification_Header)
                {
                    NotifImage.Source = "mi_0.png";
                }
            });
        }
        

    }
}