using Elasticsearch.Net;
using FinanceManagement.Data.Elasticsearch;
using Nest;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FinanceManagement.Data.Elasticsearch
{
    public class ElasticSearch : IElasticSearch
    {
        private string _indexName { get; set; }
        private readonly ElasticClient _client;
        public ElasticSearch(ElasticClient client)
        {
            _client = client;
        }

        public async Task CreateIndexIfNotExists(string indexName)
        {
            if (!_client.Indices.Exists(indexName).Exists)
            {
                await _client.Indices.CreateAsync(indexName, c => c.Map<dynamic>(m => m.AutoMap()));
            }
            _indexName = indexName;
        }

        public async Task<bool> AddOrUpdate<T>(T document) where T : class
        {
            var indexResponse = await _client.IndexAsync(document, idx => idx.Index(_indexName).OpType(OpType.Index));
            return indexResponse.IsValid;
        }

        public async Task<List<T>?> GetAll<T>() where T : class
        {
            var searchResponse = await _client.SearchAsync<T>(s => s.Index(_indexName).Query(q => q.MatchAll()));
            return searchResponse.IsValid ? searchResponse.Documents.ToList() : null;
        }

        public async Task<List<T>?> Query<T>(QueryContainer predicate) where T : class
        {
            var searchResponse = await _client.SearchAsync<T>(s => s.Index(_indexName).Query(q => predicate));
            return searchResponse.IsValid ? searchResponse.Documents.ToList() : null;
        }

        public async Task<long> RemoveAll<T>() where T : class
        {
            var response = await _client.DeleteByQueryAsync<T>(d => d.Index(_indexName).Query(q => q.MatchAll()));
            return response.Deleted;
        }
    }
}
