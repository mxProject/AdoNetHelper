using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using mxProject.Data;

namespace Test.Data.AdoNet
{
    internal class SampleDbContext : IDbContext
    {
        internal SampleDbContext()
        {
            m_Connection = null;
            m_Transaction = null;
        }

        internal SampleDbContext(IDbConnection connection)
        {
            m_Connection = connection;
            m_Transaction = null;
        }

        internal SampleDbContext(IDbTransaction transaction)
        {
            m_Connection = transaction?.Connection;
            m_Transaction = transaction;
        }

        private readonly IDbConnection? m_Connection;
        private readonly IDbTransaction? m_Transaction;

        public IDbConnection GetCurrentConnection()
        {
            return m_Connection!;
        }

        public IDbTransaction? GetCurrentTransaction()
        {
            if (m_Transaction?.Connection == null) { return null; }
            return m_Transaction;
        }
    }
}
