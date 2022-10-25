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
           return  UpdateDatabase.getInstance();
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


        public static async Task SyncTiersToServer()
        {
            try
            {
                //UserDialogs.Instance.ShowLoading(AppResources.txt_Waiting);
                var Tiers = await getInstance().Table<View_TRS_TIERS>().ToListAsync();
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
                //UserDialogs.Instance.HideLoading();
            }
            catch (Exception ex)
            {
                UserDialogs.Instance.HideLoading();
                await UserDialogs.Instance.AlertAsync(WSApi2.GetExceptionMessage(ex), AppResources.alrt_msg_Alert, AppResources.alrt_msg_Ok);
                //await UserDialogs.Instance.AlertAsync("erreur de synchronisation des Tiers!!", AppResources.alrt_msg_Alert, AppResources.alrt_msg_Ok);
            }
        }

        public static async Task SyncEncaissToServer()
        {
            try
            {
                //UserDialogs.Instance.ShowLoading(AppResources.txt_Waiting);
                var encaiss = await getInstance().Table<View_TRS_ENCAISS>().ToListAsync();
                if (encaiss.Count > 0 && encaiss != null)
                {
                    var bll = new EncaissManager();
                    var res = await bll.SyncEncaiss(encaiss);
                    await getInstance().DeleteAllAsync<View_TRS_ENCAISS>();
                }
                //UserDialogs.Instance.HideLoading();
            }
            catch (Exception ex)
            {
                UserDialogs.Instance.HideLoading();
                await UserDialogs.Instance.AlertAsync(WSApi2.GetExceptionMessage(ex), AppResources.alrt_msg_Alert, AppResources.alrt_msg_Ok);
                //await UserDialogs.Instance.AlertAsync("erreur de synchronisation des Encaissements !!", AppResources.alrt_msg_Alert, AppResources.alrt_msg_Ok);
            }
        }


        public static async Task<string> SyncVenteToServer()
        {
            try
            {
                //UserDialogs.Instance.ShowLoading(AppResources.txt_Waiting);

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

                    await getInstance().DeleteAllAsync<View_VTE_VENTE>();
                    await getInstance().DeleteAllAsync<View_VTE_VENTE_LOT>();

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

        public static async Task<string> SyncRetourStock()
        {
            try
            {
                var ListEntrees = await getInstance().Table<View_STK_ENTREE>().ToListAsync();
                var EntreesDetails = await getInstance().Table<View_STK_ENTREE_DETAIL>().ToListAsync();
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
                await getInstance().DeleteAllAsync<View_STK_ENTREE>();
                await getInstance().DeleteAllAsync<View_STK_ENTREE_DETAIL>();
                return res;
            }
            catch (Exception ex)
            {
                UserDialogs.Instance.HideLoading();
                await UserDialogs.Instance.AlertAsync(WSApi2.GetExceptionMessage(ex), AppResources.alrt_msg_Alert, AppResources.alrt_msg_Ok);
                //await UserDialogs.Instance.AlertAsync("Erreur de synchronisation des ventes!!", AppResources.alrt_msg_Alert, AppResources.alrt_msg_Ok);
            }
            return string.Empty;
        }

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

            //using (IProgressDialog progress = UserDialogs.Instance.Progress("Progress", null, null, true, MaskType.Black))
            //{
            //    progress.PercentComplete = 0;

            //    //5%
            //    progress.PercentComplete = 5;

            //    //30
            //    progress.PercentComplete = 35;

            //    //25
            //    progress.PercentComplete = 60;


            //    //5%
                
            //    progress.PercentComplete = 65;

            //    //15%
            //    progress.PercentComplete = 80;

            //    //5%
               
            //    progress.PercentComplete = 85;

            //    //15%
                
            //    progress.PercentComplete = 100;

            //}



            bool isconnected = await App.IsConected();
            if (isconnected)
            {
                //add try 
                UserDialogs.Instance.ShowLoading(AppResources.txt_Waiting);
                await UpdateDatabase.initialisationDbLocal();



                await SyncProductAutoriserVente();
                await UpdateDatabase.SyncData<View_TRS_TIERS, TRS_TIERS>();

                await SyncLivTournee();
                await SyncLivTourneeDetail();
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
