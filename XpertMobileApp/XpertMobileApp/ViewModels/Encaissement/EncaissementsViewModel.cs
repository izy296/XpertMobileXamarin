using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.Text;
using System.Threading.Tasks;
using Xamarin.Forms;
using Xamarin.Forms.Extended;
using Xpert.Pharm.DAL;
using XpertMobileApp.Helpers;
using XpertMobileApp.Services;
using XpertMobileApp.Views.Encaissement;

namespace XpertMobileApp.ViewModels
{
    public enum EncaissDisplayType { None, All, ENC, DEC };

    public class EncaissementsViewModel : BaseViewModel
    {
        private const int PageSize = 10;

        private int elementsCount;

        public EncaissDisplayType EncaissDisplayType { get; set; }
        public DateTime StartDate { get; set; } = DateTime.Now.AddDays(-90);
        public DateTime EndDate { get; set; } = DateTime.Now;

        public InfiniteScrollCollection<View_TRS_ENCAISS> Items { get; set; }
        public Command LoadItemsCommand { get; set; }
        public Command AddItemCommand { get; set; }
        public Command DeleteItemCommand { get; set; }
        public Command UpdateItemCommand { get; set; }

        public ObservableCollection<View_BSE_COMPTE> Comptes { get; set; }
        public View_BSE_COMPTE SelectedCompte { get; set; }
        public Command LoadComptesCommand { get; set; }


        public EncaissementsViewModel()
        {
            Title = AppResources.pn_encaissement;

            Comptes = new ObservableCollection<View_BSE_COMPTE>();
            LoadComptesCommand = new Command(async () => await ExecuteLoadComptesCommand());

            // Listing
            LoadItemsCommand = new Command(async () => await ExecuteLoadItemsCommand());

            // Ajout
            AddItemCommand = new Command<View_TRS_ENCAISS>(async (View_TRS_ENCAISS item) => await ExecuteAddItemCommand(item));
            MessagingCenter.Subscribe<NewEncaissementPage, View_TRS_ENCAISS>(this, MCDico.ADD_ITEM, async (obj, item) =>
            {
                AddItemCommand.Execute(item);
            });

            // Supression
            DeleteItemCommand = new Command<View_TRS_ENCAISS>(async (View_TRS_ENCAISS item) => await ExecuteDeleteItemCommand(item));
            MessagingCenter.Subscribe<EncaissementDetailPage, View_TRS_ENCAISS>(this, MCDico.DELETE_ITEM, async (obj, item) =>
            {
                DeleteItemCommand.Execute(item);
            });

            // Modification
            UpdateItemCommand = new Command<View_TRS_ENCAISS>(async (View_TRS_ENCAISS item) => await ExecuteUpdateItemCommand(item));
            MessagingCenter.Subscribe<NewEncaissementPage, View_TRS_ENCAISS>(this, MCDico.UPDATE_ITEM, async (obj, item) =>
            {
                UpdateItemCommand.Execute(item);
            });

            // chargement infini
            Items = new InfiniteScrollCollection<View_TRS_ENCAISS>
            {
                OnLoadMore = async () =>
                {

                    IsBusy = true;

                    // Recupérer le type a afficher ENC,DEC or All
                    string type = GetCurrentType();

                    elementsCount = await WebServiceClient.GetEncaissementsCount(App.RestServiceUrl, type, "all", StartDate, EndDate, "", "", SelectedCompte?.CODE_COMPTE);

                    // load the next page
                    var page = (Items.Count / PageSize) + 1;
                    var items = await WebServiceClient.GetEncaissements(App.RestServiceUrl, type, page.ToString(), "all", StartDate, EndDate, "", "", SelectedCompte?.CODE_COMPTE);
                    UpdateItemIndex(items);

                    IsBusy = false;

                    // return the items that need to be added
                    return items;
                },
                OnCanLoadMore = () =>
                {
                    return Items.Count < elementsCount;
                }
            };
        }

        async Task ExecuteUpdateItemCommand(View_TRS_ENCAISS item)
        {
            if (App.IsConected)
            {
                var newItem = item as View_TRS_ENCAISS;

                // Save the added Item in the local bdd
                // await DataStore.DeleteItemAsync(newItem);

                // TODO : test if connected else mark as not synchronizd
                View_TRS_ENCAISS result = await WebServiceClient.UpdateEncaissement(newItem);
                MessagingCenter.Send(this, MCDico.REFRESH_ITEM, result);
                if (result != null)
                {
                    int idx = Items.IndexOf(item);
                    Items[idx] = result;
                }
            }
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
                case EncaissDisplayType.ENC:
                    type = "ENC";
                    break;
                case EncaissDisplayType.DEC:
                    type = "DEC";
                    break;
            }

            return type;
        }

        async Task ExecuteLoadComptesCommand()
        {
            /*
            if (IsBusy)
                return;
            */

            IsBusy = true;

            try
            {
                Comptes.Clear();
                var itemsC = await WebServiceClient.getComptes();
                foreach (var itemC in itemsC)
                {
                    Comptes.Add(itemC);
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
