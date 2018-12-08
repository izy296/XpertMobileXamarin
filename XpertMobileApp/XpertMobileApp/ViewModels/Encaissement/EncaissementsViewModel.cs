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
    public class EncaissementsViewModel : BaseViewModel
    {
        public ObservableCollection<View_TRS_ENCAISS> Items { get; set; }
        public Command LoadItemsCommand { get; set; }

        public EncaissementsViewModel()
        {
            Title = AppResources.pn_encaissement;

            Items = new ObservableCollection<View_TRS_ENCAISS>();
            LoadItemsCommand = new Command(async () => await ExecuteLoadItemsCommand());

            MessagingCenter.Subscribe<NewEncaissementPage, View_TRS_ENCAISS>(this, "AddItem", async (obj, item) =>
            {
                var newItem = item as View_TRS_ENCAISS;
                Items.Add(newItem);
              //  await DataStore.AddItemAsync(newItem);
            });

        }

        async Task ExecuteLoadItemsCommand()
        {
            if (IsBusy)
                return;

            IsBusy = true;

            try
            {
                Items.Clear();
                var items = await WebServiceClient.GetEncaissements(App.RestServiceUrl, "all", "1", "all", "", "", "", "", "");
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
    }

}
