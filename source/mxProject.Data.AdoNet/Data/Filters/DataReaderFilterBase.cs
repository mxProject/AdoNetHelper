using System;
using System.Collections.Generic;
using System.Data;

namespace mxProject.Data.Filters
{
    /// <summary>  
    /// Base class for filtering and extending the behavior of an <see cref="IDataReader"/>.  
    /// </summary>  
    public abstract class DataReaderFilterBase : IDataReaderFilter
    {
        /// <summary>  
        /// Initializes a new instance of the <see cref="DataReaderFilterBase"/> class.  
        /// </summary>  
        protected DataReaderFilterBase()
        {
        }

        /// <inheritdoc/>  
        public virtual DataReaderFilterTargets TargetMethods => DataReaderFilterTargets.None;

        /// <inheritdoc/>  
        public virtual void Close(IDataReader reader, Action<IDataReader> continuation)
        {
            continuation(reader);
        }

        /// <inheritdoc/>  
        public virtual void Dispose(IDataReader reader, Action<IDataReader> continuation)
        {
            continuation(reader);
        }

        /// <inheritdoc/>  
        public virtual bool GetBoolean(IDataReader reader, int i, Func<IDataReader, int, bool> continuation)
        {
            return continuation(reader, i);
        }

        /// <inheritdoc/>  
        public virtual byte GetByte(IDataReader reader, int i, Func<IDataReader, int, byte> continuation)
        {
            return continuation(reader, i);
        }

        /// <inheritdoc/>  
        public virtual long GetBytes(IDataReader reader, int i, long fieldOffset, byte[] buffer, int bufferoffset, int length, Func<IDataReader, int, long, byte[], int, int, long> continuation)
        {
            return continuation(reader, i, fieldOffset, buffer, bufferoffset, length);
        }

        /// <inheritdoc/>  
        public virtual char GetChar(IDataReader reader, int i, Func<IDataReader, int, char> continuation)
        {
            return continuation(reader, i);
        }

        /// <inheritdoc/>  
        public virtual long GetChars(IDataReader reader, int i, long fieldoffset, char[] buffer, int bufferoffset, int length, Func<IDataReader, int, long, char[], int, int, long> continuation)
        {
            return continuation(reader, i, fieldoffset, buffer, bufferoffset, length);
        }

        /// <inheritdoc/>  
        public virtual IDataReader GetData(IDataReader reader, int i, Func<IDataReader, int, IDataReader> continuation)
        {
            return continuation(reader, i);
        }

        /// <inheritdoc/>  
        public virtual string GetDataTypeName(IDataReader reader, int i, Func<IDataReader, int, string> continuation)
        {
            return continuation(reader, i);
        }

        /// <inheritdoc/>  
        public virtual DateTime GetDateTime(IDataReader reader, int i, Func<IDataReader, int, DateTime> continuation)
        {
            return continuation(reader, i);
        }

        /// <inheritdoc/>  
        public virtual decimal GetDecimal(IDataReader reader, int i, Func<IDataReader, int, decimal> continuation)
        {
            return continuation(reader, i);
        }

        /// <inheritdoc/>  
        public virtual double GetDouble(IDataReader reader, int i, Func<IDataReader, int, double> continuation)
        {
            return continuation(reader, i);
        }

        /// <inheritdoc/>  
        public virtual Type GetFieldType(IDataReader reader, int i, Func<IDataReader, int, Type> continuation)
        {
            return continuation(reader, i);
        }

        /// <inheritdoc/>  
        public virtual float GetFloat(IDataReader reader, int i, Func<IDataReader, int, float> continuation)
        {
            return continuation(reader, i);
        }

        /// <inheritdoc/>  
        public virtual Guid GetGuid(IDataReader reader, int i, Func<IDataReader, int, Guid> continuation)
        {
            return continuation(reader, i);
        }

        /// <inheritdoc/>  
        public virtual short GetInt16(IDataReader reader, int i, Func<IDataReader, int, short> continuation)
        {
            return continuation(reader, i);
        }

        /// <inheritdoc/>  
        public virtual int GetInt32(IDataReader reader, int i, Func<IDataReader, int, int> continuation)
        {
            return continuation(reader, i);
        }

        /// <inheritdoc/>  
        public virtual long GetInt64(IDataReader reader, int i, Func<IDataReader, int, long> continuation)
        {
            return continuation(reader, i);
        }

        /// <inheritdoc/>  
        public virtual string GetName(IDataReader reader, int i, Func<IDataReader, int, string> continuation)
        {
            return continuation(reader, i);
        }

        /// <inheritdoc/>  
        public virtual int GetOrdinal(IDataReader reader, string name, Func<IDataReader, string, int> continuation)
        {
            return continuation(reader, name);
        }

        /// <inheritdoc/>  
        public virtual DataTable GetSchemaTable(IDataReader reader, Func<IDataReader, DataTable> continuation)
        {
            return continuation(reader);
        }

        /// <inheritdoc/>  
        public virtual string GetString(IDataReader reader, int i, Func<IDataReader, int, string> continuation)
        {
            return continuation(reader, i);
        }

        /// <inheritdoc/>  
        public virtual object GetValue(IDataReader reader, int i, Func<IDataReader, int, object> continuation)
        {
            return continuation(reader, i);
        }

        /// <inheritdoc/>  
        public virtual int GetValues(IDataReader reader, object[] values, Func<IDataReader, object[], int> continuation)
        {
            return continuation(reader, values);
        }

        /// <inheritdoc/>  
        public virtual bool IsDBNull(IDataReader reader, int i, Func<IDataReader, int, bool> continuation)
        {
            return continuation(reader, i);
        }

        /// <inheritdoc/>  
        public virtual bool NextResult(IDataReader reader, Func<IDataReader, bool> continuation)
        {
            return continuation(reader);
        }

        /// <inheritdoc/>  
        public virtual bool Read(IDataReader reader, Func<IDataReader, bool> continuation)
        {
            return continuation(reader);
        }
    }
}
