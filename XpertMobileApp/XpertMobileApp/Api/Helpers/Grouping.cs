using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace XpertMobileApp.Helpers
{
    public class Grouping<K, O, T> : ObservableCollection<T>
    {
        public K Key { get; private set; }

        public Grouping(K key, O orderBy, IEnumerable<T> items)
        {
            Key = key;
            foreach (var item in items)
                this.Items.Add(item);
        }
    }
}
