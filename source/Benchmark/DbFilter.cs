using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Data.SqlClient; 
using BenchmarkDotNet.Attributes;
using mxProject.Data;
using mxProject.Data.Filters;

namespace ConsoleApp1
{
    [MemoryDiagnoser]
    [ShortRunJob]
    [MinColumn, MaxColumn]
    public class DbFilter
    {
        internal static IDbConnection CreateConnection()
        {
            return new SqlConnection("Data Source=(localdb)\\SampleDB; Initial Catalog=TestDataAdoNet; Integrated Security=True");
        }

        internal static IEnumerable<IDbConnectionFilter> CreateFilters(int count)
        {
            for (int i = 0; i < count; i++)
            {
                yield return new DbConnectionFilter($"filter{i}");
            }
        }

        private class DbConnectionFilter : DbConnectionFilterBase
        {
            internal DbConnectionFilter(string name)
            {
                m_Name = name;
            }

            private readonly string m_Name;

            public override DbConnectionFilterTargets TargetMethods => DbConnectionFilterTargets.All;
        }

        [Benchmark]
        public void SqlConnection()
        {
            using var connection = CreateConnection();

            connection.Open();
            connection.Close();
            connection.Dispose();   
        }

        [Benchmark]
        public void WithZeroFilter()
        {
            using var connection = CreateConnection().WithFilter(CreateFilters(0));

            connection.Open();
            connection.Close();
            connection.Dispose();
        }

        [Benchmark]
        public void WithOneFilter()
        {
            using var connection = CreateConnection().WithFilter(CreateFilters(1));

            connection.Open();
            connection.Close();
            connection.Dispose();
        }

        [Benchmark]
        public void WithTwoFilter()
        {
            using var connection = CreateConnection().WithFilter(CreateFilters(2));

            connection.Open();
            connection.Close();
            connection.Dispose();
        }

        [Benchmark]
        public void WithThreeFilter()
        {
            using var connection = CreateConnection().WithFilter(CreateFilters(3));

            connection.Open();
            connection.Close();
            connection.Dispose();
        }
    }
}
