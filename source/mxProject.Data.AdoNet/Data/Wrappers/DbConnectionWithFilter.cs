using mxProject.Data.Filters;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;

namespace mxProject.Data.Wrappers
{
    /// <summary>
    /// Represents a database connection with filters applied to its operations.
    /// </summary>
    internal sealed class DbConnectionWithFilter : IDbConnection
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="DbConnectionWithFilter"/> class.
        /// </summary>
        /// <param name="connection">The underlying database connection to wrap.</param>
        /// <param name="connectionFilters">The filters to apply to connection operations.</param>
        /// <param name="transactionFilters">The filters to apply to transaction operations.</param>
        /// <param name="commandFilters">The filters to apply to command operations.</param>
        /// <param name="parametersFilters">The filters to apply to parameters operations.</param>
        /// <param name="readerFilters">The filters to apply to datareader operations.</param>
        internal DbConnectionWithFilter(
            IDbConnection connection,
            DbFilterSet<DbConnectionFilterTargets, IDbConnectionFilter> connectionFilters,
            DbFilterSet<DbTransactionFilterTargets, IDbTransactionFilter> transactionFilters,
            DbFilterSet<DbCommandFilterTargets, IDbCommandFilter> commandFilters,
            DbFilterSet<DataParameterCollectionFilterTargets, IDataParameterCollectionFilter> parametersFilters,
            DbFilterSet<DataReaderFilterTargets, IDataReaderFilter> readerFilters
            )
        {
            WrappedConnection = connection;

            m_ConnectionFilters = connectionFilters;
            m_TransactionFilters= transactionFilters;
            m_CommandFilters = commandFilters;
            m_ParametersFilters = parametersFilters;
            m_ReaderFilters = readerFilters;
        }

        #region connection

        /// <summary>
        /// Gets the underlying database connection.
        /// </summary>
        internal IDbConnection WrappedConnection { get; }

        #endregion

        #region transaction

        private DbTransactionWithFilter? m_CurrentTransaction;

        /// <summary>
        /// Gets the current transaction, if any.
        /// </summary>
        internal IDbTransaction? CurrentTransaction
        {
            get { return m_CurrentTransaction; }
        }

        /// <summary>
        /// Ends the current transaction.
        /// </summary>
        internal void OnEndTransaction()
        {
            m_CurrentTransaction = null;
        }

        #endregion

        #region filter

        private readonly DbFilterSet<DbConnectionFilterTargets, IDbConnectionFilter> m_ConnectionFilters;
        private readonly DbFilterSet<DbTransactionFilterTargets, IDbTransactionFilter> m_TransactionFilters;
        private readonly DbFilterSet<DbCommandFilterTargets, IDbCommandFilter> m_CommandFilters;
        private readonly DbFilterSet<DataParameterCollectionFilterTargets, IDataParameterCollectionFilter> m_ParametersFilters;
        private readonly DbFilterSet<DataReaderFilterTargets, IDataReaderFilter> m_ReaderFilters;

        #endregion

        /// <inheritdoc/>
        public string ConnectionString
        {
            get => WrappedConnection.ConnectionString;
            set => WrappedConnection.ConnectionString = value;
        }

        /// <inheritdoc/>
        public int ConnectionTimeout => WrappedConnection.ConnectionTimeout;

        /// <inheritdoc/>
        public string Database => WrappedConnection.Database;

        /// <inheritdoc/>
        public ConnectionState State => WrappedConnection.State;

        /// <inheritdoc/>
        public IDbTransaction BeginTransaction()
        {
            var transaction = m_ConnectionFilters[DbConnectionFilterTargets.BeginTransaction].BeginTransaction(WrappedConnection, IsolationLevel.Unspecified);

            m_CurrentTransaction = new DbTransactionWithFilter(this, transaction, m_TransactionFilters);

            transaction = m_CurrentTransaction;

            return transaction;
        }

        /// <inheritdoc/>
        public IDbTransaction BeginTransaction(IsolationLevel il)
        {
            var transaction = m_ConnectionFilters[DbConnectionFilterTargets.BeginTransaction].BeginTransaction(WrappedConnection, il);

            m_CurrentTransaction = new DbTransactionWithFilter(this, transaction, m_TransactionFilters);

            transaction = m_CurrentTransaction;

            return transaction;
        }

        /// <inheritdoc/>
        public void ChangeDatabase(string databaseName)
        {
            m_ConnectionFilters[DbConnectionFilterTargets.ChangeDatabase].ChangeDatabase(WrappedConnection, databaseName);
        }

        /// <inheritdoc/>
        public void Close()
        {
            m_ConnectionFilters[DbConnectionFilterTargets.Close].Close(WrappedConnection);

            m_CurrentTransaction = null;
        }

        /// <inheritdoc/>
        public IDbCommand CreateCommand()
        {
            var command = m_ConnectionFilters[DbConnectionFilterTargets.CreateCommand].CreateCommand(WrappedConnection);

            command = new DbCommandWithFilter(this, command, m_CommandFilters, m_ParametersFilters, m_ReaderFilters);

            command.Transaction = m_CurrentTransaction!;

            return command;
        }

        /// <inheritdoc/>
        public void Dispose()
        {
            m_ConnectionFilters[DbConnectionFilterTargets.Dispose].Dispose(WrappedConnection);

            m_CurrentTransaction = null;
        }

        /// <inheritdoc/>
        public void Open()
        {
            m_ConnectionFilters[DbConnectionFilterTargets.Open].Open(WrappedConnection);

            m_CurrentTransaction = null;
        }
    }
}
