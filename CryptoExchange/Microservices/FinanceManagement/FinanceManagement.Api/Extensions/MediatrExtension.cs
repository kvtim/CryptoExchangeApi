using FinanceManagement.Data.Wallets.Commands.CreateWallet;

namespace FinanceManagement.Api.Extensions
{
    public static class MediatrExtension
    {
        public static void AddMediatrWithConfiguration(this IServiceCollection services)
        {
            services.AddMediatR(
                cfg => cfg.RegisterServicesFromAssembly(typeof(CreateWalletCommand).Assembly));
        }
    }
}
