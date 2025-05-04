using System;
using System.Collections.Generic;
using System.Data;
using System.Security.Cryptography;

namespace mxProject.Data
{
    /// <summary>  
    /// Provides an interface to filter and extend the behavior of an <see cref="IDataReader"/>.  
    /// </summary>  
    public interface IDataReaderFilter
    {
        /// <summary>
        /// Gets the target methods for the filter.
        /// </summary>
        DataReaderFilterTargets TargetMethods { get; }

        /// <summary>
        /// Closes the specified <see cref="IDataReader"/>.
        /// </summary>
        /// <param name="reader">The data reader to close.</param>
        /// <param name="continuation">The continuation action to execute after closing.</param>
        void Close(IDataReader reader, Action<IDataReader> continuation);

        /// <summary>
        /// Disposes the specified <see cref="IDataReader"/>.
        /// </summary>
        /// <param name="reader">The data reader to dispose.</param>
        /// <param name="continuation">The continuation action to execute after disposing.</param>
        void Dispose(IDataReader reader, Action<IDataReader> continuation);

        /// <summary>
        /// Gets a boolean value from the specified column.
        /// </summary>
        /// <param name="reader">The data reader.</param>
        /// <param name="i">The column index.</param>
        /// <param name="continuation">The continuation function to execute.</param>
        /// <returns>The boolean value.</returns>
        bool GetBoolean(IDataReader reader, int i, Func<IDataReader, int, bool> continuation);

        /// <summary>
        /// Gets a byte value from the specified column.
        /// </summary>
        /// <param name="reader">The data reader.</param>
        /// <param name="i">The column index.</param>
        /// <param name="continuation">The continuation function to execute.</param>
        /// <returns>The byte value.</returns>
        byte GetByte(IDataReader reader, int i, Func<IDataReader, int, byte> continuation);

        /// <summary>
        /// Reads a stream of bytes from the specified column.
        /// </summary>
        /// <param name="reader">The data reader.</param>
        /// <param name="i">The column index.</param>
        /// <param name="fieldOffset">The field offset.</param>
        /// <param name="buffer">The buffer to store the bytes.</param>
        /// <param name="bufferoffset">The buffer offset.</param>
        /// <param name="length">The number of bytes to read.</param>
        /// <param name="continuation">The continuation function to execute.</param>
        /// <returns>The actual number of bytes read.</returns>
        long GetBytes(IDataReader reader, int i, long fieldOffset, byte[] buffer, int bufferoffset, int length, Func<IDataReader, int, long, byte[], int, int, long> continuation);

        /// <summary>
        /// Gets a character value from the specified column.
        /// </summary>
        /// <param name="reader">The data reader.</param>
        /// <param name="i">The column index.</param>
        /// <param name="continuation">The continuation function to execute.</param>
        /// <returns>The character value.</returns>
        char GetChar(IDataReader reader, int i, Func<IDataReader, int, char> continuation);

        /// <summary>
        /// Reads a stream of characters from the specified column.
        /// </summary>
        /// <param name="reader">The data reader.</param>
        /// <param name="i">The column index.</param>
        /// <param name="fieldoffset">The field offset.</param>
        /// <param name="buffer">The buffer to store the characters.</param>
        /// <param name="bufferoffset">The buffer offset.</param>
        /// <param name="length">The number of characters to read.</param>
        /// <param name="continuation">The continuation function to execute.</param>
        /// <returns>The actual number of characters read.</returns>
        long GetChars(IDataReader reader, int i, long fieldoffset, char[] buffer, int bufferoffset, int length, Func<IDataReader, int, long, char[], int, int, long> continuation);

        /// <summary>
        /// Gets an <see cref="IDataReader"/> for the specified column.
        /// </summary>
        /// <param name="reader">The data reader.</param>
        /// <param name="i">The column index.</param>
        /// <param name="continuation">The continuation function to execute.</param>
        /// <returns>The <see cref="IDataReader"/>.</returns>
        IDataReader GetData(IDataReader reader, int i, Func<IDataReader, int, IDataReader> continuation);

        /// <summary>
        /// Gets the data type name of the specified column.
        /// </summary>
        /// <param name="reader">The data reader.</param>
        /// <param name="i">The column index.</param>
        /// <param name="continuation">The continuation function to execute.</param>
        /// <returns>The data type name.</returns>
        string GetDataTypeName(IDataReader reader, int i, Func<IDataReader, int, string> continuation);

        /// <summary>
        /// Gets a <see cref="DateTime"/> value from the specified column.
        /// </summary>
        /// <param name="reader">The data reader.</param>
        /// <param name="i">The column index.</param>
        /// <param name="continuation">The continuation function to execute.</param>
        /// <returns>The <see cref="DateTime"/> value.</returns>
        DateTime GetDateTime(IDataReader reader, int i, Func<IDataReader, int, DateTime> continuation);

        /// <summary>
        /// Gets a <see cref="decimal"/> value from the specified column.
        /// </summary>
        /// <param name="reader">The data reader.</param>
        /// <param name="i">The column index.</param>
        /// <param name="continuation">The continuation function to execute.</param>
        /// <returns>The <see cref="decimal"/> value.</returns>
        decimal GetDecimal(IDataReader reader, int i, Func<IDataReader, int, decimal> continuation);

        /// <summary>
        /// Gets a <see cref="double"/> value from the specified column.
        /// </summary>
        /// <param name="reader">The data reader.</param>
        /// <param name="i">The column index.</param>
        /// <param name="continuation">The continuation function to execute.</param>
        /// <returns>The <see cref="double"/> value.</returns>
        double GetDouble(IDataReader reader, int i, Func<IDataReader, int, double> continuation);

        /// <summary>
        /// Gets the <see cref="Type"/> of the specified column.
        /// </summary>
        /// <param name="reader">The data reader.</param>
        /// <param name="i">The column index.</param>
        /// <param name="continuation">The continuation function to execute.</param>
        /// <returns>The <see cref="Type"/> of the column.</returns>
        Type GetFieldType(IDataReader reader, int i, Func<IDataReader, int, Type> continuation);

        /// <summary>
        /// Gets a <see cref="float"/> value from the specified column.
        /// </summary>
        /// <param name="reader">The data reader.</param>
        /// <param name="i">The column index.</param>
        /// <param name="continuation">The continuation function to execute.</param>
        /// <returns>The <see cref="float"/> value.</returns>
        float GetFloat(IDataReader reader, int i, Func<IDataReader, int, float> continuation);

        /// <summary>
        /// Gets a <see cref="Guid"/> value from the specified column.
        /// </summary>
        /// <param name="reader">The data reader.</param>
        /// <param name="i">The column index.</param>
        /// <param name="continuation">The continuation function to execute.</param>
        /// <returns>The <see cref="Guid"/> value.</returns>
        Guid GetGuid(IDataReader reader, int i, Func<IDataReader, int, Guid> continuation);

        /// <summary>
        /// Gets a <see cref="short"/> value from the specified column.
        /// </summary>
        /// <param name="reader">The data reader.</param>
        /// <param name="i">The column index.</param>
        /// <param name="continuation">The continuation function to execute.</param>
        /// <returns>The <see cref="short"/> value.</returns>
        short GetInt16(IDataReader reader, int i, Func<IDataReader, int, short> continuation);

        /// <summary>
        /// Gets an <see cref="int"/> value from the specified column.
        /// </summary>
        /// <param name="reader">The data reader.</param>
        /// <param name="i">The column index.</param>
        /// <param name="continuation">The continuation function to execute.</param>
        /// <returns>The <see cref="int"/> value.</returns>
        int GetInt32(IDataReader reader, int i, Func<IDataReader, int, int> continuation);

        /// <summary>
        /// Gets a <see cref="long"/> value from the specified column.
        /// </summary>
        /// <param name="reader">The data reader.</param>
        /// <param name="i">The column index.</param>
        /// <param name="continuation">The continuation function to execute.</param>
        /// <returns>The <see cref="long"/> value.</returns>
        long GetInt64(IDataReader reader, int i, Func<IDataReader, int, long> continuation);

        /// <summary>
        /// Gets the name of the specified column.
        /// </summary>
        /// <param name="reader">The data reader.</param>
        /// <param name="i">The column index.</param>
        /// <param name="continuation">The continuation function to execute.</param>
        /// <returns>The name of the column.</returns>
        string GetName(IDataReader reader, int i, Func<IDataReader, int, string> continuation);

        /// <summary>
        /// Gets the ordinal of the specified column by name.
        /// </summary>
        /// <param name="reader">The data reader.</param>
        /// <param name="name">The name of the column.</param>
        /// <param name="continuation">The continuation function to execute.</param>
        /// <returns>The ordinal of the column.</returns>
        int GetOrdinal(IDataReader reader, string name, Func<IDataReader, string, int> continuation);

        /// <summary>
        /// Gets the schema table of the data reader.
        /// </summary>
        /// <param name="reader">The data reader.</param>
        /// <param name="continuation">The continuation function to execute.</param>
        /// <returns>The schema table.</returns>
        DataTable GetSchemaTable(IDataReader reader, Func<IDataReader, DataTable> continuation);

        /// <summary>
        /// Gets a <see cref="string"/> value from the specified column.
        /// </summary>
        /// <param name="reader">The data reader.</param>
        /// <param name="i">The column index.</param>
        /// <param name="continuation">The continuation function to execute.</param>
        /// <returns>The <see cref="string"/> value.</returns>
        string GetString(IDataReader reader, int i, Func<IDataReader, int, string> continuation);

        /// <summary>
        /// Gets the value of the specified column as an <see cref="object"/>.
        /// </summary>
        /// <param name="reader">The data reader.</param>
        /// <param name="i">The column index.</param>
        /// <param name="continuation">The continuation function to execute.</param>
        /// <returns>The value of the column as an <see cref="object"/>.</returns>
        object GetValue(IDataReader reader, int i, Func<IDataReader, int, object> continuation);

        /// <summary>
        /// Gets all the column values of the current row.
        /// </summary>
        /// <param name="reader">The data reader.</param>
        /// <param name="values">An array to store the column values.</param>
        /// <param name="continuation">The continuation function to execute.</param>
        /// <returns>The number of column values copied to the array.</returns>
        int GetValues(IDataReader reader, object[] values, Func<IDataReader, object[], int> continuation);

        /// <summary>
        /// Determines whether the specified column is <see cref="DBNull"/>.
        /// </summary>
        /// <param name="reader">The data reader.</param>
        /// <param name="i">The column index.</param>
        /// <param name="continuation">The continuation function to execute.</param>
        /// <returns><c>true</c> if the column is <see cref="DBNull"/>; otherwise, <c>false</c>.</returns>
        bool IsDBNull(IDataReader reader, int i, Func<IDataReader, int, bool> continuation);

        /// <summary>
        /// Advances the data reader to the next result set.
        /// </summary>
        /// <param name="reader">The data reader.</param>
        /// <param name="continuation">The continuation function to execute.</param>
        /// <returns><c>true</c> if there are more result sets; otherwise, <c>false</c>.</returns>
        bool NextResult(IDataReader reader, Func<IDataReader, bool> continuation);

        /// <summary>
        /// Advances the data reader to the next record.
        /// </summary>
        /// <param name="reader">The data reader.</param>
        /// <param name="continuation">The continuation function to execute.</param>
        /// <returns><c>true</c> if there are more rows; otherwise, <c>false</c>.</returns>
        bool Read(IDataReader reader, Func<IDataReader, bool> continuation);
    }
}
