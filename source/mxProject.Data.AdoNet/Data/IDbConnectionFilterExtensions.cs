using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Common;
using System.Threading;
using System.Threading.Tasks;
using mxProject.Data.Extensions;
using mxProject.Data.Wrappers;
using mxProject.Data.Filters;

namespace mxProject.Data
{
    /// <summary>
    /// Provides extension methods for <see cref="IDbConnectionFilter"/> arrays to handle database connection operations.
    /// </summary>
    public static class IDbConnectionFilterExtensions
    {
        #region wrap connection

        /// <summary>
        /// Wraps the specified database connection with the provided filters.
        /// </summary>
        /// <param name="this">The database connection to wrap.</param>
        /// <param name="connectionFilters">The connection filters to apply.</param>
        /// <param name="transactionFilters">The transaction filters to apply.</param>
        /// <param name="commandFilters">The command filters to apply.</param>
        /// <param name="parametersFilters">The parameter collection filters to apply.</param>
        /// <param name="readerFilters">The datareader filters to apply.</param>
        /// <returns>A wrapped database connection.</returns>
        public static IDbConnection WithFilter(
            this IDbConnection @this,
            IEnumerable<IDbConnectionFilter>? connectionFilters = null,
            IEnumerable<IDbTransactionFilter>? transactionFilters = null,
            IEnumerable<IDbCommandFilter>? commandFilters = null,
            IEnumerable<IDataParameterCollectionFilter>? parametersFilters = null,
            IEnumerable<IDataReaderFilter>? readerFilters = null
            )
        {
            return WithFilter(
                @this,
                connectionFilters == null ? null : DbFilterSet.Create(connectionFilters, x => x.TargetMethods),
                transactionFilters == null ? null : DbFilterSet.Create(transactionFilters, x => x.TargetMethods),
                commandFilters == null ? null : DbFilterSet.Create(commandFilters, x => x.TargetMethods),
                parametersFilters == null ? null : DbFilterSet.Create(parametersFilters, x => x.TargetMethods),
                readerFilters == null ? null : DbFilterSet.Create(readerFilters, x => x.TargetMethods)
                );
        }

        /// <summary>
        /// Wraps the specified database connection with the provided filters.
        /// </summary>
        /// <param name="this">The database connection to wrap.</param>
        /// <param name="connectionFilters">The connection filters to apply.</param>
        /// <param name="transactionFilters">The transaction filters to apply.</param>
        /// <param name="commandFilters">The command filters to apply.</param>
        /// <param name="parametersFilters">The parameter collection filters to apply.</param>
        /// <param name="readerFilters">The datareader filters to apply.</param>
        /// <returns>A wrapped database connection.</returns>
        internal static IDbConnection WithFilter(
            this IDbConnection @this,
            DbFilterSet<DbConnectionFilterTargets, IDbConnectionFilter>? connectionFilters,
            DbFilterSet<DbTransactionFilterTargets, IDbTransactionFilter>? transactionFilters,
            DbFilterSet<DbCommandFilterTargets, IDbCommandFilter>? commandFilters,
            DbFilterSet<DataParameterCollectionFilterTargets, IDataParameterCollectionFilter>? parametersFilters,
            DbFilterSet<DataReaderFilterTargets, IDataReaderFilter>? readerFilters
            )
        {
            return new DbConnectionWithFilter(
                @this,
                connectionFilters ?? DbFilterSet.EmptyConnectionFilters,
                transactionFilters ?? DbFilterSet.EmptyTransactionFilters,
                commandFilters ?? DbFilterSet.EmptyCommandFilters,
                parametersFilters ?? DbFilterSet.EmptyParametersFilters,
                readerFilters ?? DbFilterSet.EmptyReaderFilters
                );
        }

        #endregion

        #region Dispose

        /// <summary>
        /// Disposes the specified database connection using the provided filters.
        /// </summary>
        /// <param name="this">The array of <see cref="IDbConnectionFilter"/>.</param>
        /// <param name="connection">The database connection to dispose.</param>
        public static void Dispose(this IDbConnectionFilter[] @this, IDbConnection connection)
        {
            if (@this == null || @this.Length == 0)
            {
                connection.Dispose();
            }
            else
            {
                FilterUtility.InvokeAction(
                    @this,
                    connection,
                    _connection => _connection.Dispose(),
                    (_filter, _connection, _continuation) => _filter.Dispose(_connection, _continuation)
                    );
            }
        }

        #endregion

        #region Open

        /// <summary>
        /// Opens the specified database connection using the provided filters.
        /// </summary>
        /// <param name="this">The array of <see cref="IDbConnectionFilter"/>.</param>
        /// <param name="connection">The database connection to open.</param>
        public static void Open(this IDbConnectionFilter[] @this, IDbConnection connection)
        {
            if (@this == null || @this.Length == 0)
            {
                connection.Open();
            }
            else
            {
                FilterUtility.InvokeAction(
                    @this,
                    connection,
                    _connection => _connection.Open(),
                    (_filter, _connection, _continuation) => _filter.Open(_connection, _continuation)
                    );
            }
        }

        #endregion

        #region Close

        /// <summary>
        /// Closes the specified database connection using the provided filters.
        /// </summary>
        /// <param name="this">The array of <see cref="IDbConnectionFilter"/>.</param>
        /// <param name="connection">The database connection to close.</param>
        public static void Close(this IDbConnectionFilter[] @this, IDbConnection connection)
        {
            if (@this == null || @this.Length == 0)
            {
                connection.Close();
            }
            else
            {
                FilterUtility.InvokeAction(
                    @this,
                    connection,
                    _connection => _connection.Close(),
                    (_filter, _connection, _continuation) => _filter.Close(_connection, _continuation)
                    );
            }
        }

        #endregion

        #region ChangeDatabase

        /// <summary>
        /// Changes the database for the specified connection using the provided filters.
        /// </summary>
        /// <param name="this">The array of <see cref="IDbConnectionFilter"/>.</param>
        /// <param name="connection">The database connection to change.</param>
        /// <param name="databaseName">The name of the database to switch to.</param>
        public static void ChangeDatabase(this IDbConnectionFilter[] @this, IDbConnection connection, string databaseName)
        {
            if (@this == null || @this.Length == 0)
            {
                connection.ChangeDatabase(databaseName);
            }
            else
            {
                FilterUtility.InvokeAction(
                    @this,
                    connection,
                    databaseName,
                    (_connection, _databaseName) => _connection.ChangeDatabase(_databaseName),
                    (_filter, _connection, _databaseName, _continuation) => _filter.ChangeDatabase(_connection, _databaseName, _continuation)
                    );
            }
        }

        #endregion

        #region BeginTransaction

        /// <summary>
        /// Begins a transaction on the specified connection using the provided filters.
        /// </summary>
        /// <param name="this">The array of <see cref="IDbConnectionFilter"/>.</param>
        /// <param name="connection">The database connection to begin the transaction on.</param>
        /// <param name="isolationLevel">The isolation level for the transaction.</param>
        /// <returns>The transaction object.</returns>
        public static IDbTransaction BeginTransaction(this IDbConnectionFilter[] @this, IDbConnection connection, IsolationLevel isolationLevel)
        {
            if (@this == null || @this.Length == 0)
            {
                return connection.BeginTransaction(isolationLevel);
            }
            else
            {
                return FilterUtility.InvokeFunc(
                    @this,
                    connection,
                    isolationLevel,
                    (_connection, _isolationLevel) => _connection.BeginTransaction(_isolationLevel),
                    (_filter, _connection, _isolationLevel, _continuation) => _filter.BeginTransaction(_connection, _isolationLevel, _continuation)
                    );
            }
        }

        #endregion

        #region CreateCommand

        /// <summary>
        /// Creates a command for the specified connection using the provided filters.
        /// </summary>
        /// <param name="this">The array of <see cref="IDbConnectionFilter"/>.</param>
        /// <param name="connection">The database connection to create the command for.</param>
        /// <returns>The created database command.</returns>
        public static IDbCommand CreateCommand(this IDbConnectionFilter[] @this, IDbConnection connection)
        {
            if (@this == null || @this.Length == 0)
            {
                return connection.CreateCommand();
            }
            else
            {
                return FilterUtility.InvokeFunc(
                    @this,
                    connection,
                    _connection => _connection.CreateCommand(),
                    (_filter, _connection, _continuation) => _filter.CreateCommand(_connection, _continuation)
                );
            }
        }

        #endregion
    }
}