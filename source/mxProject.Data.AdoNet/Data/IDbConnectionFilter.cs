using System;
using System.Collections.Generic;
using System.Data;
using System.Threading;
using System.Threading.Tasks;

namespace mxProject.Data
{
    /// <summary>
    /// Defines a filter for <see cref="IDbConnection"/> operations.
    /// </summary>
    public interface IDbConnectionFilter
    {
        /// <summary>
        /// Gets the methods to filter.
        /// </summary>
        DbConnectionFilterTargets TargetMethods { get; }

        /// <summary>
        /// Disposes the specified connection.
        /// </summary>
        /// <param name="connection">The database connection to dispose.</param>
        /// <param name="continuation">The continuation action to execute after disposing.</param>
        void Dispose(IDbConnection connection, Action<IDbConnection> continuation);

        /// <summary>
        /// Opens the specified connection.
        /// </summary>
        /// <param name="connection">The database connection to open.</param>
        /// <param name="continuation">The continuation action to execute after opening.</param>
        void Open(IDbConnection connection, Action<IDbConnection> continuation);

        /// <summary>
        /// Closes the specified connection.
        /// </summary>
        /// <param name="connection">The database connection to close.</param>
        /// <param name="continuation">The continuation action to execute after closing.</param>
        void Close(IDbConnection connection, Action<IDbConnection> continuation);

        /// <summary>
        /// Changes the database for the specified connection.
        /// </summary>
        /// <param name="connection">The database connection to change.</param>
        /// <param name="databaseName">The name of the database to switch to.</param>
        /// <param name="continuation">The continuation action to execute after changing the database.</param>
        void ChangeDatabase(IDbConnection connection, string databaseName, Action<IDbConnection, string> continuation);

        /// <summary>
        /// Begins a transaction on the specified connection.
        /// </summary>
        /// <param name="connection">The database connection to begin the transaction on.</param>
        /// <param name="isolationLevel">The isolation level for the transaction.</param>
        /// <param name="continuation">The continuation function to execute after beginning the transaction.</param>
        /// <returns>The transaction object.</returns>
        IDbTransaction BeginTransaction(IDbConnection connection, IsolationLevel isolationLevel, Func<IDbConnection, IsolationLevel, IDbTransaction> continuation);

        /// <summary>
        /// Creates a command for the specified connection.
        /// </summary>
        /// <param name="connection">The database connection to create the command for.</param>
        /// <param name="continuation">The continuation function to execute after creating the command.</param>
        /// <returns>The created database command.</returns>
        IDbCommand CreateCommand(IDbConnection connection, Func<IDbConnection, IDbCommand> continuation);
    }
}
