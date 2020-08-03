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
       where T : new()
       where TD : new()
    {
        public Command LoadRowsCommand { get; set; }

        private View_TRS_TIERS selectedTiers;
        public View_TRS_TIERS SelectedTiers
        {
            get { return selectedTiers; }
            set { SetProperty(ref selectedTiers, value); }
        }

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

        public ItemRowsDetailViewModel()
        {
            ItemRows = new ObservableCollection<TD>();
        }

        public ItemRowsDetailViewModel(T obj, string itemId)
        {
            Item = obj;

            this.ItemId = itemId;

            Title = string.IsNullOrEmpty(ItemId) ? AppResources.pn_NewCommande : obj?.ToString();

            // Listing
            ItemRows = new ObservableCollection<TD>();            
        }
    }
}
