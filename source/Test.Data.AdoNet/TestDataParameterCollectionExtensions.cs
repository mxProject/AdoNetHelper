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
    /// Test class for IDataParameterCollection extensions.  
    /// </summary>  
    public class TestDataParameterCollectionExtensions
    {
        /// <summary>
        /// Creates a new instance.
        /// </summary>
        /// <param name="output"></param>
        public TestDataParameterCollectionExtensions(ITestOutputHelper output)
        {
            m_Output = output;
        }

        private readonly ITestOutputHelper m_Output;

        [Fact]
        public void Cast()
        {
            using var connection = SampleDatabase.CreateConnection();

            using (var command = connection.CreateCommand())
            {
                command.Parameters.Cast<SqlParameterCollection>(x =>
                {
                    x.AddWithValue("id", 1);
                });

                Assert.Single(command.Parameters);   
            }

            var wrapper = connection.WithFilter();

            using (var command = wrapper.CreateCommand())
            {
                command.Parameters.Cast<SqlParameterCollection>(x =>
                {
                    x.AddWithValue("id", 1);
                });

                Assert.Single(command.Parameters);
            }
        }
    }
}
