using Acr.UserDialogs;
using SQLite;
using System;
using System.Collections.Generic;
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

namespace XpertMobileApp.SQLite_Managment
{
    class UpdateDatabase
    {
        static SQLiteAsyncConnection db;
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

        public static SQLiteAsyncConnection getInstance()
        {
            if (db == null)
            {
                // Get an absolute path to the database file
                var databasePath = Path.Combine(FileSystem.AppDataDirectory, "MyData.db");
                db = new SQLiteAsyncConnection(databasePath);
                return db;
            }
            else
            {
                return db;
            }
        }
        public static async Task initialisationDbLocal()
        {
            try
            {
                //await getInstance().DropTableAsync<BSE_TABLE_TYPE>();

                //if (!TableExists(db))
                //{
                await getInstance().CreateTableAsync<View_STK_PRODUITS>();
                await getInstance().CreateTableAsync<View_TRS_TIERS>();
                await getInstance().CreateTableAsync<View_LIV_TOURNEE>();
                await getInstance().CreateTableAsync<View_LIV_TOURNEE_DETAIL>();
                await getInstance().CreateTableAsync<View_STK_STOCK>();
                await getInstance().CreateTableAsync<View_VTE_VENTE_LOT>();
                await getInstance().CreateTableAsync<View_VTE_VENTE>();
                await getInstance().CreateTableAsync<SYS_USER>();
                await getInstance().CreateTableAsync<SYS_OBJET_PERMISSION>();
                await getInstance().CreateTableAsync<TRS_JOURNEES>();
                await getInstance().CreateTableAsync<Token>();
                await getInstance().CreateTableAsync<View_BSE_TIERS_FAMILLE>();
                await getInstance().CreateTableAsync<BSE_TABLE_TYPE>();
                await getInstance().CreateTableAsync<BSE_TABLE>();
                await getInstance().CreateTableAsync<SYS_CONFIGURATION_MACHINE>();

            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
            }
            //await synchronise();
            //await db.CreateTableAsync<View_LIV_TOURNEE>();
            //}

        }

        public static async Task<IEnumerable<TView>> getItemsUnsyncronised<TView, TTabel>(bool _selectAll = true, string param = "", string methodName = "")
        {
            ICurdService<TView> service = new CrudService<TView>(App.RestServiceUrl, typeof(TTabel).Name, App.User.Token);
            if (_selectAll)
            {
                return await service.GetItemsAsync();
            }
            else
            {
                //return await service.SelectByPage(,);
                return await service.GetItemsAsyncWithUrl(methodName, param);
            }

        }

        public static async Task SyncData<TView, TTabel>(bool _selectAll = true, string param = "", string methodName = "")
        {
            var ListItems = new List<TView>();

            var items = await getItemsUnsyncronised<TView, TTabel>(_selectAll, param, methodName);
            await getInstance().DeleteAllAsync<TView>();

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
            var id = await getInstance().InsertAllAsync(ListItems);

            ListItems = null;
        }

        internal static async Task<View_TRS_TIERS> SelectScanedTiers(string text)
        {
            var tiers = await getInstance().Table<View_TRS_TIERS>().ToListAsync();
            return tiers?.Find(e=>e.CODE_TIERS.Equals(text));
        }

        internal static async Task<int> UpdateTourneeItemVisited(View_LIV_TOURNEE_DETAIL selectedItem)
        {
            selectedItem.CODE_ETAT = TourneeStatus.Visited;
            selectedItem.ETAT_COLOR = "#FFA500";
            return await getInstance().UpdateAsync(selectedItem);
        }

        public static async Task synchronise()
        {
            UserDialogs.Instance.ShowLoading(AppResources.txt_Waiting);
            bool isconnected =  await App.IsConected();
            if (isconnected)
            {
                await getPrefix();
                await AjoutPrefix();
                await initialisationDbLocal();


                await SyncTiersToServer();


                await SyncData<View_STK_PRODUITS, STK_PRODUITS>();
                await SyncData<View_TRS_TIERS, TRS_TIERS>();
                await SyncLivTournee();
                await SyncLivTourneeDetail();
                await SyncStock();
                //await SyncData<View_VTE_VENTE, VTE_VENTE>();
                await SyncUsers();
                await syncPermission();
                await syncSession();
                await SyncFamille();
                await SyncTypeTiers();
                await SyncSecteurs();
                UserDialogs.Instance.HideLoading();
                await getInstance().DeleteAllAsync<View_VTE_VENTE>();
                await getInstance().DeleteAllAsync<View_VTE_VENTE_LOT>();
                await SyncVenteToServer();

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
                await SyncData<View_LIV_TOURNEE, LIV_TOURNEE>(false, paramLivTournee, TourneeMethodName);
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
                await SyncData<View_LIV_TOURNEE_DETAIL, LIV_TOURNEE_DETAIL>(false, paramLivTourneeDetail, TourneeDetailMethodName);

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
                await SyncData<View_STK_STOCK, STK_STOCK>(false, paramStock, StockMethodName);
            }
        }

        public static async Task SyncUsers()
        {
            UsersMethodName = "SyncUsers";
            await SyncData<SYS_USER, SYS_USER>(false, "", UsersMethodName);
        }

        public static async Task syncPermission()
        {
            idGroup = App.User.UserGroup;
            paramPermission = "idGroup=" + idGroup;
            PermissionMethodName = "GetPermissions";
            await SyncData<SYS_OBJET_PERMISSION, SYS_OBJET_PERMISSION>(false, paramPermission, PermissionMethodName);
        }

        public static async Task syncSession()
        {
            var session = await CrudManager.Sessions.GetCurrentSession();
            await getInstance().DeleteAllAsync<TRS_JOURNEES>();
            var id = await getInstance().InsertAsync(session);
            //SessionMethodName = "GetCurrentSession";
            //await SyncData<TRS_JOURNEES, TRS_JOURNEES>(false, "", SessionMethodName);
        }

        public static async Task SyncFamille()
        {
            try
            {
                var itemsC = await WebServiceClient.getTiersFamilles();
                await getInstance().DeleteAllAsync<View_BSE_TIERS_FAMILLE>();

                View_BSE_TIERS_FAMILLE allElem = new View_BSE_TIERS_FAMILLE();
                allElem.CODE_FAMILLE = "";
                allElem.DESIGN_FAMILLE = "";
                var id = await getInstance().InsertAsync(allElem);

                var id2 = await getInstance().InsertAllAsync(itemsC);
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
                await getInstance().DeleteAllAsync<BSE_TABLE_TYPE>();

                BSE_TABLE_TYPE allElem = new BSE_TABLE_TYPE();
                allElem.CODE_TYPE = "";
                allElem.DESIGNATION_TYPE = "";
                var id = await getInstance().InsertAsync(allElem);

                var id2 = await getInstance().InsertAllAsync(itemsC);
            }
            catch (Exception ex)
            {
                await UserDialogs.Instance.AlertAsync(WSApi2.GetExceptionMessage(ex), AppResources.alrt_msg_Alert,
                    AppResources.alrt_msg_Ok);
            }
        }

        public static async Task SyncSecteurs()
        {
            try
            {
                var itemsC = await CrudManager.BSE_LIEUX.GetItemsAsync();
                await getInstance().DeleteAllAsync<BSE_TABLE>();

                BSE_TABLE allElem = new BSE_TABLE();
                allElem.CODE = "";
                allElem.DESIGNATION = "";
                var id = await getInstance().InsertAsync(allElem);

                var id2 = await getInstance().InsertAllAsync(itemsC);
            }
            catch (Exception ex)
            {
                await UserDialogs.Instance.AlertAsync(WSApi2.GetExceptionMessage(ex), AppResources.alrt_msg_Alert,
                    AppResources.alrt_msg_Ok);
            }
        }


        public static async Task<string> AjoutVente(View_VTE_VENTE vente)
        {
            if ((vente.TOTAL_PAYE + vente.MBL_MT_VERCEMENT) > vente.TOTAL_TTC)
            {
                await UserDialogs.Instance.AlertAsync("Montant versé suppérieur au montant à payer!", AppResources.alrt_msg_Alert, AppResources.alrt_msg_Ok);
                return vente.MBL_MT_VERCEMENT.ToString();
            }
            else
            {
                //var obj = await getInstance().Table<View_VTE_VENTE>().ToListAsync();
                vente.CODE_VENTE = vente.ID + "/" + App.PrefixCodification;
                vente.NUM_VENTE = vente.CODE_VENTE;
                var id = await getInstance().InsertAsync(vente);
                foreach (var item in vente.Details)
                {
                    item.CODE_VENTE = vente.CODE_VENTE;
                }
                var id2 = await getInstance().InsertAllAsync(vente.Details);
                await UpdateStock(vente);
                await UpdateTourneeDetail(vente);
                await UpdateTournee();
                return vente.CODE_VENTE;
            }
        }

        public static async Task AjoutTiers(View_TRS_TIERS tiers)
        {
            tiers.NOM_TIERS1 = tiers.NOM_TIERS + " " + tiers.PRENOM_TIERS;

            List<BSE_TABLE_TYPE> Types = await getInstance().Table<BSE_TABLE_TYPE>().ToListAsync();
            tiers.DESIGNATION_TYPE = Types.Where(e => e.CODE_TYPE == tiers.CODE_TYPE).FirstOrDefault()?.DESIGNATION_TYPE;

            List<View_BSE_TIERS_FAMILLE> familles = await getInstance().Table<View_BSE_TIERS_FAMILLE>().ToListAsync();
            tiers.DESIGN_FAMILLE = familles.Where(e => e.CODE_FAMILLE == tiers.CODE_FAMILLE).FirstOrDefault()?.DESIGN_FAMILLE;

            tiers.ETAT_TIERS = STAT_TIERS_MOBILE.ADDED;
            var id = await getInstance().InsertAsync(tiers);
        }

        public static async Task UpdateTiers(View_TRS_TIERS tiers)
        {
            tiers.NOM_TIERS1 = tiers.NOM_TIERS + " " + tiers.PRENOM_TIERS;

            List<BSE_TABLE_TYPE> Types = await getInstance().Table<BSE_TABLE_TYPE>().ToListAsync();
            tiers.DESIGNATION_TYPE = Types.Where(e => e.CODE_TYPE == tiers.CODE_TYPE).FirstOrDefault()?.DESIGNATION_TYPE;

            List<View_BSE_TIERS_FAMILLE> familles = await getInstance().Table<View_BSE_TIERS_FAMILLE>().ToListAsync();
            tiers.DESIGN_FAMILLE = familles.Where(e => e.CODE_FAMILLE == tiers.CODE_FAMILLE).FirstOrDefault()?.DESIGN_FAMILLE;

            tiers.ETAT_TIERS = STAT_TIERS_MOBILE.UPDATED;
            await getInstance().UpdateAsync(tiers);
        }

        public static async Task AjoutToken(Token token)
        {
            await getInstance().DeleteAllAsync<Token>();

            var id = await getInstance().InsertAsync(token);

        }

        public static async Task UpdateStock(View_VTE_VENTE vente)
        {
            var stock = await getInstance().Table<View_STK_STOCK>().ToListAsync();

            foreach (var item in stock)
            {
                foreach (var items in vente.Details)
                {
                    if (item.ID_STOCK == items.ID_STOCK)
                    {
                        item.OLD_QUANTITE = item.OLD_QUANTITE - items.QUANTITE;
                        item.QUANTITE = item.QUANTITE - items.QUANTITE;
                        await getInstance().UpdateAsync(item);
                    }
                }
            }
        }

        public static async Task UpdateTourneeDetail(View_VTE_VENTE vente)
        {
            string codeTourneeDetail = vente.MBL_CODE_TOURNEE_DETAIL;


            var tournees = await getInstance().Table<View_LIV_TOURNEE_DETAIL>().ToListAsync();

            foreach (var item in tournees)
            {
                if (item.CODE_DETAIL == codeTourneeDetail)
                {
                    item.CODE_VENTE = vente.CODE_VENTE;
                    item.CODE_ETAT = TourneeStatus.Delevred;
                    item.ETAT_COLOR = "#008000";
                    item.GPS_LATITUDE = vente.GPS_LATITUDE;
                    item.GPS_LONGITUDE = vente.GPS_LATITUDE;
                    await getInstance().UpdateAsync(item);
                }
            }
        }

        public static async Task<bool> saveGPSToTiers(View_TRS_TIERS tiers)
        {
            if (!XpertHelper.IsNullOrEmpty(tiers) && !XpertHelper.IsNullOrEmpty(tiers.CODE_TIERS))
            {
                var tournees = await getInstance().Table<View_TRS_TIERS>().ToListAsync();

                foreach (var item in tournees)
                {
                    if (item.CODE_TIERS == tiers.CODE_TIERS)
                    {
                        item.GPS_LATITUDE = tiers.GPS_LATITUDE;
                        item.GPS_LONGITUDE = tiers.GPS_LONGITUDE;
                        await getInstance().UpdateAsync(item);
                        return true;
                    }
                }
            }
            return false;
        }

        public static async Task UpdateTournee()
        {
            var tournee = await getInstance().Table<View_LIV_TOURNEE>().ToListAsync();
            foreach (var item in tournee)
            {
                item.NBR_EN_DELEVRED = item.NBR_EN_DELEVRED + 1;
                await getInstance().UpdateAsync(item);
            }
        }

        public static async Task<bool> AuthUser(User user)
        {
            bool validInformations = false;
            var Users = await getInstance().Table<SYS_USER>().ToListAsync();
            foreach (var item in Users)
            {
                var password = XpertHelper.GetMD5Hash(user.PassWord);
                if (item.ID_USER.ToLower() == user.UserName.ToLower() && item.PASS_USER == password)
                {
                    validInformations = true;
                    return validInformations;
                }
            }
            return validInformations;
        }

        public static async Task<SYS_USER> getUserInfo(string userid)
        {
            if (userid != null)
            {
                var Users = await getInstance().Table<SYS_USER>().ToListAsync();
                var user = Users.Where(x => x.ID_USER.ToLower() == userid.ToLower()).FirstOrDefault();
                if (user != null)
                {
                    return user;
                }
            }
            return null;
        }


        public static async Task<Token> getToken(User user)
        {
            Token validToken = new Token();
            var Tokens = await getInstance().Table<Token>().ToListAsync();
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

        public static async Task<List<SYS_OBJET_PERMISSION>> getPermission()
        {
            var permission = await getInstance().Table<SYS_OBJET_PERMISSION>().ToListAsync();
            return permission;
        }
        
        public static async Task<List<View_BSE_TIERS_FAMILLE>> getFamille()
        {
            var Famille = await getInstance().Table<View_BSE_TIERS_FAMILLE>().ToListAsync();
            return Famille;
        }

        public static async Task<List<BSE_TABLE_TYPE>> getTypeTiers()
        {
            var TypeTiers = await getInstance().Table<BSE_TABLE_TYPE>().ToListAsync();
            return TypeTiers;
        }

        public static async Task<List<BSE_TABLE>> getSecteurs()
        {
            var Secteurs = await getInstance().Table<BSE_TABLE>().ToListAsync();
            return Secteurs;
        }



        public static async Task<string> SyncVenteToServer()
        {
            try
            {
                UserDialogs.Instance.ShowLoading(AppResources.txt_Waiting);

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
                    var bll = CrudManager.GetVteBll(VentesTypes.Livraison);
                    var res = await bll.SyncVentes(ListVentes);

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

        public static async Task SyncTiersToServer()
        {
            try
            {
                UserDialogs.Instance.ShowLoading(AppResources.txt_Waiting);
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
                UserDialogs.Instance.ShowLoading(AppResources.txt_Waiting);
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
                await UserDialogs.Instance.AlertAsync(WSApi2.GetExceptionMessage(ex), AppResources.alrt_msg_Alert,AppResources.alrt_msg_Ok);
                //await UserDialogs.Instance.AlertAsync("erreur de synchronisation des Encaissements !!", AppResources.alrt_msg_Alert, AppResources.alrt_msg_Ok);
            }
        }

        public static async Task<List<View_VTE_VENTE_LOT>> getVenteDetails(string CodeVente)
        {
            List<View_VTE_VENTE_LOT> ventes = await getInstance().Table<View_VTE_VENTE_LOT>().ToListAsync();
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
            var prefix = await getInstance().Table<SYS_CONFIGURATION_MACHINE>().FirstOrDefaultAsync();
            string deviceName = prefix.MACHINE;
            SYS_MACHINE_CONFIG_Manager bll = new SYS_MACHINE_CONFIG_Manager();
            var res = await bll.GetPrefix(deviceName);
            await getInstance().DeleteAllAsync<SYS_CONFIGURATION_MACHINE>();
            var id = await getInstance().InsertAsync(res);
            return res;
        }
        public static async Task AssignPrefix()
        {
            var prefix = await getInstance().Table<SYS_CONFIGURATION_MACHINE>().FirstOrDefaultAsync();
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
            var obj = await getInstance().Table<View_LIV_TOURNEE>().ToListAsync();
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
