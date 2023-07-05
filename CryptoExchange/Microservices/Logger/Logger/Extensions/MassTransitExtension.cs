using EventBus.Messages.Common;
using Logger.EventBusConsumer;
using MassTransit;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Logger.Extensions
{
    public static class MassTransitExtension
    {
        public static void ConfigureMassTransit(
            this IServiceCollection services,
            ConfigurationManager configuration)
        {
            services.AddMassTransit(config =>
            {
                config.AddConsumer<CreateNewLogConsumer>();
                config.UsingRabbitMq((ctx, cfg) =>
                {
                    cfg.Host(configuration["EventBusSettings:HostAddress"]);
                    cfg.ReceiveEndpoint(EventBusConstants.CreateNewLog, c => {
                        c.ConfigureConsumer<CreateNewLogConsumer>(ctx);
                    }); 
                });
            });
        }
    }
}
