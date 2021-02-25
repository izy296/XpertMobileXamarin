using Realms;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using XpertMobileApp.DAL;
using XpertMobileApp.Services;

namespace XpertMobileApp.Api
{
    public class RLMDataManager<T> 
        where T : View_VTE_VENTE
    {
        Realm _realm = Realm.GetInstance(typeof(T).Name);

        public void AddItemAsync(T item)
        {
            _realm.Write(() =>
            {
                _realm.Add(item);
            });
        }
            
        public void DeleteItemAsync(string id)
        {
            var param = this.GetItemAsync(id);
            using (var trans = _realm.BeginWrite())
            {
                _realm.Remove(param);
                trans.Commit();
            }
        }

        public void UpdateItemAsync(T item)
        {
            _realm.Write(() =>
            {
                 _realm.Add(item);
            });
        }

        public T GetItemAsync(string id)
        {
            var result = _realm.All<T>().First(a => a.ID == id);
            return result;
        }

        public List<T> GetItemsAsync(bool forceRefresh = false)
        {
            var result = _realm.All<T>().ToList();
            return result;
        }
    }
}
