using Acr.UserDialogs;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Globalization;
using System.Threading.Tasks;
using Xamarin.Forms;
using Xamarin.Forms.Extended;
using Xpert.Common.WSClient.Helpers;
using XpertMobileApp.Api.Services;
using XpertMobileApp.Api.ViewModels;
using XpertMobileApp.DAL;
using XpertMobileApp.Helpers;
using XpertMobileApp.Services;
using XpertMobileApp.Views.Encaissement;

namespace XpertMobileApp.ViewModels
{
    public enum EncaissDisplayType { None, All, ENC, DEC };

    public class EncaissementsViewModel : CrudBaseViewModel<TRS_ENCAISS, View_TRS_ENCAISS>
    {

        public EncaissDisplayType EncaissDisplayType { get; set; }

        public DateTime StartDate { get; set; } = DateTime.Now;

        public DateTime EndDate { get; set; } = DateTime.Now;

        public ObservableCollection<View_BSE_COMPTE> Comptes { get; set; }
        public View_BSE_COMPTE SelectedCompte { get; set; }

        public EncaissementsViewModel()
        {
            Title = AppResources.pn_encaissement;

            Comptes = new ObservableCollection<View_BSE_COMPTE>();
            LoadExtrasDataCommand = new Command(async () => await ExecuteLoadExtrasDataCommand());
        }

        protected override Dictionary<string, string> GetFilterParams()
        {

            Dictionary<string, string> result = base.GetFilterParams();
            string type = GetCurrentType();

            result.Add("type", type);            
            result.Add("startDate", WSApi2.GetStartDateQuery(StartDate));
            result.Add("endDate", WSApi2.GetEndDateQuery(EndDate));
            if(!string.IsNullOrEmpty(SelectedCompte?.CODE_COMPTE))
                result.Add("codeCompte", SelectedCompte?.CODE_COMPTE);

            // result.Add("id_caisse", "all");
            // result.Add("codeMotif", "all");
            // result.Add("codeTiers", "all");

            return result;
        }

        protected override void OnAfterLoadItems(IEnumerable<View_TRS_ENCAISS> list)
        {
            base.OnAfterLoadItems(list);

            int i = 0;
            foreach (var item in list)
            {
                i += 1;
                (item as BASE_CLASS).Index = i;
            }
        }

        private string GetCurrentType()
        {
            string type = "";
            switch (EncaissDisplayType)
            {
                case EncaissDisplayType.All:
                    type = "all";
                    break;
                case EncaissDisplayType.ENC:
                    type = "ENC";
                    break;
                case EncaissDisplayType.DEC:
                    type = "DEC";
                    break;
            }

            return type;
        }

        async Task ExecuteLoadExtrasDataCommand()
        {
            
            if (IsLoadExtrasBusy)
                return;
            
            try
            {
                IsLoadExtrasBusy = true;
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
                IsLoadExtrasBusy = false;
            }
        }
    }

}
