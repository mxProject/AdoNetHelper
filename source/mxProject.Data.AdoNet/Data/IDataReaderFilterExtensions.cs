using mxProject.Data.Extensions;
using System;
using System.Collections.Generic;
using System.Data;
using System.Text;

namespace mxProject.Data
{
    /// <summary>
    /// Provides extension methods for applying filters to <see cref="IDataReader"/> operations.
    /// </summary>
    public static class IDataReaderFilterExtensions
    {
        #region Dispose

        /// <summary>
        /// Disposes the specified <see cref="IDataReader"/> with the provided filters.
        /// </summary>
        /// <param name="this">The array of <see cref="IDataReaderFilter"/> to apply.</param>
        /// <param name="reader">The <see cref="IDataReader"/> to dispose.</param>
        public static void Dispose(this IDataReaderFilter[] @this, IDataReader reader)
        {
            if (@this == null || @this.Length == 0)
            {
                reader.Dispose();
            }
            else
            {
                FilterUtility.InvokeAction(
                    @this,
                    reader,
                    _reader => _reader.Dispose(),
                    (_filter, _reader, _continuation) => _filter.Dispose(_reader, _continuation)
                    );
            }
        }

        #endregion

        #region Close

        /// <summary>
        /// Closes the specified <see cref="IDataReader"/> with the provided filters.
        /// </summary>
        /// <param name="this">The array of <see cref="IDataReaderFilter"/> to apply.</param>
        /// <param name="reader">The <see cref="IDataReader"/> to close.</param>
        public static void Close(this IDataReaderFilter[] @this, IDataReader reader)
        {
            if (@this == null || @this.Length == 0)
            {
                reader.Close();
            }
            else
            {
                FilterUtility.InvokeAction(
                    @this,
                    reader,
                    _reader => _reader.Close(),
                    (_filter, _reader, _continuation) => _filter.Close(_reader, _continuation)
                    );
            }
        }

        #endregion

        #region Read

        /// <summary>
        /// Reads the next record from the data reader, applying the specified filters.
        /// </summary>
        /// <param name="this">The array of <see cref="IDataReaderFilter"/> to apply.</param>
        /// <param name="reader">The <see cref="IDataReader"/> to read from.</param>
        /// <returns><c>true</c> if there are more rows; otherwise, <c>false</c>.</returns>
        public static bool Read(this IDataReaderFilter[] @this, IDataReader reader)
        {
            if (@this == null || @this.Length == 0)
            {
                return reader.Read();
            }
            else
            {
                return FilterUtility.InvokeFunc(
                    @this,
                    reader,
                    _reader => _reader.Read(),
                    (_filter, _reader, _continuation) => _filter.Read(_reader, _continuation)
                    );
            }
        }

        /// <summary>
        /// Advances the data reader to the next result set, applying the specified filters.
        /// </summary>
        /// <param name="this">The array of <see cref="IDataReaderFilter"/> to apply.</param>
        /// <param name="reader">The <see cref="IDataReader"/> to advance.</param>
        /// <returns><c>true</c> if there are more result sets; otherwise, <c>false</c>.</returns>
        public static bool NextResult(this IDataReaderFilter[] @this, IDataReader reader)
        {
            if (@this == null || @this.Length == 0)
            {
                return reader.NextResult();
            }
            else
            {
                return FilterUtility.InvokeFunc(
                    @this,
                    reader,
                    _reader => _reader.NextResult(),
                    (_filter, _reader, _continuation) => _filter.NextResult(_reader, _continuation)
                    );
            }
        }

        /// <summary>
        /// Determines whether the specified column contains a <c>DBNull</c> value, applying the specified filters.
        /// </summary>
        /// <param name="this">The array of <see cref="IDataReaderFilter"/> to apply.</param>
        /// <param name="reader">The <see cref="IDataReader"/> to check.</param>
        /// <param name="index">The zero-based column ordinal.</param>
        /// <returns><c>true</c> if the column contains a <c>DBNull</c> value; otherwise, <c>false</c>.</returns>
        public static bool IsDBNull(this IDataReaderFilter[] @this, IDataReader reader, int index)
        {
            if (@this == null || @this.Length == 0)
            {
                return reader.IsDBNull(index);
            }
            else
            {
                return FilterUtility.InvokeFunc(
                    @this,
                    reader,
                    index,
                    (_reader, _index) => _reader.IsDBNull(_index),
                    (_filter, _reader, _index, _continuation) => _filter.IsDBNull(_reader, _index, _continuation)
                    );
            }
        }

        #endregion

        #region GetName

        /// <summary>
        /// Invokes the GetName method on the specified IDataReader, applying the provided filters.
        /// </summary>
        /// <param name="this">The array of <see cref="IDataReaderFilter"/> to apply.</param>
        /// <param name="reader">The <see cref="IDataReader"/> to retrieve the column name from.</param>
        /// <param name="index">The zero-based column ordinal.</param>
        /// <returns>The name of the column.</returns>
        public static string GetName(this IDataReaderFilter[] @this, IDataReader reader, int index)
        {
            if (@this == null || @this.Length == 0)
            {
                return reader.GetName(index);
            }
            else
            {
                return FilterUtility.InvokeFunc(
                    @this,
                    reader,
                    index,
                    (_reader, _index) => _reader.GetName(_index),
                    (_filter, _reader, _index, _continuation) => _filter.GetName(_reader, _index, _continuation)
                    );
            }
        }

        /// <summary>
        /// Invokes the GetOrdinal method on the specified IDataReader, applying the provided filters.
        /// </summary>
        /// <param name="this">The array of <see cref="IDataReaderFilter"/> to apply.</param>
        /// <param name="reader">The <see cref="IDataReader"/> to retrieve the column ordinal from.</param>
        /// <param name="name">The name of the column.</param>
        /// <returns>The zero-based column ordinal.</returns>
        public static int GetOrdinal(this IDataReaderFilter[] @this, IDataReader reader, string name)
        {
            if (@this == null || @this.Length == 0)
            {
                return reader.GetOrdinal(name);
            }
            else
            {
                return FilterUtility.InvokeFunc(
                    @this,
                    reader,
                    name,
                    (_reader, _name) => _reader.GetOrdinal(_name),
                    (_filter, _reader, _name, _continuation) => _filter.GetOrdinal(_reader, _name, _continuation)
                    );
            }
        }

        /// <summary>
        /// Invokes the GetSchemaTable method on the specified IDataReader, applying the provided filters.
        /// </summary>
        /// <param name="this">The array of <see cref="IDataReaderFilter"/> to apply.</param>
        /// <param name="reader">The <see cref="IDataReader"/> to retrieve the schema table from.</param>
        /// <returns>A <see cref="DataTable"/> that describes the column metadata.</returns>
        public static DataTable GetSchemaTable(this IDataReaderFilter[] @this, IDataReader reader)
        {
            if (@this == null || @this.Length == 0)
            {
                return reader.GetSchemaTable();
            }
            else
            {
                return FilterUtility.InvokeFunc(
                    @this,
                    reader,
                    _reader => _reader.GetSchemaTable(),
                    (_filter, _reader, _continuation) => _filter.GetSchemaTable(_reader, _continuation)
                    );
            }
        }

        /// <summary>
        /// Invokes the GetFieldType method on the specified IDataReader, applying the provided filters.
        /// </summary>
        /// <param name="this">The array of <see cref="IDataReaderFilter"/> to apply.</param>
        /// <param name="reader">The <see cref="IDataReader"/> to retrieve the field type from.</param>
        /// <param name="index">The zero-based column ordinal.</param>
        /// <returns>The type of the field.</returns>
        public static Type GetFieldType(this IDataReaderFilter[] @this, IDataReader reader, int index)
        {
            if (@this == null || @this.Length == 0)
            {
                return reader.GetFieldType(index);
            }
            else
            {
                return FilterUtility.InvokeFunc(
                    @this,
                    reader,
                    index,
                    (_reader, _index) => _reader.GetFieldType(_index),
                    (_filter, _reader, _index, _continuation) => _filter.GetFieldType(_reader, _index, _continuation)
                    );
            }
        }

        /// <summary>
        /// Invokes the GetDataTypeName method on the specified IDataReader, applying the provided filters.
        /// </summary>
        /// <param name="this">The array of <see cref="IDataReaderFilter"/> to apply.</param>
        /// <param name="reader">The <see cref="IDataReader"/> to retrieve the data type name from.</param>
        /// <param name="index">The zero-based column ordinal.</param>
        /// <returns>The name of the data type of the column.</returns>
        public static string GetDataTypeName(this IDataReaderFilter[] @this, IDataReader reader, int index)
        {
            if (@this == null || @this.Length == 0)
            {
                return reader.GetDataTypeName(index);
            }
            else
            {
                return FilterUtility.InvokeFunc(
                    @this,
                    reader,
                    index,
                    (_reader, _index) => _reader.GetDataTypeName(_index),
                    (_filter, _reader, _index, _continuation) => _filter.GetDataTypeName(_reader, _index, _continuation)
                    );
            }
        }

        #endregion

        #region GetValue

        /// <summary>
        /// Invokes the GetBoolean method on the specified IDataReader, applying the provided filters.
        /// </summary>
        /// <param name="this">The array of <see cref="IDataReaderFilter"/> to apply.</param>
        /// <param name="reader">The <see cref="IDataReader"/> to retrieve the boolean value from.</param>
        /// <param name="index">The zero-based column ordinal.</param>
        /// <returns>The boolean value of the column.</returns>
        public static bool GetBoolean(this IDataReaderFilter[] @this, IDataReader reader, int index)
        {
            if (@this == null || @this.Length == 0)
            {
                return reader.GetBoolean(index);
            }
            else
            {
                return FilterUtility.InvokeFunc(
                    @this,
                    reader,
                    index,
                    (_reader, _index) => _reader.GetBoolean(_index),
                    (_filter, _reader, _index, _continuation) => _filter.GetBoolean(_reader, _index, _continuation)
                    );
            }
        }

        /// <summary>
        /// Invokes the GetByte method on the specified IDataReader, applying the provided filters.
        /// </summary>
        /// <param name="this">The array of <see cref="IDataReaderFilter"/> to apply.</param>
        /// <param name="reader">The <see cref="IDataReader"/> to retrieve the byte value from.</param>
        /// <param name="index">The zero-based column ordinal.</param>
        /// <returns>The byte value of the column.</returns>
        public static byte GetByte(this IDataReaderFilter[] @this, IDataReader reader, int index)
        {
            if (@this == null || @this.Length == 0)
            {
                return reader.GetByte(index);
            }
            else
            {
                return FilterUtility.InvokeFunc(
                    @this,
                    reader,
                    index,
                    (_reader, _index) => _reader.GetByte(_index),
                    (_filter, _reader, _index, _continuation) => _filter.GetByte(_reader, _index, _continuation)
                    );
            }
        }

        /// <summary>
        /// Invokes the GetChar method on the specified IDataReader, applying the provided filters.
        /// </summary>
        /// <param name="this">The array of <see cref="IDataReaderFilter"/> to apply.</param>
        /// <param name="reader">The <see cref="IDataReader"/> to retrieve the character value from.</param>
        /// <param name="index">The zero-based column ordinal.</param>
        /// <returns>The character value of the column.</returns>
        public static char GetChar(this IDataReaderFilter[] @this, IDataReader reader, int index)
        {
            if (@this == null || @this.Length == 0)
            {
                return reader.GetChar(index);
            }
            else
            {
                return FilterUtility.InvokeFunc(
                    @this,
                    reader,
                    index,
                    (_reader, _index) => _reader.GetChar(_index),
                    (_filter, _reader, _index, _continuation) => _filter.GetChar(_reader, _index, _continuation)
                    );
            }
        }

        /// <summary>
        /// Invokes the GetData method on the specified IDataReader, applying the provided filters.
        /// </summary>
        /// <param name="this">The array of <see cref="IDataReaderFilter"/> to apply.</param>
        /// <param name="reader">The <see cref="IDataReader"/> to retrieve the nested data reader from.</param>
        /// <param name="index">The zero-based column ordinal.</param>
        /// <returns>The nested data reader.</returns>
        public static IDataReader GetData(this IDataReaderFilter[] @this, IDataReader reader, int index)
        {
            if (@this == null || @this.Length == 0)
            {
                return reader.GetData(index);
            }
            else
            {
                return FilterUtility.InvokeFunc(
                    @this,
                    reader,
                    index,
                    (_reader, _index) => _reader.GetData(_index),
                    (_filter, _reader, _index, _continuation) => _filter.GetData(_reader, _index, _continuation)
                    );
            }
        }

        /// <summary>
        /// Invokes the GetDateTime method on the specified IDataReader, applying the provided filters.
        /// </summary>
        /// <param name="this">The array of <see cref="IDataReaderFilter"/> to apply.</param>
        /// <param name="reader">The <see cref="IDataReader"/> to retrieve the date and time value from.</param>
        /// <param name="index">The zero-based column ordinal.</param>
        /// <returns>The date and time value of the column.</returns>
        public static DateTime GetDateTime(this IDataReaderFilter[] @this, IDataReader reader, int index)
        {
            if (@this == null || @this.Length == 0)
            {
                return reader.GetDateTime(index);
            }
            else
            {
                return FilterUtility.InvokeFunc(
                    @this,
                    reader,
                    index,
                    (_reader, _index) => _reader.GetDateTime(_index),
                    (_filter, _reader, _index, _continuation) => _filter.GetDateTime(_reader, _index, _continuation)
                    );
            }
        }

        /// <summary>
        /// Invokes the GetDecimal method on the specified IDataReader, applying the provided filters.
        /// </summary>
        /// <param name="this">The array of <see cref="IDataReaderFilter"/> to apply.</param>
        /// <param name="reader">The <see cref="IDataReader"/> to retrieve the decimal value from.</param>
        /// <param name="index">The zero-based column ordinal.</param>
        /// <returns>The decimal value of the column.</returns>
        public static decimal GetDecimal(this IDataReaderFilter[] @this, IDataReader reader, int index)
        {
            if (@this == null || @this.Length == 0)
            {
                return reader.GetDecimal(index);
            }
            else
            {
                return FilterUtility.InvokeFunc(
                    @this,
                    reader,
                    index,
                    (_reader, _index) => _reader.GetDecimal(_index),
                    (_filter, _reader, _index, _continuation) => _filter.GetDecimal(_reader, _index, _continuation)
                    );
            }
        }

        /// <summary>
        /// Invokes the GetDouble method on the specified IDataReader, applying the provided filters.
        /// </summary>
        /// <param name="this">The array of <see cref="IDataReaderFilter"/> to apply.</param>
        /// <param name="reader">The <see cref="IDataReader"/> to retrieve the double value from.</param>
        /// <param name="index">The zero-based column ordinal.</param>
        /// <returns>The double value of the column.</returns>
        public static double GetDouble(this IDataReaderFilter[] @this, IDataReader reader, int index)
        {
            if (@this == null || @this.Length == 0)
            {
                return reader.GetDouble(index);
            }
            else
            {
                return FilterUtility.InvokeFunc(
                    @this,
                    reader,
                    index,
                    (_reader, _index) => _reader.GetDouble(_index),
                    (_filter, _reader, _index, _continuation) => _filter.GetDouble(_reader, _index, _continuation)
                    );
            }
        }

        /// <summary>
        /// Invokes the GetFloat method on the specified IDataReader, applying the provided filters.
        /// </summary>
        /// <param name="this">The array of <see cref="IDataReaderFilter"/> to apply.</param>
        /// <param name="reader">The <see cref="IDataReader"/> to retrieve the float value from.</param>
        /// <param name="index">The zero-based column ordinal.</param>
        /// <returns>The float value of the column.</returns>
        public static float GetFloat(this IDataReaderFilter[] @this, IDataReader reader, int index)
        {
            if (@this == null || @this.Length == 0)
            {
                return reader.GetFloat(index);
            }
            else
            {
                return FilterUtility.InvokeFunc(
                    @this,
                    reader,
                    index,
                    (_reader, _index) => _reader.GetFloat(_index),
                    (_filter, _reader, _index, _continuation) => _filter.GetFloat(_reader, _index, _continuation)
                    );
            }
        }

        /// <summary>
        /// Invokes the GetGuid method on the specified IDataReader, applying the provided filters.
        /// </summary>
        /// <param name="this">The array of <see cref="IDataReaderFilter"/> to apply.</param>
        /// <param name="reader">The <see cref="IDataReader"/> to retrieve the GUID value from.</param>
        /// <param name="index">The zero-based column ordinal.</param>
        /// <returns>The GUID value of the column.</returns>
        public static Guid GetGuid(this IDataReaderFilter[] @this, IDataReader reader, int index)
        {
            if (@this == null || @this.Length == 0)
            {
                return reader.GetGuid(index);
            }
            else
            {
                return FilterUtility.InvokeFunc(
                    @this,
                    reader,
                    index,
                    (_reader, _index) => _reader.GetGuid(_index),
                    (_filter, _reader, _index, _continuation) => _filter.GetGuid(_reader, _index, _continuation)
                    );
            }
        }

        /// <summary>
        /// Invokes the GetInt16 method on the specified IDataReader, applying the provided filters.
        /// </summary>
        /// <param name="this">The array of <see cref="IDataReaderFilter"/> to apply.</param>
        /// <param name="reader">The <see cref="IDataReader"/> to retrieve the 16-bit integer value from.</param>
        /// <param name="index">The zero-based column ordinal.</param>
        /// <returns>The 16-bit integer value of the column.</returns>
        public static short GetInt16(this IDataReaderFilter[] @this, IDataReader reader, int index)
        {
            if (@this == null || @this.Length == 0)
            {
                return reader.GetInt16(index);
            }
            else
            {
                return FilterUtility.InvokeFunc(
                    @this,
                    reader,
                    index,
                    (_reader, _index) => _reader.GetInt16(_index),
                    (_filter, _reader, _index, _continuation) => _filter.GetInt16(_reader, _index, _continuation)
                    );
            }
        }

        /// <summary>
        /// Invokes the GetInt32 method on the specified IDataReader, applying the provided filters.
        /// </summary>
        /// <param name="this">The array of <see cref="IDataReaderFilter"/> to apply.</param>
        /// <param name="reader">The <see cref="IDataReader"/> to retrieve the 32-bit integer value from.</param>
        /// <param name="index">The zero-based column ordinal.</param>
        /// <returns>The 32-bit integer value of the column.</returns>
        public static int GetInt32(this IDataReaderFilter[] @this, IDataReader reader, int index)
        {
            if (@this == null || @this.Length == 0)
            {
                return reader.GetInt32(index);
            }
            else
            {
                return FilterUtility.InvokeFunc(
                    @this,
                    reader,
                    index,
                    (_reader, _index) => _reader.GetInt32(_index),
                    (_filter, _reader, _index, _continuation) => _filter.GetInt32(_reader, _index, _continuation)
                    );
            }
        }

        /// <summary>
        /// Invokes the GetInt64 method on the specified IDataReader, applying the provided filters.
        /// </summary>
        /// <param name="this">The array of <see cref="IDataReaderFilter"/> to apply.</param>
        /// <param name="reader">The <see cref="IDataReader"/> to retrieve the 64-bit integer value from.</param>
        /// <param name="index">The zero-based column ordinal.</param>
        /// <returns>The 64-bit integer value of the column.</returns>
        public static long GetInt64(this IDataReaderFilter[] @this, IDataReader reader, int index)
        {
            if (@this == null || @this.Length == 0)
            {
                return reader.GetInt64(index);
            }
            else
            {
                return FilterUtility.InvokeFunc(
                    @this,
                    reader,
                    index,
                    (_reader, _index) => _reader.GetInt64(_index),
                    (_filter, _reader, _index, _continuation) => _filter.GetInt64(_reader, _index, _continuation)
                    );
            }
        }

        /// <summary>
        /// Invokes the GetString method on the specified IDataReader, applying the provided filters.
        /// </summary>
        /// <param name="this">The array of <see cref="IDataReaderFilter"/> to apply.</param>
        /// <param name="reader">The <see cref="IDataReader"/> to retrieve the string value from.</param>
        /// <param name="index">The zero-based column ordinal.</param>
        /// <returns>The string value of the column.</returns>
        public static string GetString(this IDataReaderFilter[] @this, IDataReader reader, int index)
        {
            if (@this == null || @this.Length == 0)
            {
                return reader.GetString(index);
            }
            else
            {
                return FilterUtility.InvokeFunc(
                    @this,
                    reader,
                    index,
                    (_reader, _index) => _reader.GetString(_index),
                    (_filter, _reader, _index, _continuation) => _filter.GetString(_reader, _index, _continuation)
                    );
            }
        }

        /// <summary>
        /// Invokes the GetValue method on the specified IDataReader, applying the provided filters.
        /// </summary>
        /// <param name="this">The array of <see cref="IDataReaderFilter"/> to apply.</param>
        /// <param name="reader">The <see cref="IDataReader"/> to retrieve the value from.</param>
        /// <param name="index">The zero-based column ordinal.</param>
        /// <returns>The value of the column.</returns>
        public static object GetValue(this IDataReaderFilter[] @this, IDataReader reader, int index)
        {
            if (@this == null || @this.Length == 0)
            {
                return reader.GetValue(index);
            }
            else
            {
                return FilterUtility.InvokeFunc(
                    @this,
                    reader,
                    index,
                    (_reader, _index) => _reader.GetValue(_index),
                    (_filter, _reader, _index, _continuation) => _filter.GetValue(_reader, _index, _continuation)
                    );
            }
        }

        /// <summary>
        /// Invokes the GetValues method on the specified IDataReader, applying the provided filters.
        /// </summary>
        /// <param name="this">The array of <see cref="IDataReaderFilter"/> to apply.</param>
        /// <param name="reader">The <see cref="IDataReader"/> to retrieve the values from.</param>
        /// <param name="values">The array to copy values into.</param>
        /// <returns>The number of values copied.</returns>
        public static int GetValues(this IDataReaderFilter[] @this, IDataReader reader, object[] values)
        {
            if (@this == null || @this.Length == 0)
            {
                return reader.GetValues(values);
            }
            else
            {
                return FilterUtility.InvokeFunc(
                    @this,
                    reader,
                    values,
                    (_reader, _values) => _reader.GetValues(_values),
                    (_filter, _reader, _values, _continuation) => _filter.GetValues(_reader, _values, _continuation)
                    );
            }
        }

        /// <summary>
        /// Invokes the GetBytes method on the specified IDataReader, applying the provided filters.
        /// </summary>
        /// <param name="this">The array of <see cref="IDataReaderFilter"/> to apply.</param>
        /// <param name="reader">The <see cref="IDataReader"/> to retrieve the bytes from.</param>
        /// <param name="index">The zero-based column ordinal.</param>
        /// <param name="fieldOffset">The field offset.</param>
        /// <param name="buffer">The buffer to copy data into.</param>
        /// <param name="bufferoffset">The buffer offset.</param>
        /// <param name="length">The number of bytes to read.</param>
        /// <returns>The number of bytes read.</returns>
        public static long GetBytes(this IDataReaderFilter[] @this, IDataReader reader, int index, long fieldOffset, byte[] buffer, int bufferoffset, int length)
        {
            if (@this == null || @this.Length == 0)
            {
                return reader.GetBytes(index, fieldOffset, buffer, bufferoffset, length);
            }
            else
            {
                return FilterUtility.InvokeFunc(
                    @this,
                    reader,
                    (index, fieldOffset, buffer, bufferoffset, length),
                    (_reader, args) => _reader.GetBytes(args.index, args.fieldOffset, args.buffer, args.bufferoffset, args.length),
                    (_filter, _reader, args, _continuation) => _filter.GetBytes(_reader, args.index, args.fieldOffset, args.buffer, args.bufferoffset, args.length, (_reader, index, fieldOffset, buffer, bufferoffset, length) => _continuation(_reader, (index, fieldOffset, buffer, bufferoffset, length)))
                    );
            }
        }

        /// <summary>
        /// Invokes the GetChars method on the specified IDataReader, applying the provided filters.
        /// </summary>
        /// <param name="this">The array of <see cref="IDataReaderFilter"/> to apply.</param>
        /// <param name="reader">The <see cref="IDataReader"/> to retrieve the characters from.</param>
        /// <param name="index">The zero-based column ordinal.</param>
        /// <param name="fieldOffset">The field offset.</param>
        /// <param name="buffer">The buffer to copy data into.</param>
        /// <param name="bufferoffset">The buffer offset.</param>
        /// <param name="length">The number of characters to read.</param>
        /// <returns>The number of characters read.</returns>
        public static long GetChars(this IDataReaderFilter[] @this, IDataReader reader, int index, long fieldOffset, char[] buffer, int bufferoffset, int length)
        {
            if (@this == null || @this.Length == 0)
            {
                return reader.GetChars(index, fieldOffset, buffer, bufferoffset, length);
            }
            else
            {
                return FilterUtility.InvokeFunc(
                    @this,
                    reader,
                    (index, fieldOffset, buffer, bufferoffset, length),
                    (_reader, args) => _reader.GetChars(args.index, args.fieldOffset, args.buffer, args.bufferoffset, args.length),
                    (_filter, _reader, args, _continuation) => _filter.GetChars(_reader, args.index, args.fieldOffset, args.buffer, args.bufferoffset, args.length, (_reader, index, fieldOffset, buffer, bufferoffset, length) => _continuation(_reader, (index, fieldOffset, buffer, bufferoffset, length)))
                    );
            }
        }

        #endregion
    }
}
