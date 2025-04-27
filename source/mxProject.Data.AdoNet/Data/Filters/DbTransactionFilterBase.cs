using System;
using System.Collections.Generic;
using System.Data;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace mxProject.Data.Filters
{
    /// <summary>
    /// Base class for implementing <see cref="IDbTransactionFilter"/> to filter database transaction operations.
    /// </summary>
    public abstract class DbTransactionFilterBase : IDbTransactionFilter
    {
        /// <summary>
        /// Creates a new instance of the <see cref="DbTransactionFilterBase"/> class.
        /// </summary>
        protected DbTransactionFilterBase()
        {
        }

        /// <inheritdoc/>
        public virtual DbTransactionFilterTargets TargetMethods => DbTransactionFilterTargets.None;

        /// <inheritdoc/>
        public virtual void Commit(IDbTransaction transaction, Action<IDbTransaction> continuation)
        {
            continuation(transaction);
        }

        /// <inheritdoc/>
        public virtual void Dispose(IDbTransaction transaction, Action<IDbTransaction> continuation)
        {
            continuation(transaction);
        }

        /// <inheritdoc/>
        public virtual void Rollback(IDbTransaction transaction, Action<IDbTransaction> continuation)
        {
            continuation(transaction);
        }
    }
}
