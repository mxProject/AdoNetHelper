using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Data.SqlClient;

namespace Test.Data.AdoNet
{
    internal class SampleDatabase
    {
        internal static IDbConnection CreateConnection()
        {
            return new SqlConnection("Data Source=(localdb)\\SampleDB; Initial Catalog=TestDataAdoNet; Integrated Security=True");
        }
    }
}
