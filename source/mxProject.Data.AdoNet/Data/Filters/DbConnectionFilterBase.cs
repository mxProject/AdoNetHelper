using System;
using System.Collections.Generic;
using System.Data;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace mxProject.Data.Filters
{
    /// <summary>
    /// Base class for implementing <see cref="IDbConnectionFilter"/> to filter database connection operations.
    /// </summary>
    public abstract class DbConnectionFilterBase : IDbConnectionFilter
    {
        /// <summary>
        /// Creates a new instance of the <see cref="DbConnectionFilterBase"/> class.
        /// </summary>
        protected DbConnectionFilterBase()
        {
        }

        /// <inheritdoc/>
        public virtual DbConnectionFilterTargets TargetMethods => DbConnectionFilterTargets.None;

        /// <inheritdoc/>
        public virtual IDbTransaction BeginTransaction(IDbConnection connection, IsolationLevel isolationLevel, Func<IDbConnection, IsolationLevel, IDbTransaction> continuation)
        {
            return continuation(connection, isolationLevel);
        }

        /// <inheritdoc/>
        public virtual void ChangeDatabase(IDbConnection connection, string databaseName, Action<IDbConnection, string> continuation)
        {
            continuation(connection, databaseName);
        }

        /// <inheritdoc/>
        public virtual void Close(IDbConnection connection, Action<IDbConnection> continuation)
        {
            continuation(connection);
        }

        /// <inheritdoc/>
        public virtual IDbCommand CreateCommand(IDbConnection connection, Func<IDbConnection, IDbCommand> continuation)
        {
            return continuation(connection);
        }

        /// <inheritdoc/>
        public virtual void Dispose(IDbConnection connection, Action<IDbConnection> continuation)
        {
            continuation(connection);
        }

        /// <inheritdoc/>
        public virtual void Open(IDbConnection connection, Action<IDbConnection> continuation)
        {
            continuation(connection);
        }
    }
}
