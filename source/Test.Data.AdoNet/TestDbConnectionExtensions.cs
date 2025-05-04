using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Data.SqlClient;
using mxProject.Data;
using mxProject.Data.Extensions;

namespace Test.Data.AdoNet
{
    public class TestDbConnectionExtensions
    {
        #region async

        [Fact]
        public async Task AsyncOperations()
        {
            using var connection = SampleDatabase.CreateConnection();

            // Open
            await connection.OpenAsync();

            using (var command = connection.CreateCommand())
            {
                command.CommandText = "select 1 as id, 'a' as name";

                // ExecureReader
                using var reader = await command.ExecuteReaderAsync();

                object[] values = new object[2];

                // Read
                while (await reader.ReadAsync())
                {
                    // GetValue
                    var id = await reader.GetValueAsync(0);

                    Assert.Equal(1, id);

                    // GetValues
                    var count = await reader.GetValuesAsync(values);

                    Assert.Equal(2, count);
                    Assert.Equal(1, values[0]);
                    Assert.Equal("a", values[1]);

                    // IsDBNull
                    var isnull = await reader.IsDBNullAsync(0);

                    Assert.False(isnull);
                }

                // Close
                await reader.CloseAsync();

                // Dispose
                await reader.DisposeAsync();
            }

            int maxID;

            using (var command = connection.CreateCommand())
            {
                command.CommandText = "select max(ID) from SAMPLE_TABLE";

                // ExecureScalar
                maxID = (int)(await command.ExecuteScalarAsync());
            }

            using var transaction = await connection.BeginTransactionAsync();  

            using (var command = connection.CreateCommand())
            {
                command.CommandText = "insert into SAMPLE_TABLE values (@ID, @NAME)";
                command.AddInt32Parameter("@ID", maxID + 1);
                command.AddStringParameter("@NAME", $"Name{maxID + 1}");
                command.Transaction = transaction;

                // ExecureNonQuery
                var affectedCount = await command.ExecuteNonQueryAsync();

                Assert.Equal(1, affectedCount);
            }

            // Rollback
            await transaction.RollbackAsync();

            // Dispose
            await transaction.DisposeAsync();

            // Close
            await connection.CloseAsync();

            // Dispose
            await connection.DisposeAsync();
        }

        #endregion

        #region Cast

        [Fact]
        public void Cast()
        {
            using var connection = SampleDatabase.CreateConnection();

            connection.Cast<SqlConnection>(x => x.Credential = null);

            var wrapper = connection.WithFilter();

            wrapper.Cast<SqlConnection>(x => x.Credential = null);
        }

        #endregion
    }
}
