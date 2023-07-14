using EventBus.Messages.Common;
using Logger.EventBusConsumer;
using Logger.KafkaConsumer;
using Logger.Logger;
using Logger.Models;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.OpenApi.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Logger.Extensions
{
    public static class ServiceBuilderExtension
    {
        public static void ConfigureServices(
            this IServiceCollection services,
             ConfigurationManager configuration)
        {
            services.AddJWTAuthentication(configuration);
            services.AddControllers();
            services.AddEndpointsApiExplorer();
            services.ConfigureMassTransit(configuration);

            services.AddScoped<CreateNewLogConsumer>();

            services.ConfigureElasticSearch(configuration);

            var provider = services.BuildServiceProvider();
            var elasticLogger = provider.GetService<IElasticLogger>();
            
            KafkaConsumerHandler currencyConsumer = new KafkaConsumerHandler(
                configuration["KafkaSettings:HostAddress"],
                "currency_logs_group",
                elasticLogger
                );

            Task.Run(() => currencyConsumer.StartAsync(TopicNamesConstants.CurrencyLogsTopic));
            Task.Run(() => currencyConsumer.StartAsync(TopicNamesConstants.FinanceLogsTopic));
            Task.Run(() => currencyConsumer.StartAsync(TopicNamesConstants.UserLogsTopic));

            services.ConfigureSwagger();
        }
    }
}
