using Acr.UserDialogs;
using Newtonsoft.Json;
using Rg.Plugins.Popup.Services;
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
using XpertMobileApp.Views.Helper;
using XpertWebApi.Models;

namespace XpertMobileApp.ViewModels
{
    public class VteValidationViewModel : BaseViewModel
    {
        private bool pointFideliteParam;
        public bool PointFideliteParam
        {
            get
            {
                return this.pointFideliteParam;
            }

            set
            {
                if (PointFideliteParam != value)
                {
                    this.pointFideliteParam = value;
                    OnPropertyChanged();
                }
            }
        }

        private View_TRS_TIERS selectedTiers;
        public View_TRS_TIERS SelectedTiers
        {
            get { return selectedTiers; }
            set
            {
                if (SelectedTiers != value)
                {
                    SetProperty(ref selectedTiers, value);
                    Item.CODE_TIERS = value?.CODE_TIERS;
                    Item.NOM_TIERS = value?.NOM_TIERS1;
                }
            }
        }

        private View_VTE_VENTE item;
        public View_VTE_VENTE Item
        {
            get { return item; }
            set { SetProperty(ref item, value); }
        }
        public bool imprimerTecketCaiss { get; set; } = true;
        public VteValidationViewModel(View_VTE_VENTE item, string title = "", View_TRS_TIERS tiers = null)
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

        internal async Task<string> ValidateVte(View_VTE_VENTE item)
        {
            try
            {
                string printerToUse = App.Settings.PrinterName;
                item.MBL_NUM_CARTE_FEDILITE = SelectedTiers.NUM_CARTE_FIDELITE;
                item.CODE_CARTE_FIDELITE = SelectedTiers.CODE_CARTE_FIDELITE;
                item.CODE_MODE = "";
                var bll = CrudManager.GetVteBll(item.TYPE_DOC);


                if (App.Settings.EnableMultiPrinter)
                {
                    List<XPrinter> Liste;
                    if (Manager.isJson(App.Settings.MultiPrinterList))
                    {
                        Liste = JsonConvert.DeserializeObject<List<XPrinter>>(App.Settings.MultiPrinterList);

                        if (Liste != null && Liste.Count != 0)
                        {
                            var popupPrinter = new MultiPrinterSelector(Liste);
                            await PopupNavigation.Instance.PushAsync(popupPrinter);
                            var resPop = await popupPrinter.PopupClosedTask;
                            if (resPop != "Null")
                                printerToUse = resPop;
                        }
                        else await Application.Current.MainPage.DisplayAlert(AppResources.alrt_msg_Alert, AppResources.txt_Msg_List_Impremant_Vide, AppResources.alrt_msg_Ok);
                    }
                    else
                    {
                        await Application.Current.MainPage.DisplayAlert(AppResources.alrt_msg_Alert, AppResources.txt_Msg_List_Impremant_Vide, AppResources.alrt_msg_Ok);
                    }
                }
                UserDialogs.Instance.ShowLoading(AppResources.txt_Waiting);
                var res = await bll.ValidateVente(item, printerToUse);
                UserDialogs.Instance.HideLoading();
                return res;
            }

            catch (Exception ex)
            {
                UserDialogs.Instance.HideLoading();
                if (ex.Message.Contains("ne sont pas valides"))
                {
                    return "Validation avec success mais probleme dans l'impression du ticket !";
                }

                else
                {
                    await UserDialogs.Instance.AlertAsync(WSApi2.GetExceptionMessage(ex), AppResources.alrt_msg_Alert, AppResources.alrt_msg_Ok);
                    return "";
                }
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
                var tiers = await CrudManager.TiersManager.SelectByPage(qb.QueryInfos, 1, 1);
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
