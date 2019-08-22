using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace Wallet.Api.DataAccess
{
    public static class Configuration
    {
        public static IServiceCollection AddDataAccess(this IServiceCollection services, IConfiguration configuration)
        {
            services
                .AddDbContext<WalletDbContext>(options =>
                    options.UseSqlServer(configuration.GetConnectionString("Wallet")));
            return services;
        }
    }
}
