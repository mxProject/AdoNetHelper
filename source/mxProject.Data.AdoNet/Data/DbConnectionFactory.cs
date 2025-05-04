using System;
using System.Collections.Generic;
using System.Data;
using mxProject.Data.Filters;

namespace mxProject.Data
{
    /// <summary>  
    /// Factory class for creating database connections with optional filters.  
    /// </summary>  
    public class DbConnectionFactory : IDbConnectionFactory
    {
        /// <summary>  
        /// Initializes a new instance of the <see cref="DbConnectionFactory"/> class.  
        /// </summary>  
        /// <param name="connectionActivator">A function to create a new <see cref="IDbConnection"/> instance.</param>  
        /// <param name="connectionFilters">Optional filters for <see cref="IDbConnection"/> operations.</param>  
        /// <param name="transactionFilters">Optional filters for <see cref="IDbTransaction"/> operations.</param>  
        /// <param name="commandFilters">Optional filters for <see cref="IDbCommand"/> operations.</param>  
        /// <param name="parametersFilters">Optional filters for <see cref="IDataParameterCollection"/> operations.</param>  
        /// <param name="readerFilters">Optional filters for <see cref="IDataReader"/> operations.</param>  
        public DbConnectionFactory(
            Func<IDbConnection> connectionActivator,
            IEnumerable<IDbConnectionFilter>? connectionFilters = null,
            IEnumerable<IDbTransactionFilter>? transactionFilters = null,
            IEnumerable<IDbCommandFilter>? commandFilters = null,
            IEnumerable<IDataParameterCollectionFilter>? parametersFilters = null,
            IEnumerable<IDataReaderFilter>? readerFilters = null
            )
        {
            m_ConnectionActivator = connectionActivator;

            if (connectionFilters != null) { m_ConnectionFilters = DbFilterSet.Create(connectionFilters, x => x.TargetMethods); }
            if (transactionFilters != null) { m_TransactionFilters = DbFilterSet.Create(transactionFilters, x => x.TargetMethods); }
            if (commandFilters != null) { m_CommandFilters = DbFilterSet.Create(commandFilters, x => x.TargetMethods); }
            if (parametersFilters != null) { m_ParametersFilters = DbFilterSet.Create(parametersFilters, x => x.TargetMethods); }
            if (readerFilters != null) { m_ReaderFilters = DbFilterSet.Create(readerFilters, x => x.TargetMethods); }
        }

        private readonly Func<IDbConnection> m_ConnectionActivator;
        private readonly DbFilterSet<DbConnectionFilterTargets, IDbConnectionFilter>? m_ConnectionFilters;
        private readonly DbFilterSet<DbTransactionFilterTargets, IDbTransactionFilter>? m_TransactionFilters;
        private readonly DbFilterSet<DbCommandFilterTargets, IDbCommandFilter>? m_CommandFilters;
        private readonly DbFilterSet<DataParameterCollectionFilterTargets, IDataParameterCollectionFilter>? m_ParametersFilters;
        private readonly DbFilterSet<DataReaderFilterTargets, IDataReaderFilter>? m_ReaderFilters;

        /// <summary>  
        /// Creates a new <see cref="IDbConnection"/> instance with the applied filters.  
        /// </summary>  
        /// <returns>A new <see cref="IDbConnection"/> instance.</returns>  
        public IDbConnection CreateConnection()
        {
            var connection = m_ConnectionActivator();

            if (!NeedWrap()) { return connection; }

            return connection.WithFilter(
                m_ConnectionFilters,
                m_TransactionFilters,
                m_CommandFilters,
                m_ParametersFilters,
                m_ReaderFilters
            );
        }

        /// <summary>
        /// Determines whether the connection needs to be wrapped with filters.
        /// </summary>
        /// <returns><c>true</c> if wrapping is needed; otherwise, <c>false</c>.</returns>
        private bool NeedWrap()
        {
            if (m_ConnectionFilters != null && m_ConnectionFilters.Targets != DbConnectionFilterTargets.None) { return true; }
            if (m_TransactionFilters != null && m_TransactionFilters.Targets != DbTransactionFilterTargets.None) { return true; }
            if (m_CommandFilters != null && m_CommandFilters.Targets != DbCommandFilterTargets.None) { return true; }
            if (m_ParametersFilters != null && m_ParametersFilters.Targets != DataParameterCollectionFilterTargets.None) { return true; }
            if (m_ReaderFilters != null && m_ReaderFilters.Targets != DataReaderFilterTargets.None) { return true; }

            return false;
        }

    }
}
