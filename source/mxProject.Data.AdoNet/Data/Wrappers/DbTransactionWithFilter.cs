using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using mxProject.Data.Filters;

namespace mxProject.Data.Wrappers
{
    /// <summary>
    /// Represents a database transaction with additional filtering capabilities.
    /// </summary>
    internal sealed class DbTransactionWithFilter : IDbTransaction, IDbTransactionWrapper
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="DbTransactionWithFilter"/> class.
        /// </summary>
        /// <param name="connection">The database connection associated with the transaction.</param>
        /// <param name="transaction">The underlying database transaction.</param>
        /// <param name="filters">The collection of transaction filters to apply.</param>
        internal DbTransactionWithFilter(DbConnectionWithFilter connection, IDbTransaction transaction, DbFilterSet<DbTransactionFilterTargets, IDbTransactionFilter> filters)
        {
            ConnectionWrapper = connection;
            WrappedTransaction = transaction;

            m_TransactionFilters = filters;
        }

        #region connection

        /// <summary>
        /// Gets the database connection associated with this transaction.
        /// </summary>
        internal DbConnectionWithFilter ConnectionWrapper { get; }

        #endregion

        #region transaction

        /// <summary>
        /// Gets the underlying database transaction.
        /// </summary>
        internal IDbTransaction WrappedTransaction { get; }

        IDbTransaction IDbTransactionWrapper.WrappedTransaction => WrappedTransaction;

        #endregion

        #region filter

        private readonly DbFilterSet<DbTransactionFilterTargets, IDbTransactionFilter> m_TransactionFilters;

        #endregion

        /// <inheritdoc/>
        public IDbConnection Connection => ConnectionWrapper;

        /// <inheritdoc/>
        public IsolationLevel IsolationLevel => WrappedTransaction.IsolationLevel;

        /// <inheritdoc/>
        public void Commit()
        {
            m_TransactionFilters[DbTransactionFilterTargets.Commmit].Commit(WrappedTransaction);

            ConnectionWrapper.OnEndTransaction();
        }

        /// <inheritdoc/>
        public void Dispose()
        {
            m_TransactionFilters[DbTransactionFilterTargets.Dispose].Dispose(WrappedTransaction);

            ConnectionWrapper.OnEndTransaction();
        }

        /// <inheritdoc/>
        public void Rollback()
        {
            m_TransactionFilters[DbTransactionFilterTargets.Rollback].Rollback(WrappedTransaction);

            ConnectionWrapper.OnEndTransaction();
        }
    }
}
