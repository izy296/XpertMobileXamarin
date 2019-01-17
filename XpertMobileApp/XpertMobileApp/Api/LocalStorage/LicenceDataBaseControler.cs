using XpertMobileApp.Models;
using SQLite;
using System.Collections.Generic;
using System.Threading.Tasks;
using Xamarin.Forms;

namespace XpertMobileApp.Data
{
    public class ClientDatabaseControler
    {
        static object locker = new object();

        SQLite.SQLiteAsyncConnection database;

        public ClientDatabaseControler(string dbPath)
        {
            database = new SQLiteAsyncConnection(dbPath);
            database.CreateTableAsync<Client>().Wait();
        }

        public Task<List<Client>> GetItemsAsync()
        {
            return database.Table<Client>().ToListAsync();
        }

        public Task<Client> GetItemAsync(string id)
        {
            return database.Table<Client>().Where(i => i.Id == id).FirstOrDefaultAsync();
        }

        public Task<Client> GetFirstItemAsync()
        {
            return database.Table<Client>().FirstOrDefaultAsync();
        }

        public Task<int> SaveItemAsync(Client item)
        {
            if (item.Id != "" && GetItemAsync(item.Id).Result != null)
            {
                return database.UpdateAsync(item);
            }
            else
            {
                return database.InsertAsync(item);
            }
        }

        public Task<int> DeleteItemAsync(Client item)
        {
            return database.DeleteAsync(item);
        }
    }
}
