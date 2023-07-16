using Acr.UserDialogs;
using Newtonsoft.Json;
using Rg.Plugins.Popup.Services;
using Syncfusion.Linq;
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
using XpertMobileApp.Api.Managers;
using XpertMobileApp.Api.Services;
using XpertMobileApp.Api.ViewModels;
using XpertMobileApp.DAL;
using XpertMobileApp.Models;
using XpertMobileApp.Services;
using XpertMobileApp.SQLite_Managment;
using XpertMobileApp.Views;

namespace XpertMobileApp.ViewModels
{
    public class ProductGroup : List<View_STK_PRODUITS>
    {
        public string GroupTitle { get; set; }
        public ProductGroup(string title, List<View_STK_PRODUITS> produits) : base(produits)
        {
            this.GroupTitle = title;
        }
    }

    public class ProduitsViewModel : CrudBaseViewModel2<STK_PRODUITS, View_STK_PRODUITS>
    {

        public bool DisplayWithQuantity { get; set; }

        private bool orderWithFamille = false;
        public bool OrderWithFamille
        {
            get
            {
                return orderWithFamille;
            }
            set
            {
                orderWithFamille = value;
            }
        }
        private bool orderWithType = false;
        public bool OrderWithType
        {
            get
            {
                return orderWithType;
            }
            set
            {
                orderWithType = value;
            }
        }

        private bool orderWithMarque = false;
        public bool OrderWithMarque
        {
            get
            {
                return orderWithMarque;
            }
            set
            {
                orderWithMarque = value;
            }
        }
        string currentQB = null;

        public InfiniteScrollCollection<View_STK_PRODUITS> ItemsWithQteMagasin { get; set; }
        public InfiniteScrollCollection<ProductGroup> ListOfGroupedProducts { get; set; }
        public InfiniteScrollCollection<View_STK_PRODUITS> ListOfallProducts { get; set; }
        public ProduitsViewModel()
        {
            Title = AppResources.pn_Produits;

            Types = new ObservableCollection<BSE_TABLE_TYPE>();
            Familles = new ObservableCollection<BSE_TABLE>();
            Labos = new ObservableCollection<BSE_PRODUIT_LABO>();
            Unites = new ObservableCollection<BSE_PRODUIT_UNITE>();
            SelectedTag = new List<BSE_PRODUIT_TAG>();
            ItemsWithQteMagasin = new InfiniteScrollCollection<View_STK_PRODUITS>();
            ListOfGroupedProducts = new InfiniteScrollCollection<ProductGroup>();
            ListOfallProducts = new InfiniteScrollCollection<View_STK_PRODUITS>();
            OnCanLoadMoreBackup = Items.OnCanLoadMore;

            LoadExtrasDataCommand = new Command(async () => await ExecuteLoadExtrasDataCommand());
        }

        protected override QueryInfos GetFilterParams()
        {
            base.GetFilterParams();
            // this.AddSelect<View_STK_STOCK, View_STK_STOCK>(e=>e.)

            this.AddCondition<View_STK_PRODUITS, string>(e => e.DESIGNATION_PRODUIT, Operator.LIKE_ANY, SearchedText);

            if (!DisplayWithQuantity)
                this.AddCondition<View_STK_PRODUITS, decimal>(e => e.QTE_STOCK, Operator.GREATER, 0);

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


            if (EtatOperator == "active")
                this.AddCondition<View_STK_PRODUITS, bool>(e => e.ACTIF, 1);
            else if (EtatOperator == "nonActive")
                this.AddCondition<View_STK_PRODUITS, bool>(e => e.ACTIF, 0);

            if (Constants.AppName == Apps.XCOM_Mob)
            {
                if (FidelOperator == "FYes")
                    this.AddCondition<View_STK_PRODUITS, bool>(e => e.INCLUDE_IN_CARD_FIDELITE, 1);
                else if (FidelOperator == "FNo")
                    this.AddCondition<View_STK_PRODUITS, bool>(e => e.INCLUDE_IN_CARD_FIDELITE, 0);
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
            if (orderWithFamille)
            {
                this.AddOrderBy<View_STK_PRODUITS, string>(e => e.CODE_FAMILLE, Sort.DESC);
            }
            else if (orderWithMarque)
            {
                this.AddOrderBy<View_STK_PRODUITS, string>(e => e.CODE_LABO, Sort.DESC);
            }
            else if (orderWithType)
            {
                this.AddOrderBy<View_STK_PRODUITS, string>(e => e.TYPE_PRODUIT, Sort.DESC);
            }
            else
            {
                this.AddOrderBy<View_STK_PRODUITS, string>(e => e.DESIGNATION_PRODUIT);
            }

            return qb.QueryInfos;
        }

        protected override string ContoleurName
        {
            get
            {
                return Constants.AppName == Apps.XCOM_Mob ? "STK_PRODUITS_XCOM" : "STK_PRODUITS";
            }
        }

        public async Task GetScanedProduct()
        {
            try
            {

                UserDialogs.Instance.ShowLoading(AppResources.txt_Waiting);
                Items.Clear();
                List<View_STK_PRODUITS> products = new List<View_STK_PRODUITS>();

                if (App.Online)
                {
                    products = await CrudManager.Products.SelectProduitByCodeBarre(BareCode);

                    UserDialogs.Instance.HideLoading();
                    if (products.Count > 1)
                    {
                        await PopupNavigation.Instance.PushAsync(new CodeBarrePopUp(AppResources.txt_alert, AppResources.txt_mssg_codeBarre_p_produits, BareCode));
                        //await Application.Current.MainPage.DisplayAlert(AppResources.txt_alert, AppResources.txt_mssg_codeBarre_p_produits+ System.Environment.NewLine+ BareCode, AppResources.alrt_msg_Ok);
                        await ExecuteLoadItemsCommand();
                    }
                    else if (products.Count == 0)
                    {
                        await PopupNavigation.Instance.PushAsync(new CodeBarrePopUp(AppResources.txt_alert, AppResources.txt_mssg_codeBarre_aucun_produits, BareCode));
                        //await Application.Current.MainPage.DisplayAlert(AppResources.txt_alert, AppResources.txt_mssg_codeBarre_aucun_produits + System.Environment.NewLine + BareCode, AppResources.alrt_msg_Ok);
                        await ExecuteLoadItemsCommand();
                    }
                    else
                    {
                        Items.AddRange(products);
                    }
                }
                else
                {
                    products = await SQLite_Manager.GetProductByBarCode(BareCode);
                    if (products.Count == 1)
                    {
                        Items.AddRange(products);
                        UserDialogs.Instance.HideLoading();
                    }
                    else
                    {
                        UserDialogs.Instance.HideLoading();
                        await PopupNavigation.Instance.PushAsync(new CodeBarrePopUp(AppResources.txt_alert, AppResources.txt_mssg_codeBarre_aucun_produits, BareCode));
                    }
                }
            }
            catch (Exception ex)
            {
                UserDialogs.Instance.HideLoading();
                await UserDialogs.Instance.AlertAsync(WSApi2.GetExceptionMessage(ex), AppResources.alrt_msg_Alert,
                    AppResources.alrt_msg_Ok);
            }
        }

        protected override QueryInfos GetSelectParams()
        {
            base.GetSelectParams();
            this.AddSelect<View_STK_PRODUITS, string>(e => e.DESIGNATION_PRODUIT);
            this.AddSelect<View_STK_PRODUITS, string>(e => e.DESIGN_TYPE);
            this.AddSelect<View_STK_PRODUITS, string>(e => e.DESIGN_TYPE);
            this.AddSelect<View_STK_PRODUITS, decimal>(e => e.QTE_STOCK);
            this.AddSelect<View_STK_PRODUITS, string>(e => e.REFERENCE);
            this.AddSelect<View_STK_PRODUITS, string>(e => e.CODE_PRODUIT);
            this.AddSelect<View_STK_PRODUITS, decimal>(e => e.PRIX_VENTE_HT);
            this.AddSelect<View_STK_PRODUITS, decimal>(e => e.PRIX_ACHAT_HT);
            this.AddSelect<View_STK_PRODUITS, string>(e => e.DESIGN_DCI);
            this.AddSelect<View_STK_PRODUITS, string>(e => e.DESIGN_FAMILLE);
            this.AddSelect<View_STK_PRODUITS, string>(e => e.DESIGN_LABO);
            this.AddSelect<View_STK_PRODUITS, string>(e => e.DESIGN_TYPE);

            return qb.QueryInfos;

        }

        /// <summary>
        /// remplacer la valeur par défaut ExecuteLoadItemsCommand pour redéfinir la fonction OnCanLoadMore 
        /// sur l'événement d'actualisation de la page ou en appuyant sur le bouton Appliquer sur le filtre
        /// après avoir montré un produit qui est le résultat d'un code-barres scanné
        /// </summary>
        /// <returns></returns>
        internal override async Task ExecuteLoadItemsCommand()
        {
            Items.OnCanLoadMore = OnCanLoadMoreBackup;
            if (IsBusy)
                return;

            try
            {
                IsBusy = true;
                if (currentQB != null && currentQB != GetFilterParams().StringCondition && BareCode == null)
                {
                    currentQB = GetFilterParams().StringCondition;
                    Items.Clear();
                    ItemsWithQteMagasin.Clear();
                }
                else
                {
                    if (Items.Count >= ElementsCount && Items.Count != 0)
                        return;
                    currentQB = GetFilterParams().StringCondition;
                }
                if (String.IsNullOrEmpty(BareCode))
                    await Items.LoadMoreAsync();
                IsBusy = false;
            }
            catch (Exception ex)
            {
                await UserDialogs.Instance.AlertAsync(WSApi2.GetExceptionMessage(ex), AppResources.alrt_msg_Alert,
                    AppResources.alrt_msg_Ok);
            }
            finally
            {
                IsBusy = false;
            }

        }

        public Command PullTORefresh
        {
            get
            {
                return new Command(async () =>
                {
                    if (IsBusy)
                    {
                        return;
                    }
                    currentQB = "Empty";
                    await ExecuteLoadItemsCommand();
                    IsBusy = false;
                });
            }
        }
        protected override async void OnAfterLoadItems(IEnumerable<View_STK_PRODUITS> list)
        {
            base.OnAfterLoadItems(list);
            if (!DisplayWithQuantity && Constants.AppName == Apps.X_DISTRIBUTION)
            {
                List<View_STK_PRODUITS> tempList = new List<View_STK_PRODUITS>();
                if (list != null)
                {
                    foreach (View_STK_PRODUITS item in list)
                    {
                        if (item.QTE_STOCK > 0)
                        {
                            tempList.Add(item);
                        }
                    }
                    ItemsWithQteMagasin.AddRange(tempList);
                }
            }
            else
            {
                List<View_STK_PRODUITS> tempList = new List<View_STK_PRODUITS>();
                if (list != null)
                {
                    foreach (View_STK_PRODUITS item in list)
                    {
                        if (item.QTE_STOCK <= 0)
                        {
                            tempList.Add(item);
                        }
                    }
                    ItemsWithQteMagasin.AddRange(tempList);
                }
            }


            /* Partie concerné par l'affichage de la liste groupé */

            if (!String.IsNullOrEmpty(SearchedText))
            {
                ListOfallProducts.Clear();

                ListOfallProducts.AddRange(list);
                if (ListOfallProducts.Count > 0)
                {
                    if (OrderWithFamille)
                    {
                        await GroupByFamille();
                        LoadItemsCommand.Execute(null);
                    }
                    else if (OrderWithMarque)
                    {
                        await GroupByBrand();
                        LoadItemsCommand.Execute(null);
                    }
                    else if (OrderWithType)
                    {
                        await GroupByType();
                        LoadItemsCommand.Execute(null);
                    }

                    ListOfallProducts.Clear();
                }
                else
                {
                    Device.BeginInvokeOnMainThread(() =>
                    {
                        ListOfGroupedProducts.Clear();
                    });
                }
            }
            else if (SearchedText == "")
            {
                ListOfallProducts.Clear();
                Device.BeginInvokeOnMainThread(() =>
                {
                    ListOfGroupedProducts.Clear();
                });
                SearchedText = null;
                if (OrderWithFamille)
                {
                    await GroupByFamille();
                    LoadItemsCommand.Execute(null);
                }
                else if (OrderWithMarque)
                {
                    await GroupByBrand();
                    LoadItemsCommand.Execute(null);
                }
                else if (OrderWithType)
                {
                    await GroupByType();
                    LoadItemsCommand.Execute(null);
                }
            }
            /* Fin du traitement de la partie pour l'affichage de la liste groupé */
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

        private bool selectedEtatTous;
        public bool SelectedEtatTous
        {
            get
            {
                return selectedEtatTous;
            }
            set
            {
                selectedEtatTous = value;
            }
        }

        private bool selectedEtatActif;
        public bool SelectedEtatActif
        {
            get
            {
                return selectedEtatActif;
            }
            set
            {
                selectedEtatActif = value;
            }
        }

        private bool selectedEtatInActive;
        public bool SelectionedEtatInActive
        {
            get
            {
                return selectedEtatInActive;
            }
            set
            {
                selectedEtatInActive = value;
            }
        }

        private bool selectedFidAll;
        public bool SelectedFidAll
        {
            get
            {
                return selectedFidAll;
            }
            set
            {
                selectedFidAll = value;
            }
        }

        private bool selectedFidYes;
        public bool SelectedFidYes
        {
            get
            {
                return selectedFidYes;
            }
            set
            {
                selectedFidYes = value;
            }
        }

        private bool selectedFidNo;
        public bool SelectedFidNo
        {
            get
            {
                return selectedFidNo;
            }
            set
            {
                selectedFidNo = value;
            }
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

        public ObservableCollection<BSE_PRODUIT_UNITE> Unites { get; set; }
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
                if (App.Online)
                {
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
                else
                {
                    var itemsC = await SQLite_Manager.GetLabos();
                    foreach (var item in itemsC)
                    {
                        Labos.Add(item);
                    }
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
            else
            {
                Types.Clear();
                var itemsC = await SQLite_Manager.GetProduitType();
                BSE_TABLE_TYPE allElem = new BSE_TABLE_TYPE();
                allElem.CODE_TYPE = "";
                allElem.DESIGNATION_TYPE = AppResources.txt_All;
                Types.Add(allElem);

                foreach (var item in itemsC)
                {
                    Types.Add(new BSE_TABLE_TYPE()
                    {
                        CODE_TYPE = item.CODE_TYPE,
                        DESIGNATION_TYPE = item.DESIGNATION_TYPE
                    });
                }
            }
        }

        async Task ExecuteLoadFamillesCommand()
        {
            try
            {
                Familles.Clear();
                if (App.Online)
                {

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
                else
                {
                    var itemsC = await SQLite_Manager.GetProduitFamilles();
                    BSE_TABLE allElem = new BSE_TABLE();
                    allElem.CODE = "";
                    allElem.DESIGNATION = AppResources.txt_All;
                    Familles.Add(allElem);

                    foreach (var itemC in itemsC)
                    {
                        Familles.Add(new BSE_TABLE()
                        {
                            CODE = itemC.CODE,
                            DESIGNATION = itemC.DESIGNATION
                        });
                    }
                }

            }
            catch (Exception ex)
            {
                await UserDialogs.Instance.AlertAsync(WSApi2.GetExceptionMessage(ex), AppResources.alrt_msg_Alert,
                    AppResources.alrt_msg_Ok);
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


        public async override Task<List<View_STK_PRODUITS>> SelectByPageFromSqlLite(QueryInfos filter)
        {
            var sqliteRes = await base.SelectByPageFromSqlLite(filter);
            if (!string.IsNullOrEmpty(BareCode))
                sqliteRes = sqliteRes.Where(e => e.CODE_BARRE.Equals(BareCode)).ToList();

            else
            {

                if (!string.IsNullOrEmpty(SelectedFamille?.DESIGNATION))
                    sqliteRes = sqliteRes.Where(e => e.CODE_FAMILLE == selectedFamille.CODE).ToList();

                if (!string.IsNullOrEmpty(SelectedType?.DESIGNATION_TYPE))
                    sqliteRes = sqliteRes.Where(e => e.TYPE_PRODUIT == selectedType.CODE_TYPE).ToList();

                if (!string.IsNullOrEmpty(SearchedRef))
                    sqliteRes = sqliteRes.Where(e => e.REFERENCE.Contains(SearchedRef)).ToList();

                if (!string.IsNullOrEmpty(SearchedText))
                    sqliteRes = sqliteRes.Where(e => e.DESIGNATION.Contains(SearchedText)).ToList();

                if (CheckBoxSM)
                {
                    sqliteRes = sqliteRes.Where(e => e.QTE_STOCK < e.STOCK_MIN && e.STOCK_MIN != 0).ToList();
                }
                if (!DisplayWithQuantity)
                    sqliteRes = sqliteRes.Where(e => e.QTE_STOCK > 0).ToList();

                if (CheckBoxS)
                {
                    sqliteRes = sqliteRes.Where(e => e.IS_STOCKABLE == !CheckBoxS).ToList();
                }

                if (CheckBoxR)
                {
                    sqliteRes = sqliteRes.Where(e => e.RUPTURE == CheckBoxR).ToList();
                }

                if (EtatOperator == "active")
                    sqliteRes = sqliteRes.Where(e => e.ACTIF == true).ToList();

                else if (EtatOperator == "nonActive")
                    sqliteRes = sqliteRes.Where(e => e.ACTIF == false).ToList();

                if (orderWithFamille)
                {
                    sqliteRes = sqliteRes.OrderByDescending(e => e.CODE_FAMILLE).ToList();
                }
                else if (orderWithMarque)
                {
                    sqliteRes = sqliteRes.OrderByDescending(e => e.CODE_LABO).ToList();
                }
                else if (orderWithType)
                {
                    sqliteRes = sqliteRes.OrderByDescending(e => e.TYPE_PRODUIT).ToList();
                }
                else
                {
                    sqliteRes = sqliteRes.OrderByDescending(e => e.DESIGNATION_PRODUIT).ToList();
                }
            }
            return sqliteRes;
        }
        #endregion


        #region Grouped List functions 

        public async Task GroupByType()
        {
            try
            {
                UserDialogs.Instance.ShowLoading(AppResources.txt_Waiting);
                if (!String.IsNullOrEmpty(SearchedText))
                {
                    Device.BeginInvokeOnMainThread(() =>
                    {
                        ListOfGroupedProducts.Clear();
                    });
                }
                if (ListOfallProducts.Count() <= 0)
                {
                    await ExecuteLoadAllProductCommand();
                }
                if (ListOfallProducts != null)
                {
                    // Create an empty list so we can add products with no Type.
                    List<View_STK_PRODUITS> listProductWithoutType = new List<View_STK_PRODUITS>();

                    // Create the liste of all types 
                    if (Types != null && Types.Count() > 0)
                    {
                        foreach (var type in Types)
                        {
                            List<View_STK_PRODUITS> tempList = new List<View_STK_PRODUITS>();
                            foreach (var product in ListOfallProducts)
                            {
                                if (DisplayWithQuantity)
                                {
                                    if (product.DESIGN_TYPE.ToLower() == type.DESIGNATION_TYPE.ToLower())
                                    {
                                        tempList.Add(product);
                                    }
                                }
                                else if (product.DESIGN_TYPE.ToLower() == type.DESIGNATION_TYPE.ToLower() && product.QTE_STOCK > 0)
                                {
                                    tempList.Add(product);
                                }
                            }
                            if ((type.DESIGNATION_TYPE.ToLower() != "tous" && type.DESIGNATION_TYPE.ToLower() != "") && tempList.Count > 0)
                            {
                                Device.BeginInvokeOnMainThread(() =>
                                {
                                    ListOfGroupedProducts.Add(new ProductGroup(type.DESIGNATION_TYPE.ToString().ToUpper(), tempList));
                                });
                            }
                        }
                        foreach (var product in ListOfallProducts)
                        {
                            if (DisplayWithQuantity)
                            {
                                if (String.IsNullOrEmpty(product.DESIGN_TYPE) && product.QTE_STOCK > 0)
                                {
                                    listProductWithoutType.Add(product);
                                }
                            }
                            else if (String.IsNullOrEmpty(product.DESIGN_TYPE))
                            {
                                listProductWithoutType.Add(product);
                            }
                        }

                        if (listProductWithoutType.Count > 0)
                        {
                            Device.BeginInvokeOnMainThread(() =>
                            {
                                ListOfGroupedProducts.Add(new ProductGroup("Autre Type".ToString().ToUpper(), listProductWithoutType));
                            });
                        }
                    }
                }
                UserDialogs.Instance.HideLoading();
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        public async Task GroupByFamille()
        {
            try
            {

                if (!String.IsNullOrEmpty(SearchedText))
                {
                    Device.BeginInvokeOnMainThread(async () =>
                    {
                        ListOfGroupedProducts.Clear();
                    });
                }
                if (ListOfallProducts.Count() <= 0)
                {
                    await ExecuteLoadAllProductCommand();
                }
                if (ListOfallProducts != null)
                {
                    // Create an empty list so we can add products with no familly.
                    List<View_STK_PRODUITS> listProductWithoutFamille = new List<View_STK_PRODUITS>();

                    // Create the liste of all types 
                    if (Familles != null && Familles.Count() > 0)
                    {
                        foreach (var famille in Familles)
                        {
                            List<View_STK_PRODUITS> tempList = new List<View_STK_PRODUITS>();
                            foreach (var product in ListOfallProducts)
                            {
                                if (DisplayWithQuantity)
                                {
                                    if (product.DESIGN_FAMILLE.ToLower() == famille.DESIGNATION.ToLower())
                                    {
                                        tempList.Add(product);
                                    }
                                }
                                else if (product.DESIGN_FAMILLE.ToLower() == famille.DESIGNATION.ToLower() && product.QTE_STOCK > 0)
                                {
                                    tempList.Add(product);
                                }
                            }
                            if ((famille.DESIGNATION.ToLower() != "tous" && famille.DESIGNATION.ToLower() != "") && tempList.Count > 0)
                            {
                                Device.BeginInvokeOnMainThread(() =>
                                {
                                    ListOfGroupedProducts.Add(new ProductGroup(famille.DESIGNATION.ToString().ToUpper(), tempList));
                                });
                            }
                        }
                        foreach (var product in ListOfallProducts)
                        {
                            if (DisplayWithQuantity)
                            {
                                if (String.IsNullOrEmpty(product.DESIGN_FAMILLE) && product.QTE_STOCK > 0)
                                {
                                    listProductWithoutFamille.Add(product);
                                }
                            }
                            else if (String.IsNullOrEmpty(product.DESIGN_FAMILLE))
                            {
                                listProductWithoutFamille.Add(product);
                            }
                        }

                        if (listProductWithoutFamille.Count > 0)
                        {
                            Device.BeginInvokeOnMainThread(() =>
                            {
                                ListOfGroupedProducts.Add(new ProductGroup("Autre famille".ToString().ToUpper(), listProductWithoutFamille));
                            });
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        public async Task GroupByBrand()
        {
            try
            {
                if (!String.IsNullOrEmpty(SearchedText))
                {
                    Device.BeginInvokeOnMainThread(() =>
                    {
                        ListOfGroupedProducts.Clear();
                    });
                }
                if (ListOfallProducts.Count() <= 0)
                {
                    await ExecuteLoadAllProductCommand();
                }
                if (ListOfallProducts != null)
                {

                    // Create an empty list so we can add products with no Brand.
                    List<View_STK_PRODUITS> listProductWithoutBrand = new List<View_STK_PRODUITS>();

                    // Create the liste of all types 
                    if (Labos != null && Labos.Count() > 0)
                    {
                        foreach (var labo in Labos)
                        {
                            List<View_STK_PRODUITS> tempList = new List<View_STK_PRODUITS>();
                            foreach (var product in ListOfallProducts)
                            {
                                if (DisplayWithQuantity)
                                {
                                    if (product.DESIGN_LABO.ToLower() == labo.DESIGNATION.ToLower())
                                    {
                                        tempList.Add(product);
                                    }
                                }
                                else if (product.DESIGN_LABO.ToLower() == labo.DESIGNATION.ToLower() && product.QTE_STOCK > 0)
                                {
                                    tempList.Add(product);
                                }
                            }
                            if ((labo.DESIGNATION.ToLower() != "tous" && labo.DESIGNATION.ToLower() != "") && tempList.Count > 0)
                            {
                                Device.BeginInvokeOnMainThread(() =>
                                {
                                    ListOfGroupedProducts.Add(new ProductGroup(labo.DESIGNATION.ToString().ToUpper(), tempList));
                                });
                            }
                        }
                        foreach (var product in ListOfallProducts)
                        {
                            if (DisplayWithQuantity)
                            {
                                if (String.IsNullOrEmpty(product.DESIGN_LABO) && product.QTE_STOCK > 0)
                                {
                                    listProductWithoutBrand.Add(product);
                                }
                            }
                            else if (String.IsNullOrEmpty(product.DESIGN_LABO))
                            {
                                listProductWithoutBrand.Add(product);
                            }
                        }

                        if (listProductWithoutBrand.Count > 0)
                        {
                            Device.BeginInvokeOnMainThread(() =>
                            {
                                ListOfGroupedProducts.Add(new ProductGroup("Autre Marque".ToString().ToUpper(), listProductWithoutBrand));
                            });
                        }
                    }

                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        async Task<InfiniteScrollCollection<View_STK_PRODUITS>> ExecuteLoadAllProductCommand()
        {
            try
            {
                if (App.Online)
                {
                    ListOfallProducts.Clear();
                    var itemsC = await WebServiceClient.GetAllProduct();

                    foreach (var itemC in itemsC)
                    {
                        ListOfallProducts.Add(itemC);
                    }
                    return ListOfallProducts;
                }
                else
                {
                    ListOfallProducts.Clear();
                    var liste = await SQLite_Manager.GetInstance().Table<View_STK_PRODUITS>().ToListAsync();
                    foreach (var product in liste)
                    {
                        ListOfallProducts.Add(product);
                    }


                    return ListOfallProducts;
                }
            }
            catch (Exception e)
            {

                throw e;
            }
        }

        #endregion
    }

}
