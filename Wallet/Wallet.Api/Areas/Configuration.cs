using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Wallet.Api.Areas.Transactions;
using Wallet.Api.Areas.Users;

// ReSharper disable once CheckNamespace - Ignore for configurations.
namespace VirtualAssessors.Api.Areas
{
    public static class Configuration
    {
        public static IServiceCollection AddAreas(this IServiceCollection services, IConfiguration configuration)
        {
            services
                .AddUsers(configuration)
                .AddTransactions(configuration);
            return services;
        }

        public static IApplicationBuilder UseAreas(this IApplicationBuilder builder)
        {
            builder.UseUsers();
            return builder;
        }
    }
}
