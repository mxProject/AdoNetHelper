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
    /// Testing <see cref="DbCommandExecutor"/>.
    /// </summary>
    public class TestDbCommandExecutor
    {
        public TestDbCommandExecutor(ITestOutputHelper outputHelper)
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
            var executor = new DbCommandExecutor(SampleDatabase.CreateConnection, true, ConfigureCommand);

            int recordCount = 0;

            using (var transactionScope = new TransactionScope())
            {
                // get records.
                var records = executor.ExecuteIteratorOnNewConnection(EnumerateRecords);

                foreach (var record in records)
                {
                    m_OutputHelper.WriteLine($"ID = {record["ID"]}, NAME = {record["NAME"]}");
                    recordCount++;
                }

                Assert.Equal(5, recordCount);

                // insert 
                executor.ExecuteOnNewConnection(Insert);

                // get count
                recordCount = executor.ExecuteOnNewConnection(GetCount);

                Assert.Equal(6, recordCount);

                // rollback
                transactionScope.Dispose();
            }

            // get count
            recordCount = executor.ExecuteOnNewConnection(GetCount);

            Assert.Equal(5, recordCount);
        }

        /// <summary>
        /// Tests executing a command within a TransactionScope and verifies the rollback.
        /// </summary>
        [Fact]
        public async Task ExecuteIteratorOnNewConnectionAsync_TransactionScope()
        {
            var executor = new DbCommandExecutor(SampleDatabase.CreateConnection, true, ConfigureCommand);

            int recordCount = 0;

            using (var transactionScope = new TransactionScope())
            {
                // get records.
                var records = executor.ExecuteIteratorOnNewConnectionAsync(EnumerateRecordsAsync);

                await foreach (var record in records.ConfigureAwait(false))
                {
                    m_OutputHelper.WriteLine($"ID = {record["ID"]}, NAME = {record["NAME"]}");
                    recordCount++;
                }

                Assert.Equal(5, recordCount);

                // insert 
                await executor.ExecuteOnNewConnectionAsync(InsertAsync);

                // get count
                recordCount = await executor.ExecuteOnNewConnectionAsync(GetCountAsync);

                Assert.Equal(6, recordCount);

                // rollback
                transactionScope.Dispose();
            }

            // get count
            recordCount = executor.ExecuteOnNewConnection(GetCount);

            Assert.Equal(5, recordCount);
        }

        /// <summary>
        /// Tests executing a command with state within a TransactionScope and verifies the rollback.
        /// </summary>
        [Fact]
        public void ExecuteIteratorOnNewConnection_State_TransactionScope()
        {
            var executor = new DbCommandExecutor(SampleDatabase.CreateConnection, true, ConfigureCommand);

            int recordCount = 0;

            using (var transactionScope = new TransactionScope())
            {
                // get records.
                var records = executor.ExecuteIteratorOnNewConnection(this, (@this, commandActivator) => @this.EnumerateRecords(commandActivator));

                foreach (var record in records)
                {
                    m_OutputHelper.WriteLine($"ID = {record["ID"]}, NAME = {record["NAME"]}");
                    recordCount++;
                }

                Assert.Equal(5, recordCount);

                // insert 
                executor.ExecuteOnNewConnection(this, (@this, commandActivator) => @this.Insert(commandActivator));

                // get count
                recordCount = executor.ExecuteOnNewConnection(this, (@this, commandActivator) => @this.GetCount(commandActivator));

                Assert.Equal(6, recordCount);

                // rollback
                transactionScope.Dispose();
            }

            // get count
            recordCount = executor.ExecuteOnNewConnection(this, (@this, commandActivator) => @this.GetCount(commandActivator));

            Assert.Equal(5, recordCount);
        }

        /// <summary>
        /// Tests executing a command with state within a TransactionScope and verifies the rollback.
        /// </summary>
        [Fact]
        public async Task ExecuteIteratorOnNewConnectionAsync_State_TransactionScope()
        {
            var executor = new DbCommandExecutor(SampleDatabase.CreateConnection, true, ConfigureCommand);

            int recordCount = 0;

            using (var transactionScope = new TransactionScope())
            {
                // get records.
                var records = executor.ExecuteIteratorOnNewConnectionAsync(this, (@this, commandActivator) => @this.EnumerateRecordsAsync(commandActivator));

                await foreach (var record in records.ConfigureAwait(false))
                {
                    m_OutputHelper.WriteLine($"ID = {record["ID"]}, NAME = {record["NAME"]}");
                    recordCount++;
                }

                Assert.Equal(5, recordCount);

                // insert 
                await executor.ExecuteOnNewConnectionAsync(this, (@this, commandActivator) => @this.InsertAsync(commandActivator));

                // get count
                recordCount = await executor.ExecuteOnNewConnectionAsync(this, (@this, commandActivator) => @this.GetCountAsync(commandActivator));

                Assert.Equal(6, recordCount);

                // rollback
                transactionScope.Dispose();
            }

            // get count
            recordCount = await executor.ExecuteOnNewConnectionAsync(this, (@this, commandActivator) => @this.GetCountAsync(commandActivator));

            Assert.Equal(5, recordCount);
        }

        #endregion

        #region ExecuteIteratorOnNewConnection

        /// <summary>
        /// Tests executing a command to get records and verifies the record count.
        /// </summary>
        [Fact]
        public void ExecuteIteratorOnNewConnection_GetRecords()
        {
            var executor = new DbCommandExecutor(SampleDatabase.CreateConnection, true, ConfigureCommand);

            var records = executor.ExecuteIteratorOnNewConnection(EnumerateRecords);

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
            var executor = new DbCommandExecutor(SampleDatabase.CreateConnection, true, ConfigureCommand);

            var records = executor.ExecuteIteratorOnNewConnection(this, (@this, commandActivator) => @this.EnumerateRecords(commandActivator));

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
            var executor = new DbCommandExecutor(SampleDatabase.CreateConnection, true, ConfigureCommand);

            Assert.Throws<InvalidOperationException>(() =>
            {
                var records = executor.ExecuteOnNewConnection(EnumerateRecords);

                int recordCount = 0;

                // Connection has been closed
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
            var executor = new DbCommandExecutor(SampleDatabase.CreateConnection, true, ConfigureCommand);

            Assert.Throws<InvalidOperationException>(() =>
            {
                var records = executor.ExecuteOnNewConnection(this, (@this, commandActivator) => @this.EnumerateRecords(commandActivator));

                int recordCount = 0;

                // Connection has been closed
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
            var executor = new DbCommandExecutor(SampleDatabase.CreateConnection, true, ConfigureCommand);

            var count = executor.ExecuteOnNewConnection(GetCount);

            m_OutputHelper.WriteLine($"Count = {count}");

            Assert.Equal(5, count);
        }

        /// <summary>
        /// Tests executing a command with state to get the count of records and verifies the count.
        /// </summary>
        [Fact]
        public void ExecuteOnNewConnection_State_GetCount()
        {
            var executor = new DbCommandExecutor(SampleDatabase.CreateConnection, true, ConfigureCommand);

            var count = executor.ExecuteOnNewConnection(this, (@this, commandActivator) => @this.GetCount(commandActivator));

            m_OutputHelper.WriteLine($"Count = {count}");

            Assert.Equal(5, count);
        }

        /// <summary>
        /// Tests executing a command to insert a record and expects a SqlException.
        /// </summary>
        [Fact]
        public void ExecuteOnNewConnection_Insert()
        {
            var executor = new DbCommandExecutor(SampleDatabase.CreateConnection, true, ConfigureCommand);

            Assert.Throws<SqlException>(() =>
            {
                // An exception will be thrown and the update will be rolled back.
                executor.ExecuteOnNewConnection(InsertFailed);
            });

            var count = executor.ExecuteOnNewConnection(GetCount);

            Assert.Equal(5, count);
        }

        /// <summary>
        /// Tests executing a command with state to insert a record and expects a SqlException.
        /// </summary>
        [Fact]
        public void ExecuteOnNewConnection_State_Insert()
        {
            var executor = new DbCommandExecutor(SampleDatabase.CreateConnection, true, ConfigureCommand);

            Assert.Throws<SqlException>(() =>
            {
                // An exception will be thrown and the update will be rolled back.
                executor.ExecuteOnNewConnection(this, (@this, commandActivator) => @this.InsertFailed(commandActivator));
            });

            var count = executor.ExecuteOnNewConnection(this, (@this, commandActivator) => @this.GetCount(commandActivator));

            Assert.Equal(5, count);
        }

        #endregion

        #region ExecuteIteratorOnNewConnectionAsync

        /// <summary>
        /// Tests executing a command to get records and verifies the record count.
        /// </summary>
        [Fact]
        public async Task ExecuteIteratorOnNewConnectionAsync_GetRecords()
        {
            var executor = new DbCommandExecutor(SampleDatabase.CreateConnection, true, ConfigureCommand);

            var records = executor.ExecuteIteratorOnNewConnectionAsync(EnumerateRecordsAsync);

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
        public async Task ExecuteIteratorOnNewConnectionAsync_State_GetRecords()
        {
            var executor = new DbCommandExecutor(SampleDatabase.CreateConnection, true, ConfigureCommand);

            var records = executor.ExecuteIteratorOnNewConnectionAsync(this, (@this, commandActivator) => @this.EnumerateRecordsAsync(commandActivator));

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
        public async Task ExecuteOnNewConnectionAsync_GetRecords()
        {
            var executor = new DbCommandExecutor(SampleDatabase.CreateConnection, true, ConfigureCommand);

            await Assert.ThrowsAsync<InvalidOperationException>(async () =>
            {
                var reader = await executor.ExecuteOnNewConnectionAsync(GetRecordsAsync);

                int recordCount = 0;

                // Connection has been closed
                while (await reader.ReadAsync())
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
        public async Task ExecuteOnNewConnectionAsync_State_GetRecords()
        {
            var executor = new DbCommandExecutor(SampleDatabase.CreateConnection, true, ConfigureCommand);

            await Assert.ThrowsAsync<InvalidOperationException>(async () =>
            {
                var reader = await executor.ExecuteOnNewConnectionAsync(this, (@this, commandActivator) => @this.GetRecordsAsync(commandActivator));

                int recordCount = 0;

                // Connection has been closed
                while (await reader.ReadAsync())
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
            var executor = new DbCommandExecutor(SampleDatabase.CreateConnection, true, ConfigureCommand);

            var count = await executor.ExecuteOnNewConnectionAsync(GetCountAsync);

            m_OutputHelper.WriteLine($"Count = {count}");

            Assert.Equal(5, count);
        }

        /// <summary>
        /// Tests executing a command with state to get the count of records and verifies the count.
        /// </summary>
        [Fact]
        public async Task ExecuteOnNewConnectionAsync_State_GetCount()
        {
            var executor = new DbCommandExecutor(SampleDatabase.CreateConnection, true, ConfigureCommand);

            var count = await executor.ExecuteOnNewConnectionAsync(this, (@this, commandActivator) => @this.GetCountAsync(commandActivator));

            m_OutputHelper.WriteLine($"Count = {count}");

            Assert.Equal(5, count);
        }

        /// <summary>
        /// Tests executing a command to insert a record and expects a SqlException.
        /// </summary>
        [Fact]
        public async Task ExecuteOnNewConnectionAsync_Insert()
        {
            var executor = new DbCommandExecutor(SampleDatabase.CreateConnection, true, ConfigureCommand);

            await Assert.ThrowsAsync<SqlException>(async () =>
            {
                // An exception will be thrown and the update will be rolled back.
                await executor.ExecuteOnNewConnectionAsync(InsertFailedAsync);
            });

            var count = await executor.ExecuteOnNewConnectionAsync(GetCountAsync);

            Assert.Equal(5, count);
        }

        /// <summary>
        /// Tests executing a command with state to insert a record and expects a SqlException.
        /// </summary>
        [Fact]
        public async Task ExecuteOnNewConnectionAsync_State_Insert()
        {
            var executor = new DbCommandExecutor(SampleDatabase.CreateConnection, true, ConfigureCommand);

            await Assert.ThrowsAsync<SqlException>(async () =>
            {
                // An exception will be thrown and the update will be rolled back.
                await executor.ExecuteOnNewConnectionAsync(this, (@this, commandActivator) => @this.InsertFailedAsync(commandActivator));
            });

            var count = await executor.ExecuteOnNewConnectionAsync(this, (@this, commandActivator) => @this.GetCountAsync(commandActivator));

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
            var executor = new DbCommandExecutor(SampleDatabase.CreateConnection, true, ConfigureCommand);

            using var connection = SampleDatabase.CreateConnection();

            connection.Open();

            var records = executor.ExecuteOnConnection(connection, EnumerateRecords);

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
            var executor = new DbCommandExecutor(SampleDatabase.CreateConnection, true, ConfigureCommand);

            using var connection = SampleDatabase.CreateConnection();

            connection.Open();

            var records = executor.ExecuteOnConnection(this, connection, (@this, commandActivator) => @this.EnumerateRecords(commandActivator));

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
            var executor = new DbCommandExecutor(SampleDatabase.CreateConnection, true, ConfigureCommand);

            using var connection = SampleDatabase.CreateConnection();

            connection.Open();

            var count = executor.ExecuteOnConnection(connection, GetCount);

            m_OutputHelper.WriteLine($"Count = {count}");

            Assert.Equal(5, count);
        }

        /// <summary>
        /// Tests executing a command with state on an open connection to get the count of records and verifies the count.
        /// </summary>
        [Fact]
        public void ExecuteOnConnection_State_GetCount()
        {
            var executor = new DbCommandExecutor(SampleDatabase.CreateConnection, true, ConfigureCommand);

            using var connection = SampleDatabase.CreateConnection();

            connection.Open();

            var count = executor.ExecuteOnConnection(this, connection, (@this, commandActivator) => @this.GetCount(commandActivator));

            m_OutputHelper.WriteLine($"Count = {count}");

            Assert.Equal(5, count);
        }

        /// <summary>
        /// Tests executing a command on an open connection to insert a record and expects a SqlException.
        /// </summary>
        [Fact]
        public void ExecuteOnConnection_Insert()
        {
            var executor = new DbCommandExecutor(SampleDatabase.CreateConnection, true, ConfigureCommand);

            using var connection = SampleDatabase.CreateConnection();

            connection.Open();

            try
            {
                Assert.Throws<SqlException>(() =>
                {
                    // An exception will be thrown and the update will not be rolled back.
                    executor.ExecuteOnConnection(connection, InsertFailed);
                });

                var count = executor.ExecuteOnConnection(connection, GetCount);

                Assert.Equal(6, count);
            }
            finally
            {
                executor.ExecuteOnConnection(connection, Delete);
            }
        }

        /// <summary>
        /// Tests executing a command with state on an open connection to insert a record and expects a SqlException.
        /// </summary>
        [Fact]
        public void ExecuteOnConnection_State_Insert()
        {
            var executor = new DbCommandExecutor(SampleDatabase.CreateConnection, true, ConfigureCommand);

            using var connection = SampleDatabase.CreateConnection();

            connection.Open();

            try
            {
                Assert.Throws<SqlException>(() =>
                {
                    // An exception will be thrown and the update will not be rolled back.
                    executor.ExecuteOnConnection(this, connection, (@this, commandActivator) => @this.InsertFailed(commandActivator));
                });

                var count = executor.ExecuteOnConnection(this, connection, (@this, commandActivator) => @this.GetCount(commandActivator));

                Assert.Equal(6, count);
            }
            finally
            {
                executor.ExecuteOnConnection(this, connection, (@this, commandActivator) => @this.Delete(commandActivator));
            }
        }

        #endregion

        #region ExecuteOnConnectionAsync

        /// <summary>
        /// Tests executing a command on an open connection to get records and verifies the record count.
        /// </summary>
        [Fact]
        public async Task ExecuteOnConnectionAsync_EnumerateRecords()
        {
            var executor = new DbCommandExecutor(SampleDatabase.CreateConnection, true, ConfigureCommand);

            using var connection = SampleDatabase.CreateConnection();

            connection.Open();

            var records = executor.ExecuteIteratorOnConnectionAsync(connection, EnumerateRecordsAsync);

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
            var executor = new DbCommandExecutor(SampleDatabase.CreateConnection, true, ConfigureCommand);

            using var connection = SampleDatabase.CreateConnection();

            connection.Open();

            var records = executor.ExecuteIteratorOnConnectionAsync(this, connection, (@this, commandActivator) => @this.EnumerateRecordsAsync(commandActivator));

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
            var executor = new DbCommandExecutor(SampleDatabase.CreateConnection, true, ConfigureCommand);

            using var connection = SampleDatabase.CreateConnection();

            connection.Open();

            var count = await executor.ExecuteOnConnectionAsync(connection, GetCountAsync);

            m_OutputHelper.WriteLine($"Count = {count}");

            Assert.Equal(5, count);
        }

        /// <summary>
        /// Tests executing a command with state on an open connection to get the count of records and verifies the count.
        /// </summary>
        [Fact]
        public async Task ExecuteOnConnectionAsync_State_GetCount()
        {
            var executor = new DbCommandExecutor(SampleDatabase.CreateConnection, true, ConfigureCommand);

            using var connection = SampleDatabase.CreateConnection();

            connection.Open();

            var count = await executor.ExecuteOnConnectionAsync(this, connection, (@this, commandActivator) => @this.GetCountAsync(commandActivator));

            m_OutputHelper.WriteLine($"Count = {count}");

            Assert.Equal(5, count);
        }

        /// <summary>
        /// Tests executing a command on an open connection to insert a record and expects a SqlException.
        /// </summary>
        [Fact]
        public async Task ExecuteOnConnectionAsync_Insert()
        {
            var executor = new DbCommandExecutor(SampleDatabase.CreateConnection, true, ConfigureCommand);

            using var connection = SampleDatabase.CreateConnection();

            connection.Open();

            try
            {
                await Assert.ThrowsAsync<SqlException>(async () =>
                {
                    // An exception will be thrown and the update will not be rolled back.
                    await executor.ExecuteOnConnectionAsync(connection, InsertFailedAsync);
                });

                var count = await executor.ExecuteOnConnectionAsync(connection, GetCountAsync);

                Assert.Equal(6, count);
            }
            finally
            {
                executor.ExecuteOnConnection(connection, Delete);
            }
        }

        /// <summary>
        /// Tests executing a command with state on an open connection to insert a record and expects a SqlException.
        /// </summary>
        [Fact]
        public async Task ExecuteOnConnectionAsync_State_Insert()
        {
            var executor = new DbCommandExecutor(SampleDatabase.CreateConnection, true, ConfigureCommand);

            using var connection = SampleDatabase.CreateConnection();

            connection.Open();

            try
            {
                await Assert.ThrowsAsync<SqlException>(async () =>
                {
                    // An exception will be thrown and the update will not be rolled back.
                    await executor.ExecuteOnConnectionAsync(this, connection, (@this, commandActivator) => @this.InsertFailedAsync(commandActivator));
                });

                var count = await executor.ExecuteOnConnectionAsync(this, connection, (@this, commandActivator) => @this.GetCountAsync(commandActivator));

                Assert.Equal(6, count);
            }
            finally
            {
                executor.ExecuteOnConnection(this, connection, (@this, commandActivator) => @this.Delete(commandActivator));
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
            var executor = new DbCommandExecutor(SampleDatabase.CreateConnection, true, ConfigureCommand);

            using var connection = SampleDatabase.CreateConnection();

            connection.Open();

            using var transaction = connection.BeginTransaction();

            var records = executor.ExecuteOnTransaction(transaction, EnumerateRecords);

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
            var executor = new DbCommandExecutor(SampleDatabase.CreateConnection, true, ConfigureCommand);

            using var connection = SampleDatabase.CreateConnection();

            connection.Open();

            using var transaction = connection.BeginTransaction();

            var records = executor.ExecuteOnTransaction(this, transaction, (@this, commandActivator) => @this.EnumerateRecords(commandActivator));

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
            var executor = new DbCommandExecutor(SampleDatabase.CreateConnection, true, ConfigureCommand);

            using var connection = SampleDatabase.CreateConnection();

            connection.Open();

            using var transaction = connection.BeginTransaction();

            var count = executor.ExecuteOnTransaction(transaction, GetCount);

            m_OutputHelper.WriteLine($"Count = {count}");

            Assert.Equal(5, count);
        }

        /// <summary>
        /// Tests executing a command with state within a transaction to get the count of records and verifies the count.
        /// </summary>
        [Fact]
        public void ExecuteOnTransaction_State_GetCount()
        {
            var executor = new DbCommandExecutor(SampleDatabase.CreateConnection, true, ConfigureCommand);

            using var connection = SampleDatabase.CreateConnection();

            connection.Open();

            using var transaction = connection.BeginTransaction();

            var count = executor.ExecuteOnTransaction(this, transaction, (@this, commandActivator) => @this.GetCount(commandActivator));

            m_OutputHelper.WriteLine($"Count = {count}");

            Assert.Equal(5, count);
        }

        /// <summary>
        /// Tests executing a command within a transaction to insert a record and expects a SqlException.
        /// </summary>
        [Fact]
        public void ExecuteOnTransaction_Insert()
        {
            var executor = new DbCommandExecutor(SampleDatabase.CreateConnection, true, ConfigureCommand);

            using var connection = SampleDatabase.CreateConnection();

            connection.Open();

            using var transaction = connection.BeginTransaction();

            try
            {
                Assert.Throws<SqlException>(() =>
                {
                    // An exception will be thrown.
                    executor.ExecuteOnTransaction(transaction, InsertFailed);
                });
            }
            finally
            {
                transaction.Rollback();
            }

            var count = executor.ExecuteOnNewConnection(GetCount);

            Assert.Equal(5, count);
        }

        /// <summary>
        /// Tests executing a command with state within a transaction to insert a record and expects a SqlException.
        /// </summary>
        [Fact]
        public void ExecuteOnTransaction_State_Insert()
        {
            var executor = new DbCommandExecutor(SampleDatabase.CreateConnection, true, ConfigureCommand);

            using var connection = SampleDatabase.CreateConnection();

            connection.Open();

            using var transaction = connection.BeginTransaction();

            try
            {
                Assert.Throws<SqlException>(() =>
                {
                    // An exception will be thrown.
                    executor.ExecuteOnTransaction(this, transaction, (@this, commandActivator) => @this.InsertFailed(commandActivator));
                });
            }
            finally
            {
                transaction.Rollback();
            }

            var count = executor.ExecuteOnNewConnection(this, (@this, commandActivator) => @this.GetCount(commandActivator));

            Assert.Equal(5, count);
        }

        #endregion

        #region ExecuteOnTransactionAsync

        /// <summary>
        /// Tests executing a command within a transaction to get records and verifies the record count.
        /// </summary>
        [Fact]
        public async Task ExecuteOnTransactionAsync_EnumerateRecords()
        {
            var executor = new DbCommandExecutor(SampleDatabase.CreateConnection, true, ConfigureCommand);

            using var connection = SampleDatabase.CreateConnection();

            connection.Open();

            using var transaction = connection.BeginTransaction();

            var records = executor.ExecuteIteratorOnTransactionAsync(transaction, EnumerateRecordsAsync);

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
            var executor = new DbCommandExecutor(SampleDatabase.CreateConnection, true, ConfigureCommand);

            using var connection = SampleDatabase.CreateConnection();

            connection.Open();

            using var transaction = connection.BeginTransaction();

            var records = executor.ExecuteIteratorOnTransactionAsync(this, transaction, (@this, commandActivator) => @this.EnumerateRecordsAsync(commandActivator));

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
            var executor = new DbCommandExecutor(SampleDatabase.CreateConnection, true, ConfigureCommand);

            using var connection = SampleDatabase.CreateConnection();

            connection.Open();

            using var transaction = connection.BeginTransaction();

            var count = await executor.ExecuteOnTransactionAsync(transaction, GetCountAsync);

            m_OutputHelper.WriteLine($"Count = {count}");

            Assert.Equal(5, count);
        }

        /// <summary>
        /// Tests executing a command with state within a transaction to get the count of records and verifies the count.
        /// </summary>
        [Fact]
        public async Task ExecuteOnTransactionAsync_State_GetCount()
        {
            var executor = new DbCommandExecutor(SampleDatabase.CreateConnection, true, ConfigureCommand);

            using var connection = SampleDatabase.CreateConnection();

            connection.Open();

            using var transaction = connection.BeginTransaction();

            var count = await executor.ExecuteOnTransactionAsync(this, transaction, (@this, commandActivator) => @this.GetCountAsync(commandActivator));

            m_OutputHelper.WriteLine($"Count = {count}");

            Assert.Equal(5, count);
        }

        /// <summary>
        /// Tests executing a command within a transaction to insert a record and expects a SqlException.
        /// </summary>
        [Fact]
        public async Task ExecuteOnTransactionAsync_Insert()
        {
            var executor = new DbCommandExecutor(SampleDatabase.CreateConnection, true, ConfigureCommand);

            using var connection = SampleDatabase.CreateConnection();

            connection.Open();

            using var transaction = connection.BeginTransaction();

            try
            {
                await Assert.ThrowsAsync<SqlException>(async () =>
                {
                    // An exception will be thrown.
                    await executor.ExecuteOnTransactionAsync(transaction, InsertFailedAsync);
                });
            }
            finally
            {
                transaction.Rollback();
            }

            var count = executor.ExecuteOnNewConnection(GetCount);

            Assert.Equal(5, count);
        }

        /// <summary>
        /// Tests executing a command with state within a transaction to insert a record and expects a SqlException.
        /// </summary>
        [Fact]
        public async Task ExecuteOnTransactionAsync_State_Insert()
        {
            var executor = new DbCommandExecutor(SampleDatabase.CreateConnection, true, ConfigureCommand);

            using var connection = SampleDatabase.CreateConnection();

            connection.Open();

            using var transaction = connection.BeginTransaction();

            try
            {
                await Assert.ThrowsAsync<SqlException>(async () =>
                {
                    // An exception will be thrown.
                    await executor.ExecuteOnTransactionAsync(this, transaction, (@this, commandActivator) => @this.InsertFailedAsync(commandActivator));
                });
            }
            finally
            {
                transaction.Rollback();
            }

            var count = executor.ExecuteOnNewConnection(this, (@this, commandActivator) => @this.GetCount(commandActivator));

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
        private IDataReader GetRecords(Func<IDbCommand> commandActivator)
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
        private ValueTask<IDataReader> GetRecordsAsync(Func<IDbCommand> commandActivator)
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
        private IEnumerable<IDataRecord> EnumerateRecords(Func<IDbCommand> commandActivator)
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
        private async IAsyncEnumerable<IDataRecord> EnumerateRecordsAsync(Func<IDbCommand> commandActivator)
        {
            using var command = commandActivator();

            Assert.Equal(60, command.CommandTimeout);

            command.CommandText = "select * from SAMPLE_TABLE";

            using (var reader = await command.ExecuteReaderAsync().ConfigureAwait(false))
            {
                while (await reader.ReadAsync().ConfigureAwait(false))
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
        private int GetCount(Func<IDbCommand> commandActivator)
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
        private async ValueTask<int> GetCountAsync(Func<IDbCommand> commandActivator)
        {
            using var command = commandActivator();

            Assert.Equal(60, command.CommandTimeout);

            command.CommandText = "select count(*) from SAMPLE_TABLE";

            return (int)await command.ExecuteScalarAsync()!;
        }

        /// <summary>
        /// Inserts a record into the database.
        /// </summary>
        /// <param name="commandActivator">The command activator.</param>
        private void Insert(Func<IDbCommand> commandActivator)
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
        private async ValueTask InsertAsync(Func<IDbCommand> commandActivator)
        {
            using var command = commandActivator();

            Assert.Equal(60, command.CommandTimeout);

            command.CommandText = "insert into SAMPLE_TABLE values (6, 'Name6')";

            await command.ExecuteNonQueryAsync().ConfigureAwait(false);
        }

        /// <summary>
        /// Attempts to insert a record into the database and throws an exception.
        /// </summary>
        /// <param name="commandActivator">The command activator.</param>
        private void InsertFailed(Func<IDbCommand> commandActivator)
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
        private async ValueTask InsertFailedAsync(Func<IDbCommand> commandActivator)
        {
            using var command = commandActivator();

            Assert.Equal(60, command.CommandTimeout);

            command.CommandText = "insert into SAMPLE_TABLE values (6, 'Name6')";

            await command.ExecuteNonQueryAsync().ConfigureAwait(false);

            // An exception will be thrown.
            await command.ExecuteNonQueryAsync().ConfigureAwait(false);
        }

        /// <summary>
        /// Deletes a record from the database.
        /// </summary>
        /// <param name="commandActivator">The command activator.</param>
        private void Delete(Func<IDbCommand> commandActivator)
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
        private async ValueTask DeleteAsync(Func<IDbCommand> commandActivator)
        {
            using var command = commandActivator();

            Assert.Equal(60, command.CommandTimeout);

            command.CommandText = "delete from SAMPLE_TABLE where ID = 6";

            await command.ExecuteNonQueryAsync().ConfigureAwait(false);
        }
    }
}
