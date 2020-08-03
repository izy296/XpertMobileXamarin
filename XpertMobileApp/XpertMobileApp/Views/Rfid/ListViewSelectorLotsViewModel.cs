using Acr.UserDialogs;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Threading.Tasks;
using Xamarin.Forms;
using Xamarin.Forms.Extended;
using XpertMobileApp.DAL;
using XpertMobileApp.Models;
using XpertMobileApp.Services;

namespace XpertMobileApp.ViewModels
{
    public class ListViewSelectorLotsViewModel : BaseViewModel
    {

        public ObservableCollection<View_STK_STOCK> Items { get; set; }
        public View_STK_STOCK SelectedItem { get; set; }

        public ListViewSelectorLotsViewModel(List<View_STK_STOCK>  Lots)
        {
            Items = new ObservableCollection<View_STK_STOCK>();
            foreach (var item in Lots)
            {
                Items.Add(item);

            }

        }

    }
}
