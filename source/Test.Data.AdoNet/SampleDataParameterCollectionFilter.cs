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
    internal class SampleDataParameterCollectionFilter : DataParameterCollectionFilterBase
    {
        internal SampleDataParameterCollectionFilter(string name, DataParameterCollectionFilterTargets targets, ITestOutputHelper outputHelper)
        {
            m_Name = name;
            m_Targets = targets;
            m_OutputHelper = outputHelper;
        }

        private readonly string m_Name;
        private readonly DataParameterCollectionFilterTargets m_Targets;
        private readonly ITestOutputHelper m_OutputHelper;

        private void OutputEnter(string methodName)
        {
            m_OutputHelper.WriteLine($"[{m_Name}] IDataParameterCollection.{methodName} enter");
        }

        private void OutputExit(string methodName)
        {
            m_OutputHelper.WriteLine($"[{m_Name}] IDataParameterCollection.{methodName} exit");
        }

        public override DataParameterCollectionFilterTargets TargetMethods => m_Targets;

        public override int Add(IDataParameterCollection parameters, object parameter, Func<IDataParameterCollection, object, int> continuation)
        {
            try
            {
                OutputEnter(nameof(Add));
                return base.Add(parameters, parameter, continuation);
            }
            finally
            {
                OutputExit(nameof(Add));
            }
        }

        public override void Clear(IDataParameterCollection parameters, Action<IDataParameterCollection> continuation)
        {
            try
            {
                OutputEnter(nameof(Clear));
                base.Clear(parameters, continuation);
            }
            finally
            {
                OutputExit(nameof(Clear));
            }
        }

        public override void Insert(IDataParameterCollection parameters, int index, object parameter, Action<IDataParameterCollection, int, object> continuation)
        {
            try
            {
                OutputEnter(nameof(Insert));
                base.Insert(parameters, index, parameter, continuation);
            }
            finally
            {
                OutputExit(nameof(Insert));
            }
        }

        public override void Remove(IDataParameterCollection parameters, object parameter, Action<IDataParameterCollection, object> continuation)
        {
            try
            {
                OutputEnter(nameof(Remove));
                base.Remove(parameters, parameter, continuation);
            }
            finally
            {
                OutputExit(nameof(Remove));
            }
        }

        public override void RemoveAt(IDataParameterCollection parameters, int index, Action<IDataParameterCollection, int> continuation)
        {
            try
            {
                OutputEnter(nameof(RemoveAt));
                base.RemoveAt(parameters, index, continuation);
            }
            finally
            {
                OutputExit(nameof(RemoveAt));
            }
        }

        public override void RemoveAt(IDataParameterCollection parameters, string parameterName, Action<IDataParameterCollection, string> continuation)
        {
            try
            {
                OutputEnter(nameof(RemoveAt));
                base.RemoveAt(parameters, parameterName, continuation);
            }
            finally
            {
                OutputExit(nameof(RemoveAt));
            }
        }
    }
}
