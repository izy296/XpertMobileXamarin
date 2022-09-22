using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Text;
using Xamarin.Forms;
using Xpert;
using XpertMobileApp.Api;
using XpertMobileApp.Api.Models;
using XpertMobileApp.Views;

namespace XpertMobileApp.Models
{
    public class HomeMenuItem : INotifyPropertyChanged
    {
        private int countOfNotifications = 0;

        public XpertObjets CodeObjet { get; set; } = XpertObjets.None;

        public XpertActions Action { get; set; } = XpertActions.None;

        public bool HasPermission
        {
            get
            {
                return AppManager.HasPermission(CodeObjet, Action);
            }
        }

        public MenuItemType Id { get; set; }

        public string Title { get; set; }

        public string Image { get; set; }

        public int Mobile_Edition { get; set; }

        public Type TargetType { get; set; }

        public bool VisibleToGuest { get; set; } = true;
        public bool NotificationBadgeIsVisible { get; set; } = false;
        public bool IsNewModule { get; set; } = false;

        public int CountOfNotifications
        {
            get => countOfNotifications;
            set
            {
                if (value != countOfNotifications)
                {
                    countOfNotifications = value;
                    NotifyPropertyChanged();
                }
            }
        }
        public MenuItemGroup ItemGroup { get; set; }

        public event PropertyChangedEventHandler PropertyChanged;

        private void NotifyPropertyChanged([CallerMemberName] String propertyName = "")
        {
            if (propertyName == "CountOfNotifications")
            {
                if (CountOfNotifications > 0)
                    App.IsThereNotification = true;
                else App.IsThereNotification = false;
                MessagingCenter.Send(this, "refreshBell", "");
            }
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }

}
