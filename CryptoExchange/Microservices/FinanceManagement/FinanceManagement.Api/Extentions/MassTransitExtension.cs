using EventBus.Messages.Common;
using FinanceManagement.Api.EventBusConsumer;
using MassTransit;

namespace FinanceManagement.Api.Extentions
{
    public static class MassTransitExtension
    {
        public static void ConfigureMassTransit(
            this IServiceCollection services,
            ConfigurationManager configuration)
        {
            services.AddMassTransit(config =>
            {
                config.AddConsumer<CreateNewUserWalletConsumer>();
                config.UsingRabbitMq((ctx, cfg) =>
                {
                    cfg.Host(configuration["EventBusSettings:HostAddress"]);
                    cfg.ReceiveEndpoint(EventBusConstants.CreateNewUserWallet, c => {
                        c.ConfigureConsumer<CreateNewUserWalletConsumer>(ctx);
                    });
                });
            });
        }
    }
}
