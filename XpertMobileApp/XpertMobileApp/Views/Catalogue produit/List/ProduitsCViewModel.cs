using Acr.UserDialogs;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Threading.Tasks;
using Xamarin.Forms;
using Xpert.Common.DAO;
using Xpert.Common.WSClient.Helpers;
using XpertMobileApp.Api.ViewModels;
using XpertMobileApp.DAL;
using XpertMobileApp.Services;

namespace XpertMobileApp.ViewModels
{

    public class ProduitsCViewModel : CrudBaseViewModel2<STK_PRODUITS, View_STK_PRODUITS>
    {
        public ProduitsCViewModel()
        {
            Title = AppResources.pn_Produits;

            Types = new ObservableCollection<BSE_TABLE_TYPE>();
            Familles = new ObservableCollection<BSE_TABLE>();

            LoadExtrasDataCommand = new Command(async () => await ExecuteLoadExtrasDataCommand());
        }

        protected override QueryInfos GetFilterParams()
        {
            base.GetFilterParams();

            // this.AddSelect<View_STK_STOCK, View_STK_STOCK>(e=>e.)

            this.AddCondition<View_STK_PRODUITS, string>(e => e.DESIGNATION_PRODUIT, Operator.LIKE_ANY, SearchedText);

            if (!string.IsNullOrEmpty(SelectedFamille?.CODE))
                this.AddCondition<View_STK_PRODUITS, string>(e => e.CODE_FAMILLE, SelectedFamille?.CODE);

            if (!string.IsNullOrEmpty(SelectedType?.CODE_TYPE))
                this.AddCondition<View_STK_PRODUITS, string>(e => e.TYPE_PRODUIT, SelectedType?.CODE_TYPE);

            this.AddOrderBy<View_STK_PRODUITS, string>(e => e.DESIGNATION_PRODUIT);
            return qb.QueryInfos;
        }

        protected override void OnAfterLoadItems(IEnumerable<View_STK_PRODUITS> list)
        {
            base.OnAfterLoadItems(list);

            int i = 0;
            foreach (var item in list)
            {
                i += 1;
                item.IMAGE_URL = App.RestServiceUrl.Replace("api/","") + string.Format("Images/GetImage?codeProduit={0}", item.CODE_PRODUIT);
                (item as BASE_CLASS).Index = i;
            }
        }

        #region filters data

        public string SearchedText { get; set; } = "";

        public ObservableCollection<BSE_TABLE_TYPE> Types { get; set; }
        public BSE_TABLE_TYPE SelectedType { get; set; }

        public ObservableCollection<BSE_TABLE> Familles { get; set; }
        public BSE_TABLE SelectedFamille { get; set; }

        async Task ExecuteLoadExtrasDataCommand()
        {

            if (IsLoadExtrasBusy)
                return;

            try
            {
                IsLoadExtrasBusy = true;
                await ExecuteLoadFamillesCommand();
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

        async Task ExecuteLoadTypesCommand()
        {

            try
            {
                Types.Clear();
                var itemsC = await WebServiceClient.GetProduitTypes();

                BSE_TABLE_TYPE allElem = new BSE_TABLE_TYPE();
                allElem.CODE_TYPE = "";
                allElem.DESIGNATION_TYPE = AppResources.txt_All;
                itemsC.Add(allElem);

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

        async Task ExecuteLoadFamillesCommand()
        {
            try
            {
                Familles.Clear();
                var itemsC = await WebServiceClient.GetProduitFamilles();

                BSE_TABLE allElem = new BSE_TABLE();
                allElem.CODE = "";
                allElem.DESIGNATION = AppResources.txt_All;
                Familles.Add(allElem);

                foreach (var itemC in itemsC)
                {
                    Familles.Add(itemC);
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
