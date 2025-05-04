using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Data.SqlClient;
using mxProject.Data;
using mxProject.Data.Extensions;
using Xunit.Abstractions;

namespace Test.Data.AdoNet
{
    /// <summary>  
    /// Test class for IDataReader extensions.  
    /// </summary>  
    public class TestDataReaderExtensions
    {
        /// <summary>
        /// Creates a new instance.
        /// </summary>
        /// <param name="output"></param>
        public TestDataReaderExtensions(ITestOutputHelper output)
        {
            m_Output = output;
        }

        private readonly ITestOutputHelper m_Output;

        [Fact]
        public void Cast()
        {
            using var connection = SampleDatabase.CreateConnection();

            connection.Open();

            using (var command = connection.CreateCommand())
            {
                command.CommandText = "select 1 as ID, 'abc' as NAME";

                using var reader = command.ExecuteReader();

                reader.Cast<SqlDataReader>(x =>
                {
                    x.Read();
                    Assert.Equal(1, x.GetInt32(0));
                    Assert.Equal("abc", x.GetString(1));
                });
            }

            var wrapper = connection.WithFilter();

            using (var command = wrapper.CreateCommand())
            {
                command.CommandText = "select 1 as ID, 'abc' as NAME";

                using var reader = command.ExecuteReader();

                reader.Cast<SqlDataReader>(x =>
                {
                    var idType = x.GetProviderSpecificFieldType(0);
                });
            }
        }
    }
}
