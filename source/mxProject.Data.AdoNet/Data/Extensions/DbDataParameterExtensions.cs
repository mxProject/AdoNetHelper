using System;
using System.Collections.Generic;
using System.Data;

namespace mxProject.Data.Extensions
{
    /// <summary>
    /// Provides extension methods for <see cref="IDbDataParameter"/>.
    /// </summary>
    public static class DbDataParameterExtensions
    {
        #region SetValue

        /// <summary>
        /// Sets the value of the parameter. If the value is <c>null</c>, it sets <see cref="DBNull.Value"/>.
        /// </summary>
        /// <param name="this">The parameter to set the value for.</param>
        /// <param name="value">The value to set. If <c>null</c>, <see cref="DBNull.Value"/> is used.</param>
        public static void SetValue(this IDbDataParameter @this, object? value)
        {
            if (value == null)
            {
                @this.Value = DBNull.Value;
            }
            else
            {
                @this.Value = value;
            }
        }

        /// <summary>
        /// Sets the value of the parameter as a boolean. If the value is <c>null</c>, it sets <see cref="DBNull.Value"/>.
        /// </summary>
        /// <param name="this">The parameter to set the value for.</param>
        /// <param name="value">The boolean value to set. If <c>null</c>, <see cref="DBNull.Value"/> is used.</param>
        public static void SetBooleanValue(this IDbDataParameter @this, bool? value)
        {
            if (value == null)
            {
                @this.Value = DBNull.Value;
            }
            else
            {
                @this.Value = value.Value;
            }
        }

        /// <summary>
        /// Sets the value of the parameter as a byte. If the value is <c>null</c>, it sets <see cref="DBNull.Value"/>.
        /// </summary>
        /// <param name="this">The parameter to set the value for.</param>
        /// <param name="value">The byte value to set. If <c>null</c>, <see cref="DBNull.Value"/> is used.</param>
        public static void SetByteValue(this IDbDataParameter @this, byte? value)
        {
            if (value == null)
            {
                @this.Value = DBNull.Value;
            }
            else
            {
                @this.Value = value.Value;
            }
        }

        /// <summary>
        /// Sets the value of the parameter as a 16-bit integer. If the value is <c>null</c>, it sets <see cref="DBNull.Value"/>.
        /// </summary>
        /// <param name="this">The parameter to set the value for.</param>
        /// <param name="value">The 16-bit integer value to set. If <c>null</c>, <see cref="DBNull.Value"/> is used.</param>
        public static void SetInt16Value(this IDbDataParameter @this, short? value)
        {
            if (value == null)
            {
                @this.Value = DBNull.Value;
            }
            else
            {
                @this.Value = value.Value;
            }
        }

        /// <summary>
        /// Sets the value of the parameter as a 32-bit integer. If the value is <c>null</c>, it sets <see cref="DBNull.Value"/>.
        /// </summary>
        /// <param name="this">The parameter to set the value for.</param>
        /// <param name="value">The 32-bit integer value to set. If <c>null</c>, <see cref="DBNull.Value"/> is used.</param>
        public static void SetInt32Value(this IDbDataParameter @this, int? value)
        {
            if (value == null)
            {
                @this.Value = DBNull.Value;
            }
            else
            {
                @this.Value = value.Value;
            }
        }

        /// <summary>
        /// Sets the value of the parameter as a 64-bit integer. If the value is <c>null</c>, it sets <see cref="DBNull.Value"/>.
        /// </summary>
        /// <param name="this">The parameter to set the value for.</param>
        /// <param name="value">The 64-bit integer value to set. If <c>null</c>, <see cref="DBNull.Value"/> is used.</param>
        public static void SetInt64Value(this IDbDataParameter @this, long? value)
        {
            if (value == null)
            {
                @this.Value = DBNull.Value;
            }
            else
            {
                @this.Value = value.Value;
            }
        }

        /// <summary>
        /// Sets the value of the parameter as a signed byte. If the value is <c>null</c>, it sets <see cref="DBNull.Value"/>.
        /// </summary>
        /// <param name="this">The parameter to set the value for.</param>
        /// <param name="value">The byte value to set. If <c>null</c>, <see cref="DBNull.Value"/> is used.</param>
        /// <exception cref="ArgumentException">
        /// For OracleParameter, setting the value of signed byte will throw an exception.
        /// </exception>
        public static void SetSByteValue(this IDbDataParameter @this, sbyte? value)
        {
            if (value == null)
            {
                @this.Value = DBNull.Value;
            }
            else
            {
                @this.Value = value.Value;
            }
        }

        /// <summary>
        /// Sets the value of the parameter as a 16-bit unsigned integer. If the value is <c>null</c>, it sets <see cref="DBNull.Value"/>.
        /// </summary>
        /// <param name="this">The parameter to set the value for.</param>
        /// <param name="value">The 16-bit integer value to set. If <c>null</c>, <see cref="DBNull.Value"/> is used.</param>
        /// <exception cref="ArgumentException">
        /// For OracleParameter, setting the value of unsigned integer will throw an exception.
        /// </exception>
        public static void SetUInt16Value(this IDbDataParameter @this, ushort? value)
        {
            if (value == null)
            {
                @this.Value = DBNull.Value;
            }
            else
            {
                @this.Value = value.Value;
            }
        }

        /// <summary>
        /// Sets the value of the parameter as a 32-bit unsigned integer. If the value is <c>null</c>, it sets <see cref="DBNull.Value"/>.
        /// </summary>
        /// <param name="this">The parameter to set the value for.</param>
        /// <param name="value">The 32-bit integer value to set. If <c>null</c>, <see cref="DBNull.Value"/> is used.</param>
        /// <exception cref="ArgumentException">
        /// For OracleParameter, setting the value of unsigned integer will throw an exception.
        /// </exception>
        public static void SetUInt32Value(this IDbDataParameter @this, uint? value)
        {
            if (value == null)
            {
                @this.Value = DBNull.Value;
            }
            else
            {
                @this.Value = value.Value;
            }
        }

        /// <summary>
        /// Sets the value of the parameter as a 64-bit unsigned integer. If the value is <c>null</c>, it sets <see cref="DBNull.Value"/>.
        /// </summary>
        /// <param name="this">The parameter to set the value for.</param>
        /// <param name="value">The 64-bit integer value to set. If <c>null</c>, <see cref="DBNull.Value"/> is used.</param>
        /// <exception cref="ArgumentException">
        /// For OracleParameter, setting the value of unsigned integer will throw an exception.
        /// </exception>
        public static void SetUInt64Value(this IDbDataParameter @this, ulong? value)
        {
            if (value == null)
            {
                @this.Value = DBNull.Value;
            }
            else
            {
                @this.Value = value.Value;
            }
        }
        
        /// <summary>
                 /// Sets the value of the parameter as a single-precision floating-point number. If the value is <c>null</c>, it sets <see cref="DBNull.Value"/>.
                 /// </summary>
                 /// <param name="this">The parameter to set the value for.</param>
                 /// <param name="value">The single-precision floating-point value to set. If <c>null</c>, <see cref="DBNull.Value"/> is used.</param>
        public static void SetSingleValue(this IDbDataParameter @this, float? value)
        {
            if (value == null)
            {
                @this.Value = DBNull.Value;
            }
            else
            {
                @this.Value = value.Value;
            }
        }

        /// <summary>
        /// Sets the value of the parameter as a double-precision floating-point number. If the value is <c>null</c>, it sets <see cref="DBNull.Value"/>.
        /// </summary>
        /// <param name="this">The parameter to set the value for.</param>
        /// <param name="value">The double-precision floating-point value to set. If <c>null</c>, <see cref="DBNull.Value"/> is used.</param>
        public static void SetDoubleValue(this IDbDataParameter @this, double? value)
        {
            if (value == null)
            {
                @this.Value = DBNull.Value;
            }
            else
            {
                @this.Value = value.Value;
            }
        }

        /// <summary>
        /// Sets the value of the parameter as a decimal. If the value is <c>null</c>, it sets <see cref="DBNull.Value"/>.
        /// </summary>
        /// <param name="this">The parameter to set the value for.</param>
        /// <param name="value">The decimal value to set. If <c>null</c>, <see cref="DBNull.Value"/> is used.</param>
        public static void SetDecimalValue(this IDbDataParameter @this, decimal? value)
        {
            if (value == null)
            {
                @this.Value = DBNull.Value;
            }
            else
            {
                @this.Value = value.Value;
            }
        }

        /// <summary>
        /// Sets the value of the parameter as a <see cref="DateTime"/>. If the value is <c>null</c>, it sets <see cref="DBNull.Value"/>.
        /// </summary>
        /// <param name="this">The parameter to set the value for.</param>
        /// <param name="value">The <see cref="DateTime"/> value to set. If <c>null</c>, <see cref="DBNull.Value"/> is used.</param>
        public static void SetDateTimeValue(this IDbDataParameter @this, DateTime? value)
        {
            if (value == null)
            {
                @this.Value = DBNull.Value;
            }
            else
            {
                @this.Value = value.Value;
            }
        }

        /// <summary>
        /// Sets the value of the parameter as a <see cref="DateTimeOffset"/>. If the value is <c>null</c>, it sets <see cref="DBNull.Value"/>.
        /// </summary>
        /// <param name="this">The parameter to set the value for.</param>
        /// <param name="value">The <see cref="DateTimeOffset"/> value to set. If <c>null</c>, <see cref="DBNull.Value"/> is used.</param>
        public static void SetDateTimeOffsetValue(this IDbDataParameter @this, DateTimeOffset? value)
        {
            if (value == null)
            {
                @this.Value = DBNull.Value;
            }
            else
            {
                @this.Value = value.Value;
            }
        }

        /// <summary>
        /// Sets the value of the parameter as a <see cref="TimeSpan"/>. If the value is <c>null</c>, it sets <see cref="DBNull.Value"/>.
        /// </summary>
        /// <param name="this">The parameter to set the value for.</param>
        /// <param name="value">The <see cref="TimeSpan"/> value to set. If <c>null</c>, <see cref="DBNull.Value"/> is used.</param>
        public static void SetTimeValue(this IDbDataParameter @this, TimeSpan? value)
        {
            if (value == null)
            {
                @this.Value = DBNull.Value;
            }
            else
            {
                @this.Value = value.Value;
            }
        }

        /// <summary>
        /// Sets the value of the parameter as a string. If the value is <c>null</c>, it sets <see cref="DBNull.Value"/>.
        /// </summary>
        /// <param name="this">The parameter to set the value for.</param>
        /// <param name="value">The string value to set. If <c>null</c>, <see cref="DBNull.Value"/> is used.</param>
        public static void SetStringValue(this IDbDataParameter @this, string? value)
        {
            if (value == null)
            {
                @this.Value = DBNull.Value;
            }
            else
            {
                @this.Value = value;
            }
        }

        /// <summary>
        /// Sets the value of the parameter as a character. If the value is <c>null</c>, it sets <see cref="DBNull.Value"/>.
        /// </summary>
        /// <param name="this">The parameter to set the value for.</param>
        /// <param name="value">The character value to set. If <c>null</c>, <see cref="DBNull.Value"/> is used.</param>
        public static void SetCharValue(this IDbDataParameter @this, Char? value)
        {
            if (value == null)
            {
                @this.Value = DBNull.Value;
            }
            else
            {
                @this.Value = value;
            }
        }

        /// <summary>
        /// Sets the value of the parameter as a <see cref="Guid"/>. If the value is <c>null</c>, it sets <see cref="DBNull.Value"/>.
        /// </summary>
        /// <param name="this">The parameter to set the value for.</param>
        /// <param name="value">The <see cref="Guid"/> value to set. If <c>null</c>, <see cref="DBNull.Value"/> is used.</param>
        /// <exception cref="ArgumentException">
        /// For OracleParameter, setting the value of Guid will throw an exception.
        /// </exception>
        public static void SetGuidValue(this IDbDataParameter @this, Guid? value)
        {
            if (value == null)
            {
                @this.Value = DBNull.Value;
            }
            else
            {
                @this.Value = value.Value;
            }
        }

        /// <summary>
        /// Sets the value of the parameter as a binary array. If the value is <c>null</c>, it sets <see cref="DBNull.Value"/>.
        /// </summary>
        /// <param name="this">The parameter to set the value for.</param>
        /// <param name="value">The binary array value to set. If <c>null</c>, <see cref="DBNull.Value"/> is used.</param>
        public static void SetBinaryValue(this IDbDataParameter @this, byte[]? value)
        {
            if (value == null)
            {
                @this.Value = DBNull.Value;
            }
            else
            {
                @this.Value = value;
            }
        }

        #endregion

        #region Configure

        ///// <summary>
        ///// Configures the paramter collection by applying the specified action.
        ///// </summary>
        ///// <typeparam name="TParameters">The type of the paramter collection.</typeparam>
        ///// <param name="this">The database paramter collection.</param>
        ///// <param name="configure">The action to apply to the paramter collection.</param>
        ///// <exception cref="InvalidOperationException">Thrown if the paramter collection is not of the specified type.</exception>
        //public static void Configure<TParameters>(this IDataParameterCollection @this, Action<TParameters> configure) where TParameters : IDataParameterCollection
        //{
        //    var paramters = @this.Find<TParameters>();

        //    if (paramters == null) { throw new InvalidOperationException($"The paramter collection is not of type {typeof(TParameters).Name}."); }

        //    configure(paramters);
        //}

        /// <summary>
        /// Finds the paramter collection of the specified type.
        /// </summary>
        /// <typeparam name="TParameters">The type of the paramter collection to find.</typeparam>
        /// <param name="this">The database paramter collection.</param>
        /// <returns>The paramter collection of the specified type, or <c>null</c> if not found.</returns>
        private static TParameters? Find<TParameters>(this IDataParameterCollection @this) where TParameters : IDataParameterCollection
        {
            if (@this is TParameters paramters)
            {
                return paramters;
            }

            if (@this is Wrappers.DataParameterCollectionWithFilter wrapper)
            {
                return wrapper.Find<TParameters>();
            }

            return default;
        }

        #endregion
    }
}
