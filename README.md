# Narochno.Dapper

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
        //using Dapper here!
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

    public IList<Address> GetAllAddresses()
    {
        //TODO use cacheManager!
        return database.Query(new GetAllAddresses());
    }

    public Address GetAddressById(long id)
    {
        return GetAllAddresses().FirstOrDefault(x => x.Id == id);
    }    
}
```

### Transactions

I try to avoid transactions by just making inserts/updates not dependant on other inserts/updates but it's unavoidable.

This supports nesting `BeginTransaction` by using a stacks so your calls to other repositories don't need to know or care about transactions.  A commit only occurs on the final commit call.  The using block will auto rollback if commit isn't called.

```csharp
using (var transaction = session.BeginTransaction())
{
    ...
    transaction.Commit();
}
```