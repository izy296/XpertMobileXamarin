using Acr.UserDialogs;
using Syncfusion.Linq;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Threading.Tasks;
using Xamarin.Forms;
using Xpert.Common.DAO;
using Xpert.Common.WSClient.Helpers;
using XpertMobileApp.Api;
using XpertMobileApp.Api.Managers;
using XpertMobileApp.Api.Services;
using XpertMobileApp.Api.ViewModels;
using XpertMobileApp.DAL;
using XpertMobileApp.Models;
using XpertMobileApp.Services;

namespace XpertMobileApp.ViewModels
{

    public class ProduitsViewModel : CrudBaseViewModel2<STK_PRODUITS, View_STK_PRODUITS>
    {


        public ProduitsViewModel()
        {
            Title = AppResources.pn_Produits;

            Types = new ObservableCollection<BSE_TABLE_TYPE>();
            Familles = new ObservableCollection<BSE_TABLE>();
            Labos = new ObservableCollection<BSE_PRODUIT_LABO>();
            Unites = new ObservableCollection<BSE_PRODUIT_UNITE>();
            SelectedTag = new List<BSE_PRODUIT_TAG>();

            OnCanLoadMoreBackup = Items.OnCanLoadMore;

            LoadExtrasDataCommand = new Command(async () => await ExecuteLoadExtrasDataCommand());
        }

        protected override QueryInfos GetFilterParams()
        {
            base.GetFilterParams();

            // this.AddSelect<View_STK_STOCK, View_STK_STOCK>(e=>e.)

            this.AddCondition<View_STK_PRODUITS, string>(e => e.DESIGNATION_PRODUIT, Operator.LIKE_ANY, SearchedText);

            if (!string.IsNullOrEmpty(SearchedRef))
                this.AddCondition<View_STK_PRODUITS, string>(e => e.REFERENCE, SearchedRef);

            if (!string.IsNullOrEmpty(SelectedFamille?.CODE))
                this.AddCondition<View_STK_PRODUITS, string>(e => e.CODE_FAMILLE, SelectedFamille?.CODE);

            if (!string.IsNullOrEmpty(SelectedType?.CODE_TYPE))
                this.AddCondition<View_STK_PRODUITS, string>(e => e.TYPE_PRODUIT, SelectedType?.CODE_TYPE);


            if (!string.IsNullOrEmpty(SelectedLabo?.CODE))
                this.AddCondition<View_STK_PRODUITS, string>(e => e.CODE_LABO, SelectedLabo?.CODE);
            //List<STK_PRODUITS> x = new ProductManager().GetProduitUniteByCode(SelectedUnite?.CODE);
            //if (!string.IsNullOrEmpty(SelectedUnite?.CODE))
            //    this.AddCondition<View_STK_PRODUITS, string>(e => e.UNI, SelectedUnite?.CODE);



            if (CheckBoxSM)
            {
                this.AddCondition("(QTE_STOCK < STOCK_MIN AND STOCK_MIN <> 0)");
            }

            if (CheckBoxS)
            {
                this.AddCondition<View_STK_PRODUITS, bool>(e => e.IS_STOCKABLE, !CheckBoxS);
            }

            if (CheckBoxR)
            {
                this.AddCondition<View_STK_PRODUITS, bool>(e => e.RUPTURE, CheckBoxR);
            }

            if (SelectedTag.Count != 0)
            {
                string tagList = "";
                for (int i = 0; i < SelectedTag.Count; i++)
                {
                    tagList += SelectedTag[i].CODE;
                    if (i != SelectedTag.Count - 1)
                        tagList += ",";
                }
                this.AddJoin(@"INNER JOIN(select CODE_PRODUIT from STK_PRODUIT_TAG where CODE_TAG in(" + tagList + ")  group by CODE_PRODUIT)" +
                       "AS orig on orig.CODE_PRODUIT = View_STK_PRODUITS.CODE_PRODUIT");
            }//this.AddCondition<View_STK_PRODUITS, string>(e => e., SelectedTag?.DESIGNATION);

            if (Constants.AppName == Apps.XCOM_Mob)
            {
                if (FidelOperator == "FYes")
                    this.AddCondition<View_STK_PRODUITS, bool>(e => e.INCLUDE_IN_CARD_FIDELITE, 1);
                else if (FidelOperator == "FNo")
                    this.AddCondition<View_STK_PRODUITS, bool>(e => e.INCLUDE_IN_CARD_FIDELITE, 0);

                if (EtatOperator == "active")
                    this.AddCondition<View_STK_PRODUITS, bool>(e => e.ACTIF, 1);
                else if (EtatOperator == "nonActive")
                    this.AddCondition<View_STK_PRODUITS, bool>(e => e.ACTIF, 0);
            }


            //if (QuantityOperator == ">")
            //    this.AddCondition<View_STK_PRODUITS, decimal>(e => e.QTE_STOCK, Operator.GREATER, 0);
            //else if (QuantityOperator == "<")
            //    this.AddCondition<View_STK_PRODUITS, decimal>(e => e.QTE_STOCK, Operator.LESS, 0);
            //else if (QuantityOperator == "=")
            //    this.AddCondition<View_STK_PRODUITS, decimal>(e => e.QTE_STOCK, Operator.EQUAL, 0);

            /*
             * SELECT *  FROM View_STK_PRODUITS INNER JOIN( 
                   select CODE_PRODUIT from STK_PRODUIT_TAG where CODE_TAG in(1)  group by CODE_PRODUIT)AS orig on orig.CODE_PRODUIT = View_STK_PRODUITS.CODE_PRODUIT

            string tagList = XpertHelper.GetValues(schTag.SelectedValues, ',', false);
                this.ProdBLL.QueryBuilder.AddJoin(@"INNER JOIN(
                   select CODE_PRODUIT from STK_PRODUIT_TAG where CODE_TAG in("+ tagList + ")  group by CODE_PRODUIT)"+
                   "AS orig on orig.CODE_PRODUIT = " + this.ProdBLL.TableView + ".CODE_PRODUIT");
            */

            //if (!string.IsNullOrEmpty(BareCode))
            //    this.AddCondition<View_STK_PRODUITS, string>(e => e.CODE_BARRE, BareCode);

            this.AddOrderBy<View_STK_PRODUITS, string>(e => e.DESIGNATION_PRODUIT);
            return qb.QueryInfos;
        }

        public async Task GetScanedProduct()
        {
            try
            {
                UserDialogs.Instance.ShowLoading(AppResources.txt_Waiting);
                Items.Clear();
                List<View_STK_PRODUITS> products = new List<View_STK_PRODUITS>();
                products = await CrudManager.Products.SelectProduitByCodeBarre(BareCode);
                UserDialogs.Instance.HideLoading();
                if (products.Count > 1)
                {
                    await Application.Current.MainPage.DisplayAlert(AppResources.txt_alert, AppResources.txt_mssg_codeBarre_p_produits, AppResources.alrt_msg_Ok);
                    await ExecuteLoadItemsCommand();
                }
                else if (products.Count == 0)
                {
                    await Application.Current.MainPage.DisplayAlert(AppResources.txt_alert, AppResources.txt_mssg_codeBarre_aucun_produits, AppResources.alrt_msg_Ok);
                    await ExecuteLoadItemsCommand();
                }
                else
                {
                    Items.AddRange(products);
                }
            }
            catch (Exception ex)
            {
                UserDialogs.Instance.HideLoading();
                await UserDialogs.Instance.AlertAsync(WSApi2.GetExceptionMessage(ex), AppResources.alrt_msg_Alert,
                    AppResources.alrt_msg_Ok);
            }


        }

        /// <summary>
        /// remplacer la valeur par défaut ExecuteLoadItemsCommand pour redéfinir la fonction OnCanLoadMore 
        /// sur l'événement d'actualisation de la page ou en appuyant sur le bouton Appliquer sur le filtre
        /// après avoir montré un produit qui est le résultat d'un code-barres scanné
        /// </summary>
        /// <returns></returns>
        internal override Task ExecuteLoadItemsCommand()
        {
            Items.OnCanLoadMore = OnCanLoadMoreBackup;
            return base.ExecuteLoadItemsCommand();
        }

        protected override void OnAfterLoadItems(IEnumerable<View_STK_PRODUITS> list)
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
        private string searchedText;
        public string SearchedText
        {
            get { return searchedText; }
            set { SetProperty(ref searchedText, value); }
        }

        private string searchedRef;
        public string SearchedRef
        {
            get { return searchedRef; }
            set { SetProperty(ref searchedRef, value); }
        }

        private Func<bool> onCanLoadMoreBackup;

        public Func<bool> OnCanLoadMoreBackup
        {
            get { return onCanLoadMoreBackup; }
            set { SetProperty(ref onCanLoadMoreBackup, value); }
        }

        private string bareCode;

        public string BareCode
        {
            get { return bareCode; }
            set { SetProperty(ref bareCode, value); }
        }

        public ObservableCollection<BSE_TABLE_TYPE> Types { get; set; }
        private BSE_TABLE_TYPE selectedType;
        public BSE_TABLE_TYPE SelectedType
        {
            get { return selectedType; }
            set { SetProperty(ref selectedType, value); }
        }

        public ObservableCollection<BSE_TABLE> Familles { get; set; }
        private BSE_TABLE selectedFamille;
        public BSE_TABLE SelectedFamille
        {
            get { return selectedFamille; }
            set { SetProperty(ref selectedFamille, value); }
        }

        public ObservableCollection<BSE_PRODUIT_UNITE> Unites{ get; set; }
        private BSE_PRODUIT_UNITE selectedUnite;
        public BSE_PRODUIT_UNITE SelectedUnite
        {
            get { return selectedUnite; }
            set { SetProperty(ref selectedUnite, value); }
        }

        public ObservableCollection<BSE_PRODUIT_TAG> Tags { get; set; }
        private List<BSE_PRODUIT_TAG> selectedTag;
        public List<BSE_PRODUIT_TAG> SelectedTag
        {
            get { return selectedTag; }
            set { SetProperty(ref selectedTag, value); }
        }

        public ObservableCollection<BSE_PRODUIT_LABO> Labos { get; set; }
        private BSE_PRODUIT_LABO selectedLabo;
        public BSE_PRODUIT_LABO SelectedLabo
        {
            get { return selectedLabo; }
            set { SetProperty(ref selectedLabo, value); }
        }


        private string etatOperator;
        public string EtatOperator
        {
            get { return etatOperator; }
            set { SetProperty(ref etatOperator, value); }
        }

        private bool checkboxSM = false;
        private bool checkboxS = false;
        private bool checkboxR = false;

        public bool CheckBoxSM
        {
            get { return checkboxSM; }
            set { SetProperty(ref checkboxSM, value); }
        }
        public bool CheckBoxS
        {
            get { return checkboxS; }
            set { SetProperty(ref checkboxS, value); }
        }
        public bool CheckBoxR
        {
            get { return checkboxR; }
            set { SetProperty(ref checkboxR, value); }
        }

        private string fidelOperator;
        public string FidelOperator
        {
            get { return fidelOperator; }
            set { SetProperty(ref fidelOperator, value); }
        }


        public override void ClearFilters()
        {
            base.ClearFilters();
            searchedText = "";
            SelectedType = null;
            SelectedFamille = null;
            BareCode = "";
            EtatOperator = "";
            CheckBoxSM = false;
            CheckBoxS = false;
            CheckBoxR = false;
            SelectedTag = new List<BSE_PRODUIT_TAG>();
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
                await ExecuteLoadLabosCommand();
                await ExecuteLoadUniteCommand();
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

        //async Task ExecuteLoadTagsCommand()
        //{
        //        try
        //        {
        //            Tags.Clear();
        //            var itemsC = await WebServiceClient.GetProduitTags();

        //            BSE_PRODUIT_TAG allElem = new BSE_PRODUIT_TAG();
        //            allElem.CODE = "";
        //            allElem.DESIGNATION = "";
        //            Tags.Add(allElem);

        //            foreach (var itemC in itemsC)
        //            {
        //                Tags.Add(itemC);
        //            }
        //        }
        //        catch (Exception ex)
        //        {
        //            await UserDialogs.Instance.AlertAsync(WSApi2.GetExceptionMessage(ex), AppResources.alrt_msg_Alert,
        //                AppResources.alrt_msg_Ok);
        //        }
        //}


        async Task ExecuteLoadLabosCommand()
        {
            try
            {
                Labos.Clear();
                var itemsC = await WebServiceClient.GetProduitLabos();

                BSE_PRODUIT_LABO allElem = new BSE_PRODUIT_LABO();
                allElem.CODE = "";
                allElem.DESIGNATION = "";
                Labos.Add(allElem);

                foreach (var itemC in itemsC)
                {
                    Labos.Add(itemC);
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
            if (App.Online)
            {
                try
                {
                    Types.Clear();
                    var itemsC = await WebServiceClient.GetProduitTypes();

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
        }

        async Task ExecuteLoadFamillesCommand()
        {
            if (App.Online)
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
        }

        async Task ExecuteLoadUniteCommand()
        {
            if (App.Online)
            {
                try
                {
                    Unites.Clear();
                    var itemsC = await WebServiceClient.GetProduitUnite();

                    BSE_PRODUIT_UNITE allElem = new BSE_PRODUIT_UNITE();
                    allElem.CODE = "";
                    allElem.DESIGNATION = "";
                    Unites.Add(allElem);

                    foreach (var itemC in itemsC)
                    {
                        Unites.Add(itemC);
                    }
                }
                catch (Exception ex)
                {
                    await UserDialogs.Instance.AlertAsync(WSApi2.GetExceptionMessage(ex), AppResources.alrt_msg_Alert,
                        AppResources.alrt_msg_Ok);
                }
            }
        }

        #endregion
    }

}
