using Acr.UserDialogs;
using System;
using System.Collections.ObjectModel;
using System.Threading.Tasks;
using Xamarin.Forms;
using Xamarin.Forms.Extended;
using Xpert.Common.WSClient.Helpers;
using XpertMobileApp.Api.Services;
using XpertMobileApp.DAL;
using XpertMobileApp.Helpers;
using XpertMobileApp.Services;
using XpertMobileApp.Views.Encaissement;

namespace XpertMobileApp.ViewModels
{

    public class ProduitsViewModel : BaseViewModel
    {
        private const int PageSize = 10;

        private int elementsCount;

        public Command AddItemCommand { get; set; }
        public Command DeleteItemCommand { get; set; }
        public Command UpdateItemCommand { get; set; }

        public InfiniteScrollCollection<STK_PRODUITS> Items { get; set; }
        public Command LoadItemsCommand { get; set; }

        public string SearchedText { get; set; }

        public ObservableCollection<BSE_TABLE_TYPE> Types { get; set; }
        public BSE_TABLE_TYPE SelectedType { get; set; }
        public Command LoadTypesCommand { get; set; }

        public ObservableCollection<BSE_TABLE> Familles { get; set; }
        public BSE_TABLE SelectedFamille { get; set; }
        public Command LoadFamillesCommand { get; set; }

        public ProduitsViewModel()
        {
            Title = AppResources.pn_Produits;

            Types = new ObservableCollection<BSE_TABLE_TYPE>();
            Familles = new ObservableCollection<BSE_TABLE>();
            LoadTypesCommand = new Command(async () => await ExecuteLoadTypesCommand());

            Familles = new ObservableCollection<BSE_TABLE>();
            LoadFamillesCommand = new Command(async () => await ExecuteLoadFamillesCommand());

            // Listing
            LoadItemsCommand = new Command(async () => await ExecuteLoadItemsCommand());

            // Ajout
            AddItemCommand = new Command<STK_PRODUITS>(async (STK_PRODUITS item) => await ExecuteAddItemCommand(item));
            MessagingCenter.Subscribe<NewEncaissementPage, STK_PRODUITS>(this, MCDico.ADD_ITEM, async (obj, item) =>
            {
                AddItemCommand.Execute(item);
            });

            // Supression
            DeleteItemCommand = new Command<STK_PRODUITS>(async (STK_PRODUITS item) => await ExecuteDeleteItemCommand(item));
            MessagingCenter.Subscribe<EncaissementDetailPage, STK_PRODUITS>(this, MCDico.DELETE_ITEM, async (obj, item) =>
            {
                DeleteItemCommand.Execute(item);
            });

            // Modification
            UpdateItemCommand = new Command<STK_PRODUITS>(async (STK_PRODUITS item) => await ExecuteUpdateItemCommand(item));
            MessagingCenter.Subscribe<NewEncaissementPage, STK_PRODUITS>(this, MCDico.UPDATE_ITEM, async (obj, item) =>
            {
                UpdateItemCommand.Execute(item);
            });

            // chargement infini
            Items = new InfiniteScrollCollection<STK_PRODUITS>
            {
                OnLoadMore = async () =>
                {
                    IsBusy = true;

                    elementsCount = await WebServiceClient.GetProduitsCount(SelectedType?.CODE_TYPE, SelectedFamille?.CODE, SearchedText);

                    // load the next page
                    var page = (Items.Count / PageSize) + 1;
                    var items = await WebServiceClient.GetProduits(page, PageSize, SelectedType?.CODE_TYPE, SelectedFamille?.CODE, SearchedText);

                    XpertHelper.UpdateItemIndex(items);

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

        async Task ExecuteUpdateItemCommand(STK_PRODUITS item)
        {
            try
            {
                if (App.IsConected)
                {
                    var newItem = item as STK_PRODUITS;

                    // Save the added Item in the local bdd
                    // await DataStore.DeleteItemAsync(newItem);

                    // TODO : test if connected else mark as not synchronizd
                    /*
                    STK_PRODUITS result = await WebServiceClient.UpdateEncaissement(newItem);
                    MessagingCenter.Send(this, MCDico.REFRESH_ITEM, result);
                    if (result != null)
                    {
                        int idx = Items.IndexOf(item);
                        Items[idx] = result;
                    }
                    */
                }
            }
            catch (Exception ex)
            {
                await UserDialogs.Instance.AlertAsync(WSApi2.GetExceptionMessage(ex), AppResources.alrt_msg_Alert,
                    AppResources.alrt_msg_Ok);
            }
            finally
            {
                IsBusy = false;
            }
        }

        async Task ExecuteDeleteItemCommand(STK_PRODUITS item)
        {
            try
            {
                if (App.IsConected)
                {
                    var newItem = item as STK_PRODUITS;

                    // Save the added Item in the local bdd
                    // await DataStore.DeleteItemAsync(newItem);

                    // TODO : test if connected else mark as not synchronizd
                    /*
                    bool result = await WebServiceClient.DeleteEncaissement(newItem);
                    if (result)
                    {
                        Items.Remove(item);
                    }
                    */
                }
            }
            catch (Exception ex)
            {
                await UserDialogs.Instance.AlertAsync(WSApi2.GetExceptionMessage(ex), AppResources.alrt_msg_Alert,
                    AppResources.alrt_msg_Ok);
            }
            finally
            {
                IsBusy = false;
            }
        }

        async Task ExecuteAddItemCommand(STK_PRODUITS item)
        {
            try
            {
                if (App.IsConected)
                {
                    var newItem = item as STK_PRODUITS;

                    // Save the added Item in the local bdd
                    //  await DataStore.AddItemAsync(newItem);

                    // TODO : test if connected else mark as not synchronizd
                    /*
                     STK_PRODUITS result = await WebServiceClient.SaveEncaissements(newItem);

                     Items.Insert(0, result);
                    */

                    UpdateItemIndex<STK_PRODUITS>(Items);
                }
            }
            catch (Exception ex)
            {
                await UserDialogs.Instance.AlertAsync(WSApi2.GetExceptionMessage(ex), AppResources.alrt_msg_Alert,
                    AppResources.alrt_msg_Ok);
            }
            finally
            {
                IsBusy = false;
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
                await UserDialogs.Instance.AlertAsync(WSApi2.GetExceptionMessage(ex), AppResources.alrt_msg_Alert,
                    AppResources.alrt_msg_Ok);
            }
            finally
            {
                IsBusy = false;
            }
        }


        async Task ExecuteLoadTypesCommand()
        {
            /*
            if (IsBusy)
             return;

            IsBusy = true;
            */


            try
            {
                Types.Clear();
                var itemsC = await WebServiceClient.GetProduitTypes();

                BSE_TABLE_TYPE allElem = new BSE_TABLE_TYPE();
                allElem.CODE_TYPE = "";
                allElem.DESIGNATION_TYPE = AppResources.txt_All;
                itemsC.Add(allElem);

                foreach (var itemC in itemsC)
                {
                    Types.Add(itemC);
                }
            }
            catch (Exception ex)
            {
                await UserDialogs.Instance.AlertAsync(WSApi2.GetExceptionMessage(ex), AppResources.alrt_msg_Alert,
                    AppResources.alrt_msg_Ok);
            }
            finally
            {
                IsBusy = false;
            }
        }

        async Task ExecuteLoadFamillesCommand()
        {
            /*
            if (IsBusy)
             return;
             
            IsBusy = true;
            */


            try
            {
                Familles.Clear();
                var itemsC = await WebServiceClient.GetProduitFamilles();

                BSE_TABLE allElem = new BSE_TABLE();
                allElem.CODE = "";
                allElem.DESIGNATION = AppResources.txt_All;
                Familles.Add(allElem);

                foreach (var itemC in itemsC)
                {
                    Familles.Add(itemC);
                }
            }
            catch (Exception ex)
            {
                await UserDialogs.Instance.AlertAsync(WSApi2.GetExceptionMessage(ex), AppResources.alrt_msg_Alert,
                    AppResources.alrt_msg_Ok);
            }
            finally
            {
                IsBusy = false;
            }
        }
    }

}
