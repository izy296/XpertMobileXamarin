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
     public class TiersViewModel : CrudBaseViewModel<TRS_TIERS, View_TRS_TIERS>
     {
        
         public TiersViewModel()
         {
             Title = AppResources.pn_Tiers;

             Familles = new ObservableCollection<View_BSE_TIERS_FAMILLE>();
             Types = new ObservableCollection<BSE_TABLE_TYPE>();

             LoadExtrasDataCommand = new Command(async () => await ExecuteLoadExtrasDataCommand());
         }

         protected override Dictionary<string, string> GetFilterParams()
         {
             Dictionary<string, string> result = base.GetFilterParams();

             result.Add("searchText", SearchedText);

             if (!string.IsNullOrEmpty(SelectedFamille?.CODE_FAMILLE))
                 result.Add("famille", SelectedFamille?.CODE_FAMILLE);

             if (!string.IsNullOrEmpty(SelectedType?.CODE_TYPE))
                 result.Add("type", SelectedType?.CODE_TYPE);

             return result;
         }

         protected override void OnAfterLoadItems(IEnumerable<View_TRS_TIERS> list)
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

         public string SearchedText { get; set; }

         public ObservableCollection<View_BSE_TIERS_FAMILLE> Familles { get; set; }
         public View_BSE_TIERS_FAMILLE SelectedFamille { get; set; }

         public ObservableCollection<BSE_TABLE_TYPE> Types { get; set; }
         public BSE_TABLE_TYPE SelectedType { get; set; }

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
                 var itemsC = await WebServiceClient.getTiersTypes();

                 BSE_TABLE_TYPE allElem = new BSE_TABLE_TYPE();
                 allElem.CODE_TYPE = "";
                 allElem.DESIGNATION_TYPE = AppResources.txt_All;
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

         async Task ExecuteLoadFamillesCommand()
         {

             try
             {
                 Familles.Clear();
                 var itemsC = await WebServiceClient.getTiersFamilles();

                 View_BSE_TIERS_FAMILLE allElem = new View_BSE_TIERS_FAMILLE();
                 allElem.CODE_FAMILLE = "";
                 allElem.DESIGN_FAMILLE = AppResources.txt_All;
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
