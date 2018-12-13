using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.Text;
using System.Threading.Tasks;
using Xamarin.Forms;
using Xamarin.Forms.Extended;
using Xpert.Pharm.DAL;
using XpertMobileApp.Services;
using XpertMobileApp.Views.Encaissement;

namespace XpertMobileApp.ViewModels
{
    public enum EncaissDisplayType { None, All, Encaiss, Decaiss };

    public class EncaissementsViewModel : BaseViewModel
    {
        private const int PageSize = 10;

        public EncaissDisplayType EncaissDisplayType;

        public InfiniteScrollCollection<View_TRS_ENCAISS> Items { get; set; }
        public Command LoadItemsCommand { get; set; }
        public Command AddItemCommand { get; set; }
        public Command DeleteItemCommand { get; set; }

        public EncaissementsViewModel()
        {
            Title = AppResources.pn_encaissement;

            // Items = new InfiniteScrollCollection<View_TRS_ENCAISS>();
            LoadItemsCommand = new Command(async () => await ExecuteLoadItemsCommand());

            AddItemCommand = new Command<View_TRS_ENCAISS>(async (View_TRS_ENCAISS item) => await ExecuteAddItemCommand(item));
            MessagingCenter.Subscribe<NewEncaissementPage, View_TRS_ENCAISS>(this, "AddItem", async (obj, item) =>
            {
                AddItemCommand.Execute(item);
            });

            DeleteItemCommand = new Command<View_TRS_ENCAISS>(async (View_TRS_ENCAISS item) => await ExecuteDeleteItemCommand(item));
            MessagingCenter.Subscribe<EncaissementDetailPage, View_TRS_ENCAISS>(this, "DeleteItem", async (obj, item) =>
            {
                DeleteItemCommand.Execute(item);
            });

            Items = new InfiniteScrollCollection<View_TRS_ENCAISS>
            {
                OnLoadMore = async () =>
                {
                    IsBusy = true;

                    // Recupérer le type a afficher ENC,DEC or All
                    string type = GetCurrentType();
                    // load the next page
                    var page = (Items.Count / PageSize) + 1;
                    var items = await WebServiceClient.GetEncaissements(App.RestServiceUrl, type, page.ToString(), "all", "", "", "", "", "");
                    UpdateItemIndex(items);

                    IsBusy = false;

                    // return the items that need to be added
                    return items;
                }
            };
        }

        async Task ExecuteDeleteItemCommand(View_TRS_ENCAISS item)
        {
            if (App.IsConected)
            {
                var newItem = item as View_TRS_ENCAISS;

                // Save the added Item in the local bdd
                // await DataStore.DeleteItemAsync(newItem);

                // TODO : test if connected else mark as not synchronizd
                bool result = await WebServiceClient.DeleteEncaissement(newItem);
                if (result)
                { 
                    Items.Remove(item);
                }
            }
        }

        async Task ExecuteAddItemCommand(View_TRS_ENCAISS item)
        {
            if (App.IsConected)
            { 
                var newItem = item as View_TRS_ENCAISS;

                // Save the added Item in the local bdd
                //  await DataStore.AddItemAsync(newItem);

                // TODO : test if connected else mark as not synchronizd
                View_TRS_ENCAISS result = await WebServiceClient.SaveEncaissements(newItem);

                Items.Insert(0, result);

                UpdateItemIndex<View_TRS_ENCAISS>(Items);
            }
        }

        private void UpdateItemIndex<T>(InfiniteScrollCollection<T> items)
        {
            int i = 0;
            foreach (var item in items)
            {
                i += 1;
                (item as BASE_CLASS).Index = i;
            }
        }

        private void UpdateItemIndex<T>(List<T> items)
        {
            int i = 0;
            foreach (var item in items)
            {
                i += 1;
                (item as BASE_CLASS).Index = i;
            }
        }

        async Task ExecuteLoadItemsCommand()
        {
            if (IsBusy)
                return;

            try
            {
                Items.Clear();
                await Items.LoadMoreAsync();
            }
            catch (Exception ex)
            {
                Debug.WriteLine(ex);
            }
        }

        private string GetCurrentType()
        {
            string type = "";
            switch (EncaissDisplayType)
            {
                case EncaissDisplayType.All:
                    type = "all";
                    break;
                case EncaissDisplayType.Encaiss:
                    type = "ENC";
                    break;
                case EncaissDisplayType.Decaiss:
                    type = "DEC";
                    break;
            }

            return type;
        }
    }

}
