using System;
using XpertMobileApp.ViewModels;
using XpertMobileApp.DAL;
using System.Collections.ObjectModel;
using Xamarin.Forms;
using System.Threading.Tasks;
using Acr.UserDialogs;
using XpertMobileApp.Api.Managers;
using Xpert.Common.WSClient.Helpers;

namespace XpertMobileApp.ViewModels
{
    public class VerifTiersViewModel : BaseViewModel
    {
        private View_TRS_TIERS tiers;
        public View_TRS_TIERS Tiers
        {
            get { return tiers; }
            set
            {
                tiers = value;
                OnPropertyChanged("Tiers");
            }
        }

        private View_PRD_AGRICULTURE_DETAIL_INFO productionInfos;
        public View_PRD_AGRICULTURE_DETAIL_INFO ProductionInfos
        {
            get { return productionInfos; }
            set
            {
                productionInfos = value;
                OnPropertyChanged("ProductionInfos");
            }
        }
        

        public ObservableCollection<View_PRD_AGRICULTURE_DETAIL> ProductionsDetails { get; set; }

        public Command LoadTiersCommand { get; set; }

        public Command LoadProdInfosCommand { get; set; }

        public VerifTiersViewModel()
        {
            Title = "Vérification";

            LoadProdInfosCommand = new Command(async () => await ExecuteLoadProdInfosCommand(""));
            LoadTiersCommand = new Command(async () => await ExecuteLoadTiersCommand(""));
        }

        public async Task ExecuteLoadProdInfosCommand(string codeDoc)
        {
            if (IsBusy)
                return;

            try
            {
                UserDialogs.Instance.ShowLoading("Traitement en cours ...", MaskType.Black);
                IsBusy = true;
                ProductionInfos = await CrudManager.ProductionInfosManager.GetItemAsync(codeDoc);
            }
            catch (Exception ex)
            {
                await UserDialogs.Instance.AlertAsync(WSApi2.GetExceptionMessage(ex), AppResources.alrt_msg_Alert,
                    AppResources.alrt_msg_Ok);
            }
            finally
            {
                IsBusy = false;
                UserDialogs.Instance.HideLoading();
            }
        }

        public async Task ExecuteLoadTiersCommand(string codeDoc)
        {
            if (IsBusy)
                return;

            try
            {
                IsBusy = true;
                UserDialogs.Instance.ShowLoading("Traitement en cours ...", MaskType.Black);
                Tiers = await CrudManager.TiersManager.GetItemAsync(codeDoc);
            }
            catch (Exception ex)
            {
                await UserDialogs.Instance.AlertAsync(WSApi2.GetExceptionMessage(ex), AppResources.alrt_msg_Alert,
                    AppResources.alrt_msg_Ok);
            }
            finally
            {
                IsBusy = false;
                UserDialogs.Instance.HideLoading();
            }
        }
    }
}
