using Logger.EventBusConsumer;
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
           
            services.ConfigureMassTransit(configuration);

            services.AddScoped<CreateNewLogConsumer>();

            services.ConfigureElasticSearch(configuration);

            services.ConfigureSwagger();
        }
    }
}
