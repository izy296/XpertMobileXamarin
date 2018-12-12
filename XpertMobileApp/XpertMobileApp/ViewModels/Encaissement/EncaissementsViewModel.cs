using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.Text;
using System.Threading.Tasks;
using Xamarin.Forms;
using Xpert.Pharm.DAL;
using XpertMobileApp.Services;
using XpertMobileApp.Views.Encaissement;

namespace XpertMobileApp.ViewModels
{
    public enum EncaissDisplayType { None, Encaiss, Decaiss };

    public class EncaissementsViewModel : BaseViewModel
    {
        public EncaissDisplayType EncaissDisplayType;

        public ObservableCollection<View_TRS_ENCAISS> Items { get; set; }
        public Command LoadItemsCommand { get; set; }
        public Command AddItemCommand { get; set; }

        public EncaissementsViewModel()
        {
            Title = AppResources.pn_encaissement;

            Items = new ObservableCollection<View_TRS_ENCAISS>();
            LoadItemsCommand = new Command(async () => await ExecuteLoadItemsCommand());
            AddItemCommand = new Command<View_TRS_ENCAISS>(async (View_TRS_ENCAISS item) => await ExecuteAddItemCommand(item));

            MessagingCenter.Subscribe<NewEncaissementPage, View_TRS_ENCAISS>(this, "AddItem", async (obj, item) =>
            {
                AddItemCommand.Execute(item);
            });

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

                UpdateItemIndex(Items);
            }
        }

        private void UpdateItemIndex<T>(ObservableCollection<T> items)
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

            IsBusy = true;

            // Recupérer le type a afficher ENC,DEC or All
            string type = GetCurrentType();

            try
            {
                Items.Clear();
                var items = await WebServiceClient.GetEncaissements(App.RestServiceUrl, type, "1", "all", "", "", "", "", "");
                int index = 1;
                foreach (var item in items)
                {
                    item.Index = index;
                    Items.Add(item);
                    index += 1;
                }
            }
            catch (Exception ex)
            {
                Debug.WriteLine(ex);
            }
            finally
            {
                IsBusy = false;
            }
        }

        private string GetCurrentType()
        {
            string type = "";
            switch (EncaissDisplayType)
            {
                case EncaissDisplayType.None:
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
