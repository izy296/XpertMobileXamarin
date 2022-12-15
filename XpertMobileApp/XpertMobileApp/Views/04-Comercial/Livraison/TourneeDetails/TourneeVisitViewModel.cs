using Acr.UserDialogs;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using Xpert.Common.WSClient.Helpers;
using XpertMobileApp.Api.Managers;
using XpertMobileApp.Api.ViewModels;
using XpertMobileApp.DAL;
using XpertMobileApp.SQLite_Managment;

namespace XpertMobileApp.ViewModels
{
    class TourneeVisitViewModel : CrudBaseViewModel2<LIV_TOURNEE_DETAIL, View_LIV_TOURNEE_DETAIL>
    {
        private string title;
        public string Title
        {
            get { return title; }
            set { SetProperty(ref title, value); }
        }

        private View_LIV_TOURNEE_DETAIL item;
        public View_LIV_TOURNEE_DETAIL Item
        {
            get { return item; }
            set { SetProperty(ref item, value); }
        }

        private List<View_TRS_TIERS_ACTIVITY> activitys;
        public List<View_TRS_TIERS_ACTIVITY> Activitys
        {
            get { return activitys; }
            set { SetProperty(ref activitys, value); }
        }
        public TourneeVisitViewModel()
        {
            Title = "";
        }

        public TourneeVisitViewModel(View_LIV_TOURNEE_DETAIL Item)
        {
            this.Item = Item;
            Title = this.Item.FULL_NOM_TIERS;
        }

        public async Task ExecuteLoadActiviteCommand()
        {
            if (IsBusy)
                return;

            try
            {
                IsBusy = true;
                var Activity = await SQLite_Manager.Get_TRS_TIERS_ACTIVITY_Async(Item.CODE_TIERS);
                var updatedTierSolde = await SQLite_Manager.GetClient(Item.CODE_TIERS);
                if(updatedTierSolde != null)
                {
                    Item.SOLDE_TIERS = updatedTierSolde.SOLDE_TIERS;
                }
                if (Activity != null)
                    Activitys = Activity as List<View_TRS_TIERS_ACTIVITY>;
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

        public async Task UpdateVisteStatus(TourneeStatus selectedItem,bool tourneClosed=false)
        {
            bool modified = false;
            if (Item.CODE_ETAT_VISITE != selectedItem)
            {

                var answer = await App.Current.MainPage.DisplayAlert(AppResources.visiteStatusChangeConfirm, AppResources.alrt_msg_Alert, AppResources.exit_Button_Yes, AppResources.exit_Button_No);
                if (answer)
                {
                    if (!tourneClosed)
                    {
                        Item.CODE_ETAT_VISITE = selectedItem;
                        modified = true;
                    }
                    else
                    {
                        await UserDialogs.Instance.AlertAsync(AppResources.tourneeClosedMessage, AppResources.alrt_msg_Alert,
    AppResources.alrt_msg_Ok);
                    }
                }
            }

            if (modified)
                if (App.Online)
                {
                    await CrudManager.TourneeDetails.UpdateItemAsync(Item);
                }
                else
                {
                    await SQLite_Manager.GetInstance().UpdateAsync(Item);
                }
        }
    }
}
