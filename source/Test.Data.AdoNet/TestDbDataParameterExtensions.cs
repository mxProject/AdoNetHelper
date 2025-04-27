using Microsoft.Data.SqlClient;
using Microsoft.Data.Sqlite;
using Npgsql;
using Oracle.ManagedDataAccess.Client;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Odbc;
using System.Data.OleDb;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using mxProject.Data.Extensions;
using Newtonsoft.Json.Linq;
using Xunit.Abstractions;

namespace Test.Data.AdoNet
{
    /// <summary>  
    /// Test class for IDbCommand extensions.  
    /// </summary>  
    public class TestDbDataParameterExtensions
    {
        /// <summary>
        /// Creates a new instance.
        /// </summary>
        /// <param name="output"></param>
        public TestDbDataParameterExtensions(ITestOutputHelper output)
        {
            m_Output = output;
        }

        private readonly ITestOutputHelper m_Output;

        /// <summary>  
        /// Tests the SetValue extension method for IDbDataParameter.  
        /// </summary>  
        /// <param name="commandType">The type of the command to create.</param>  
        [Theory]
        [MemberData(nameof(SetValue_MemberData))]
        public void SetValue(Type commandType)
        {
            using var command = (IDbCommand)Activator.CreateInstance(commandType)!;

            var parameter = command.CreateParameter();

            parameter.SetValue(1);

            Assert.Equal(1, parameter.Value);

            parameter.SetValue(null);

            Assert.Equal(DBNull.Value, parameter.Value);
        }

        /// <summary>  
        /// Tests the SetBooleanValue extension method for IDbDataParameter.  
        /// </summary>  
        /// <param name="commandType">The type of the command to create.</param>  
        [Theory]
        [MemberData(nameof(SetValue_MemberData))]
        public void SetBooleanValue(Type commandType)
        {
            using var command = (IDbCommand)Activator.CreateInstance(commandType)!;

            var parameter = command.CreateParameter();

            bool value = true;

            parameter.SetBooleanValue(value);

            Assert.Equal(value, parameter.Value);

            parameter.SetBooleanValue(null);

            Assert.Equal(DBNull.Value, parameter.Value);
        }

        /// <summary>  
        /// Tests the SetByteValue extension method for IDbDataParameter.  
        /// </summary>  
        /// <param name="commandType">The type of the command to create.</param>  
        [Theory]
        [MemberData(nameof(SetValue_MemberData))]
        public void SetByteValue(Type commandType)
        {
            using var command = (IDbCommand)Activator.CreateInstance(commandType)!;

            var parameter = command.CreateParameter();

            byte value = 1;

            parameter.SetByteValue(value);

            Assert.Equal(value, parameter.Value);

            parameter.SetByteValue(null);

            Assert.Equal(DBNull.Value, parameter.Value);
        }

        /// <summary>  
        /// Tests the SetSByteValue extension method for IDbDataParameter.  
        /// </summary>  
        /// <param name="commandType">The type of the command to create.</param>  
        /// <remarks>
        /// <list type="bullet">
        /// <item>
        /// For OracleParameter, setting the value of signed byte will throw an exception.
        /// </item>
        /// </remarks>
        [Theory]
        [MemberData(nameof(SetValue_MemberData))]
        public void SetSByteValue(Type commandType)
        {
            using var command = (IDbCommand)Activator.CreateInstance(commandType)!;

            var parameter = command.CreateParameter();

            sbyte value = 1;

            try
            {
                parameter.SetSByteValue(value);

                Assert.Equal(value, parameter.Value);
            }
            catch (ArgumentException ex) when (commandType == typeof(OracleCommand))
            {
                m_Output.WriteLine(ex.ToString());
            }

            parameter.SetSByteValue(null);

            Assert.Equal(DBNull.Value, parameter.Value);
        }

        /// <summary>  
        /// Tests the SetInt16Value extension method for IDbDataParameter.  
        /// </summary>  
        /// <param name="commandType">The type of the command to create.</param>  
        [Theory]
        [MemberData(nameof(SetValue_MemberData))]
        public void SetInt16Value(Type commandType)
        {
            using var command = (IDbCommand)Activator.CreateInstance(commandType)!;

            var parameter = command.CreateParameter();

            short value = 1;

            parameter.SetInt16Value(value);

            Assert.Equal(value, parameter.Value);

            parameter.SetInt16Value(null);

            Assert.Equal(DBNull.Value, parameter.Value);
        }

        /// <summary>  
        /// Tests the SetUInt16Value extension method for IDbDataParameter.  
        /// </summary>  
        /// <param name="commandType">The type of the command to create.</param>  
        /// <remarks>
        /// <list type="bullet">
        /// <item>
        /// For OracleParameter, setting the value of unsigned integer will throw an exception.
        /// </item>
        /// </remarks>
        [Theory]
        [MemberData(nameof(SetValue_MemberData))]
        public void SetUInt16Value(Type commandType)
        {
            using var command = (IDbCommand)Activator.CreateInstance(commandType)!;

            var parameter = command.CreateParameter();

            ushort value = 1;

            try
            {
                parameter.SetUInt16Value(value);

                Assert.Equal(value, parameter.Value);
            }
            catch (ArgumentException ex) when (commandType == typeof(OracleCommand))
            {
                m_Output.WriteLine(ex.ToString());
            }

            parameter.SetUInt16Value(null);

            Assert.Equal(DBNull.Value, parameter.Value);
        }

        /// <summary>  
        /// Tests the SetInt32Value extension method for IDbDataParameter.  
        /// </summary>  
        /// <param name="commandType">The type of the command to create.</param>  
        [Theory]
        [MemberData(nameof(SetValue_MemberData))]
        public void SetInt32Value(Type commandType)
        {
            using var command = (IDbCommand)Activator.CreateInstance(commandType)!;

            var parameter = command.CreateParameter();

            int value = 1;

            parameter.SetInt32Value(value);

            Assert.Equal(value, parameter.Value);

            parameter.SetInt32Value(null);

            Assert.Equal(DBNull.Value, parameter.Value);
        }

        /// <summary>  
        /// Tests the SetUInt32Value extension method for IDbDataParameter.  
        /// </summary>  
        /// <param name="commandType">The type of the command to create.</param>  
        /// <remarks>
        /// <list type="bullet">
        /// <item>
        /// For OracleParameter, setting the value of unsigned integer will throw an exception.
        /// </item>
        /// </remarks>
        [Theory]
        [MemberData(nameof(SetValue_MemberData))]
        public void SetUInt32Value(Type commandType)
        {
            using var command = (IDbCommand)Activator.CreateInstance(commandType)!;

            var parameter = command.CreateParameter();

            uint value = 1;

            try
            {
                parameter.SetUInt32Value(value);

                Assert.Equal(value, parameter.Value);
            }
            catch (ArgumentException ex) when (commandType == typeof(OracleCommand))
            {
                m_Output.WriteLine(ex.ToString());
            }

            parameter.SetUInt32Value(null);

            Assert.Equal(DBNull.Value, parameter.Value);
        }

        /// <summary>  
        /// Tests the SetInt64Value extension method for IDbDataParameter.  
        /// </summary>  
        /// <param name="commandType">The type of the command to create.</param>  
        [Theory]
        [MemberData(nameof(SetValue_MemberData))]
        public void SetInt64Value(Type commandType)
        {
            using var command = (IDbCommand)Activator.CreateInstance(commandType)!;

            var parameter = command.CreateParameter();

            long value = 1;

            parameter.SetInt64Value(value);

            Assert.Equal(value, parameter.Value);

            parameter.SetInt64Value(null);

            Assert.Equal(DBNull.Value, parameter.Value);
        }

        /// <summary>  
        /// Tests the SetUInt64Value extension method for IDbDataParameter.  
        /// </summary>  
        /// <param name="commandType">The type of the command to create.</param>  
        /// <remarks>
        /// <list type="bullet">
        /// <item>
        /// For OracleParameter, setting the value of unsigned integer will throw an exception.
        /// </item>
        /// </remarks>
        [Theory]
        [MemberData(nameof(SetValue_MemberData))]
        public void SetUInt64Value(Type commandType)
        {
            using var command = (IDbCommand)Activator.CreateInstance(commandType)!;

            var parameter = command.CreateParameter();

            ulong value = 1;

            try
            {
                parameter.SetUInt64Value(value);

                Assert.Equal(value, parameter.Value);
            }
            catch (ArgumentException ex) when (commandType == typeof(OracleCommand))
            {
                m_Output.WriteLine(ex.ToString());
            }

            parameter.SetUInt64Value(null);

            Assert.Equal(DBNull.Value, parameter.Value);
        }

        /// <summary>  
        /// Tests the SetSingleValue extension method for IDbDataParameter.  
        /// </summary>  
        /// <param name="commandType">The type of the command to create.</param>  
        [Theory]
        [MemberData(nameof(SetValue_MemberData))]
        public void SetSingleValue(Type commandType)
        {
            using var command = (IDbCommand)Activator.CreateInstance(commandType)!;

            var parameter = command.CreateParameter();

            float value = 1;

            parameter.SetSingleValue(value);

            Assert.Equal(value, parameter.Value);

            parameter.SetSingleValue(null);

            Assert.Equal(DBNull.Value, parameter.Value);
        }

        /// <summary>  
        /// Tests the SetDoubleValue extension method for IDbDataParameter.  
        /// </summary>  
        /// <param name="commandType">The type of the command to create.</param>  
        [Theory]
        [MemberData(nameof(SetValue_MemberData))]
        public void SetDoubleValue(Type commandType)
        {
            using var command = (IDbCommand)Activator.CreateInstance(commandType)!;

            var parameter = command.CreateParameter();

            double value = 1;

            parameter.SetDoubleValue(value);

            Assert.Equal(value, parameter.Value);

            parameter.SetDoubleValue(null);

            Assert.Equal(DBNull.Value, parameter.Value);
        }

        /// <summary>  
        /// Tests the SetDecimalValue extension method for IDbDataParameter.  
        /// </summary>  
        /// <param name="commandType">The type of the command to create.</param>  
        [Theory]
        [MemberData(nameof(SetValue_MemberData))]
        public void SetDecimalValue(Type commandType)
        {
            using var command = (IDbCommand)Activator.CreateInstance(commandType)!;

            var parameter = command.CreateParameter();

            decimal value = 1;

            parameter.SetDecimalValue(value);

            Assert.Equal(value, parameter.Value);

            parameter.SetDecimalValue(null);

            Assert.Equal(DBNull.Value, parameter.Value);
        }

        /// <summary>
        /// Tests the SetDateTimeValue extension method for IDbDataParameter.  
        /// </summary>  
        /// <param name="commandType">The type of the command to create.</param>  
        [Theory]
        [MemberData(nameof(SetValue_MemberData))]
        public void SetDateTimeValue(Type commandType)
        {
            using var command = (IDbCommand)Activator.CreateInstance(commandType)!;

            var parameter = command.CreateParameter();

            DateTime value = DateTime.Now;

            parameter.SetDateTimeValue(value);

            Assert.Equal(value, parameter.Value);

            parameter.SetDateTimeValue(null);

            Assert.Equal(DBNull.Value, parameter.Value);
        }

        /// <summary>
        /// Tests the SetDateTimeOffsetValue extension method for IDbDataParameter.  
        /// </summary>  
        /// <param name="commandType">The type of the command to create.</param>  
        [Theory]
        [MemberData(nameof(SetValue_MemberData))]
        public void SetDateTimeOffsetValue(Type commandType)
        {
            using var command = (IDbCommand)Activator.CreateInstance(commandType)!;

            var parameter = command.CreateParameter();

            DateTimeOffset value = DateTimeOffset.UtcNow;

            parameter.SetDateTimeOffsetValue(value);

            Assert.Equal(value, parameter.Value);

            parameter.SetDateTimeOffsetValue(null);

            Assert.Equal(DBNull.Value, parameter.Value);
        }

        /// <summary>
        /// Tests the SetTimeValue extension method for IDbDataParameter.  
        /// </summary>  
        /// <param name="commandType">The type of the command to create.</param>  
        [Theory]
        [MemberData(nameof(SetValue_MemberData))]
        public void SetTimeValue(Type commandType)
        {
            using var command = (IDbCommand)Activator.CreateInstance(commandType)!;

            var parameter = command.CreateParameter();

            TimeSpan value = DateTime.Now.TimeOfDay;

            parameter.SetTimeValue(value);

            Assert.Equal(value, parameter.Value);

            parameter.SetTimeValue(null);

            Assert.Equal(DBNull.Value, parameter.Value);
        }

        /// <summary>
        /// Tests the SetStringValue extension method for IDbDataParameter.  
        /// </summary>  
        /// <param name="commandType">The type of the command to create.</param>  
        [Theory]
        [MemberData(nameof(SetValue_MemberData))]
        public void SetStringValue(Type commandType)
        {
            using var command = (IDbCommand)Activator.CreateInstance(commandType)!;

            var parameter = command.CreateParameter();

            string value = "abc";

            parameter.SetStringValue(value);

            Assert.Equal(value, parameter.Value);

            parameter.SetStringValue(null);

            Assert.Equal(DBNull.Value, parameter.Value);
        }

        /// <summary>
        /// Tests the SetCharValue extension method for IDbDataParameter.  
        /// </summary>  
        /// <param name="commandType">The type of the command to create.</param>  
        [Theory]
        [MemberData(nameof(SetValue_MemberData))]
        public void SetCharValue(Type commandType)
        {
            using var command = (IDbCommand)Activator.CreateInstance(commandType)!;

            var parameter = command.CreateParameter();

            char value = 'a';

            parameter.SetCharValue(value);

            Assert.Equal(value, parameter.Value);

            parameter.SetCharValue(null);

            Assert.Equal(DBNull.Value, parameter.Value);
        }

        /// <summary>
        /// Tests the SetGuidValue extension method for IDbDataParameter.  
        /// </summary>  
        /// <param name="commandType">The type of the command to create.</param>  
        /// <remarks>
        /// <list type="bullet">
        /// <item>
        /// For OracleParameter, setting the value of Guid will throw an exception.
        /// </item>
        /// </remarks>
        [Theory]
        [MemberData(nameof(SetValue_MemberData))]
        public void SetGuidValue(Type commandType)
        {
            using var command = (IDbCommand)Activator.CreateInstance(commandType)!;
            
            var parameter = command.CreateParameter();

            Guid value = Guid.NewGuid();

            try
            {
                parameter.SetGuidValue(value);

                Assert.Equal(value, parameter.Value);
            }
            catch (ArgumentException ex) when (commandType == typeof(OracleCommand))
            {
                m_Output.WriteLine(ex.ToString());
            }

            parameter.SetGuidValue(null);

            Assert.Equal(DBNull.Value, parameter.Value);
        }

        /// <summary>
        /// Tests the SetBinaryValue extension method for IDbDataParameter.  
        /// </summary>  
        /// <param name="commandType">The type of the command to create.</param>  
        [Theory]
        [MemberData(nameof(SetValue_MemberData))]
        public void SetBinaryValue(Type commandType)
        {
            using var command = (IDbCommand)Activator.CreateInstance(commandType)!;

            var parameter = command.CreateParameter();

            byte[] value = [1, 2, 3];

            parameter.SetBinaryValue(value);

            Assert.Equal(value, parameter.Value);

            parameter.SetBinaryValue(null);

            Assert.Equal(DBNull.Value, parameter.Value);
        }

        public static IEnumerable<object[]> SetValue_MemberData()
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
    }
}
