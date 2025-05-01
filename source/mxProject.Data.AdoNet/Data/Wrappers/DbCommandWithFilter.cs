using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Common;
using System.Linq;
using mxProject.Data.Filters;

namespace mxProject.Data.Wrappers
{
    /// <summary>  
    /// Represents a database command with filters applied to its operations.  
    /// </summary>  
    internal sealed class DbCommandWithFilter : IDbCommand, IDbCommandWrapper
    {
        /// <summary>  
        /// Initializes a new instance of the <see cref="DbCommandWithFilter"/> class.  
        /// </summary>  
        /// <param name="connection">The database connection with filters.</param>  
        /// <param name="command">The underlying database command to wrap.</param>  
        /// <param name="commandFilters">The filters to apply to command operations.</param>  
        /// <param name="parametersFilters">The filters to apply to parameter collection operations.</param>  
        /// <param name="readerFilters">The filters to apply to datareader operations.</param>
        internal DbCommandWithFilter(DbConnectionWithFilter connection, IDbCommand command, DbFilterSet<DbCommandFilterTargets, IDbCommandFilter> commandFilters, DbFilterSet<DataParameterCollectionFilterTargets, IDataParameterCollectionFilter> parametersFilters, DbFilterSet<DataReaderFilterTargets, IDataReaderFilter> readerFilters)
        {
            ConnectionWrapper = connection;
            WrappedCommand = command;

            if (parametersFilters.Targets != DataParameterCollectionFilterTargets.None)
            {
                m_Parameters = new DataParameterCollectionWithFilter(command.Parameters, parametersFilters);
            }
            else
            {
                m_Parameters = command.Parameters;
            }

            m_CommandFilters = commandFilters;
            m_ReaderFilters = readerFilters;
        }

        #region connection  

        /// <summary>  
        /// Gets or sets the database connection associated with this command.  
        /// </summary>  
        internal DbConnectionWithFilter ConnectionWrapper { get; set; }

        #endregion

        #region command  

        /// <summary>  
        /// Gets the underlying database command.  
        /// </summary>  
        internal IDbCommand WrappedCommand { get; }

        IDbCommand IDbCommandWrapper.WrappedCommand => WrappedCommand;

        #endregion

        #region parameters

        /// <summary>  
        /// Gets the parameter collection wrapper with filters applied.  
        /// </summary>  
        private readonly IDataParameterCollection m_Parameters;

        #endregion

        #region filter  

        /// <summary>  
        /// The filters applied to command operations.  
        /// </summary>  
        private readonly DbFilterSet<DbCommandFilterTargets, IDbCommandFilter> m_CommandFilters;

        /// <summary>
        /// The filters applied to datareader operations.  
        /// </summary>
        private readonly DbFilterSet<DataReaderFilterTargets, IDataReaderFilter> m_ReaderFilters;

        #endregion

        /// <inheritdoc/>  
        public string CommandText
        {
            get => WrappedCommand.CommandText;
            set => WrappedCommand.CommandText = value;
        }

        /// <inheritdoc/>  
        public int CommandTimeout
        {
            get => WrappedCommand.CommandTimeout;
            set => WrappedCommand.CommandTimeout = value;
        }

        /// <inheritdoc/>  
        public CommandType CommandType
        {
            get => WrappedCommand.CommandType;
            set => WrappedCommand.CommandType = value;
        }

        /// <inheritdoc/>  
        public IDbConnection Connection
        {
            get => ConnectionWrapper;
            set
            {
                if (object.Equals(ConnectionWrapper, value)) { return; }
                ConnectionWrapper = (DbConnectionWithFilter)value;
                WrappedCommand.Connection = ConnectionWrapper?.WrappedConnection;
                Transaction = ConnectionWrapper?.CurrentTransaction!;
            }
        }

        /// <inheritdoc/>  
        public IDataParameterCollection Parameters
        {
            get => m_Parameters;
        }

        /// <inheritdoc/>  
        public IDbTransaction Transaction
        {
            get => m_TransactionWrapper!;
            set
            {
                if (object.Equals(m_TransactionWrapper, value)) { return; }
                m_TransactionWrapper = (DbTransactionWithFilter)value;
                WrappedCommand.Transaction = m_TransactionWrapper?.WrappedTransaction;
            }
        }
        private DbTransactionWithFilter? m_TransactionWrapper;

        /// <inheritdoc/>  
        public UpdateRowSource UpdatedRowSource
        {
            get => WrappedCommand.UpdatedRowSource;
            set => WrappedCommand.UpdatedRowSource = value;
        }

        /// <inheritdoc/>  
        public void Cancel()
        {
            m_CommandFilters[DbCommandFilterTargets.Cancel].Cancel(WrappedCommand);
        }

        /// <inheritdoc/>  
        public IDbDataParameter CreateParameter()
        {
            return m_CommandFilters[DbCommandFilterTargets.CreateParameter].CreateParameter(WrappedCommand);
        }

        /// <inheritdoc/>  
        public void Dispose()
        {
            m_CommandFilters[DbCommandFilterTargets.Dispose].Dispose(WrappedCommand);
        }

        /// <inheritdoc/>  
        public int ExecuteNonQuery()
        {
            return m_CommandFilters[DbCommandFilterTargets.ExecuteNonQuery].ExecuteNonQuery(WrappedCommand);
        }

        /// <inheritdoc/>  
        public IDataReader ExecuteReader()
        {
            var reader = m_CommandFilters[DbCommandFilterTargets.ExecuteReader].ExecuteReader(WrappedCommand, CommandBehavior.Default);

            if (m_ReaderFilters.Targets != DataReaderFilterTargets.None)
            {
                reader = new DataReaderWithFilter(reader, m_ReaderFilters);
            }

            return reader;
        }

        /// <inheritdoc/>  
        public IDataReader ExecuteReader(CommandBehavior behavior)
        {
            var reader = m_CommandFilters[DbCommandFilterTargets.ExecuteReader].ExecuteReader(WrappedCommand, behavior);

            if (m_ReaderFilters.Targets != DataReaderFilterTargets.None)
            {
                reader = new DataReaderWithFilter(reader, m_ReaderFilters);
            }

            return reader;
        }

        /// <inheritdoc/>  
        public object ExecuteScalar()
        {
            return m_CommandFilters[DbCommandFilterTargets.ExecuteScalar].ExecuteScalar(WrappedCommand);
        }

        /// <inheritdoc/>  
        public void Prepare()
        {
            m_CommandFilters[DbCommandFilterTargets.Prepare].Prepare(WrappedCommand);
        }
    }
}
