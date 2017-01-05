using Microsoft.Extensions.DependencyInjection;
using Narochno.Data.Internal;

namespace Narochno.Data
{
    public static class IServiceCollectionExtensions
    {
        public static void AddVisibilityData(this IServiceCollection serviceCollection, IDbConnectionFactory dbConnectionFactory)
        {
            serviceCollection.AddScoped<ISession, Session>();
            serviceCollection.AddTransient<ITransaction, Transaction>();
            serviceCollection.AddTransient<IDatabase, Database>();
            serviceCollection.AddSingleton(dbConnectionFactory);
        }
    }
}
