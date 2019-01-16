using Acr.UserDialogs;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xamarin.Forms;
using Xamarin.Forms.Extended;
using Xpert.Pharm.DAL;
using XpertMobileApp.Services;
using XpertMobileApp.ViewModels;

namespace XpertMobileApp.ViewModels
{
    public class ListViewSelectorViewModel : BaseViewModel
    {
        private const int PageSize = 10;

        private int elementsCount;

        private string SearchText ="";

        public InfiniteScrollCollection<View_TRS_TIERS> Items { get; set; }
        public View_TRS_TIERS SelectedItem { get; set; }
        public Command LoadItemsCommand { get; set; }


        public ListViewSelectorViewModel(string title= "" )
        {
            Title = title;

            LoadItemsCommand = new Command(async () => await ExecuteLoadItemsCommand());

            // chargement infini
            Items = new InfiniteScrollCollection<View_TRS_TIERS>
            {
                OnLoadMore = async () =>
                {
                    IsBusy = true;

                    elementsCount = await WebServiceClient.GetTiersCount("C", this.SearchText);

                    // load the next page
                    var page = (Items.Count / PageSize) + 1;
                    var items = await WebServiceClient.GetTiers("C", page, PageSize, this.SearchText);

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

        public async Task FilterItems(string txt = "")
        {
            this.SearchText = txt;

            Items.Clear();
            await Items.LoadMoreAsync();
        }
    }
}
