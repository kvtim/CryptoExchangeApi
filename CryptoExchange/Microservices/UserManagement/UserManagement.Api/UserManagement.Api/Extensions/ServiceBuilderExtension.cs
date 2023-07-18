using UserManagement.Data;
using UserManagement.Data.Repositories;
using UserManagement.Data.UnitOfWork;
using UserManagement.Services.Services;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.OpenApi.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UserManagement.Core.Models;
using UserManagement.Core.Repositories;
using UserManagement.Core.Security;
using UserManagement.Core.Services;
using UserManagement.Core.UnitOfWork;
using MassTransit;

namespace UserManagement.Api.Extensions
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

            services.AddScoped<IUserRepository, UserRepository>();

            services.AddScoped<IHasher, Hasher>();

            services.AddScoped<IUserService, UserService>();
            services.AddScoped<IJWTTokenService, JWTTokenService>();
            services.AddScoped<IAuthenticationService, AuthenticationService>();
            services.AddScoped<IRegistrationService, RegistrationService>();

            services.AddScoped<IUnitOfWork, UnitOfWork>();

            services.AddControllersWithJsonConfiguration();

            services.AddMassTransit(config => {
                config.UsingRabbitMq((ctx, cfg) => {
                    cfg.Host(configuration["EventBusSettings:HostAddress"]);
                });
            });

            services.AddScoped<IKafkaProducerService, KafkaProducerService>();

            services.AddEndpointsApiExplorer();

            services.ConfigureSwagger();
        }
    }
}
