using Acr.UserDialogs;
using SQLite;
using System;
using System.Collections.Generic;
using System.IO;
using System.Threading.Tasks;
using Xamarin.Essentials;
using Xpert.Common.WSClient.Model;
using Xpert.Common.WSClient.Services;
using XpertMobileApp.Api.Managers;
using XpertMobileApp.Api.Services;
using XpertMobileApp.DAL;
using XpertMobileApp.Models;


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
                //await getInstance().DropTableAsync<View_VTE_VENTE>();

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


        public static async Task synchronise()
        {
            UserDialogs.Instance.ShowLoading(AppResources.txt_Waiting);
            bool isconnected =  await App.IsConected();
            if (isconnected)
            {
                await initialisationDbLocal();
                await SyncData<View_STK_PRODUITS, STK_PRODUITS>();
                await SyncData<View_TRS_TIERS, TRS_TIERS>();
                await SyncLivTournee();
                await SyncLivTourneeDetail();
                await SyncStock();
                //await SyncData<View_VTE_VENTE, VTE_VENTE>();
                await SyncUsers();
                await syncPermission();
                await syncSession();
                UserDialogs.Instance.HideLoading();
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
                var id = await getInstance().InsertAsync(vente);
                await UpdateTourneeDetail(vente);
                await UpdateTournee();
                return vente.CODE_VENTE;
            }
        }

        public static async Task AjoutToken(Token token)
        {
            bool added = false;
            var OldTokens = await getInstance().Table<Token>().ToListAsync();
            foreach (var item in OldTokens)
            {
                if (item.Id == token.Id)
                {
                    item.access_token = token.access_token;
                    item.Id = token.Id;
                    item.expires_in = token.expires_in;
                    item.expire_Date = token.expire_Date;
                    await getInstance().UpdateAsync(item);
                    added = true;
                }
            }
            if (!added)
            {
                var id = await getInstance().InsertAsync(token);
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
                    await getInstance().UpdateAsync(item);
                }
            }
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
