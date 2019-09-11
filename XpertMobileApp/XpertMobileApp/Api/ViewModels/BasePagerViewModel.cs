using Acr.UserDialogs;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xamarin.Forms;
using Xpert.Common.WSClient.Helpers;
using Xpert.Common.WSClient.Model;
using Xpert.Common.WSClient.Services;
using XpertMobileApp.Api.Managers;
using XpertMobileApp.DAL;
using XpertMobileApp.Helpers;
using XpertMobileApp.ViewModels;

namespace XpertMobileApp.Api.ViewModels
{
    public class BasePagerViewModel<TTable, TView> : BaseViewModel
     where TTable : new()
     where TView : new()
    {
        ICurdService<TView> service;

        bool isLoadExtrasBusy = false;
        public bool IsLoadExtrasBusy
        {
            get { return isLoadExtrasBusy; }
            set { SetProperty(ref isLoadExtrasBusy, value); }
        }

        public int itemCount { get; set; }

        private int pageCount;
        public int PageCount
        {
            get { return pageCount; }
            set { SetProperty(ref pageCount, value); }
        }

        private int pageSize = -1;
        public int PageSize
        {
            get
            {
                if (pageSize == -1)
                {
                    return Device.Idiom == TargetIdiom.Phone ? 6 : 9;
                }
                return pageSize;
            }
            set
            {
                SetProperty(ref pageSize, value);
            }
        }

        private int pageIndex;
        public int PageIndex
        {
            get
            {
                return pageIndex;
            }
            set
            {
                SetProperty(ref pageIndex, value);
            }
        }

        public ObservableCollection<TView> Items { get; set; }

        public TView SelectedItem { get; set; }

        public Command LoadItemsCommand { get; set; }

        public Command LoadExtrasDataCommand { get; set; }

        protected virtual string ContoleurName
        {
            get
            {
                return typeof(TTable).Name;
            }
        }

        public BasePagerViewModel()
        {
            InitConstructor();
        }

        protected virtual void InitConstructor()
        {
            service = new CrudService<TView>(App.RestServiceUrl, ContoleurName, App.User.Token);

            Items = new ObservableCollection<TView>();

            // Listing
            LoadItemsCommand = new Command(async () => await ExecuteLoadItemsCommand());
        }

        protected virtual Dictionary<string, string> GetFilterParams()
        {
            Dictionary<string, string> dico = new Dictionary<string, string>();
            return dico;
        }

        protected virtual void OnAfterLoadItems(IEnumerable<TView> list)
        {

        }

        async Task ExecuteLoadItemsCommand()
        {
            if (IsBusy)
                return;

            try
            {
                IsBusy = true;
                Items.Clear();

                int itemCount = await service.ItemsCount(GetFilterParams());
                PageCount = itemCount / PageSize;
                if(PageCount * itemCount < itemCount)
                {
                    PageCount += 1; 
                }

                var currentPage = (PageIndex / PageSize) + 1;
                var source = await service.SelectByPage(GetFilterParams(), currentPage, PageSize);

                OnAfterLoadItems(source);

                foreach (var itm in source)
                  Items.Add(itm);

                MessagingCenter.Send(App.MsgCenter, MCDico.DATA_COUNT_LOADED, PageCount);

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
