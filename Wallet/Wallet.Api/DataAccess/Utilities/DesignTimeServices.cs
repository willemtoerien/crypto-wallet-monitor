using Microsoft.EntityFrameworkCore.Design;
using Microsoft.Extensions.DependencyInjection;

namespace Wallet.Api.DataAccess.Utilities
{
    public class DesignTimeServices : IDesignTimeServices
    {
        public void ConfigureDesignTimeServices(IServiceCollection serviceCollection)
        {
            serviceCollection.AddSingleton<IPluralizer, Pluralizer>();
        }
    }
}
