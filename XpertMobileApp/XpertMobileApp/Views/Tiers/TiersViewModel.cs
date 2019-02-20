using Acr.UserDialogs;
using System;
using System.Collections.ObjectModel;
using System.Threading.Tasks;
using Xamarin.Forms;
using Xamarin.Forms.Extended;
using Xpert.Pharm.DAL;
using XpertMobileApp.Api.Services;
using XpertMobileApp.Services;


namespace XpertMobileApp.ViewModels
    {
        public class TiersViewModel : BaseViewModel
        {
            private const int PageSize = 10;

            private int elementsCount;

            public string SearchedText { get; set; }

            public InfiniteScrollCollection<View_TRS_TIERS> Items { get; set; }
            public View_TRS_TIERS SelectedItem { get; set; }
            public Command LoadItemsCommand { get; set; }

            public ObservableCollection<View_BSE_TIERS_FAMILLE> Familles { get; set; }
            public View_BSE_TIERS_FAMILLE SelectedFamille { get; set; }
            public Command LoadFamillesCommand { get; set; }

            public ObservableCollection<BSE_TABLE_TYPE> Types { get; set; }
            public BSE_TABLE_TYPE SelectedType { get; set; }
            public Command LoadTypesCommand { get; set; }

        public TiersViewModel(string title = "")
            {
                Title = AppResources.pn_Tiers;

                Familles = new ObservableCollection<View_BSE_TIERS_FAMILLE>();
                LoadFamillesCommand = new Command(async () => await ExecuteLoadFamillesCommand());

                Types = new ObservableCollection<BSE_TABLE_TYPE>();
                LoadTypesCommand = new Command(async () => await ExecuteLoadTypesCommand());

                LoadItemsCommand = new Command(async () => await ExecuteLoadItemsCommand());

                // chargement infini
                Items = new InfiniteScrollCollection<View_TRS_TIERS>
                {
                    OnLoadMore = async () =>
                    {
                        IsBusy = true;

                        elementsCount = await WebServiceClient.GetTiersCount(SelectedType?.CODE_TYPE, 
                                             SelectedFamille?.CODE_FAMILLE, this.SearchedText);

                        // load the next page
                        var page = (Items.Count / PageSize) + 1;
                        var items = await WebServiceClient.GetTiers(page, PageSize, SelectedType?.CODE_TYPE, 
                                            SelectedFamille?.CODE_FAMILLE, this.SearchedText);

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

        async Task ExecuteLoadTypesCommand()
        {
            /*
            if (IsBusy)
                return;
            */

            IsBusy = true;

            try
            {
                Types.Clear();
                var itemsC = await WebServiceClient.getTiersTypes();
                foreach (var itemC in itemsC)
                {
                    Types.Add(itemC);
                }
            }
            catch (Exception ex)
            {
                await UserDialogs.Instance.AlertAsync(ex.Message, AppResources.alrt_msg_Alert,
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
            */

            IsBusy = true;

            try
            {
                Familles.Clear();
                var itemsC = await WebServiceClient.getTiersFamilles();
                foreach (var itemC in itemsC)
                {
                    Familles.Add(itemC);
                }
            }
            catch (Exception ex)
            {
                await UserDialogs.Instance.AlertAsync(ex.Message, AppResources.alrt_msg_Alert,
                    AppResources.alrt_msg_Ok);
            }
            finally
            {
                IsBusy = false;
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
                    await UserDialogs.Instance.AlertAsync(ex.Message, AppResources.alrt_msg_Alert,
                        AppResources.alrt_msg_Ok);
                }
                finally
                {

                }

            }


        }
    }
