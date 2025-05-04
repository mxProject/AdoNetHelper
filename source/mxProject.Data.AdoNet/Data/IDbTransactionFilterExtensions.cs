using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Common;
using System.Threading;
using System.Threading.Tasks;
using mxProject.Data.Extensions;
using mxProject.Data.Wrappers;

namespace mxProject.Data
{
    /// <summary>
    /// Provides extension methods for <see cref="IDbTransactionFilter"/> arrays to handle database transaction operations.
    /// </summary>
    public static class IDbTransactionFilterExtensions
    {
        #region Dispose

        /// <summary>
        /// Disposes the specified database transaction using the provided filters.
        /// </summary>
        /// <param name="this">The array of <see cref="IDbTransactionFilter"/>.</param>
        /// <param name="transaction">The database transaction to dispose.</param>
        public static void Dispose(this IDbTransactionFilter[] @this, IDbTransaction transaction)
        {
            if (@this == null || @this.Length == 0)
            {
                transaction.Dispose();
            }
            else
            {
                FilterUtility.InvokeAction(
                    @this,
                    transaction,
                    _transaction => _transaction.Dispose(),
                    (_filter, _transaction, _continuation) => _filter.Dispose(_transaction, _continuation)
                    );
            }
        }

        #endregion

        #region Commit

        /// <summary>
        /// Commits the specified database transaction using the provided filters.
        /// </summary>
        /// <param name="this">The array of <see cref="IDbTransactionFilter"/>.</param>
        /// <param name="transaction">The database transaction to commit.</param>
        public static void Commit(this IDbTransactionFilter[] @this, IDbTransaction transaction)
        {
            if (@this == null || @this.Length == 0)
            {
                transaction.Commit();
            }
            else
            {
                FilterUtility.InvokeAction(
                    @this,
                    transaction,
                    _transaction => _transaction.Commit(),
                    (_filter, _transaction, _continuation) => _filter.Commit(_transaction, _continuation)
                    );
            }
        }

        #endregion

        #region Rollback

        /// <summary>
        /// Rollbacks the specified database transaction using the provided filters.
        /// </summary>
        /// <param name="this">The array of <see cref="IDbTransactionFilter"/>.</param>
        /// <param name="transaction">The database transaction to rollback.</param>
        public static void Rollback(this IDbTransactionFilter[] @this, IDbTransaction transaction)
        {
            if (@this == null || @this.Length == 0)
            {
                transaction.Rollback();
            }
            else
            {
                FilterUtility.InvokeAction(
                    @this,
                    transaction,
                    _transaction => _transaction.Rollback(),
                    (_filter, _transaction, _continuation) => _filter.Rollback(_transaction, _continuation)
                    );
            }
        }

        #endregion
    }
}