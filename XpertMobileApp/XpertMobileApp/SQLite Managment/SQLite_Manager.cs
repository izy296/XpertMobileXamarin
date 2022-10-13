using Acr.UserDialogs;
using SQLite;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using Xamarin.Essentials;
using Xpert.Common.WSClient.Helpers;
using Xpert.Common.WSClient.Model;
using Xpert.Common.WSClient.Services;
using XpertMobileApp.Api;
using XpertMobileApp.Api.Managers;
using XpertMobileApp.Api.Services;
using XpertMobileApp.DAL;
using XpertMobileApp.Models;
using XpertMobileApp.Services;
using XpertMobileApp.ViewModels;

namespace XpertMobileApp.SQLite_Managment
{
    class SQLite_Manager
    {
        static SQLiteAsyncConnection db;
        static string codeVendeur;
        static string CodeTournee;
        static string paramCommande;
        static string CommandeMethode;
        static string paramLivTournee;
        static string paramLivTourneeDetail;
        static string TourneeMethodName;
        static string TourneeDetailMethodName;
        static string UsersMethodName;
        static string PermissionMethodName;
        static string paramPermission;
        static string idGroup;
        static string SessionMethodName;
        static string paramStock;
        static string StockMethodName;
        static string CodeMagasin;

        public class TableName
        {
            public TableName() { }
            public string name { get; set; }
        }
        #region Methode db standard

        /// <summary>
        /// Retourne la db crée ...
        /// </summary>
        /// <returns></returns>
        public static SQLiteAsyncConnection GetInstance()
        {
            if (db == null)
            {
                // Get an absolute path to the database file
                string clientId = App.Settings.ClientId.ToString();
                string AppName = Constants.AppName.ToString();
                var databasePath = Path.Combine(FileSystem.AppDataDirectory, $"{clientId}{AppName}Data.db");
                db = new SQLiteAsyncConnection(databasePath);
                return db;
            }
            else
            {
                return db;
            }
        }

        /// <summary>
        /// Création des différents table dans la db local ...
        /// </summary>
        /// <returns></returns>
        public static async Task InitialisationDbLocal()
        {
            try
            {
                await GetInstance().CreateTableAsync<View_STK_PRODUITS>();
                await GetInstance().CreateTableAsync<View_TRS_TIERS>();
                await GetInstance().CreateTableAsync<View_LIV_TOURNEE>();
                await GetInstance().CreateTableAsync<View_LIV_TOURNEE_DETAIL>();
                await GetInstance().CreateTableAsync<View_STK_STOCK>();
                await GetInstance().CreateTableAsync<View_VTE_VENTE_LOT>();
                await GetInstance().CreateTableAsync<View_VTE_VENTE>();
                await GetInstance().CreateTableAsync<SYS_USER>();
                await GetInstance().CreateTableAsync<SYS_OBJET_PERMISSION>();
                await GetInstance().CreateTableAsync<TRS_JOURNEES>();
                await GetInstance().CreateTableAsync<Token>();
                await GetInstance().CreateTableAsync<View_BSE_TIERS_FAMILLE>();
                await GetInstance().CreateTableAsync<BSE_TABLE_TYPE>();
                await GetInstance().CreateTableAsync<BSE_TABLE>();
                await GetInstance().CreateTableAsync<SYS_CONFIGURATION_MACHINE>();
                await GetInstance().CreateTableAsync<SYS_MOBILE_PARAMETRE>();
                await GetInstance().CreateTableAsync<View_TRS_ENCAISS>();
                await GetInstance().CreateTableAsync<View_BSE_COMPTE>();
                await GetInstance().CreateTableAsync<BSE_ENCAISS_MOTIFS>();
                await GetInstance().CreateTableAsync<View_VTE_COMMANDE>();
                await GetInstance().CreateTableAsync<View_BSE_PRODUIT_PRIX_VENTE_BY_QUANTITY>();
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
            }
        }
        //Vérifie si les toutes les tables sont vide ou non ...
        public static async Task<bool> CheckAllTablesIfEmpty()
        {
            try
            {
                List<int> countListe = new List<int>();
                var obj = await GetInstance().Table<View_STK_PRODUITS>().ToListAsync();
                countListe.Add(obj.Count);

                var obj1 = await GetInstance().Table<View_TRS_TIERS>().ToListAsync();
                countListe.Add(obj1.Count);
                var obj2 = await GetInstance().Table<View_LIV_TOURNEE>().ToListAsync();
                countListe.Add(obj2.Count);
                var obj3 = await GetInstance().Table<View_LIV_TOURNEE_DETAIL>().ToListAsync();
                countListe.Add(obj3.Count);
                var obj4 = await GetInstance().Table<View_STK_STOCK>().ToListAsync();
                countListe.Add(obj4.Count);
                var obj5 = await GetInstance().Table<View_VTE_VENTE_LOT>().ToListAsync();
                countListe.Add(obj5.Count);
                var obj6 = await GetInstance().Table<View_VTE_VENTE>().ToListAsync();
                countListe.Add(obj6.Count);
                var obj7 = await GetInstance().Table<SYS_USER>().ToListAsync();
                countListe.Add(obj7.Count);
                var obj8 = await GetInstance().Table<SYS_OBJET_PERMISSION>().ToListAsync();
                countListe.Add(obj8.Count);
                var obj9 = await GetInstance().Table<TRS_JOURNEES>().ToListAsync();
                countListe.Add(obj9.Count);
                var obj10 = await GetInstance().Table<Token>().ToListAsync();
                countListe.Add(obj10.Count);
                var obj11 = await GetInstance().Table<View_BSE_TIERS_FAMILLE>().ToListAsync();
                countListe.Add(obj11.Count);
                var obj12 = await GetInstance().Table<BSE_TABLE_TYPE>().ToListAsync();
                countListe.Add(obj12.Count);
                var obj13 = await GetInstance().Table<BSE_TABLE>().ToListAsync();
                countListe.Add(obj13.Count);

                var obj14 = await GetInstance().Table<SYS_CONFIGURATION_MACHINE>().ToListAsync();
                countListe.Add(obj14.Count);
                var obj15 = await GetInstance().Table<SYS_MOBILE_PARAMETRE>().ToListAsync();
                countListe.Add(obj15.Count);
                var obj16 = await GetInstance().Table<View_TRS_ENCAISS>().ToListAsync();
                countListe.Add(obj16.Count);
                var obj17 = await GetInstance().Table<View_BSE_COMPTE>().ToListAsync();
                countListe.Add(obj17.Count);
                var obj18 = await GetInstance().Table<BSE_ENCAISS_MOTIFS>().ToListAsync();
                countListe.Add(obj18.Count);
                var obj19 = await GetInstance().Table<View_VTE_COMMANDE>().ToListAsync();
                countListe.Add(obj19.Count);
                var obj20 = await GetInstance().Table<View_BSE_PRODUIT_PRIX_VENTE_BY_QUANTITY>().ToListAsync();
                countListe.Add(obj20.Count);

                foreach (var count in countListe)
                {
                    if (count > 0)
                    {
                        // false means that the liste is not empty
                        return false;
                    }

                }
                return true;

            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                return false;
            }
        }

        /// <summary>
        /// retoune items qui ne sont pas synchronisés depuis sqlServer
        /// </summary>
        /// <typeparam name="TView"></typeparam>
        /// <typeparam name="TTabel"></typeparam>
        /// <param name="_selectAll"></param>
        /// <param name="param"></param>
        /// <param name="methodName"></param>
        /// <returns></returns>
        public static async Task<IEnumerable<TView>> getItemsUnsyncronised<TView, TTabel>(bool _selectAll = true, string param = "", string methodName = "")
        {
            ICurdService<TView> service = new CrudService<TView>(App.RestServiceUrl, typeof(TTabel).Name, App.User.Token);
            if (_selectAll)
            {
                return await service.GetItemsAsync();
            }
            else
            {
                return await service.GetItemsAsyncWithUrl(methodName, param);
            }

        }

        /// <summary>
        /// Supprime tous les elements d'une Tview et les ajouter a une liste aprés les insérés dans la table spécifique...
        /// </summary>
        /// <typeparam name="TView"></typeparam>
        /// <typeparam name="TTabel"></typeparam>
        /// <param name="_selectAll"></param>
        /// <param name="param"></param>
        /// <param name="methodName"></param>
        /// <returns></returns>
        public static async Task SyncData<TView, TTabel>(bool _selectAll = true, string param = "", string methodName = "")
        {
            var ListItems = new List<TView>();

            var items = await getItemsUnsyncronised<TView, TTabel>(_selectAll, param, methodName);
            await GetInstance().DeleteAllAsync<TView>();

            try
            {
                if (items != null)//&& items.IsCompleted && items.Status == TaskStatus.RanToCompletion)
                {
                    ListItems.AddRange(items);
                }
            }
            catch (Exception exc)
            {
                Console.WriteLine(exc.Message);
            }
            var id = await GetInstance().InsertAllAsync(ListItems);

            ListItems = null;
        }

        /// <summary>
        /// Synchronise toutes les tables sqlite mobile a partir de la bd sqlserver
        /// </summary>
        /// <returns></returns>
        public static async Task SynchroniseDownload()
        {
            bool isconnected = await App.IsConected();
            if (isconnected)
            {
                await InitialisationDbLocal();

                UserDialogs.Instance.ShowLoading(AppResources.txt_Waiting);
                await SyncData<View_STK_PRODUITS, STK_PRODUITS>(); // worked !
                await SyncData<View_TRS_TIERS, TRS_TIERS>(); //worked !
                await SyncCommande(); //worked
                await SyncLivTournee();//worked !
                await SyncLivTourneeDetail(); //worked !
                await SyncStock();//worked !
                await syncPermission();//worked ! 
                await SyncSysParams(); //worked !
                await syncSession(); //worked !
                await SyncFamille();//worked !
                await SyncTypeTiers();//worked !
                await SyncSecteurs();//worked !
                await SyncBseCompte();//worked !
                await SyncMotifs();//worked !
                await SyncUsers(); //worked !
                await SyncConfigMachine(); //worked
                await SyncMagasin();                                                                                           //await SyncProduct();
                //await SyncData<View_STK_STOCK, STK_STOCK>();
                //await SyncData<View_VTE_VENTE, VTE_VENTE>();
                //await SyncProductPriceByQuantity();       this probelem here 
                //await GetInstance().DeleteAllAsync<View_VTE_VENTE>();
                //await GetInstance().DeleteAllAsync<View_VTE_VENTE_LOT>();
                UserDialogs.Instance.HideLoading();
                await UserDialogs.Instance.AlertAsync("Synchronisation faite avec succes", AppResources.alrt_msg_Alert, AppResources.alrt_msg_Ok);
            }
            else
            {
                UserDialogs.Instance.HideLoading();
                await UserDialogs.Instance.AlertAsync("Veuillez verifier votre connexion au serveur ! ", AppResources.alrt_msg_Alert, AppResources.alrt_msg_Ok);
            }
        }

        /// <summary>
        /// uploader les donnés aux base de donné distante ...
        /// </summary>
        /// <returns></returns>
        public static async Task synchroniseUpload()
        {
            bool isconnected = await App.IsConected();
            if (isconnected)
            {
                UserDialogs.Instance.ShowLoading(AppResources.txt_Waiting);
                await SyncTiersToServer();
                await SyncEncaissToServer();
                UserDialogs.Instance.HideLoading();
                //await SyncVenteToServer(); // error while coppying content to a stream
                await UserDialogs.Instance.AlertAsync("Synchronisation faite avec succes", AppResources.alrt_msg_Alert, AppResources.alrt_msg_Ok);
            }
            else
            {
                UserDialogs.Instance.HideLoading();
                await UserDialogs.Instance.AlertAsync("Veuillez verifier votre connexion au serveur ! ", AppResources.alrt_msg_Alert, AppResources.alrt_msg_Ok);
            }
        }

        /// <summary>
        /// Synchronisation du magasin 
        /// Obtenir le code magasin de la table Sys_config_machine
        /// assigné le code magasin a l'objet App
        /// </summary>
        /// <returns></returns>
        public static async Task SyncMagasin()
        {
            try
            {
                var listeConfigMachine = await GetInstance().Table<SYS_CONFIGURATION_MACHINE>().ToListAsync();
                if (listeConfigMachine.Count > 0)
                {
                    var codeMagasin = listeConfigMachine.Where(x => x.ID_USER == App.User.UserName).FirstOrDefault()?.CODE_MAGASIN;
                    if (codeMagasin != null)
                    {
                        App.CODE_MAGASIN = codeMagasin;
                    }
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        //public static async Task<bool> CheckDbIsEmpty()
        //{
        //    try
        //    {


        //    catch (Exception ex)
        //    {

        //        throw ex;
        //    }
        //}
        #endregion

        #region Tresorerie

        /// <summary>
        /// Obtenir le tiers par le code_tiers...
        /// </summary>
        /// <param name="text"></param>
        /// <returns></returns>
        internal static async Task<View_TRS_TIERS> SelectScanedTiers(string text)
        {
            var tiers = await GetInstance().Table<View_TRS_TIERS>().ToListAsync();
            return tiers?.Find(e => e.CODE_TIERS.Equals(text));
        }

        public static async Task syncSession()
        {
            var session = await CrudManager.Sessions.GetCurrentSession();
            await GetInstance().DeleteAllAsync<TRS_JOURNEES>();
            var id = await GetInstance().InsertAsync(session);
            //SessionMethodName = "GetCurrentSession";
            //await SyncData<TRS_JOURNEES, TRS_JOURNEES>(false, "", SessionMethodName);
        }

        public static async Task<TRS_JOURNEES> getCurrenetSession()
        {
            TRS_JOURNEES session = await GetInstance().Table<TRS_JOURNEES>().FirstOrDefaultAsync();
            return session;
        }

        public static async Task SyncFamille()
        {
            try
            {
                var itemsC = await WebServiceClient.getTiersFamilles();
                await GetInstance().DeleteAllAsync<View_BSE_TIERS_FAMILLE>();

                View_BSE_TIERS_FAMILLE allElem = new View_BSE_TIERS_FAMILLE();
                allElem.CODE_FAMILLE = "";
                allElem.DESIGN_FAMILLE = "";
                var id = await GetInstance().InsertAsync(allElem);

                var id2 = await GetInstance().InsertAllAsync(itemsC);
            }
            catch (Exception ex)
            {
                await UserDialogs.Instance.AlertAsync(WSApi2.GetExceptionMessage(ex), AppResources.alrt_msg_Alert,
                    AppResources.alrt_msg_Ok);
            }
        }

        public static async Task SyncTypeTiers()
        {
            try
            {
                var itemsC = await WebServiceClient.getTiersTypes();
                await GetInstance().DeleteAllAsync<BSE_TABLE_TYPE>();

                BSE_TABLE_TYPE allElem = new BSE_TABLE_TYPE();
                allElem.CODE_TYPE = "";
                allElem.DESIGNATION_TYPE = "";
                var id = await GetInstance().InsertAsync(allElem);

                var id2 = await GetInstance().InsertAllAsync(itemsC);
            }
            catch (Exception ex)
            {
                await UserDialogs.Instance.AlertAsync(WSApi2.GetExceptionMessage(ex), AppResources.alrt_msg_Alert,
                    AppResources.alrt_msg_Ok);
            }
        }
        #endregion

        #region Tournée

        /// <summary>
        /// Mise à jour du l'etat de couleur et l'etat de l'itemTourné ...
        /// </summary>
        /// <param name="selectedItem"></param>
        /// <returns></returns>
        internal static async Task<int> UpdateTourneeItemVisited(View_LIV_TOURNEE_DETAIL selectedItem)
        {
            selectedItem.CODE_ETAT = TourneeStatus.Visited;
            selectedItem.ETAT_COLOR = "#FFA500";
            return await GetInstance().UpdateAsync(selectedItem);
        }

        /// <summary>
        /// Synchroniser les tounrnés de livraison par le code vendeur
        /// </summary>
        /// <returns></returns>
        public static async Task SyncLivTournee()
        {
            if (App.User.UserName != null)
            {
                codeVendeur = App.User.UserName;
                paramLivTournee = "CodeVendeur=" + codeVendeur;
                TourneeMethodName = "GetTourneeParVendeur";
                await SyncData<View_LIV_TOURNEE, LIV_TOURNEE>(false, paramLivTournee, TourneeMethodName);
                var obj = await GetInstance().Table<View_LIV_TOURNEE>().ToListAsync();
                if (obj != null && obj.Count > 0)
                {
                    App.CODE_MAGASIN = obj[0].CODE_MAGASIN;
                }
            }
        }

        /// <summary>
        /// Synchroniser les détail des tournées ...
        /// </summary>
        /// <returns></returns>
        public static async Task SyncLivTourneeDetail()
        {
            //View_LIV_TOURNEE obj = await GetInstance().Table<View_LIV_TOURNEE>().OrderByDescending(x=>x.DATE_TOURNEE == DateTime.Now).FirstOrDefaultAsync();
            var obj = await GetInstance().Table<View_LIV_TOURNEE>().ToListAsync();
            if (obj != null && obj.Count > 0)
            {
                CodeTournee = obj[0].CODE_TOURNEE;
                paramLivTourneeDetail = "CodeTournee=" + CodeTournee;
                TourneeDetailMethodName = "GetDetailTournee";
                await SyncData<View_LIV_TOURNEE_DETAIL, LIV_TOURNEE_DETAIL>(false, paramLivTourneeDetail, TourneeDetailMethodName);
            }
        }

        #endregion

        #region Stock 

        /// <summary>
        /// Synchroniser le stock par magasin ...
        /// </summary>
        /// <returns></returns>
        public static async Task SyncStock()
        {
            var obj = await GetInstance().Table<View_LIV_TOURNEE>().ToListAsync();
            if (obj != null && obj.Count > 0)
            {
                CodeMagasin = obj[0].CODE_MAGASIN;
                paramStock = "CodeMagasin=" + CodeMagasin;
                StockMethodName = "GetStockByMagsin";
                await SyncData<View_STK_STOCK, STK_STOCK>(false, paramStock, StockMethodName);
            }
        }

        /// <summary>
        /// Synchronisation des produits par magazin 
        /// </summary>
        /// <returns></returns>
        public static async Task SyncProduct()
        {
            if (string.IsNullOrEmpty(App.CODE_MAGASIN))
            {
                await SyncData<View_STK_PRODUITS, STK_PRODUITS>();
            }
            else
            {
                var products = await CrudManager.Products.GetProduitFromMagasin(App.CODE_MAGASIN);
                await GetInstance().DeleteAllAsync<View_STK_PRODUITS>();
                var id = await GetInstance().InsertAllAsync(products);
            }
        }

        #endregion

        #region Systeme

        /// <summary>
        /// Synchronisation des users ...
        /// </summary>
        /// <returns></returns>
        public static async Task SyncUsers()
        {
            UsersMethodName = "SyncUsers";
            await SyncData<SYS_USER, SYS_USER>(false, "", UsersMethodName);
        }

        /// <summary>
        /// Synchronisation des permssions 
        /// </summary>
        /// <returns></returns>
        public static async Task syncPermission()
        {
            idGroup = App.User.UserGroup;
            paramPermission = "idGroup=" + idGroup;
            PermissionMethodName = "GetPermissions";
            await SyncData<SYS_OBJET_PERMISSION, SYS_OBJET_PERMISSION>(false, paramPermission, PermissionMethodName);
        }

        /// <summary>
        /// Synchronisation de la table sys_config_machine
        /// Assigné ma machine a App.Settings.MachineName
        /// </summary>
        /// <returns></returns>
        public static async Task SyncConfigMachine()
        {
            try
            {

                var bll = new SYS_MACHINE_CONFIG_Manager();
                var items = await bll.GetItemsAsync();

                var finalItem = items.Where(x => x.ID_USER == App.User.UserName).FirstOrDefault();
                if (finalItem != null)
                {
                    App.Settings.MachineName = finalItem.MACHINE;
                    await GetInstance().DeleteAllAsync<SYS_CONFIGURATION_MACHINE>();
                    var id = await GetInstance().InsertAsync(finalItem);
                }
            }
            catch (Exception ex)
            {
                await UserDialogs.Instance.AlertAsync(WSApi2.GetExceptionMessage(ex), AppResources.alrt_msg_Alert, AppResources.alrt_msg_Ok);
            }
        }

        #endregion

        #region Vente

        public static async Task SyncCommande()
        {
            try
            {
                CommandeMethode = "SyncCommande";
                await SyncData<View_VTE_COMMANDE, VTE_COMMANDE>(false, "", CommandeMethode);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public static async Task AjoutCommande(View_VTE_COMMANDE item)
        {
            try
            {
                //var listeTiers = await GetInstance().Table<View_TRS_TIERS>().ToListAsync();
                //var obj4 = await GetInstance().Table<View_VTE_COMMANDE>().ToListAsync(); // I added this line for testing purposes !!
                //item.NOM_TIERS = listeTiers.Where(x => x.CODE_TIERS == item.CODE_TIERS).FirstOrDefault()?.NOM_TIERS1;
                //item.CREATED_BY = App.User.UserName;
                //item.CODE_VENTE = await generateCode(item.TYPE_DOC, item.ID.ToString());

                //await GetInstance().InsertAsync(item);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        #endregion



        public static async Task SyncSecteurs()
        {
            try
            {
                var itemsC = await CrudManager.BSE_LIEUX.GetItemsAsync();
                await GetInstance().DeleteAllAsync<BSE_TABLE>();

                BSE_TABLE allElem = new BSE_TABLE();
                allElem.CODE = "";
                allElem.DESIGNATION = "";
                var id = await GetInstance().InsertAsync(allElem);

                var id2 = await GetInstance().InsertAllAsync(itemsC);
            }
            catch (Exception ex)
            {
                await UserDialogs.Instance.AlertAsync(WSApi2.GetExceptionMessage(ex), AppResources.alrt_msg_Alert,
                    AppResources.alrt_msg_Ok);
            }
        }

        public static async Task SyncBseCompte()
        {
            try
            {
                var itemsC = await WebServiceClient.getComptes();
                itemsC.Insert(0, new View_BSE_COMPTE()
                {
                    DESIGNATION_TYPE = AppResources.txt_All,
                    DESIGN_COMPTE = AppResources.txt_All,
                    CODE_TYPE = ""
                });
                await GetInstance().DeleteAllAsync<View_BSE_COMPTE>();
                var id = await GetInstance().InsertAllAsync(itemsC);
            }
            catch (Exception ex)
            {
                await UserDialogs.Instance.AlertAsync(WSApi2.GetExceptionMessage(ex), AppResources.alrt_msg_Alert,
                    AppResources.alrt_msg_Ok);
            }
        }

        /// <summary>
        /// Retourne  la liste des compte ...
        /// </summary>
        /// <returns></returns>
        public static async Task<List<View_BSE_COMPTE>> getComptes()
        {
            List<View_BSE_COMPTE> comptes = await GetInstance().Table<View_BSE_COMPTE>().ToListAsync();
            List<SYS_USER> users = await GetInstance().Table<SYS_USER>().ToListAsync();
            var code_compte = users.Where(x => x.ID_USER == App.User.UserName.ToUpper()).FirstOrDefault()?.CODE_COMPTE;
            if (string.IsNullOrEmpty(code_compte))
            {
                return comptes;
            }
            else
            {
                List<View_BSE_COMPTE> cpt = comptes.Where(x => x.CODE_COMPTE == code_compte).ToList();
                return cpt;
            }
        }

        public static async Task SyncMotifs()
        {
            try
            {
                var itemsM = await WebServiceClient.GetAllMotifs();
                itemsM.Insert(0, new BSE_ENCAISS_MOTIFS()
                {
                    DESIGN_MOTIF = AppResources.txt_All,
                    CODE_MOTIF = ""
                });
                await GetInstance().DeleteAllAsync<BSE_ENCAISS_MOTIFS>();
                var id = await GetInstance().InsertAllAsync(itemsM);
            }
            catch (Exception ex)
            {
                await UserDialogs.Instance.AlertAsync(WSApi2.GetExceptionMessage(ex), AppResources.alrt_msg_Alert,
                    AppResources.alrt_msg_Ok);
            }
        }

        public static async Task SyncProductPriceByQuantity()
        {
            try
            {
                var bll = new ProductManager();
                var items = await bll.SelectListePrix();
                await GetInstance().DeleteAllAsync<View_BSE_PRODUIT_PRIX_VENTE_BY_QUANTITY>();
                var id = await GetInstance().InsertAllAsync(items);
            }
            catch (Exception ex)
            {
                await UserDialogs.Instance.AlertAsync(WSApi2.GetExceptionMessage(ex), AppResources.alrt_msg_Alert,
                    AppResources.alrt_msg_Ok);
            }
        }

        /// <summary>
        /// methode utilisé seulement dans mode offline retourne pris gros ou pris details
        /// </summary>
        /// <param name="codeProduit"></param>
        /// <param name="qteVendu"></param>
        /// <returns></returns>
        public static async Task<decimal> getPrixByQuantity(string codeProduit, decimal qteVendu)
        {
            List<View_BSE_PRODUIT_PRIX_VENTE_BY_QUANTITY> AllPrices = await GetInstance().Table<View_BSE_PRODUIT_PRIX_VENTE_BY_QUANTITY>().ToListAsync();
            var prixProduct = AllPrices.Where(x => x.CODE_PRODUIT == codeProduit).FirstOrDefault();
            if (Math.Abs(qteVendu) >= prixProduct.QTE_VENTE)
            {
                return prixProduct.PRIX_GROS;
            }
            else
            {
                return prixProduct.PRIX_DETAIL;
            }
        }

        /// <summary>
        /// Retourne la liste des tiers dans les opérations de recherche...
        /// </summary>
        /// <param name="search"></param>
        /// <returns></returns>
        public static async Task<List<View_TRS_TIERS>> FilterTiers(string search)
        {
            List<View_TRS_TIERS> allTiers = await GetInstance().Table<View_TRS_TIERS>().ToListAsync();
            if (search == "")
            {
                return allTiers;
            }
            else
            {
                var tiersFiltred = allTiers.Where(x => x.FULL_NOM_TIERS.ToUpper().Contains(search.ToUpper())).ToList();
                return tiersFiltred;
            }
        }

        /// <summary>
        /// Retourne la liste des produits (utilisé dans la recherche du produit)...
        /// </summary>
        /// <param name="search"></param>
        /// <returns></returns>
        public static async Task<List<View_STK_PRODUITS>> FilterProduits(string search)
        {
            List<View_STK_PRODUITS> allTiers = await GetInstance().Table<View_STK_PRODUITS>().ToListAsync();
            if (search == "")
            {
                return allTiers;
            }
            else
            {
                var tiersFiltred = allTiers.Where(x => x.TRUNCATED_DESIGNATION.ToUpper().Contains(search.ToUpper())).ToList();
                return tiersFiltred;
            }
        }

        /// <summary>
        /// Retourne la liste des tournés ...
        /// </summary>
        /// <param name="search"></param>
        /// <returns></returns>
        public static async Task<List<View_LIV_TOURNEE_DETAIL>> FilterTournee(string search)
        {
            List<View_LIV_TOURNEE_DETAIL> allTiers = await GetInstance().Table<View_LIV_TOURNEE_DETAIL>().ToListAsync();
            if (search == "")
            {
                return allTiers;
            }
            else
            {
                var tiersFiltred = allTiers.Where(x => x.NOM_TIERS.ToUpper().Contains(search.ToUpper())).ToList();
                return tiersFiltred;
            }
        }

        /// <summary>
        /// Retourne la liste des motifs de la table 
        /// </summary>
        /// <param name="typeMotif"></param>
        /// <returns></returns>
        public static async Task<List<BSE_ENCAISS_MOTIFS>> getMotifs(string typeMotif = "ENC")
        {
            List<BSE_ENCAISS_MOTIFS> AllMotifs = await GetInstance().Table<BSE_ENCAISS_MOTIFS>().ToListAsync();
            List<BSE_ENCAISS_MOTIFS> Motifs;
            Motifs = AllMotifs.Where(e => e.TYPE_MOTIF == typeMotif).ToList();
            return Motifs;
        }

        public static async Task<List<View_TRS_ENCAISS>> LoadEncDec(string typeMotif = "")
        {
            List<View_TRS_ENCAISS> AllEncDec = await GetInstance().Table<View_TRS_ENCAISS>().ToListAsync();
            List<View_TRS_ENCAISS> Data;
            if (typeMotif == "" || typeMotif == "All")
            {
                return AllEncDec;
            }
            Data = AllEncDec.Where(e => e.CODE_TYPE == typeMotif).ToList();
            return Data;
        }

        public static async Task<bool> DeleteEncaiss(View_TRS_ENCAISS encaiss)
        {
            await GetInstance().DeleteAsync(encaiss);
            return true;
        }

        public static async Task<View_TRS_ENCAISS> getselectedItemEncaiss(View_TRS_ENCAISS encaiss)
        {
            if (encaiss != null)
            {
                List<View_TRS_ENCAISS> AllEncDec = await GetInstance().Table<View_TRS_ENCAISS>().ToListAsync();
                var item = AllEncDec.Where(x => x.CODE_ENCAISS == encaiss.CODE_ENCAISS).FirstOrDefault();
                if (item != null)
                {
                    return item;
                }
                else
                {
                    return null;
                }
            }
            else
            {
                return null;
            }
        }

        public static async Task<bool> UpdateEncaiss(View_TRS_ENCAISS encaiss)
        {
            await GetInstance().UpdateAsync(encaiss);
            return true;
        }

        public static async Task SyncSysParams()
        {
            var Params = await CrudManager.SysParams.GetParams();
            await GetInstance().DeleteAllAsync<SYS_MOBILE_PARAMETRE>();
            var id = await GetInstance().InsertAsync(Params);
        }

        public static async Task<SYS_MOBILE_PARAMETRE> getParams()
        {
            SYS_MOBILE_PARAMETRE Params = await GetInstance().Table<SYS_MOBILE_PARAMETRE>().FirstOrDefaultAsync();
            return Params;
        }

        /// <summary>
        /// Insert vente et vente detail  dans la table vente
        /// </summary>
        /// <param name="vente"></param>
        /// <returns></returns>
        public static async Task<string> AjoutVente(View_VTE_VENTE vente)
        {
            if ((vente.TOTAL_PAYE + vente.MBL_MT_VERCEMENT) > vente.TOTAL_TTC)
            {
                await UserDialogs.Instance.AlertAsync("Montant versé suppérieur au montant à payer!", AppResources.alrt_msg_Alert, AppResources.alrt_msg_Ok);
                return vente.MBL_MT_VERCEMENT.ToString();
            }
            else
            {
                //var obj = await GetInstance().Table<View_VTE_VENTE>().ToListAsync();
                vente.TOTAL_PAYE = vente.MT_VERSEMENT = vente.TOTAL_RECU;
                vente.TOTAL_RESTE = vente.TOTAL_TTC - vente.TOTAL_PAYE;
                vente.CREATED_BY = App.User.UserName;
                var id = await GetInstance().InsertAsync(vente);
                //vente.CODE_VENTE = vente.ID.ToString() + "/" + App.PrefixCodification;
                //vente.NUM_VENTE = vente.CODE_VENTE;

                vente.CODE_VENTE = await generateCode(vente.TYPE_DOC, vente.ID.ToString());
                vente.NUM_VENTE = await generateNum(vente.TYPE_DOC, vente.ID.ToString());
                vente.DATE_VENTE = DateTime.Now;
                await GetInstance().UpdateAsync(vente);
                foreach (var item in vente.Details)
                {
                    item.CODE_VENTE = vente.CODE_VENTE;
                }
                var id2 = await GetInstance().InsertAllAsync(vente.Details);
                await UpdateStock(vente);
                if (vente.TOTAL_TTC != vente.MT_VERSEMENT)
                {
                    decimal sold = vente.TOTAL_TTC - vente.MT_VERSEMENT;
                    await UpdateSoldTiers(sold, vente.CODE_TIERS);
                }

                await UpdateTourneeDetail(vente);
                await UpdateTournee();

                return vente.CODE_VENTE;
            }
        }

        private static async Task UpdateSoldTiers(decimal sold, string codeTiers)
        {
            List<View_TRS_TIERS> Tiers = await GetInstance().Table<View_TRS_TIERS>().ToListAsync();
            var UpdatedTiers = Tiers.Where(x => x.CODE_TIERS == codeTiers).FirstOrDefault();
            if (UpdatedTiers.SOLDE_TIERS > 0)
            {
                UpdatedTiers.SOLDE_TIERS = UpdatedTiers.SOLDE_TIERS + sold;
            }
            else
            {
                UpdatedTiers.SOLDE_TIERS = UpdatedTiers.SOLDE_TIERS - sold;
            }
            await GetInstance().UpdateAsync(UpdatedTiers);
        }

        private static async Task UpdateSoldTiersApresEncaiss(decimal sold, string codeTiers, string type = "ENC")
        {
            List<View_TRS_TIERS> Tiers = await GetInstance().Table<View_TRS_TIERS>().ToListAsync();
            var UpdatedTiers = Tiers.Where(x => x.CODE_TIERS == codeTiers).FirstOrDefault();
            if (type == "ENC")
            {
                UpdatedTiers.SOLDE_TIERS = UpdatedTiers.SOLDE_TIERS - sold;
            }
            else
            {
                UpdatedTiers.SOLDE_TIERS = UpdatedTiers.SOLDE_TIERS + sold;
            }
            await GetInstance().UpdateAsync(UpdatedTiers);
        }


        private static async Task<decimal> getSoldTiers(string codeTiers)
        {
            List<View_TRS_TIERS> Tiers = await GetInstance().Table<View_TRS_TIERS>().ToListAsync();
            var UpdatedTiers = Tiers.Where(x => x.CODE_TIERS == codeTiers).FirstOrDefault();
            return UpdatedTiers.SOLDE_TIERS;
        }

        /// <summary>
        /// utilisé pour génerer le codeVente...
        /// </summary>
        /// <param name="TypeDoc"></param>
        /// <param name="ID"></param>
        /// <returns></returns>
        public static async Task<string> generateCode(string TypeDoc, string ID)
        {
            var date = DateTime.Now.Year.ToString();
            var date2 = "";
            if (date.Length == 2)
            {
                date2 = "/" + date.Substring(0, 2);
            }
            else if (date.Length == 4)
            {
                date2 = "/" + date.Substring(2, 2);
            }
            var code = date + TypeDoc + ID + date2 + "/" + App.PrefixCodification;
            return code;
        }

        public static async Task<string> generateNum(string TypeDoc, string ID)
        {
            var date = DateTime.Now.Year.ToString();
            var date2 = "";
            if (date.Length == 2)
            {
                date2 = "/" + date.Substring(0, 2);
            }
            else if (date.Length == 4)
            {
                date2 = "/" + date.Substring(2, 2);
            }
            var num = ID + date2 + "/" + App.PrefixCodification;
            return num;
        }

        public static async Task AjoutTiers(View_TRS_TIERS tiers)
        {
            try
            {
                tiers.NOM_TIERS1 = tiers.NOM_TIERS + " " + tiers.PRENOM_TIERS;

                List<BSE_TABLE_TYPE> Types = await GetInstance().Table<BSE_TABLE_TYPE>().ToListAsync();
                tiers.DESIGNATION_TYPE = Types.Where(e => e.CODE_TYPE == tiers.CODE_TYPE).FirstOrDefault()?.DESIGNATION_TYPE;

                List<View_BSE_TIERS_FAMILLE> familles = await GetInstance().Table<View_BSE_TIERS_FAMILLE>().ToListAsync();
                tiers.DESIGN_FAMILLE = familles.Where(e => e.CODE_FAMILLE == tiers.CODE_FAMILLE).FirstOrDefault()?.DESIGN_FAMILLE;

                tiers.ETAT_TIERS = STAT_TIERS_MOBILE.ADDED;

                var id = await GetInstance().InsertAsync(tiers);

                tiers.CODE_TIERS = tiers.ID.ToString() + "/" + App.PrefixCodification + "/MOB";
                tiers.NUM_TIERS = tiers.ID.ToString() + "/" + App.PrefixCodification + "/MOB";
                await GetInstance().UpdateAsync(tiers);
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }

        }

        /// <summary>
        /// Obtenir un produit de la table stk_stock par le code_produit
        /// </summary>
        /// <param name="product"></param>
        /// <returns></returns>
        public static async Task<View_STK_STOCK> getProductfromStock(View_STK_PRODUITS product)
        {
            List<View_STK_STOCK> Stocks = await GetInstance().Table<View_STK_STOCK>().ToListAsync();
            var productInStock = Stocks.Where(e => e.CODE_PRODUIT == product.CODE_PRODUIT).FirstOrDefault();
            if (productInStock == null)
            {
                View_STK_STOCK newStockProduct = new View_STK_STOCK();
                //newStockProduct.ID_STOCK = null;
                newStockProduct.CODE_PRODUIT = product.CODE_PRODUIT;
                newStockProduct.CODE_MAGASIN = App.CODE_MAGASIN;
                newStockProduct.COUT_ACHAT = product.PRIX_ACHAT_TTC;
                newStockProduct.PPA = product.PPA;
                newStockProduct.SHP = product.SHP;
                //newStockProduct.QUANTITE = product.QTE_STOCK;
                newStockProduct.CODE_BARRE_LOT = product.CODE_BARRE;
                newStockProduct.DESIGNATION_PRODUIT = product.DESIGNATION;
                newStockProduct.PRIX_VENTE = product.PRIX_VENTE_HT;
                newStockProduct.HAS_NEW_ID_STOCK = true;


                var count = await GetInstance().InsertAsync(newStockProduct);

                newStockProduct.ID_STOCK = newStockProduct.ID;
                await GetInstance().UpdateAsync(newStockProduct);

                return newStockProduct;
            }
            else
            {
                //productInStock.PRIX_VENTE = product.PRIX_VENTE_HT;
                //await GetInstance().UpdateAsync(productInStock);
                return productInStock;
            }
        }

        public static async Task<List<View_STK_STOCK>> SelectByCodeBarreLot(string cb_prod, string codeMagasin)
        {
            List<View_STK_STOCK> Products = await GetInstance().Table<View_STK_STOCK>().ToListAsync();
            List<View_STK_STOCK> Produit = Products.Where(e => e.CODE_BARRE == cb_prod).Where(e => e.CODE_MAGASIN == codeMagasin).ToList();
            return Produit;
        }

        public static async Task UpdateTiers(View_TRS_TIERS tiers)
        {
            tiers.NOM_TIERS1 = tiers.NOM_TIERS + " " + tiers.PRENOM_TIERS;

            List<BSE_TABLE_TYPE> Types = await GetInstance().Table<BSE_TABLE_TYPE>().ToListAsync();
            tiers.DESIGNATION_TYPE = Types.Where(e => e.CODE_TYPE == tiers.CODE_TYPE).FirstOrDefault()?.DESIGNATION_TYPE;

            List<View_BSE_TIERS_FAMILLE> familles = await GetInstance().Table<View_BSE_TIERS_FAMILLE>().ToListAsync();
            tiers.DESIGN_FAMILLE = familles.Where(e => e.CODE_FAMILLE == tiers.CODE_FAMILLE).FirstOrDefault()?.DESIGN_FAMILLE;

            tiers.ETAT_TIERS = STAT_TIERS_MOBILE.UPDATED;
            await GetInstance().UpdateAsync(tiers);
        }

        /// <summary>
        /// Supprime tous les tokens de la table token et Inseré
        /// </summary>
        /// <param name="token"></param>
        /// <returns></returns>
        public static async Task AjoutToken(Token token)
        {
            await GetInstance().DeleteAllAsync<Token>();

            var id = await GetInstance().InsertAsync(token);

        }
        public static async Task AjoutEnciassement(View_TRS_ENCAISS item)
        {
            if (string.IsNullOrEmpty(item.CODE_ENCAISS))
            {
                List<View_TRS_TIERS> Tiers = await GetInstance().Table<View_TRS_TIERS>().ToListAsync();
                var nom = Tiers.Where(e => e.CODE_TIERS == item.CODE_TIERS).FirstOrDefault()?.NOM_TIERS;
                var prenom = Tiers.Where(e => e.CODE_TIERS == item.CODE_TIERS).FirstOrDefault()?.PRENOM_TIERS;
                item.NOM_TIERS = nom + " " + prenom;

                List<View_BSE_COMPTE> comptes = await GetInstance().Table<View_BSE_COMPTE>().ToListAsync();
                item.DESIGN_COMPTE = comptes.Where(e => e.CODE_COMPTE == item.CODE_COMPTE).FirstOrDefault()?.DESIGN_COMPTE;

                item.CREATED_BY = App.User.UserName;

                List<BSE_ENCAISS_MOTIFS> motif = await GetInstance().Table<BSE_ENCAISS_MOTIFS>().ToListAsync();
                item.DESIGN_MOTIF = motif.Where(e => e.CODE_MOTIF == item.CODE_MOTIF).FirstOrDefault()?.DESIGN_MOTIF;

                var id = await GetInstance().InsertAsync(item);
                item.CODE_ENCAISS = await generateCode(item.CODE_TYPE, item.ID.ToString());
                item.NUM_ENCAISS = await generateNum(item.CODE_TYPE, item.ID.ToString());
                await GetInstance().UpdateAsync(item);
            }
            else
            {
                List<View_BSE_COMPTE> comptes = await GetInstance().Table<View_BSE_COMPTE>().ToListAsync();
                item.DESIGN_COMPTE = comptes.Where(e => e.CODE_COMPTE == item.CODE_COMPTE).FirstOrDefault()?.DESIGN_COMPTE;

                item.CREATED_BY = App.User.UserName;

                List<BSE_ENCAISS_MOTIFS> motif = await GetInstance().Table<BSE_ENCAISS_MOTIFS>().ToListAsync();
                item.DESIGN_MOTIF = motif.Where(e => e.CODE_MOTIF == item.CODE_MOTIF).FirstOrDefault()?.DESIGN_MOTIF;

                var id = await GetInstance().UpdateAsync(item);
            }
            await UpdateSoldTiersApresEncaiss(item.TOTAL_ENCAISS, item.CODE_TIERS, item.CODE_TYPE);
        }

        /// <summary>
        /// Mise a jour de la quantité du stock  
        /// </summary>
        /// <param name="vente"></param>
        /// <returns></returns>
        public static async Task UpdateStock(View_VTE_VENTE vente)
        {
            var stock = await GetInstance().Table<View_STK_STOCK>().ToListAsync();

            foreach (var item in stock)
            {
                foreach (var items in vente.Details)
                {
                    if (item.ID_STOCK == items.ID_STOCK)
                    {
                        if (items.QUANTITE > 0)
                        {
                            item.OLD_QUANTITE = item.OLD_QUANTITE - items.QUANTITE;
                            item.QUANTITE = item.QUANTITE - items.QUANTITE;
                            await GetInstance().UpdateAsync(item);
                        }
                    }
                }
            }
        }

        public static async Task UpdateTourneeDetail(View_VTE_VENTE vente)
        {
            string codeTourneeDetail = vente.MBL_CODE_TOURNEE_DETAIL;


            var tournees = await GetInstance().Table<View_LIV_TOURNEE_DETAIL>().ToListAsync();

            foreach (var item in tournees)
            {
                if (item.CODE_DETAIL == codeTourneeDetail)
                {
                    item.CODE_VENTE = vente.CODE_VENTE;
                    item.CODE_ETAT = TourneeStatus.Delevred;
                    item.ETAT_COLOR = "#008000";
                    item.SOLDE_TIERS = await getSoldTiers(item.CODE_TIERS);
                    item.GPS_LATITUDE = vente.GPS_LATITUDE;
                    item.GPS_LONGITUDE = vente.GPS_LATITUDE;
                    await GetInstance().UpdateAsync(item);
                }
            }
        }

        /// <summary>
        /// mise à jour des tournés par les points de lat et long
        /// </summary>
        /// <param name="tiers"></param>
        /// <returns></returns>
        public static async Task<bool> saveGPSToTiers(View_TRS_TIERS tiers)
        {
            if (!XpertHelper.IsNullOrEmpty(tiers) && !XpertHelper.IsNullOrEmpty(tiers.CODE_TIERS))
            {
                var tournees = await GetInstance().Table<View_TRS_TIERS>().ToListAsync();

                foreach (var item in tournees)
                {
                    if (item.CODE_TIERS == tiers.CODE_TIERS)
                    {
                        item.GPS_LATITUDE = tiers.GPS_LATITUDE;
                        item.GPS_LONGITUDE = tiers.GPS_LONGITUDE;
                        await GetInstance().UpdateAsync(item);
                        return true;
                    }
                }
            }
            return false;
        }

        public static async Task UpdateTournee()
        {
            var tournee = await GetInstance().Table<View_LIV_TOURNEE>().ToListAsync();
            foreach (var item in tournee)
            {
                item.NBR_EN_DELEVRED = item.NBR_EN_DELEVRED + 1;
                await GetInstance().UpdateAsync(item);
            }
        }

        /// <summary>
        /// retourne true si les informations stockés dans la table sys_user sont identique a celle qui sont donnés ...
        /// </summary>
        /// <param name="user"></param>
        /// <returns></returns>
        public static async Task<SYS_USER> AuthUser(User user)
        {
            bool validInformations = false;

            //Obtenir la liste des users stocké dans la table sys_user
            var Users = await GetInstance().Table<SYS_USER>().ToListAsync();
            foreach (var item in Users)
            {
                var password = XpertHelper.GetMD5Hash(user.PassWord);

                //Compare le nom d'utilisateur avec le mot de passe ...
                //prendre en charge la connexion avec mot de pass unique 
                if (item.ID_USER.ToLower() == user.UserName.ToLower() || item.PASS_USER == password)
                {
                    return item;
                }
            }
            return null;
        }

        /// <summary>
        /// Retourne un objet user s'il existe un avec le meme userid
        /// </summary>
        /// <param name="userid"></param>
        /// <returns></returns>
        public static async Task<SYS_USER> getUserInfo(string userid)
        {
            if (userid != null)
            {
                var Users = await GetInstance().Table<SYS_USER>().ToListAsync();
                var user = Users.Where(x => x.ID_USER.ToLower() == userid.ToLower()).FirstOrDefault();
                if (user != null)
                {
                    return user;
                }
            }
            return null;
        }

        /// <summary>
        /// Obtenir le token a partir du db local en utilisant le userId...
        /// </summary>
        /// <param name="user"></param>
        /// <returns></returns>
        public static async Task<Token> getToken(User user)
        {
            Token validToken = new Token();
            var Tokens = await GetInstance().Table<Token>().ToListAsync();
            foreach (var item in Tokens)
            {
                if (item.userID.ToLower() == user.UserName.ToLower())
                {
                    validToken = item;
                    return validToken;
                }
            }
            return null;
        }

        /// <summary>
        /// Obtenir la liste des permissions de la table sys_objet_permission
        /// </summary>
        /// <returns></returns>
        public static async Task<List<SYS_OBJET_PERMISSION>> getPermission()
        {
            var permission = await GetInstance().Table<SYS_OBJET_PERMISSION>().ToListAsync();
            return permission;
        }

        /// <summary>
        /// Obtenir la liste des familles des tiers...
        /// </summary>
        /// <returns></returns>
        public static async Task<List<View_BSE_TIERS_FAMILLE>> getFamille()
        {
            var Famille = await GetInstance().Table<View_BSE_TIERS_FAMILLE>().ToListAsync();
            return Famille;
        }

        /// <summary>
        /// Obtenir la liste des types des tiers 
        /// </summary>
        /// <returns></returns>
        public static async Task<List<BSE_TABLE_TYPE>> getTypeTiers()
        {
            var TypeTiers = await GetInstance().Table<BSE_TABLE_TYPE>().ToListAsync();
            return TypeTiers;
        }

        /// <summary>
        /// Obtenir la liste des secteurs 
        /// </summary>
        /// <returns></returns>
        public static async Task<List<BSE_TABLE>> getSecteurs()
        {
            var Secteurs = await GetInstance().Table<BSE_TABLE>().ToListAsync();
            return Secteurs;
        }


        /// <summary>
        /// Synchronisation de la liste des ventes aux serveur
        /// </summary>
        /// <returns></returns>
        public static async Task<string> SyncVenteToServer()
        {
            try
            {
                UserDialogs.Instance.ShowLoading(AppResources.txt_Waiting);

                var ListVentes = await GetInstance().Table<View_VTE_VENTE>().ToListAsync();
                var vteDetails = await GetInstance().Table<View_VTE_VENTE_LOT>().ToListAsync();

                if (ListVentes.Count > 0 && ListVentes != null)
                {
                    foreach (var iVente in ListVentes)
                    {
                        List<View_VTE_VENTE_LOT> objdetail = new List<View_VTE_VENTE_LOT>();
                        try
                        {
                            objdetail = vteDetails?.Where(x => x.CODE_VENTE == iVente.CODE_VENTE)?.ToList();
                        }
                        catch
                        {
                            objdetail = null;
                        }
                        finally
                        {
                            iVente.Details = objdetail;
                        }
                    }

                    var compte = await getComptes();

                    var bll = CrudManager.GetVteBll(VentesTypes.Livraison);
                    var res = await bll.SyncVentes(ListVentes, App.PrefixCodification, App.CODE_MAGASIN, compte.FirstOrDefault().CODE_COMPTE);

                    await GetInstance().DeleteAllAsync<View_VTE_VENTE>();
                    await GetInstance().DeleteAllAsync<View_VTE_VENTE_LOT>();

                    UserDialogs.Instance.HideLoading();
                    return res;
                }
            }
            catch (Exception ex)
            {
                UserDialogs.Instance.HideLoading();
                await UserDialogs.Instance.AlertAsync(WSApi2.GetExceptionMessage(ex), AppResources.alrt_msg_Alert, AppResources.alrt_msg_Ok);
                //await UserDialogs.Instance.AlertAsync("Erreur de synchronisation des ventes!!", AppResources.alrt_msg_Alert, AppResources.alrt_msg_Ok);
            }
            return "";
        }

        /// <summary>
        /// synchronisation de la liste des tiers aux serveur
        /// </summary>
        /// <returns></returns>
        public static async Task SyncTiersToServer()
        {
            try
            {
                var Tiers = await GetInstance().Table<View_TRS_TIERS>().ToListAsync();
                List<View_TRS_TIERS> listNewTiers = new List<View_TRS_TIERS>();
                foreach (var item in Tiers)
                {
                    if (item.ETAT_TIERS == STAT_TIERS_MOBILE.ADDED || item.ETAT_TIERS == STAT_TIERS_MOBILE.UPDATED)
                    {
                        listNewTiers.Add(item);
                    }
                }
                if (listNewTiers.Count > 0 && listNewTiers != null)
                {
                    var bll = new TiersManager();
                    var res = await bll.SyncTiers(listNewTiers);
                }
            }
            catch (Exception ex)
            {
                UserDialogs.Instance.HideLoading();
                await UserDialogs.Instance.AlertAsync(WSApi2.GetExceptionMessage(ex), AppResources.alrt_msg_Alert, AppResources.alrt_msg_Ok);
                //await UserDialogs.Instance.AlertAsync("erreur de synchronisation des Tiers!!", AppResources.alrt_msg_Alert, AppResources.alrt_msg_Ok);
            }
        }

        /// <summary>
        /// Synchronisation de la liste des encaiss aux serveur nn 
        /// </summary>
        /// <returns></returns>
        public static async Task SyncEncaissToServer()
        {
            try
            {
                var encaiss = await GetInstance().Table<View_TRS_ENCAISS>().ToListAsync();
                if (encaiss.Count > 0 && encaiss != null)
                {
                    var bll = new EncaissManager();
                    var res = await bll.SyncEncaiss(encaiss);
                    await GetInstance().DeleteAllAsync<View_TRS_ENCAISS>();
                }
            }
            catch (Exception ex)
            {
                UserDialogs.Instance.HideLoading();
                await UserDialogs.Instance.AlertAsync(WSApi2.GetExceptionMessage(ex), AppResources.alrt_msg_Alert, AppResources.alrt_msg_Ok);
                //await UserDialogs.Instance.AlertAsync("erreur de synchronisation des Encaissements !!", AppResources.alrt_msg_Alert, AppResources.alrt_msg_Ok);
            }
        }

        /// <summary>
        /// Obtenire la liste des details de vente par CODE_VENTE
        /// </summary>
        /// <param name="CodeVente"></param>
        /// <returns></returns>
        public static async Task<List<View_VTE_VENTE_LOT>> getVenteDetails(string CodeVente)
        {
            List<View_VTE_VENTE_LOT> ventes = await GetInstance().Table<View_VTE_VENTE_LOT>().ToListAsync();
            List<View_VTE_VENTE_LOT> VenteDetail = ventes.Where(e => e.CODE_VENTE == CodeVente).ToList();
            return VenteDetail;
        }

        public static async Task<SYS_CONFIGURATION_MACHINE> AjoutPrefix()
        {
            UserDialogs.Instance.ShowLoading(AppResources.txt_Waiting);

            //Nom de la machine
            var userHost = Dns.GetHostName();
            //Adresse IP
            var userIp = Dns.GetHostEntry(userHost).AddressList[0].ToString();

            // Device Model (SMG-950U, iPhone10,6)
            var device = DeviceInfo.Model;

            var model = Environment.MachineName;

            // Device Name (Motz's iPhone)
            var deviceName = DeviceInfo.Name;

            var deviceNameRandom = XpertHelper.RandomString(2);

            deviceName = deviceName + "/" + deviceNameRandom;

            SYS_CONFIGURATION_MACHINE prefix = new SYS_CONFIGURATION_MACHINE();
            prefix.IP = userIp;
            prefix.MACHINE = deviceName;

            var bll = new SYS_MACHINE_CONFIG_Manager();
            var res = await bll.AddMachine(prefix);
            UserDialogs.Instance.HideLoading();
            if (res)
            {
                return prefix;
            }
            else
            {
                return null;
            }
        }

        public static async Task<SYS_CONFIGURATION_MACHINE> getPrefix()
        {
            var prefix = await GetInstance().Table<SYS_CONFIGURATION_MACHINE>().FirstOrDefaultAsync();
            if (prefix != null)
            {
                string deviceName = prefix.MACHINE;
                SYS_MACHINE_CONFIG_Manager bll = new SYS_MACHINE_CONFIG_Manager();
                var res = await bll.GetPrefix(deviceName);
                await GetInstance().DeleteAllAsync<SYS_CONFIGURATION_MACHINE>();
                var id = await GetInstance().InsertAsync(res);
                return res;
            }
            return null;
        }
        public static async Task AssignPrefix()
        {
            var prefix = await GetInstance().Table<SYS_CONFIGURATION_MACHINE>().FirstOrDefaultAsync();
            if (!(string.IsNullOrEmpty(prefix.PREFIX)))
            {
                App.PrefixCodification = prefix.PREFIX;
            }
            else
            {
                //await DisplayAlert(AppResources.alrt_msg_Alert, "Veuillez configurer votre prefixe", AppResources.alrt_msg_Ok);
            }
        }
        public static async Task AssignMagasin()
        {
            var obj = await GetInstance().Table<View_LIV_TOURNEE>().ToListAsync();
            if (obj != null && obj.Count > 0)
            {
                App.CODE_MAGASIN = obj[0].CODE_MAGASIN;
            }
        }

        public static bool TableExists(SQLiteAsyncConnection connection)
        {
            const string cmdText = "SELECT name FROM sqlite_master WHERE type='table' AND name=View_LIV_TOURNEE";
            var cmd = connection.ExecuteScalarAsync<string>(cmdText);
            if (cmd.Result.Length > 0)
            {
                return true;
            }
            else
            {
                return false;
            }
            //return string.IsNullOrEmpty(cmd) ? false : true;
        }

    }
}
