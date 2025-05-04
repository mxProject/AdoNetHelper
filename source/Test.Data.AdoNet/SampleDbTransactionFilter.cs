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
    internal class SampleDbTransactionFilter : DbTransactionFilterBase
    {
        internal SampleDbTransactionFilter(string name, DbTransactionFilterTargets targets, ITestOutputHelper outputHelper)
        {
            m_Name = name;
            m_Targets = targets;
            m_OutputHelper = outputHelper;
        }

        private readonly string m_Name;
        private readonly DbTransactionFilterTargets m_Targets;
        private readonly ITestOutputHelper m_OutputHelper;

        private void OutputEnter(string methodName)
        {
            m_OutputHelper.WriteLine($"[{m_Name}] IDbTransaction.{methodName} enter");
        }

        private void OutputExit(string methodName)
        {
            m_OutputHelper.WriteLine($"[{m_Name}] IDbTransaction.{methodName} exit");
        }

        public override DbTransactionFilterTargets TargetMethods => m_Targets;

        public override void Commit(IDbTransaction transaction, Action<IDbTransaction> continuation)
        {
            try
            {
                OutputEnter(nameof(Commit));
                base.Commit(transaction, continuation);
            }
            finally
            {
                OutputExit(nameof(Commit));
            }
        }

        public override void Dispose(IDbTransaction transaction, Action<IDbTransaction> continuation)
        {
            try
            {
                OutputEnter(nameof(Dispose));
                base.Dispose(transaction, continuation);
            }
            finally
            {
                OutputExit(nameof(Dispose));
            }
        }

        public override void Rollback(IDbTransaction transaction, Action<IDbTransaction> continuation)
        {
            try
            {
                OutputEnter(nameof(Rollback));
                base.Rollback(transaction, continuation);
            }
            finally
            {
                OutputExit(nameof(Rollback));
            }
        }
    }
}
