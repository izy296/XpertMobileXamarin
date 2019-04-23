using Acr.UserDialogs;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Threading.Tasks;
using Xamarin.Forms;
using Xamarin.Forms.Extended;
using Xpert.Common.WSClient.Helpers;
using XpertMobileApp.DAL;
using XpertMobileApp.Services;

namespace XpertMobileApp.ViewModels
{

    public class VentesViewModel : BaseViewModel
    {
        private const int PageSize = 10;

        private int elementsCount;

        decimal totalTurnover;
        public decimal TotalTurnover
        {
            get { return totalTurnover; }
            set { SetProperty(ref totalTurnover, value); }
        }

        decimal totalMargin;
        public decimal TotalMargin
        {
            get { return totalMargin; }
            set { SetProperty(ref totalMargin, value); }
        }

        public View_TRS_TIERS SelectedTiers { get; set; }

        public EncaissDisplayType EncaissDisplayType { get; set; }
        public DateTime StartDate { get; set; } = DateTime.Now;
        public DateTime EndDate { get; set; } = DateTime.Now;

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

                // Infos de la journée
                DateTime endDate = DateTime.Now;
                DateTime startDate = DateTime.Now;
                STAT_VTE_BY_USER stat = await WebServiceClient.GetTotalMargeParVendeur(startDate, endDate);
                TotalTurnover = stat.MONTANT_VENTE;
                TotalMargin = stat.MONTANT_MARGE;

                // liste des ventes
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
