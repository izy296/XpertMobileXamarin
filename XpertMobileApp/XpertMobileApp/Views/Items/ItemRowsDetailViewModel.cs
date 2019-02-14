using Acr.UserDialogs;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Threading.Tasks;
using Xamarin.Forms;
using XpertMobileApp.DAL;
using XpertMobileApp.Helpers;
using XpertMobileApp.Services;

namespace XpertMobileApp.ViewModels
{
    public class ItemRowsDetailViewModel<T,TD> : BaseViewModel
    {
        public Command LoadRowsCommand { get; set; }

        private string itemId;
        public string ItemId
        {
            get { return itemId; }
            set { SetProperty(ref itemId, value); }
        }

        private T item;
        public T Item
        {
            get { return item; }
            set { SetProperty(ref item, value); }
        }

        private ObservableCollection<TD> itemRows;
        public ObservableCollection<TD> ItemRows
        {
            get { return itemRows; }
            set { SetProperty(ref itemRows, value); }
        }

        public ItemRowsDetailViewModel(T obj, string itemId)
        {
            Title = obj?.ToString();
            Item = obj;

            this.ItemId = itemId;

            // Listing
            ItemRows = new ObservableCollection<TD>();            
        }
    }
}
