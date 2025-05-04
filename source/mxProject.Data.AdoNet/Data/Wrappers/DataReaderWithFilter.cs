using System;
using System.Collections.Generic;
using System.Text;
using System.Data;
using mxProject.Data.Filters;

namespace mxProject.Data.Wrappers
{
    internal sealed class DataReaderWithFilter : IDataReader, IDataReaderWrapper
    {
        internal DataReaderWithFilter(IDataReader reeader, DbFilterSet<DataReaderFilterTargets, IDataReaderFilter> filters)
        {
            WrappedDataReader = reeader;

            m_Filters = filters;
        }

        #region reader

        internal IDataReader WrappedDataReader { get; }

        IDataReader IDataReaderWrapper.WrappedReader => WrappedDataReader;

        #endregion

        #region filter

        private readonly DbFilterSet<DataReaderFilterTargets, IDataReaderFilter> m_Filters;

        #endregion

        /// <inheritdoc/>
        public object this[int i] => GetValue(i);

        /// <inheritdoc/>
        public object this[string name] => GetValue(GetOrdinal(name));

        /// <inheritdoc/>
        public int Depth => WrappedDataReader.Depth;

        /// <inheritdoc/>
        public bool IsClosed => WrappedDataReader.IsClosed;

        /// <inheritdoc/>
        public int RecordsAffected => WrappedDataReader.RecordsAffected;

        /// <inheritdoc/>
        public int FieldCount => WrappedDataReader.FieldCount;

        /// <inheritdoc/>
        public void Close()
        {
            m_Filters[DataReaderFilterTargets.Close].Close(WrappedDataReader);
        }

        /// <inheritdoc/>
        public void Dispose()
        {
            m_Filters[DataReaderFilterTargets.Dispose].Dispose(WrappedDataReader);
        }

        /// <inheritdoc/>
        public bool GetBoolean(int i)
        {
            return m_Filters[DataReaderFilterTargets.GetBoolean].GetBoolean(WrappedDataReader, i);
        }

        /// <inheritdoc/>
        public byte GetByte(int i)
        {
            return m_Filters[DataReaderFilterTargets.GetByte].GetByte(WrappedDataReader, i);
        }

        /// <inheritdoc/>
        public long GetBytes(int i, long fieldOffset, byte[] buffer, int bufferoffset, int length)
        {
            return m_Filters[DataReaderFilterTargets.GetBytes].GetBytes(WrappedDataReader, i, fieldOffset, buffer, bufferoffset, length);
        }

        /// <inheritdoc/>
        public char GetChar(int i)
        {
            return m_Filters[DataReaderFilterTargets.GetChar].GetChar(WrappedDataReader, i);
        }

        /// <inheritdoc/>
        public long GetChars(int i, long fieldoffset, char[] buffer, int bufferoffset, int length)
        {
            return m_Filters[DataReaderFilterTargets.GetChars].GetChars(WrappedDataReader, i, fieldoffset, buffer, bufferoffset, length);
        }

        /// <inheritdoc/>
        public IDataReader GetData(int i)
        {
            var reader = m_Filters[DataReaderFilterTargets.GetData].GetData(WrappedDataReader, i);

            if (reader == null) { return null!; }

            return new DataReaderWithFilter(reader, m_Filters);
        }

        /// <inheritdoc/>
        public string GetDataTypeName(int i)
        {
            return m_Filters[DataReaderFilterTargets.GetDataTypeName].GetDataTypeName(WrappedDataReader, i);
        }

        /// <inheritdoc/>
        public DateTime GetDateTime(int i)
        {
            return m_Filters[DataReaderFilterTargets.GetDateTime].GetDateTime(WrappedDataReader, i);
        }

        /// <inheritdoc/>
        public decimal GetDecimal(int i)
        {
            return m_Filters[DataReaderFilterTargets.GetDecimal].GetDecimal(WrappedDataReader, i);
        }

        /// <inheritdoc/>
        public double GetDouble(int i)
        {
            return m_Filters[DataReaderFilterTargets.GetDouble].GetDouble(WrappedDataReader, i);
        }

        /// <inheritdoc/>
        public Type GetFieldType(int i)
        {
            return m_Filters[DataReaderFilterTargets.GetFieldType].GetFieldType(WrappedDataReader, i);
        }

        /// <inheritdoc/>
        public float GetFloat(int i)
        {
            return m_Filters[DataReaderFilterTargets.GetFloat].GetFloat(WrappedDataReader, i);
        }

        /// <inheritdoc/>
        public Guid GetGuid(int i)
        {
            return m_Filters[DataReaderFilterTargets.GetGuid].GetGuid(WrappedDataReader, i);
        }

        /// <inheritdoc/>
        public short GetInt16(int i)
        {
            return m_Filters[DataReaderFilterTargets.GetInt16].GetInt16(WrappedDataReader, i);
        }

        /// <inheritdoc/>
        public int GetInt32(int i)
        {
            return m_Filters[DataReaderFilterTargets.GetInt32].GetInt32(WrappedDataReader, i);
        }

        /// <inheritdoc/>
        public long GetInt64(int i)
        {
            return m_Filters[DataReaderFilterTargets.GetInt64].GetInt64(WrappedDataReader, i);
        }

        /// <inheritdoc/>
        public string GetName(int i)
        {
            return m_Filters[DataReaderFilterTargets.GetName].GetName(WrappedDataReader, i);
        }

        /// <inheritdoc/>
        public int GetOrdinal(string name)
        {
            return m_Filters[DataReaderFilterTargets.GetOrdinal].GetOrdinal(WrappedDataReader, name);
        }

        /// <inheritdoc/>
        public DataTable GetSchemaTable()
        {
            return m_Filters[DataReaderFilterTargets.GetSchemaTable].GetSchemaTable(WrappedDataReader);
        }

        /// <inheritdoc/>
        public string GetString(int i)
        {
            return m_Filters[DataReaderFilterTargets.GetString].GetString(WrappedDataReader, i);
        }

        /// <inheritdoc/>
        public object GetValue(int i)
        {
            return m_Filters[DataReaderFilterTargets.GetValue].GetValue(WrappedDataReader, i);
        }

        /// <inheritdoc/>
        public int GetValues(object[] values)
        {
            return m_Filters[DataReaderFilterTargets.GetValues].GetValues(WrappedDataReader, values);
        }

        /// <inheritdoc/>
        public bool IsDBNull(int i)
        {
            return m_Filters[DataReaderFilterTargets.IsDBNull].IsDBNull(WrappedDataReader, i);
        }

        /// <inheritdoc/>
        public bool NextResult()
        {
            return m_Filters[DataReaderFilterTargets.NextResult].NextResult(WrappedDataReader);
        }

        /// <inheritdoc/>
        public bool Read()
        {
            return m_Filters[DataReaderFilterTargets.Read].Read(WrappedDataReader);
        }
    }
}
