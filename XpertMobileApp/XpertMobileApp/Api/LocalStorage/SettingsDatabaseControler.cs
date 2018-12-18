using XpertMobileApp.Models;
using SQLite;
using System.Collections.Generic;
using System.Threading.Tasks;
using Xamarin.Forms;

namespace XpertMobileApp.Data
{
    public class SettingsDatabaseControler
    {
        static object locker = new object();

        SQLite.SQLiteAsyncConnection database;

        public SettingsDatabaseControler(string dbPath)
        {
            database = new SQLiteAsyncConnection(dbPath);
            database.CreateTableAsync<Settings>().Wait();
        }


        public Task<List<Settings>> GetItemsAsync()
        {
            return database.Table<Settings>().ToListAsync();
        }

        public Task<Settings> GetItemAsync(int id)
        {
            return database.Table<Settings>().Where(i => i.Id == id).FirstOrDefaultAsync();
        }

        public Task<Settings> GetFirstItemAsync()
        {
            return database.Table<Settings>().FirstOrDefaultAsync();
        }

        public Task<int> SaveItemAsync(Settings item)
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

        public Task<int> DeleteItemAsync(Settings item)
        {
            return database.DeleteAsync(item);
        }
    }
}
