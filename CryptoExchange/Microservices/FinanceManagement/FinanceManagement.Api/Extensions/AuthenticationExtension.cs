using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FinanceManagement.Api.Extensions
{
    public static class AuthenticationExtension
    {
        public static void AddJWTAuthentication(
            this IServiceCollection services,
            ConfigurationManager configuration)
        {
            services.AddAuthentication(k =>
            {
                k.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
                k.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
            }).AddJWTBearer(configuration);
        }

        private static void AddJWTBearer(
            this AuthenticationBuilder authenticationBuilder,
            ConfigurationManager configuration)
        {
            authenticationBuilder.AddJwtBearer(p =>
             {
                 var key = Encoding.UTF8.GetBytes(configuration["JWTToken:key"]);
                 p.SaveToken = true;
                 p.TokenValidationParameters = new TokenValidationParameters
                 {
                     ValidateIssuer = false,
                     ValidateAudience = false,
                     ValidateLifetime = false,
                     ValidateIssuerSigningKey = true,
                     ValidIssuer = configuration["JWTToken:key"],
                     ValidAudience = configuration["JWTToken:key"],
                     IssuerSigningKey = new SymmetricSecurityKey(key)
                 };
             });
        }
    }
}
