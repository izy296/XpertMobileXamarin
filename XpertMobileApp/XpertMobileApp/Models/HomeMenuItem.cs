using System;
using System.Collections.Generic;
using System.Text;
using Xamarin.Forms;
using Xpert;
using XpertMobileApp.Api;
using XpertMobileApp.Api.Models;

namespace XpertMobileApp.Models
{
    public class HomeMenuItem
    {
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

        public int CountOfNotifications { get; set; } = 0;

        public MenuItemGroup ItemGroup { get; set; }
    }

}
