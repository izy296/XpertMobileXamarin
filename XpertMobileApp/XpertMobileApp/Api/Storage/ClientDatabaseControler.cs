using XpertMobileApp.Models;
using SQLite;
using System.Collections.Generic;
using System.Threading.Tasks;
using Xamarin.Forms;
using System;

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

        /// <summary>
        /// Retourne un item de la table Client avec un id spécifique ...
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public Task<Client> GetItemAsync(string id)
        {
            return database.Table<Client>().Where(i => i.Id == id).FirstOrDefaultAsync();
        }

        /// <summary>
        /// Retourne le premier element dans la table s'il existe deja ...
        /// </summary>
        /// <returns></returns>
        /// <exception cref="Exception"></exception>
        public Task<Client> GetFirstItemAsync()
        {
            try
            {
                var result = database.Table<Client>().FirstOrDefaultAsync();
                return result;
            }
            catch (Exception e)
            {
                throw new Exception(e.Message);
            }
        }
        /// <summary>
        /// Mise à jour toutes les columns avec le méme Id de la table Client
        /// S'il existe un client : 
        /// il le met à jour 
        /// Sinon : 
        /// Inserer un nouveau objet Client dans la table
        /// </summary>
        /// <param name="item"></param>
        /// <returns></returns>
        public Task<int> SaveItemAsync(Client item)
        {
            if (GetItemAsync(item.Id).Result != null)
            {
                return database.UpdateAsync(item);
            }
            else
            {
                item.Id = Guid.NewGuid().ToString();
                return database.InsertAsync(item);
            }
        }

        /// <summary>
        /// Supprime un item de la table Client...
        /// </summary>
        /// <param name="item"></param>
        /// <returns></returns>
        public Task<int> DeleteItemAsync(Client item)
        {
            return database.DeleteAsync(item);
        }
    }
}
