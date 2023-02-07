using Acr.UserDialogs;
using Rg.Plugins.Popup.Services;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Timers;
using System.Threading.Tasks;
using Xamarin.Forms;
using Xpert.Common.DAO;
using Xpert.Common.WSClient.Helpers;
using XpertMobileApp.Api.ViewModels;
using XpertMobileApp.DAL;
using XpertMobileApp.Services;
using XpertMobileApp.ViewModels;
using System.Windows.Input;
using XpertMobileApp.Api.Managers;

namespace XpertMobileApp.Views.ViewModels
{
    public class BeneficiaresDetailViewModel : CrudBaseViewModel2<FACTURE_CHIFA, View_CONVENTION_FACTURE>
    {
        public ICommand ModifyTEL1 => new Command(ModifyTEL1_Update);
        public ICommand ModifyTEL2 => new Command(ModifyTEL2_Update);

        private async void ModifyTEL1_Update(object obj)
        {
            PhonePopup popup = new PhonePopup("Modifier la telephone","Annuler","Modifier", Tier.TEL1_TIERS);
            await PopupNavigation.Instance.PushAsync(popup);
            if (await popup.PopupClosedTask)
            {
                if (popup.Result!=""  && popup.Result != Tier.TEL1_TIERS)
                {
                    Tier.TEL1_TIERS = popup.Result;
                    await CrudManager.TiersManager.UpdateItemAsync(Tier);
                }
                else
                {
                    var p = new CustomPopup("Modifier le telephone et resssayer");
                    await PopupNavigation.Instance.PushAsync(p);
                    await p.PopupClosedTask;
                }

            }
        }

        private async void ModifyTEL2_Update(object obj)
        {
            PhonePopup popup = new PhonePopup("Modifier la telephone", "Annuler", "Modifier", Tier.TEL2_TIERS);
            await PopupNavigation.Instance.PushAsync(popup);
            if (await popup.PopupClosedTask)
            {
                if (popup.Result != "" && popup.Result != Tier.TEL2_TIERS)
                {
                    Tier.TEL2_TIERS = popup.Result;
                    await CrudManager.TiersManager.UpdateItemAsync(Tier);
                }
                else
                {
                    var p = new CustomPopup("Modifier le telephone et resssayer");
                    await PopupNavigation.Instance.PushAsync(p);
                    await p.PopupClosedTask;
                }

            }
        }

        private View_CFA_MOBILE_DETAIL_FACTURE item { get; set; }
        public View_CFA_MOBILE_DETAIL_FACTURE Item
        {
            get
            {
                return item;
            }
            set
            {
                item = value;
                OnPropertyChanged("Item");
            }
        }

        private View_TRS_TIERS tier { get; set; }
        public View_TRS_TIERS Tier
        {
            get
            {
                return tier;
            }
            set
            {
                tier = value;
                OnPropertyChanged("Tier");
            }
        }

        private ObservableCollection<View_CFA_MOBILE_FACTURE> items;
        public ObservableCollection<View_CFA_MOBILE_FACTURE> Items
        {
            get
            {
                return items;
            }
            set
            {
                items = value;
                OnPropertyChanged("Items");
            }
        }

        private string title { get; set; }
        public string Title
        {
            get
            {
                return title;
            }
            set
            {
                title = value;
                OnPropertyChanged("Title");
            }
        }
        private View_CFA_MOBILE_FACTURE summary { get; set; }
        public View_CFA_MOBILE_FACTURE Summary
        {
            get
            {
                return summary;
            }
            set
            {
                summary = value;
                OnPropertyChanged("Summary");
            }
        }
        private bool isRefresing { get; set; } = false;
        public bool IsRefreshing
        {
            get
            {
                return isRefresing;
            }
            set
            {
                isRefresing = value;
                OnPropertyChanged("IsRefreshing");
            }
        }

        public Command LoadItemsMoreCommand { get; set; }

        public BeneficiaresDetailViewModel(View_CFA_MOBILE_DETAIL_FACTURE item)
        {
            Title = AppResources.pn_BordereauxChifa;
            LoadItemsMoreCommand = new Command(async () => { await ExecuteLoadMoreItemsCommand(); });
            Item = item;
            Items = new ObservableCollection<View_CFA_MOBILE_FACTURE>();

            Device.InvokeOnMainThreadAsync(async () =>
            {
                var list = await WebServiceClient.GetTier(Item.CODE_TIERS);
                if (list != null || list.Count > 0)
                    Tier = list[0];
            });
        }

        protected override QueryInfos GetSelectParams()
        {
            return base.GetSelectParams();
        }

        protected override QueryInfos GetFilterParams()
        {
            base.GetFilterParams();
            this.AddSelect<View_CONVENTION_FACTURE, string>(e => e.CODE_TIERS);
            this.AddSelect<View_CONVENTION_FACTURE, string>(e => e.NOMC_TIERS);
            this.AddSelect<View_CONVENTION_FACTURE, DateTime>(e => e.DATE_FACTURE);
            this.AddSelect<View_CONVENTION_FACTURE, Decimal>(e => e.MONT_FACTURE);
            this.AddCondition<View_CONVENTION_FACTURE, string>(e => e.CODE_TIERS, Item.CODE_TIERS);
            this.AddCondition<View_CONVENTION_FACTURE, string>(e => e.NOMC_TIERS, Item.NOMC_TIERS);
            this.AddOrderBy<View_CONVENTION_FACTURE, DateTime>(e => e.DATE_FACTURE, Sort.DESC);
            return qb.QueryInfos;
        }

        protected override void OnAfterLoadItems(IEnumerable<View_CONVENTION_FACTURE> list)
        {
            base.OnAfterLoadItems(list);
            View_CFA_MOBILE_DETAIL_FACTURE itemTemp = new View_CFA_MOBILE_DETAIL_FACTURE();
            itemTemp = Item;
            itemTemp.TOTAL_FACTURES = list.Count();

            int i = 0;
            foreach (var item in list)
            {
                i += 1;
                (item as BASE_CLASS).Index = i;
                itemTemp.TOTAL_FACTURES += item.MONT_FACTURE;
            }
            Item = itemTemp;
        }

        internal override async Task ExecuteLoadItemsCommand()
        {
            try
            {
                if (IsBusy)
                    return;
                IsBusy = true;
                Items.Clear();
                UserDialogs.Instance.ShowLoading();
                //await Items.LoadMoreAsync();
                Items = new ObservableCollection<View_CFA_MOBILE_FACTURE>(await WebServiceClient.GetFactChronic(Item.NUM_ASSURE));
                UserDialogs.Instance.HideLoading();
            }
            catch (Exception ex)
            {
                CustomPopup AlertPopup = new CustomPopup(WSApi2.GetExceptionMessage(ex), trueMessage: AppResources.alrt_msg_Ok);
                await PopupNavigation.Instance.PushAsync(AlertPopup);
            }
            finally
            {
                IsBusy = false;
            }
        }
        public async Task ExecuteLoadMoreItemsCommand()
        {
            try
            {
                if (IsBusy)
                    return;
                IsBusy = true;
                UserDialogs.Instance.ShowLoading();
                //await Items.LoadMoreAsync();
                UserDialogs.Instance.HideLoading();
            }
            catch (Exception ex)
            {
                CustomPopup AlertPopup = new CustomPopup(WSApi2.GetExceptionMessage(ex), trueMessage: AppResources.alrt_msg_Ok);
                await PopupNavigation.Instance.PushAsync(AlertPopup);
            }
            finally
            {
                IsBusy = false;
            }
        }

    }
}
