using Microsoft.Extensions.DependencyInjection;
using Narochno.Dapper.Internal;

namespace Narochno.Dapper
{
    public static class IServiceCollectionExtensions
    {
        public static void AddData(this IServiceCollection serviceCollection, IDbConnectionFactory dbConnectionFactory)
        {
            serviceCollection.AddScoped<ISession, Session>();
            serviceCollection.AddTransient<ITransaction, Transaction>();
            serviceCollection.AddTransient<IDatabase, Database>();
            serviceCollection.AddSingleton(dbConnectionFactory);
        }
    }
}
