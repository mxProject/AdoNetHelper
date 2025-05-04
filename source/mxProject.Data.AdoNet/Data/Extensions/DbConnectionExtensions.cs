using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Common;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using mxProject.Data.Wrappers;

namespace mxProject.Data.Extensions
{
    /// <summary>
    /// Provides extension methods for <see cref="IDbConnection"/>.
    /// </summary>
    public static class DbConnectionExtensions
    {
        #region async support

        // https://github.com/dotnet/runtime/blob/main/src/libraries/System.Data.Common/src/System/Data/Common/DbConnection.cs

        /// <summary>
        /// Asynchronously disposes the connection.
        /// </summary>
        /// <param name="this">The database connection.</param>
        /// <returns>A <see cref="ValueTask"/> representing the asynchronous operation.</returns>
        public static ValueTask DisposeAsync(this IDbConnection @this)
        {
#if NETSTANDARD2_1_OR_GREATER
                if (@this is DbConnection connection)
                {
                    return connection.DisposeAsync();
                }
#endif

            return AsyncUtility.ActionAsyncAsValueTask(@this, x => x.Dispose());
        }

        /// <summary>
        /// Asynchronously opens the connection.
        /// </summary>
        /// <param name="this">The database connection.</param>
        /// <param name="cancellationToken">A token to monitor for cancellation requests.</param>
        /// <returns>A <see cref="Task"/> representing the asynchronous operation.</returns>
        public static Task OpenAsync(this IDbConnection @this, CancellationToken cancellationToken = default)
        {
            if (@this is DbConnection connection)
            {
                return connection.OpenAsync(cancellationToken);
            }

            return AsyncUtility.ActionAsync(@this, x => x.Open(), cancellationToken);
        }

        /// <summary>
        /// Asynchronously closes the connection.
        /// </summary>
        /// <param name="this">The database connection.</param>
        /// <returns>A <see cref="Task"/> representing the asynchronous operation.</returns>
        public static Task CloseAsync(this IDbConnection @this)
        {
#if NETSTANDARD2_1_OR_GREATER
                if (@this is DbConnection connection)
                {
                    return connection.CloseAsync();
                }
#endif

            return AsyncUtility.ActionAsync(@this, x => x.Close());
        }

        /// <summary>
        /// Asynchronously changes the database for the connection.
        /// </summary>
        /// <param name="this">The database connection.</param>
        /// <param name="databaseName">The name of the database to use.</param>
        /// <param name="cancellationToken">A token to monitor for cancellation requests.</param>
        /// <returns>A <see cref="Task"/> representing the asynchronous operation.</returns>
        public static Task ChangeDatabaseAsync(this IDbConnection @this, string databaseName, CancellationToken cancellationToken = default)
        {
#if NETSTANDARD2_1_OR_GREATER
                if (@this is DbConnection connection)
                {
                    return connection.ChangeDatabaseAsync(databaseName, cancellationToken);
                }
#endif

            return AsyncUtility.ActionAsync(
                (@this, databaseName),
                x => x.@this.ChangeDatabase(x.databaseName),
                cancellationToken
                );
        }

        /// <summary>
        /// Asynchronously begins a database transaction.
        /// </summary>
        /// <param name="this">The database connection.</param>
        /// <param name="isolationLevel">The isolation level for the transaction.</param>
        /// <param name="cancellationToken">A token to monitor for cancellation requests.</param>
        /// <returns>A <see cref="ValueTask{TResult}"/> representing the asynchronous operation, with the result being the transaction.</returns>
        public static ValueTask<IDbTransaction> BeginTransactionAsync(this IDbConnection @this, IsolationLevel isolationLevel = IsolationLevel.Unspecified, CancellationToken cancellationToken = default)
        {
#if NETSTANDARD2_1_OR_GREATER
                if (@this is DbConnection connection)
                {
                    return connection.BeginTransactionAsync(cancellationToken);
                }
#endif

            return AsyncUtility.FuncAsyncAsValueTask(
                (@this, isolationLevel),
                x => x.@this.BeginTransaction(x.isolationLevel),
                cancellationToken
                );
        }

        #endregion

        #region OpenIfClosed

        /// <summary>
        /// Determines whether the connection is currently open.
        /// </summary>
        /// <param name="this">The database connection.</param>
        /// <returns><c>true</c> if the connection is open; otherwise, <c>false</c>.</returns>
        public static bool IsOpened(this IDbConnection @this)
        {
            return (@this.State & ConnectionState.Open) > 0;
        }

        /// <summary>
        /// Opens the connection if it is not already open.
        /// </summary>
        /// <param name="this">The database connection.</param>
        /// <returns><c>true</c> if the connection was opened; <c>false</c> if it was already open.</returns>
        public static bool OpenIfClosed(this IDbConnection @this)
        {
            if (@this.IsOpened()) { return false; }

            @this.Open();

            return true;
        }

        /// <summary>
        /// Asynchronously opens the connection if it is not already open.
        /// </summary>
        /// <param name="this">The database connection.</param>
        /// <returns>A <see cref="ValueTask{TResult}"/> representing the asynchronous operation, with the result being <c>true</c> if the connection was opened; <c>false</c> if it was already open.</returns>
        public static async ValueTask<bool> OpenIfClosedAsync(this IDbConnection @this)
        {
            if (@this.IsOpened()) { return false; }

            await @this.OpenAsync().ConfigureAwait(false);

            return true;
        }

        #endregion

        #region Cast

        /// <summary>
        /// Casts the current connection to the specified type and performs the specified action.
        /// </summary>
        /// <typeparam name="TConnection">The type to cast the connection to.</typeparam>
        /// <param name="this">The database connection.</param>
        /// <param name="action">The action to perform on the casted connection.</param>
        /// <exception cref="InvalidCastException">Thrown if the connection cannot be cast to the specified type.</exception>
        public static void Cast<TConnection>(this IDbConnection @this, Action<TConnection> action) where TConnection : IDbConnection
        {
            var connection = @this.Find<TConnection>();

            if (connection == null) { throw new InvalidCastException($"The connection is not of type {typeof(TConnection).Name}."); }

            action(connection);
        }

        /// <summary>
        /// Casts the current connection to the specified type and applies the specified function.
        /// </summary>
        /// <typeparam name="TConnection">The type to cast the connection to.</typeparam>
        /// <typeparam name="TResult">The type of the result returned by the function.</typeparam>
        /// <param name="this">The database connection.</param>
        /// <param name="func">The function to apply to the casted connection.</param>
        /// <returns>The result of the function applied to the casted connection.</returns>
        /// <exception cref="InvalidCastException">Thrown if the connection cannot be cast to the specified type.</exception>
        public static TResult Cast<TConnection, TResult>(this IDbConnection @this, Func<TConnection, TResult> func) where TConnection : IDbConnection
        {
            var connection = @this.Find<TConnection>();

            if (connection == null) { throw new InvalidCastException($"The connection is not of type {typeof(TConnection).Name}."); }

            return func(connection);
        }

        /// <summary>
        /// Finds the connection of the specified type.
        /// </summary>
        /// <typeparam name="TConnection">The type of the connection to find.</typeparam>
        /// <param name="this">The database connection.</param>
        /// <returns>The connection of the specified type, or <c>null</c> if not found.</returns>
        private static TConnection? Find<TConnection>(this IDbConnection @this) where TConnection : IDbConnection
        {
            if (@this is TConnection connection)
            {
                return connection;
            }

            if (@this is IDbConnectionWrapper wrapper)
            {
                return wrapper.WrappedConnection.Find<TConnection>();
            }

            return default;
        }

        #endregion
    }
}
