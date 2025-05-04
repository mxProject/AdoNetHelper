using System;
using System.Data;
using Xunit;
using Xunit.Abstractions;
using System.Transactions;
using Microsoft.Data.SqlClient;
using mxProject.Data;
using mxProject.Data.Extensions;
using System.Threading.Tasks;

namespace Test.Data.AdoNet
{
    /// <summary>
    /// Testing <see cref="DbCommandExecutorWithContext{TContext}"/>.
    /// </summary>
    public class TestDbCommandExecutorWithContext
    {
        public TestDbCommandExecutorWithContext(ITestOutputHelper outputHelper)
        {
            m_OutputHelper = outputHelper;
        }

        private readonly ITestOutputHelper m_OutputHelper;

        #region TransactionScope

        /// <summary>
        /// Tests executing a command within a TransactionScope and verifies the rollback.
        /// </summary>
        [Fact]
        public void ExecuteIteratorOnNewConnection_TransactionScope()
        {
            var executor = new DbCommandExecutorWithContext<SampleDbContext>(SampleDatabase.CreateConnection, true, ConfigureCommand);

            int recordCount = 0;

            using (var transactionScope = new TransactionScope())
            {
                var context = new SampleDbContext();

                // get records.
                var records = executor.ExecuteIteratorOnNewConnection(EnumerateRecords, context);

                foreach (var record in records)
                {
                    m_OutputHelper.WriteLine($"ID = {record["ID"]}, NAME = {record["NAME"]}");
                    recordCount++;
                }

                Assert.Equal(5, recordCount);

                // insert 
                executor.ExecuteOnNewConnection(Insert, context);

                // get count
                recordCount = executor.ExecuteOnNewConnection(GetCount, context);

                Assert.Equal(6, recordCount);

                // rollback
                transactionScope.Dispose();
            }

            using var connection2 = SampleDatabase.CreateConnection();

            connection2.Open();

            var context2 = new SampleDbContext(connection2);

            // get count
            recordCount = executor.ExecuteOnNewConnection(GetCount, context2);

            Assert.Equal(5, recordCount);
        }

        /// <summary>
        /// Tests executing a command within a TransactionScope and verifies the rollback.
        /// </summary>
        [Fact]
        public async Task ExecuteIteratorOnNewConnectionAsync_TransactionScope()
        {
            var executor = new DbCommandExecutorWithContext<SampleDbContext>(SampleDatabase.CreateConnection, true, ConfigureCommand);

            int recordCount = 0;

            using (var transactionScope = new TransactionScope())
            {
                var context = new SampleDbContext();

                // get records.
                var records = executor.ExecuteIteratorOnNewConnectionAsync(EnumerateRecordsAsync, context);

                await foreach (var record in records.ConfigureAwait(false))
                {
                    m_OutputHelper.WriteLine($"ID = {record["ID"]}, NAME = {record["NAME"]}");
                    recordCount++;
                }

                Assert.Equal(5, recordCount);

                // insert 
                await executor.ExecuteOnNewConnectionAsync(InsertAsync, context);

                // get count
                recordCount = await executor.ExecuteOnNewConnectionAsync(GetCountAsync, context);

                Assert.Equal(6, recordCount);

                // rollback
                transactionScope.Dispose();
            }

            using var connection2 = SampleDatabase.CreateConnection();

            await connection2.OpenAsync();

            var context2 = new SampleDbContext(connection2);

            // get count
            recordCount = await executor.ExecuteOnNewConnectionAsync(GetCountAsync, context2);

            Assert.Equal(5, recordCount);
        }

        /// <summary>
        /// Tests executing a command with state within a TransactionScope and verifies the rollback.
        /// </summary>
        [Fact]
        public void ExecuteIteratorOnNewConnection_State_TransactionScope()
        {
            var executor = new DbCommandExecutorWithContext<SampleDbContext>(SampleDatabase.CreateConnection, true, ConfigureCommand);

            int recordCount = 0;

            using (var transactionScope = new TransactionScope())
            {
                var context = new SampleDbContext();

                // get records.
                var records = executor.ExecuteIteratorOnNewConnection(this, (@this, commandActivator, @context) => @this.EnumerateRecords(commandActivator, @context), context);

                foreach (var record in records)
                {
                    m_OutputHelper.WriteLine($"ID = {record["ID"]}, NAME = {record["NAME"]}");
                    recordCount++;
                }

                Assert.Equal(5, recordCount);

                // insert 
                executor.ExecuteOnNewConnection(this, (@this, commandActivator, @context) => @this.Insert(commandActivator, @context), context);

                // get count
                recordCount = executor.ExecuteOnNewConnection(this, (@this, commandActivator, @context) => @this.GetCount(commandActivator, @context), context);

                Assert.Equal(6, recordCount);

                // rollback
                transactionScope.Dispose();
            }

            using var connection2 = SampleDatabase.CreateConnection();

            connection2.Open();

            var context2 = new SampleDbContext(connection2);

            // get count
            recordCount = executor.ExecuteOnNewConnection(this, (@this, commandActivator, @context) => @this.GetCount(commandActivator, @context), context2);

            Assert.Equal(5, recordCount);
        }

        /// <summary>
        /// Tests executing a command with state within a TransactionScope and verifies the rollback.
        /// </summary>
        [Fact]
        public async Task ExecuteIteratorOnNewConnectionAsync_State_TransactionScope()
        {
            var executor = new DbCommandExecutorWithContext<SampleDbContext>(SampleDatabase.CreateConnection, true, ConfigureCommand);

            int recordCount = 0;

            using (var transactionScope = new TransactionScope())
            {
                var context = new SampleDbContext();

                // get records.
                var records = executor.ExecuteIteratorOnNewConnectionAsync(this, (@this, commandActivator, @context) => @this.EnumerateRecordsAsync(commandActivator, @context), context);

                await foreach (var record in records.ConfigureAwait(false))
                {
                    m_OutputHelper.WriteLine($"ID = {record["ID"]}, NAME = {record["NAME"]}");
                    recordCount++;
                }

                Assert.Equal(5, recordCount);

                // insert 
                await executor.ExecuteOnNewConnectionAsync(this, (@this, commandActivator, @context) => @this.InsertAsync(commandActivator, @context), context);

                // get count
                recordCount = await executor.ExecuteOnNewConnectionAsync(this, (@this, commandActivator, @context) => @this.GetCountAsync(commandActivator, @context), context);

                Assert.Equal(6, recordCount);

                // rollback
                transactionScope.Dispose();
            }

            using var connection2 = SampleDatabase.CreateConnection();

            await connection2.OpenAsync();

            var context2 = new SampleDbContext(connection2);

            // get count
            recordCount = await executor.ExecuteOnNewConnectionAsync(this, (@this, commandActivator, @context) => @this.GetCountAsync(commandActivator, @context), context2);

            Assert.Equal(5, recordCount);
        }

        #endregion

        #region ExecuteIteratorOnNewConnection

        /// <summary>
        /// Tests executing a command to get records and verifies the record count.
        /// </summary>
        [Fact]
        public void ExecuteIteratorOnNewConnection_EnumerateRecords()
        {
            using var connection = SampleDatabase.CreateConnection();

            connection.Open();

            var context = new SampleDbContext(connection);

            var executor = new DbCommandExecutorWithContext<SampleDbContext>(SampleDatabase.CreateConnection, true, ConfigureCommand);

            var records = executor.ExecuteIteratorOnNewConnection(EnumerateRecords, context);

            int recordCount = 0;

            foreach (var record in records)
            {
                m_OutputHelper.WriteLine($"ID = {record["ID"]}, NAME = {record["NAME"]}");
                recordCount++;
            }

            Assert.Equal(5, recordCount);
        }

        /// <summary>
        /// Tests executing a command with state to get records and verifies the record count.
        /// </summary>
        [Fact]
        public void ExecuteIteratorOnNewConnection_State_EnumerateRecords()
        {
            using var connection = SampleDatabase.CreateConnection();

            connection.Open();

            var context = new SampleDbContext(connection);

            var executor = new DbCommandExecutorWithContext<SampleDbContext>(SampleDatabase.CreateConnection, true, ConfigureCommand);

            var records = executor.ExecuteIteratorOnNewConnection(this, (@this, commandActivator, @context) => @this.EnumerateRecords(commandActivator, @context), context);

            int recordCount = 0;

            foreach (var record in records)
            {
                m_OutputHelper.WriteLine($"ID = {record["ID"]}, NAME = {record["NAME"]}");
                recordCount++;
            }

            Assert.Equal(5, recordCount);
        }

        /// <summary>
        /// Tests executing a command to get records and expects an InvalidOperationException.
        /// </summary>
        [Fact]
        public void ExecuteOnNewConnection_EnumerateRecords()
        {
            using var connection = SampleDatabase.CreateConnection();

            connection.Open();

            var context = new SampleDbContext(connection);

            var executor = new DbCommandExecutorWithContext<SampleDbContext>(SampleDatabase.CreateConnection, true, ConfigureCommand);

            Assert.Throws<InvalidOperationException>(() =>
            {
                var records = executor.ExecuteOnNewConnection(EnumerateRecords, context);

                int recordCount = 0;

                foreach (var record in records)
                {
                    m_OutputHelper.WriteLine($"ID = {record["ID"]}, NAME = {record["NAME"]}");
                    recordCount++;
                }

                Assert.Equal(5, recordCount);
            });
        }

        /// <summary>
        /// Tests executing a command with state to get records and expects an InvalidOperationException.
        /// </summary>
        [Fact]
        public void ExecuteOnNewConnection_State_EnumerateRecords()
        {
            using var connection = SampleDatabase.CreateConnection();

            connection.Open();

            var context = new SampleDbContext(connection);
            
            var executor = new DbCommandExecutorWithContext<SampleDbContext>(SampleDatabase.CreateConnection, true, ConfigureCommand);

            Assert.Throws<InvalidOperationException>(() =>
            {
                var records = executor.ExecuteOnNewConnection(this, (@this, commandActivator, @context) => @this.EnumerateRecords(commandActivator, @context), context);

                int recordCount = 0;

                foreach (var record in records)
                {
                    m_OutputHelper.WriteLine($"ID = {record["ID"]}, NAME = {record["NAME"]}");
                    recordCount++;
                }

                Assert.Equal(5, recordCount);
            });
        }

        /// <summary>
        /// Tests executing a command to get the count of records and verifies the count.
        /// </summary>
        [Fact]
        public void ExecuteOnNewConnection_GetCount()
        {
            using var connection = SampleDatabase.CreateConnection();

            connection.Open();

            var context = new SampleDbContext(connection);

            var executor = new DbCommandExecutorWithContext<SampleDbContext>(SampleDatabase.CreateConnection, true, ConfigureCommand);

            var count = executor.ExecuteOnNewConnection(GetCount, context);

            m_OutputHelper.WriteLine($"Count = {count}");

            Assert.Equal(5, count);
        }

        /// <summary>
        /// Tests executing a command with state to get the count of records and verifies the count.
        /// </summary>
        [Fact]
        public void ExecuteOnNewConnection_State_GetCount()
        {
            using var connection = SampleDatabase.CreateConnection();

            connection.Open();

            var context = new SampleDbContext(connection);

            var executor = new DbCommandExecutorWithContext<SampleDbContext>(SampleDatabase.CreateConnection, true, ConfigureCommand);

            var count = executor.ExecuteOnNewConnection(this, (@this, commandActivator, @context) => @this.GetCount(commandActivator, @context), context);

            m_OutputHelper.WriteLine($"Count = {count}");

            Assert.Equal(5, count);
        }

        /// <summary>
        /// Tests executing a command to insert a record and expects a SqlException.
        /// </summary>
        [Fact]
        public void ExecuteOnNewConnection_Insert()
        {
            using var connection = SampleDatabase.CreateConnection();

            connection.Open();

            var context = new SampleDbContext(connection);

            var executor = new DbCommandExecutorWithContext<SampleDbContext>(SampleDatabase.CreateConnection, true, ConfigureCommand);

            Assert.Throws<SqlException>(() =>
            {
                // An exception will be thrown and the update will be rolled back.
                executor.ExecuteOnNewConnection(InsertFailed, context);
            });

            var count = executor.ExecuteOnNewConnection(GetCount, context);

            Assert.Equal(5, count);
        }

        /// <summary>
        /// Tests executing a command with state to insert a record and expects a SqlException.
        /// </summary>
        [Fact]
        public void ExecuteOnNewConnection_State_Insert()
        {
            using var connection = SampleDatabase.CreateConnection();

            connection.Open();

            var context = new SampleDbContext(connection);

            var executor = new DbCommandExecutorWithContext<SampleDbContext>(SampleDatabase.CreateConnection, true, ConfigureCommand);

            Assert.Throws<SqlException>(() =>
            {
                // An exception will be thrown and the update will be rolled back.
                executor.ExecuteOnNewConnection(this, (@this, commandActivator, @context) => @this.InsertFailed(commandActivator, @context), context);
            });

            var count = executor.ExecuteOnNewConnection(this, (@this, commandActivator, @context) => @this.GetCount(commandActivator, @context), context);

            Assert.Equal(5, count);
        }

        #endregion

        #region ExecuteIteratorOnNewConnectionAsync

        /// <summary>
        /// Tests executing a command to get records and verifies the record count.
        /// </summary>
        [Fact]
        public async Task ExecuteIteratorOnNewConnectionAsync_EnumerateRecords()
        {
            using var connection = SampleDatabase.CreateConnection();

            connection.Open();

            var context = new SampleDbContext(connection);

            var executor = new DbCommandExecutorWithContext<SampleDbContext>(SampleDatabase.CreateConnection, true, ConfigureCommand);

            var records = executor.ExecuteIteratorOnNewConnectionAsync(EnumerateRecordsAsync, context);

            int recordCount = 0;

            await foreach (var record in records.ConfigureAwait(false))
            {
                m_OutputHelper.WriteLine($"ID = {record["ID"]}, NAME = {record["NAME"]}");
                recordCount++;
            }

            Assert.Equal(5, recordCount);
        }

        /// <summary>
        /// Tests executing a command with state to get records and verifies the record count.
        /// </summary>
        [Fact]
        public async Task ExecuteIteratorOnNewConnectionAsync_State_EnumerateRecords()
        {
            using var connection = SampleDatabase.CreateConnection();

            connection.Open();

            var context = new SampleDbContext(connection);

            var executor = new DbCommandExecutorWithContext<SampleDbContext>(SampleDatabase.CreateConnection, true, ConfigureCommand);

            var records = executor.ExecuteIteratorOnNewConnectionAsync(this, (@this, commandActivator, @context) => @this.EnumerateRecordsAsync(commandActivator, @context), context);

            int recordCount = 0;

            await foreach (var record in records.ConfigureAwait(false))
            {
                m_OutputHelper.WriteLine($"ID = {record["ID"]}, NAME = {record["NAME"]}");
                recordCount++;
            }

            Assert.Equal(5, recordCount);
        }

        /// <summary>
        /// Tests executing a command to get records and expects an InvalidOperationException.
        /// </summary>
        [Fact]
        public async Task ExecuteOnNewConnectionAsync_EnumerateRecords()
        {
            using var connection = SampleDatabase.CreateConnection();

            connection.Open();

            var context = new SampleDbContext(connection);

            var executor = new DbCommandExecutorWithContext<SampleDbContext>(SampleDatabase.CreateConnection, true, ConfigureCommand);

            await Assert.ThrowsAsync<InvalidOperationException>(async () =>
            {
                var reader = await executor.ExecuteOnNewConnectionAsync(GetRecordsAsync, context);

                int recordCount = 0;

                // Connection has been closed
                while(reader.Read())
                {
                    m_OutputHelper.WriteLine($"ID = {reader["ID"]}, NAME = {reader["NAME"]}");
                    recordCount++;
                }

                Assert.Equal(5, recordCount);
            });
        }

        /// <summary>
        /// Tests executing a command with state to get records and expects an InvalidOperationException.
        /// </summary>
        [Fact]
        public async Task ExecuteOnNewConnectionAsync_State_EnumerateRecords()
        {
            using var connection = SampleDatabase.CreateConnection();

            connection.Open();

            var context = new SampleDbContext(connection);

            var executor = new DbCommandExecutorWithContext<SampleDbContext>(SampleDatabase.CreateConnection, true, ConfigureCommand);

            await Assert.ThrowsAsync<InvalidOperationException>(async () =>
            {
                var reader = await executor.ExecuteOnNewConnectionAsync(this, (@this, commandActivator, @context) => @this.GetRecordsAsync(commandActivator, @context), context);

                int recordCount = 0;

                // Connection has been closed
                while (reader.Read())
                {
                    m_OutputHelper.WriteLine($"ID = {reader["ID"]}, NAME = {reader["NAME"]}");
                    recordCount++;
                }

                Assert.Equal(5, recordCount);
            });
        }

        /// <summary>
        /// Tests executing a command to get the count of records and verifies the count.
        /// </summary>
        [Fact]
        public async Task ExecuteOnNewConnectionAsync_GetCount()
        {
            using var connection = SampleDatabase.CreateConnection();

            connection.Open();

            var context = new SampleDbContext(connection);

            var executor = new DbCommandExecutorWithContext<SampleDbContext>(SampleDatabase.CreateConnection, true, ConfigureCommand);

            var count = await executor.ExecuteOnNewConnectionAsync(GetCountAsync, context);

            m_OutputHelper.WriteLine($"Count = {count}");

            Assert.Equal(5, count);
        }

        /// <summary>
        /// Tests executing a command with state to get the count of records and verifies the count.
        /// </summary>
        [Fact]
        public async Task ExecuteOnNewConnectionAsync_State_GetCount()
        {
            using var connection = SampleDatabase.CreateConnection();

            connection.Open();

            var context = new SampleDbContext(connection);

            var executor = new DbCommandExecutorWithContext<SampleDbContext>(SampleDatabase.CreateConnection, true, ConfigureCommand);

            var count = await executor.ExecuteOnNewConnectionAsync(this, (@this, commandActivator, @context) => @this.GetCountAsync(commandActivator, @context), context);

            m_OutputHelper.WriteLine($"Count = {count}");

            Assert.Equal(5, count);
        }

        /// <summary>
        /// Tests executing a command to insert a record and expects a SqlException.
        /// </summary>
        [Fact]
        public async Task ExecuteOnNewConnectionAsync_Insert()
        {
            using var connection = SampleDatabase.CreateConnection();

            connection.Open();

            var context = new SampleDbContext(connection);

            var executor = new DbCommandExecutorWithContext<SampleDbContext>(SampleDatabase.CreateConnection, true, ConfigureCommand);

            await Assert.ThrowsAsync<SqlException>(async () =>
            {
                // An exception will be thrown and the update will be rolled back.
                await executor.ExecuteOnNewConnectionAsync(InsertFailedAsync, context);
            });

            var count = executor.ExecuteOnNewConnection(GetCount, context);

            Assert.Equal(5, count);
        }

        /// <summary>
        /// Tests executing a command with state to insert a record and expects a SqlException.
        /// </summary>
        [Fact]
        public async Task ExecuteOnNewConnectionAsync_State_Insert()
        {
            using var connection = SampleDatabase.CreateConnection();

            connection.Open();

            var context = new SampleDbContext(connection);

            var executor = new DbCommandExecutorWithContext<SampleDbContext>(SampleDatabase.CreateConnection, true, ConfigureCommand);

            await Assert.ThrowsAsync<SqlException>(async () =>
            {
                // An exception will be thrown and the update will be rolled back.
                await executor.ExecuteOnNewConnectionAsync(this, (@this, commandActivator, @context) => @this.InsertFailedAsync(commandActivator, @context), context);
            });

            var count = await executor.ExecuteOnNewConnectionAsync(this, (@this, commandActivator, @context) => @this.GetCountAsync(commandActivator, @context), context);

            Assert.Equal(5, count);
        }

        #endregion

        #region ExecuteOnConnection

        /// <summary>
        /// Tests executing a command on an open connection to get records and verifies the record count.
        /// </summary>
        [Fact]
        public void ExecuteOnConnection_EnumerateRecords()
        {
            var executor = new DbCommandExecutorWithContext<SampleDbContext>(SampleDatabase.CreateConnection, true, ConfigureCommand);

            using var connection = SampleDatabase.CreateConnection();

            connection.Open();

            var context = new SampleDbContext(connection);

            var records = executor.ExecuteOnConnection(connection, EnumerateRecords, context);

            int recordCount = 0;

            foreach (var record in records)
            {
                m_OutputHelper.WriteLine($"ID = {record["ID"]}, NAME = {record["NAME"]}");
                recordCount++;
            }

            Assert.Equal(5, recordCount);
        }

        /// <summary>
        /// Tests executing a command with state on an open connection to get records and verifies the record count.
        /// </summary>
        [Fact]
        public void ExecuteOnConnection_State_EnumerateRecords()
        {
            var executor = new DbCommandExecutorWithContext<SampleDbContext>(SampleDatabase.CreateConnection, true, ConfigureCommand);

            using var connection = SampleDatabase.CreateConnection();

            connection.Open();

            var context = new SampleDbContext(connection);

            var records = executor.ExecuteOnConnection(this, connection, (@this, commandActivator, @context) => @this.EnumerateRecords(commandActivator, @context), context);

            int recordCount = 0;

            foreach (var record in records)
            {
                m_OutputHelper.WriteLine($"ID = {record["ID"]}, NAME = {record["NAME"]}");
                recordCount++;
            }

            Assert.Equal(5, recordCount);
        }

        /// <summary>
        /// Tests executing a command on an open connection to get the count of records and verifies the count.
        /// </summary>
        [Fact]
        public void ExecuteOnConnection_GetCount()
        {
            var executor = new DbCommandExecutorWithContext<SampleDbContext>(SampleDatabase.CreateConnection, true, ConfigureCommand);

            using var connection = SampleDatabase.CreateConnection();

            connection.Open();

            var context = new SampleDbContext(connection);

            var count = executor.ExecuteOnConnection(connection, GetCount, context);

            m_OutputHelper.WriteLine($"Count = {count}");

            Assert.Equal(5, count);
        }

        /// <summary>
        /// Tests executing a command with state on an open connection to get the count of records and verifies the count.
        /// </summary>
        [Fact]
        public void ExecuteOnConnection_State_GetCount()
        {
            var executor = new DbCommandExecutorWithContext<SampleDbContext>(SampleDatabase.CreateConnection, true, ConfigureCommand);

            using var connection = SampleDatabase.CreateConnection();

            connection.Open();

            var context = new SampleDbContext(connection);

            var count = executor.ExecuteOnConnection(this, connection, (@this, commandActivator, @context) => @this.GetCount(commandActivator, @context), context);

            m_OutputHelper.WriteLine($"Count = {count}");

            Assert.Equal(5, count);
        }

        /// <summary>
        /// Tests executing a command on an open connection to insert a record and expects a SqlException.
        /// </summary>
        [Fact]
        public void ExecuteOnConnection_Insert()
        {
            var executor = new DbCommandExecutorWithContext<SampleDbContext>(SampleDatabase.CreateConnection, true, ConfigureCommand);

            using var connection = SampleDatabase.CreateConnection();

            connection.Open();

            var context = new SampleDbContext(connection);

            try
            {
                Assert.Throws<SqlException>(() =>
                {
                    // An exception will be thrown and the update will not be rolled back.
                    executor.ExecuteOnConnection(connection, InsertFailed, context);
                });

                var count = executor.ExecuteOnConnection(connection, GetCount, context);    

                Assert.Equal(6, count);
            }
            finally
            {
                executor.ExecuteOnConnection(connection, Delete, context    );
            }
        }

        /// <summary>
        /// Tests executing a command with state on an open connection to insert a record and expects a SqlException.
        /// </summary>
        [Fact]
        public void ExecuteOnConnection_State_Insert()
        {
            var executor = new DbCommandExecutorWithContext<SampleDbContext>(SampleDatabase.CreateConnection, true, ConfigureCommand);

            using var connection = SampleDatabase.CreateConnection();

            connection.Open();

            var context = new SampleDbContext(connection);

            try
            {
                Assert.Throws<SqlException>(() =>
                {
                    // An exception will be thrown and the update will not be rolled back.
                    executor.ExecuteOnConnection(this, connection, (@this, commandActivator, @context) => @this.InsertFailed(commandActivator, @context), context);
                });

                var count = executor.ExecuteOnConnection(this, connection, (@this, commandActivator, @context) => @this.GetCount(commandActivator, @context), context);

                Assert.Equal(6, count);
            }
            finally
            {
                executor.ExecuteOnConnection(this, connection, (@this, commandActivator, @context) => @this.Delete(commandActivator, @context), context);
            }
        }

        #endregion

        #region ExecuteOnConnection

        /// <summary>
        /// Tests executing a command on an open connection to get records and verifies the record count.
        /// </summary>
        [Fact]
        public async Task ExecuteOnConnectionAsync_EnumerateRecords()
        {
            var executor = new DbCommandExecutorWithContext<SampleDbContext>(SampleDatabase.CreateConnection, true, ConfigureCommand);

            using var connection = SampleDatabase.CreateConnection();

            connection.Open();

            var context = new SampleDbContext(connection);

            var records = executor.ExecuteIteratorOnConnectionAsync(connection, EnumerateRecordsAsync, context);

            int recordCount = 0;

            await foreach (var record in records.ConfigureAwait(false))
            {
                m_OutputHelper.WriteLine($"ID = {record["ID"]}, NAME = {record["NAME"]}");
                recordCount++;
            }

            Assert.Equal(5, recordCount);
        }

        /// <summary>
        /// Tests executing a command with state on an open connection to get records and verifies the record count.
        /// </summary>
        [Fact]
        public async Task ExecuteOnConnectionAsync_State_EnumerateRecords()
        {
            var executor = new DbCommandExecutorWithContext<SampleDbContext>(SampleDatabase.CreateConnection, true, ConfigureCommand);

            using var connection = SampleDatabase.CreateConnection();

            connection.Open();

            var context = new SampleDbContext(connection);

            var records = executor.ExecuteIteratorOnConnectionAsync(this, connection, (@this, commandActivator, @context) => @this.EnumerateRecordsAsync(commandActivator, @context), context);

            int recordCount = 0;

            await foreach (var record in records.ConfigureAwait(false))
            {
                m_OutputHelper.WriteLine($"ID = {record["ID"]}, NAME = {record["NAME"]}");
                recordCount++;
            }

            Assert.Equal(5, recordCount);
        }

        /// <summary>
        /// Tests executing a command on an open connection to get the count of records and verifies the count.
        /// </summary>
        [Fact]
        public async Task ExecuteOnConnectionAsync_GetCount()
        {
            var executor = new DbCommandExecutorWithContext<SampleDbContext>(SampleDatabase.CreateConnection, true, ConfigureCommand);

            using var connection = SampleDatabase.CreateConnection();

            connection.Open();

            var context = new SampleDbContext(connection);

            var count = await executor.ExecuteOnConnectionAsync(connection, GetCountAsync, context);

            m_OutputHelper.WriteLine($"Count = {count}");

            Assert.Equal(5, count);
        }

        /// <summary>
        /// Tests executing a command with state on an open connection to get the count of records and verifies the count.
        /// </summary>
        [Fact]
        public async Task ExecuteOnConnectionAsync_State_GetCount()
        {
            var executor = new DbCommandExecutorWithContext<SampleDbContext>(SampleDatabase.CreateConnection, true, ConfigureCommand);

            using var connection = SampleDatabase.CreateConnection();

            connection.Open();

            var context = new SampleDbContext(connection);

            var count = await executor.ExecuteOnConnectionAsync(this, connection, (@this, commandActivator, @context) => @this.GetCountAsync(commandActivator, @context), context);

            m_OutputHelper.WriteLine($"Count = {count}");

            Assert.Equal(5, count);
        }

        /// <summary>
        /// Tests executing a command on an open connection to insert a record and expects a SqlException.
        /// </summary>
        [Fact]
        public async Task ExecuteOnConnectionAsync_Insert()
        {
            var executor = new DbCommandExecutorWithContext<SampleDbContext>(SampleDatabase.CreateConnection, true, ConfigureCommand);

            using var connection = SampleDatabase.CreateConnection();

            connection.Open();

            var context = new SampleDbContext(connection);

            try
            {
                await Assert.ThrowsAsync<SqlException>(async () =>
                {
                    // An exception will be thrown and the update will not be rolled back.
                    await executor.ExecuteOnConnectionAsync(connection, InsertFailedAsync, context);
                });

                var count = await executor.ExecuteOnConnectionAsync(connection, GetCountAsync, context);

                Assert.Equal(6, count);
            }
            finally
            {
                await executor.ExecuteOnConnectionAsync(connection, DeleteAsync, context);
            }
        }

        /// <summary>
        /// Tests executing a command with state on an open connection to insert a record and expects a SqlException.
        /// </summary>
        [Fact]
        public async Task ExecuteOnConnectionAsync_State_Insert()
        {
            var executor = new DbCommandExecutorWithContext<SampleDbContext>(SampleDatabase.CreateConnection, true, ConfigureCommand);

            using var connection = SampleDatabase.CreateConnection();

            connection.Open();

            var context = new SampleDbContext(connection);

            try
            {
                await Assert.ThrowsAsync<SqlException>(async () =>
                {
                    // An exception will be thrown and the update will not be rolled back.
                    await executor.ExecuteOnConnectionAsync(this, connection, (@this, commandActivator, @context) => @this.InsertFailedAsync(commandActivator, @context), context);
                });

                var count = await executor.ExecuteOnConnectionAsync(this, connection, (@this, commandActivator, @context) => @this.GetCountAsync(commandActivator, @context), context);

                Assert.Equal(6, count);
            }
            finally
            {
                await executor.ExecuteOnConnectionAsync(this, connection, (@this, commandActivator, @context) => @this.DeleteAsync(commandActivator, @context), context);
            }
        }

        #endregion

        #region ExecuteOnTransaction

        /// <summary>
        /// Tests executing a command within a transaction to get records and verifies the record count.
        /// </summary>
        [Fact]
        public void ExecuteOnTransaction_EnumerateRecords()
        {
            var executor = new DbCommandExecutorWithContext<SampleDbContext>(SampleDatabase.CreateConnection, true, ConfigureCommand);

            using var connection = SampleDatabase.CreateConnection();

            connection.Open();

            using var transaction = connection.BeginTransaction();

            var context = new SampleDbContext(transaction);

            var records = executor.ExecuteOnTransaction(transaction, EnumerateRecords, context);

            int recordCount = 0;

            foreach (var record in records)
            {
                m_OutputHelper.WriteLine($"ID = {record["ID"]}, NAME = {record["NAME"]}");
                recordCount++;
            }

            Assert.Equal(5, recordCount);
        }

        /// <summary>
        /// Tests executing a command with state within a transaction to get records and verifies the record count.
        /// </summary>
        [Fact]
        public void ExecuteOnTransaction_State_EnumerateRecords()
        {
            var executor = new DbCommandExecutorWithContext<SampleDbContext>(SampleDatabase.CreateConnection, true, ConfigureCommand);

            using var connection = SampleDatabase.CreateConnection();

            connection.Open();

            using var transaction = connection.BeginTransaction();

            var context = new SampleDbContext(transaction);

            var records = executor.ExecuteOnTransaction(this, transaction, (@this, commandActivator, @context) => @this.EnumerateRecords(commandActivator, @context), context);

            int recordCount = 0;

            foreach (var record in records)
            {
                m_OutputHelper.WriteLine($"ID = {record["ID"]}, NAME = {record["NAME"]}");
                recordCount++;
            }

            Assert.Equal(5, recordCount);
        }

        /// <summary>
        /// Tests executing a command within a transaction to get the count of records and verifies the count.
        /// </summary>
        [Fact]
        public void ExecuteOnTransaction_GetCount()
        {
            var executor = new DbCommandExecutorWithContext<SampleDbContext>(SampleDatabase.CreateConnection, true, ConfigureCommand);

            using var connection = SampleDatabase.CreateConnection();

            connection.Open();

            using var transaction = connection.BeginTransaction();

            var context = new SampleDbContext(transaction);

            var count = executor.ExecuteOnTransaction(transaction, GetCount, context);

            m_OutputHelper.WriteLine($"Count = {count}");

            Assert.Equal(5, count);
        }

        /// <summary>
        /// Tests executing a command with state within a transaction to get the count of records and verifies the count.
        /// </summary>
        [Fact]
        public void ExecuteOnTransaction_State_GetCount()
        {
            var executor = new DbCommandExecutorWithContext<SampleDbContext>(SampleDatabase.CreateConnection, true, ConfigureCommand);

            using var connection = SampleDatabase.CreateConnection();

            connection.Open();

            using var transaction = connection.BeginTransaction();

            var context = new SampleDbContext(transaction);

            var count = executor.ExecuteOnTransaction(this, transaction, (@this, commandActivator, @context) => @this.GetCount(commandActivator, @context), context);

            m_OutputHelper.WriteLine($"Count = {count}");

            Assert.Equal(5, count);
        }

        /// <summary>
        /// Tests executing a command within a transaction to insert a record and expects a SqlException.
        /// </summary>
        [Fact]
        public void ExecuteOnTransaction_Insert()
        {
            var executor = new DbCommandExecutorWithContext<SampleDbContext>(SampleDatabase.CreateConnection, true, ConfigureCommand);

            using var connection = SampleDatabase.CreateConnection();

            connection.Open();

            using var transaction = connection.BeginTransaction();

            var context = new SampleDbContext(transaction);

            try
            {
                Assert.Throws<SqlException>(() =>
                {
                    // An exception will be thrown.
                    executor.ExecuteOnTransaction(transaction, InsertFailed, context);
                });
            }
            finally
            {
                transaction.Rollback();
            }

            var count = executor.ExecuteOnNewConnection(GetCount, context);

            Assert.Equal(5, count);
        }

        /// <summary>
        /// Tests executing a command with state within a transaction to insert a record and expects a SqlException.
        /// </summary>
        [Fact]
        public void ExecuteOnTransaction_State_Insert()
        {
            var executor = new DbCommandExecutorWithContext<SampleDbContext>(SampleDatabase.CreateConnection, true, ConfigureCommand);

            using var connection = SampleDatabase.CreateConnection();

            connection.Open();

            using var transaction = connection.BeginTransaction();

            var context = new SampleDbContext(transaction);

            try
            {
                Assert.Throws<SqlException>(() =>
                {
                    // An exception will be thrown.
                    executor.ExecuteOnTransaction(this, transaction, (@this, commandActivator, @context) => @this.InsertFailed(commandActivator, @context), context);
                });
            }
            finally
            {
                transaction.Rollback();
            }

            var count = executor.ExecuteOnNewConnection(this, (@this, commandActivator, @context) => @this.GetCount(commandActivator, @context), context);

            Assert.Equal(5, count);
        }

        #endregion

        #region ExecuteOnTransaction

        /// <summary>
        /// Tests executing a command within a transaction to get records and verifies the record count.
        /// </summary>
        [Fact]
        public async Task ExecuteOnTransactionAsync_EnumerateRecords()
        {
            var executor = new DbCommandExecutorWithContext<SampleDbContext>(SampleDatabase.CreateConnection, true, ConfigureCommand);

            using var connection = SampleDatabase.CreateConnection();

            connection.Open();

            using var transaction = connection.BeginTransaction();

            var context = new SampleDbContext(transaction);

            var records = executor.ExecuteIteratorOnTransactionAsync(transaction, EnumerateRecordsAsync, context);

            int recordCount = 0;

            await foreach (var record in records.ConfigureAwait(false))
            {
                m_OutputHelper.WriteLine($"ID = {record["ID"]}, NAME = {record["NAME"]}");
                recordCount++;
            }

            Assert.Equal(5, recordCount);
        }

        /// <summary>
        /// Tests executing a command with state within a transaction to get records and verifies the record count.
        /// </summary>
        [Fact]
        public async Task ExecuteOnTransactionAsync_State_EnumerateRecords()
        {
            var executor = new DbCommandExecutorWithContext<SampleDbContext>(SampleDatabase.CreateConnection, true, ConfigureCommand);

            using var connection = SampleDatabase.CreateConnection();

            connection.Open();

            using var transaction = connection.BeginTransaction();

            var context = new SampleDbContext(transaction);

            var records = executor.ExecuteIteratorOnTransactionAsync(this, transaction, (@this, commandActivator, @context) => @this.EnumerateRecordsAsync(commandActivator, @context), context);

            int recordCount = 0;

            await foreach (var record in records.ConfigureAwait(false))
            {
                m_OutputHelper.WriteLine($"ID = {record["ID"]}, NAME = {record["NAME"]}");
                recordCount++;
            }

            Assert.Equal(5, recordCount);
        }

        /// <summary>
        /// Tests executing a command within a transaction to get the count of records and verifies the count.
        /// </summary>
        [Fact]
        public async Task ExecuteOnTransactionAsync_GetCount()
        {
            var executor = new DbCommandExecutorWithContext<SampleDbContext>(SampleDatabase.CreateConnection, true, ConfigureCommand);

            using var connection = SampleDatabase.CreateConnection();

            connection.Open();

            using var transaction = connection.BeginTransaction();

            var context = new SampleDbContext(transaction);

            var count = await executor.ExecuteOnTransactionAsync(transaction, GetCountAsync, context);

            m_OutputHelper.WriteLine($"Count = {count}");

            Assert.Equal(5, count);
        }

        /// <summary>
        /// Tests executing a command with state within a transaction to get the count of records and verifies the count.
        /// </summary>
        [Fact]
        public async Task ExecuteOnTransactionAsync_State_GetCount()
        {
            var executor = new DbCommandExecutorWithContext<SampleDbContext>(SampleDatabase.CreateConnection, true, ConfigureCommand);

            using var connection = SampleDatabase.CreateConnection();

            connection.Open();

            using var transaction = connection.BeginTransaction();

            var context = new SampleDbContext(transaction);

            var count = await executor.ExecuteOnTransactionAsync(this, transaction, (@this, commandActivator, @context) => @this.GetCountAsync(commandActivator, @context), context);

            m_OutputHelper.WriteLine($"Count = {count}");

            Assert.Equal(5, count);
        }

        /// <summary>
        /// Tests executing a command within a transaction to insert a record and expects a SqlException.
        /// </summary>
        [Fact]
        public async Task ExecuteOnTransactionAsync_Insert()
        {
            var executor = new DbCommandExecutorWithContext<SampleDbContext>(SampleDatabase.CreateConnection, true, ConfigureCommand);

            using var connection = SampleDatabase.CreateConnection();

            connection.Open();

            using var transaction = connection.BeginTransaction();

            var context = new SampleDbContext(transaction);

            try
            {
                await Assert.ThrowsAsync<SqlException>(async () =>
                {
                    // An exception will be thrown.
                    await executor.ExecuteOnTransactionAsync(transaction, InsertFailedAsync, context);
                });
            }
            finally
            {
                transaction.Rollback();
            }

            var count = await executor.ExecuteOnNewConnectionAsync(GetCountAsync, context);

            Assert.Equal(5, count);
        }

        /// <summary>
        /// Tests executing a command with state within a transaction to insert a record and expects a SqlException.
        /// </summary>
        [Fact]
        public async Task ExecuteOnTransactionAsync_State_Insert()
        {
            var executor = new DbCommandExecutorWithContext<SampleDbContext>(SampleDatabase.CreateConnection, true, ConfigureCommand);

            using var connection = SampleDatabase.CreateConnection();

            connection.Open();

            using var transaction = connection.BeginTransaction();

            var context = new SampleDbContext(transaction);

            try
            {
                await Assert.ThrowsAsync<SqlException>(async () =>
                {
                    // An exception will be thrown.
                    await executor.ExecuteOnTransactionAsync(this, transaction, (@this, commandActivator, @context) => @this.InsertFailedAsync(commandActivator, @context), context);
                });
            }
            finally
            {
                transaction.Rollback();
            }

            var count = await executor.ExecuteOnNewConnectionAsync(this, (@this, commandActivator, @context) => @this.GetCountAsync(commandActivator, @context), context);

            Assert.Equal(5, count);
        }

        #endregion

        /// <summary>
        /// Configure the command.
        /// </summary>
        /// <param name="command">The command.</param>
        private void ConfigureCommand(IDbCommand command)
        {
            command.CommandTimeout = 60;
        }

        /// <summary>
        /// Gets records from the database.
        /// </summary>
        /// <param name="commandActivator">The command activator.</param>
        /// <returns>The records.</returns>
        private IDataReader GetRecords(Func<IDbCommand> commandActivator, SampleDbContext context)
        {
            using var command = commandActivator();

            Assert.Equal(60, command.CommandTimeout);

            command.CommandText = "select * from SAMPLE_TABLE";

            return command.ExecuteReader();
        }

        /// <summary>
        /// Gets records from the database.
        /// </summary>
        /// <param name="commandActivator">The command activator.</param>
        /// <returns>The records.</returns>
        private ValueTask<IDataReader> GetRecordsAsync(Func<IDbCommand> commandActivator, SampleDbContext context)
        {
            using var command = commandActivator();

            Assert.Equal(60, command.CommandTimeout);

            command.CommandText = "select * from SAMPLE_TABLE";

            return command.ExecuteReaderAsync();
        }

        /// <summary>
        /// Gets records from the database.
        /// </summary>
        /// <param name="commandActivator">The command activator.</param>
        /// <returns>The records.</returns>
        private IEnumerable<IDataRecord> EnumerateRecords(Func<IDbCommand> commandActivator, SampleDbContext context)
        {
            using var command = commandActivator();

            Assert.Equal(60, command.CommandTimeout);

            command.CommandText = "select * from SAMPLE_TABLE";

            using (var reader = command.ExecuteReader())
            {
                while (reader.Read())
                {
                    yield return reader;
                }
            }
        }

        /// <summary>
        /// Gets records from the database.
        /// </summary>
        /// <param name="commandActivator">The command activator.</param>
        /// <returns>The records.</returns>
        private async IAsyncEnumerable<IDataRecord> EnumerateRecordsAsync(Func<IDbCommand> commandActivator, SampleDbContext context)
        {
            using var command = commandActivator();

            Assert.Equal(60, command.CommandTimeout);

            command.CommandText = "select * from SAMPLE_TABLE";

            using (var reader = await command.ExecuteReaderAsync())
            {
                while (reader.Read())
                {
                    yield return reader;
                }
            }
        }

        /// <summary>
        /// Gets the count of records from the database.
        /// </summary>
        /// <param name="commandActivator">The command activator.</param>
        /// <returns>The count of records.</returns>
        private int GetCount(Func<IDbCommand> commandActivator, SampleDbContext context)
        {
            using var command = commandActivator();

            Assert.Equal(60, command.CommandTimeout);

            command.CommandText = "select count(*) from SAMPLE_TABLE";

            return (int)command.ExecuteScalar()!;
        }

        /// <summary>
        /// Gets the count of records from the database.
        /// </summary>
        /// <param name="commandActivator">The command activator.</param>
        /// <returns>The count of records.</returns>
        private async Task<int> GetCountAsync(Func<IDbCommand> commandActivator, SampleDbContext context)
        {
            using var command = commandActivator();

            Assert.Equal(60, command.CommandTimeout);

            command.CommandText = "select count(*) from SAMPLE_TABLE";

            return (int)await command.ExecuteScalarAsync();
        }

        /// <summary>
        /// Inserts a record into the database.
        /// </summary>
        /// <param name="commandActivator">The command activator.</param>
        private void Insert(Func<IDbCommand> commandActivator, SampleDbContext context)
        {
            using var command = commandActivator();

            Assert.Equal(60, command.CommandTimeout);

            command.CommandText = "insert into SAMPLE_TABLE values (6, 'Name6')";

            command.ExecuteNonQuery();
        }

        /// <summary>
        /// Inserts a record into the database.
        /// </summary>
        /// <param name="commandActivator">The command activator.</param>
        private async ValueTask InsertAsync(Func<IDbCommand> commandActivator, SampleDbContext context)
        {
            using var command = commandActivator();

            Assert.Equal(60, command.CommandTimeout);

            command.CommandText = "insert into SAMPLE_TABLE values (6, 'Name6')";

            await command.ExecuteNonQueryAsync();
        }

        /// <summary>
        /// Attempts to insert a record into the database and throws an exception.
        /// </summary>
        /// <param name="commandActivator">The command activator.</param>
        private void InsertFailed(Func<IDbCommand> commandActivator, SampleDbContext context)
        {
            using var command = commandActivator();

            Assert.Equal(60, command.CommandTimeout);

            command.CommandText = "insert into SAMPLE_TABLE values (6, 'Name6')";

            command.ExecuteNonQuery();

            // An exception will be thrown.
            command.ExecuteNonQuery();
        }

        /// <summary>
        /// Attempts to insert a record into the database and throws an exception.
        /// </summary>
        /// <param name="commandActivator">The command activator.</param>
        private async ValueTask InsertFailedAsync(Func<IDbCommand> commandActivator, SampleDbContext context)
        {
            using var command = commandActivator();

            Assert.Equal(60, command.CommandTimeout);

            command.CommandText = "insert into SAMPLE_TABLE values (6, 'Name6')";

            await command.ExecuteNonQueryAsync();

            // An exception will be thrown.
            await command.ExecuteNonQueryAsync();
        }

        /// <summary>
        /// Deletes a record from the database.
        /// </summary>
        /// <param name="commandActivator">The command activator.</param>
        private void Delete(Func<IDbCommand> commandActivator, SampleDbContext context)
        {
            using var command = commandActivator();

            Assert.Equal(60, command.CommandTimeout);

            command.CommandText = "delete from SAMPLE_TABLE where ID = 6";

            command.ExecuteNonQuery();
        }

        /// <summary>
        /// Deletes a record from the database.
        /// </summary>
        /// <param name="commandActivator">The command activator.</param>
        private async ValueTask DeleteAsync(Func<IDbCommand> commandActivator, SampleDbContext context)
        {
            using var command = commandActivator();

            Assert.Equal(60, command.CommandTimeout);

            command.CommandText = "delete from SAMPLE_TABLE where ID = 6";

            await command.ExecuteNonQueryAsync();
        }
    }
}
