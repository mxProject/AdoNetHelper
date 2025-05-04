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
    internal class SampleDataReaderFilter : DataReaderFilterBase
    {
        internal SampleDataReaderFilter(string name, DataReaderFilterTargets targets, ITestOutputHelper outputHelper)
        {
            m_Name = name;
            m_Targets = targets;
            m_OutputHelper = outputHelper;
        }

        private readonly string m_Name;
        private readonly DataReaderFilterTargets m_Targets;
        private readonly ITestOutputHelper m_OutputHelper;

        private void OutputEnter(string methodName)
        {
            m_OutputHelper.WriteLine($"[{m_Name}] IDataReader.{methodName} enter");
        }

        private void OutputExit(string methodName)
        {
            m_OutputHelper.WriteLine($"[{m_Name}] IDataReader.{methodName} exit");
        }

        public override DataReaderFilterTargets TargetMethods => m_Targets;

        /// <inheritdoc/>  
        public override void Close(IDataReader reader, Action<IDataReader> continuation)
        {
            try
            {
                OutputEnter(nameof(Close));
                base.Close(reader, continuation);
            }
            finally
            {
                OutputExit(nameof(Close));
            }
        }

        /// <inheritdoc/>  
        public override void Dispose(IDataReader reader, Action<IDataReader> continuation)
        {
            try
            {
                OutputEnter(nameof(Dispose));
                base.Dispose(reader, continuation);
            }
            finally
            {
                OutputExit(nameof(Dispose));
            }
        }

        /// <inheritdoc/>  
        public override bool GetBoolean(IDataReader reader, int i, Func<IDataReader, int, bool> continuation)
        {
            try
            {
                OutputEnter(nameof(GetBoolean));
                return base.GetBoolean(reader, i, continuation);
            }
            finally
            {
                OutputExit(nameof(GetBoolean));
            }
        }

        /// <inheritdoc/>  
        public override byte GetByte(IDataReader reader, int i, Func<IDataReader, int, byte> continuation)
        {
            try
            {
                OutputEnter(nameof(GetByte));
                return base.GetByte(reader, i, continuation);
            }
            finally
            {
                OutputExit(nameof(GetByte));
            }
        }

        /// <inheritdoc/>  
        public override long GetBytes(IDataReader reader, int i, long fieldOffset, byte[] buffer, int bufferoffset, int length, Func<IDataReader, int, long, byte[], int, int, long> continuation)
        {
            try
            {
                OutputEnter(nameof(GetBytes));
                return base.GetBytes(reader, i, fieldOffset, buffer, bufferoffset, length, continuation);
            }
            finally
            {
                OutputExit(nameof(GetBytes));
            }
        }

        /// <inheritdoc/>  
        public override char GetChar(IDataReader reader, int i, Func<IDataReader, int, char> continuation)
        {
            try
            {
                OutputEnter(nameof(GetChar));
                return base.GetChar(reader, i, continuation);
            }
            finally
            {
                OutputExit(nameof(GetChar));
            }
        }

        /// <inheritdoc/>  
        public override long GetChars(IDataReader reader, int i, long fieldoffset, char[] buffer, int bufferoffset, int length, Func<IDataReader, int, long, char[], int, int, long> continuation)
        {
            try
            {
                OutputEnter(nameof(GetChars));
                return base.GetChars(reader, i, fieldoffset, buffer, bufferoffset, length, continuation);
            }
            finally
            {
                OutputExit(nameof(GetChars));
            }
        }

        /// <inheritdoc/>  
        public override IDataReader GetData(IDataReader reader, int i, Func<IDataReader, int, IDataReader> continuation)
        {
            try
            {
                OutputEnter(nameof(GetData));
                return base.GetData(reader, i, continuation);
            }
            finally
            {
                OutputExit(nameof(GetData));
            }
        }

        /// <inheritdoc/>  
        public override string GetDataTypeName(IDataReader reader, int i, Func<IDataReader, int, string> continuation)
        {
            try
            {
                OutputEnter(nameof(GetDataTypeName));
                return base.GetDataTypeName(reader, i, continuation);
            }
            finally
            {
                OutputExit(nameof(GetDataTypeName));
            }
        }

        /// <inheritdoc/>  
        public override DateTime GetDateTime(IDataReader reader, int i, Func<IDataReader, int, DateTime> continuation)
        {
            try
            {
                OutputEnter(nameof(GetDateTime));
                return base.GetDateTime(reader, i, continuation);
            }
            finally
            {
                OutputExit(nameof(GetDateTime));
            }
        }

        /// <inheritdoc/>  
        public override decimal GetDecimal(IDataReader reader, int i, Func<IDataReader, int, decimal> continuation)
        {
            try
            {
                OutputEnter(nameof(GetDecimal));
                return base.GetDecimal(reader, i, continuation);
            }
            finally
            {
                OutputExit(nameof(GetDecimal));
            }
        }

        /// <inheritdoc/>  
        public override double GetDouble(IDataReader reader, int i, Func<IDataReader, int, double> continuation)
        {
            try
            {
                OutputEnter(nameof(GetDouble));
                return base.GetDouble(reader, i, continuation);
            }
            finally
            {
                OutputExit(nameof(GetDouble));
            }
        }

        /// <inheritdoc/>  
        public override Type GetFieldType(IDataReader reader, int i, Func<IDataReader, int, Type> continuation)
        {
            try
            {
                OutputEnter(nameof(GetFieldType));
                return base.GetFieldType(reader, i, continuation);
            }
            finally
            {
                OutputExit(nameof(GetFieldType));
            }
        }

        /// <inheritdoc/>  
        public override float GetFloat(IDataReader reader, int i, Func<IDataReader, int, float> continuation)
        {
            try
            {
                OutputEnter(nameof(GetFloat));
                return base.GetFloat(reader, i, continuation);
            }
            finally
            {
                OutputExit(nameof(GetFloat));
            }
        }

        /// <inheritdoc/>  
        public override Guid GetGuid(IDataReader reader, int i, Func<IDataReader, int, Guid> continuation)
        {
            try
            {
                OutputEnter(nameof(GetGuid));
                return base.GetGuid(reader, i, continuation);
            }
            finally
            {
                OutputExit(nameof(GetGuid));
            }
        }

        /// <inheritdoc/>  
        public override short GetInt16(IDataReader reader, int i, Func<IDataReader, int, short> continuation)
        {
            try
            {
                OutputEnter(nameof(GetInt16));
                return base.GetInt16(reader, i, continuation);
            }
            finally
            {
                OutputExit(nameof(GetInt16));
            }
        }

        /// <inheritdoc/>  
        public override int GetInt32(IDataReader reader, int i, Func<IDataReader, int, int> continuation)
        {
            try
            {
                OutputEnter(nameof(GetInt32));
                return base.GetInt32(reader, i, continuation);
            }
            finally
            {
                OutputExit(nameof(GetInt32));
            }
        }

        /// <inheritdoc/>  
        public override long GetInt64(IDataReader reader, int i, Func<IDataReader, int, long> continuation)
        {
            try
            {
                OutputEnter(nameof(GetInt64));
                return base.GetInt64(reader, i, continuation);
            }
            finally
            {
                OutputExit(nameof(GetInt64));
            }
        }

        /// <inheritdoc/>  
        public override string GetName(IDataReader reader, int i, Func<IDataReader, int, string> continuation)
        {
            try
            {
                OutputEnter(nameof(GetName));
                return base.GetName(reader, i, continuation);
            }
            finally
            {
                OutputExit(nameof(GetName));
            }
        }

        /// <inheritdoc/>  
        public override int GetOrdinal(IDataReader reader, string name, Func<IDataReader, string, int> continuation)
        {
            try
            {
                OutputEnter(nameof(GetOrdinal));
                return base.GetOrdinal(reader, name, continuation);
            }
            finally
            {
                OutputExit(nameof(GetOrdinal));
            }
        }

        /// <inheritdoc/>  
        public override DataTable GetSchemaTable(IDataReader reader, Func<IDataReader, DataTable> continuation)
        {
            try
            {
                OutputEnter(nameof(GetSchemaTable));
                return base.GetSchemaTable(reader, continuation);
            }
            finally
            {
                OutputExit(nameof(GetSchemaTable));
            }
        }

        /// <inheritdoc/>  
        public override string GetString(IDataReader reader, int i, Func<IDataReader, int, string> continuation)
        {
            try
            {
                OutputEnter(nameof(GetString));
                return base.GetString(reader, i, continuation);
            }
            finally
            {
                OutputExit(nameof(GetString));
            }
        }

        /// <inheritdoc/>  
        public override object GetValue(IDataReader reader, int i, Func<IDataReader, int, object> continuation)
        {
            try
            {
                OutputEnter(nameof(GetValue));
                return base.GetValue(reader, i, continuation);
            }
            finally
            {
                OutputExit(nameof(GetValue));
            }
        }

        /// <inheritdoc/>  
        public override int GetValues(IDataReader reader, object[] values, Func<IDataReader, object[], int> continuation)
        {
            try
            {
                OutputEnter(nameof(GetValues));
                return base.GetValues(reader, values, continuation);
            }
            finally
            {
                OutputExit(nameof(GetValues));
            }
        }

        /// <inheritdoc/>  
        public override bool IsDBNull(IDataReader reader, int i, Func<IDataReader, int, bool> continuation)
        {
            try
            {
                OutputEnter(nameof(IsDBNull));
                return base.IsDBNull(reader, i, continuation);
            }
            finally
            {
                OutputExit(nameof(IsDBNull));
            }
        }

        /// <inheritdoc/>  
        public override bool NextResult(IDataReader reader, Func<IDataReader, bool> continuation)
        {
            try
            {
                OutputEnter(nameof(NextResult));
                return base.NextResult(reader, continuation);
            }
            finally
            {
                OutputExit(nameof(NextResult));
            }
        }

        /// <inheritdoc/>  
        public override bool Read(IDataReader reader, Func<IDataReader, bool> continuation)
        {
            try
            {
                OutputEnter(nameof(Read));
                return base.Read(reader, continuation);
            }
            finally
            {
                OutputExit(nameof(Read));
            }
        }
    }
}
