using XpertMobileApp.Models;
using SQLite;
using System.Collections.Generic;
using System.Threading.Tasks;
using Xamarin.Forms;
using XpertMobileApp.DAL;
using XpertMobileApp.Data;

namespace XpertMobileApp.Api
{
    /// <summary>
    /// 
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public class SLDataManager<T> 
        where T : new()
    {
        private static string LOCAL_DB_NAME = Constants.LOCAL_DB_NAME;
        static object locker = new object();
        string path = DependencyService.Get<IFileHelper>().GetLocalFilePath(LOCAL_DB_NAME);
        SQLite.SQLiteAsyncConnection database;

        public SLDataManager(string dbPath)
        {
            database = new SQLiteAsyncConnection(dbPath);
            database.CreateTableAsync<T>().Wait();
        }

        public Task<List<T>> GetItemsAsync()
        {
            return database.Table<T>().ToListAsync();
        }

        public Task<List<T>> GetItemsAsync(System.Linq.Expressions.Expression<System.Func<T, bool>> predExep)
        {
            return database.Table<T>().Where(predExep).ToListAsync();
        }

        public virtual Task<T> GetItemById(string id)
        {
            throw new System.Exception("Not implemented");

            /* Récupération de la clé de la class à partir de l'attribut primaryKey
            string keyName = "ID";
            foreach (var item in typeof(T).GetProperties())
            {
                foreach (var att in item.CustomAttributes)
                {
                    if(att.AttributeType is PrimaryKeyAttribute) 
                    {
                        keyName = att.Name;
                    }
                }
            }
            */
        }

        public Task<T> GetItemAsync(System.Linq.Expressions.Expression<System.Func<T, bool>> predExep)
        {
            return database.Table<T>().Where(predExep).FirstOrDefaultAsync();
        }

        public Task<T> GetFirstItemAsync()
        {
            return database.Table<T>().FirstOrDefaultAsync();
        }

        public Task<int> SaveItemAsync(T item)
        {
            return database.InsertOrReplaceAsync(item);
            /*
            if (item.Id != 0)
            {
                return database.UpdateAsync(item);
            }
            else
            {
                return database.InsertAsync(item);
            }
            */
        }

        public Task<int> DeleteItemAsync(T item)
        {
            return database.DeleteAsync(item);
        }
    }
}
