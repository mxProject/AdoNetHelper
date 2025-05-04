using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using mxProject.Data;
using mxProject.Data.Filters;
using mxProject.Data.Extensions;
using Xunit.Abstractions;

namespace Test.Data.AdoNet
{
    internal class SampleDbCommandFilter : DbCommandFilterBase
    {
        internal SampleDbCommandFilter(string name, DbCommandFilterTargets targets, ITestOutputHelper outputHelper)
        {
            m_Name = name;
            m_Targets = targets;
            m_OutputHelper = outputHelper;
        }

        private readonly string m_Name;
        private readonly DbCommandFilterTargets m_Targets;
        private readonly ITestOutputHelper m_OutputHelper;

        private void OutputEnter(string methodName)
        {
            m_OutputHelper.WriteLine($"[{m_Name}] IDbCommand.{methodName} enter");
        }

        private void OutputExit(string methodName)
        {
            m_OutputHelper.WriteLine($"[{m_Name}] IDbCommand.{methodName} exit");
        }

        public override DbCommandFilterTargets TargetMethods => m_Targets;

        public override void Cancel(IDbCommand command, Action<IDbCommand> continuation)
        {
            try
            {
                OutputEnter(nameof(Cancel));
                base.Cancel(command, continuation);
            }
            finally
            {
                OutputExit(nameof(Cancel));
            }
        }

        public override IDbDataParameter CreateParameter(IDbCommand command, Func<IDbCommand, IDbDataParameter> continuation)
        {
            try
            {
                OutputEnter(nameof(CreateParameter));
                return base.CreateParameter(command, continuation);
            }
            finally
            {
                OutputExit(nameof(CreateParameter));
            }
        }

        public override void Dispose(IDbCommand command, Action<IDbCommand> continuation)
        {
            try
            {
                OutputEnter(nameof(Dispose));
                base.Dispose(command, continuation);
            }
            finally
            {
                OutputExit(nameof(Dispose));
            }
        }

        public override int ExecuteNonQuery(IDbCommand command, Func<IDbCommand, int> continuation)
        {
            try
            {
                OutputEnter(nameof(ExecuteNonQuery));
                return base.ExecuteNonQuery(command, continuation);
            }
            finally
            {
                OutputExit(nameof(ExecuteNonQuery));
            }
        }

        public override IDataReader ExecuteReader(IDbCommand command, CommandBehavior behavior, Func<IDbCommand, CommandBehavior, IDataReader> continuation)
        {
            try
            {
                OutputEnter(nameof(ExecuteReader));
                return base.ExecuteReader(command, behavior, continuation);
            }
            finally
            {
                OutputExit(nameof(ExecuteReader));
            }
        }

        public override object ExecuteScalar(IDbCommand command, Func<IDbCommand, object> continuation)
        {
            try
            {
                OutputEnter(nameof(ExecuteScalar));
                return base.ExecuteScalar(command, continuation);
            }
            finally
            {
                OutputExit(nameof(ExecuteScalar));
            }
        }

        public override void Prepare(IDbCommand command, Action<IDbCommand> continuation)
        {
            try
            {
                OutputEnter(nameof(Prepare));
                base.Prepare(command, continuation);
            }
            finally
            {
                OutputExit(nameof(Prepare));
            }
        }
    }
}
