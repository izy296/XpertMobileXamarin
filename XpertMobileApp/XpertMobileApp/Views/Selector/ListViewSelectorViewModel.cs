using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xamarin.Forms;
using Xpert.Pharm.DAL;
using XpertMobileApp.Services;
using XpertMobileApp.ViewModels;

namespace XpertMobileApp.ViewModels
{
    public class ListViewSelectorViewModel : BaseViewModel
    {
        public ObservableCollection<View_TRS_TIERS> AllItems { get; set; }
        public ObservableCollection<View_TRS_TIERS> Items { get; set; }
        public View_TRS_TIERS SelectedItem { get; set; }
        public Command LoadItemsCommand { get; set; }


        public ListViewSelectorViewModel()
        {
            Title = AppResources.pn_encaissement;

            AllItems = new ObservableCollection<View_TRS_TIERS>();
            Items = new ObservableCollection<View_TRS_TIERS>();

            LoadItemsCommand = new Command(async () => await ExecuteLoadItemsCommand());
        }

        async Task ExecuteLoadItemsCommand()
        {

            if (IsBusy)
                return;


            IsBusy = true;

            try
            {
                AllItems.Clear();
                var itemsC = await WebServiceClient.GetTiers("C");
                foreach (var itemC in itemsC)
                {
                    AllItems.Add(itemC);
                }
                FilterItems();
            }
            catch (Exception ex)
            {
                Debug.WriteLine(ex);
            }
            finally
            {
                IsBusy = false;
            }

        }

        public void FilterItems(string txt = "")
        {
            Items.Clear();
            foreach (var item in AllItems.Where(x => x.NOM_TIERS1.Contains(txt) || txt == ""))
            {
                Items.Add(item);
            }
        }
    }
}
