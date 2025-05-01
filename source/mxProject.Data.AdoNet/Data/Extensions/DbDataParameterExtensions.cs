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

        #region Cast

        /// <summary>
        /// Casts the parameter to the specified type and performs the specified action.
        /// </summary>
        /// <typeparam name="TParameter">The type to cast the parameter to.</typeparam>
        /// <param name="this">The parameter to cast.</param>
        /// <param name="action">The action to perform on the casted parameter.</param>
        /// <exception cref="InvalidCastException">Thrown if the parameter is not of the specified type.</exception>
        public static void Cast<TParameter>(this IDbDataParameter @this, Action<TParameter> action) where TParameter : IDbDataParameter
        {
            var paramters = @this.Find<TParameter>();

            if (paramters == null) { throw new InvalidCastException($"The paramter is not of type {typeof(TParameter).Name}."); }

            action(paramters);
        }

        /// <summary>
        /// Casts the parameter to the specified type and returns the result of the specified function.
        /// </summary>
        /// <typeparam name="TParameter">The type to cast the parameter to.</typeparam>
        /// <typeparam name="TResult">The type of the result returned by the function.</typeparam>
        /// <param name="this">The parameter to cast.</param>
        /// <param name="func">The function to execute on the casted parameter.</param>
        /// <returns>The result of the function.</returns>
        /// <exception cref="InvalidCastException">Thrown if the parameter is not of the specified type.</exception>
        public static TResult Cast<TParameter, TResult>(this IDbDataParameter @this, Func<TParameter, TResult> func) where TParameter : IDbDataParameter
        {
            var paramters = @this.Find<TParameter>();

            if (paramters == null) { throw new InvalidCastException($"The paramter is not of type {typeof(TParameter).Name}."); }

            return func(paramters);
        }

        /// <summary>
        /// Attempts to find the parameter as the specified type.
        /// </summary>
        /// <typeparam name="TParameter">The type to find the parameter as.</typeparam>
        /// <param name="this">The parameter to find.</param>
        /// <returns>The parameter casted to the specified type, or <c>null</c> if the cast is not possible.</returns>
        private static TParameter? Find<TParameter>(this IDbDataParameter @this) where TParameter : IDbDataParameter
        {
            if (@this is TParameter paramters)
            {
                return paramters;
            }

            if (@this is IDbDataParameterWrapper wrapper)
            {
                return wrapper.WrappedParameter.Find<TParameter>();
            }

            return default;
        }

        #endregion
    }
}
