using Acr.UserDialogs;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Threading.Tasks;
using Xamarin.Forms;
using Xamarin.Forms.Internals;
using Xpert.Common.DAO;
using Xpert.Common.WSClient.Helpers;
using Xpert.Common.WSClient.Services;
using XpertMobileApp;
using XpertMobileApp.Api.Managers;
using XpertMobileApp.Api.ViewModels;
using XpertMobileApp.DAL;
using XpertMobileApp.Models;
using XpertMobileApp.Services;


namespace XpertMobileApp.Views
{
    [Preserve(AllMembers = true)]
    public class BtqProductViewModel : CrudBaseViewModel2<PRODUITS, View_PRODUITS>
    {    
    
        public bool OnlyNew { get; set; }

        private int pagesSize = 8;
        public int PagesSize  
        {
            get { return pagesSize; }
            set { SetProperty(ref pagesSize, value); }
        }

        private int pageCount;
        public int PageCount
        {
            get { return pageCount; }
            set { SetProperty(ref pageCount, value); }
        }

        internal async Task<int> GetCount()
        {
            var service = new CrudService<View_PRODUITS>(App.RestServiceUrl, ContoleurName, App.User.Token);
            int i = await service.ItemsCount(GetFilterParams());
            return i;
        }

        internal async Task LoadItems(int page, int count)
        {
            try 
            { 
                UserDialogs.Instance.ShowLoading(AppResources.txt_Waiting);
                var service = new CrudService<View_PRODUITS>(App.RestServiceUrl, ContoleurName, App.User.Token);
                var items = await service.SelectByPage(GetFilterParams(), page, count);
                Items.Clear();
                if (items != null)
                {
                    PageCount = items.Count() / PagesSize;
                    foreach (var item in items)
                    {
                        Items.Add(item);
                    }
                }
                UserDialogs.Instance.HideLoading();
            }
            catch (Exception ex)
            {
                UserDialogs.Instance.HideLoading();
                await UserDialogs.Instance.AlertAsync(WSApi2.GetExceptionMessage(ex), AppResources.alrt_msg_Alert,
                    AppResources.alrt_msg_Ok);
            }
            finally
            {
                IsBusy = false;
            }
        }

        protected override void InitConstructor()
        {
            service = new CrudService<View_PRODUITS>(App.RestServiceUrl, ContoleurName, App.User.Token);
            Summaries = new ObservableCollection<SAMMUARY>();
            Items = new Xamarin.Forms.Extended.InfiniteScrollCollection<View_PRODUITS>();
            // Listing
            LoadItemsCommand = new Command(async () => await ExecuteLoadItemsCommand());
        }

        internal async override Task ExecuteLoadItemsCommand()
        {
            if (IsBusy)
                return;

            try
            {
                UserDialogs.Instance.ShowLoading(AppResources.txt_Waiting);
                await LoadItems(-1,-1);
                UserDialogs.Instance.HideLoading();
            }
            catch (Exception ex)
            {
                UserDialogs.Instance.HideLoading();
                await UserDialogs.Instance.AlertAsync(WSApi2.GetExceptionMessage(ex), AppResources.alrt_msg_Alert,
                    AppResources.alrt_msg_Ok);
            }
            finally
            {
                IsBusy = false;
            }
        }

        public BtqProductViewModel()
        {
            Title = AppResources.pn_Catalogues;
            Types = new ObservableCollection<BSE_TABLE_TYPE>();
            Familles = new ObservableCollection<BSE_TABLE>();

           // LoadExtrasDataCommand = new Command(async () => await ExecuteLoadExtrasDataCommand());
        }

        protected override QueryInfos GetFilterParams()
        {
            base.GetFilterParams();

            if (!string.IsNullOrEmpty(SearchedText))
                this.AddCondition<View_PRODUITS, string>(e => e.DESIGNATION, Operator.LIKE, SearchedText);

            /*
            if (!string.IsNullOrEmpty(SelectedFamille?.CODE))
                this.AddCondition<View_STK_PRODUITS, string>(e => e.CODE_FAMILLE, SelectedFamille?.CODE);

            if (!string.IsNullOrEmpty(SelectedType?.CODE_TYPE))
                this.AddCondition<View_STK_PRODUITS, string>(e => e.TYPE_PRODUIT, SelectedType?.CODE_TYPE);

            if (OnlyNew)
                this.AddCondition<View_STK_PRODUITS, bool>(e => e.IS_NEW, "1");

            this.AddCondition<View_STK_PRODUITS, bool>(e => e.SHOW_CATALOG, "1");
            */

            this.AddOrderBy<View_PRODUITS, string>(e => e.DESIGNATION);

            return qb.QueryInfos;
        }

        #region filters data

        public string SearchedText { get; set; } = "";

        public ObservableCollection<BSE_TABLE_TYPE> Types { get; set; }
        public BSE_TABLE_TYPE SelectedType { get; set; }

        public ObservableCollection<BSE_TABLE> Familles { get; set; }
        public BSE_TABLE SelectedFamille { get; set; }

        async Task ExecuteLoadExtrasDataCommand()
        {

            if (IsLoadExtrasBusy)
                return;

            try
            {
                IsLoadExtrasBusy = true;
                await ExecuteLoadFamillesCommand();
                await ExecuteLoadTypesCommand();
            }
            catch (Exception ex)
            {
                await UserDialogs.Instance.AlertAsync(WSApi2.GetExceptionMessage(ex), AppResources.alrt_msg_Alert,
                    AppResources.alrt_msg_Ok);
            }
            finally
            {
                IsLoadExtrasBusy = false;
            }
        }

        async Task ExecuteLoadTypesCommand()
        {

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
        }

        async Task ExecuteLoadFamillesCommand()
        {
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
        }

        #endregion
    }
}
