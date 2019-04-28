using Acr.UserDialogs;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Threading.Tasks;
using Xamarin.Forms;
using Xamarin.Forms.Extended;
using Xpert.Common.WSClient.Helpers;
using XpertMobileApp.Api.Services;
using XpertMobileApp.Api.ViewModels;
using XpertMobileApp.DAL;
using XpertMobileApp.Services;

namespace XpertMobileApp.ViewModels
{
    public class ManquantsViewModel : CrudBaseViewModel<ACH_MANQUANTS, View_ACH_MANQUANTS>
    {

        public ManquantsViewModel()
        {
            Title = AppResources.pn_Manquants;

            Types = new ObservableCollection<BSE_DOCUMENT_STATUS>();

            TypesProduit = new ObservableCollection<BSE_TABLE_TYPE>();

            LoadExtrasDataCommand = new Command(async () => await ExecuteLoadExtrasDataCommand());
        }

        protected override Dictionary<string, string> GetFilterParams()
        {
            Dictionary<string, string> result = base.GetFilterParams();

            result.Add("searchText", SearchedText);

            if (!string.IsNullOrEmpty(SelectedTypesProduit?.CODE_TYPE))
                result.Add("typeProduit", SelectedTypesProduit?.CODE_TYPE);

            if (!string.IsNullOrEmpty(SelectedType?.CODE_STATUS))
                result.Add("type", SelectedType?.CODE_STATUS);

            return result;
        }

        protected override void OnAfterLoadItems(IEnumerable<View_ACH_MANQUANTS> list)
        {
            base.OnAfterLoadItems(list);

            int i = 0;
            foreach (var item in list)
            {
                i += 1;
                (item as BASE_CLASS).Index = i;
            }
        }

        #region filters data

        public DateTime StartDate { get; set; } = DateTime.Now.AddMonths(-3);
        public DateTime EndDate { get; set; } = DateTime.Now;

        public string SearchedText { get; set; }

        public ObservableCollection<BSE_DOCUMENT_STATUS> Types { get; set; }
        public BSE_DOCUMENT_STATUS SelectedType { get; set; }

        public ObservableCollection<BSE_TABLE_TYPE> TypesProduit { get; set; }
        public BSE_TABLE_TYPE SelectedTypesProduit { get; set; }

        async Task ExecuteLoadExtrasDataCommand()
        {

            if (IsLoadExtrasBusy)
                return;

            try
            {
                IsLoadExtrasBusy = true;
                await ExecuteLoadTypesProduitCommand();
                await ExecuteLoadTypesCommand();
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

        async Task ExecuteLoadTypesProduitCommand()
        {

            try
            {
                TypesProduit.Clear();
                var itemsC = await WebServiceClient.GetProduitTypes();

                BSE_TABLE_TYPE allElem = new BSE_TABLE_TYPE();
                allElem.CODE_TYPE = "";
                allElem.DESIGNATION_TYPE = AppResources.txt_All;
                itemsC.Add(allElem);

                foreach (var itemC in itemsC)
                {
                    TypesProduit.Add(itemC);
                }
            }
            catch (Exception ex)
            {
                await UserDialogs.Instance.AlertAsync(WSApi2.GetExceptionMessage(ex), AppResources.alrt_msg_Alert,
                    AppResources.alrt_msg_Ok);
            }

        }

        async Task ExecuteLoadTypesCommand()
        {

            try
            {
                Types.Clear();
                var itemsC = await WebServiceClient.getManquantsTypes();

                BSE_DOCUMENT_STATUS allElem = new BSE_DOCUMENT_STATUS();
                allElem.CODE_STATUS = "";
                allElem.NAME = AppResources.txt_All;
                allElem.DESCRIPTION = AppResources.txt_All;
                Types.Add(allElem);

                foreach (var itemC in itemsC)
                {
                    Types.Add(itemC);
                }
            }
            catch (Exception ex)
            {
                await UserDialogs.Instance.AlertAsync(WSApi2.GetExceptionMessage(ex), AppResources.alrt_msg_Alert,
                    AppResources.alrt_msg_Ok);
            }
        }

        #endregion
    }
}
