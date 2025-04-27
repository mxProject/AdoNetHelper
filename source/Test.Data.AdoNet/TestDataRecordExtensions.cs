using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using Microsoft.VisualStudio.CodeCoverage;
using mxProject.Data.Extensions;

namespace Test.Data.AdoNet
{
    /// <summary>
    /// Provides test methods for data record extensions.
    /// </summary>
    /// <remarks>
    /// Using SqlServer.
    /// </remarks>
    public class TestDataRecordExtensions
    {
        /// <summary>
        /// Tests the GetBoolean extension method for various scenarios.
        /// </summary>
        [Fact]
        public async Task GetBoolean()
        {
            using var connection = SampleDatabase.CreateConnection();

            connection.Open();

            using var transaction = connection.BeginTransaction();

            // create temporary table
            var tableName = $"#{nameof(TestDataRecordExtensions)}_{nameof(GetBoolean)}";

            using (var command = connection.CreateCommand())
            {
                command.Transaction = transaction;
                command.CommandText = $"create table {tableName} ( field1 bit, field2 bit )";
                command.ExecuteNonQuery();
            }

            bool value = true;

            // insert data
            using (var command = connection.CreateCommand())
            {
                command.Transaction = transaction;
                command.CommandText = $"insert into {tableName} values ( {(value ? 1 : 0)}, null )";
                command.ExecuteNonQuery();
            }

            // get field value
            using (var command = connection.CreateCommand())
            {
                command.Transaction = transaction;
                command.CommandText = $"select * from {tableName}";

                using var reader = command.ExecuteReader();

                while (reader.Read())
                {
                    Assert.Equal(value, reader.GetBoolean(0));
                    Assert.Equal(value, await reader.GetBooleanAsync(0));

                    Assert.Equal(value, reader.GetBoolean("field1"));
                    Assert.Equal(value, await reader.GetBooleanAsync("field1"));

                    Assert.Equal(value, reader.GetBooleanOrNull(0)!.Value);
                    Assert.Equal(value, (await reader.GetBooleanOrNullAsync(0)).Value);

                    Assert.Equal(value, reader.GetBooleanOrNull("field1")!.Value);
                    Assert.Equal(value, (await reader.GetBooleanOrNullAsync("field1")).Value);

                    Assert.Equal(value, reader.GetBooleanOrDefault(0));
                    Assert.Equal(value, await reader.GetBooleanOrDefaultAsync(0));

                    Assert.Equal(value, reader.GetBooleanOrDefault("field1"));
                    Assert.Equal(value, await reader.GetBooleanOrDefaultAsync("field1"));

                    Assert.True(reader.IsDBNull(1));
                    Assert.True(reader.IsDBNull("field2"));

                    Assert.True(await reader.IsDBNullAsync(1));
                    Assert.True(await reader.IsDBNullAsync("field2"));

                    Assert.False(reader.GetBooleanOrNull(1).HasValue);
                    Assert.False((await reader.GetBooleanOrNullAsync(1)).HasValue);

                    Assert.False(reader.GetBooleanOrNull("field2").HasValue);
                    Assert.False((await reader.GetBooleanOrNullAsync("field2")).HasValue);

                    Assert.False(reader.GetBooleanOrDefault("field2"));
                    Assert.False(await reader.GetBooleanOrDefaultAsync("field2"));
                }
            }

            transaction.Rollback();
            connection.Close();
        }

        /// <summary>
        /// Tests the GetByte extension method for various scenarios.
        /// </summary>
        [Fact]
        public async Task GetByte()
        {
            using var connection = SampleDatabase.CreateConnection();

            connection.Open();

            using var transaction = connection.BeginTransaction();

            // create temporary table
            var tableName = $"#{nameof(TestDataRecordExtensions)}_{nameof(GetByte)}";

            using (var command = connection.CreateCommand())
            {
                command.Transaction = transaction;
                command.CommandText = $"create table {tableName} ( field1 tinyint, field2 tinyint )";
                command.ExecuteNonQuery();
            }

            byte value = 1;

            // insert data
            using (var command = connection.CreateCommand())
            {
                command.Transaction = transaction;
                command.CommandText = $"insert into {tableName} values ( {value}, null )";
                command.ExecuteNonQuery();
            }

            // get field value
            using (var command = connection.CreateCommand())
            {
                command.Transaction = transaction;
                command.CommandText = $"select * from {tableName}";

                using var reader = command.ExecuteReader();

                while (reader.Read())
                {
                    Assert.Equal(value, reader.GetByte(0));
                    Assert.Equal(value, await reader.GetByteAsync(0));

                    Assert.Equal(value, reader.GetByte("field1"));
                    Assert.Equal(value, await reader.GetByteAsync("field1"));

                    Assert.Equal(value, reader.GetByteOrNull(0)!.Value);
                    Assert.Equal(value, (await reader.GetByteOrNullAsync(0)).Value);

                    Assert.Equal(value, reader.GetByteOrNull("field1")!.Value);
                    Assert.Equal(value, (await reader.GetByteOrNullAsync("field1")).Value);

                    Assert.Equal(value, reader.GetByteOrDefault(0));
                    Assert.Equal(value, await reader.GetByteOrDefaultAsync(0));

                    Assert.Equal(value, reader.GetByteOrDefault("field1"));
                    Assert.Equal(value, await reader.GetByteOrDefaultAsync("field1"));

                    Assert.True(reader.IsDBNull(1));
                    Assert.True(reader.IsDBNull("field2"));

                    Assert.True(await reader.IsDBNullAsync(1));
                    Assert.True(await reader.IsDBNullAsync("field2"));

                    Assert.False(reader.GetByteOrNull(1).HasValue);
                    Assert.False((await reader.GetByteOrNullAsync(1)).HasValue);

                    Assert.False(reader.GetByteOrNull("field2").HasValue);
                    Assert.False((await reader.GetByteOrNullAsync("field2")).HasValue);

                    Assert.Equal(default(byte), reader.GetByteOrDefault("field2"));
                    Assert.Equal(default(byte), await reader.GetByteOrDefaultAsync("field2"));
                }
            }

            transaction.Rollback();
            connection.Close();
        }

        /// <summary>
        /// Tests the GetInt16 extension method for various scenarios.
        /// </summary>
        [Fact]
        public async Task GetInt16()
        {
            using var connection = SampleDatabase.CreateConnection();

            connection.Open();

            using var transaction = connection.BeginTransaction();

            // create temporary table
            var tableName = $"#{nameof(TestDataRecordExtensions)}_{nameof(GetInt16)}";

            using (var command = connection.CreateCommand())
            {
                command.Transaction = transaction;
                command.CommandText = $"create table {tableName} ( field1 smallint, field2 smallint )";
                command.ExecuteNonQuery();
            }

            short value = 1;

            // insert data
            using (var command = connection.CreateCommand())
            {
                command.Transaction = transaction;
                command.CommandText = $"insert into {tableName} values ( {value}, null )";
                command.ExecuteNonQuery();
            }

            // get field value
            using (var command = connection.CreateCommand())
            {
                command.Transaction = transaction;
                command.CommandText = $"select * from {tableName}";

                using var reader = command.ExecuteReader();

                while (reader.Read())
                {
                    Assert.Equal(value, reader.GetInt16(0));
                    Assert.Equal(value, await reader.GetInt16Async(0));

                    Assert.Equal(value, reader.GetInt16("field1"));
                    Assert.Equal(value, await reader.GetInt16Async("field1"));

                    Assert.Equal(value, reader.GetInt16OrNull(0)!.Value);
                    Assert.Equal(value, (await reader.GetInt16OrNullAsync(0)).Value);

                    Assert.Equal(value, reader.GetInt16OrNull("field1")!.Value);
                    Assert.Equal(value, (await reader.GetInt16OrNullAsync("field1")).Value);

                    Assert.Equal(value, reader.GetInt16OrDefault(0));
                    Assert.Equal(value, await reader.GetInt16OrDefaultAsync(0));

                    Assert.Equal(value, reader.GetInt16OrDefault("field1"));
                    Assert.Equal(value, await reader.GetInt16OrDefaultAsync("field1"));

                    Assert.True(reader.IsDBNull(1));
                    Assert.True(reader.IsDBNull("field2"));

                    Assert.True(await reader.IsDBNullAsync(1));
                    Assert.True(await reader.IsDBNullAsync("field2"));

                    Assert.False(reader.GetInt16OrNull(1).HasValue);
                    Assert.False((await reader.GetInt16OrNullAsync(1)).HasValue);

                    Assert.False(reader.GetInt16OrNull("field2").HasValue);
                    Assert.False((await reader.GetInt16OrNullAsync("field2")).HasValue);

                    Assert.Equal(default(short), reader.GetInt16OrDefault("field2"));
                    Assert.Equal(default(short), await reader.GetInt16OrDefaultAsync("field2"));
                }
            }

            transaction.Rollback();
            connection.Close();
        }

        /// <summary>
        /// Tests the GetInt32 extension method for various scenarios.
        /// </summary>
        [Fact]
        public async Task GetInt32()
        {
            using var connection = SampleDatabase.CreateConnection();

            connection.Open();

            using var transaction = connection.BeginTransaction();

            // create temporary table
            var tableName = $"#{nameof(TestDataRecordExtensions)}_{nameof(GetInt32)}";

            using (var command = connection.CreateCommand())
            {
                command.Transaction = transaction;
                command.CommandText = $"create table {tableName} ( field1 int, field2 int )";
                command.ExecuteNonQuery();
            }

            int value = 1;

            // insert data
            using (var command = connection.CreateCommand())
            {
                command.Transaction = transaction;
                command.CommandText = $"insert into {tableName} values ( {value}, null )";
                command.ExecuteNonQuery();
            }

            // get field value
            using (var command = connection.CreateCommand())
            {
                command.Transaction = transaction;
                command.CommandText = $"select * from {tableName}";

                using var reader = command.ExecuteReader();

                while (reader.Read())
                {
                    Assert.Equal(value, reader.GetInt32(0));
                    Assert.Equal(value, await reader.GetInt32Async(0));

                    Assert.Equal(value, reader.GetInt32("field1"));
                    Assert.Equal(value, await reader.GetInt32Async("field1"));

                    Assert.Equal(value, reader.GetInt32OrNull(0)!.Value);
                    Assert.Equal(value, (await reader.GetInt32OrNullAsync(0)).Value);

                    Assert.Equal(value, reader.GetInt32OrNull("field1")!.Value);
                    Assert.Equal(value, (await reader.GetInt32OrNullAsync("field1")).Value);

                    Assert.Equal(value, reader.GetInt32OrDefault(0));
                    Assert.Equal(value, await reader.GetInt32OrDefaultAsync(0));

                    Assert.Equal(value, reader.GetInt32OrDefault("field1"));
                    Assert.Equal(value, await reader.GetInt32OrDefaultAsync("field1"));

                    Assert.True(reader.IsDBNull(1));
                    Assert.True(reader.IsDBNull("field2"));

                    Assert.True(await reader.IsDBNullAsync(1));
                    Assert.True(await reader.IsDBNullAsync("field2"));

                    Assert.False(reader.GetInt32OrNull(1).HasValue);
                    Assert.False((await reader.GetInt32OrNullAsync(1)).HasValue);

                    Assert.False(reader.GetInt32OrNull("field2").HasValue);
                    Assert.False((await reader.GetInt32OrNullAsync("field2")).HasValue);

                    Assert.Equal(default(int), reader.GetInt32OrDefault("field2"));
                    Assert.Equal(default(int), await reader.GetInt32OrDefaultAsync("field2"));
                }
            }

            transaction.Rollback();
            connection.Close();
        }

        /// <summary>
        /// Tests the GetInt64 extension method for various scenarios.
        /// </summary>
        [Fact]
        public async Task GetInt64()
        {
            using var connection = SampleDatabase.CreateConnection();

            connection.Open();

            using var transaction = connection.BeginTransaction();

            // create temporary table
            var tableName = $"#{nameof(TestDataRecordExtensions)}_{nameof(GetInt64)}";

            using (var command = connection.CreateCommand())
            {
                command.Transaction = transaction;
                command.CommandText = $"create table {tableName} ( field1 bigint, field2 bigint )";
                command.ExecuteNonQuery();
            }

            long value = 1;

            // insert data
            using (var command = connection.CreateCommand())
            {
                command.Transaction = transaction;
                command.CommandText = $"insert into {tableName} values ( {value}, null )";
                command.ExecuteNonQuery();
            }

            // get field value
            using (var command = connection.CreateCommand())
            {
                command.Transaction = transaction;
                command.CommandText = $"select * from {tableName}";

                using var reader = command.ExecuteReader();

                while (reader.Read())
                {
                    Assert.Equal(value, reader.GetInt64(0));
                    Assert.Equal(value, await reader.GetInt64Async(0));

                    Assert.Equal(value, reader.GetInt64("field1"));
                    Assert.Equal(value, await reader.GetInt64Async("field1"));

                    Assert.Equal(value, reader.GetInt64OrNull(0)!.Value);
                    Assert.Equal(value, (await reader.GetInt64OrNullAsync(0)).Value);

                    Assert.Equal(value, reader.GetInt64OrNull("field1")!.Value);
                    Assert.Equal(value, (await reader.GetInt64OrNullAsync("field1")).Value);

                    Assert.Equal(value, reader.GetInt64OrDefault(0));
                    Assert.Equal(value, await reader.GetInt64OrDefaultAsync(0));

                    Assert.Equal(value, reader.GetInt64OrDefault("field1"));
                    Assert.Equal(value, await reader.GetInt64OrDefaultAsync("field1"));

                    Assert.True(reader.IsDBNull(1));
                    Assert.True(reader.IsDBNull("field2"));

                    Assert.True(await reader.IsDBNullAsync(1));
                    Assert.True(await reader.IsDBNullAsync("field2"));

                    Assert.False(reader.GetInt64OrNull(1).HasValue);
                    Assert.False((await reader.GetInt64OrNullAsync(1)).HasValue);

                    Assert.False(reader.GetInt64OrNull("field2").HasValue);
                    Assert.False((await reader.GetInt64OrNullAsync("field2")).HasValue);

                    Assert.Equal(default(long), reader.GetInt64OrDefault("field2"));
                    Assert.Equal(default(long), await reader.GetInt64OrDefaultAsync("field2"));
                }
            }

            transaction.Rollback();
            connection.Close();
        }

        /// <summary>
        /// Tests the GetFloat extension method for various scenarios.
        /// </summary>
        [Fact]
        public async Task GetFloat()
        {
            using var connection = SampleDatabase.CreateConnection();

            connection.Open();

            using var transaction = connection.BeginTransaction();

            // create temporary table
            var tableName = $"#{nameof(TestDataRecordExtensions)}_{nameof(GetFloat)}";

            using (var command = connection.CreateCommand())
            {
                command.Transaction = transaction;
                command.CommandText = $"create table {tableName} ( field1 real, field2 real )";
                command.ExecuteNonQuery();
            }

            float value = 1.1f;

            // insert data
            using (var command = connection.CreateCommand())
            {
                command.Transaction = transaction;
                command.CommandText = $"insert into {tableName} values ( {value}, null )";
                command.ExecuteNonQuery();
            }

            // get field value
            using (var command = connection.CreateCommand())
            {
                command.Transaction = transaction;
                command.CommandText = $"select * from {tableName}";

                using var reader = command.ExecuteReader();

                while (reader.Read())
                {
                    Assert.Equal(value, reader.GetFloat(0));
                    Assert.Equal(value, await reader.GetFloatAsync(0));

                    Assert.Equal(value, reader.GetFloat("field1"));
                    Assert.Equal(value, await reader.GetFloatAsync("field1"));

                    Assert.Equal(value, reader.GetFloatOrNull(0)!.Value);
                    Assert.Equal(value, (await reader.GetFloatOrNullAsync(0)).Value);

                    Assert.Equal(value, reader.GetFloatOrNull("field1")!.Value);
                    Assert.Equal(value, (await reader.GetFloatOrNullAsync("field1")).Value);

                    Assert.Equal(value, reader.GetFloatOrDefault(0));
                    Assert.Equal(value, await reader.GetFloatOrDefaultAsync(0));

                    Assert.Equal(value, reader.GetFloatOrDefault("field1"));
                    Assert.Equal(value, await reader.GetFloatOrDefaultAsync("field1"));

                    Assert.True(reader.IsDBNull(1));
                    Assert.True(reader.IsDBNull("field2"));

                    Assert.True(await reader.IsDBNullAsync(1));
                    Assert.True(await reader.IsDBNullAsync("field2"));

                    Assert.False(reader.GetFloatOrNull(1).HasValue);
                    Assert.False((await reader.GetFloatOrNullAsync(1)).HasValue);

                    Assert.False(reader.GetFloatOrNull("field2").HasValue);
                    Assert.False((await reader.GetFloatOrNullAsync("field2")).HasValue);

                    Assert.Equal(default(float), reader.GetFloatOrDefault("field2"));
                    Assert.Equal(default(float), await reader.GetFloatOrDefaultAsync("field2"));
                }
            }

            transaction.Rollback();
            connection.Close();
        }

        /// <summary>
        /// Tests the GetDouble extension method for various scenarios.
        /// </summary>
        [Fact]
        public async Task GetDouble()
        {
            using var connection = SampleDatabase.CreateConnection();

            connection.Open();

            using var transaction = connection.BeginTransaction();

            // create temporary table
            var tableName = $"#{nameof(TestDataRecordExtensions)}_{nameof(GetDouble)}";

            using (var command = connection.CreateCommand())
            {
                command.Transaction = transaction;
                command.CommandText = $"create table {tableName} ( field1 float, field2 float )";
                command.ExecuteNonQuery();
            }

            double value = 1.1;

            // insert data
            using (var command = connection.CreateCommand())
            {
                command.Transaction = transaction;
                command.CommandText = $"insert into {tableName} values ( {value}, null )";
                command.ExecuteNonQuery();
            }

            // get field value
            using (var command = connection.CreateCommand())
            {
                command.Transaction = transaction;
                command.CommandText = $"select * from {tableName}";

                using var reader = command.ExecuteReader();

                while (reader.Read())
                {
                    Assert.Equal(value, reader.GetDouble(0));
                    Assert.Equal(value, await reader.GetDoubleAsync(0));

                    Assert.Equal(value, reader.GetDouble("field1"));
                    Assert.Equal(value, await reader.GetDoubleAsync("field1"));

                    Assert.Equal(value, reader.GetDoubleOrNull(0)!.Value);
                    Assert.Equal(value, (await reader.GetDoubleOrNullAsync(0)).Value);

                    Assert.Equal(value, reader.GetDoubleOrNull("field1")!.Value);
                    Assert.Equal(value, (await reader.GetDoubleOrNullAsync("field1")).Value);

                    Assert.Equal(value, reader.GetDoubleOrDefault(0));
                    Assert.Equal(value, await reader.GetDoubleOrDefaultAsync(0));

                    Assert.Equal(value, reader.GetDoubleOrDefault("field1"));
                    Assert.Equal(value, await reader.GetDoubleOrDefaultAsync("field1"));

                    Assert.True(reader.IsDBNull(1));
                    Assert.True(reader.IsDBNull("field2"));

                    Assert.True(await reader.IsDBNullAsync(1));
                    Assert.True(await reader.IsDBNullAsync("field2"));

                    Assert.False(reader.GetDoubleOrNull(1).HasValue);
                    Assert.False((await reader.GetDoubleOrNullAsync(1)).HasValue);

                    Assert.False(reader.GetDoubleOrNull("field2").HasValue);
                    Assert.False((await reader.GetDoubleOrNullAsync("field2")).HasValue);

                    Assert.Equal(default(double), reader.GetDoubleOrDefault("field2"));
                    Assert.Equal(default(double), await reader.GetDoubleOrDefaultAsync("field2"));
                }
            }

            transaction.Rollback();
            connection.Close();
        }

        /// <summary>
        /// Tests the GetDecimal extension method for various scenarios.
        /// </summary>
        [Fact]
        public async Task GetDecimal()
        {
            using var connection = SampleDatabase.CreateConnection();

            connection.Open();

            using var transaction = connection.BeginTransaction();

            // create temporary table
            var tableName = $"#{nameof(TestDataRecordExtensions)}_{nameof(GetDecimal)}";

            using (var command = connection.CreateCommand())
            {
                command.Transaction = transaction;
                command.CommandText = $"create table {tableName} ( field1 decimal(5, 1), field2 decimal(5, 1) )";
                command.ExecuteNonQuery();
            }

            decimal value = 1.1m;

            // insert data
            using (var command = connection.CreateCommand())
            {
                command.Transaction = transaction;
                command.CommandText = $"insert into {tableName} values ( {value}, null )";
                command.ExecuteNonQuery();
            }

            // get field value
            using (var command = connection.CreateCommand())
            {
                command.Transaction = transaction;
                command.CommandText = $"select * from {tableName}";

                using var reader = command.ExecuteReader();

                while (reader.Read())
                {
                    Assert.Equal(value, reader.GetDecimal(0));
                    Assert.Equal(value, await reader.GetDecimalAsync(0));

                    Assert.Equal(value, reader.GetDecimal("field1"));
                    Assert.Equal(value, await reader.GetDecimalAsync("field1"));

                    Assert.Equal(value, reader.GetDecimalOrNull(0)!.Value);
                    Assert.Equal(value, (await reader.GetDecimalOrNullAsync(0)).Value);

                    Assert.Equal(value, reader.GetDecimalOrNull("field1")!.Value);
                    Assert.Equal(value, (await reader.GetDecimalOrNullAsync("field1")).Value);

                    Assert.Equal(value, reader.GetDecimalOrDefault(0));
                    Assert.Equal(value, await reader.GetDecimalOrDefaultAsync(0));

                    Assert.Equal(value, reader.GetDecimalOrDefault("field1"));
                    Assert.Equal(value, await reader.GetDecimalOrDefaultAsync("field1"));

                    Assert.True(reader.IsDBNull(1));
                    Assert.True(reader.IsDBNull("field2"));

                    Assert.True(await reader.IsDBNullAsync(1));
                    Assert.True(await reader.IsDBNullAsync("field2"));

                    Assert.False(reader.GetDecimalOrNull(1).HasValue);
                    Assert.False((await reader.GetDecimalOrNullAsync(1)).HasValue);

                    Assert.False(reader.GetDecimalOrNull("field2").HasValue);
                    Assert.False((await reader.GetDecimalOrNullAsync("field2")).HasValue);

                    Assert.Equal(default(decimal), reader.GetDecimalOrDefault("field2"));
                    Assert.Equal(default(decimal), await reader.GetDecimalOrDefaultAsync("field2"));
                }
            }

            transaction.Rollback();
            connection.Close();
        }

        /// <summary>
        /// Tests the GetDateTime extension method for various scenarios.
        /// </summary>
        [Fact]
        public async Task GetDateTime()
        {
            using var connection = SampleDatabase.CreateConnection();

            connection.Open();

            using var transaction = connection.BeginTransaction();

            // create temporary table
            var tableName = $"#{nameof(TestDataRecordExtensions)}_{nameof(GetDateTime)}";

            using (var command = connection.CreateCommand())
            {
                command.Transaction = transaction;
                command.CommandText = $"create table {tableName} ( field1 datetime, field2 datetime )";
                command.ExecuteNonQuery();
            }

            var value = new DateTime(2025, 1, 2, 13, 14, 15);

            // insert data
            using (var command = connection.CreateCommand())
            {
                command.Transaction = transaction;
                command.CommandText = $"insert into {tableName} values ( convert(datetime, '{value:yyyy/MM/dd HH:mm:ss}'), null )";
                command.ExecuteNonQuery();
            }

            // get field value
            using (var command = connection.CreateCommand())
            {
                command.Transaction = transaction;
                command.CommandText = $"select * from {tableName}";

                using var reader = command.ExecuteReader();

                while (reader.Read())
                {
                    Assert.Equal(value, reader.GetDateTime(0));
                    Assert.Equal(value, await reader.GetDateTimeAsync(0));

                    Assert.Equal(value, reader.GetDateTime("field1"));
                    Assert.Equal(value, await reader.GetDateTimeAsync("field1"));

                    Assert.Equal(value, reader.GetDateTimeOrNull(0)!.Value);
                    Assert.Equal(value, (await reader.GetDateTimeOrNullAsync(0)).Value);

                    Assert.Equal(value, reader.GetDateTimeOrNull("field1")!.Value);
                    Assert.Equal(value, (await reader.GetDateTimeOrNullAsync("field1")).Value);

                    Assert.Equal(value, reader.GetDateTimeOrDefault(0));
                    Assert.Equal(value, await reader.GetDateTimeOrDefaultAsync(0));

                    Assert.Equal(value, reader.GetDateTimeOrDefault("field1"));
                    Assert.Equal(value, await reader.GetDateTimeOrDefaultAsync("field1"));

                    Assert.True(reader.IsDBNull(1));
                    Assert.True(reader.IsDBNull("field2"));

                    Assert.True(await reader.IsDBNullAsync(1));
                    Assert.True(await reader.IsDBNullAsync("field2"));

                    Assert.False(reader.GetDateTimeOrNull(1).HasValue);
                    Assert.False((await reader.GetDateTimeOrNullAsync(1)).HasValue);

                    Assert.False(reader.GetDateTimeOrNull("field2").HasValue);
                    Assert.False((await reader.GetDateTimeOrNullAsync("field2")).HasValue);

                    Assert.Equal(default(DateTime), reader.GetDateTimeOrDefault("field2"));
                    Assert.Equal(default(DateTime), await reader.GetDateTimeOrDefaultAsync("field2"));
                }
            }

            transaction.Rollback();
            connection.Close();
        }

        /// <summary>
        /// Tests the GetChar extension method for various scenarios.
        /// </summary>
        /// <exception cref="NotSupportedException">
        /// SqlDataReader does not support GetChar.
        /// </exception>
        [Fact(Skip = "SqlDataReader does not support GetChar.")]
        public async Task GetChar()
        {
            using var connection = SampleDatabase.CreateConnection();

            connection.Open();

            using var transaction = connection.BeginTransaction();

            // create temporary table
            var tableName = $"#{nameof(TestDataRecordExtensions)}_{nameof(GetChar)}";

            using (var command = connection.CreateCommand())
            {
                command.Transaction = transaction;
                command.CommandText = $"create table {tableName} ( field1 nchar(1), field2 nchar(1) )";
                command.ExecuteNonQuery();
            }

            char value = 'a';

            // insert data
            using (var command = connection.CreateCommand())
            {
                command.Transaction = transaction;
                command.CommandText = $"insert into {tableName} values ( '{value}', null )";
                command.ExecuteNonQuery();
            }

            // get field value
            using (var command = connection.CreateCommand())
            {
                command.Transaction = transaction;
                command.CommandText = $"select * from {tableName}";

                using var reader = command.ExecuteReader();

                while (reader.Read())
                {
                    Assert.Equal(value, reader.GetChar(0));
                    Assert.Equal(value, await reader.GetCharAsync(0));

                    Assert.Equal(value, reader.GetChar("field1"));
                    Assert.Equal(value, await reader.GetCharAsync("field1"));

                    Assert.Equal(value, reader.GetCharOrNull(0)!.Value);
                    Assert.Equal(value, (await reader.GetCharOrNullAsync(0)).Value);

                    Assert.Equal(value, reader.GetCharOrNull("field1")!.Value);
                    Assert.Equal(value, (await reader.GetCharOrNullAsync("field1")).Value);

                    Assert.Equal(value, reader.GetCharOrDefault(0));
                    Assert.Equal(value, await reader.GetCharOrDefaultAsync(0));

                    Assert.Equal(value, reader.GetCharOrDefault("field1"));
                    Assert.Equal(value, await reader.GetCharOrDefaultAsync("field1"));

                    Assert.True(reader.IsDBNull(1));
                    Assert.True(reader.IsDBNull("field2"));

                    Assert.True(await reader.IsDBNullAsync(1));
                    Assert.True(await reader.IsDBNullAsync("field2"));

                    Assert.False(reader.GetCharOrNull(1).HasValue);
                    Assert.False((await reader.GetCharOrNullAsync(1)).HasValue);

                    Assert.False(reader.GetCharOrNull("field2").HasValue);
                    Assert.False((await reader.GetCharOrNullAsync("field2")).HasValue);

                    Assert.Equal(default(char), reader.GetCharOrDefault("field2"));
                    Assert.Equal(default(char), await reader.GetCharOrDefaultAsync("field2"));
                }
            }

            transaction.Rollback();
            connection.Close();
        }

        /// <summary>
        /// Tests the GetString extension method for various scenarios.
        /// </summary>
        [Fact]
        public async Task GetString()
        {
            using var connection = SampleDatabase.CreateConnection();

            connection.Open();

            using var transaction = connection.BeginTransaction();

            // create temporary table
            var tableName = $"#{nameof(TestDataRecordExtensions)}_{nameof(GetString)}";

            using (var command = connection.CreateCommand())
            {
                command.Transaction = transaction;
                command.CommandText = $"create table {tableName} ( field1 nvarchar(5), field2 nvarchar(5) )";
                command.ExecuteNonQuery();
            }

            string value = "abc";

            // insert data
            using (var command = connection.CreateCommand())
            {
                command.Transaction = transaction;
                command.CommandText = $"insert into {tableName} values ( '{value}', null )";
                command.ExecuteNonQuery();
            }

            // get field value
            using (var command = connection.CreateCommand())
            {
                command.Transaction = transaction;
                command.CommandText = $"select * from {tableName}";

                using var reader = command.ExecuteReader();

                while (reader.Read())
                {
                    Assert.Equal(value, reader.GetString(0));
                    Assert.Equal(value, await reader.GetStringAsync(0));

                    Assert.Equal(value, reader.GetString("field1"));
                    Assert.Equal(value, await reader.GetStringAsync("field1"));

                    Assert.Equal(value, reader.GetStringOrNull(0));
                    Assert.Equal(value, (await reader.GetStringOrNullAsync(0)));

                    Assert.Equal(value, reader.GetStringOrNull("field1"));
                    Assert.Equal(value, (await reader.GetStringOrNullAsync("field1")));

                    Assert.True(reader.IsDBNull(1));
                    Assert.True(reader.IsDBNull("field2"));

                    Assert.True(await reader.IsDBNullAsync(1));
                    Assert.True(await reader.IsDBNullAsync("field2"));

                    Assert.Null(reader.GetStringOrNull(1));
                    Assert.Null((await reader.GetStringOrNullAsync(1)));

                    Assert.Null(reader.GetStringOrNull("field2"));
                    Assert.Null((await reader.GetStringOrNullAsync("field2")));
                }
            }

            transaction.Rollback();
            connection.Close();
        }

        /// <summary>
        /// Tests the GetGuid extension method for various scenarios.
        /// </summary>
        [Fact]
        public async Task GetGuid()
        {
            using var connection = SampleDatabase.CreateConnection();

            connection.Open();

            using var transaction = connection.BeginTransaction();

            // create temporary table
            var tableName = $"#{nameof(TestDataRecordExtensions)}_{nameof(GetGuid)}";

            using (var command = connection.CreateCommand())
            {
                command.Transaction = transaction;
                command.CommandText = $"create table {tableName} ( field1 uniqueIdentifier, field2 uniqueIdentifier )";
                command.ExecuteNonQuery();
            }

            var value = Guid.NewGuid();

            // insert data
            using (var command = connection.CreateCommand())
            {
                command.Transaction = transaction;
                command.CommandText = $"insert into {tableName} values ( '{value}', null )";
                command.ExecuteNonQuery();
            }

            // get field value
            using (var command = connection.CreateCommand())
            {
                command.Transaction = transaction;
                command.CommandText = $"select * from {tableName}";

                using var reader = command.ExecuteReader();

                while (reader.Read())
                {
                    Assert.Equal(value, reader.GetGuid(0));
                    Assert.Equal(value, await reader.GetGuidAsync(0));

                    Assert.Equal(value, reader.GetGuid("field1"));
                    Assert.Equal(value, await reader.GetGuidAsync("field1"));

                    Assert.Equal(value, reader.GetGuidOrNull(0)!.Value);
                    Assert.Equal(value, (await reader.GetGuidOrNullAsync(0)).Value);

                    Assert.Equal(value, reader.GetGuidOrNull("field1")!.Value);
                    Assert.Equal(value, (await reader.GetGuidOrNullAsync("field1")).Value);

                    Assert.Equal(value, reader.GetGuidOrDefault(0));
                    Assert.Equal(value, await reader.GetGuidOrDefaultAsync(0));

                    Assert.Equal(value, reader.GetGuidOrDefault("field1"));
                    Assert.Equal(value, await reader.GetGuidOrDefaultAsync("field1"));

                    Assert.True(reader.IsDBNull(1));
                    Assert.True(reader.IsDBNull("field2"));

                    Assert.True(await reader.IsDBNullAsync(1));
                    Assert.True(await reader.IsDBNullAsync("field2"));

                    Assert.False(reader.GetGuidOrNull(1).HasValue);
                    Assert.False((await reader.GetGuidOrNullAsync(1)).HasValue);

                    Assert.False(reader.GetGuidOrNull("field2").HasValue);
                    Assert.False((await reader.GetGuidOrNullAsync("field2")).HasValue);

                    Assert.Equal(default(Guid), reader.GetGuidOrDefault("field2"));
                    Assert.Equal(default(Guid), await reader.GetGuidOrDefaultAsync("field2"));
                }
            }

            transaction.Rollback();
            connection.Close();
        }

        /// <summary>
        /// Tests the GetBytes extension method for various scenarios.
        /// </summary>
        [Fact]
        public async Task GetBytes()
        {
            using var connection = SampleDatabase.CreateConnection();

            connection.Open();

            using var transaction = connection.BeginTransaction();

            // create temporary table
            var tableName = $"#{nameof(TestDataRecordExtensions)}_{nameof(GetBytes)}";

            using (var command = connection.CreateCommand())
            {
                command.Transaction = transaction;
                command.CommandText = $"create table {tableName} ( field1 varbinary(10), field2 varbinary(10) )";
                command.ExecuteNonQuery();
            }

            byte[] value = { 1, 2, 3 };

            // insert data
            using (var command = connection.CreateCommand())
            {
                command.Transaction = transaction;
                command.CommandText = $"insert into {tableName} values ( @value, null )";
                command.AddBinaryParameter("value", value);
                command.ExecuteNonQuery();
            }

            // get field value
            using (var command = connection.CreateCommand())
            {
                command.Transaction = transaction;
                command.CommandText = $"select * from {tableName}";

                using var reader = command.ExecuteReader();

                while (reader.Read())
                {
                    byte[] buffer = new byte[3];

                    reader.GetBytes(0, 0, buffer, 0, buffer.Length);
                    Assert.Equal(value, buffer);
                    await reader.GetBytesAsync(0, 0, buffer, 0, buffer.Length);
                    Assert.Equal(value, buffer);

                    reader.GetBytes("field1", 0, buffer, 0, buffer.Length);
                    Assert.Equal(value, buffer);
                    await reader.GetBytesAsync("field1", 0, buffer, 0, buffer.Length);
                    Assert.Equal(value, buffer);

                    Assert.True(reader.IsDBNull(1));
                    Assert.True(reader.IsDBNull("field2"));

                    Assert.True(await reader.IsDBNullAsync(1));
                    Assert.True(await reader.IsDBNullAsync("field2"));
                }
            }

            transaction.Rollback();
            connection.Close();
        }

        /// <summary>
        /// Tests the GetChars extension method for various scenarios.
        /// </summary>
        [Fact]
        public async Task GetChars()
        {
            using var connection = SampleDatabase.CreateConnection();

            connection.Open();

            using var transaction = connection.BeginTransaction();

            // create temporary table
            var tableName = $"#{nameof(TestDataRecordExtensions)}_{nameof(GetChars)}";

            using (var command = connection.CreateCommand())
            {
                command.Transaction = transaction;
                command.CommandText = $"create table {tableName} ( field1 nvarchar(10), field2 nvarchar(10) )";
                command.ExecuteNonQuery();
            }

            char[] value = { 'a', 'b', 'c' };

            // insert data
            using (var command = connection.CreateCommand())
            {
                command.Transaction = transaction;
                command.CommandText = $"insert into {tableName} values ( '{new string(value)}', null )";
                command.ExecuteNonQuery();
            }

            // get field value
            using (var command = connection.CreateCommand())
            {
                command.Transaction = transaction;
                command.CommandText = $"select * from {tableName}";

                using var reader = command.ExecuteReader();

                while (reader.Read())
                {
                    char[] buffer = new char[3];

                    reader.GetChars(0, 0, buffer, 0, buffer.Length);
                    Assert.Equal(value, buffer);
                    await reader.GetCharsAsync(0, 0, buffer, 0, buffer.Length);
                    Assert.Equal(value, buffer);

                    reader.GetChars("field1", 0, buffer, 0, buffer.Length);
                    Assert.Equal(value, buffer);
                    await reader.GetCharsAsync("field1", 0, buffer, 0, buffer.Length);
                    Assert.Equal(value, buffer);

                    Assert.True(reader.IsDBNull(1));
                    Assert.True(reader.IsDBNull("field2"));

                    Assert.True(await reader.IsDBNullAsync(1));
                    Assert.True(await reader.IsDBNullAsync("field2"));
                }
            }

            transaction.Rollback();
            connection.Close();
        }
    }
}
