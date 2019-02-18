using Acr.UserDialogs;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.Text;
using System.Threading.Tasks;
using Xamarin.Forms;
using Xamarin.Forms.Extended;
using Xpert.Pharm.DAL;
using XpertMobileApp.DAL;
using XpertMobileApp.Helpers;
using XpertMobileApp.Services;
using XpertMobileApp.Views.Encaissement;

namespace XpertMobileApp.ViewModels
{

    public class VentesViewModel : BaseViewModel
    {
        private const int PageSize = 10;

        private int elementsCount;

        public View_TRS_TIERS SelectedTiers { get; set; }

        public EncaissDisplayType EncaissDisplayType { get; set; }
        public DateTime StartDate { get; set; } = DateTime.Now.AddDays(-90);
        public DateTime EndDate { get; set; } = DateTime.Now.AddDays(1);

        public InfiniteScrollCollection<View_VTE_VENTE> Items { get; set; }
        public Command LoadItemsCommand { get; set; }

        public ObservableCollection<View_BSE_COMPTE> Comptes { get; set; }
        public View_BSE_COMPTE SelectedCompte { get; set; }
        public Command LoadComptesCommand { get; set; }

        public ObservableCollection<View_BSE_COMPTE> Client { get; set; }
        public View_BSE_COMPTE SelectedClient { get; set; }
        public Command LoadClientsCommand { get; set; }

        public ObservableCollection<View_BSE_COMPTE> User { get; set; }
        public View_BSE_COMPTE SelectedUser { get; set; }
        public Command LoadUsersCommand { get; set; }

        public VentesViewModel()
        {
            Title = AppResources.pn_Ventes;

            // Listing
            LoadItemsCommand = new Command(async () => await ExecuteLoadItemsCommand());

            // chargement infini
            Items = new InfiniteScrollCollection<View_VTE_VENTE>
            {
                OnLoadMore = async () =>
                {
                    IsBusy = true;
                    
                    elementsCount = await WebServiceClient.GetVentesCount("all", "all", StartDate, EndDate, SelectedTiers?.CODE_TIERS, "", SelectedCompte?.CODE_COMPTE);

                    // load the next page
                    var page = (Items.Count / PageSize) + 1;
                    var items = await WebServiceClient.GetVentes("all", page.ToString(), "all", StartDate, EndDate, SelectedTiers?.CODE_TIERS, "", SelectedCompte?.CODE_COMPTE);
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
                await UserDialogs.Instance.AlertAsync(ex.Message, AppResources.alrt_msg_Alert,
                    AppResources.alrt_msg_Ok);
            }
            finally
            {
                IsBusy = false;
            }
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
                await UserDialogs.Instance.AlertAsync(ex.Message, AppResources.alrt_msg_Alert,
                    AppResources.alrt_msg_Ok);
            }
            finally
            {
                IsBusy = false;
            }
        }
    }

}
