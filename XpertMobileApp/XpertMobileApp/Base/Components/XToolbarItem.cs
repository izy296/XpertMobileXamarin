using System;
using System.Collections.Generic;
using System.Text;
using Xamarin.Forms;

namespace XpertMobileApp.Components
{
    public class XToolbarItem : ToolbarItem
    {
        public static readonly BindableProperty IsVisibleProperty = BindableProperty.Create(nameof(IsVisible), typeof(bool), typeof(XToolbarItem), true, BindingMode.TwoWay, propertyChanged: OnIsVisibleChanged);

        public static readonly BindableProperty PrivilegeIdProperty = BindableProperty.Create(nameof(PrivilegeId), typeof(string), typeof(XToolbarItem), "", BindingMode.TwoWay);

        public bool IsVisible
        {
            get => (bool)GetValue(IsVisibleProperty);
            set => SetValue(IsVisibleProperty, value);
        }

        public string PrivilegeId
        {
            get => (string)GetValue(PrivilegeIdProperty);
            set => SetValue(PrivilegeIdProperty, value);
        }

        private static void OnIsVisibleChanged(BindableObject bindable, object oldvalue, object newvalue)
        {
            var item = bindable as XToolbarItem;

            if (item == null || item.Parent == null)
                return;

            var toolbarItems = ((ContentPage)item.Parent).ToolbarItems;

            if ((bool)newvalue && !toolbarItems.Contains(item))
            {
                Device.BeginInvokeOnMainThread(() => { toolbarItems.Add(item); });
            }
            else if (!(bool)newvalue && toolbarItems.Contains(item))
            {
                Device.BeginInvokeOnMainThread(() => { toolbarItems.Remove(item); });
            }
        }
    }
}
