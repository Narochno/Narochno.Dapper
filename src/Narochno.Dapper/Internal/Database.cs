namespace Narochno.Dapper.Internal
{
    public class Database : IDatabase
    {
        private ISession _session { get; set; }

        public Database(ISession session)
        {
            _session = session;
        }

        public T Query<T>(IQuery<T> query)
        {
            var result = query.Execute(_session);
            return result;
        }

        public void Execute(ICommand command)
        {
            command.Execute(_session);
        }
    }
}