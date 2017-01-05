using System.Data;

namespace Narochno.Data
{
    public interface IDbConnectionFactory
    {
        IDbConnection NewConnection();
    }
}