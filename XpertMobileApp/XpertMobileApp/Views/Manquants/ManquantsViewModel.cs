using Acr.UserDialogs;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Threading.Tasks;
using Xamarin.Forms;
using Xamarin.Forms.Extended;
using Xpert.Common.DAO;
using Xpert.Common.WSClient.Helpers;
using XpertMobileApp.Api.Services;
using XpertMobileApp.Api.ViewModels;
using XpertMobileApp.DAL;
using XpertMobileApp.Services;

namespace XpertMobileApp.ViewModels
{
    public class ManquantsViewModel : CrudBaseViewModel2<ACH_MANQUANTS, View_ACH_MANQUANTS>
    {

        public ManquantsViewModel()
        {
            Title = AppResources.pn_Manquants;

            Types = new ObservableCollection<BSE_DOCUMENT_STATUS>();

            TypesProduit = new ObservableCollection<BSE_TABLE_TYPE>();

            LoadExtrasDataCommand = new Command(async () => await ExecuteLoadExtrasDataCommand());
        }

        protected override QueryInfos GetFilterParams()
        {
            base.GetFilterParams();

            this.AddCondition<View_ACH_MANQUANTS, string>(e => e.DESIGNATION_PRODUIT, Operator.LIKE_ANY, SearchedText);

            if (!string.IsNullOrEmpty(SelectedTypesProduit?.CODE_TYPE))
                this.AddCondition<View_ACH_MANQUANTS, short>(e => e.TYPE_PRODUIT, SelectedTypesProduit?.CODE_TYPE); 


            if (!string.IsNullOrEmpty(SelectedType?.CODE_STATUS))
                this.AddCondition<View_ACH_MANQUANTS, string>(e => e.CODE_TYPE, SelectedType?.CODE_STATUS);

            qb.AddOrderBy<View_ACH_MANQUANTS, string>(e => e.DESIGNATION_PRODUIT);

            return qb.QueryInfos;
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
                TypesProduit.Add(allElem);

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
