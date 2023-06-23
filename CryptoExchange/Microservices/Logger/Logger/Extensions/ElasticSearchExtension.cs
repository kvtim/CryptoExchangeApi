using Elasticsearch.Net;
using Logger.ElasticSearch;
using Logger.Logger;
using Nest;

namespace Logger.Extensions
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

            services.AddSingleton<IElasticSearch, ElasticSearch.ElasticSearch>();

            services.AddSingleton<IElasticLogger, ElasticLogger>();
        }
    }
}
