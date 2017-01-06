using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using Dapper;
using Microsoft.Extensions.Logging;

namespace Narochno.Dapper.Internal
{
    public class Session : ISession, IDisposable
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

        private readonly Stack<TransactionScope> scopes = new Stack<TransactionScope>();

        private readonly ILogger<Session> logger;
        private readonly IDbConnectionFactory dbConnectionFactory;
        private IDbConnection connection;

        public Session(ILogger<Session> logger, IDbConnectionFactory dbConnectionFactory)
        {
            this.logger = logger;
            this.dbConnectionFactory = dbConnectionFactory;
        }

        public ITransaction BeginTransaction()
        {
            if (Transaction == null)
            {
                Transaction = Connection.BeginTransaction();
            }
            var scope = new TransactionScope(this);
            scopes.Push(scope);
            return scope;
        }

        public void CommitTransactionScope(TransactionScope scope)
        {
            var s = scopes.Pop();
            if (s != scope)
            {
                throw new InvalidOperationException("Inconsistent scope");
            }
            //none left? commit!
            if (!scopes.Any())
            {
                if (Transaction == null)
                {
                    throw new InvalidOperationException("Cannot commit rollback.");
                }
                Transaction.Commit();
                Transaction.Dispose();
                Transaction = null;
            }
            scope.Committed = true;
        }

        public void CleanTransactionScope(TransactionScope scope)
        {
            var s = scopes.Pop();
            if (s != scope)
            {
                throw new InvalidOperationException("Inconsistent scope");
            }
            if (!scope.Committed)
            {
                Transaction.Rollback();
                Transaction.Dispose();
                Transaction = null;
                logger.LogWarning("Uncommited scope processed.");
            }
        }

        public IDbTransaction Transaction { get; private set; }

        public IDbConnection Connection
        {
            get
            {
                if (connection == null)
                {
                    connection = dbConnectionFactory.NewConnection();
                }

                if (connection.State == ConnectionState.Closed)
                {
                    connection.Open();
                }

                return connection;
            }
        }
        
        public void Dispose()
        {
            if (Transaction != null)
            {
                Transaction.Dispose();
                Transaction = null;
            }

            if (connection != null && connection.State != ConnectionState.Closed)
            {
                connection.Close();
                connection = null;
            }
        }

        public IEnumerable<T> Query<T>(string query, object param)
        {
            if (logger.IsEnabled(LogLevel.Debug))
            {
                logger.LogDebug($"Type: {typeof(T).FullName}Query: {query}");
            }
            return Connection.Query<T>(query, param, Transaction);
        }

        public T Get<T>(string query, object param)
        {
            if (logger.IsEnabled(LogLevel.Debug))
            {
                logger.LogDebug($"Type: {typeof(T).FullName} Get: {query}");
            }
            return Connection.QuerySingle<T>(query, param, Transaction);
        }

        public void Execute(string sql, object param)
        {
            if (logger.IsEnabled(LogLevel.Debug))
            {
                logger.LogDebug($"Execute: {sql}");
            }
            Connection.Execute(sql, param, Transaction);
        }
    }
}