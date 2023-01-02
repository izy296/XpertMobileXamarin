using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Xpert.Common.DAO;

namespace Xpert.Common.WSClient.Model
{
    public interface ICurdService<T>
    {
        Task<T> GetItemAsync(string id);
        Task<IEnumerable<T>> GetItemsAsync();
        Task<IEnumerable<T>> GetItemsAsync(QueryInfos filter);
        Task<IEnumerable<T>> GetItemsAsyncWithUrl(string MethodName, string param);
        Task<string> AddItemAsync(T item);
        Task<bool> UpdateItemAsync(T item);
        Task<bool> DeleteItemAsync(string id);
        Task<int> ItemsCount(QueryInfos filter);
        Task<IEnumerable<T>> SelectByPage(QueryInfos filter, int page, int count);
        Task<decimal> ItemsSum(QueryInfos filter);
        Task<SortedDictionary<string, decimal>> ItemsSums(QueryInfos filter);

    }
}

