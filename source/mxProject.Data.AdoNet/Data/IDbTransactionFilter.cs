using System;
using System.Collections.Generic;
using System.Data;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace mxProject.Data
{
    /// <summary>
    /// Represents a filter for database transactions.
    /// </summary>
    public interface IDbTransactionFilter
    {
        /// <summary>
        /// Gets the methods to filter.
        /// </summary>
        DbTransactionFilterTargets TargetMethods { get; }

        /// <summary>
        /// Disposes the specified transaction.
        /// </summary>
        /// <param name="transaction">The database transaction to dispose.</param>
        /// <param name="continuation">The continuation action to execute after disposing.</param>
        void Dispose(IDbTransaction transaction, Action<IDbTransaction> continuation);

        /// <summary>
        /// Commits the specified transaction.
        /// </summary>
        /// <param name="transaction">The database transaction to commit.</param>
        /// <param name="continuation">The continuation action to execute after committing.</param>
        void Commit(IDbTransaction transaction, Action<IDbTransaction> continuation);

        /// <summary>
        /// Rollbacks back the specified transaction.
        /// </summary>
        /// <param name="transaction">The database transaction to rollback.</param>
        /// <param name="continuation">The continuation action to execute after rolling back.</param>
        void Rollback(IDbTransaction transaction, Action<IDbTransaction> continuation);
    }
}
