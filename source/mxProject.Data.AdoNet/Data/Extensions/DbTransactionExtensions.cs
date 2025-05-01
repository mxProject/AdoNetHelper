using System;
using System.Collections.Generic;
using System.Data;
using System.Threading;
using System.Threading.Tasks;

namespace mxProject.Data.Extensions
{
    /// <summary>  
    /// Provides extension methods for <see cref="IDbTransaction"/>.  
    /// </summary>  
    public static class DbTransactionExtensions
    {
        #region async support  

        // https://github.com/dotnet/runtime/blob/main/src/libraries/System.Data.Common/src/System/Data/Common/DbTransaction.cs  

        /// <summary>  
        /// Disposes the transaction asynchronously.  
        /// </summary>  
        /// <param name="this">The transaction to dispose.</param>  
        /// <returns>A task that represents the asynchronous dispose operation.</returns>  
        public static ValueTask DisposeAsync(this IDbTransaction @this)
        {
#if NETSTANDARD2_1_OR_GREATER
               if (@this is DbTransaction transaction)  
               {  
                   return transaction.DisposeAsync();  
               }  
#endif

            return AsyncUtility.ActionAsyncAsValueTask(@this, x => x.Dispose());
        }

        /// <summary>  
        /// Commits the transaction asynchronously.  
        /// </summary>  
        /// <param name="this">The transaction to commit.</param>  
        /// <param name="cancellation">A token to monitor for cancellation requests.</param>  
        /// <returns>A task that represents the asynchronous commit operation.</returns>  
        public static Task CommitAsync(this IDbTransaction @this, CancellationToken cancellation = default)
        {
#if NETSTANDARD2_1_OR_GREATER
               if (@this is DbTransaction transaction)  
               {  
                   return transaction.CommitAsync(cancellation);  
               }  
#endif

            return AsyncUtility.ActionAsync(@this, x => x.Commit(), cancellation);
        }

        /// <summary>  
        /// Rolls back the transaction asynchronously.  
        /// </summary>  
        /// <param name="this">The transaction to roll back.</param>  
        /// <param name="cancellation">A token to monitor for cancellation requests.</param>  
        /// <returns>A task that represents the asynchronous rollback operation.</returns>  
        public static Task RollbackAsync(this IDbTransaction @this, CancellationToken cancellation = default)
        {
#if NETSTANDARD2_1_OR_GREATER
               if (@this is DbTransaction transaction)  
               {  
                   return transaction.RollbackAsync(cancellation);  
               }  
#endif

            return AsyncUtility.ActionAsync(@this, x => x.Rollback(), cancellation);
        }

        #endregion

        #region Cast

        /// <summary>
        /// Casts the current transaction to the specified type and performs the given action.
        /// </summary>
        /// <typeparam name="TTransaction">The type to cast the transaction to.</typeparam>
        /// <param name="this">The current transaction.</param>
        /// <param name="action">The action to perform on the casted transaction.</param>
        /// <exception cref="InvalidCastException">Thrown if the transaction is not of the specified type.</exception>
        public static void Cast<TTransaction>(this IDbTransaction @this, Action<TTransaction> action) where TTransaction : IDbTransaction
        {
            var transaction = @this.Find<TTransaction>();

            if (transaction == null) { throw new InvalidCastException($"The transaction is not of type {typeof(TTransaction).Name}."); }

            action(transaction);
        }

        /// <summary>
        /// Casts the current transaction to the specified type and executes the given function.
        /// </summary>
        /// <typeparam name="TTransaction">The type to cast the transaction to.</typeparam>
        /// <typeparam name="TResult">The return type of the function.</typeparam>
        /// <param name="this">The current transaction.</param>
        /// <param name="func">The function to execute on the casted transaction.</param>
        /// <returns>The result of the function.</returns>
        /// <exception cref="InvalidCastException">Thrown if the transaction is not of the specified type.</exception>
        public static TResult Cast<TTransaction, TResult>(this IDbTransaction @this, Func<TTransaction, TResult> func) where TTransaction : IDbTransaction
        {
            var transaction = @this.Find<TTransaction>();

            if (transaction == null) { throw new InvalidCastException($"The transaction is not of type {typeof(TTransaction).Name}."); }

            return func(transaction);
        }

        /// <summary>
        /// Finds the transaction of the specified type.
        /// </summary>
        /// <typeparam name="TTransaction">The type of the transaction to find.</typeparam>
        /// <param name="this">The database transaction.</param>
        /// <returns>The transaction of the specified type, or <c>null</c> if not found.</returns>
        private static TTransaction? Find<TTransaction>(this IDbTransaction @this) where TTransaction : IDbTransaction
        {
            if (@this is TTransaction transaction)
            {
                return transaction;
            }

            if (@this is IDbTransactionWrapper wrapper)
            {
                return wrapper.WrappedTransaction.Find<TTransaction>();
            }

            return default;
        }

        #endregion
    }
}
