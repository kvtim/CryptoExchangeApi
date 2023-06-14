using Elasticsearch.Net;
using FinanceManagement.Data.Elasticsearch;
using FinanceManagement.Data.Logger;
using FinanceManagement.Data.Wallets.Commands.CreateWallet;
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
