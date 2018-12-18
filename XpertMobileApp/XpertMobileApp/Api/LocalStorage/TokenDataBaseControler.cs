using XpertMobileApp.Models;
using SQLite;
using System.Collections.Generic;
using System.Threading.Tasks;
using Xamarin.Forms;

namespace XpertMobileApp.Data
{
    public class TokenDatabaseControler
    {
        static object locker = new object();

        SQLite.SQLiteAsyncConnection database;

        public TokenDatabaseControler(string dbPath)
        {
            database = new SQLiteAsyncConnection(dbPath);
            database.CreateTableAsync<Token>().Wait();
        }

        public Task<List<Token>> GetItemsAsync()
        {
            return database.Table<Token>().ToListAsync();
        }

        public Task<Token> GetItemAsync(int id)
        {
            return database.Table<Token>().Where(i => i.Id == id).FirstOrDefaultAsync();
        }

        public Task<Token> GetFirstItemAsync()
        {
            return database.Table<Token>().FirstOrDefaultAsync();
        }

        public Task<int> SaveItemAsync(Token item)
        {
            if (item.Id != 0 && GetItemAsync(item.Id).Result != null)
            {
                return database.UpdateAsync(item);
            }
            else
            {
                return database.InsertAsync(item);
            }
        }

        public Task<int> DeleteItemAsync(Token item)
        {
            return database.DeleteAsync(item);
        }
    }
}
