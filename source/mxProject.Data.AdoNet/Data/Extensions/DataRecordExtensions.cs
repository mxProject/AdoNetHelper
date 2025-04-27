using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using System.Data;
using System.Net.Http.Headers;

namespace mxProject.Data.Extensions
{
    /// <summary>
    /// Provides extension methods for <see cref="IDataRecord"/>.
    /// </summary>
    public static class DataRecordExtensions
    {
        #region async support

        // https://github.com/dotnet/runtime/blob/main/src/libraries/System.Data.Common/src/System/Data/Common/DbDataReader.cs

        /// <summary>
        /// Asynchronously determines whether the specified column contains non-existent or missing values.
        /// </summary>
        /// <param name="this">The data record.</param>
        /// <param name="ordinal">The zero-based column ordinal.</param>
        /// <param name="cancellationToken">A token to monitor for cancellation requests.</param>
        /// <returns>A task that represents the asynchronous operation. The task result contains true if the specified column is equivalent to <see cref="DBNull"/>; otherwise, false.</returns>
        public static Task<bool> IsDBNullAsync(this IDataRecord @this, int ordinal, CancellationToken cancellationToken = default)
        {
#if NETSTANDARD2_1_OR_GREATER
            if (@this is DbDataReader reader)
            {
                return reader.IsDBNullAsync(cancellationToken);
            }
#endif

            return AsyncUtility.FuncAsync(
                (@this, ordinal),
                s => s.@this.IsDBNull(s.ordinal),
                cancellationToken
                );
        }

        /// <summary>
        /// Asynchronously retrieves the value of the specified column as an object.
        /// </summary>
        /// <param name="this">The data record.</param>
        /// <param name="ordinal">The zero-based column ordinal.</param>
        /// <param name="cancellationToken">A token to monitor for cancellation requests.</param>
        /// <returns>A value task that represents the asynchronous operation. The task result contains the value of the specified column.</returns>
        public static ValueTask<object> GetValueAsync(this IDataRecord @this, int ordinal, CancellationToken cancellationToken = default)
        {
            return AsyncUtility.FuncAsyncAsValueTask(
                (@this, ordinal),
                s => s.@this.GetValue(s.ordinal),
                cancellationToken
                );
        }

        /// <summary>
        /// Asynchronously populates an array of objects with the column values of the current record.
        /// </summary>
        /// <param name="this">The data record.</param>
        /// <param name="values">An array of objects to copy the attribute fields into.</param>
        /// <param name="cancellationToken">A token to monitor for cancellation requests.</param>
        /// <returns>A value task that represents the asynchronous operation. The task result contains the number of instances of <see cref="Object"/> in the array.</returns>
        public static ValueTask<int> GetValuesAsync(this IDataRecord @this, object[] values, CancellationToken cancellationToken = default)
        {
            return AsyncUtility.FuncAsyncAsValueTask(
                (@this, values),
                s => s.@this.GetValues(s.values),
                cancellationToken
                );
        }

        #endregion

        #region IsDBNull(name)

        /// <summary>
        /// Determines whether the specified column contains non-existent or missing values.
        /// </summary>
        /// <param name="this">The data record.</param>
        /// <param name="name">The name of the column.</param>
        /// <returns>True if the specified column is equivalent to <see cref="DBNull"/>; otherwise, false.</returns>
        public static bool IsDBNull(this IDataRecord @this, string name)
        {
            return @this.IsDBNull(GetAndAssertOrdinal(@this, name));
        }

        /// <summary>
        /// Determines whether the specified column contains non-existent or missing values.
        /// </summary>
        /// <param name="this">The data record.</param>
        /// <param name="name">The name of the column.</param>
        /// <param name="cancellationToken">A token to monitor for cancellation requests.</param>
        /// <returns>True if the specified column is equivalent to <see cref="DBNull"/>; otherwise, false.</returns>
        public static ValueTask<bool> IsDBNullAsync(this IDataRecord @this, string name, CancellationToken cancellationToken = default)
        {
            return AsyncUtility.FuncAsyncAsValueTask(
                (@this, name),
                s => s.@this.IsDBNull(s.name),
                cancellationToken
                );
        }

        #endregion

        #region GetValue(name)

        /// <summary>
        /// Gets the value of the specified column as a boolean.
        /// </summary>
        /// <param name="this">The data record.</param>
        /// <param name="name">The name of the column.</param>
        /// <returns>The value of the column as a boolean.</returns>
        public static bool GetBoolean(this IDataRecord @this, string name)
        {
            return @this.GetBoolean(GetAndAssertOrdinal(@this, name));
        }

        /// <summary>
        /// Gets the value of the specified column as a byte.
        /// </summary>
        /// <param name="this">The data record.</param>
        /// <param name="name">The name of the column.</param>
        /// <returns>The value of the column as a byte.</returns>
        public static byte GetByte(this IDataRecord @this, string name)
        {
            return @this.GetByte(GetAndAssertOrdinal(@this, name));
        }

        /// <summary>
        /// Gets the value of the specified column as a 16-bit integer.
        /// </summary>
        /// <param name="this">The data record.</param>
        /// <param name="name">The name of the column.</param>
        /// <returns>The value of the column as a 16-bit integer.</returns>
        public static short GetInt16(this IDataRecord @this, string name)
        {
            return @this.GetInt16(GetAndAssertOrdinal(@this, name));
        }

        /// <summary>
        /// Gets the value of the specified column as a 32-bit integer.
        /// </summary>
        /// <param name="this">The data record.</param>
        /// <param name="name">The name of the column.</param>
        /// <returns>The value of the column as a 32-bit integer.</returns>
        public static int GetInt32(this IDataRecord @this, string name)
        {
            return @this.GetInt32(GetAndAssertOrdinal(@this, name));
        }

        /// <summary>
        /// Gets the value of the specified column as a 64-bit integer.
        /// </summary>
        /// <param name="this">The data record.</param>
        /// <param name="name">The name of the column.</param>
        /// <returns>The value of the column as a 64-bit integer.</returns>
        public static long GetInt64(this IDataRecord @this, string name)
        {
            return @this.GetInt64(GetAndAssertOrdinal(@this, name));
        }

        /// <summary>
        /// Gets the value of the specified column as a single-precision floating-point number.
        /// </summary>
        /// <param name="this">The data record.</param>
        /// <param name="name">The name of the column.</param>
        /// <returns>The value of the column as a single-precision floating-point number.</returns>
        public static float GetFloat(this IDataRecord @this, string name)
        {
            return @this.GetFloat(GetAndAssertOrdinal(@this, name));
        }

        /// <summary>
        /// Gets the value of the specified column as a double-precision floating-point number.
        /// </summary>
        /// <param name="this">The data record.</param>
        /// <param name="name">The name of the column.</param>
        /// <returns>The value of the column as a double-precision floating-point number.</returns>
        public static double GetDouble(this IDataRecord @this, string name)
        {
            return @this.GetDouble(GetAndAssertOrdinal(@this, name));
        }

        /// <summary>
        /// Gets the value of the specified column as a decimal.
        /// </summary>
        /// <param name="this">The data record.</param>
        /// <param name="name">The name of the column.</param>
        /// <returns>The value of the column as a decimal.</returns>
        public static decimal GetDecimal(this IDataRecord @this, string name)
        {
            return @this.GetDecimal(GetAndAssertOrdinal(@this, name));
        }

        /// <summary>
        /// Gets the value of the specified column as a DateTime.
        /// </summary>
        /// <param name="this">The data record.</param>
        /// <param name="name">The name of the column.</param>
        /// <returns>The value of the column as a DateTime.</returns>
        public static DateTime GetDateTime(this IDataRecord @this, string name)
        {
            return @this.GetDateTime(GetAndAssertOrdinal(@this, name));
        }

        /// <summary>
        /// Gets the value of the specified column as a char.
        /// </summary>
        /// <param name="this">The data record.</param>
        /// <param name="name">The name of the column.</param>
        /// <returns>The value of the column as a char.</returns>
        public static char GetChar(this IDataRecord @this, string name)
        {
            return @this.GetChar(GetAndAssertOrdinal(@this, name));
        }

        /// <summary>
        /// Gets the value of the specified column as a string.
        /// </summary>
        /// <param name="this">The data record.</param>
        /// <param name="name">The name of the column.</param>
        /// <returns>The value of the column as a string.</returns>
        public static string GetString(this IDataRecord @this, string name)
        {
            return @this.GetString(GetAndAssertOrdinal(@this, name));
        }

        /// <summary>
        /// Gets the value of the specified column as a Guid.
        /// </summary>
        /// <param name="this">The data record.</param>
        /// <param name="name">The name of the column.</param>
        /// <returns>The value of the column as a Guid.</returns>
        public static Guid GetGuid(this IDataRecord @this, string name)
        {
            return @this.GetGuid(GetAndAssertOrdinal(@this, name));
        }

        /// <summary>
        /// Reads a stream of bytes from the specified column offset into the buffer as an array.
        /// </summary>
        /// <param name="this">The data record.</param>
        /// <param name="name">The name of the column.</param>
        /// <param name="dataOffset">The index within the field from which to begin the read operation.</param>
        /// <param name="buffer">The buffer into which to copy data.</param>
        /// <param name="bufferOffset">The index for buffer to begin the read operation.</param>
        /// <param name="length">The maximum length to copy into the buffer.</param>
        /// <returns>The actual number of bytes read.</returns>
        public static long GetBytes(this IDataRecord @this, string name, long dataOffset, byte[]? buffer, int bufferOffset, int length)
        {
            return @this.GetBytes(GetAndAssertOrdinal(@this, name), dataOffset, buffer, bufferOffset, length);
        }

        /// <summary>
        /// Reads a stream of characters from the specified column offset into the buffer as an array.
        /// </summary>
        /// <param name="this">The data record.</param>
        /// <param name="name">The name of the column.</param>
        /// <param name="dataOffset">The index within the field from which to begin the read operation.</param>
        /// <param name="buffer">The buffer into which to copy data.</param>
        /// <param name="bufferOffset">The index for buffer to begin the read operation.</param>
        /// <param name="length">The maximum length to copy into the buffer.</param>
        /// <returns>The actual number of characters read.</returns>
        public static long GetChars(this IDataRecord @this, string name, long dataOffset, char[]? buffer, int bufferOffset, int length)
        {
            return @this.GetChars(GetAndAssertOrdinal(@this, name), dataOffset, buffer, bufferOffset, length);
        }

        /// <summary>
        /// Gets the zero-based column ordinal for the specified column name and ensures it is valid.
        /// </summary>
        /// <param name="this">The data record.</param>
        /// <param name="name">The name of the column.</param>
        /// <returns>The zero-based column ordinal.</returns>
        /// <exception cref="ArgumentOutOfRangeException">Thrown when the column name is not found.</exception>
        private static int GetAndAssertOrdinal(IDataRecord @this, string name)
        {
            var ordinal = @this.GetOrdinal(name);

            if (ordinal < 0)
            {
                throw new ArgumentOutOfRangeException(nameof(name), $"Column '{name}' not found.");
            }

            return ordinal;
        }

        #endregion

        #region GetValueAsync

        /// <summary>
        /// Asynchronously gets the value of the specified column as a boolean.
        /// </summary>
        /// <param name="this">The data record.</param>
        /// <param name="ordinal">The zero-based column ordinal.</param>
        /// <param name="cancellationToken">A token to monitor for cancellation requests.</param>
        /// <returns>The value of the column as a boolean.</returns>
        public static ValueTask<bool> GetBooleanAsync(this IDataRecord @this, int ordinal, CancellationToken cancellationToken = default)
        {
            return AsyncUtility.FuncAsyncAsValueTask(
                (@this, ordinal),
                s => s.@this.GetBoolean(s.ordinal),
                cancellationToken
                );
        }

        /// <summary>
        /// Asynchronously gets the value of the specified column as a byte.
        /// </summary>
        /// <param name="this">The data record.</param>
        /// <param name="ordinal">The zero-based column ordinal.</param>
        /// <param name="cancellationToken">A token to monitor for cancellation requests.</param>
        /// <returns>The value of the column as a byte.</returns>
        public static ValueTask<byte> GetByteAsync(this IDataRecord @this, int ordinal, CancellationToken cancellationToken = default)
        {
            return AsyncUtility.FuncAsyncAsValueTask(
                (@this, ordinal),
                s => s.@this.GetByte(s.ordinal),
                cancellationToken
                );
        }

        /// <summary>
        /// Asynchronously gets the value of the specified column as a 16-bit integer.
        /// </summary>
        /// <param name="this">The data record.</param>
        /// <param name="ordinal">The zero-based column ordinal.</param>
        /// <param name="cancellationToken">A token to monitor for cancellation requests.</param>
        /// <returns>The value of the column as a 16-bit integer.</returns>
        public static ValueTask<short> GetInt16Async(this IDataRecord @this, int ordinal, CancellationToken cancellationToken = default)
        {
            return AsyncUtility.FuncAsyncAsValueTask(
                (@this, ordinal),
                s => s.@this.GetInt16(s.ordinal),
                cancellationToken
                );
        }

        /// <summary>
        /// Asynchronously gets the value of the specified column as a 32-bit integer.
        /// </summary>
        /// <param name="this">The data record.</param>
        /// <param name="ordinal">The zero-based column ordinal.</param>
        /// <param name="cancellationToken">A token to monitor for cancellation requests.</param>
        /// <returns>The value of the column as a 32-bit integer.</returns>
        public static ValueTask<int> GetInt32Async(this IDataRecord @this, int ordinal, CancellationToken cancellationToken = default)
        {
            return AsyncUtility.FuncAsyncAsValueTask(
                (@this, ordinal),
                s => s.@this.GetInt32(s.ordinal),
                cancellationToken
                );
        }

        /// <summary>
        /// Asynchronously gets the value of the specified column as a 64-bit integer.
        /// </summary>
        /// <param name="this">The data record.</param>
        /// <param name="ordinal">The zero-based column ordinal.</param>
        /// <param name="cancellationToken">A token to monitor for cancellation requests.</param>
        /// <returns>The value of the column as a 64-bit integer.</returns>
        public static ValueTask<long> GetInt64Async(this IDataRecord @this, int ordinal, CancellationToken cancellationToken = default)
        {
            return AsyncUtility.FuncAsyncAsValueTask(
                (@this, ordinal),
                s => s.@this.GetInt64(s.ordinal),
                cancellationToken
                );
        }

        /// <summary>
        /// Asynchronously gets the value of the specified column as a single-precision floating-point number.
        /// </summary>
        /// <param name="this">The data record.</param>
        /// <param name="ordinal">The zero-based column ordinal.</param>
        /// <param name="cancellationToken">A token to monitor for cancellation requests.</param>
        /// <returns>The value of the column as a single-precision floating-point number.</returns>
        public static ValueTask<float> GetFloatAsync(this IDataRecord @this, int ordinal, CancellationToken cancellationToken = default)
        {
            return AsyncUtility.FuncAsyncAsValueTask(
                (@this, ordinal),
                s => s.@this.GetFloat(s.ordinal),
                cancellationToken
                );
        }

        /// <summary>
        /// Asynchronously gets the value of the specified column as a double-precision floating-point number.
        /// </summary>
        /// <param name="this">The data record.</param>
        /// <param name="ordinal">The zero-based column ordinal.</param>
        /// <param name="cancellationToken">A token to monitor for cancellation requests.</param>
        /// <returns>The value of the column as a double-precision floating-point number.</returns>
        public static ValueTask<double> GetDoubleAsync(this IDataRecord @this, int ordinal, CancellationToken cancellationToken = default)
        {
            return AsyncUtility.FuncAsyncAsValueTask(
                (@this, ordinal),
                s => s.@this.GetDouble(s.ordinal),
                cancellationToken
                );
        }

        /// <summary>
        /// Asynchronously gets the value of the specified column as a decimal.
        /// </summary>
        /// <param name="this">The data record.</param>
        /// <param name="ordinal">The zero-based column ordinal.</param>
        /// <param name="cancellationToken">A token to monitor for cancellation requests.</param>
        /// <returns>The value of the column as a decimal.</returns>
        public static ValueTask<decimal> GetDecimalAsync(this IDataRecord @this, int ordinal, CancellationToken cancellationToken = default)
        {
            return AsyncUtility.FuncAsyncAsValueTask(
                (@this, ordinal),
                s => s.@this.GetDecimal(s.ordinal),
                cancellationToken
                );
        }

        /// <summary>
        /// Asynchronously gets the value of the specified column as a DateTime.
        /// </summary>
        /// <param name="this">The data record.</param>
        /// <param name="ordinal">The zero-based column ordinal.</param>
        /// <param name="cancellationToken">A token to monitor for cancellation requests.</param>
        /// <returns>The value of the column as a DateTime.</returns>
        public static ValueTask<DateTime> GetDateTimeAsync(this IDataRecord @this, int ordinal, CancellationToken cancellationToken = default)
        {
            return AsyncUtility.FuncAsyncAsValueTask(
                (@this, ordinal),
                s => s.@this.GetDateTime(s.ordinal),
                cancellationToken
                );
        }

        /// <summary>
        /// Asynchronously gets the value of the specified column as a char.
        /// </summary>
        /// <param name="this">The data record.</param>
        /// <param name="ordinal">The zero-based column ordinal.</param>
        /// <param name="cancellationToken">A token to monitor for cancellation requests.</param>
        /// <returns>The value of the column as a char.</returns>
        public static ValueTask<char> GetCharAsync(this IDataRecord @this, int ordinal, CancellationToken cancellationToken = default)
        {
            return AsyncUtility.FuncAsyncAsValueTask(
                (@this, ordinal),
                s => s.@this.GetChar(s.ordinal),
                cancellationToken
                );
        }

        /// <summary>
        /// Asynchronously gets the value of the specified column as a string.
        /// </summary>
        /// <param name="this">The data record.</param>
        /// <param name="ordinal">The zero-based column ordinal.</param>
        /// <param name="cancellationToken">A token to monitor for cancellation requests.</param>
        /// <returns>The value of the column as a string.</returns>
        public static ValueTask<string> GetStringAsync(this IDataRecord @this, int ordinal, CancellationToken cancellationToken = default)
        {
            return AsyncUtility.FuncAsyncAsValueTask(
                (@this, ordinal),
                s => s.@this.GetString(s.ordinal),
                cancellationToken
                );
        }

        /// <summary>
        /// Asynchronously gets the value of the specified column as a Guid.
        /// </summary>
        /// <param name="this">The data record.</param>
        /// <param name="ordinal">The zero-based column ordinal.</param>
        /// <param name="cancellationToken">A token to monitor for cancellation requests.</param>
        /// <returns>The value of the column as a Guid.</returns>
        public static ValueTask<Guid> GetGuidAsync(this IDataRecord @this, int ordinal, CancellationToken cancellationToken = default)
        {
            return AsyncUtility.FuncAsyncAsValueTask(
                (@this, ordinal),
                s => s.@this.GetGuid(s.ordinal),
                cancellationToken
                );
        }

        /// <summary>
        /// Reads a stream of bytes from the specified column offset into the buffer as an array.
        /// </summary>
        /// <param name="this">The data record.</param>
        /// <param name="ordinal">The zero-based column ordinal.</param>
        /// <param name="dataOffset">The index within the field from which to begin the read operation.</param>
        /// <param name="buffer">The buffer into which to copy data.</param>
        /// <param name="bufferOffset">The index for buffer to begin the read operation.</param>
        /// <param name="length">The maximum length to copy into the buffer.</param>
        /// <param name="cancellationToken">A token to monitor for cancellation requests.</param>
        /// <returns>The actual number of bytes read.</returns>
        public static ValueTask<long> GetBytesAsync(this IDataRecord @this, int ordinal, long dataOffset, byte[]? buffer, int bufferOffset, int length, CancellationToken cancellationToken = default)
        {
            return AsyncUtility.FuncAsyncAsValueTask(
                (@this, ordinal, dataOffset, buffer, bufferOffset, length),
                s => s.@this.GetBytes(s.ordinal, s.dataOffset, s.buffer, s.bufferOffset, s.length),
                cancellationToken
                );
        }

        /// <summary>
        /// Reads a stream of characters from the specified column offset into the buffer as an array.
        /// </summary>
        /// <param name="this">The data record.</param>
        /// <param name="ordinal">The zero-based column ordinal.</param>
        /// <param name="dataOffset">The index within the field from which to begin the read operation.</param>
        /// <param name="buffer">The buffer into which to copy data.</param>
        /// <param name="bufferOffset">The index for buffer to begin the read operation.</param>
        /// <param name="length">The maximum length to copy into the buffer.</param>
        /// <param name="cancellationToken">A token to monitor for cancellation requests.</param>
        /// <returns>The actual number of characters read.</returns>
        public static ValueTask<long> GetCharsAsync(this IDataRecord @this, int ordinal, long dataOffset, char[]? buffer, int bufferOffset, int length, CancellationToken cancellationToken = default)
        {
            return AsyncUtility.FuncAsyncAsValueTask(
                (@this, ordinal, dataOffset, buffer, bufferOffset, length),
                s => s.@this.GetChars(s.ordinal, s.dataOffset, s.buffer, s.bufferOffset, s.length),
                cancellationToken
                );
        }

        /// <summary>
        /// Asynchronously gets the value of the specified column as a boolean.
        /// </summary>
        /// <param name="this">The data record.</param>
        /// <param name="name">The name of the column.</param>
        /// <param name="cancellationToken">A token to monitor for cancellation requests.</param>
        /// <returns>The value of the column as a boolean.</returns>
        public static ValueTask<bool> GetBooleanAsync(this IDataRecord @this, string name, CancellationToken cancellationToken = default)
        {
            return AsyncUtility.FuncAsyncAsValueTask(
                (@this, name),
                s => s.@this.GetBoolean(s.name),
                cancellationToken
                );
        }

        /// <summary>
        /// Asynchronously gets the value of the specified column as a byte.
        /// </summary>
        /// <param name="this">The data record.</param>
        /// <param name="name">The name of the column.</param>
        /// <param name="cancellationToken">A token to monitor for cancellation requests.</param>
        /// <returns>The value of the column as a byte.</returns>
        public static ValueTask<byte> GetByteAsync(this IDataRecord @this, string name, CancellationToken cancellationToken = default)
        {
            return AsyncUtility.FuncAsyncAsValueTask(
                (@this, name),
                s => s.@this.GetByte(s.name),
                cancellationToken
                );
        }

        /// <summary>
        /// Asynchronously gets the value of the specified column as a 16-bit integer.
        /// </summary>
        /// <param name="this">The data record.</param>
        /// <param name="name">The name of the column.</param>
        /// <param name="cancellationToken">A token to monitor for cancellation requests.</param>
        /// <returns>The value of the column as a 16-bit integer.</returns>
        public static ValueTask<short> GetInt16Async(this IDataRecord @this, string name, CancellationToken cancellationToken = default)
        {
            return AsyncUtility.FuncAsyncAsValueTask(
                (@this, name),
                s => s.@this.GetInt16(s.name),
                cancellationToken
                );
        }

        /// <summary>
        /// Asynchronously gets the value of the specified column as a 32-bit integer.
        /// </summary>
        /// <param name="this">The data record.</param>
        /// <param name="name">The name of the column.</param>
        /// <param name="cancellationToken">A token to monitor for cancellation requests.</param>
        /// <returns>The value of the column as a 32-bit integer.</returns>
        public static ValueTask<int> GetInt32Async(this IDataRecord @this, string name, CancellationToken cancellationToken = default)
        {
            return AsyncUtility.FuncAsyncAsValueTask(
                (@this, name),
                s => s.@this.GetInt32(s.name),
                cancellationToken
                );
        }

        /// <summary>
        /// Asynchronously gets the value of the specified column as a 64-bit integer.
        /// </summary>
        /// <param name="this">The data record.</param>
        /// <param name="name">The name of the column.</param>
        /// <param name="cancellationToken">A token to monitor for cancellation requests.</param>
        /// <returns>The value of the column as a 64-bit integer.</returns>
        public static ValueTask<long> GetInt64Async(this IDataRecord @this, string name, CancellationToken cancellationToken = default)
        {
            return AsyncUtility.FuncAsyncAsValueTask(
                (@this, name),
                s => s.@this.GetInt64(s.name),
                cancellationToken
                );
        }

        /// <summary>
        /// Asynchronously gets the value of the specified column as a single-precision floating-point number.
        /// </summary>
        /// <param name="this">The data record.</param>
        /// <param name="name">The name of the column.</param>
        /// <param name="cancellationToken">A token to monitor for cancellation requests.</param>
        /// <returns>The value of the column as a single-precision floating-point number.</returns>
        public static ValueTask<float> GetFloatAsync(this IDataRecord @this, string name, CancellationToken cancellationToken = default)
        {
            return AsyncUtility.FuncAsyncAsValueTask(
                (@this, name),
                s => s.@this.GetFloat(s.name),
                cancellationToken
                );
        }

        /// <summary>
        /// Asynchronously gets the value of the specified column as a double-precision floating-point number.
        /// </summary>
        /// <param name="this">The data record.</param>
        /// <param name="name">The name of the column.</param>
        /// <param name="cancellationToken">A token to monitor for cancellation requests.</param>
        /// <returns>The value of the column as a double-precision floating-point number.</returns>
        public static ValueTask<double> GetDoubleAsync(this IDataRecord @this, string name, CancellationToken cancellationToken = default)
        {
            return AsyncUtility.FuncAsyncAsValueTask(
                (@this, name),
                s => s.@this.GetDouble(s.name),
                cancellationToken
                );
        }

        /// <summary>
        /// Asynchronously gets the value of the specified column as a decimal.
        /// </summary>
        /// <param name="this">The data record.</param>
        /// <param name="name">The name of the column.</param>
        /// <param name="cancellationToken">A token to monitor for cancellation requests.</param>
        /// <returns>The value of the column as a decimal.</returns>
        public static ValueTask<decimal> GetDecimalAsync(this IDataRecord @this, string name, CancellationToken cancellationToken = default)
        {
            return AsyncUtility.FuncAsyncAsValueTask(
                (@this, name),
                s => s.@this.GetDecimal(s.name),
                cancellationToken
                );
        }

        /// <summary>
        /// Asynchronously gets the value of the specified column as a DateTime.
        /// </summary>
        /// <param name="this">The data record.</param>
        /// <param name="name">The name of the column.</param>
        /// <param name="cancellationToken">A token to monitor for cancellation requests.</param>
        /// <returns>The value of the column as a DateTime.</returns>
        public static ValueTask<DateTime> GetDateTimeAsync(this IDataRecord @this, string name, CancellationToken cancellationToken = default)
        {
            return AsyncUtility.FuncAsyncAsValueTask(
                (@this, name),
                s => s.@this.GetDateTime(s.name),
                cancellationToken
                );
        }

        /// <summary>
        /// Asynchronously gets the value of the specified column as a char.
        /// </summary>
        /// <param name="this">The data record.</param>
        /// <param name="name">The name of the column.</param>
        /// <param name="cancellationToken">A token to monitor for cancellation requests.</param>
        /// <returns>The value of the column as a char.</returns>
        public static ValueTask<char> GetCharAsync(this IDataRecord @this, string name, CancellationToken cancellationToken = default)
        {
            return AsyncUtility.FuncAsyncAsValueTask(
                (@this, name),
                s => s.@this.GetChar(s.name),
                cancellationToken
                );
        }

        /// <summary>
        /// Asynchronously gets the value of the specified column as a string.
        /// </summary>
        /// <param name="this">The data record.</param>
        /// <param name="name">The name of the column.</param>
        /// <param name="cancellationToken">A token to monitor for cancellation requests.</param>
        /// <returns>The value of the column as a string.</returns>
        public static ValueTask<string> GetStringAsync(this IDataRecord @this, string name, CancellationToken cancellationToken = default)
        {
            return AsyncUtility.FuncAsyncAsValueTask(
                (@this, name),
                s => s.@this.GetString(s.name),
                cancellationToken
                );
        }

        /// <summary>
        /// Asynchronously gets the value of the specified column as a Guid.
        /// </summary>
        /// <param name="this">The data record.</param>
        /// <param name="name">The name of the column.</param>
        /// <param name="cancellationToken">A token to monitor for cancellation requests.</param>
        /// <returns>The value of the column as a Guid.</returns>
        public static ValueTask<Guid> GetGuidAsync(this IDataRecord @this, string name, CancellationToken cancellationToken = default)
        {
            return AsyncUtility.FuncAsyncAsValueTask(
                (@this, name),
                s => s.@this.GetGuid(s.name),
                cancellationToken
                );
        }

        /// <summary>
        /// Reads a stream of bytes from the specified column offset into the buffer as an array.
        /// </summary>
        /// <param name="this">The data record.</param>
        /// <param name="name">The name of the column.</param>
        /// <param name="dataOffset">The index within the field from which to begin the read operation.</param>
        /// <param name="buffer">The buffer into which to copy data.</param>
        /// <param name="bufferOffset">The index for buffer to begin the read operation.</param>
        /// <param name="length">The maximum length to copy into the buffer.</param>
        /// <param name="cancellationToken">A token to monitor for cancellation requests.</param>
        /// <returns>The actual number of bytes read.</returns>
        public static ValueTask<long> GetBytesAsync(this IDataRecord @this, string name, long dataOffset, byte[]? buffer, int bufferOffset, int length, CancellationToken cancellationToken = default)
        {
            return AsyncUtility.FuncAsyncAsValueTask(
                (@this, name, dataOffset, buffer, bufferOffset, length),
                s => s.@this.GetBytes(s.name, s.dataOffset, s.buffer, s.bufferOffset, s.length),
                cancellationToken
                );
        }

        /// <summary>
        /// Reads a stream of characters from the specified column offset into the buffer as an array.
        /// </summary>
        /// <param name="this">The data record.</param>
        /// <param name="name">The name of the column.</param>
        /// <param name="dataOffset">The index within the field from which to begin the read operation.</param>
        /// <param name="buffer">The buffer into which to copy data.</param>
        /// <param name="bufferOffset">The index for buffer to begin the read operation.</param>
        /// <param name="length">The maximum length to copy into the buffer.</param>
        /// <param name="cancellationToken">A token to monitor for cancellation requests.</param>
        /// <returns>The actual number of characters read.</returns>
        public static ValueTask<long> GetCharsAsync(this IDataRecord @this, string name, long dataOffset, char[]? buffer, int bufferOffset, int length, CancellationToken cancellationToken = default)
        {
            return AsyncUtility.FuncAsyncAsValueTask(
                (@this, name, dataOffset, buffer, bufferOffset, length),
                s => s.@this.GetChars(s.name, s.dataOffset, s.buffer, s.bufferOffset, s.length),
                cancellationToken
                );
        }

        #endregion

        #region GetValueOrNull

        /// <summary>
        /// Gets the value of the specified column as a nullable boolean.
        /// </summary>
        /// <param name="this">The data record.</param>
        /// <param name="ordinal">The zero-based column ordinal.</param>
        /// <returns>The value of the column as a nullable boolean, or null if the column is <see cref="DBNull"/>.</returns>
        public static bool? GetBooleanOrNull(this IDataRecord @this, int ordinal)
        {
            if (@this.IsDBNull(ordinal)) { return null; }
            return @this.GetBoolean(ordinal);
        }

        /// <summary>
        /// Gets the value of the specified column as a nullable boolean.
        /// </summary>
        /// <param name="this">The data record.</param>
        /// <param name="name">The name of the column.</param>
        /// <returns>The value of the column as a nullable boolean, or null if the column is <see cref="DBNull"/>.</returns>
        public static bool? GetBooleanOrNull(this IDataRecord @this, string name)
        {
            return @this.GetBooleanOrNull(GetAndAssertOrdinal(@this, name));
        }

        /// <summary>
        /// Gets the value of the specified column as a nullable byte.
        /// </summary>
        /// <param name="this">The data record.</param>
        /// <param name="ordinal">The zero-based column ordinal.</param>
        /// <returns>The value of the column as a nullable byte, or null if the column is <see cref="DBNull"/>.</returns>
        public static byte? GetByteOrNull(this IDataRecord @this, int ordinal)
        {
            if (@this.IsDBNull(ordinal)) { return null; }
            return @this.GetByte(ordinal);
        }

        /// <summary>
        /// Gets the value of the specified column as a nullable byte.
        /// </summary>
        /// <param name="this">The data record.</param>
        /// <param name="name">The name of the column.</param>
        /// <returns>The value of the column as a nullable byte, or null if the column is <see cref="DBNull"/>.</returns>
        public static byte? GetByteOrNull(this IDataRecord @this, string name)
        {
            return @this.GetByteOrNull(GetAndAssertOrdinal(@this, name));
        }

        /// <summary>
        /// Gets the value of the specified column as a nullable 16-bit integer.
        /// </summary>
        /// <param name="this">The data record.</param>
        /// <param name="ordinal">The zero-based column ordinal.</param>
        /// <returns>The value of the column as a nullable 16-bit integer, or null if the column is <see cref="DBNull"/>.</returns>
        public static short? GetInt16OrNull(this IDataRecord @this, int ordinal)
        {
            if (@this.IsDBNull(ordinal)) { return null; }
            return @this.GetInt16(ordinal);
        }

        /// <summary>
        /// Gets the value of the specified column as a nullable 16-bit integer.
        /// </summary>
        /// <param name="this">The data record.</param>
        /// <param name="name">The name of the column.</param>
        /// <returns>The value of the column as a nullable 16-bit integer, or null if the column is <see cref="DBNull"/>.</returns>
        public static short? GetInt16OrNull(this IDataRecord @this, string name)
        {
            return @this.GetInt16OrNull(GetAndAssertOrdinal(@this, name));
        }

        /// <summary>
        /// Gets the value of the specified column as a nullable 32-bit integer.
        /// </summary>
        /// <param name="this">The data record.</param>
        /// <param name="ordinal">The zero-based column ordinal.</param>
        /// <returns>The value of the column as a nullable 32-bit integer, or null if the column is <see cref="DBNull"/>.</returns>
        public static int? GetInt32OrNull(this IDataRecord @this, int ordinal)
        {
            if (@this.IsDBNull(ordinal)) { return null; }
            return @this.GetInt32(ordinal);
        }

        /// <summary>
        /// Gets the value of the specified column as a nullable 32-bit integer.
        /// </summary>
        /// <param name="this">The data record.</param>
        /// <param name="name">The name of the column.</param>
        /// <returns>The value of the column as a nullable 32-bit integer, or null if the column is <see cref="DBNull"/>.</returns>
        public static int? GetInt32OrNull(this IDataRecord @this, string name)
        {
            return @this.GetInt32OrNull(GetAndAssertOrdinal(@this, name));
        }

        /// <summary>
        /// Gets the value of the specified column as a nullable 64-bit integer.
        /// </summary>
        /// <param name="this">The data record.</param>
        /// <param name="ordinal">The zero-based column ordinal.</param>
        /// <returns>The value of the column as a nullable 64-bit integer, or null if the column is <see cref="DBNull"/>.</returns>
        public static long? GetInt64OrNull(this IDataRecord @this, int ordinal)
        {
            if (@this.IsDBNull(ordinal)) { return null; }
            return @this.GetInt64(ordinal);
        }

        /// <summary>
        /// Gets the value of the specified column as a nullable 64-bit integer.
        /// </summary>
        /// <param name="this">The data record.</param>
        /// <param name="name">The name of the column.</param>
        /// <returns>The value of the column as a nullable 64-bit integer, or null if the column is <see cref="DBNull"/>.</returns>
        public static long? GetInt64OrNull(this IDataRecord @this, string name)
        {
            return @this.GetInt64OrNull(GetAndAssertOrdinal(@this, name));
        }

        /// <summary>
        /// Gets the value of the specified column as a nullable single-precision floating-point number.
        /// </summary>
        /// <param name="this">The data record.</param>
        /// <param name="ordinal">The zero-based column ordinal.</param>
        /// <returns>The value of the column as a nullable single-precision floating-point number, or null if the column is <see cref="DBNull"/>.</returns>
        public static float? GetFloatOrNull(this IDataRecord @this, int ordinal)
        {
            if (@this.IsDBNull(ordinal)) { return null; }
            return @this.GetFloat(ordinal);
        }

        /// <summary>
        /// Gets the value of the specified column as a nullable single-precision floating-point number.
        /// </summary>
        /// <param name="this">The data record.</param>
        /// <param name="name">The name of the column.</param>
        /// <returns>The value of the column as a nullable single-precision floating-point number, or null if the column is <see cref="DBNull"/>.</returns>
        public static float? GetFloatOrNull(this IDataRecord @this, string name)
        {
            return @this.GetFloatOrNull(GetAndAssertOrdinal(@this, name));
        }

        /// <summary>
        /// Gets the value of the specified column as a nullable double-precision floating-point number.
        /// </summary>
        /// <param name="this">The data record.</param>
        /// <param name="ordinal">The zero-based column ordinal.</param>
        /// <returns>The value of the column as a nullable double-precision floating-point number, or null if the column is <see cref="DBNull"/>.</returns>
        public static double? GetDoubleOrNull(this IDataRecord @this, int ordinal)
        {
            if (@this.IsDBNull(ordinal)) { return null; }
            return @this.GetDouble(ordinal);
        }

        /// <summary>
        /// Gets the value of the specified column as a nullable double-precision floating-point number.
        /// </summary>
        /// <param name="this">The data record.</param>
        /// <param name="name">The name of the column.</param>
        /// <returns>The value of the column as a nullable double-precision floating-point number, or null if the column is <see cref="DBNull"/>.</returns>
        public static double? GetDoubleOrNull(this IDataRecord @this, string name)
        {
            return @this.GetDoubleOrNull(GetAndAssertOrdinal(@this, name));
        }

        /// <summary>
        /// Gets the value of the specified column as a nullable decimal.
        /// </summary>
        /// <param name="this">The data record.</param>
        /// <param name="ordinal">The zero-based column ordinal.</param>
        /// <returns>The value of the column as a nullable decimal, or null if the column is <see cref="DBNull"/>.</returns>
        public static decimal? GetDecimalOrNull(this IDataRecord @this, int ordinal)
        {
            if (@this.IsDBNull(ordinal)) { return null; }
            return @this.GetDecimal(ordinal);
        }

        /// <summary>
        /// Gets the value of the specified column as a nullable decimal.
        /// </summary>
        /// <param name="this">The data record.</param>
        /// <param name="name">The name of the column.</param>
        /// <returns>The value of the column as a nullable decimal, or null if the column is <see cref="DBNull"/>.</returns>
        public static decimal? GetDecimalOrNull(this IDataRecord @this, string name)
        {
            return @this.GetDecimalOrNull(GetAndAssertOrdinal(@this, name));
        }

        /// <summary>
        /// Gets the value of the specified column as a nullable DateTime.
        /// </summary>
        /// <param name="this">The data record.</param>
        /// <param name="ordinal">The zero-based column ordinal.</param>
        /// <returns>The value of the column as a nullable DateTime, or null if the column is <see cref="DBNull"/>.</returns>
        public static DateTime? GetDateTimeOrNull(this IDataRecord @this, int ordinal)
        {
            if (@this.IsDBNull(ordinal)) { return null; }
            return @this.GetDateTime(ordinal);
        }

        /// <summary>
        /// Gets the value of the specified column as a nullable DateTime.
        /// </summary>
        /// <param name="this">The data record.</param>
        /// <param name="name">The name of the column.</param>
        /// <returns>The value of the column as a nullable DateTime, or null if the column is <see cref="DBNull"/>.</returns>
        public static DateTime? GetDateTimeOrNull(this IDataRecord @this, string name)
        {
            return @this.GetDateTimeOrNull(GetAndAssertOrdinal(@this, name));
        }

        /// <summary>
        /// Gets the value of the specified column as a nullable char.
        /// </summary>
        /// <param name="this">The data record.</param>
        /// <param name="ordinal">The zero-based column ordinal.</param>
        /// <returns>The value of the column as a nullable char, or null if the column is <see cref="DBNull"/>.</returns>
        public static char? GetCharOrNull(this IDataRecord @this, int ordinal)
        {
            if (@this.IsDBNull(ordinal)) { return null; }
            return @this.GetChar(ordinal);
        }

        /// <summary>
        /// Gets the value of the specified column as a nullable char.
        /// </summary>
        /// <param name="this">The data record.</param>
        /// <param name="name">The name of the column.</param>
        /// <returns>The value of the column as a nullable char, or null if the column is <see cref="DBNull"/>.</returns>
        public static char? GetCharOrNull(this IDataRecord @this, string name)
        {
            return @this.GetCharOrNull(GetAndAssertOrdinal(@this, name));
        }

        /// <summary>
        /// Gets the value of the specified column as a nullable string.
        /// </summary>
        /// <param name="this">The data record.</param>
        /// <param name="ordinal">The zero-based column ordinal.</param>
        /// <returns>The value of the column as a nullable string, or null if the column is <see cref="DBNull"/>.</returns>
        public static string? GetStringOrNull(this IDataRecord @this, int ordinal)
        {
            if (@this.IsDBNull(ordinal)) { return null; }
            return @this.GetString(ordinal);
        }

        /// <summary>
        /// Gets the value of the specified column as a nullable string.
        /// </summary>
        /// <param name="this">The data record.</param>
        /// <param name="name">The name of the column.</param>
        /// <returns>The value of the column as a nullable string, or null if the column is <see cref="DBNull"/>.</returns>
        public static string? GetStringOrNull(this IDataRecord @this, string name)
        {
            return @this.GetStringOrNull(GetAndAssertOrdinal(@this, name));
        }

        /// <summary>
        /// Gets the value of the specified column as a nullable Guid.
        /// </summary>
        /// <param name="this">The data record.</param>
        /// <param name="ordinal">The zero-based column ordinal.</param>
        /// <returns>The value of the column as a nullable Guid, or null if the column is <see cref="DBNull"/>.</returns>
        public static Guid? GetGuidOrNull(this IDataRecord @this, int ordinal)
        {
            if (@this.IsDBNull(ordinal)) { return null; }
            return @this.GetGuid(ordinal);
        }

        /// <summary>
        /// Gets the value of the specified column as a nullable Guid.
        /// </summary>
        /// <param name="this">The data record.</param>
        /// <param name="name">The name of the column.</param>
        /// <returns>The value of the column as a nullable Guid, or null if the column is <see cref="DBNull"/>.</returns>
        public static Guid? GetGuidOrNull(this IDataRecord @this, string name)
        {
            return @this.GetGuidOrNull(GetAndAssertOrdinal(@this, name));
        }

        #endregion

        #region GetValueOrNullAsync

        /// <summary>
        /// Asynchronously gets the value of the specified column as a nullable boolean.
        /// </summary>
        /// <param name="this">The data record.</param>
        /// <param name="ordinal">The zero-based column ordinal.</param>
        /// <param name="cancellationToken">A token to monitor for cancellation requests.</param>
        /// <returns>The value of the column as a nullable boolean, or null if the column is <see cref="DBNull"/>.</returns>
        public static ValueTask<bool?> GetBooleanOrNullAsync(this IDataRecord @this, int ordinal, CancellationToken cancellationToken = default)
        {
            return AsyncUtility.FuncAsyncAsValueTask(
                (@this, ordinal),
                s => s.@this.GetBooleanOrNull(s.ordinal),
                cancellationToken
                );
        }

        /// <summary>
        /// Asynchronously gets the value of the specified column as a nullable boolean.
        /// </summary>
        /// <param name="this">The data record.</param>
        /// <param name="name">The name of the column.</param>
        /// <param name="cancellationToken">A token to monitor for cancellation requests.</param>
        /// <returns>The value of the column as a nullable boolean, or null if the column is <see cref="DBNull"/>.</returns>
        public static ValueTask<bool?> GetBooleanOrNullAsync(this IDataRecord @this, string name, CancellationToken cancellationToken = default)
        {
            return AsyncUtility.FuncAsyncAsValueTask(
                (@this, name),
                s => s.@this.GetBooleanOrNull(s.name),
                cancellationToken
                );
        }

        /// <summary>
        /// Asynchronously gets the value of the specified column as a nullable byte.
        /// </summary>
        /// <param name="this">The data record.</param>
        /// <param name="ordinal">The zero-based column ordinal.</param>
        /// <param name="cancellationToken">A token to monitor for cancellation requests.</param>
        /// <returns>The value of the column as a nullable byte, or null if the column is <see cref="DBNull"/>.</returns>
        public static ValueTask<byte?> GetByteOrNullAsync(this IDataRecord @this, int ordinal, CancellationToken cancellationToken = default)
        {
            return AsyncUtility.FuncAsyncAsValueTask(
                (@this, ordinal),
                s => s.@this.GetByteOrNull(s.ordinal),
                cancellationToken
                );
        }

        /// <summary>
        /// Asynchronously gets the value of the specified column as a nullable byte.
        /// </summary>
        /// <param name="this">The data record.</param>
        /// <param name="name">The name of the column.</param>
        /// <param name="cancellationToken">A token to monitor for cancellation requests.</param>
        /// <returns>The value of the column as a nullable byte, or null if the column is <see cref="DBNull"/>.</returns>
        public static ValueTask<byte?> GetByteOrNullAsync(this IDataRecord @this, string name, CancellationToken cancellationToken = default)
        {
            return AsyncUtility.FuncAsyncAsValueTask(
                (@this, name),
                s => s.@this.GetByteOrNull(s.name),
                cancellationToken
                );
        }

        /// <summary>
        /// Asynchronously gets the value of the specified column as a nullable 16-bit integer.
        /// </summary>
        /// <param name="this">The data record.</param>
        /// <param name="ordinal">The zero-based column ordinal.</param>
        /// <param name="cancellationToken">A token to monitor for cancellation requests.</param>
        /// <returns>The value of the column as a nullable 16-bit integer, or null if the column is <see cref="DBNull"/>.</returns>
        public static ValueTask<short?> GetInt16OrNullAsync(this IDataRecord @this, int ordinal, CancellationToken cancellationToken = default)
        {
            return AsyncUtility.FuncAsyncAsValueTask(
                (@this, ordinal),
                s => s.@this.GetInt16OrNull(s.ordinal),
                cancellationToken
                );
        }

        /// <summary>
        /// Asynchronously gets the value of the specified column as a nullable 16-bit integer.
        /// </summary>
        /// <param name="this">The data record.</param>
        /// <param name="name">The name of the column.</param>
        /// <param name="cancellationToken">A token to monitor for cancellation requests.</param>
        /// <returns>The value of the column as a nullable 16-bit integer, or null if the column is <see cref="DBNull"/>.</returns>
        public static ValueTask<short?> GetInt16OrNullAsync(this IDataRecord @this, string name, CancellationToken cancellationToken = default)
        {
            return AsyncUtility.FuncAsyncAsValueTask(
                (@this, name),
                s => s.@this.GetInt16OrNull(s.name),
                cancellationToken
                );
        }

        /// <summary>
        /// Asynchronously gets the value of the specified column as a nullable 32-bit integer.
        /// </summary>
        /// <param name="this">The data record.</param>
        /// <param name="ordinal">The zero-based column ordinal.</param>
        /// <param name="cancellationToken">A token to monitor for cancellation requests.</param>
        /// <returns>The value of the column as a nullable 32-bit integer, or null if the column is <see cref="DBNull"/>.</returns>
        public static ValueTask<int?> GetInt32OrNullAsync(this IDataRecord @this, int ordinal, CancellationToken cancellationToken = default)
        {
            return AsyncUtility.FuncAsyncAsValueTask(
                (@this, ordinal),
                s => s.@this.GetInt32OrNull(s.ordinal),
                cancellationToken
                );
        }

        /// <summary>
        /// Asynchronously gets the value of the specified column as a nullable 32-bit integer.
        /// </summary>
        /// <param name="this">The data record.</param>
        /// <param name="name">The name of the column.</param>
        /// <param name="cancellationToken">A token to monitor for cancellation requests.</param>
        /// <returns>The value of the column as a nullable 32-bit integer, or null if the column is <see cref="DBNull"/>.</returns>
        public static ValueTask<int?> GetInt32OrNullAsync(this IDataRecord @this, string name, CancellationToken cancellationToken = default)
        {
            return AsyncUtility.FuncAsyncAsValueTask(
                (@this, name),
                s => s.@this.GetInt32OrNull(s.name),
                cancellationToken
                );
        }

        /// <summary>
        /// Asynchronously gets the value of the specified column as a nullable 64-bit integer.
        /// </summary>
        /// <param name="this">The data record.</param>
        /// <param name="ordinal">The zero-based column ordinal.</param>
        /// <param name="cancellationToken">A token to monitor for cancellation requests.</param>
        /// <returns>The value of the column as a nullable 64-bit integer, or null if the column is <see cref="DBNull"/>.</returns>
        public static ValueTask<long?> GetInt64OrNullAsync(this IDataRecord @this, int ordinal, CancellationToken cancellationToken = default)
        {
            return AsyncUtility.FuncAsyncAsValueTask(
                (@this, ordinal),
                s => s.@this.GetInt64OrNull(s.ordinal),
                cancellationToken
                );
        }

        /// <summary>
        /// Asynchronously gets the value of the specified column as a nullable 64-bit integer.
        /// </summary>
        /// <param name="this">The data record.</param>
        /// <param name="name">The name of the column.</param>
        /// <param name="cancellationToken">A token to monitor for cancellation requests.</param>
        /// <returns>The value of the column as a nullable 64-bit integer, or null if the column is <see cref="DBNull"/>.</returns>
        public static ValueTask<long?> GetInt64OrNullAsync(this IDataRecord @this, string name, CancellationToken cancellationToken = default)
        {
            return AsyncUtility.FuncAsyncAsValueTask(
                (@this, name),
                s => s.@this.GetInt64OrNull(s.name),
                cancellationToken
                );
        }

        /// <summary>
        /// Asynchronously gets the value of the specified column as a nullable single-precision floating-point number.
        /// </summary>
        /// <param name="this">The data record.</param>
        /// <param name="ordinal">The zero-based column ordinal.</param>
        /// <param name="cancellationToken">A token to monitor for cancellation requests.</param>
        /// <returns>The value of the column as a nullable single-precision floating-point number, or null if the column is <see cref="DBNull"/>.</returns>
        public static ValueTask<float?> GetFloatOrNullAsync(this IDataRecord @this, int ordinal, CancellationToken cancellationToken = default)
        {
            return AsyncUtility.FuncAsyncAsValueTask(
                (@this, ordinal),
                s => s.@this.GetFloatOrNull(s.ordinal),
                cancellationToken
                );
        }

        /// <summary>
        /// Asynchronously gets the value of the specified column as a nullable single-precision floating-point number.
        /// </summary>
        /// <param name="this">The data record.</param>
        /// <param name="name">The name of the column.</param>
        /// <param name="cancellationToken">A token to monitor for cancellation requests.</param>
        /// <returns>The value of the column as a nullable single-precision floating-point number, or null if the column is <see cref="DBNull"/>.</returns>
        public static ValueTask<float?> GetFloatOrNullAsync(this IDataRecord @this, string name, CancellationToken cancellationToken = default)
        {
            return AsyncUtility.FuncAsyncAsValueTask(
                (@this, name),
                s => s.@this.GetFloatOrNull(s.name),
                cancellationToken
                );
        }

        /// <summary>
        /// Asynchronously gets the value of the specified column as a nullable double-precision floating-point number.
        /// </summary>
        /// <param name="this">The data record.</param>
        /// <param name="ordinal">The zero-based column ordinal.</param>
        /// <param name="cancellationToken">A token to monitor for cancellation requests.</param>
        /// <returns>The value of the column as a nullable double-precision floating-point number, or null if the column is <see cref="DBNull"/>.</returns>
        public static ValueTask<double?> GetDoubleOrNullAsync(this IDataRecord @this, int ordinal, CancellationToken cancellationToken = default)
        {
            return AsyncUtility.FuncAsyncAsValueTask(
                (@this, ordinal),
                s => s.@this.GetDoubleOrNull(s.ordinal),
                cancellationToken
                );
        }

        /// <summary>
        /// Asynchronously gets the value of the specified column as a nullable double-precision floating-point number.
        /// </summary>
        /// <param name="this">The data record.</param>
        /// <param name="name">The name of the column.</param>
        /// <param name="cancellationToken">A token to monitor for cancellation requests.</param>
        /// <returns>The value of the column as a nullable double-precision floating-point number, or null if the column is <see cref="DBNull"/>.</returns>
        public static ValueTask<double?> GetDoubleOrNullAsync(this IDataRecord @this, string name, CancellationToken cancellationToken = default)
        {
            return AsyncUtility.FuncAsyncAsValueTask(
                (@this, name),
                s => s.@this.GetDoubleOrNull(s.name),
                cancellationToken
                );
        }

        /// <summary>
        /// Asynchronously gets the value of the specified column as a nullable decimal.
        /// </summary>
        /// <param name="this">The data record.</param>
        /// <param name="ordinal">The zero-based column ordinal.</param>
        /// <param name="cancellationToken">A token to monitor for cancellation requests.</param>
        /// <returns>The value of the column as a nullable decimal, or null if the column is <see cref="DBNull"/>.</returns>
        public static ValueTask<decimal?> GetDecimalOrNullAsync(this IDataRecord @this, int ordinal, CancellationToken cancellationToken = default)
        {
            return AsyncUtility.FuncAsyncAsValueTask(
                (@this, ordinal),
                s => s.@this.GetDecimalOrNull(s.ordinal),
                cancellationToken
                );
        }

        /// <summary>
        /// Asynchronously gets the value of the specified column as a nullable decimal.
        /// </summary>
        /// <param name="this">The data record.</param>
        /// <param name="name">The name of the column.</param>
        /// <param name="cancellationToken">A token to monitor for cancellation requests.</param>
        /// <returns>The value of the column as a nullable decimal, or null if the column is <see cref="DBNull"/>.</returns>
        public static ValueTask<decimal?> GetDecimalOrNullAsync(this IDataRecord @this, string name, CancellationToken cancellationToken = default)
        {
            return AsyncUtility.FuncAsyncAsValueTask(
                (@this, name),
                s => s.@this.GetDecimalOrNull(s.name),
                cancellationToken
                );
        }

        /// <summary>
        /// Asynchronously gets the value of the specified column as a nullable DateTime.
        /// </summary>
        /// <param name="this">The data record.</param>
        /// <param name="ordinal">The zero-based column ordinal.</param>
        /// <param name="cancellationToken">A token to monitor for cancellation requests.</param>
        /// <returns>The value of the column as a nullable DateTime, or null if the column is <see cref="DBNull"/>.</returns>
        public static ValueTask<DateTime?> GetDateTimeOrNullAsync(this IDataRecord @this, int ordinal, CancellationToken cancellationToken = default)
        {
            return AsyncUtility.FuncAsyncAsValueTask(
                (@this, ordinal),
                s => s.@this.GetDateTimeOrNull(s.ordinal),
                cancellationToken
                );
        }

        /// <summary>
        /// Asynchronously gets the value of the specified column as a nullable DateTime.
        /// </summary>
        /// <param name="this">The data record.</param>
        /// <param name="name">The name of the column.</param>
        /// <param name="cancellationToken">A token to monitor for cancellation requests.</param>
        /// <returns>The value of the column as a nullable DateTime, or null if the column is <see cref="DBNull"/>.</returns>
        public static ValueTask<DateTime?> GetDateTimeOrNullAsync(this IDataRecord @this, string name, CancellationToken cancellationToken = default)
        {
            return AsyncUtility.FuncAsyncAsValueTask(
                (@this, name),
                s => s.@this.GetDateTimeOrNull(s.name),
                cancellationToken
                );
        }

        /// <summary>
        /// Asynchronously gets the value of the specified column as a nullable char.
        /// </summary>
        /// <param name="this">The data record.</param>
        /// <param name="ordinal">The zero-based column ordinal.</param>
        /// <param name="cancellationToken">A token to monitor for cancellation requests.</param>
        /// <returns>The value of the column as a nullable char, or null if the column is <see cref="DBNull"/>.</returns>
        public static ValueTask<char?> GetCharOrNullAsync(this IDataRecord @this, int ordinal, CancellationToken cancellationToken = default)
        {
            return AsyncUtility.FuncAsyncAsValueTask(
                (@this, ordinal),
                s => s.@this.GetCharOrNull(s.ordinal),
                cancellationToken
                );
        }

        /// <summary>
        /// Asynchronously gets the value of the specified column as a nullable char.
        /// </summary>
        /// <param name="this">The data record.</param>
        /// <param name="name">The name of the column.</param>
        /// <param name="cancellationToken">A token to monitor for cancellation requests.</param>
        /// <returns>The value of the column as a nullable char, or null if the column is <see cref="DBNull"/>.</returns>
        public static ValueTask<char?> GetCharOrNullAsync(this IDataRecord @this, string name, CancellationToken cancellationToken = default)
        {
            return AsyncUtility.FuncAsyncAsValueTask(
                (@this, name),
                s => s.@this.GetCharOrNull(s.name),
                cancellationToken
                );
        }

        /// <summary>
        /// Asynchronously gets the value of the specified column as a nullable string.
        /// </summary>
        /// <param name="this">The data record.</param>
        /// <param name="ordinal">The zero-based column ordinal.</param>
        /// <param name="cancellationToken">A token to monitor for cancellation requests.</param>
        /// <returns>The value of the column as a nullable string, or null if the column is <see cref="DBNull"/>.</returns>
        public static ValueTask<string?> GetStringOrNullAsync(this IDataRecord @this, int ordinal, CancellationToken cancellationToken = default)
        {
            return AsyncUtility.FuncAsyncAsValueTask(
                (@this, ordinal),
                s => s.@this.GetStringOrNull(s.ordinal),
                cancellationToken
                );
        }

        /// <summary>
        /// Asynchronously gets the value of the specified column as a nullable string.
        /// </summary>
        /// <param name="this">The data record.</param>
        /// <param name="name">The name of the column.</param>
        /// <param name="cancellationToken">A token to monitor for cancellation requests.</param>
        /// <returns>The value of the column as a nullable string, or null if the column is <see cref="DBNull"/>.</returns>
        public static ValueTask<string?> GetStringOrNullAsync(this IDataRecord @this, string name, CancellationToken cancellationToken = default)
        {
            return AsyncUtility.FuncAsyncAsValueTask(
                (@this, name),
                s => s.@this.GetStringOrNull(s.name),
                cancellationToken
                );
        }

        /// <summary>
        /// Asynchronously gets the value of the specified column as a nullable Guid.
        /// </summary>
        /// <param name="this">The data record.</param>
        /// <param name="ordinal">The zero-based column ordinal.</param>
        /// <param name="cancellationToken">A token to monitor for cancellation requests.</param>
        /// <returns>The value of the column as a nullable Guid, or null if the column is <see cref="DBNull"/>.</returns>
        public static ValueTask<Guid?> GetGuidOrNullAsync(this IDataRecord @this, int ordinal, CancellationToken cancellationToken = default)
        {
            return AsyncUtility.FuncAsyncAsValueTask(
                (@this, ordinal),
                s => s.@this.GetGuidOrNull(s.ordinal),
                cancellationToken
                );
        }

        /// <summary>
        /// Asynchronously gets the value of the specified column as a nullable Guid.
        /// </summary>
        /// <param name="this">The data record.</param>
        /// <param name="name">The name of the column.</param>
        /// <param name="cancellationToken">A token to monitor for cancellation requests.</param>
        /// <returns>The value of the column as a nullable Guid, or null if the column is <see cref="DBNull"/>.</returns>
        public static ValueTask<Guid?> GetGuidOrNullAsync(this IDataRecord @this, string name, CancellationToken cancellationToken = default)
        {
            return AsyncUtility.FuncAsyncAsValueTask(
                (@this, name),
                s => s.@this.GetGuidOrNull(s.name),
                cancellationToken
                );
        }

        #endregion

        #region GetValueOrDefault

        /// <summary>
        /// Gets the value of the specified column as a boolean, or the default value if the column is <see cref="DBNull"/>.
        /// </summary>
        /// <param name="this">The data record.</param>
        /// <param name="ordinal">The zero-based column ordinal.</param>
        /// <returns>The value of the column as a boolean, or the default value.</returns>
        public static bool GetBooleanOrDefault(this IDataRecord @this, int ordinal)
        {
            if (@this.IsDBNull(ordinal)) { return default; }
            return @this.GetBoolean(ordinal);
        }

        /// <summary>
        /// Gets the value of the specified column as a boolean, or the default value if the column is <see cref="DBNull"/>.
        /// </summary>
        /// <param name="this">The data record.</param>
        /// <param name="name">The name of the column.</param>
        /// <returns>The value of the column as a boolean, or the default value.</returns>
        public static bool GetBooleanOrDefault(this IDataRecord @this, string name)
        {
            return @this.GetBooleanOrDefault(GetAndAssertOrdinal(@this, name));
        }

        /// <summary>
        /// Gets the value of the specified column as a byte, or the default value if the column is <see cref="DBNull"/>.
        /// </summary>
        /// <param name="this">The data record.</param>
        /// <param name="ordinal">The zero-based column ordinal.</param>
        /// <returns>The value of the column as a byte, or the default value.</returns>
        public static byte GetByteOrDefault(this IDataRecord @this, int ordinal)
        {
            if (@this.IsDBNull(ordinal)) { return default; }
            return @this.GetByte(ordinal);
        }

        /// <summary>
        /// Gets the value of the specified column as a byte, or the default value if the column is <see cref="DBNull"/>.
        /// </summary>
        /// <param name="this">The data record.</param>
        /// <param name="name">The name of the column.</param>
        /// <returns>The value of the column as a byte, or the default value.</returns>
        public static byte GetByteOrDefault(this IDataRecord @this, string name)
        {
            return @this.GetByteOrDefault(GetAndAssertOrdinal(@this, name));
        }

        /// <summary>
        /// Gets the value of the specified column as a 16-bit integer, or the default value if the column is <see cref="DBNull"/>.
        /// </summary>
        /// <param name="this">The data record.</param>
        /// <param name="ordinal">The zero-based column ordinal.</param>
        /// <returns>The value of the column as a 16-bit integer, or the default value.</returns>
        public static short GetInt16OrDefault(this IDataRecord @this, int ordinal)
        {
            if (@this.IsDBNull(ordinal)) { return default; }
            return @this.GetInt16(ordinal);
        }

        /// <summary>
        /// Gets the value of the specified column as a 16-bit integer, or the default value if the column is <see cref="DBNull"/>.
        /// </summary>
        /// <param name="this">The data record.</param>
        /// <param name="name">The name of the column.</param>
        /// <returns>The value of the column as a 16-bit integer, or the default value.</returns>
        public static short GetInt16OrDefault(this IDataRecord @this, string name)
        {
            return @this.GetInt16OrDefault(GetAndAssertOrdinal(@this, name));
        }

        /// <summary>
        /// Gets the value of the specified column as a 32-bit integer, or the default value if the column is <see cref="DBNull"/>.
        /// </summary>
        /// <param name="this">The data record.</param>
        /// <param name="ordinal">The zero-based column ordinal.</param>
        /// <returns>The value of the column as a 32-bit integer, or the default value.</returns>
        public static int GetInt32OrDefault(this IDataRecord @this, int ordinal)
        {
            if (@this.IsDBNull(ordinal)) { return default; }
            return @this.GetInt32(ordinal);
        }

        /// <summary>
        /// Gets the value of the specified column as a 32-bit integer, or the default value if the column is <see cref="DBNull"/>.
        /// </summary>
        /// <param name="this">The data record.</param>
        /// <param name="name">The name of the column.</param>
        /// <returns>The value of the column as a 32-bit integer, or the default value.</returns>
        public static int GetInt32OrDefault(this IDataRecord @this, string name)
        {
            return @this.GetInt32OrDefault(GetAndAssertOrdinal(@this, name));
        }

        /// <summary>
        /// Gets the value of the specified column as a 64-bit integer, or the default value if the column is <see cref="DBNull"/>.
        /// </summary>
        /// <param name="this">The data record.</param>
        /// <param name="ordinal">The zero-based column ordinal.</param>
        /// <returns>The value of the column as a 64-bit integer, or the default value.</returns>
        public static long GetInt64OrDefault(this IDataRecord @this, int ordinal)
        {
            if (@this.IsDBNull(ordinal)) { return default; }
            return @this.GetInt64(ordinal);
        }

        /// <summary>
        /// Gets the value of the specified column as a 64-bit integer, or the default value if the column is <see cref="DBNull"/>.
        /// </summary>
        /// <param name="this">The data record.</param>
        /// <param name="name">The name of the column.</param>
        /// <returns>The value of the column as a 64-bit integer, or the default value.</returns>
        public static long GetInt64OrDefault(this IDataRecord @this, string name)
        {
            return @this.GetInt64OrDefault(GetAndAssertOrdinal(@this, name));
        }

        /// <summary>
        /// Gets the value of the specified column as a single-precision floating-point number, or the default value if the column is <see cref="DBNull"/>.
        /// </summary>
        /// <param name="this">The data record.</param>
        /// <param name="ordinal">The zero-based column ordinal.</param>
        /// <returns>The value of the column as a single-precision floating-point number, or the default value.</returns>
        public static float GetFloatOrDefault(this IDataRecord @this, int ordinal)
        {
            if (@this.IsDBNull(ordinal)) { return default; }
            return @this.GetFloat(ordinal);
        }

        /// <summary>
        /// Gets the value of the specified column as a single-precision floating-point number, or the default value if the column is <see cref="DBNull"/>.
        /// </summary>
        /// <param name="this">The data record.</param>
        /// <param name="name">The name of the column.</param>
        /// <returns>The value of the column as a single-precision floating-point number, or the default value.</returns>
        public static float GetFloatOrDefault(this IDataRecord @this, string name)
        {
            return @this.GetFloatOrDefault(GetAndAssertOrdinal(@this, name));
        }

        /// <summary>
        /// Gets the value of the specified column as a double-precision floating-point number, or the default value if the column is <see cref="DBNull"/>.
        /// </summary>
        /// <param name="this">The data record.</param>
        /// <param name="ordinal">The zero-based column ordinal.</param>
        /// <returns>The value of the column as a double-precision floating-point number, or the default value.</returns>
        public static double GetDoubleOrDefault(this IDataRecord @this, int ordinal)
        {
            if (@this.IsDBNull(ordinal)) { return default; }
            return @this.GetDouble(ordinal);
        }

        /// <summary>
        /// Gets the value of the specified column as a double-precision floating-point number, or the default value if the column is <see cref="DBNull"/>.
        /// </summary>
        /// <param name="this">The data record.</param>
        /// <param name="name">The name of the column.</param>
        /// <returns>The value of the column as a double-precision floating-point number, or the default value.</returns>
        public static double GetDoubleOrDefault(this IDataRecord @this, string name)
        {
            return @this.GetDoubleOrDefault(GetAndAssertOrdinal(@this, name));
        }

        /// <summary>
        /// Gets the value of the specified column as a decimal, or the default value if the column is <see cref="DBNull"/>.
        /// </summary>
        /// <param name="this">The data record.</param>
        /// <param name="ordinal">The zero-based column ordinal.</param>
        /// <returns>The value of the column as a decimal, or the default value.</returns>
        public static decimal GetDecimalOrDefault(this IDataRecord @this, int ordinal)
        {
            if (@this.IsDBNull(ordinal)) { return default; }
            return @this.GetDecimal(ordinal);
        }

        /// <summary>
        /// Gets the value of the specified column as a decimal, or the default value if the column is <see cref="DBNull"/>.
        /// </summary>
        /// <param name="this">The data record.</param>
        /// <param name="name">The name of the column.</param>
        /// <returns>The value of the column as a decimal, or the default value.</returns>
        public static decimal GetDecimalOrDefault(this IDataRecord @this, string name)
        {
            return @this.GetDecimalOrDefault(GetAndAssertOrdinal(@this, name));
        }

        /// <summary>
        /// Gets the value of the specified column as a DateTime, or the default value if the column is <see cref="DBNull"/>.
        /// </summary>
        /// <param name="this">The data record.</param>
        /// <param name="ordinal">The zero-based column ordinal.</param>
        /// <returns>The value of the column as a DateTime, or the default value.</returns>
        public static DateTime GetDateTimeOrDefault(this IDataRecord @this, int ordinal)
        {
            if (@this.IsDBNull(ordinal)) { return default; }
            return @this.GetDateTime(ordinal);
        }

        /// <summary>
        /// Gets the value of the specified column as a DateTime, or the default value if the column is <see cref="DBNull"/>.
        /// </summary>
        /// <param name="this">The data record.</param>
        /// <param name="name">The name of the column.</param>
        /// <returns>The value of the column as a DateTime, or the default value.</returns>
        public static DateTime GetDateTimeOrDefault(this IDataRecord @this, string name)
        {
            return @this.GetDateTimeOrDefault(GetAndAssertOrdinal(@this, name));
        }

        /// <summary>
        /// Gets the value of the specified column as a char, or the default value if the column is <see cref="DBNull"/>.
        /// </summary>
        /// <param name="this">The data record.</param>
        /// <param name="ordinal">The zero-based column ordinal.</param>
        /// <returns>The value of the column as a char, or the default value.</returns>
        public static char GetCharOrDefault(this IDataRecord @this, int ordinal)
        {
            if (@this.IsDBNull(ordinal)) { return default; }
            return @this.GetChar(ordinal);
        }

        /// <summary>
        /// Gets the value of the specified column as a char, or the default value if the column is <see cref="DBNull"/>.
        /// </summary>
        /// <param name="this">The data record.</param>
        /// <param name="name">The name of the column.</param>
        /// <returns>The value of the column as a char, or the default value.</returns>
        public static char GetCharOrDefault(this IDataRecord @this, string name)
        {
            return @this.GetCharOrDefault(GetAndAssertOrdinal(@this, name));
        }

        /// <summary>
        /// Gets the value of the specified column as a Guid, or the default value if the column is <see cref="DBNull"/>.
        /// </summary>
        /// <param name="this">The data record.</param>
        /// <param name="ordinal">The zero-based column ordinal.</param>
        /// <returns>The value of the column as a Guid, or the default value.</returns>
        public static Guid GetGuidOrDefault(this IDataRecord @this, int ordinal)
        {
            if (@this.IsDBNull(ordinal)) { return default; }
            return @this.GetGuid(ordinal);
        }

        /// <summary>
        /// Gets the value of the specified column as a Guid, or the default value if the column is <see cref="DBNull"/>.
        /// </summary>
        /// <param name="this">The data record.</param>
        /// <param name="name">The name of the column.</param>
        /// <returns>The value of the column as a Guid, or the default value.</returns>
        public static Guid GetGuidOrDefault(this IDataRecord @this, string name)
        {
            return @this.GetGuidOrDefault(GetAndAssertOrdinal(@this, name));
        }

        #endregion

        #region GetValueOrDefaultAsync

        /// <summary>
        /// Asynchronously gets the value of the specified column as a boolean, or the default value if the column is <see cref="DBNull"/>.
        /// </summary>
        /// <param name="this">The data record.</param>
        /// <param name="ordinal">The zero-based column ordinal.</param>
        /// <param name="cancellationToken">A token to monitor for cancellation requests.</param>
        /// <returns>The value of the column as a boolean, or the default value.</returns>
        public static ValueTask<bool> GetBooleanOrDefaultAsync(this IDataRecord @this, int ordinal, CancellationToken cancellationToken = default)
        {
            return AsyncUtility.FuncAsyncAsValueTask(
                (@this, ordinal),
                s => s.@this.GetBooleanOrDefault(s.ordinal),
                cancellationToken
                );
        }

        /// <summary>
        /// Asynchronously gets the value of the specified column as a boolean, or the default value if the column is <see cref="DBNull"/>.
        /// </summary>
        /// <param name="this">The data record.</param>
        /// <param name="name">The name of the column.</param>
        /// <param name="cancellationToken">A token to monitor for cancellation requests.</param>
        /// <returns>The value of the column as a boolean, or the default value.</returns>
        public static ValueTask<bool> GetBooleanOrDefaultAsync(this IDataRecord @this, string name, CancellationToken cancellationToken = default)
        {
            return AsyncUtility.FuncAsyncAsValueTask(
                (@this, name),
                s => s.@this.GetBooleanOrDefault(s.name),
                cancellationToken
                );
        }

        /// <summary>
        /// Asynchronously gets the value of the specified column as a byte, or the default value if the column is <see cref="DBNull"/>.
        /// </summary>
        /// <param name="this">The data record.</param>
        /// <param name="ordinal">The zero-based column ordinal.</param>
        /// <param name="cancellationToken">A token to monitor for cancellation requests.</param>
        /// <returns>The value of the column as a byte, or the default value.</returns>
        public static ValueTask<byte> GetByteOrDefaultAsync(this IDataRecord @this, int ordinal, CancellationToken cancellationToken = default)
        {
            return AsyncUtility.FuncAsyncAsValueTask(
                (@this, ordinal),
                s => s.@this.GetByteOrDefault(s.ordinal),
                cancellationToken
                );
        }

        /// <summary>
        /// Asynchronously gets the value of the specified column as a byte, or the default value if the column is <see cref="DBNull"/>.
        /// </summary>
        /// <param name="this">The data record.</param>
        /// <param name="name">The name of the column.</param>
        /// <param name="cancellationToken">A token to monitor for cancellation requests.</param>
        /// <returns>The value of the column as a byte, or the default value.</returns>
        public static ValueTask<byte> GetByteOrDefaultAsync(this IDataRecord @this, string name, CancellationToken cancellationToken = default)
        {
            return AsyncUtility.FuncAsyncAsValueTask(
                (@this, name),
                s => s.@this.GetByteOrDefault(s.name),
                cancellationToken
                );
        }

        /// <summary>
        /// Asynchronously gets the value of the specified column as a 16-bit integer, or the default value if the column is <see cref="DBNull"/>.
        /// </summary>
        /// <param name="this">The data record.</param>
        /// <param name="ordinal">The zero-based column ordinal.</param>
        /// <param name="cancellationToken">A token to monitor for cancellation requests.</param>
        /// <returns>The value of the column as a 16-bit integer, or the default value.</returns>
        public static ValueTask<short> GetInt16OrDefaultAsync(this IDataRecord @this, int ordinal, CancellationToken cancellationToken = default)
        {
            return AsyncUtility.FuncAsyncAsValueTask(
                (@this, ordinal),
                s => s.@this.GetInt16OrDefault(s.ordinal),
                cancellationToken
                );
        }

        /// <summary>
        /// Asynchronously gets the value of the specified column as a 16-bit integer, or the default value if the column is <see cref="DBNull"/>.
        /// </summary>
        /// <param name="this">The data record.</param>
        /// <param name="name">The name of the column.</param>
        /// <param name="cancellationToken">A token to monitor for cancellation requests.</param>
        /// <returns>The value of the column as a 16-bit integer, or the default value.</returns>
        public static ValueTask<short> GetInt16OrDefaultAsync(this IDataRecord @this, string name, CancellationToken cancellationToken = default)
        {
            return AsyncUtility.FuncAsyncAsValueTask(
                (@this, name),
                s => s.@this.GetInt16OrDefault(s.name),
                cancellationToken
                );
        }

        /// <summary>
        /// Asynchronously gets the value of the specified column as a 32-bit integer, or the default value if the column is <see cref="DBNull"/>.
        /// </summary>
        /// <param name="this">The data record.</param>
        /// <param name="ordinal">The zero-based column ordinal.</param>
        /// <param name="cancellationToken">A token to monitor for cancellation requests.</param>
        /// <returns>The value of the column as a 32-bit integer, or the default value.</returns>
        public static ValueTask<int> GetInt32OrDefaultAsync(this IDataRecord @this, int ordinal, CancellationToken cancellationToken = default)
        {
            return AsyncUtility.FuncAsyncAsValueTask(
                (@this, ordinal),
                s => s.@this.GetInt32OrDefault(s.ordinal),
                cancellationToken
                );
        }

        /// <summary>
        /// Asynchronously gets the value of the specified column as a 32-bit integer, or the default value if the column is <see cref="DBNull"/>.
        /// </summary>
        /// <param name="this">The data record.</param>
        /// <param name="name">The name of the column.</param>
        /// <param name="cancellationToken">A token to monitor for cancellation requests.</param>
        /// <returns>The value of the column as a 32-bit integer, or the default value.</returns>
        public static ValueTask<int> GetInt32OrDefaultAsync(this IDataRecord @this, string name, CancellationToken cancellationToken = default)
        {
            return AsyncUtility.FuncAsyncAsValueTask(
                (@this, name),
                s => s.@this.GetInt32OrDefault(s.name),
                cancellationToken
                );
        }

        /// <summary>
        /// Asynchronously gets the value of the specified column as a 64-bit integer, or the default value if the column is <see cref="DBNull"/>.
        /// </summary>
        /// <param name="this">The data record.</param>
        /// <param name="ordinal">The zero-based column ordinal.</param>
        /// <param name="cancellationToken">A token to monitor for cancellation requests.</param>
        /// <returns>The value of the column as a 64-bit integer, or the default value.</returns>
        public static ValueTask<long> GetInt64OrDefaultAsync(this IDataRecord @this, int ordinal, CancellationToken cancellationToken = default)
        {
            return AsyncUtility.FuncAsyncAsValueTask(
                (@this, ordinal),
                s => s.@this.GetInt64OrDefault(s.ordinal),
                cancellationToken
                );
        }

        /// <summary>
        /// Asynchronously gets the value of the specified column as a 64-bit integer, or the default value if the column is <see cref="DBNull"/>.
        /// </summary>
        /// <param name="this">The data record.</param>
        /// <param name="name">The name of the column.</param>
        /// <param name="cancellationToken">A token to monitor for cancellation requests.</param>
        /// <returns>The value of the column as a 64-bit integer, or the default value.</returns>
        public static ValueTask<long> GetInt64OrDefaultAsync(this IDataRecord @this, string name, CancellationToken cancellationToken = default)
        {
            return AsyncUtility.FuncAsyncAsValueTask(
                (@this, name),
                s => s.@this.GetInt64OrDefault(s.name),
                cancellationToken
                );
        }

        /// <summary>
        /// Asynchronously gets the value of the specified column as a single-precision floating-point number, or the default value if the column is <see cref="DBNull"/>.
        /// </summary>
        /// <param name="this">The data record.</param>
        /// <param name="ordinal">The zero-based column ordinal.</param>
        /// <param name="cancellationToken">A token to monitor for cancellation requests.</param>
        /// <returns>The value of the column as a single-precision floating-point number, or the default value.</returns>
        public static ValueTask<float> GetFloatOrDefaultAsync(this IDataRecord @this, int ordinal, CancellationToken cancellationToken = default)
        {
            return AsyncUtility.FuncAsyncAsValueTask(
                (@this, ordinal),
                s => s.@this.GetFloatOrDefault(s.ordinal),
                cancellationToken
                );
        }

        /// <summary>
        /// Asynchronously gets the value of the specified column as a single-precision floating-point number, or the default value if the column is <see cref="DBNull"/>.
        /// </summary>
        /// <param name="this">The data record.</param>
        /// <param name="name">The name of the column.</param>
        /// <param name="cancellationToken">A token to monitor for cancellation requests.</param>
        /// <returns>The value of the column as a single-precision floating-point number, or the default value.</returns>
        public static ValueTask<float> GetFloatOrDefaultAsync(this IDataRecord @this, string name, CancellationToken cancellationToken = default)
        {
            return AsyncUtility.FuncAsyncAsValueTask(
                (@this, name),
                s => s.@this.GetFloatOrDefault(s.name),
                cancellationToken
                );
        }

        /// <summary>
        /// Asynchronously gets the value of the specified column as a double-precision floating-point number, or the default value if the column is <see cref="DBNull"/>.
        /// </summary>
        /// <param name="this">The data record.</param>
        /// <param name="ordinal">The zero-based column ordinal.</param>
        /// <param name="cancellationToken">A token to monitor for cancellation requests.</param>
        /// <returns>The value of the column as a double-precision floating-point number, or the default value.</returns>
        public static ValueTask<double> GetDoubleOrDefaultAsync(this IDataRecord @this, int ordinal, CancellationToken cancellationToken = default)
        {
            return AsyncUtility.FuncAsyncAsValueTask(
                (@this, ordinal),
                s => s.@this.GetDoubleOrDefault(s.ordinal),
                cancellationToken
                );
        }

        /// <summary>
        /// Asynchronously gets the value of the specified column as a double-precision floating-point number, or the default value if the column is <see cref="DBNull"/>.
        /// </summary>
        /// <param name="this">The data record.</param>
        /// <param name="name">The name of the column.</param>
        /// <param name="cancellationToken">A token to monitor for cancellation requests.</param>
        /// <returns>The value of the column as a double-precision floating-point number, or the default value.</returns>
        public static ValueTask<double> GetDoubleOrDefaultAsync(this IDataRecord @this, string name, CancellationToken cancellationToken = default)
        {
            return AsyncUtility.FuncAsyncAsValueTask(
                (@this, name),
                s => s.@this.GetDoubleOrDefault(s.name),
                cancellationToken
                );
        }

        /// <summary>
        /// Asynchronously gets the value of the specified column as a decimal, or the default value if the column is <see cref="DBNull"/>.
        /// </summary>
        /// <param name="this">The data record.</param>
        /// <param name="ordinal">The zero-based column ordinal.</param>
        /// <param name="cancellationToken">A token to monitor for cancellation requests.</param>
        /// <returns>The value of the column as a decimal, or the default value.</returns>
        public static ValueTask<decimal> GetDecimalOrDefaultAsync(this IDataRecord @this, int ordinal, CancellationToken cancellationToken = default)
        {
            return AsyncUtility.FuncAsyncAsValueTask(
                (@this, ordinal),
                s => s.@this.GetDecimalOrDefault(s.ordinal),
                cancellationToken
                );
        }

        /// <summary>
        /// Asynchronously gets the value of the specified column as a decimal, or the default value if the column is <see cref="DBNull"/>.
        /// </summary>
        /// <param name="this">The data record.</param>
        /// <param name="name">The name of the column.</param>
        /// <param name="cancellationToken">A token to monitor for cancellation requests.</param>
        /// <returns>The value of the column as a decimal, or the default value.</returns>
        public static ValueTask<decimal> GetDecimalOrDefaultAsync(this IDataRecord @this, string name, CancellationToken cancellationToken = default)
        {
            return AsyncUtility.FuncAsyncAsValueTask(
                (@this, name),
                s => s.@this.GetDecimalOrDefault(s.name),
                cancellationToken
                );
        }

        /// <summary>
        /// Asynchronously gets the value of the specified column as a DateTime, or the default value if the column is <see cref="DBNull"/>.
        /// </summary>
        /// <param name="this">The data record.</param>
        /// <param name="ordinal">The zero-based column ordinal.</param>
        /// <param name="cancellationToken">A token to monitor for cancellation requests.</param>
        /// <returns>The value of the column as a DateTime, or the default value.</returns>
        public static ValueTask<DateTime> GetDateTimeOrDefaultAsync(this IDataRecord @this, int ordinal, CancellationToken cancellationToken = default)
        {
            return AsyncUtility.FuncAsyncAsValueTask(
                (@this, ordinal),
                s => s.@this.GetDateTimeOrDefault(s.ordinal),
                cancellationToken
                );
        }

        /// <summary>
        /// Asynchronously gets the value of the specified column as a DateTime, or the default value if the column is <see cref="DBNull"/>.
        /// </summary>
        /// <param name="this">The data record.</param>
        /// <param name="name">The name of the column.</param>
        /// <param name="cancellationToken">A token to monitor for cancellation requests.</param>
        /// <returns>The value of the column as a DateTime, or the default value.</returns>
        public static ValueTask<DateTime> GetDateTimeOrDefaultAsync(this IDataRecord @this, string name, CancellationToken cancellationToken = default)
        {
            return AsyncUtility.FuncAsyncAsValueTask(
                (@this, name),
                s => s.@this.GetDateTimeOrDefault(s.name),
                cancellationToken
                );
        }

        /// <summary>
        /// Asynchronously gets the value of the specified column as a char, or the default value if the column is <see cref="DBNull"/>.
        /// </summary>
        /// <param name="this">The data record.</param>
        /// <param name="ordinal">The zero-based column ordinal.</param>
        /// <param name="cancellationToken">A token to monitor for cancellation requests.</param>
        /// <returns>The value of the column as a char, or the default value.</returns>
        public static ValueTask<char> GetCharOrDefaultAsync(this IDataRecord @this, int ordinal, CancellationToken cancellationToken = default)
        {
            return AsyncUtility.FuncAsyncAsValueTask(
                (@this, ordinal),
                s => s.@this.GetCharOrDefault(s.ordinal),
                cancellationToken
                );
        }

        /// <summary>
        /// Asynchronously gets the value of the specified column as a char, or the default value if the column is <see cref="DBNull"/>.
        /// </summary>
        /// <param name="this">The data record.</param>
        /// <param name="name">The name of the column.</param>
        /// <param name="cancellationToken">A token to monitor for cancellation requests.</param>
        /// <returns>The value of the column as a char, or the default value.</returns>
        public static ValueTask<char> GetCharOrDefaultAsync(this IDataRecord @this, string name, CancellationToken cancellationToken = default)
        {
            return AsyncUtility.FuncAsyncAsValueTask(
                (@this, name),
                s => s.@this.GetCharOrDefault(s.name),
                cancellationToken
                );
        }

        /// <summary>
        /// Asynchronously gets the value of the specified column as a Guid, or the default value if the column is <see cref="DBNull"/>.
        /// </summary>
        /// <param name="this">The data record.</param>
        /// <param name="ordinal">The zero-based column ordinal.</param>
        /// <param name="cancellationToken">A token to monitor for cancellation requests.</param>
        /// <returns>The value of the column as a Guid, or the default value.</returns>
        public static ValueTask<Guid> GetGuidOrDefaultAsync(this IDataRecord @this, int ordinal, CancellationToken cancellationToken = default)
        {
            return AsyncUtility.FuncAsyncAsValueTask(
                (@this, ordinal),
                s => s.@this.GetGuidOrDefault(s.ordinal),
                cancellationToken
                );
        }

        /// <summary>
        /// Asynchronously gets the value of the specified column as a Guid, or the default value if the column is <see cref="DBNull"/>.
        /// </summary>
        /// <param name="this">The data record.</param>
        /// <param name="name">The name of the column.</param>
        /// <param name="cancellationToken">A token to monitor for cancellation requests.</param>
        /// <returns>The value of the column as a Guid, or the default value.</returns>
        public static ValueTask<Guid> GetGuidOrDefaultAsync(this IDataRecord @this, string name, CancellationToken cancellationToken = default)
        {
            return AsyncUtility.FuncAsyncAsValueTask(
                (@this, name),
                s => s.@this.GetGuidOrDefault(s.name),
                cancellationToken
                );
        }

        #endregion
    }
}