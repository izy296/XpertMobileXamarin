using Acr.UserDialogs;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Threading.Tasks;
using Xamarin.Forms;
using Xamarin.Forms.Extended;
using Xpert.Common.DAO;
using Xpert.Common.WSClient.Helpers;
using XpertMobileApp.Api;
using XpertMobileApp.Api.Services;
using XpertMobileApp.Api.ViewModels;
using XpertMobileApp.DAL;
using XpertMobileApp.Services;
using XpertMobileApp.SQLite_Managment;

namespace XpertMobileApp.ViewModels
{
     public class TiersViewModel : CrudBaseViewModel2<TRS_TIERS, View_TRS_TIERS>
     {

        public bool hasViewSolde
        {
            get
            {
                if (AppManager.HasAdmin)
                {
                    return true;
                }
                bool result = false;
                if (AppManager.permissions != null)
                {
                    var obj = AppManager.permissions.Where(x => x.CodeObjet == "BSE_TIERS_FAMILLE_SOLDE").FirstOrDefault();
                    result = obj != null && obj.AcSelect > 0;
                }
                return result;
            }
        }

        public TiersViewModel()
         {
             Title = AppResources.pn_Tiers;

             Familles = new ObservableCollection<View_BSE_TIERS_FAMILLE>();
             Types = new ObservableCollection<BSE_TABLE_TYPE>();

             LoadExtrasDataCommand = new Command(async () => await ExecuteLoadExtrasDataCommand());
         }

         protected override QueryInfos GetFilterParams()
         {
            base.GetFilterParams();

            this.AddCondition<View_TRS_TIERS, string>(e => e.NOM_TIERS1, Operator.LIKE_ANY, SearchedText);

            this.AddCondition<View_TRS_TIERS, short>(e => e.ACTIF_TIERS, 1);
            if (SoldOperator == ">")
                this.AddCondition<View_TRS_TIERS, decimal>(e => e.SOLDE_TIERS, Operator.GREATER, 0);
            else if (SoldOperator == "<")
                this.AddCondition<View_TRS_TIERS, decimal>(e => e.SOLDE_TIERS, Operator.LESS, 0);
            else if (SoldOperator == "=")
                this.AddCondition<View_TRS_TIERS, decimal>(e => e.SOLDE_TIERS, Operator.EQUAL, 0);

            if (!string.IsNullOrEmpty(SelectedFamille?.CODE_FAMILLE))
                this.AddCondition<View_TRS_TIERS, string>(e => e.CODE_FAMILLE, SelectedFamille?.CODE_FAMILLE);

             if (!string.IsNullOrEmpty(SelectedType?.CODE_TYPE))
                this.AddCondition<View_TRS_TIERS, string>(e => e.CODE_TYPE, SelectedType?.CODE_TYPE);

            qb.AddOrderBy<View_TRS_TIERS, string>(e => e.NOM_TIERS1);

            return qb.QueryInfos;
        }

         protected override void OnAfterLoadItems(IEnumerable<View_TRS_TIERS> list)
         {
            base.OnAfterLoadItems(list);

            int i = 0;
            foreach (var item in list)
            {
                i += 1;
                (item as BASE_CLASS).Index = i;
                if (hasViewSolde)
                { 
                    item.SOLDE_TIERS_TXT = hasViewSolde ? item.SOLDE_TIERS.ToString("N2") + " DA" : "";
                }
                else
                {
                    item.SOLDE_TIERS_TXT = "tél. " + item.TEL1_TIERS;
                }
            }
         }

        #region filters data

        private string searchedText;
        public string SearchedText
        {
            get { return searchedText; }
            set { SetProperty(ref searchedText, value); }
        }

        private string soldOperator;
        public string SoldOperator
        {
            get { return soldOperator; }
            set { SetProperty(ref soldOperator, value); }
        }
        public ObservableCollection<View_BSE_TIERS_FAMILLE> Familles { get; set; }
    
         View_BSE_TIERS_FAMILLE selectedFamille;
         public View_BSE_TIERS_FAMILLE SelectedFamille
         {
             get { return selectedFamille; }
             set { SetProperty(ref selectedFamille, value); }
         }

         public ObservableCollection<BSE_TABLE_TYPE> Types { get; set; }
         private BSE_TABLE_TYPE selectedType;
         public BSE_TABLE_TYPE SelectedType
         {
            get { return selectedType; }
            set { SetProperty(ref selectedType, value); }
         }
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
            if (App.Online)
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
            else
            {
                Types.Clear();
                var itemsC = await SQLite_Manager.getTypeTiers();
                foreach (var itemC in itemsC)
                {
                    Types.Add(itemC);
                }
            }
             
         }

         async Task ExecuteLoadFamillesCommand()
         {
            if (App.Online)
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
            else
            {
                Familles.Clear();
                var itemsC = await SQLite_Manager.getFamille();
                foreach (var itemC in itemsC)
                {
                    Familles.Add(itemC);
                }
            }
         }

        public override void ClearFilters()
        {
            base.ClearFilters();
            SearchedText = "";
            SelectedFamille = null;
            SelectedType = null;
            SoldOperator = "";
        }

        #endregion
    }
}
