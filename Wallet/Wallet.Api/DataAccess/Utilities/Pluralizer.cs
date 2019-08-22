using System.Globalization;
using Microsoft.EntityFrameworkCore.Design;

namespace Wallet.Api.DataAccess.Utilities
{
    /// <summary>
    /// Used to singularize the database table names when scaffolding.
    /// </summary>
    public class Pluralizer : IPluralizer
    {
        public string Pluralize(string identifier)
        {
            var inflector = new Inflector.Inflector(CultureInfo.CurrentCulture);
            return inflector.Pluralize(identifier);
        }

        public string Singularize(string identifier)
        {
            var inflector = new Inflector.Inflector(CultureInfo.CurrentCulture);
            return inflector.Singularize(identifier);
        }
    }
}
