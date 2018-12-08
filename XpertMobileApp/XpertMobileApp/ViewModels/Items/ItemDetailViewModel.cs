using System;
using Xpert.Pharm.DAL;
using XpertMobileApp.Models;

namespace XpertMobileApp.ViewModels
{
    public class ItemDetailViewModel<T> : BaseViewModel
    {
        public T Item { get; set; }
        public ItemDetailViewModel(T item)
        {
            Title = item?.ToString();
            Item = item;
        }
    }
}
