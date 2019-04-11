using Acr.UserDialogs;
using System;
using System.Collections.ObjectModel;
using System.Threading.Tasks;
using Xamarin.Forms;
using Xamarin.Forms.Extended;
using XpertMobileApp.Api.Services;
using XpertMobileApp.DAL;
using XpertMobileApp.Services;


namespace XpertMobileApp.ViewModels
{
    public class ManquantsViewModel : BaseViewModel
    {
        private const int PageSize = 10;

        public DateTime StartDate { get; set; } = DateTime.Now.AddMonths(-3);
        public DateTime EndDate { get; set; } = DateTime.Now;

        private int elementsCount;

        public string SearchedText { get; set; }

        public InfiniteScrollCollection<View_ACH_MANQUANTS> Items { get; set; }
        public View_TRS_TIERS SelectedItem { get; set; }
        public Command LoadItemsCommand { get; set; }

        public ObservableCollection<BSE_DOCUMENT_STATUS> Types { get; set; }
        public BSE_DOCUMENT_STATUS SelectedType { get; set; }
        public Command LoadTypesCommand { get; set; }

        public ObservableCollection<BSE_TABLE_TYPE> TypesProduit { get; set; }
        public BSE_TABLE_TYPE SelectedTypesProduit { get; set; }
        public Command LoadTypesProduitCommand { get; set; }

        public ManquantsViewModel(string title = "")
        {
            Title = AppResources.pn_Manquants;

            Types = new ObservableCollection<BSE_DOCUMENT_STATUS>();
            LoadTypesCommand = new Command(async () => await ExecuteLoadTypesCommand());

            TypesProduit = new ObservableCollection<BSE_TABLE_TYPE>();
            LoadTypesProduitCommand = new Command(async () => await ExecuteLoadTypesProduitCommand());

            LoadItemsCommand = new Command(async () => await ExecuteLoadItemsCommand());

            // chargement infini
            Items = new InfiniteScrollCollection<View_ACH_MANQUANTS>
            {
                OnLoadMore = async () =>
                {
                    try
                    { 

                        IsBusy = true;

                        elementsCount = await WebServiceClient.GetManquantsCount(SelectedType?.CODE_STATUS, SelectedTypesProduit?.CODE_TYPE,
                                             this.SearchedText);

                        // load the next page
                        var page = (Items.Count / PageSize) + 1;
                        var items = await WebServiceClient.GetManquants(page, PageSize, SelectedType?.CODE_STATUS, SelectedTypesProduit?.CODE_TYPE,
                                            this.SearchedText);

                        XpertHelper.UpdateItemIndex(items);

                        IsBusy = false;

                        // return the items that need to be added
                        return items;
                    }
                    catch (Exception ex)
                    {
                        throw new Exception(ex.Message);
                    }
                },
                OnCanLoadMore = () =>
                {
                    return Items.Count < elementsCount;
                }
            };
        }

        async Task ExecuteLoadTypesProduitCommand()
        {
            /*
            if (IsBusy)
             return;

            IsBusy = true;
            */


            try
            {
                TypesProduit.Clear();
                var itemsC = await WebServiceClient.GetProduitTypes();

                BSE_TABLE_TYPE allElem = new BSE_TABLE_TYPE();
                allElem.CODE_TYPE = "";
                allElem.DESIGNATION_TYPE = AppResources.txt_All;
                itemsC.Add(allElem);

                foreach (var itemC in itemsC)
                {
                    TypesProduit.Add(itemC);
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
                var itemsC = await WebServiceClient.getManquantsTypes();

                BSE_DOCUMENT_STATUS allElem = new BSE_DOCUMENT_STATUS();
                allElem.CODE_STATUS = "";
                allElem.NAME = AppResources.txt_All;
                allElem.DESCRIPTION = AppResources.txt_All;
                Types.Add(allElem);

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
               // IsBusy = false;
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
