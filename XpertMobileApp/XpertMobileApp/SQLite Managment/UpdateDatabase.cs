using SQLite;
using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Text;
using System.Threading.Tasks;
using Xamarin.Essentials;
using Xpert.Common.WSClient.Model;
using Xpert.Common.WSClient.Services;
using XpertMobileApp.DAL;
using XpertMobileApp.Models;
using XpertMobileApp.ViewModels;


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
        
        public static SQLiteAsyncConnection getInstance()
        {
            if (db == null) {
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


            }
            catch (Exception e )
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
                return await service.GetItemsAsyncWithUrl(methodName , param);
            }
            
        }

        public static async Task SyncData<TView, TTabel>(bool _selectAll = true, string param="" , string methodName = "")
        {
            var ListItems = new List<TView>();

            var items = await getItemsUnsyncronised<TView, TTabel>(_selectAll, param , methodName);
            await getInstance().DeleteAllAsync<TView>();

            try
            {
                if (items != null )//&& items.IsCompleted && items.Status == TaskStatus.RanToCompletion)
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
            await initialisationDbLocal();
            await SyncData<View_STK_PRODUITS, STK_PRODUITS>();
            await SyncData<View_TRS_TIERS, TRS_TIERS>();
            await SyncLivTournee();
            await SyncLivTourneeDetail();
            await SyncData<View_STK_STOCK, STK_STOCK>();
            //await SyncData<View_VTE_VENTE, VTE_VENTE>();
            await SyncUsers();
            await syncPermission();
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


        //public static Boolean TableExistss(String tableName, SQLiteAsyncConnection connection)
        //{
        //    SQLite.TableMapping map = new TableMapping(typeof(SqlDbType)); // Instead of mapping to a specific table just map the whole database type
        //    object[] ps = new object[0]; // An empty parameters object since I never worked out how to use it properly! (At least I'm honest)

        //    Int32 tableCount = connection.QueryAsync(map, "SELECT * FROM sqlite_master WHERE type = 'table' AND name = '" + tableName + "'", ps).Count; // Executes the query from which we can count the results
        //    if (tableCount == 0)
        //    {
        //        return false;
        //    }
        //    else if (tableCount == 1)
        //    {
        //        return true;
        //    }
        //    else
        //    {
        //        throw new Exception("More than one table by the name of " + tableName + " exists in the database.", null);
        //    }

        //}

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

        //public static bool TableExistssss(String tableName, SQLiteConnection connection)
        //{
        //    SQLiteCommand cmd = connection.CreateCommand();
        //    cmd.CommandText = "SELECT * FROM sqlite_master WHERE type = 'table' AND name = @name";
        //    cmd.Parameters.Add("@name", DbType.String).Value = tableName;
        //    return (cmd.ExecuteScalar() != null);
        //}


        //public static bool TableExistssss(String tableName, SQLiteAsyncConnection connection)
        //{
        //    using (SQLiteCommand cmd = new SQLiteCommand())
        //    {
        //        cmd.CommandType = CommandType.Text;
        //        cmd.Connection = connection;
        //        cmd.CommandText = "SELECT * FROM sqlite_master WHERE type = 'table' AND name = @name";
        //        cmd.Parameters.AddWithValue("@name", tableName);

        //        using (SqliteDataReader sqlDataReader = cmd.ExecuteReader())
        //        {
        //            if (sqlDataReader.Read())
        //                return true;
        //            else
        //                return false;
        //        }
        //    }
        //}

    }
}
