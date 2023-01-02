using Acr.UserDialogs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Xamarin.Forms;
using Xamarin.Forms.Extended;
using Xpert.Common.DAO;
using Xpert.Common.WSClient.Helpers;
using XpertMobileApp.Api.Managers;
using XpertMobileApp.Api.Services;
using XpertMobileApp.Api.ViewModels;
using XpertMobileApp.DAL;
using XpertMobileApp.Models;
using XpertMobileApp.Services;
using XpertMobileApp.Views;

namespace XpertMobileApp.ViewModels
{
    public class RSValidationViewModel : BaseViewModel
    {
        private View_TRS_TIERS selectedTiers;
        public View_TRS_TIERS SelectedTiers
        {
            get { return selectedTiers; }
            set 
            { 
                if(SelectedTiers != value) 
                {  
                    SetProperty(ref selectedTiers, value);
                    Item.CODE_TIERS = value?.CODE_TIERS;
                    Item.NOM_TIERS = value?.NOM_TIERS1;
                }
            }
        }

        private View_STK_ENTREE item;
        public View_STK_ENTREE Item
        {
            get { return item; }
            set { SetProperty(ref item, value); }
        }
        public bool imprimerTecketCaiss { get; set; } = true;
        public RSValidationViewModel(View_STK_ENTREE item, string title= "", View_TRS_TIERS tiers = null)
        {
            Title = title;
            Item = item;
            if (tiers == null) 
            { 
                SelectedTiers = new View_TRS_TIERS()
                {
                    CODE_TIERS = "CXPERTCOMPTOIR",
                    NOM_TIERS1 = "COMPTOIR"
                };
            }
            else 
            {
                SelectedTiers = tiers;
            }
        }

        

        internal async Task SelectScanedTiers(string cb_tiers)
        {
            try
            {
                // Récupérer le lot depuis le serveur
                XpertSqlBuilder qb = new XpertSqlBuilder();
                qb.AddCondition<View_TRS_TIERS, string>(x => x.NUM_CARTE_FIDELITE, cb_tiers);
                qb.AddOrderBy<View_TRS_TIERS, string>(x => x.CODE_TIERS);
                var tiers = await CrudManager.TiersManager.SelectByPage(qb.QueryInfos,1,1);
                if (tiers == null)
                    return;

                XpertHelper.PeepScan();

                if (tiers.Count() > 1)
                {
                    await UserDialogs.Instance.AlertAsync("Plusieurs tiers pour ce code barre!", AppResources.alrt_msg_Alert, AppResources.alrt_msg_Ok);
                }
                else if (tiers.Count() == 0)
                {
                    await UserDialogs.Instance.AlertAsync("Aucun tiers pour ce code barre!", AppResources.alrt_msg_Alert, AppResources.alrt_msg_Ok);
                }
                else 
                { 
                    SelectedTiers = tiers.FirstOrDefault();
                }
            }
            catch (Exception ex)
            {
                await UserDialogs.Instance.AlertAsync(WSApi2.GetExceptionMessage(ex), AppResources.alrt_msg_Alert,
                    AppResources.alrt_msg_Ok);
            }
        }
    }
}
