using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.IdentityModel.Tokens;
using Wallet.Api.Areas.Userss.Services;

namespace Wallet.Api.Areas.Users
{
    public static class Configuration
    {
        public static IServiceCollection AddUsers(this IServiceCollection services, IConfiguration configuration)
        {
            var usersOptions = new UsersOptions();
            var section = configuration.GetSection("Areas:Users");
            section.Bind(usersOptions);
            services.AddAuthentication(options =>
            {
                options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
                options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
            })
                .AddJwtBearer(options =>
                {
                    options.SaveToken = true;
                    options.RequireHttpsMetadata = false;
                    options.TokenValidationParameters = new TokenValidationParameters
                    {
                        ValidateIssuerSigningKey = true,
                        IssuerSigningKey =
                            new SymmetricSecurityKey(Encoding.ASCII.GetBytes(usersOptions.AuthenticationTokenKey)),
                        ValidateIssuer = false,
                        ValidateAudience = false
                    };
                });
            services
                .Configure<UsersOptions>(section)
                .AddTransient<Authenticator>()
                .AddTransient<PasswordHasher>()
                .AddTransient<TokenProvider>();
            return services;
        }

        public static IApplicationBuilder UseUsers(this IApplicationBuilder builder)
        {
            builder.UseAuthentication();
            return builder;
        }
    }
}
