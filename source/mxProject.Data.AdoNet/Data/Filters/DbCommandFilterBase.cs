using System;
using System.Collections.Generic;
using System.Data;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace mxProject.Data.Filters
{
    /// <summary>
    /// Base class for implementing <see cref="IDbCommandFilter"/> to filter database command operations.
    /// </summary>
    public abstract class DbCommandFilterBase : IDbCommandFilter
    {
        /// <summary>
        /// Creates a new instance of the <see cref="DbCommandFilterBase"/> class.
        /// </summary>
        protected DbCommandFilterBase()
        {
        }

        /// <inheritdoc/>
        public virtual DbCommandFilterTargets TargetMethods => DbCommandFilterTargets.None;

        /// <inheritdoc/>
        public virtual void Cancel(IDbCommand command, Action<IDbCommand> continuation)
        {
            continuation(command);
        }

        /// <inheritdoc/>
        public virtual IDbDataParameter CreateParameter(IDbCommand command, Func<IDbCommand, IDbDataParameter> continuation)
        {
            return continuation(command);
        }

        /// <inheritdoc/>
        public virtual void Dispose(IDbCommand command, Action<IDbCommand> continuation)
        {
            continuation(command);
        }

        /// <inheritdoc/>
        public virtual int ExecuteNonQuery(IDbCommand command, Func<IDbCommand, int> continuation)
        {
            return continuation(command);
        }

        /// <inheritdoc/>
        public virtual IDataReader ExecuteReader(IDbCommand command, CommandBehavior behavior, Func<IDbCommand, CommandBehavior, IDataReader> continuation)
        {
            return continuation(command, behavior);
        }

        /// <inheritdoc/>
        public virtual object ExecuteScalar(IDbCommand command, Func<IDbCommand, object> continuation)
        {
            return continuation(command);
        }

        /// <inheritdoc/>
        public virtual void Prepare(IDbCommand command, Action<IDbCommand> continuation)
        {
            continuation(command);
        }
    }
}
