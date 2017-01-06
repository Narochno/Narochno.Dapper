using Microsoft.Extensions.DependencyInjection;
using Narochno.Dapper.Internal;

namespace Narochno.Dapper
{
    public static class NarochnoDapperServiceCollectionExtensions
    {
        public static void AddNarochnoDapper(this IServiceCollection serviceCollection, IDbConnectionFactory dbConnectionFactory)
        {
            serviceCollection.AddScoped<ISession, Session>();
            serviceCollection.AddTransient<IDatabase, Database>();
            serviceCollection.AddSingleton(dbConnectionFactory);
        }
    }
}
