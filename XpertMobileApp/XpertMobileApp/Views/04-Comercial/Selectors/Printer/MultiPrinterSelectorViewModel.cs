using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Text;
using Xamarin.Forms;
using Xpert.Common.DAO;
using XpertMobileApp.Api.ViewModels;
using XpertMobileApp.DAL;
using XpertMobileApp.Helpers;
using XpertMobileApp.Models;
using XpertWebApi.Models;

namespace XpertMobileApp.ViewModels
{
    public class MultiPrinterSelectorViewModel : INotifyPropertyChanged
    {
        private List<XPrinter> items = new List<XPrinter>();

        public event PropertyChangedEventHandler PropertyChanged;
        private void NotifyPropertyChanged([CallerMemberName] String propertyName = "")
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

        public List<XPrinter> Items { get { return items; } set { items = value;
                NotifyPropertyChanged();
            } }

        public bool IsBusy { get; set; }

        public List<XPrinter> SelectedItem { get; set; }

        public MultiPrinterSelectorViewModel()
        {
        }
    }
}
