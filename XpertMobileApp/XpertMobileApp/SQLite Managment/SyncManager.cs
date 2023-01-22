using Acr.UserDialogs;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using Xpert.Common.WSClient.Helpers;
using XpertMobileApp;
using XpertMobileApp.Api;
using XpertMobileApp.Api.Managers;
using XpertMobileApp.DAL;
using XpertMobileApp.SQLite_Managment;
using XpertMobileApp.ViewModels;
using System.Linq;
using XpertMobileApp.Models;
using SQLite;
using XpertMobileApp.Api.Services;

namespace XpertMobileAppManafiaa.SQLite_Managment
{
    internal class SyncManager
    {
        static string codeVendeur;
        static string CodeTournee;
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

        private static SQLiteAsyncConnection getInstance()
        {
            return UpdateDatabase.getInstance();
        }

        #region send data from mobile to server

        public static async Task synchroniseUpload()
        {
            bool isconnected = await App.IsConected();
            if (isconnected)
            {
                UserDialogs.Instance.ShowLoading(AppResources.txt_Waiting);

                await ClotureTournee();
                await SyncTiersToServer();
                await SyncEncaissToServer();
                await SyncVenteToServer();
                await SyncRetourStock();
                UserDialogs.Instance.HideLoading();
                await UserDialogs.Instance.AlertAsync("Synchronisation faite avec succes", AppResources.alrt_msg_Alert, AppResources.alrt_msg_Ok);
            }
            else
            {
                UserDialogs.Instance.HideLoading();
                await UserDialogs.Instance.AlertAsync("Veuillez verifier votre connexion au serveur ! ", AppResources.alrt_msg_Alert, AppResources.alrt_msg_Ok);
            }
        }
        /* begin section of Synchronisation tiers */

        /// <summary>
        /// Uploader la liste des tiers aux serveur
        /// </summary>
        /// <returns></returns>
        public static async Task<bool> SyncTiersToServer()
        {
            try
            {
                List<View_TRS_TIERS> listNewTiers = await getInstance().Table<View_TRS_TIERS>().Where(item => item.ETAT_TIERS == STAT_TIERS_MOBILE.ADDED || item.ETAT_TIERS == STAT_TIERS_MOBILE.UPDATED).ToListAsync();

                if (listNewTiers.Count > 0 && listNewTiers != null)
                {
                    var bll = new TiersManager();
                    var res = await bll.SyncTiers(listNewTiers);
                    if (!string.IsNullOrEmpty(res))
                    {
                        if (await UpdateLogs("TIERS"))
                        {
                            return true;
                        }
                        return false;
                    }
                    return false;
                }
                return true;
            }
            catch (Exception ex)
            {
                UserDialogs.Instance.HideLoading();
                ex.Data["opeartion"] = "TIERS";
                throw ex;
            }
        }
        /// <summary>
        /// Delete all the clients (tiers) which was stored in sqlite ... 
        /// </summary>
        public async static void DeleteAllTiersSInQLite()
        {
            try
            {
                await getInstance().DeleteAllAsync<View_TRS_TIERS>();
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        /* End section of Synchronisation tiers */



        /// <summary>
        /// Update all logs 
        /// </summary>
        /// <param name="operation"></param>
        /// <returns></returns>
        public async static Task<bool> UpdateLogs(string operation)
        {
            try
            {
                var tournee = await getInstance().Table<View_LIV_TOURNEE>().ToListAsync();
                var listCodeTournee = tournee.Select(x => x.CODE_TOURNEE).ToArray();
                var logListe = await getInstance().Table<LOG_SYNCHRONISATION>().Where(element => listCodeTournee.Contains(element.CODE_TOURNEE)).ToListAsync();

                bool allRowChecked = false;
                foreach (var log in logListe)
                {
                    allRowChecked = await UpdateLog(log, operation);
                    if (!allRowChecked)
                        return false;
                }
                return allRowChecked;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        /// <summary>
        /// Update One Log with datetime.now
        /// </summary>
        /// <param name="log"></param>
        /// <param name="operation"></param>
        /// <returns></returns>
        public static async Task<bool> UpdateLog(LOG_SYNCHRONISATION log, string operation)
        {
            try
            {
                switch (operation)
                {
                    case "COMMANDE":
                        log.SYNC_COMMANDE = DateTime.Now;
                        break;
                    case "VENTE":
                        log.SYNC_VENTE = DateTime.Now;
                        break;
                    case "TOURNEE":
                        log.SYNC_TOURNEE = DateTime.Now;
                        break;
                    case "TIERS":
                        log.SYNC_TIERS = DateTime.Now;
                        break;
                    case "ENCAISS":
                        log.SYNC_ENCAISS = DateTime.Now;
                        break;
                    default:
                        return false;
                }

                await getInstance().UpdateAsync(log);
                var tourneeDetails = await getInstance().Table<LOG_SYNCHRONISATION>().ToListAsync();
                return true;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }



        /* Begin Section of Synchronisation Encaiss */

        /// <summary>
        /// Synchronisation de la liste des encaiss aux serveur 
        /// </summary>
        /// <returns></returns>
        public static async Task<bool> SyncEncaissToServer()
        {
            try
            {
                var encaissList = await getInstance().Table<View_TRS_ENCAISS>().Where(e => e.IS_SYNCHRONISABLE == true).ToListAsync();
                if (encaissList.Count() > 0 && encaissList != null)
                {
                    var bll = new EncaissManager();
                    var res = await bll.SyncEncaiss(encaissList);
                    if (!string.IsNullOrEmpty(res))
                    {
                        if (await UpdateLogs("ENCAISS"))
                        {
                            return true;
                        }
                        return false;
                    }
                    return false;
                }
                return true;
            }
            catch (Exception ex)
            {
                UserDialogs.Instance.HideLoading();
                ex.Data["opeartion"] = "ENCAISS";
                throw ex;
            }
        }

        /// <summary>
        /// Delete all the encaiss which was stored in  sqlite
        /// </summary>
        public static async void DeleteAllEncaissInSQlite()
        {
            try
            {
                await getInstance().DeleteAllAsync<View_TRS_ENCAISS>();
            }
            catch (Exception ex)
            {

                throw ex;
            }
        }

        /* End Section of Synchronisation Encaiss*/


        /* Begin section of Synchronisation VENTES */

        /// <summary>
        /// Delete all the VENTES Which is stored in sqlite 
        /// </summary>
        /// /// <summary>
        /// Synchronisation de la liste des ventes aux serveur
        /// </summary>
        /// <returns></returns>
        public static async Task<bool> SyncVenteToServer()
        {
            try
            {


                var ListVentes = await getInstance().Table<View_VTE_VENTE>().ToListAsync();
                var vteDetails = await getInstance().Table<View_VTE_VENTE_LOT>().ToListAsync();

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

                    string CodeCompteUser = App.User.CODE_COMPTE;
                    if (string.IsNullOrEmpty(App.CODE_MAGASIN))
                    {
                        View_LIV_TOURNEE Tournee = await getInstance().Table<View_LIV_TOURNEE>().FirstOrDefaultAsync();
                        if (Tournee != null)
                        {
                            App.CODE_MAGASIN = Tournee.CODE_MAGASIN;
                        }
                    }
                    var bll = CrudManager.GetVteBll(VentesTypes.Livraison);
                    var res = await bll.SyncVentes(ListVentes, App.PrefixCodification, App.CODE_MAGASIN, CodeCompteUser);

                    if (!string.IsNullOrEmpty(res))
                    {
                        if (await UpdateLogs("VENTE"))
                        {
                            return true;
                        }
                        return false;
                    }
                    return true;

                    // Commenter cette section a cause de donnes utilisé avec les commandes et vider les tableauxs apres la synchronisation
                    // des commandes
                }
                return true;
            }
            catch (Exception ex)
            {
                UserDialogs.Instance.HideLoading();
                ex.Data["opeartion"] = "VENTES";
                throw ex;
            }
            return false;
        }
        public static async void DeleteAllVentesInSQLite()
        {
            try
            {
                await getInstance().DeleteAllAsync<View_VTE_VENTE>();
                await getInstance().DeleteAllAsync<View_VTE_VENTE_LOT>();
                //await getInstance().DeleteAllAsync<View_VTE_VENTE_LIVRAISON>();
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        /* End section of Synchronisation VENTES */





        /* begin  section of Synchronisation retour stock */

        public static async Task<bool> SyncRetourStock()
        {
            try
            {
                var ListEntrees = await getInstance().Table<View_STK_ENTREE>().ToListAsync();
                var EntreesDetails = await getInstance().Table<View_STK_ENTREE_DETAIL>().ToListAsync();
                if (ListEntrees.Count > 0 && ListEntrees != null)
                {

                    foreach (var item in ListEntrees)
                    {
                        List<View_STK_ENTREE_DETAIL> objdetail = new List<View_STK_ENTREE_DETAIL>();
                        try
                        {


                            objdetail = EntreesDetails?.Where(x => x.CODE_ENTREE == item.CODE_ENTREE)?.ToList();
                        }
                        catch
                        {
                            objdetail = null;
                        }
                        finally
                        {
                            item.Details = objdetail;
                        }

                    }
                    if (string.IsNullOrEmpty(App.CODE_MAGASIN))
                    {
                        View_LIV_TOURNEE Tournee = await getInstance().Table<View_LIV_TOURNEE>().FirstOrDefaultAsync();
                        if (Tournee != null)
                        {
                            App.CODE_MAGASIN = Tournee.CODE_MAGASIN;
                        }
                    }

                    var res = await CrudManager.Stock.SyncRetourStock(ListEntrees, App.CODE_MAGASIN);
                    if (!string.IsNullOrEmpty(res))
                    {
                        if (await UpdateLogs("VENTE"))
                        {
                            return true;
                        }
                        return false;
                    }
                }


                return true;



            }
            catch (Exception ex)
            {
                SyncManager.DeleteAllretourStockInSQLite();

                UserDialogs.Instance.HideLoading();
                await UserDialogs.Instance.AlertAsync(WSApi2.GetExceptionMessage(ex), AppResources.alrt_msg_Alert, AppResources.alrt_msg_Ok);
                return false;
            }
        }
        public static async void DeleteAllretourStockInSQLite()
        {
            try
            {
                await getInstance().DeleteAllAsync<View_STK_ENTREE>();
                await getInstance().DeleteAllAsync<View_STK_ENTREE_DETAIL>();
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }


        /* End section of Synchronisation retour stock  */




        /* begin  section of Synchronisation TOURNEE */

        /// <summary>
        /// Synchronisation de la liste des Tournees aux serveur
        /// </summary>
        /// <returns></returns>
        public static async Task<bool> SyncTourneesToServer()
        {
            try
            {

                //var ListTorunee = await getInstance().Table<View_LIV_TOURNEE>().ToListAsync();
                //var tourneeDetails = await getInstance().Table<View_LIV_TOURNEE_DETAIL>().ToListAsync();

                //if (ListTorunee.Count > 0 && ListTorunee != null)
                //{
                //    foreach (var iTournee in ListTorunee)
                //    {
                //        var details = tourneeDetails.Where(e => e.CODE_TOURNEE == iTournee.CODE_TOURNEE).ToList();
                //        if (details.Count() > 0 && details != null)
                //            iTournee.Details = details;
                //    }

                //    var compte = await getComptes();

                //    var bll = CrudManager.Tournee;
                //    var res = await bll.SyncTournee(ListTorunee);
                //    if (res)
                //    {
                //        if (await UpdateLogs("COMMANDE"))
                //        {
                //            return true;
                //        }
                //        return false;
                //    }
                //    return false;
                //}
                return true;
            }

            catch (Exception ex)
            {
                UserDialogs.Instance.HideLoading();
                ex.Data["opeartion"] = "TOURNEE";
                throw ex;
            }
        }


        public async static void DeleteAllTourneeInSQLite()
        {
            try
            {
                await getInstance().DeleteAllAsync<View_LIV_TOURNEE>();
                await getInstance().DeleteAllAsync<View_LIV_TOURNEE_DETAIL>();
            }
            catch (Exception ex)
            {

                throw ex;
            }
        }


        /* End Section of synchronisation TOURNEE */
        public static async Task<View_LIV_TOURNEE> ClotureTournee()
        {

            View_LIV_TOURNEE Tournne = await getInstance().Table<View_LIV_TOURNEE>().FirstOrDefaultAsync();
            List<View_VTE_VENTE> ALLVentes = await getInstance().Table<View_VTE_VENTE>().ToListAsync();
            List<View_TRS_ENCAISS> ALLEncaissements = await getInstance().Table<View_TRS_ENCAISS>().ToListAsync();
            List<View_STK_ENTREE> ALLRetourStock = await getInstance().Table<View_STK_ENTREE>().ToListAsync();
            List<View_TRS_TIERS> Tiers = await getInstance().Table<View_TRS_TIERS>().ToListAsync();
            List<View_STK_STOCK> stock = await getInstance().Table<View_STK_STOCK>().ToListAsync();


            Tournne.TOTAL_CREDIT_TIERS = (decimal)Tiers?.Where(x => x.SOLDE_TIERS > 0)?.Sum(x => x.SOLDE_TIERS);
            Tournne.Total_Credit_Journee = (decimal)ALLVentes?.Sum(x => x.TOTAL_RESTE);
            Tournne.TOTAL_Vente = (decimal)ALLVentes?.Sum(x => x.TOTAL_PAYE);
            Tournne.TOTAL_PaiementCredit = (decimal)(ALLEncaissements?.Sum(x => x.TOTAL_ENCAISS));
            Tournne.TOTAL_RETOUR_STOCK = (decimal)ALLRetourStock?.Sum(x => x.TOTAL_TTC);
            Tournne.TOTAL_STOCK_APRES = stock.Sum(x => x.PRIX_VENTE * x.QUANTITE);
            Tournne.TOTAL_TOURNEE = Tournne.TOTAL_PaiementCredit + Tournne.TOTAL_Vente;
            await getInstance().UpdateAsync(Tournne);
            return Tournne;
        }

        #endregion



        #region send data from server to mobile

        public static async Task synchroniseDownload()
        {

            try
            {
                bool isconnected = await App.IsConected();
                if (isconnected)
                {
                    //add try 
                    UserDialogs.Instance.ShowLoading(AppResources.txt_Waiting);
                    await UpdateDatabase.initialisationDbLocal();
                    await SyncLivTournee();
                    await SyncLivTourneeDetail();


                    await SyncProductAutoriserVente();
                    await UpdateDatabase.SyncData<View_TRS_TIERS, TRS_TIERS>();


                    await SyncStock();
                    await SyncUsers();

                    await syncPermission();
                    await UpdateDatabase.SyncSysParams();
                    await syncSession();
                    await UpdateDatabase.SyncFamille();
                    await UpdateDatabase.SyncTypeTiers();
                    await UpdateDatabase.SyncSecteurs();
                    await UpdateDatabase.SyncBseCompte();
                    await UpdateDatabase.SyncMotifs();
                    await UpdateDatabase.SyncProductPriceByQuantity();
                    await UpdateDatabase.AssignMagasin();

                    UserDialogs.Instance.HideLoading();
                    await UserDialogs.Instance.AlertAsync("Synchronisation faite avec succes", AppResources.alrt_msg_Alert, AppResources.alrt_msg_Ok);
                }
                else
                {
                    UserDialogs.Instance.HideLoading();
                    await UserDialogs.Instance.AlertAsync("Veuillez verifier votre connexion au serveur ! ", AppResources.alrt_msg_Alert, AppResources.alrt_msg_Ok);
                }
            }
            catch (Exception)
            {
                UserDialogs.Instance.HideLoading();
                throw;
            }


        }

        public static async Task SyncLivTournee()
        {
            if (App.User.UserName != null)
            {
                codeVendeur = App.User.UserName;
                paramLivTournee = "CodeVendeur=" + codeVendeur;
                TourneeMethodName = "GetTourneeParVendeur";
                await UpdateDatabase.SyncData<View_LIV_TOURNEE, LIV_TOURNEE>(false, paramLivTournee, TourneeMethodName);
                var obj = await getInstance().Table<View_LIV_TOURNEE>().ToListAsync();
                if (obj != null && obj.Count > 0)
                {
                    App.CODE_MAGASIN = obj[0].CODE_MAGASIN;
                    /* Initialisation de la table Log */
                    LOG_SYNCHRONISATION logItem = new LOG_SYNCHRONISATION();
                    foreach (var item in obj)
                    {
                        logItem.CODE_TOURNEE = item.CODE_TOURNEE;
                        await getInstance().InsertAsync(logItem);
                    }

                }
                else
                {
                    throw new Exception("Veuillez verifier votre tournee");
                }
            }
        }

        public static async Task SyncLivTourneeDetail()
        {
            //View_LIV_TOURNEE obj = await getInstance().Table<View_LIV_TOURNEE>().OrderByDescending(x=>x.DATE_TOURNEE == DateTime.Now).FirstOrDefaultAsync();
            var obj = await getInstance().Table<View_LIV_TOURNEE>().ToListAsync();
            if (obj != null && obj.Count > 0)
            {
                CodeTournee = obj[0].CODE_TOURNEE;
                paramLivTourneeDetail = "CodeTournee=" + CodeTournee;
                TourneeDetailMethodName = "GetDetailTournee";
                await UpdateDatabase.SyncData<View_LIV_TOURNEE_DETAIL, LIV_TOURNEE_DETAIL>(false, paramLivTourneeDetail, TourneeDetailMethodName);

            }
        }

        public static async Task SyncStock()
        {
            var obj = await getInstance().Table<View_LIV_TOURNEE>().ToListAsync();
            if (obj != null && obj.Count > 0)
            {
                CodeMagasin = obj[0].CODE_MAGASIN;
                paramStock = "CodeMagasin=" + CodeMagasin;
                StockMethodName = "GetStockByMagsin";
                await UpdateDatabase.SyncData<View_STK_STOCK, STK_STOCK>(false, paramStock, StockMethodName);
            }
        }



        public static async Task SyncUsers()
        {
            UsersMethodName = "SyncUsers";
            await UpdateDatabase.SyncData<SYS_USER, SYS_USER>(false, string.Empty, UsersMethodName);
        }
        public static async Task SyncProductAutoriserVente()
        {
            var products = await CrudManager.Products.GetProduitAutoriserVente();
            await getInstance().DeleteAllAsync<View_STK_PRODUITS>();
            var id = await getInstance().InsertAllAsync(products);
        }

        public static async Task syncPermission()
        {
            idGroup = App.User.UserGroup;
            paramPermission = "idGroup=" + idGroup;
            PermissionMethodName = "GetPermissions";
            await UpdateDatabase.SyncData<SYS_OBJET_PERMISSION, SYS_OBJET_PERMISSION>(false, paramPermission, PermissionMethodName);
        }

        public static async Task syncSession()
        {
            var session = await CrudManager.Sessions.GetCurrentSession();
            await getInstance().DeleteAllAsync<TRS_JOURNEES>();
            var id = await getInstance().InsertAsync(session);
            //SessionMethodName = "GetCurrentSession";
            //await SyncData<TRS_JOURNEES, TRS_JOURNEES>(false, "", SessionMethodName);
        }

        #endregion
    }
}
