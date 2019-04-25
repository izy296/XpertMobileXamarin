using Acr.UserDialogs;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Xamarin.Forms;
using Xamarin.Forms.Extended;
using Xpert.Common.WSClient.Helpers;
using Xpert.Common.WSClient.Model;
using Xpert.Common.WSClient.Services;
using XpertMobileApp.Api.Services;
using XpertMobileApp.DAL;
using XpertMobileApp.Helpers;
using XpertMobileApp.Models;
using XpertMobileApp.ViewModels;

namespace XpertMobileApp.Api.ViewModels
{
    public class CrudBaseViewModel<TTable, TView> : BaseViewModel
    where TTable : new()
    where TView : new()
    {
        ICurdService<TView> service;

        public const int PageSize = 10;

        private int elementsCount;

        bool isLoadExtrasBusy = false;
        public bool IsLoadExtrasBusy
        {
            get { return isLoadExtrasBusy; }
            set { SetProperty(ref isLoadExtrasBusy, value); }
        }

        public InfiniteScrollCollection<TView> Items { get; set; }

        public Command LoadItemsCommand { get; set; }

        public Command LoadExtrasDataCommand { get; set; }

        public Command AddItemCommand { get; set; }

        public Command DeleteItemCommand { get; set; }

        public Command UpdateItemCommand { get; set; }

        public CrudBaseViewModel()
        {
            InitConstructor();
        }

        protected virtual Dictionary<string, string> GetFilterParams()
        {
            Dictionary<string, string> dico = new Dictionary<string, string>();
            return dico;
        }

        protected virtual void OnAfterLoadItems(IEnumerable<TView> list)
        {

        }

        protected virtual void InitConstructor()
        {
            service = new CrudService<TView>(App.RestServiceUrl, typeof(TTable).Name, App.User.Token);

            // Listing
            LoadItemsCommand = new Command(async () => await ExecuteLoadItemsCommand());

            // Ajout
            AddItemCommand = new Command<TView>(async (TView item) => await ExecuteAddItemCommand(item));
            MessagingCenter.Subscribe<MsgCenter, TView>(this, MCDico.ADD_ITEM, async (obj, item) =>
            {
                AddItemCommand.Execute(item);
            });

            // Supression
            DeleteItemCommand = new Command<string>(async (string idElem) => await ExecuteDeleteItemCommand(idElem));
            MessagingCenter.Subscribe<MsgCenter, TView>(this, MCDico.DELETE_ITEM, async (obj, item) =>
            {
                DeleteItemCommand.Execute(item);
            });

            // Modification
            UpdateItemCommand = new Command<TView>(async (TView item) => await ExecuteUpdateItemCommand(item));
            MessagingCenter.Subscribe<MsgCenter, TView>(this, MCDico.UPDATE_ITEM, async (obj, item) =>
            {
                UpdateItemCommand.Execute(item);
            });

            // chargement infini
            Items = new InfiniteScrollCollection<TView>
            {
                OnLoadMore = async () =>
                {
                    IsBusy = true;

                    elementsCount = await service.ItemsCount(GetFilterParams());

                    // load the next page
                    var page = (Items.Count / PageSize) + 1;

                    var items = await service.SelectByPage(GetFilterParams(), page, PageSize);

                    OnAfterLoadItems(items);

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

        async Task ExecuteLoadItemsCommand()
        {
            if (IsBusy)
                return;

            try
            {
                IsBusy = true;
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

        async Task ExecuteUpdateItemCommand(TView item)
        {
            if (IsBusy)
                return;

            try
            {
                if (App.IsConected)
                {
                    IsBusy = true;
                    await service.UpdateItemAsync(item);
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

        async Task ExecuteDeleteItemCommand(string codeItem)
        {
            if (IsBusy)
                return;

            try
            {
                if (App.IsConected)
                {
                    IsBusy = true;
                    await service.DeleteItemAsync(codeItem);
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

        async Task ExecuteAddItemCommand(TView item)
        {
            if (IsBusy)
                return;

            try
            {
                if (App.IsConected)
                {
                    IsBusy = true;
                    await service.AddItemAsync(item);
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
