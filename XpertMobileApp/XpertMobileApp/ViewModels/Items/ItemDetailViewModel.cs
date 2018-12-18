using System;
using Xamarin.Forms;
using Xpert.Pharm.DAL;
using XpertMobileApp.Helpers;
using XpertMobileApp.Models;
using XpertMobileApp.Views;

namespace XpertMobileApp.ViewModels
{
    public class ItemDetailViewModel<T> : BaseViewModel
    {
        private T item;
        public T Item
        {
            get { return item; }
            set { SetProperty(ref item, value); }
        }

        
        public ItemDetailViewModel(T obj)
        {
            Title = obj?.ToString();
            Item = obj;
            
            MessagingCenter.Subscribe<ContentPage, T >(this, MCDico.REFRESH_ITEM, async (generic, itm) =>
            {
                Item = itm;
            });
                    
        }
    }
}
