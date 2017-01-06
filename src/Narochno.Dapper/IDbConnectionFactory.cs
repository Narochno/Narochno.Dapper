using System.Data;

namespace Narochno.Dapper
{
    public interface IDbConnectionFactory
    {
        IDbConnection NewConnection();
    }
}