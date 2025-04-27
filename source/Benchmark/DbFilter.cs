using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Data.SqlClient; 
using BenchmarkDotNet.Attributes;
using mxProject.Data;
using mxProject.Data.Filters;
using mxProject.Data.Extensions;

namespace Benchmark
{
    /// <summary>
    /// Provides benchmarks for database connection and filter operations.
    /// </summary>
    [MemoryDiagnoser]
    [ShortRunJob]
    [MinColumn, MaxColumn]
    public class DbFilter
    {
        /// <summary>
        /// Creates a new database connection.
        /// </summary>
        /// <returns>An <see cref="IDbConnection"/> instance.</returns>
        internal static IDbConnection CreateConnection()
        {
            return new SqlConnection("Data Source=(localdb)\\SampleDB; Initial Catalog=TestDataAdoNet; Integrated Security=True");
        }

        /// <summary>
        /// Creates a collection of connection filters.
        /// </summary>
        /// <param name="count">The number of filters to create.</param>
        /// <returns>An enumerable of <see cref="IDbConnectionFilter"/>.</returns>
        internal static IEnumerable<IDbConnectionFilter> CreateConnectionFilters(int count)
        {
            for (int i = 0; i < count; i++)
            {
                yield return new DbConnectionFilter($"filter{i}");
            }
        }

        /// <summary>
        /// Creates a collection of transaction filters.
        /// </summary>
        /// <param name="count">The number of filters to create.</param>
        /// <returns>An enumerable of <see cref="IDbTransactionFilter"/>.</returns>
        internal static IEnumerable<IDbTransactionFilter> CreateTransactionFilters(int count)
        {
            for (int i = 0; i < count; i++)
            {
                yield return new DbTransactionFilter($"filter{i}");
            }
        }

        /// <summary>
        /// Creates a collection of command filters.
        /// </summary>
        /// <param name="count">The number of filters to create.</param>
        /// <returns>An enumerable of <see cref="IDbCommandFilter"/>.</returns>
        internal static IEnumerable<IDbCommandFilter> CreateCommandFilters(int count)
        {
            for (int i = 0; i < count; i++)
            {
                yield return new DbCommandFilter($"filter{i}");
            }
        }

        /// <summary>
        /// Creates a collection of parameter filters.
        /// </summary>
        /// <param name="count">The number of filters to create.</param>
        /// <returns>An enumerable of <see cref="IDataParameterCollectionFilter"/>.</returns>
        internal static IEnumerable<IDataParameterCollectionFilter> CreateParametersFilters(int count)
        {
            for (int i = 0; i < count; i++)
            {
                yield return new DbParametersFilter($"filter{i}");
            }
        }

        /// <summary>
        /// Creates a collection of data reader filters.
        /// </summary>
        /// <param name="count">The number of filters to create.</param>
        /// <returns>An enumerable of <see cref="IDataReaderFilter"/>.</returns>
        internal static IEnumerable<IDataReaderFilter> CreateDataReaderFilters(int count)
        {
            for (int i = 0; i < count; i++)
            {
                yield return new DataReaderFilter($"filter{i}");
            }
        }

        /// <summary>
        /// Represents a filter for database connections.
        /// </summary>
        private class DbConnectionFilter : DbConnectionFilterBase
        {
            /// <summary>
            /// Initializes a new instance of the <see cref="DbConnectionFilter"/> class.
            /// </summary>
            /// <param name="name">The name of the filter.</param>
            internal DbConnectionFilter(string name)
            {
                m_Name = name;
            }

            private readonly string m_Name;

            /// <inheritdoc/>
            public override DbConnectionFilterTargets TargetMethods => DbConnectionFilterTargets.All;
        }

        /// <summary>
        /// Represents a filter for database transactions.
        /// </summary>
        private class DbTransactionFilter : DbTransactionFilterBase
        {
            /// <summary>
            /// Initializes a new instance of the <see cref="DbTransactionFilter"/> class.
            /// </summary>
            /// <param name="name">The name of the filter.</param>
            internal DbTransactionFilter(string name)
            {
                m_Name = name;
            }

            private readonly string m_Name;

            /// <inheritdoc/>
            public override DbTransactionFilterTargets TargetMethods => DbTransactionFilterTargets.All;
        }

        /// <summary>
        /// Represents a filter for database commands.
        /// </summary>
        private class DbCommandFilter : DbCommandFilterBase
        {
            /// <summary>
            /// Initializes a new instance of the <see cref="DbCommandFilter"/> class.
            /// </summary>
            /// <param name="name">The name of the filter.</param>
            internal DbCommandFilter(string name)
            {
                m_Name = name;
            }

            private readonly string m_Name;

            /// <inheritdoc/>
            public override DbCommandFilterTargets TargetMethods => DbCommandFilterTargets.All;
        }

        /// <summary>
        /// Represents a filter for parameter collections.
        /// </summary>
        private class DbParametersFilter : DataParameterCollectionFilterBase
        {
            /// <summary>
            /// Initializes a new instance of the <see cref="DbParametersFilter"/> class.
            /// </summary>
            /// <param name="name">The name of the filter.</param>
            internal DbParametersFilter(string name)
            {
                m_Name = name;
            }

            private readonly string m_Name;

            /// <inheritdoc/>
            public override DataParameterCollectionFilterTargets TargetMethods => DataParameterCollectionFilterTargets.All;
        }

        /// <summary>
        /// Represents a filter for data readers.
        /// </summary>
        private class DataReaderFilter : DataReaderFilterBase
        {
            /// <summary>
            /// Initializes a new instance of the <see cref="DataReaderFilter"/> class.
            /// </summary>
            /// <param name="name">The name of the filter.</param>
            internal DataReaderFilter(string name)
            {
                m_Name = name;
            }

            private readonly string m_Name;

            /// <inheritdoc/>
            public override DataReaderFilterTargets TargetMethods => DataReaderFilterTargets.All;
        }

        /// <summary>
        /// Benchmarks the original connection without filters.
        /// </summary>
        [Benchmark]
        public void OriginalConnection()
        {
            using var connection = CreateConnection();

            Execute(connection);
        }

        /// <summary>
        /// Benchmarks a connection with no filters applied.
        /// </summary>
        [Benchmark]
        public void With0Filter()
        {
            using var connection = CreateConnection().WithFilter(
                CreateConnectionFilters(0),
                CreateTransactionFilters(0),
                CreateCommandFilters(0),
                CreateParametersFilters(0),
                CreateDataReaderFilters(0)
            );

            Execute(connection);
        }

        /// <summary>
        /// Benchmarks a connection with one filter applied.
        /// </summary>
        [Benchmark]
        public void With1Filter()
        {
            using var connection = CreateConnection().WithFilter(
                CreateConnectionFilters(1),
                CreateTransactionFilters(1),
                CreateCommandFilters(1),
                CreateParametersFilters(1),
                CreateDataReaderFilters(1)
            );

            Execute(connection);
        }

        /// <summary>
        /// Benchmarks a connection with two filters applied.
        /// </summary>
        [Benchmark]
        public void With2Filter()
        {
            using var connection = CreateConnection().WithFilter(
                CreateConnectionFilters(2),
                CreateTransactionFilters(2),
                CreateCommandFilters(2),
                CreateParametersFilters(2),
                CreateDataReaderFilters(2)
            );

            Execute(connection);
        }

        /// <summary>
        /// Benchmarks a connection with three filters applied.
        /// </summary>
        [Benchmark]
        public void With3Filter()
        {
            using var connection = CreateConnection().WithFilter(
                CreateConnectionFilters(3),
                CreateTransactionFilters(3),
                CreateCommandFilters(3),
                CreateParametersFilters(3),
                CreateDataReaderFilters(3)
            );

            Execute(connection);
        }

        /// <summary>
        /// Benchmarks a connection with four filters applied.
        /// </summary>
        [Benchmark]
        public void With4Filter()
        {
            using var connection = CreateConnection().WithFilter(
                CreateConnectionFilters(4),
                CreateTransactionFilters(4),
                CreateCommandFilters(4),
                CreateParametersFilters(4),
                CreateDataReaderFilters(4)
            );

            Execute(connection);
        }

        /// <summary>
        /// Benchmarks a connection created using the factory with four filters.
        /// </summary>
        [Benchmark]
        public void With4FilterUsingFactory()
        {
            using var connection = s_ConnectionFactory.CreateConnection();

            Execute(connection);
        }

        private static readonly DbConnectionFactory s_ConnectionFactory = new(
            CreateConnection,
            CreateConnectionFilters(4),
            CreateTransactionFilters(4),
            CreateCommandFilters(4),
            CreateParametersFilters(4),
            CreateDataReaderFilters(4)
            );


        /// <summary>
        /// Executes a sample database operation.
        /// </summary>
        /// <param name="connection">The database connection to use.</param>
        private static void Execute(IDbConnection connection)
        {
            connection.Open();

            using var transaction = connection.BeginTransaction();

            using var command = connection.CreateCommand();

            command.CommandText = "SELECT @value";
            command.AddInt32Parameter("@value", 1);
            command.Transaction = transaction;

            using (var reader = command.ExecuteReader())
            {
                reader.Read();
            }

            transaction.Rollback();

            connection.Close();
        }
    }
}
