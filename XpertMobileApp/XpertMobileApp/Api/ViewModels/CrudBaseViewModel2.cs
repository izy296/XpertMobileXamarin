using Acr.UserDialogs;
using System;
using System.Collections.Generic;
using System.Linq;
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
    public class CrudBaseViewModel2<TTable, TView> : BaseViewModel
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

        public InfiniteScrollCollection<COM_DOC> Items { get; set; }

        public TView SelectedItem { get; set; }

        public Command LoadItemsCommand { get; set; }

        public Command LoadExtrasDataCommand { get; set; }

        public Command AddItemCommand { get; set; }

        public Command DeleteItemCommand { get; set; }

        public Command UpdateItemCommand { get; set; }

        public CrudBaseViewModel2()
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
            Items = new InfiniteScrollCollection<COM_DOC>
            {
                OnLoadMore = async () =>
                {
                    IsBusy = true;

                    elementsCount = await service.ItemsCount(GetFilterParams());

                    // load the next page
                    var page = (Items.Count / PageSize) + 1;

                    var items = await service.SelectByPage(GetFilterParams(), page, PageSize);
                    var Items_D = GetDisplayList(items);


                    OnAfterLoadItems(items);

                    IsBusy = false;

                    // return the items that need to be added
                    return Items_D;
                },
                OnCanLoadMore = () =>
                {
                    return Items.Count < elementsCount;
                }
            };
        }

        protected virtual List<COM_DOC> GetDisplayList(IEnumerable<TView> list)
        {
            List<COM_DOC> result = new List<COM_DOC>();
            foreach (var item in list)
            {
                COM_DOC t = new COM_DOC();
                this.CopyPropertiesTo(item, t);
                result.Add(t);
            }
            return result;
        }

        protected virtual void CopyPropertiesTo(TView source, COM_DOC dest)
        {
            XpertHelper.CopyPropertiesTo(source, dest);
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
