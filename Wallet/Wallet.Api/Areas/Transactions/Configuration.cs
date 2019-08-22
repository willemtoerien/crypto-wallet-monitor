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
using Wallet.Api.Areas.Transactions.Services;
using Wallet.Api.Areas.Users.Services;

namespace Wallet.Api.Areas.Transactions
{
    public static class Configuration
    {
        public static IServiceCollection AddTransactions(this IServiceCollection services, IConfiguration configuration)
        {
            var section = configuration.GetSection("Areas:Transactions");
            services
                .Configure<TransactionsOptions>(section)
                .AddTransient<TransactionsService>();
            return services;
        }
    }
}
