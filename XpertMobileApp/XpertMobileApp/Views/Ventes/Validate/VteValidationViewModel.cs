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
        public View_TRS_TIERS SelectedTiers { get; set; }
        public View_VTE_VENTE Item { get; set; }
        public VteValidationViewModel(string title= "" )
        {
            Title = title;
        }

        internal async void ValidateVte(View_VTE_VENTE item)
        {
            var bll = CrudManager.GetVteBll(item.TYPE_DOC);

            item.MBL_NUM_CARTE_FEDILITE = "";
            item.CODE_MODE = "";

            var res = await bll.ValidateVente(item, "1");
        }
    }
}
