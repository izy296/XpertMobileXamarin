using System;
using System.Collections.Generic;
using System.Text;
using Xpert;
using XpertMobileApp.Api;

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
    }
}
