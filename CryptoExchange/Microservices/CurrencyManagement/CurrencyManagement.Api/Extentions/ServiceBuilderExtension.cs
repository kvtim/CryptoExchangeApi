using CurrencyManagement.Api.Extentions;
using CurrencyManagement.Core.BiqQuery;
using CurrencyManagement.Core.Cassandra;
using CurrencyManagement.Core.Repositories;
using CurrencyManagement.Core.Services;
using CurrencyManagement.Core.UnitOfWork;
using CurrencyManagement.Data;
using CurrencyManagement.Data.BigQuery;
using CurrencyManagement.Data.Cassandra;
using CurrencyManagement.Data.Repositories;
using CurrencyManagement.Data.UnitOfWork;
using CurrencyManagement.Services.Services;
using MassTransit;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.OpenApi.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CurrencyManagement.Api.Extensions
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

            services.AddScoped<ICurrencyRepository, CurrencyRepository>();

            services.AddScoped<IUnitOfWork, UnitOfWork>();

            services.AddScoped<ICurrencyService, CurrencyService>();

            services.AddScoped<IBigQuery, BigQuery>();

            services.AddScoped<IAnalysisService, AnalysisService>();

            services.ConfigureCassandra();

            services.AddScoped<ICassandraRepository, CassandraRepository>();
            
            services.AddScoped<ICassandraService, CassandraService>();

            services.AddScoped<IKafkaProducerService, KafkaProducerService>();

            services.AddControllersWithJsonConfiguration();

            services.AddEndpointsApiExplorer();

            services.ConfigureSwagger();
        }
    }
}
