using FinanceManagement.Api.EventBusConsumer;
using FinanceManagement.Api.Extentions;
using FinanceManagement.Core.Repositories;
using FinanceManagement.Data;
using FinanceManagement.Data.Repositories;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.OpenApi.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FinanceManagement.Api.Extensions
{
    public static class ServiceBuilderExtension
    {
        public static void ConfigureServices(
            this IServiceCollection services,
             ConfigurationManager configuration)
        {
            services.AddJWTAuthentication(configuration);

            services.AddDbContext<ApplicationDbContext>(options =>
            {
                options.UseNpgsql(configuration.GetConnectionString("DefaultConnection"));
            });

            services.AddAutoMapper(typeof(Program));

            services.AddScoped(typeof(IRepository<>), typeof(Repository<>));
            services.AddScoped<IWalletRepository, WalletRepository>();
            services.AddScoped<ITransactionRepository, TransactionRepository>();

            services.AddMediatrWithConfiguration();

            services.AddControllersWithJsonConfiguration();

            // Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
            services.AddEndpointsApiExplorer();
            services.AddSwaggerGen();

            services.ConfigureMassTransit(configuration);

            services.AddScoped<CreateNewUserWalletConsumer>();

            services.ConfigureSwagger();
        }
    }
}
