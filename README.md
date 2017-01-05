# Narochno.Data

A Unit of Work pattern for Dapper.
Use database driver that supports ADO.NET.

## Philosophy

* Avoid Heavy ORMs like Entity Framework and NHibernate! 
* Do the leg work of making entities against your tables and care about the data.
* Avoid heavy joins.  Cache reference data and denormalize.
* Cache heavily

## Example Usage

### Implement IDbConnectionFactory

```csharp
serviceCollection.AddData(new NpgsqlConnectionFactory(connectionString));
```

### Create Queries or Commands

```csharp
public class GetAllAddresses : IQuery<IList<Address>>
{
    public IList<Address> Execute(ISession session)
    {
        return session.Query<Address>("select * from address").ToList();
    }
}
```

### Create Repositories

```csharp
public class AddressRepository : IAddressRepository
{
    private readonly IDatabase database;
    private readonly ICacheManager cacheManager;

    public AddressRepository(IDatabase database, ICacheManager cacheManager)
    {
        this.database = database;
        this.cacheManager = cacheManager;
    }

    public async Task<IList<Address>> GetAllAddresses()
    {
        return await cacheManager.Retrieve<IList<Address>, GetAllAddresses>(() => database.Query(new GetAllAddresses()).ActiveFilter()).ConfigureAwait(false);
    }

    public async Task<Address> GetAddressById(long id)
    {
        return (await GetAllAddresses().ConfigureAwait(false)).FirstOrDefault(x => x.Id == id);
    }    
}
```