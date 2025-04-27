using System;
using System.Collections.Generic;
using System.Linq;
using System.Data;
using System.Threading.Tasks;
using System.Text;
using Xunit.Abstractions;
using mxProject.Data;
using mxProject.Data.Extensions;

namespace Test.Data.AdoNet
{
    /// <summary>  
    /// Provides tests for database filters.  
    /// </summary>  
    public class TestDbFilter
    {
        /// <summary>  
        /// Initializes a new instance of the <see cref="TestDbFilter"/> class.  
        /// </summary>  
        /// <param name="outputHelper">The output helper for logging test output.</param>  
        public TestDbFilter(ITestOutputHelper outputHelper)
        {
            m_OutputHelper = outputHelper;
        }

        private readonly ITestOutputHelper m_OutputHelper;

        /// <summary>  
        /// Creates a collection of connection filters.  
        /// </summary>  
        /// <param name="filterCount">The number of filters to create.</param>  
        /// <param name="targets">The target methods for the filters.</param>  
        /// <returns>An enumerable of connection filters.</returns>  
        private IEnumerable<IDbConnectionFilter> CreateConnectionFilters(int filterCount, DbConnectionFilterTargets targets)
        {
            for (int i = 0; i < filterCount; i++)
            {
                yield return new SampleDbConnectionFilter($"ConnectionFilter{i + 1}", targets, m_OutputHelper);
            }
        }

        /// <summary>  
        /// Creates a collection of transaction filters.  
        /// </summary>  
        /// <param name="filterCount">The number of filters to create.</param>  
        /// <param name="targets">The target methods for the filters.</param>  
        /// <returns>An enumerable of transaction filters.</returns>  
        private IEnumerable<IDbTransactionFilter> CreateTransactionFilters(int filterCount, DbTransactionFilterTargets targets)
        {
            for (int i = 0; i < filterCount; i++)
            {
                yield return new SampleDbTransactionFilter($"TransactionFilter{i + 1}", targets, m_OutputHelper);
            }
        }

        /// <summary>  
        /// Creates a collection of command filters.  
        /// </summary>  
        /// <param name="filterCount">The number of filters to create.</param>  
        /// <param name="targets">The target methods for the filters.</param>  
        /// <returns>An enumerable of command filters.</returns>  
        private IEnumerable<IDbCommandFilter> CreateCommandFilters(int filterCount, DbCommandFilterTargets targets)
        {
            for (int i = 0; i < filterCount; i++)
            {
                yield return new SampleDbCommandFilter($"CommandFilter{i + 1}", targets, m_OutputHelper);
            }
        }

        /// <summary>  
        /// Creates a collection of parameter collection filters.  
        /// </summary>  
        /// <param name="filterCount">The number of filters to create.</param>  
        /// <param name="targets">The target methods for the filters.</param>  
        /// <returns>An enumerable of parameter collection filters.</returns>  
        private IEnumerable<IDataParameterCollectionFilter> CreateParametersFilters(int filterCount, DataParameterCollectionFilterTargets targets)
        {
            for (int i = 0; i < filterCount; i++)
            {
                yield return new SampleDataParameterCollectionFilter($"ParametersFilter{i + 1}", targets, m_OutputHelper);
            }
        }

        /// <summary>  
        /// Creates a collection of datareader filters.  
        /// </summary>  
        /// <param name="filterCount">The number of filters to create.</param>  
        /// <param name="targets">The target methods for the filters.</param>  
        /// <returns>An enumerable of datareader filters.</returns>  
        private IEnumerable<IDataReaderFilter> CreateDataReaderFilters(int filterCount, DataReaderFilterTargets targets)
        {
            for (int i = 0; i < filterCount; i++)
            {
                yield return new SampleDataReaderFilter($"ReaderFilter{i + 1}", targets, m_OutputHelper);
            }
        }

        /// <summary>  
        /// Represents the settings for database filters.  
        /// </summary>  
        public class DbFilterSettings
        {
            /// <summary>  
            /// Gets or sets the number of filters.  
            /// </summary>  
            public int FilterCount { get; set; }

            /// <summary>  
            /// Gets or sets the connection filter targets.  
            /// </summary>  
            public DbConnectionFilterTargets ConnectionFilterTargets { get; set; }

            /// <summary>  
            /// Gets or sets the transaction filter targets.  
            /// </summary>  
            public DbTransactionFilterTargets TransactionFilterTargets { get; set; }

            /// <summary>  
            /// Gets or sets the command filter targets.  
            /// </summary>  
            public DbCommandFilterTargets CommandFilterTargets { get; set; }

            /// <summary>  
            /// Gets or sets the parameter collection filter targets.  
            /// </summary>  
            public DataParameterCollectionFilterTargets ParametersFilterTargets { get; set; }

            /// <summary>  
            /// Gets or sets the datareader filter targets.  
            /// </summary>  
            public DataReaderFilterTargets DataReaderFilterTargets { get; set; }

            /// <summary>  
            /// Returns a string representation of the filter settings.  
            /// </summary>  
            /// <returns>A string representation of the filter settings.</returns>  
            public override string ToString()
            {
                StringBuilder sb = new();

                sb.Append($"FilterCount={FilterCount}");

                if (ConnectionFilterTargets != DbConnectionFilterTargets.None)
                {
                    sb.Append($", ConnectionFilter={ConnectionFilterTargets}");
                }

                if (TransactionFilterTargets != DbTransactionFilterTargets.None)
                {
                    sb.Append($", TransactionFilter={TransactionFilterTargets}");
                }

                if (CommandFilterTargets != DbCommandFilterTargets.None)
                {
                    sb.Append($", CommandFilter={CommandFilterTargets}");
                }

                if (ParametersFilterTargets != DataParameterCollectionFilterTargets.None)
                {
                    sb.Append($", ParametersFilter={ParametersFilterTargets}");
                }

                if (DataReaderFilterTargets != DataReaderFilterTargets.None)
                {
                    sb.Append($", ReaderFilter={DataReaderFilterTargets}");
                }

                return sb.ToString();
            }
        }

        /// <summary>  
        /// Provides test data for the <see cref="WithFilter"/> test.  
        /// </summary>  
        /// <returns>An enumerable of test data.</returns>  
        public static IEnumerable<object[]> WithFilterArguments()
        {
            int maxFilterCount = 3;

            foreach (var target in Enum.GetValues<DbConnectionFilterTargets>())
            {
                for (var filterCount = 0; filterCount <= maxFilterCount; ++filterCount)
                {
                    yield return new object[] { SerializableMemberData.Create(new DbFilterSettings() { FilterCount = filterCount, ConnectionFilterTargets = target }) };
                }
            }

            foreach (var target in Enum.GetValues<DbTransactionFilterTargets>())
            {
                for (var filterCount = 0; filterCount <= maxFilterCount; ++filterCount)
                {
                    yield return new object[] { SerializableMemberData.Create(new DbFilterSettings() { FilterCount = filterCount, TransactionFilterTargets = target }) };
                }
            }

            foreach (var target in Enum.GetValues<DbCommandFilterTargets>())
            {
                for (var filterCount = 0; filterCount <= maxFilterCount; ++filterCount)
                {
                    yield return new object[] { SerializableMemberData.Create(new DbFilterSettings() { FilterCount = filterCount, CommandFilterTargets = target }) };
                }
            }

            foreach (var target in Enum.GetValues<DataParameterCollectionFilterTargets>())
            {
                for (var filterCount = 0; filterCount <= maxFilterCount; ++filterCount)
                {
                    yield return new object[] { SerializableMemberData.Create(new DbFilterSettings() { FilterCount = filterCount, ParametersFilterTargets = target }) };
                }
            }

            foreach (var target in Enum.GetValues<DataReaderFilterTargets>())
            {
                for (var filterCount = 0; filterCount <= maxFilterCount; ++filterCount)
                {
                    yield return new object[] { SerializableMemberData.Create(new DbFilterSettings() { FilterCount = filterCount, DataReaderFilterTargets = target }) };
                }
            }
        }

        /// <summary>  
        /// Tests the behavior of database filters with various configurations.  
        /// </summary>  
        /// <param name="args">The filter settings to test.</param>  
        [Theory]
        [MemberData(nameof(WithFilterArguments))]
        public void WithFilter(SerializableMemberData<DbFilterSettings> args)
        {
            var settings = args.MemberData;

            m_OutputHelper.WriteLine(settings.ToString());

            var connectionFilters = CreateConnectionFilters(settings.FilterCount, settings.ConnectionFilterTargets);
            var transactionFilters = CreateTransactionFilters(settings.FilterCount, settings.TransactionFilterTargets);
            var commandFilters = CreateCommandFilters(settings.FilterCount, settings.CommandFilterTargets);
            var parametersFilters = CreateParametersFilters(settings.FilterCount, settings.ParametersFilterTargets);
            var readerFilters = CreateDataReaderFilters(settings.FilterCount, settings.DataReaderFilterTargets);

            using var connection = SampleDatabase.CreateConnection().WithFilter(connectionFilters, transactionFilters, commandFilters, parametersFilters, readerFilters);

            m_OutputHelper.WriteLine("connection.Open");
            connection.Open();

            m_OutputHelper.WriteLine("connection.ChangeDatabase");
            connection.ChangeDatabase("TestDataAdoNet");

            m_OutputHelper.WriteLine("connection.BeginTransaction");
            using (var transaction = connection.BeginTransaction())
            {
                m_OutputHelper.WriteLine("connection.CreateCommand");
                using (var command = connection.CreateCommand())
                {
                    command.CommandText = "create table #TEMP_TABLE (ID int, NAME varchar(50))";

                    m_OutputHelper.WriteLine("command.Cancel");
                    command.Cancel();

                    m_OutputHelper.WriteLine("command.Prepare");
                    command.Prepare();

                    m_OutputHelper.WriteLine("command.ExecuteNonQuery");
                    command.ExecuteNonQuery();

                    command.CommandText = "insert into #TEMP_TABLE values (@ID, @NAME)";

                    m_OutputHelper.WriteLine("command.AddInt32Parameter");
                    command.AddInt32Parameter("ID", 1);

                    m_OutputHelper.WriteLine("command.AddInt32Parameter");
                    command.AddStringParameter("NAME", "aaa");

                    m_OutputHelper.WriteLine("command.ExecuteNonQuery");
                    command.ExecuteNonQuery();

                    m_OutputHelper.WriteLine("command.Parameters.RemoveAt");
                    command.Parameters.RemoveAt("ID");

                    m_OutputHelper.WriteLine("command.Parameters.RemoveAt");
                    command.Parameters.RemoveAt(0);

                    m_OutputHelper.WriteLine("command.AddInt32Parameter");
                    command.AddInt32Parameter("ID", 2);

                    m_OutputHelper.WriteLine("command.AddInt32Parameter");
                    command.AddStringParameter("NAME", "bbb");

                    m_OutputHelper.WriteLine("command.ExecuteNonQuery");
                    command.ExecuteNonQuery();

                    m_OutputHelper.WriteLine("command.Parameters.Clear");
                    command.Parameters.Clear();

                    m_OutputHelper.WriteLine("command.AddInt32Parameter");
                    command.AddInt32Parameter("ID", 3);

                    m_OutputHelper.WriteLine("command.AddInt32Parameter");
                    command.AddStringParameter("NAME", "ccc");

                    m_OutputHelper.WriteLine("command.ExecuteNonQuery");
                    command.ExecuteNonQuery();
                }

                m_OutputHelper.WriteLine("connection.CreateCommand");
                using (var command = connection.CreateCommand())
                {
                    command.CommandText = "select count(*) from #TEMP_TABLE";

                    m_OutputHelper.WriteLine("command.ExecuteScalar");
                    Assert.Equal(3, command.ExecuteScalar());
                }

                m_OutputHelper.WriteLine("connection.CreateCommand");
                using (var command = connection.CreateCommand())
                {
                    command.CommandText = "select * from #TEMP_TABLE";

                    m_OutputHelper.WriteLine("command.ExecuteReader");
                    using var reader = command.ExecuteReader();

                    while (reader.Read())
                    {
                        m_OutputHelper.WriteLine($"ID: {reader["id"]}, Name: {reader["name"]}");
                    }
                }

                m_OutputHelper.WriteLine("transaction.Rollback");
                transaction.Rollback();
            }

            m_OutputHelper.WriteLine("connection.BeginTransaction");
            using (var transaction = connection.BeginTransaction())
            {
                m_OutputHelper.WriteLine("transaction.Commit");
                transaction.Commit();
            }

            m_OutputHelper.WriteLine("connection.Close");
            connection.Close();
        }

        /// <summary>  
        /// Tests the behavior of database filters with various configurations.  
        /// </summary>  
        /// <param name="args">The filter settings to test.</param>  
        [Theory]
        [MemberData(nameof(WithFilterArguments))]
        public async Task WithFilterAsync(SerializableMemberData<DbFilterSettings> args)
        {
            var settings = args.MemberData;

            m_OutputHelper.WriteLine(settings.ToString());

            var connectionFilters = CreateConnectionFilters(settings.FilterCount, settings.ConnectionFilterTargets);
            var transactionFilters = CreateTransactionFilters(settings.FilterCount, settings.TransactionFilterTargets);
            var commandFilters = CreateCommandFilters(settings.FilterCount, settings.CommandFilterTargets);
            var parametersFilters = CreateParametersFilters(settings.FilterCount, settings.ParametersFilterTargets);

            using var connection = SampleDatabase.CreateConnection().WithFilter(connectionFilters, transactionFilters, commandFilters, parametersFilters);

            m_OutputHelper.WriteLine("connection.OpenAsync");
            await connection.OpenAsync();

            m_OutputHelper.WriteLine("connection.ChangeDatabaseAsync");
            await connection.ChangeDatabaseAsync("TestDataAdoNet");

            m_OutputHelper.WriteLine("connection.BeginTransactionAsync");
            using (var transaction = await connection.BeginTransactionAsync())
            {
                m_OutputHelper.WriteLine("connection.CreateCommand");
                using (var command = connection.CreateCommand())
                {
                    command.CommandText = "create table #TEMP_TABLE (ID int, NAME varchar(50))";

                    m_OutputHelper.WriteLine("command.Cancel");
                    command.Cancel();

                    m_OutputHelper.WriteLine("command.PrepareAsync");
                    await command.PrepareAsync();

                    m_OutputHelper.WriteLine("command.ExecuteNonQueryAsync");
                    await command.ExecuteNonQueryAsync();

                    command.CommandText = "insert into #TEMP_TABLE values (@ID, @NAME)";

                    m_OutputHelper.WriteLine("command.AddInt32Parameter");
                    command.AddInt32Parameter("ID", 1);

                    m_OutputHelper.WriteLine("command.AddInt32Parameter");
                    command.AddStringParameter("NAME", "aaa");

                    m_OutputHelper.WriteLine("command.ExecuteNonQueryAsync");
                    await command.ExecuteNonQueryAsync();

                    m_OutputHelper.WriteLine("command.Parameters.RemoveAt");
                    command.Parameters.RemoveAt("ID");

                    m_OutputHelper.WriteLine("command.Parameters.RemoveAt");
                    command.Parameters.RemoveAt(0);

                    m_OutputHelper.WriteLine("command.AddInt32Parameter");
                    command.AddInt32Parameter("ID", 2);

                    m_OutputHelper.WriteLine("command.AddInt32Parameter");
                    command.AddStringParameter("NAME", "bbb");

                    m_OutputHelper.WriteLine("command.ExecuteNonQueryAsync");
                    await command.ExecuteNonQueryAsync();

                    m_OutputHelper.WriteLine("command.Parameters.Clear");
                    command.Parameters.Clear();

                    m_OutputHelper.WriteLine("command.AddInt32Parameter");
                    command.AddInt32Parameter("ID", 3);

                    m_OutputHelper.WriteLine("command.AddInt32Parameter");
                    command.AddStringParameter("NAME", "ccc");

                    m_OutputHelper.WriteLine("command.ExecuteNonQueryAsync");
                    await command.ExecuteNonQueryAsync();
                }

                m_OutputHelper.WriteLine("connection.CreateCommand");
                using (var command = connection.CreateCommand())
                {
                    command.CommandText = "select count(*) from #TEMP_TABLE";

                    m_OutputHelper.WriteLine("command.ExecuteScalarAsync");
                    Assert.Equal(3, await command.ExecuteScalarAsync());
                }

                m_OutputHelper.WriteLine("connection.CreateCommand");
                using (var command = connection.CreateCommand())
                {
                    command.CommandText = "select * from #TEMP_TABLE";

                    m_OutputHelper.WriteLine("command.ExecuteReaderAsync");
                    using var reader = await command.ExecuteReaderAsync();

                    while (await reader.ReadAsync())
                    {
                        m_OutputHelper.WriteLine($"ID: {reader["id"]}, Name: {reader["name"]}");
                    }

                    m_OutputHelper.WriteLine("command.DisposeAsync");
                    await command.DisposeAsync();
                }

                m_OutputHelper.WriteLine("transaction.RollbackAsync");
                await transaction.RollbackAsync();

                m_OutputHelper.WriteLine("transaction.DisposeAsync");
                await transaction.DisposeAsync();
            }

            m_OutputHelper.WriteLine("connection.BeginTransactionAsync");
            using (var transaction = await connection.BeginTransactionAsync())
            {
                m_OutputHelper.WriteLine("transaction.CommitAsync");
                await transaction.CommitAsync();
            }

            m_OutputHelper.WriteLine("connection.CloseAsync");
            await connection.CloseAsync();

            m_OutputHelper.WriteLine("connection.DisposeAsync");
            await connection.DisposeAsync();
        }
    }
}
