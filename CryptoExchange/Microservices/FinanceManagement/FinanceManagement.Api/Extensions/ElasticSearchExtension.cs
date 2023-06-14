using Elasticsearch.Net;
using FinanceManagement.Core.ElasticSearch;
using FinanceManagement.Core.Logger;
using FinanceManagement.Data.ElasticSearch;
using FinanceManagement.Data.Logger;
using Nest;

namespace FinanceManagement.Api.Extensions
{
    public static class ElasticSearchExtension
    {
        public static void ConfigureElasticSearch(
            this IServiceCollection services,
            ConfigurationManager configuration)
        {
            var pool = new SingleNodeConnectionPool(new Uri(configuration["ElasticSearch:Uri"]));
            var settings = new ConnectionSettings(pool);
            var client = new ElasticClient(settings);
            services.AddSingleton(client);

            services.AddSingleton<IElasticSearch, ElasticSearch>();

            services.AddSingleton<IFinanceLogger, FinanceLogger>();
        }
    }
}
