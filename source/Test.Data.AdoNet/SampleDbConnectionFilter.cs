using System;
using System.Collections.Generic;
using System.Linq;
using System.Data;
using System.Threading.Tasks;
using mxProject.Data;
using mxProject.Data.Filters;
using Xunit.Abstractions;

namespace Test.Data.AdoNet
{
    internal class SampleDbConnectionFilter : DbConnectionFilterBase
    {
        internal SampleDbConnectionFilter(string name, DbConnectionFilterTargets targets, ITestOutputHelper outputHelper)
        {
            m_Name = name;
            m_Targets = targets;
            m_OutputHelper = outputHelper;
        }

        private readonly string m_Name;
        private readonly DbConnectionFilterTargets m_Targets;
        private readonly ITestOutputHelper m_OutputHelper;

        private void OutputEnter(string methodName)
        {
            m_OutputHelper.WriteLine($"[{m_Name}] IDbConnection.{methodName} enter");
        }

        private void OutputExit(string methodName)
        {
            m_OutputHelper.WriteLine($"[{m_Name}] IDbConnection.{methodName} exit");
        }

        public override DbConnectionFilterTargets TargetMethods => m_Targets;

        public override IDbTransaction BeginTransaction(IDbConnection connection, IsolationLevel isolationLevel, Func<IDbConnection, IsolationLevel, IDbTransaction> continuation)
        {
            try
            {
                OutputEnter(nameof(BeginTransaction));

                return base.BeginTransaction(connection, isolationLevel, continuation);
            }
            finally
            {
                OutputExit(nameof(BeginTransaction));
            }
        }

        public override void ChangeDatabase(IDbConnection connection, string databaseName, Action<IDbConnection, string> continuation)
        {
            try
            {
                OutputEnter(nameof(ChangeDatabase));
                base.ChangeDatabase(connection, databaseName, continuation);
            }
            finally
            {
                OutputExit(nameof(ChangeDatabase));
            }
        }

        public override void Close(IDbConnection connection, Action<IDbConnection> continuation)
        {
            try
            {
                OutputEnter(nameof(Close));
                base.Close(connection, continuation);
            }
            finally
            {
                OutputExit(nameof(Close));
            }
        }

        public override IDbCommand CreateCommand(IDbConnection connection, Func<IDbConnection, IDbCommand> continuation)
        {
            try
            {
                OutputEnter(nameof(CreateCommand));
                return base.CreateCommand(connection, continuation);
            }
            finally
            {
                OutputExit(nameof(CreateCommand));
            }
        }

        public override void Dispose(IDbConnection connection, Action<IDbConnection> continuation)
        {
            try
            {
                OutputEnter(nameof(Dispose));
                base.Dispose(connection, continuation);
            }
            finally
            {
                OutputExit(nameof(Dispose));
            }
        }

        public override void Open(IDbConnection connection, Action<IDbConnection> continuation)
        {
            try
            {
                OutputEnter(nameof(Open));
                base.Open(connection, continuation);
            }
            finally
            {
                OutputExit(nameof(Open));
            }
        }
    }
}
