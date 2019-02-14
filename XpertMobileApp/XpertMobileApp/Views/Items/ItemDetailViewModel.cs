using Xamarin.Forms;
using XpertMobileApp.Helpers;

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
