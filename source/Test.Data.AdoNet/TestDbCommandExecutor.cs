using System;
using System.Data;
using Xunit;
using mxProject.Data;
using Xunit.Abstractions;
using System.Transactions;
using Microsoft.Data.SqlClient;

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
                var records = executor.ExecuteIteratorOnNewConnection(GetRecords);

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
                var records = executor.ExecuteIteratorOnNewConnection(this, (@this, commandActivator) => @this.GetRecords(commandActivator));

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

        #endregion

        #region ExecuteIteratorOnNewConnection

        /// <summary>
        /// Tests executing a command to get records and verifies the record count.
        /// </summary>
        [Fact]
        public void ExecuteIteratorOnNewConnection_GetRecords()
        {
            var executor = new DbCommandExecutor(SampleDatabase.CreateConnection, true, ConfigureCommand);

            var records = executor.ExecuteIteratorOnNewConnection(GetRecords);

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
        public void ExecuteIteratorOnNewConnection_State_GetRecords()
        {
            var executor = new DbCommandExecutor(SampleDatabase.CreateConnection, true, ConfigureCommand);

            var records = executor.ExecuteIteratorOnNewConnection(this, (@this, commandActivator) => @this.GetRecords(commandActivator));

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
        public void ExecuteOnNewConnection_GetRecords()
        {
            var executor = new DbCommandExecutor(SampleDatabase.CreateConnection, true, ConfigureCommand);

            Assert.Throws<InvalidOperationException>(() =>
            {
                var records = executor.ExecuteOnNewConnection(GetRecords);

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
        public void ExecuteOnNewConnection_State_GetRecords()
        {
            var executor = new DbCommandExecutor(SampleDatabase.CreateConnection, true, ConfigureCommand);

            Assert.Throws<InvalidOperationException>(() =>
            {
                var records = executor.ExecuteOnNewConnection(this, (@this, commandActivator) => @this.GetRecords(commandActivator));

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

        #region ExecuteOnConnection

        /// <summary>
        /// Tests executing a command on an open connection to get records and verifies the record count.
        /// </summary>
        [Fact]
        public void ExecuteOnConnection_GetRecords()
        {
            var executor = new DbCommandExecutor(SampleDatabase.CreateConnection, true, ConfigureCommand);

            using var connection = SampleDatabase.CreateConnection();

            connection.Open();

            var records = executor.ExecuteOnConnection(connection, GetRecords);

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
        public void ExecuteOnConnection_State_GetRecords()
        {
            var executor = new DbCommandExecutor(SampleDatabase.CreateConnection, true, ConfigureCommand);

            using var connection = SampleDatabase.CreateConnection();

            connection.Open();

            var records = executor.ExecuteOnConnection(this, connection, (@this, commandActivator) => @this.GetRecords(commandActivator));

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

        #region ExecuteOnTransaction

        /// <summary>
        /// Tests executing a command within a transaction to get records and verifies the record count.
        /// </summary>
        [Fact]
        public void ExecuteOnTransaction_GetRecords()
        {
            var executor = new DbCommandExecutor(SampleDatabase.CreateConnection, true, ConfigureCommand);

            using var connection = SampleDatabase.CreateConnection();

            connection.Open();

            using var transaction = connection.BeginTransaction();

            var records = executor.ExecuteOnTransaction(transaction, GetRecords);

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
        public void ExecuteOnTransaction_State_GetRecords()
        {
            var executor = new DbCommandExecutor(SampleDatabase.CreateConnection, true, ConfigureCommand);

            using var connection = SampleDatabase.CreateConnection();

            connection.Open();

            using var transaction = connection.BeginTransaction();

            var records = executor.ExecuteOnTransaction(this, transaction, (@this, commandActivator) => @this.GetRecords(commandActivator));

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
        private IEnumerable<IDataRecord> GetRecords(Func<IDbCommand> commandActivator)
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
    }
}
