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

        /// <summary>
        /// Création de la table Settings ...
        /// </summary>
        /// <param name="dbPath"></param>
        public SettingsDatabaseControler(string dbPath)
        {
            database = new SQLiteAsyncConnection(dbPath);
            database.CreateTableAsync<Settings>().Wait();
        }


        public Task<List<Settings>> GetItemsAsync()
        {
            return database.Table<Settings>().ToListAsync();
        }

        /// <summary>
        /// Obenir un item de la table Settings avec l'id ...
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public Task<Settings> GetItemAsync(int id)
        {
            return database.Table<Settings>().Where(i => i.Id == id).FirstOrDefaultAsync();
        }


        /// <summary>
        /// Obtenir le premier item dans la table Settings ...
        /// </summary>
        /// <returns></returns>
        public Task<Settings> GetFirstItemAsync()
        {
            return database.Table<Settings>().FirstOrDefaultAsync();
        }

        /// <summary>
        /// Insérer ou remplacer l'objet settings...
        /// </summary>
        /// <param name="item"></param>
        /// <returns></returns>
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

        /// <summary>
        /// Supprime un item de la table Settings ...
        /// </summary>
        /// <param name="item"></param>
        /// <returns></returns>
        public Task<int> DeleteItemAsync(Settings item)
        {
            return database.DeleteAsync(item);
        }
    }
}
