using Acr.UserDialogs;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Threading.Tasks;
using Xamarin.Forms;
using Xpert.Common.WSClient.Helpers;
using XpertMobileApp.Api.ViewModels;
using XpertMobileApp.DAL;
using XpertMobileApp.Services;

namespace XpertMobileApp.ViewModels
{

    public class SessionsViewModel : CrudBaseViewModel<TRS_JOURNEES, TRS_JOURNEES>
    {

        EncaissDisplayType encaissDisplayType;
        public EncaissDisplayType EncaissDisplayType
        {
            get { return encaissDisplayType; }
            set { SetProperty(ref encaissDisplayType, value); }
        }

        DateTime startDate = DateTime.Now.AddDays(-15);
        public DateTime StartDate
        {
            get { return startDate; }
            set { SetProperty(ref startDate, value); }
        }

        DateTime endDate = DateTime.Now;
        public DateTime EndDate
        {
            get { return endDate; }
            set { SetProperty(ref endDate, value); }
        }

        public ObservableCollection<View_BSE_COMPTE> Comptes { get; set; }
        View_BSE_COMPTE selectedCompte;
        public View_BSE_COMPTE SelectedCompte
        {
            get { return selectedCompte; }
            set { SetProperty(ref selectedCompte, value); }
        }

        public override void ClearFilters()
        {
            base.ClearFilters();
            StartDate = DateTime.Now.AddDays(-15);
            EndDate = DateTime.Now;
            SelectedCompte = null;
        }

        public SessionsViewModel()
        {
            Title = AppResources.pn_session;

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
            result.Add("onlyOpned", "");
            
            if (!string.IsNullOrEmpty(SelectedCompte?.CODE_COMPTE))
                result.Add("compte", SelectedCompte?.CODE_COMPTE);

            // result.Add("id_caisse", "all");
            // result.Add("codeMotif", "all");
            // result.Add("codeTiers", "all");

            return result;
        }

        protected override void OnAfterLoadItems(IEnumerable<TRS_JOURNEES> list)
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
