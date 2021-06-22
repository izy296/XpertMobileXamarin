using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;

using Xamarin.Forms;
using Xpert;
using XpertMobileApp.Api;
using XpertMobileApp.Api.Services;
using XpertMobileApp.Models;
using XpertMobileApp.Services;

namespace XpertMobileApp.ViewModels
{
    public class BaseViewModel : INotifyPropertyChanged
    {
        public string CurrentStream = Guid.NewGuid().ToString();

        public bool HasAdmin
        {
            get { return App.User.UserGroup == "AD"; }
        }

        bool isBusy = false;
        public bool IsBusy
        {
            get { return isBusy; }
            set { SetProperty(ref isBusy, value); }
        }

        string title = string.Empty;
        public string Title
        {
            get { return title; }
            set { SetProperty(ref title, value); }
        }

        public static bool HasPrivilege(XpertObjets codeObject, XpertActions action)
        {
            if (AppManager.HasAdmin) return true;
            if (codeObject == XpertObjets.None || action == XpertActions.None) return true;

            bool result = false;
            if (AppManager.permissions != null)
            {
                var obj = AppManager.permissions.Where(x => x.CodeObjet == codeObject.ToString()).FirstOrDefault();
                var res = XpertHelper.GetValue(obj, action.ToString());
                result = obj != null && Convert.ToInt16(res) > 0;
            }
            return result;
        }


        protected bool SetProperty<T>(ref T backingStore, T value,
            [CallerMemberName]string propertyName = "",
            Action onChanged = null)
        {
            if (EqualityComparer<T>.Default.Equals(backingStore, value))
                return false;

            backingStore = value;
            onChanged?.Invoke();
            OnPropertyChanged(propertyName);
            return true;
        }

        #region INotifyPropertyChanged
        public event PropertyChangedEventHandler PropertyChanged;
        protected void OnPropertyChanged([CallerMemberName] string propertyName = "")
        {
            var changed = PropertyChanged;
            if (changed == null)
                return;

            changed.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

        #endregion
    }
}
