using System.Data;

namespace Narochno.Dapper.Internal
{
    public class Transaction : ITransaction
    {
        private readonly IDbTransaction transaction;
        private bool committed;

        public Transaction(IDbTransaction transaction)
        {
            this.transaction = transaction;
        }

        public void Dispose()
        {
            if (!committed)
            {
                transaction.Rollback();
            }
        }

        public void Commit()
        {
            transaction.Commit();
            committed = true;
        }
    }
}