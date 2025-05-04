using System.Data;
using Xunit.Abstractions;
using System.Data.OleDb;
using System.Data.Odbc;
using Microsoft.Data.SqlClient;
using Microsoft.Data.Sqlite;
using Oracle.ManagedDataAccess.Client;
using Npgsql;
using mxProject.Data;
using mxProject.Data.Extensions;

namespace Test.Data.AdoNet
{
    /// <summary>
    /// Test class for IDbCommand extensions. 
    /// </summary>
    public class TestDbCommandExtensions
    {
        /// <summary>
        /// Creates a new instance.
        /// </summary>
        /// <param name="output"></param>
        public TestDbCommandExtensions(ITestOutputHelper output)
        {
            m_Output = output;
        }

        private readonly ITestOutputHelper m_Output;

        #region AddParameter

        /// <summary>
        /// Tests the addition of a parameter to a database command.
        /// </summary>
        /// <param name="commandType">The type of the database command.</param>
        [Theory]
        [MemberData(nameof(AddParameter_MemberData))]
        public void AddParameter(Type commandType)
        {
            using var command = (IDbCommand)Activator.CreateInstance(commandType)!;

            var (index, parameter) = command.AddParameter("param1", 1);

            Assert.Equal(0, index);
            Assert.Equal("param1", parameter.ParameterName);
            Assert.Equal(1, parameter.Value);

            (index, parameter) = command.AddParameter("param2");

            Assert.Equal(1, index);
            Assert.Equal("param2", parameter.ParameterName);
            Assert.Equal(DBNull.Value, parameter.Value);
        }

        /// <summary>
        /// Tests the addition of a boolean parameter to a database command.
        /// </summary>
        /// <param name="commandType">The type of the database command.</param>
        [Theory]
        [MemberData(nameof(AddParameter_MemberData))]
        public void AddBooleanParameter(Type commandType)
        {
            using var command = (IDbCommand)Activator.CreateInstance(commandType)!;

            bool expectedValue = true;
            DbType expectedDbType = DbType.Boolean;

            var (index, parameter) = command.AddBooleanParameter("param1", expectedValue);
            
            Assert.Equal(0, index);
            Assert.Equal("param1", parameter.ParameterName);
            Assert.Equal(expectedValue, parameter.Value);
            Assert.Equal(expectedDbType, parameter.DbType);

            (index, parameter) = command.AddBooleanParameter("param2");

            Assert.Equal(1, index);
            Assert.Equal("param2", parameter.ParameterName);
            Assert.Equal(DBNull.Value, parameter.Value);
            Assert.Equal(expectedDbType, parameter.DbType);

            command.SetBooleanParameterValue("param2", expectedValue);

            Assert.Equal(expectedValue, parameter.Value);
        }

        /// <summary>
        /// Tests the addition of a byte parameter to a database command.
        /// </summary>
        /// <param name="commandType">The type of the database command.</param>
        /// <remarks>
        /// <list type="bullet">
        /// <item>
        /// For NpgsqlCommand this maps to DbType.Int16.
        /// </item>
        /// </remarks>
        [Theory]
        [MemberData(nameof(AddParameter_MemberData))]
        public void AddByteParameter(Type commandType)
        {
            using var command = (IDbCommand)Activator.CreateInstance(commandType)!;

            byte expectedValue = 1;
            DbType expectedDbType = commandType == typeof(NpgsqlCommand) ? DbType.Int16 : DbType.Byte;

            var (index, parameter) = command.AddByteParameter("param1", expectedValue);

            Assert.Equal(0, index);
            Assert.Equal("param1", parameter.ParameterName);
            Assert.Equal(expectedValue, parameter.Value);
            Assert.Equal(expectedDbType, parameter.DbType);

            (index, parameter) = command.AddByteParameter("param2");

            Assert.Equal(1, index);
            Assert.Equal("param2", parameter.ParameterName);
            Assert.Equal(DBNull.Value, parameter.Value);
            Assert.Equal(expectedDbType, parameter.DbType);

            command.SetByteParameterValue("param2", expectedValue);

            Assert.Equal(expectedValue, parameter.Value);
        }

        /// <summary>
        /// Tests the addition of a signed byte parameter to a database command.
        /// </summary>
        /// <param name="commandType">The type of the database command.</param>
        /// <remarks>
        /// <list type="bullet">
        /// <item>
        /// OdbcCommand does not support DbType.SByte.
        /// </item>
        /// <item>
        /// SqlCommand does not support DbType.SByte.
        /// </item>
        /// <item>
        /// OracleCommand does not support DbType.SByte.
        /// </item>
        /// <item>
        /// NpgsqlCommand does not support DbType.SByte.
        /// </item>
        /// </remarks>
        [Theory]
        [MemberData(nameof(AddParameter_MemberData))]
        public void AddSByteParameter(Type commandType)
        {
            try
            {
                using var command = (IDbCommand)Activator.CreateInstance(commandType)!;

                sbyte expectedValue = 1;
                DbType expectedDbType = DbType.SByte;

                var (index, parameter) = command.AddSByteParameter("param1", expectedValue);

                Assert.Equal(0, index);
                Assert.Equal("param1", parameter.ParameterName);
                Assert.Equal(expectedValue, parameter.Value);
                Assert.Equal(expectedDbType, parameter.DbType);

                (index, parameter) = command.AddSByteParameter("param2");

                Assert.Equal(1, index);
                Assert.Equal("param2", parameter.ParameterName);
                Assert.Equal(DBNull.Value, parameter.Value);
                Assert.Equal(expectedDbType, parameter.DbType);

                command.SetSByteParameterValue("param2", expectedValue);

                Assert.Equal(expectedValue, parameter.Value);
            }
            catch (ArgumentException ex) when (commandType == typeof(OdbcCommand) || commandType == typeof(SqlCommand) || commandType == typeof(OracleCommand))
            {
                m_Output.WriteLine(ex.ToString());
            }
            catch (NotSupportedException ex) when (commandType == typeof(NpgsqlCommand))
            {
                m_Output.WriteLine(ex.ToString());
            }
        }

        /// <summary>
        /// Tests the addition of a 16-bit integer parameter to a database command.
        /// </summary>
        /// <param name="commandType">The type of the database command.</param>
        [Theory]
        [MemberData(nameof(AddParameter_MemberData))]
        public void AddInt16Parameter(Type commandType)
        {
            using var command = (IDbCommand)Activator.CreateInstance(commandType)!;

            short expectedValue = 1;
            DbType expectedDbType = DbType.Int16;

            var (index, parameter) = command.AddInt16Parameter("param1", expectedValue);

            Assert.Equal(0, index);
            Assert.Equal("param1", parameter.ParameterName);
            Assert.Equal(expectedValue, parameter.Value);
            Assert.Equal(expectedDbType, parameter.DbType);

            (index, parameter) = command.AddInt16Parameter("param2");

            Assert.Equal(1, index);
            Assert.Equal("param2", parameter.ParameterName);
            Assert.Equal(DBNull.Value, parameter.Value);
            Assert.Equal(expectedDbType, parameter.DbType);

            command.SetInt16ParameterValue("param2", expectedValue);

            Assert.Equal(expectedValue, parameter.Value);
        }

        /// <summary>
        /// Tests the addition of a 16-bit unsigned integer parameter to a database command.
        /// </summary>
        /// <param name="commandType">The type of the database command.</param>
        /// <remarks>
        /// <list type="bullet">
        /// <item>
        /// OdbcCommand does not support DbType.UInt16.
        /// </item>
        /// <item>
        /// SqlCommand does not support DbType.UInt16.
        /// </item>
        /// <item>
        /// OracleCommand does not support DbType.UInt16.
        /// </item>
        /// <item>
        /// NpgsqlCommand does not support DbType.UInt16.
        /// </item>
        /// </remarks>
        [Theory]
        [MemberData(nameof(AddParameter_MemberData))]
        public void AddUInt16Parameter(Type commandType)
        {
            try
            {
                using var command = (IDbCommand)Activator.CreateInstance(commandType)!;

                ushort expectedValue = 1;
                DbType expectedDbType = DbType.UInt16;

                var (index, parameter) = command.AddUInt16Parameter("param1", expectedValue);

                Assert.Equal(0, index);
                Assert.Equal("param1", parameter.ParameterName);
                Assert.Equal(expectedValue, parameter.Value);
                Assert.Equal(expectedDbType, parameter.DbType);

                (index, parameter) = command.AddUInt16Parameter("param2");

                Assert.Equal(1, index);
                Assert.Equal("param2", parameter.ParameterName);
                Assert.Equal(DBNull.Value, parameter.Value);
                Assert.Equal(expectedDbType, parameter.DbType);

                command.SetUInt16ParameterValue("param2", expectedValue);

                Assert.Equal(expectedValue, parameter.Value);
            }
            catch (ArgumentException ex) when (commandType == typeof(OdbcCommand) || commandType == typeof(SqlCommand) || commandType == typeof(OracleCommand))
            {
                m_Output.WriteLine(ex.ToString());
            }
            catch (NotSupportedException ex) when (commandType == typeof(NpgsqlCommand))
            {
                m_Output.WriteLine(ex.ToString());
            }
        }

        /// <summary>
        /// Tests the addition of a 32-bit integer parameter to a database command.
        /// </summary>
        /// <param name="commandType">The type of the database command.</param>
        [Theory]
        [MemberData(nameof(AddParameter_MemberData))]
        public void AddInt32Parameter(Type commandType)
        {
            using var command = (IDbCommand)Activator.CreateInstance(commandType)!;

            int expectedValue = 1;
            DbType expectedDbType = DbType.Int32;

            var (index, parameter) = command.AddInt32Parameter("param1", expectedValue);

            Assert.Equal(0, index);
            Assert.Equal("param1", parameter.ParameterName);
            Assert.Equal(expectedValue, parameter.Value);
            Assert.Equal(expectedDbType, parameter.DbType);

            (index, parameter) = command.AddInt32Parameter("param2");

            Assert.Equal(1, index);
            Assert.Equal("param2", parameter.ParameterName);
            Assert.Equal(DBNull.Value, parameter.Value);
            Assert.Equal(expectedDbType, parameter.DbType);

            command.SetInt32ParameterValue("param2", expectedValue);

            Assert.Equal(expectedValue, parameter.Value);
        }

        /// <summary>
        /// Tests the addition of a 32-bit unsigned integer parameter to a database command.
        /// </summary>
        /// <param name="commandType">The type of the database command.</param>
        /// <remarks>
        /// <list type="bullet">
        /// <item>
        /// OdbcCommand does not support DbType.UInt32.
        /// </item>
        /// <item>
        /// SqlCommand does not support DbType.UInt32.
        /// </item>
        /// <item>
        /// OracleCommand does not support DbType.UInt32.
        /// </item>
        /// <item>
        /// NpgsqlCommand does not support DbType.UInt32.
        /// </item>
        /// </remarks>
        [Theory]
        [MemberData(nameof(AddParameter_MemberData))]
        public void AddUInt32Parameter(Type commandType)
        {
            try
            {
                using var command = (IDbCommand)Activator.CreateInstance(commandType)!;

                uint expectedValue = 1;
                DbType expectedDbType = DbType.UInt32;

                var (index, parameter) = command.AddUInt32Parameter("param1", expectedValue);

                Assert.Equal(0, index);
                Assert.Equal("param1", parameter.ParameterName);
                Assert.Equal(expectedValue, parameter.Value);
                Assert.Equal(expectedDbType, parameter.DbType);

                (index, parameter) = command.AddUInt32Parameter("param2");

                Assert.Equal(1, index);
                Assert.Equal("param2", parameter.ParameterName);
                Assert.Equal(DBNull.Value, parameter.Value);
                Assert.Equal(expectedDbType, parameter.DbType);

                command.SetUInt32ParameterValue("param2", expectedValue);

                Assert.Equal(expectedValue, parameter.Value);
            }
            catch (ArgumentException ex) when (commandType == typeof(OdbcCommand) || commandType == typeof(SqlCommand) || commandType == typeof(OracleCommand))
            {
                m_Output.WriteLine(ex.ToString());
            }
            catch (NotSupportedException ex) when (commandType == typeof(NpgsqlCommand))
            {
                m_Output.WriteLine(ex.ToString());
            }
        }

        /// <summary>
        /// Tests the addition of a 64-bit integer parameter to a database command.
        /// </summary>
        /// <param name="commandType">The type of the database command.</param>
        [Theory]
        [MemberData(nameof(AddParameter_MemberData))]
        public void AddInt64Parameter(Type commandType)
        {
            using var command = (IDbCommand)Activator.CreateInstance(commandType)!;

            long expectedValue = 1;
            DbType expectedDbType = DbType.Int64;

            var (index, parameter) = command.AddInt64Parameter("param1", expectedValue);

            Assert.Equal(0, index);
            Assert.Equal("param1", parameter.ParameterName);
            Assert.Equal(expectedValue, parameter.Value);
            Assert.Equal(expectedDbType, parameter.DbType);

            (index, parameter) = command.AddInt64Parameter("param2");

            Assert.Equal(1, index);
            Assert.Equal("param2", parameter.ParameterName);
            Assert.Equal(DBNull.Value, parameter.Value);
            Assert.Equal(expectedDbType, parameter.DbType);

            command.SetInt64ParameterValue("param2", expectedValue);

            Assert.Equal(expectedValue, parameter.Value);
        }

        /// <summary>
        /// Tests the addition of a 64-bit unsigned integer parameter to a database command.
        /// </summary>
        /// <param name="commandType">The type of the database command.</param>
        /// <remarks>
        /// <list type="bullet">
        /// <item>
        /// OdbcCommand does not support DbType.UInt32.
        /// </item>
        /// <item>
        /// SqlCommand does not support DbType.UInt32.
        /// </item>
        /// <item>
        /// OracleCommand does not support DbType.UInt32.
        /// </item>
        /// <item>
        /// NpgsqlCommand does not support DbType.UInt32.
        /// </item>
        /// </remarks>
        [Theory]
        [MemberData(nameof(AddParameter_MemberData))]
        public void AddUInt64Parameter(Type commandType)
        {
            try
            {
                using var command = (IDbCommand)Activator.CreateInstance(commandType)!;

                ulong expectedValue = 1;
                DbType expectedDbType = DbType.UInt64;

                var (index, parameter) = command.AddUInt64Parameter("param1", expectedValue);

                Assert.Equal(0, index);
                Assert.Equal("param1", parameter.ParameterName);
                Assert.Equal(expectedValue, parameter.Value);
                Assert.Equal(expectedDbType, parameter.DbType);

                (index, parameter) = command.AddUInt64Parameter("param2");

                Assert.Equal(1, index);
                Assert.Equal("param2", parameter.ParameterName);
                Assert.Equal(DBNull.Value, parameter.Value);
                Assert.Equal(expectedDbType, parameter.DbType);

                command.SetUInt64ParameterValue("param2", expectedValue);

                Assert.Equal(expectedValue, parameter.Value);
            }
            catch (ArgumentException ex) when (commandType == typeof(OdbcCommand) || commandType == typeof(SqlCommand) || commandType == typeof(OracleCommand))
            {
                m_Output.WriteLine(ex.ToString());
            }
            catch (NotSupportedException ex) when (commandType == typeof(NpgsqlCommand))
            {
                m_Output.WriteLine(ex.ToString());
            }
        }

        /// <summary>
        /// Tests the addition of a single-precision floating-point parameter to a database command.
        /// </summary>
        /// <param name="commandType">The type of the database command.</param>
        [Theory]
        [MemberData(nameof(AddParameter_MemberData))]
        public void AddSingleParameter(Type commandType)
        {
            using var command = (IDbCommand)Activator.CreateInstance(commandType)!;

            float expectedValue = 1;
            DbType expectedDbType = DbType.Single;

            var (index, parameter) = command.AddSingleParameter("param1", expectedValue);

            Assert.Equal(0, index);
            Assert.Equal("param1", parameter.ParameterName);
            Assert.Equal(expectedValue, parameter.Value);
            Assert.Equal(expectedDbType, parameter.DbType);

            (index, parameter) = command.AddSingleParameter("param2");

            Assert.Equal(1, index);
            Assert.Equal("param2", parameter.ParameterName);
            Assert.Equal(DBNull.Value, parameter.Value);
            Assert.Equal(expectedDbType, parameter.DbType);

            command.SetSingleParameterValue("param2", expectedValue);

            Assert.Equal(expectedValue, parameter.Value);
        }

        /// <summary>
        /// Tests the addition of a double-precision floating-point parameter to a database command.
        /// </summary>
        /// <param name="commandType">The type of the database command.</param>
        [Theory]
        [MemberData(nameof(AddParameter_MemberData))]
        public void AddDoubleParameter(Type commandType)
        {
            using var command = (IDbCommand)Activator.CreateInstance(commandType)!;

            double expectedValue = 1;
            DbType expectedDbType = DbType.Double;

            var (index, parameter) = command.AddDoubleParameter("param1", expectedValue);

            Assert.Equal(0, index);
            Assert.Equal("param1", parameter.ParameterName);
            Assert.Equal(expectedValue, parameter.Value);
            Assert.Equal(expectedDbType, parameter.DbType);

            (index, parameter) = command.AddDoubleParameter("param2");

            Assert.Equal(1, index);
            Assert.Equal("param2", parameter.ParameterName);
            Assert.Equal(DBNull.Value, parameter.Value);
            Assert.Equal(expectedDbType, parameter.DbType);

            command.SetDoubleParameterValue("param2", expectedValue);

            Assert.Equal(expectedValue, parameter.Value);
        }

        /// <summary>
        /// Tests the addition of a decimal parameter to a database command.
        /// </summary>
        /// <param name="commandType">The type of the database command.</param>
        [Theory]
        [MemberData(nameof(AddParameter_MemberData))]
        public void AddDecimalParameter(Type commandType)
        {
            using var command = (IDbCommand)Activator.CreateInstance(commandType)!;

            decimal expectedValue = 1;
            DbType expectedDbType = DbType.Decimal;

            var (index, parameter) = command.AddDecimalParameter("param1", expectedValue);

            Assert.Equal(0, index);
            Assert.Equal("param1", parameter.ParameterName);
            Assert.Equal(expectedValue, parameter.Value);
            Assert.Equal(expectedDbType, parameter.DbType);

            (index, parameter) = command.AddDecimalParameter("param2");

            Assert.Equal(1, index);
            Assert.Equal("param2", parameter.ParameterName);
            Assert.Equal(DBNull.Value, parameter.Value);
            Assert.Equal(expectedDbType, parameter.DbType);

            command.SetDecimalParameterValue("param2", expectedValue);

            Assert.Equal(expectedValue, parameter.Value);
        }

        /// <summary>
        /// Tests the addition of a DateTime parameter to a database command.
        /// </summary>
        /// <param name="commandType">The type of the database command.</param>
        [Theory]
        [MemberData(nameof(AddParameter_MemberData))]
        public void AddDateTimeParameter(Type commandType)
        {
            using var command = (IDbCommand)Activator.CreateInstance(commandType)!;

            DateTime expectedValue = DateTime.Now;
            DbType expectedDbType = DbType.DateTime;

            var (index, parameter) = command.AddDateTimeParameter("param1", expectedValue);

            Assert.Equal(0, index);
            Assert.Equal("param1", parameter.ParameterName);
            Assert.Equal(expectedValue, parameter.Value);
            Assert.Equal(expectedDbType, parameter.DbType);

            (index, parameter) = command.AddDateTimeParameter("param2");

            Assert.Equal(1, index);
            Assert.Equal("param2", parameter.ParameterName);
            Assert.Equal(DBNull.Value, parameter.Value);
            Assert.Equal(expectedDbType, parameter.DbType);

            command.SetDateTimeParameterValue("param2", expectedValue);

            Assert.Equal(expectedValue, parameter.Value);
        }

        /// <summary>
        /// Tests the addition of a DateTimeOffset parameter to a database command.
        /// </summary>
        /// <param name="commandType">The type of the database command.</param>
        /// <remarks>
        /// <list type="bullet">
        /// <item>
        /// For NpgsqlCommand this maps to DbType.DateTime.
        /// </item>
        /// <item>
        /// OdbcCommand does not support DbType.DateTimeOffset.
        /// </item>
        /// <item>
        /// OleDbCommand does not support DbType.DateTimeOffset.
        /// </item>
        /// </remarks>
        [Theory]
        [MemberData(nameof(AddParameter_MemberData))]
        public void AddDateTimeOffsetParameter(Type commandType)
        {
            try
            {
                using var command = (IDbCommand)Activator.CreateInstance(commandType)!;

                DateTimeOffset expectedValue = DateTimeOffset.UtcNow;
                DbType expectedDbType = commandType == typeof(NpgsqlCommand) ? DbType.DateTime : DbType.DateTimeOffset;

                var (index, parameter) = command.AddDateTimeOffsetParameter("param1", expectedValue);

                Assert.Equal(0, index);
                Assert.Equal("param1", parameter.ParameterName);
                Assert.Equal(expectedValue, parameter.Value);
                Assert.Equal(expectedDbType, parameter.DbType);

                (index, parameter) = command.AddDateTimeOffsetParameter("param2");

                Assert.Equal(1, index);
                Assert.Equal("param2", parameter.ParameterName);
                Assert.Equal(DBNull.Value, parameter.Value);
                Assert.Equal(expectedDbType, parameter.DbType);

                command.SetDateTimeOffsetParameterValue("param2", expectedValue);

                Assert.Equal(expectedValue, parameter.Value);
            }
            catch (ArgumentException ex) when (commandType == typeof(OdbcCommand) || commandType == typeof(OleDbCommand))
            {
                m_Output.WriteLine(ex.ToString());
            }
        }

        /// <summary>
        /// Tests the addition of a TimeSpan parameter to a database command.
        /// </summary>
        /// <param name="commandType">The type of the database command.</param>
        [Theory]
        [MemberData(nameof(AddParameter_MemberData))]
        public void AddTimeParameter(Type commandType)
        {
            using var command = (IDbCommand)Activator.CreateInstance(commandType)!;

            TimeSpan expectedValue = DateTime.Now.TimeOfDay;
            DbType expectedDbType = DbType.Time;

            var (index, parameter) = command.AddTimeParameter("param1", expectedValue);

            Assert.Equal(0, index);
            Assert.Equal("param1", parameter.ParameterName);
            Assert.Equal(expectedValue, parameter.Value);
            Assert.Equal(expectedDbType, parameter.DbType);

            (index, parameter) = command.AddTimeParameter("param2");

            Assert.Equal(1, index);
            Assert.Equal("param2", parameter.ParameterName);
            Assert.Equal(DBNull.Value, parameter.Value);
            Assert.Equal(expectedDbType, parameter.DbType);

            command.SetTimeParameterValue("param2", expectedValue);

            Assert.Equal(expectedValue, parameter.Value);
        }

        /// <summary>
        /// Tests the addition of a string parameter to a database command.
        /// </summary>
        /// <param name="commandType">The type of the database command.</param>
        [Theory]
        [MemberData(nameof(AddParameter_MemberData))]
        public void AddStringParameter(Type commandType)
        {
            using var command = (IDbCommand)Activator.CreateInstance(commandType)!;

            string expectedValue = "a";
            DbType expectedDbType = DbType.String;

            var (index, parameter) = command.AddStringParameter("param1", expectedValue);

            Assert.Equal(0, index);
            Assert.Equal("param1", parameter.ParameterName);
            Assert.Equal(expectedValue, parameter.Value);
            Assert.Equal(expectedDbType, parameter.DbType);

            (index, parameter) = command.AddStringParameter("param2");

            Assert.Equal(1, index);
            Assert.Equal("param2", parameter.ParameterName);
            Assert.Equal(DBNull.Value, parameter.Value);
            Assert.Equal(expectedDbType, parameter.DbType);

            command.SetStringParameterValue("param2", expectedValue);

            Assert.Equal(expectedValue, parameter.Value);
        }

        /// <summary>
        /// Tests the addition of a character parameter to a database command.
        /// </summary>
        /// <param name="commandType">The type of the database command.</param>
        [Theory]
        [MemberData(nameof(AddParameter_MemberData))]
        public void AddCharParameter(Type commandType)
        {
            using var command = (IDbCommand)Activator.CreateInstance(commandType)!;

            char expectedValue = 'a';
            DbType expectedDbType = DbType.String;

            var (index, parameter) = command.AddCharParameter("param1", expectedValue);

            Assert.Equal(0, index);
            Assert.Equal("param1", parameter.ParameterName);
            Assert.Equal(expectedValue, parameter.Value);
            Assert.Equal(expectedDbType, parameter.DbType);

            (index, parameter) = command.AddCharParameter("param2");

            Assert.Equal(1, index);
            Assert.Equal("param2", parameter.ParameterName);
            Assert.Equal(DBNull.Value, parameter.Value);
            Assert.Equal(expectedDbType, parameter.DbType);

            command.SetCharParameterValue("param2", expectedValue);

            Assert.Equal(expectedValue, parameter.Value);
        }

        /// <summary>
        /// Tests the addition of a GUID parameter to a database command.
        /// </summary>
        /// <param name="commandType">The type of the database command.</param>
        /// <remarks>
        /// <list type="bullet">
        /// <item>
        /// OracleCommand does not support DbType.Guid.
        /// </item>
        /// </remarks>
        [Theory]
        [MemberData(nameof(AddParameter_MemberData))]
        public void AddGuidParameter(Type commandType)
        {
            try
            {
                using var command = (IDbCommand)Activator.CreateInstance(commandType)!;

                Guid expectedValue = Guid.NewGuid();
                DbType expectedDbType = DbType.Guid;

                var (index, parameter) = command.AddGuidParameter("param1", expectedValue);

                Assert.Equal(0, index);
                Assert.Equal("param1", parameter.ParameterName);
                Assert.Equal(expectedValue, parameter.Value);
                Assert.Equal(expectedDbType, parameter.DbType);

                (index, parameter) = command.AddGuidParameter("param2");

                Assert.Equal(1, index);
                Assert.Equal("param2", parameter.ParameterName);
                Assert.Equal(DBNull.Value, parameter.Value);
                Assert.Equal(expectedDbType, parameter.DbType);

                command.SetGuidParameterValue("param2", expectedValue);

                Assert.Equal(expectedValue, parameter.Value);
            }
            catch (ArgumentException ex) when (commandType == typeof(OracleCommand))
            {
                m_Output.WriteLine(ex.ToString());
            }
        }

        /// <summary>
        /// Tests the addition of a binary parameter to a database command.
        /// </summary>
        /// <param name="commandType">The type of the database command.</param>
        [Theory]
        [MemberData(nameof(AddParameter_MemberData))]
        public void AddBinaryParameter(Type commandType)
        {
            using var command = (IDbCommand)Activator.CreateInstance(commandType)!;

            byte[] expectedValue = [1, 2, 3];
            DbType expectedDbType = DbType.Binary;

            var (index, parameter) = command.AddBinaryParameter("param1", expectedValue);

            Assert.Equal(0, index);
            Assert.Equal("param1", parameter.ParameterName);
            Assert.Equal(expectedValue, parameter.Value);
            Assert.Equal(expectedDbType, parameter.DbType);

            (index, parameter) = command.AddBinaryParameter("param2");

            Assert.Equal(1, index);
            Assert.Equal("param2", parameter.ParameterName);
            Assert.Equal(DBNull.Value, parameter.Value);
            Assert.Equal(expectedDbType, parameter.DbType);

            command.SetBinaryParameterValue("param2", expectedValue);

            Assert.Equal(expectedValue, parameter.Value);
        }

        public static IEnumerable<object[]> AddParameter_MemberData()
        {
            yield return new object[]
            {
                typeof(OleDbCommand)
            };
            yield return new object[]
            {
                typeof(OdbcCommand)
            };
            yield return new object[]
            {
                typeof(SqlCommand)
            };
            yield return new object[]
            {
                typeof(SqliteCommand)
            };
            yield return new object[]
            {
                typeof(NpgsqlCommand)
            };
            yield return new object[]
            {
                typeof(OracleCommand)
            };
        }

        #endregion

        #region GetParameter

        /// <summary>
        /// Retrieves a parameter from the database command by its name.
        /// </summary>
        /// <param name="commandType">The type of the database command.</param>
        /// <remarks>
        /// <list type="bullet">
        /// <item>
        /// In SqliteCommand, parameter names are case sensitive.
        /// </item>
        /// </list>
        /// </remarks>
        [Theory]
        [MemberData(nameof(AddParameter_MemberData))]
        public void GetParameter(Type commandType)
        {
            using var command = (IDbCommand)Activator.CreateInstance(commandType)!;

            Assert.Throws<ArgumentException>(
                () => command.GetParameter("param1")
                );

            command.AddParameter("param1");

            var parameter = command.GetParameter("param1");

            Assert.NotNull(parameter);
            Assert.Equal("param1", parameter.ParameterName);

            try
            {
                parameter = command.GetParameter("PARAM1");

                Assert.NotNull(parameter);
                Assert.Equal("param1", parameter.ParameterName);
            }
            catch (ArgumentException ex) when (commandType == typeof(SqliteCommand))
            {
                m_Output.WriteLine(ex.ToString());
            }
        }

        #endregion

        #region Cast

        [Fact]
        public void Cast()
        {
            using var connection = SampleDatabase.CreateConnection();

            using (var command = connection.CreateCommand())
            {
                command.Cast<SqlCommand>(x => x.EnableOptimizedParameterBinding = true);

                var enable = command.Cast<SqlCommand, bool>(x => x.EnableOptimizedParameterBinding);

                Assert.True(enable);
            }

            var wrapper = connection.WithFilter();

            using (var command = wrapper.CreateCommand())
            {
                command.Cast<SqlCommand>(x => x.EnableOptimizedParameterBinding = false);

                var enable = command.Cast<SqlCommand, bool>(x => x.EnableOptimizedParameterBinding);

                Assert.False(enable);
            }
        }

        #endregion
    }
}
