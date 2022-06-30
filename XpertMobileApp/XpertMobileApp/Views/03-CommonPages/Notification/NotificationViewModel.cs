using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Text;
using Xamarin.Forms;
using XpertMobileApp.Api.Models;

namespace XpertMobileApp.ViewModels
{
    public class NotificationViewModel : BaseViewModel
    {
        private ObservableCollection<Notification> items;

        public ObservableCollection<Notification> Items
        {
            get { return items; }
            set { SetProperty(ref items, value); }
        }

        public Command RefreshNotification
        {
            get
            {
                return new Command(async =>
                {
                    IsBusy = true;
                    Items = new SettingsModel().getNotificationAsync();
                    IsBusy = false;
                });
            }
        }

        public NotificationViewModel()
        {
            Items = new SettingsModel().getNotificationAsync();

        }



    }
}
