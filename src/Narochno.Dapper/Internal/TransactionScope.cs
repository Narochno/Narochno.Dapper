namespace Narochno.Dapper.Internal
{
    public class TransactionScope : ITransaction
    {
        private readonly Session session;

        public TransactionScope(Session session)
        {
            this.session = session;
        }

        public bool Committed { get; set; }

        public void Commit()
        {
            session.CommitTransactionScope(this);
        }

        public void Dispose()
        {
            if (!Committed)
            {
                session.CleanTransactionScope(this);
            }
        }
    }
}