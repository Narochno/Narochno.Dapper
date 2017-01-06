using System.Collections.Generic;

namespace Narochno.Dapper
{
    public interface ISession
    {
        ITransaction BeginTransaction();
        IEnumerable<T> Query<T>(string query, object param = null);
        void Execute(string query, object param = null);
        T Get<T>(string query, object param);
    }
}