using Nest;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FinanceManagement.Core.ElasticSearch
{
    public interface IElasticSearch
    {
        Task CreateIndexIfNotExists(string indexName);
        Task<bool> AddOrUpdate<T>(T document) where T : class;
        Task<List<T>?> GetAll<T>() where T : class;
        Task<List<T>?> Query<T>(QueryContainer predicate) where T : class;
        Task<long> RemoveAll<T>() where T : class;
    }
}
