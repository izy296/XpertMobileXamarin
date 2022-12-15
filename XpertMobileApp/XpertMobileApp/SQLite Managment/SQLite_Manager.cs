using Acr.UserDialogs;
using SQLite;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Linq;
using System.Net;
using System.Threading;
using System.Threading.Tasks;
using Xamarin.Essentials;
using Xamarin.Forms;
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
using XpertMobileApp.Views.Helper;

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
                await GetInstance().CreateTableAsync<BSE_PRODUIT_FAMILLE>();
                await GetInstance().CreateTableAsync<SYS_CONFIGURATION_MACHINE>();
                await GetInstance().CreateTableAsync<SYS_MOBILE_PARAMETRE>();
                await GetInstance().CreateTableAsync<View_TRS_ENCAISS>();
                await GetInstance().CreateTableAsync<View_BSE_COMPTE>();
                await GetInstance().CreateTableAsync<BSE_ENCAISS_MOTIFS>();
                await GetInstance().CreateTableAsync<View_VTE_COMMANDE>();
                await GetInstance().CreateTableAsync<View_BSE_PRODUIT_PRIX_VENTE_BY_QUANTITY>();
                await GetInstance().CreateTableAsync<View_STK_TRANSFERT>();
                await GetInstance().CreateTableAsync<View_STK_TRANSFERT_DETAIL>();
                await GetInstance().CreateTableAsync<View_BSE_PRODUIT_AUTRE_UNITE>();
                await GetInstance().CreateTableAsync<View_BSE_PRODUIT_UNITE_COEFFICIENT>();
                await GetInstance().CreateTableAsync<View_BSE_PRODUIT_PRIX_VENTE>();
                await GetInstance().CreateTableAsync<View_VTE_VENTE_LIVRAISON>();
                await GetInstance().CreateTableAsync<BSE_DOCUMENT_STATUS>();
                await GetInstance().CreateTableAsync<BSE_PRODUIT_TYPE>();
                await GetInstance().CreateTableAsync<STK_PRODUITS_IMAGES>();
                await GetInstance().CreateTableAsync<BSE_PRODUIT_LABO>();
                await CreateView_TRS_TIERS_ACTIVITY_Async();
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
            }
        }


        public static async Task<IEnumerable<View_STK_STOCK>> GetProduitPrixUniteByCodeFamille(string codeFamille, string columnName = "p.DESIGNATION_PRODUIT", string order = "ASC")
        {

            // 
            string query = $@"SELECT DISTINCT s.ID_STOCK, s.LOT, s.DATE_PEREMPTION, p.CODE_PRODUIT,p.DESIGNATION_PRODUIT, s.CODE_BARRE_LOT, s.CODE_BARRE,
                                             CASE 
                                                    WHEN pv.CODE_FAMILLE = '' THEN p.PRIX_VENTE_HT 
                                                    WHEN pv.VALEUR = 0 THEN p.PRIX_VENTE_HT 
                                                    WHEN pv.VALEUR IS NULL THEN p.PRIX_VENTE_HT
                                                    ELSE pv.VALEUR 
                                                end PRIX_VENTE, 
                                                s.QUANTITE,
                                                s.QTE_STOCK, 
                                                p.CODE_UNITE_ACHAT, 
                                                p.CODE_UNITE_VENTE,
                                                pim.IMAGE
                                            FROM View_STK_STOCK s 
                                            JOIN View_STK_PRODUITS p on p.CODE_PRODUIT = s.CODE_PRODUIT 
                                            LEFT JOIN STK_PRODUITS_IMAGES pim on p.CODE_PRODUIT = pim.CODE_PRODUIT AND pim.DEFAULT_IMAGE = 1
                                            LEFT JOIN View_BSE_PRODUIT_PRIX_VENTE pv on p.CODE_PRODUIT = pv.CODE_PRODUIT AND pv.CODE_FAMILLE= '{codeFamille}'
                                ORDER BY {columnName} {order}";
            var list = await GetInstance().QueryAsync<View_STK_STOCK>(query);
            return list;
        }

        public static async Task<View_STK_STOCK> GetProduitPrixUniteByCodeProduit(string codeFamille, string codeProduit = "", string columnName = "p.DESIGNATION_PRODUIT", string order = "ASC")
        {

            // 
            string query = $@"SELECT DISTINCT s.ID_STOCK, s.LOT, s.DATE_PEREMPTION, p.CODE_PRODUIT,p.DESIGNATION_PRODUIT, s.CODE_BARRE_LOT, s.CODE_BARRE,
                                             CASE 
                                                    WHEN pv.CODE_FAMILLE = '' THEN p.PRIX_VENTE_HT 
                                                    WHEN pv.VALEUR = 0 THEN p.PRIX_VENTE_HT 
                                                    WHEN pv.VALEUR IS NULL THEN p.PRIX_VENTE_HT
                                                    ELSE pv.VALEUR 
                                                end PRIX_VENTE, 
                                                s.QUANTITE,
                                                s.QTE_STOCK, 
                                                p.CODE_UNITE_ACHAT, 
                                                p.CODE_UNITE_VENTE,
                                                pim.IMAGE
                                            FROM View_STK_STOCK s 
                                            JOIN View_STK_PRODUITS p on p.CODE_PRODUIT = s.CODE_PRODUIT 
                                            LEFT JOIN STK_PRODUITS_IMAGES pim on p.CODE_PRODUIT = pim.CODE_PRODUIT AND pim.DEFAULT_IMAGE = 1
                                            LEFT JOIN View_BSE_PRODUIT_PRIX_VENTE pv on p.CODE_PRODUIT = pv.CODE_PRODUIT AND pv.CODE_FAMILLE= '{codeFamille}'
                                WHERE p.CODE_PRODUIT = '{codeProduit}'
                                ORDER BY {columnName} {order}";
            var list = await GetInstance().QueryAsync<View_STK_STOCK>(query);
            return list.FirstOrDefault();
        }

        public static async Task SyncImages()
        {
            try
            {
                await SyncData<STK_PRODUITS_IMAGES, STK_PRODUITS_IMAGES_XCOM>(false, "", "GetImages");
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public static byte[] GetImage(string codeProduit)
        {
            try
            {
                var listImages = GetInstance().Table<STK_PRODUITS_IMAGES>().Where(e => e.CODE_PRODUIT == codeProduit).FirstOrDefaultAsync().Result;
                if (listImages != null)
                    return listImages.IMAGE;
                return null;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public static async Task<IEnumerable<View_BSE_PRODUIT_AUTRE_UNITE>> GetUniteByProduit(string codeProduit = "")
        {

            // 
            string query = $@"SELECT DISTINCT DESIGNATION_UNITE,PRIX_VENTE,COEFFICIENT FROM View_BSE_PRODUIT_AUTRE_UNITE WHERE CODE_PRODUIT ='{codeProduit}'";
            var list = await GetInstance().QueryAsync<View_BSE_PRODUIT_AUTRE_UNITE>(query);
            return list;
        }


        public static async Task<bool> CreateView_TRS_TIERS_ACTIVITY_Async()
        {
            var res = true;
            try
            {
                string dropView = "DROP VIEW View_TRS_TIERS_ACTIVITY";

                await GetInstance().ExecuteAsync(dropView);

            }
            catch (Exception)
            {
                //
            }

            try
            {
                string queryNew = @"CREATE VIEW View_TRS_TIERS_ACTIVITY AS 
                                    SELECT * FROM ( 
                                    SELECT v.CODE_VENTE CODE_DOC,  
                                            v.TOTAL_PAYE, 
                                            v.TYPE_DOC, 
                                            v.CODE_TIERS, 
                                            v.DATE_VENTE DATE_DOC
                                            FROM View_VTE_VENTE v 
                                            WHERE v.TYPE_DOC ='BL' or v.TYPE_DOC ='BR' 
                                    UNION ALL 
                                    SELECT c.CODE_VENTE CODE_DOC,  
                                            c.TOTAL_PAYE, 
                                            c.TYPE_DOC, 
                                            c.CODE_TIERS, 
                                            c.DATE_VENTE DATE_DOC
                                            FROM View_VTE_COMMANDE c 
                                    UNION ALL 
                                    SELECT e.CODE_ENCAISS CODE_DOC, 
                                              e.TOTAL_ENCAISS TOTAL_PAYE, 
                                              'ENC' TYPE_DOC, 
                                              e.CODE_TIERS, 
                                              e.DATE_ENCAISS DATE_DOC 
                                            FROM View_TRS_ENCAISS e 
                                            )T ORDER BY DATE_DOC DESC";

                await GetInstance().ExecuteAsync(queryNew);

            }
            catch (Exception Ex)
            {
                //
            }
            return res;
        }

        public static async Task<IEnumerable<View_TRS_TIERS_ACTIVITY>> Get_TRS_TIERS_ACTIVITY_Async(string codeTiers)
        {
            string Query = String.Format("SELECT * FROM View_TRS_TIERS_ACTIVITY WHERE CODE_TIERS = '{0}'", codeTiers);
            var res = await GetInstance().QueryAsync<View_TRS_TIERS_ACTIVITY>(Query);
            if (res.Count != 0 && res != null)
            {
                return res;
            }
            else return null;
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

                var obj21 = await GetInstance().Table<View_VTE_VENTE_LIVRAISON>().ToListAsync();
                countListe.Add(obj21.Count);

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
                throw ex;
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
        public static async Task SyncData<TView, TTabel>(bool _selectAll = true, string param = "", string methodName = "", bool delete = false)
        {
            var ListItems = new List<TView>();

            var items = await getItemsUnsyncronised<TView, TTabel>(_selectAll, param, methodName);
            if (delete)
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
            try
            {
                //bool isconnected = await App.IsConected();
                if (App.Online)
                {
                    await InitialisationDbLocal();
                    UserDialogs.Instance.ShowLoading(AppResources.txt_Waiting);
                    await SyncLabos();
                    await SyncProduitFamille();
                    await SyncStatusCommande();
                    await SyncProduitType();
                    await SyncCommande(); //worked  await SyncLivTournee();//worked !
                    await SyncStatusCommande(); //To Delete On Download
                    await SyncProduitType(); //To Delete On Download
                    await SyncCommande(); //worked //To Delete On Download

                    await SyncLivTournee();//worked !
                    await SyncLivTourneeDetail(); //worked !
                    await SyncTiers();
                    await SyncStock();//worked !

                    await syncPermission();//worked ! //To Delete On Download
                    await SyncSysParams(); //worked ! //To Delete On Download

                    await SyncProductPriceByQuantity();
                    await SyncProduitsByMagasin();

                    await SyncFamille();//worked ! //To Delete On Download
                    await SyncTypeTiers();//worked ! //To Delete On Download
                    await SyncSecteurs();//worked ! //To Delete On Download
                    await SyncBseCompte();//worked ! //To Delete On Download
                    await SyncMotifs();//worked ! //To Delete On Download

                    await SyncUsers(); //worked ! 
                    await SyncConfigMachine(); //worked 
                    await SyncTransfers();
                    await SyncTransfersDetail();
                    await SyncProduiteUnite();
                    await SyncProduiteUniteAutre();
                    await SyncData<View_BSE_PRODUIT_PRIX_VENTE, BSE_PRODUIT_PRIX_VENTE>();
                    await SyncImages();



                    //await SyncData<View_TRS_ENCAISS, TRS_ENCAISS>();
                    //await syncSession(); //worked !
                    //await SyncMagasin();                                                                                           
                    //await SyncProduct();
                    //await SyncData<View_TRS_ENCAISS, TRS_ENCAISS>(); // we don't need to sync all the encaiss 
                    //await SyncData<View_STK_STOCK, STK_STOCK>();
                    //await SyncData<View_VTE_VENTE, VTE_VENTE>();
                    //await SyncData<View_VTE_VENTE_LIVRAISON,VTE_VENTE>();
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
            catch (Exception ex)
            {
                UserDialogs.Instance.HideLoading();
                throw ex;
            }
        }

        private static async Task SyncTiers()
        {
            try
            {
                var listeTourneeClents = await GetInstance().Table<View_LIV_TOURNEE>().ToListAsync();
                foreach (var tournee in listeTourneeClents)
                {
                    var itemsS = await WebServiceClient.GetClients(tournee.CODE_TOURNEE);
                    if (itemsS != null)
                        await GetInstance().InsertAllAsync(itemsS);
                }
            }
            catch (Exception ex)
            {

                throw ex;
            }
        }

        public static async Task SynchroniseDELETE()
        {
            try
            {
                await GetInstance().DeleteAllAsync<View_STK_PRODUITS>();
                await GetInstance().DeleteAllAsync<TRS_JOURNEES>();
                await GetInstance().DeleteAllAsync<View_STK_STOCK>();
                await GetInstance().DeleteAllAsync<View_LIV_TOURNEE_DETAIL>();
                await GetInstance().DeleteAllAsync<View_LIV_TOURNEE>();
                await GetInstance().DeleteAllAsync<View_TRS_TIERS>();
                await GetInstance().DeleteAllAsync<View_VTE_VENTE>();
                await GetInstance().DeleteAllAsync<View_VTE_VENTE_LOT>();
                await GetInstance().DeleteAllAsync<View_TRS_ENCAISS>();
                await GetInstance().DeleteAllAsync<BSE_PRODUIT_FAMILLE>();
                await GetInstance().DeleteAllAsync<View_STK_PRODUITS>();
                await UserDialogs.Instance.AlertAsync("Suppression des tables de base faite avec succes", AppResources.alrt_msg_Alert, AppResources.alrt_msg_Ok);
            }
            catch (Exception ex)
            {
                UserDialogs.Instance.HideLoading();
                throw ex;
            }
        }

        public static async Task<List<View_TRS_TIERS>> GetClients(string codeTournee)
        {
            try
            {
                var listeTourneeClents = await GetInstance().Table<View_LIV_TOURNEE_DETAIL>().ToListAsync();
                var listClients = await GetInstance().Table<View_TRS_TIERS>().ToListAsync();

                var tourneeCLients = listClients.Where(p => listeTourneeClents.Any(p2 => p2.CODE_TIERS == p.CODE_TIERS)).ToList();
                return tourneeCLients as List<View_TRS_TIERS>;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public static async Task<View_TRS_TIERS> GetClient(string codeTier)
        {
            try
            {
                var listClients = await GetInstance().Table<View_TRS_TIERS>().ToListAsync();

                var tourneeCLients = listClients.Where(p => p.CODE_TIERS == codeTier);
                if (tourneeCLients.Count() != 0)
                    return tourneeCLients.First();
                else return null;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        /// <summary>
        /// Synchronisation des produits par Magasin
        /// </summary>
        /// <returns></returns>

        public static async Task SyncProduitsByMagasin()
        {
            try
            {
                await SyncData<View_STK_PRODUITS, STK_PRODUITS_XCOM>(false, "codeMagasin=" + App.CODE_MAGASIN, "GetProduitFromMagasin");
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        /// <summary>
        /// Synchronisation de vue View_BSE_PRODUIT_UNITE_COEFFICIENT
        /// </summary>
        /// <returns></returns>

        public static async Task SyncProduiteUnite()
        {
            try
            {
                var MethodName = "GetAllProduiteUniteCoeficient";
                await SyncData<View_BSE_PRODUIT_UNITE_COEFFICIENT, BSE_PRODUIT_AUTRE_UNITE_XCOM>(false, "", MethodName);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        /// <summary>
        /// Synchronisation de vue View_BSE_PRODUIT_AUTRE_UNITE
        /// </summary>
        /// <returns></returns>

        public static async Task SyncProduiteUniteAutre()
        {
            try
            {
                var MethodName = "GetAllProduiteAutreUnite";
                await SyncData<View_BSE_PRODUIT_AUTRE_UNITE, BSE_PRODUIT_AUTRE_UNITE_XCOM>(false, "", MethodName);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        /// <summary>
        /// Synchronisation de vue View_TRS_ENCAISS
        /// </summary>
        /// <returns></returns>

        public static async Task SyncEncaiss()
        {
            try
            {
                var MethodName = "GetEncaissParTournee";
                //var parmetre
                await SyncData<View_TRS_ENCAISS, TRS_ENCAISS>(false, "", MethodName);
            }
            catch (Exception ex)
            {
                throw ex;
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
                //await SyncTiersToServer();
                await SyncEncaissToServer();
                //await SyncVenteToServer(); // error while coppying content to a stream
                await SyncTourneesToServer();

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

        /// <summary>
        /// Uploader la liste des tiers aux serveur
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

                await GetInstance().DeleteAllAsync<View_TRS_TIERS>();
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
                var encaissList = encaiss.Where(e => e.IS_SYNCHRONISABLE == true).ToList<View_TRS_ENCAISS>();
                if (encaissList.Count() > 0 && encaissList != null)
                {
                    var bll = new EncaissManager();
                    var res = await bll.SyncEncaiss(encaissList);
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
        /// Obtenir la liste des types des tiers 
        /// </summary>
        /// <returns></returns>
        public static async Task<List<BSE_TABLE_TYPE>> GetTypeTiers()
        {
            var TypeTiers = await GetInstance().Table<BSE_TABLE_TYPE>().ToListAsync();
            return TypeTiers;
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
        /// Ajout les encaissements aux sqlite ...
        /// </summary>
        /// <param name="item"></param>
        /// <returns></returns>
        public static async Task AjoutEncaissement(View_TRS_ENCAISS item, Location location, string codeCompte = "")
        {
            if (!string.IsNullOrEmpty(App.PrefixCodification))
            {
                if (string.IsNullOrEmpty(item.CODE_ENCAISS))
                {
                    List<View_TRS_TIERS> Tiers = await GetInstance().Table<View_TRS_TIERS>().ToListAsync();
                    var nom = Tiers.Where(e => e.CODE_TIERS == item.CODE_TIERS).FirstOrDefault()?.NOM_TIERS;
                    var prenom = Tiers.Where(e => e.CODE_TIERS == item.CODE_TIERS).FirstOrDefault()?.PRENOM_TIERS;
                    item.NOM_TIERS = nom + " " + prenom;

                    List<View_BSE_COMPTE> comptes = await GetInstance().Table<View_BSE_COMPTE>().ToListAsync();
                    item.DESIGN_COMPTE = comptes.Where(e => e.CODE_COMPTE == item.CODE_COMPTE).FirstOrDefault()?.DESIGN_COMPTE;
                    item.CREATED_ON = DateTime.Now;
                    item.CREATED_BY = App.User.UserName;
                    List<BSE_ENCAISS_MOTIFS> motif = await GetInstance().Table<BSE_ENCAISS_MOTIFS>().ToListAsync();
                    item.DESIGN_MOTIF = motif.Where(e => e.CODE_MOTIF == item.CODE_MOTIF).FirstOrDefault()?.DESIGN_MOTIF;
                    if (location != null)
                    {
                        item.GPS_LATITUDE = location.Latitude;
                        item.GPS_LONGITUDE = location.Longitude;
                    }
                    var id = await GetInstance().InsertAsync(item);

                    await GetInstance().UpdateAsync(item);
                    if (item.CODE_TYPE == "ENC")
                    {
                        await UserDialogs.Instance.AlertAsync("Versement a été effectuée avec succès!", AppResources.alrt_msg_Alert, AppResources.alrt_msg_Ok);
                    }
                    else
                    {
                        await UserDialogs.Instance.AlertAsync("Remboursement a été effectuée avec succès!", AppResources.alrt_msg_Alert, AppResources.alrt_msg_Ok);
                    }
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
                await UpdateSoldeTiers(item.TOTAL_ENCAISS, item.CODE_TIERS, item.CODE_TYPE);
            }
            else
            {
                await UserDialogs.Instance.AlertAsync("Veuillez configurer le Prefix", AppResources.alrt_msg_Alert, AppResources.alrt_msg_Ok);
            }
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

        public static async Task<bool> DeleteEncaiss(View_TRS_ENCAISS encaiss)
        {
            await GetInstance().DeleteAsync(encaiss);
            return true;
        }

        public static async Task<bool> UpdateEncaiss(View_TRS_ENCAISS encaiss)
        {
            await GetInstance().UpdateAsync(encaiss);
            return true;
        }

        //private static async Task UpdateSoldTiers(decimal sold, string codeTiers)
        //{
        //    List<View_TRS_TIERS> Tiers = await GetInstance().Table<View_TRS_TIERS>().ToListAsync();
        //    var UpdatedTiers = Tiers.Where(x => x.CODE_TIERS == codeTiers).FirstOrDefault();
        //    if (UpdatedTiers.SOLDE_TIERS > 0)
        //    {
        //        UpdatedTiers.SOLDE_TIERS = UpdatedTiers.SOLDE_TIERS + sold;
        //    }
        //    else
        //    {
        //        UpdatedTiers.SOLDE_TIERS = UpdatedTiers.SOLDE_TIERS - sold;
        //    }
        //    await GetInstance().UpdateAsync(UpdatedTiers);
        //}

        private static async Task UpdateSoldeTiers(decimal sold, string codeTiers, string type = "ENC")
        {
            try
            {
                View_TRS_TIERS UpdatedTiers = await GetInstance().Table<View_TRS_TIERS>().FirstOrDefaultAsync(x => x.CODE_TIERS == codeTiers);
                View_LIV_TOURNEE_DETAIL Tournnee = await GetInstance().Table<View_LIV_TOURNEE_DETAIL>().FirstOrDefaultAsync(x => x.CODE_TIERS == codeTiers);
                if (UpdatedTiers != null)
                {
                    if (type == "ENC")
                    {
                        UpdatedTiers.SOLDE_TIERS = UpdatedTiers.SOLDE_TIERS - sold;
                    }
                    else
                    {
                        UpdatedTiers.SOLDE_TIERS = UpdatedTiers.SOLDE_TIERS + sold;
                    }
                    await GetInstance().UpdateAsync(UpdatedTiers);

                    // juste dans offline :: update solde tierss dans tournnee juste dans offline car le champs de solde tiers dans tournnee not calculable dans offline 
                    if (Tournnee != null)
                    {
                        Tournnee.SOLDE_TIERS = UpdatedTiers.SOLDE_TIERS;
                        await GetInstance().UpdateAsync(Tournnee);
                    }
                }
            }
            catch (Exception ex)
            {
                await UserDialogs.Instance.AlertAsync(ex.Message, AppResources.alrt_msg_Alert, AppResources.alrt_msg_Ok);
            }


        }

        private static async Task<decimal> GetSoldTiers(string codeTiers)
        {
            List<View_TRS_TIERS> Tiers = await GetInstance().Table<View_TRS_TIERS>().ToListAsync();
            var UpdatedTiers = Tiers.Where(x => x.CODE_TIERS == codeTiers).FirstOrDefault();
            return UpdatedTiers.SOLDE_TIERS;
        }

        public static async Task AjoutTiers(View_TRS_TIERS tiers)
        {
            try
            {
                if (!string.IsNullOrEmpty(App.PrefixCodification))
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
                else
                {
                    await UserDialogs.Instance.AlertAsync("Veuillez configurer votre prefix", AppResources.alrt_msg_Alert, AppResources.alrt_msg_Ok);

                }
            }
            catch (Exception ex)
            {
                await UserDialogs.Instance.AlertAsync(ex.Message, AppResources.alrt_msg_Alert, AppResources.alrt_msg_Ok);
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

        #endregion

        #region Tournée

        /// <summary>
        /// Mise à jour du l'etat de couleur et l'etat de l'itemTourné ...
        /// </summary>
        /// <param name="selectedItem"></param>
        /// <returns></returns>
        internal static async Task<int> UpdateTourneeItemVisited(View_LIV_TOURNEE_DETAIL selectedItem)
        {
            selectedItem.CODE_ETAT_VISITE = TourneeStatus.Visited;
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
                string codeVendeur = App.User.UserName;
                paramLivTournee = "CodeVendeur=" + codeVendeur;
                TourneeMethodName = "GetTourneeParVendeur";
                await SyncData<View_LIV_TOURNEE, LIV_TOURNEE>(false, paramLivTournee, TourneeMethodName);
                var obj = await GetInstance().Table<View_LIV_TOURNEE>().ToListAsync();
                App.CODE_MAGASIN = null;
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
            try
            {
                var obj = await GetInstance().Table<View_LIV_TOURNEE>().ToListAsync();
                if (obj != null && obj.Count > 0)
                {
                    CodeTournee = obj[0].CODE_TOURNEE;
                    paramLivTourneeDetail = "CodeTournee=" + CodeTournee;
                    TourneeDetailMethodName = "GetDetailTournee";
                    await SyncData<View_LIV_TOURNEE_DETAIL, LIV_TOURNEE_DETAIL>(false, paramLivTourneeDetail, TourneeDetailMethodName);
                }
            }
            catch (Exception)
            {

                throw;
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
                var tiersFiltred = allTiers.Where(x => x.FULL_NOM_TIERS.ToUpper().Contains(search.ToUpper())).ToList();
                return tiersFiltred;
            }
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

        public static async Task SyncProduitType()
        {
            try
            {
                var itemsC = await WebServiceClient.GetProduitTypes();
                List<BSE_PRODUIT_TYPE> itemsTypes = new List<BSE_PRODUIT_TYPE>();
                foreach (var item in itemsC)
                {
                    itemsTypes.Add(new BSE_PRODUIT_TYPE()
                    {
                        CODE_TYPE = item.CODE_TYPE,
                        DESIGNATION_TYPE = item.DESIGNATION_TYPE
                    });
                }
                var id = await GetInstance().InsertAllAsync(itemsTypes);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public static async Task<List<BSE_PRODUIT_TYPE>> GetProduitType()
        {
            try
            {
                List<BSE_PRODUIT_TYPE> listeItemsType = await GetInstance().Table<BSE_PRODUIT_TYPE>().ToListAsync();
                return listeItemsType;
            }
            catch (Exception ex)
            {

                throw ex;
            }
        }
        public static async Task SyncProduitFamille()
        {
            try
            {
                var itemsC = await WebServiceClient.GetProduitFamilles();
                List<BSE_PRODUIT_FAMILLE> itemsFamilles = new List<BSE_PRODUIT_FAMILLE>();
                foreach (var item in itemsC)
                {
                    itemsFamilles.Add(new BSE_PRODUIT_FAMILLE()
                    {
                        CODE = item.CODE,
                        DESIGNATION = item.DESIGNATION
                    });
                }
                var id = await GetInstance().InsertAllAsync(itemsFamilles);

            }
            catch (Exception ex)
            {

                throw ex;
            }
        }
        public static async Task<List<BSE_PRODUIT_FAMILLE>> GetProduitFamilles()
        {
            try
            {
                List<BSE_PRODUIT_FAMILLE> listeProduitsFamille = await GetInstance().Table<BSE_PRODUIT_FAMILLE>().ToListAsync();
                return listeProduitsFamille;
            }
            catch (Exception ex)
            {

                throw ex;
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
                    item.CODE_ETAT_VISITE = TourneeStatus.Delivered;
                    item.SOLDE_TIERS = await GetSoldTiers(item.CODE_TIERS);
                    item.GPS_LATITUDE = vente.GPS_LATITUDE;
                    item.GPS_LONGITUDE = vente.GPS_LATITUDE;
                    await GetInstance().UpdateAsync(item);
                }
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


        public static async Task SyncLabos()
        {
            try
            {
                await SyncData<BSE_PRODUIT_LABO, BSE_PRODUIT_LABO>();
                var listeLabos = await GetInstance().Table<BSE_PRODUIT_LABO>().ToListAsync();
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public static async Task<List<BSE_PRODUIT_LABO>> GetLabos()
        {
            try
            {
                var listLabos = await GetInstance().Table<BSE_PRODUIT_LABO>().ToListAsync();
                return listLabos;
            }
            catch (Exception ex)
            {
                throw ex;
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

                // check if Details is empty ( in case of Pharm or Comm)
                if (vente.Details != null)
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
                // check if DetailsDistrib is empty or not ( in case of Ditribution using different viewModel )
                else if (vente.DetailsDistrib != null)
                {
                    foreach (var items in vente.DetailsDistrib)
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
        }

        public static async Task<List<View_STK_STOCK>> SelectByCodeBarreLot(string cb_prod, string codeMagasin)
        {
            List<View_STK_STOCK> Products = await GetInstance().Table<View_STK_STOCK>().ToListAsync();
            List<View_STK_STOCK> Produit = Products.Where(e => e.CODE_BARRE == cb_prod).Where(e => e.CODE_MAGASIN == codeMagasin).ToList();
            return Produit;
        }

        public static async Task<List<View_STK_PRODUITS>> GetProductByBarCode(string codeBar)
        {
            try
            {
                var produitsList = await GetInstance().Table<View_STK_PRODUITS>().ToListAsync();
                var produit = produitsList.Where(element => element.CODE_BARRE.Equals(codeBar)).ToList();
                return produit;
            }
            catch (Exception ex)
            {

                throw ex;
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
        /// Insert Transfers dans la table Transfers
        /// </summary>
        /// <returns></returns>
        public static async Task SyncTransfers()
        {
            var codeMagasin = App.CODE_MAGASIN;
            var paremetre = "codeMagasin=" + codeMagasin;
            var TransferMethodName = "GetInvaldiateTransfertByMagasin";
            await SyncData<View_STK_TRANSFERT, STK_TRANSFERT>(false, paremetre, TransferMethodName);
        }


        /// <summary>
        /// Recuperer l'entete d'un transfert
        /// </summary>
        /// <returns></returns>
        public static async Task<View_STK_TRANSFERT> GetTransfertHeader(string codeTransfert)
        {
            var transferDetails = await GetInstance().Table<View_STK_TRANSFERT>().ToListAsync();

            var transfer = from e in transferDetails
                           where e.CODE_TRANSFERT == codeTransfert
                           select e;
            return transfer.First();
        }


        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public static async Task SyncTransfersDetail()
        {
            var codeMagasin = App.CODE_MAGASIN;
            var paramTransferDetail = "codeMagasin=" + codeMagasin;
            var transfertDetailMethodName = "GetDetailsTransferts";
            await SyncData<View_STK_TRANSFERT_DETAIL, STK_TRANSFERT_DETAIL>(false, paramTransferDetail, transfertDetailMethodName);
        }

        /// <summary>
        /// Recuperer la detail d'un transfert
        /// </summary>
        /// <returns></returns>
        public static async Task<List<View_STK_TRANSFERT_DETAIL>> GetTransfertDetail(string codeTransfert)
        {
            var transferDetails = await GetInstance().Table<View_STK_TRANSFERT_DETAIL>().ToListAsync();

            var transfer = from e in transferDetails
                           where e.CODE_TRANSFERT == codeTransfert
                           select e;
            return transfer.ToList();
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

        /// <summary>
        /// Obtenir la liste des permissions de la table sys_objet_permission
        /// </summary>
        /// <returns></returns>
        public static async Task<List<SYS_OBJET_PERMISSION>> getPermission()
        {
            try
            {
                var permission = await GetInstance().Table<SYS_OBJET_PERMISSION>().ToListAsync();
                return permission;
            }
            catch (Exception ex)
            {
                throw ex;
            }

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

        public static async Task SyncSysParams()
        {
            var Params = await CrudManager.SysParams.GetParams();
            await GetInstance().DeleteAllAsync<SYS_MOBILE_PARAMETRE>();
            var id = await GetInstance().InsertAsync(Params);
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

        /// <summary>
        /// Synchronisation des Status de commande 
        /// </summary>
        /// <returns></returns>
        public static async Task SyncStatusCommande()
        {
            try
            {
                var itemsS = await WebServiceClient.GetStatusCommande();
                if (itemsS != null)
                    await GetInstance().InsertAllAsync(itemsS);
                var list = await GetInstance().Table<View_TRS_TIERS>().ToListAsync();// testing only 
            }
            catch (Exception ex)
            {

                throw ex;
            }
        }
        /// <summary>
        /// Obtenire une vente par CODE_VENTE
        /// </summary>
        /// <param name="CodeVente"></param>
        /// <returns></returns>
        public static async Task<View_VTE_VENTE> GetVente(string CodeVente)
        {
            try
            {
                var exception = SQLite_Manager.GetInstance().Table<View_VTE_VENTE>().ToListAsync().Exception;
                if (exception == null)
                {

                    var ventes = await GetInstance().Table<View_VTE_VENTE>().ToListAsync();
                    var res = ventes.Where(e => e.CODE_VENTE == CodeVente).FirstOrDefault();
                    return res;
                }
                else throw exception;
            }
            catch (Exception ex)
            {
                await UserDialogs.Instance.AlertAsync(WSApi2.GetExceptionMessage(ex), AppResources.alrt_msg_Alert, AppResources.alrt_msg_Ok);
                return null;
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
        /// <summary>
        /// Obtenire la liste des details de vente par CODE_VENTE
        /// </summary>
        /// <param name="CodeVente"></param>
        /// <returns></returns>
        public static async Task<List<View_VTE_VENTE_LIVRAISON>> getVenteDetailsDistrib(string CodeVente)
        {
            List<View_VTE_VENTE_LIVRAISON> ventes = await GetInstance().Table<View_VTE_VENTE_LIVRAISON>().ToListAsync();
            List<View_VTE_VENTE_LIVRAISON> VenteDetail = ventes.Where(e => e.CODE_VENTE == CodeVente).ToList();
            return VenteDetail;
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
                var vteDetailsDistrib = await GetInstance().Table<View_VTE_VENTE_LIVRAISON>().ToListAsync();

                if (ListVentes.Count > 0 && ListVentes != null)
                {
                    foreach (var iVente in ListVentes)
                    {
                        List<View_VTE_VENTE_LOT> objdetail = new List<View_VTE_VENTE_LOT>();
                        List<View_VTE_VENTE_LIVRAISON> objdetailDistrib = new List<View_VTE_VENTE_LIVRAISON>();
                        try
                        {
                            if (vteDetails.Count > 0)
                                objdetail = vteDetails?.Where(x => x.CODE_VENTE == iVente.CODE_VENTE)?.ToList();
                            else
                                objdetailDistrib = vteDetailsDistrib?.Where(x => x.CODE_VENTE == iVente.CODE_VENTE)?.ToList();
                        }
                        catch
                        {
                            objdetail = null;
                        }
                        finally
                        {
                            if (objdetail.Count > 0)
                                iVente.Details = objdetail;
                            else if (objdetailDistrib.Count > 0)
                            {
                                iVente.Details = new List<View_VTE_VENTE_LOT>();
                                foreach (var v in objdetailDistrib)
                                {

                                    iVente.Details.Add(XpertHelper.CloneObject<View_VTE_VENTE_LOT>(v));
                                }
                            }
                        }
                    }

                    var compte = await getComptes();

                    var bll = CrudManager.GetVteBll(VentesTypes.Livraison);
                    var res = await bll.SyncVentes(ListVentes, App.PrefixCodification, App.CODE_MAGASIN, compte.FirstOrDefault().CODE_COMPTE);

                    await GetInstance().DeleteAllAsync<View_VTE_VENTE>();
                    await GetInstance().DeleteAllAsync<View_VTE_VENTE_LOT>();
                    await GetInstance().DeleteAllAsync<View_VTE_VENTE_LIVRAISON>();

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
        /// Synchronisation de la liste des Tournees aux serveur
        /// </summary>
        /// <returns></returns>
        public static async Task<string> SyncTourneesToServer()
        {
            try
            {
                UserDialogs.Instance.ShowLoading(AppResources.txt_Waiting);

                var ListTorunee = await GetInstance().Table<View_LIV_TOURNEE>().ToListAsync();
                var tourneeDetails = await GetInstance().Table<View_LIV_TOURNEE_DETAIL>().ToListAsync();

                if (ListTorunee.Count > 0 && ListTorunee != null)
                {
                    foreach (var iTournee in ListTorunee)
                    {
                        var details = tourneeDetails.Where(e => e.CODE_TOURNEE == iTournee.CODE_TOURNEE).ToList();
                        if (details.Count() > 0 && details != null)
                            iTournee.Details = details;
                    }

                    var compte = await getComptes();

                    var bll = CrudManager.Tournee;
                    var res = await bll.SyncTournee(ListTorunee);

                    await GetInstance().DeleteAllAsync<View_LIV_TOURNEE>();
                    await GetInstance().DeleteAllAsync<View_LIV_TOURNEE_DETAIL>();

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
                if (string.IsNullOrEmpty(vente.CODE_VENTE))
                {
                    var encaissement = new View_TRS_ENCAISS();
                    var listeTiers = await GetInstance().Table<View_TRS_TIERS>().ToListAsync();
                    //var obj = await GetInstance().Table<View_VTE_VENTE>().ToListAsync();
                    vente.TOTAL_PAYE = vente.MT_VERSEMENT = vente.TOTAL_RECU;
                    vente.TOTAL_RESTE = vente.TOTAL_TTC - vente.TOTAL_PAYE;
                    vente.CREATED_BY = App.User.UserName;
                    var id = await GetInstance().InsertAsync(vente);
                    //vente.CODE_VENTE = vente.ID.ToString() + "/" + App.PrefixCodification;
                    //vente.NUM_VENTE = vente.CODE_VENTE;
                    vente.NOM_TIERS = listeTiers.Where(x => x.CODE_TIERS == vente.CODE_TIERS).FirstOrDefault()?.NOM_TIERS1;
                    vente.CODE_VENTE = await generateCode(vente.TYPE_DOC, vente.ID.ToString());
                    vente.NUM_VENTE = await generateNum(vente.TYPE_DOC, vente.ID.ToString());
                    vente.DATE_VENTE = DateTime.Now;
                    await GetInstance().UpdateAsync(vente);

                    //check if vente.Detail is empty than use 

                    if (vente.Details != null)
                        foreach (var item in vente.Details)
                        {
                            item.CODE_VENTE = vente.CODE_VENTE;
                        }
                    else if (vente.DetailsDistrib != null)
                        foreach (var item in vente.DetailsDistrib)
                        {
                            item.CODE_VENTE = vente.CODE_VENTE;

                            // mise a jour la quantite dans la BL on ajout la quantite d'unites des mesures a la quantite

                            item.QUANTITE += Manager.TotalQuantiteUnite(item.UnitesList);
                        }
                    if (vente.Details != null)
                    {
                        var id2 = await GetInstance().InsertAllAsync(vente.Details);
                    }
                    else if (vente.DetailsDistrib != null)
                    {
                        var id2 = await GetInstance().InsertAllAsync(vente.DetailsDistrib);
                    }

                    await UpdateStock(vente);
                    if (vente.TOTAL_TTC != vente.MT_VERSEMENT)
                    {
                        decimal sold = vente.TOTAL_TTC - vente.MT_VERSEMENT;
                        await UpdateSoldeTiers(sold, vente.CODE_TIERS);
                    }

                    encaissement.CODE_TYPE = "ENC";
                    encaissement.CODE_TIERS = vente.CODE_TIERS;
                    encaissement.TOTAL_ENCAISS = vente.TOTAL_RECU;
                    encaissement.CODE_TOURNEE = vente.CODE_TOURNEE;
                    encaissement.CODE_MOTIF = "PCR";
                    //encaissement.CODE_COMPTE= 
                    encaissement.DATE_ENCAISS = DateTime.Now;
                    encaissement.IS_SYNCHRONISABLE = false;
                    await AjoutEncaissement(encaissement, null);

                    await UpdateTourneeDetail(vente);
                    await UpdateTournee();

                    return vente.CODE_VENTE;
                }
                else
                {
                    if (vente.Details != null)
                        foreach (var item in vente.Details)
                        {
                            item.CODE_VENTE = vente.CODE_VENTE;
                        }
                    else if (vente.DetailsDistrib != null)
                        foreach (var item in vente.DetailsDistrib)
                        {
                            item.CODE_VENTE = vente.CODE_VENTE;

                            // mise a jour la quantite dans la BL on ajout la quantite d'unites des mesures a la quantite

                            item.QUANTITE += Manager.TotalQuantiteUnite(item.UnitesList);
                        }
                    if (vente.Details != null)
                    {
                        var id2 = await GetInstance().UpdateAllAsync(vente.Details);
                    }
                    else if (vente.DetailsDistrib != null)
                    {
                        var id2 = await GetInstance().UpdateAllAsync(vente.DetailsDistrib);
                    }

                    var encaiss = await GetInstance().Table<View_TRS_ENCAISS>().ToListAsync();
                    var encaissElement = encaiss.Where(e => e.CODE_TOURNEE == vente.CODE_TOURNEE && e.CODE_TIERS == vente.CODE_TIERS).FirstOrDefault();
                    encaissElement.TOTAL_ENCAISS = vente.TOTAL_RECU;
                    encaissElement.DATE_ENCAISS = DateTime.Now;


                    await GetInstance().UpdateAsync(vente);
                    await GetInstance().UpdateAsync(encaissElement);


                    return vente.CODE_VENTE;
                }
            }
        }

        public static async Task AjoutCommande(View_VTE_COMMANDE item)
        {
            try
            {
                //Test si le prefix est configurer ou non...
                if (!string.IsNullOrEmpty(App.PrefixCodification))
                {
                    var listeTiers = await GetInstance().Table<View_TRS_TIERS>().ToListAsync();

                    item.NOM_TIERS = listeTiers.Where(x => x.CODE_TIERS == item.CODE_TIERS).FirstOrDefault()?.NOM_TIERS1;
                    item.CREATED_BY = App.User.UserName;
                    item.CODE_VENTE = await generateCode(item.TYPE_DOC, item.ID.ToString());
                    await GetInstance().InsertAsync(item);
                }
                else
                {
                    await UserDialogs.Instance.AlertAsync("Veuillez configurer le Prefix", AppResources.alrt_msg_Alert, AppResources.alrt_msg_Ok);
                }

            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        #endregion


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
            if (prixProduct != null)
            {
                if (Math.Abs(qteVendu) >= prixProduct.QTE_VENTE)
                {
                    return prixProduct.PRIX_GROS;
                }
                else
                {
                    return prixProduct.PRIX_DETAIL;
                }
            }
            else
            {
                return 100;
            }

        }

        public static async Task<SYS_MOBILE_PARAMETRE> getParams()
        {
            SYS_MOBILE_PARAMETRE Params = await GetInstance().Table<SYS_MOBILE_PARAMETRE>().FirstOrDefaultAsync();
            return Params;
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
            try
            {
                var prefix = await GetInstance().Table<SYS_CONFIGURATION_MACHINE>().FirstOrDefaultAsync();
                if (prefix != null)
                {
                    if (!(string.IsNullOrEmpty(prefix.PREFIX)))
                    {
                        App.PrefixCodification = prefix.PREFIX;
                    }
                    else
                    {
                        await UserDialogs.Instance.AlertAsync("Veuillez configurer votre prefix", AppResources.alrt_msg_Alert, AppResources.alrt_msg_Ok);
                    }
                }

                else
                {
                    await UserDialogs.Instance.AlertAsync("Veuillez configurer votre prefix", AppResources.alrt_msg_Alert, AppResources.alrt_msg_Ok);
                }
            }
            catch (Exception)
            {

                await UserDialogs.Instance.AlertAsync("Veuillez configurer votre prefix", AppResources.alrt_msg_Alert, AppResources.alrt_msg_Ok);
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

    }
}
