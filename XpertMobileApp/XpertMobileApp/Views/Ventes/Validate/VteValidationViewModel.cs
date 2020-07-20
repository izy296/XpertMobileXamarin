using Acr.UserDialogs;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Xamarin.Forms;
using Xamarin.Forms.Extended;
using Xpert.Common.DAO;
using Xpert.Common.WSClient.Helpers;
using XpertMobileApp.Api.Managers;
using XpertMobileApp.Api.ViewModels;
using XpertMobileApp.DAL;
using XpertMobileApp.Models;
using XpertMobileApp.Services;
using XpertMobileApp.Views;

namespace XpertMobileApp.ViewModels
{
    public class VteValidationViewModel : BaseViewModel
    {
        private View_TRS_TIERS selectedTiers;
        public View_TRS_TIERS SelectedTiers
        {
            get { return selectedTiers; }
            set { SetProperty(ref selectedTiers, value); }
        }

        private View_VTE_VENTE item;
        public View_VTE_VENTE Item
        {
            get { return item; }
            set { SetProperty(ref item, value); }
        }

        public VteValidationViewModel(string title= "" )
        {
            Title = title;
        }

        internal async Task<bool> ValidateVte(View_VTE_VENTE item)
        {
            try
            {
                var bll = CrudManager.GetVteBll(item.TYPE_DOC);

                item.MBL_NUM_CARTE_FEDILITE = "";
                item.CODE_MODE = "";

                var res = await bll.ValidateVente(item, "1");
                return res;
            }
            catch (Exception ex) 
            {
                await UserDialogs.Instance.AlertAsync(WSApi2.GetExceptionMessage(ex), AppResources.alrt_msg_Alert, AppResources.alrt_msg_Ok);
                return false;
            }
        }
    }
}
