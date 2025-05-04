using Microsoft.Win32.SafeHandles;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Common;
using System.Threading;
using System.Threading.Tasks;
using System.Transactions;

namespace mxProject.Data.Extensions
{
    /// <summary>
    /// Provides extension methods for <see cref="IDbCommand"/>.
    /// </summary>
    public static class DbCommandExtensions
    {
        #region async support

        // https://github.com/dotnet/runtime/blob/main/src/libraries/System.Data.Common/src/System/Data/Common/DbCommand.cs

        /// <summary>
        /// Asynchronously disposes the specified <see cref="IDbCommand"/>.
        /// </summary>
        /// <param name="this">The <see cref="IDbCommand"/> to dispose.</param>
        /// <returns>A <see cref="ValueTask"/> representing the asynchronous operation.</returns>
        public static ValueTask DisposeAsync(this IDbCommand @this)
        {
#if NETSTANDARD2_1_OR_GREATER
            if (@this is DbCommand command)
            {
                return command.DisposeAsync();
            }
#endif

            return AsyncUtility.ActionAsyncAsValueTask(@this, s => s.Dispose());
        }

        /// <summary>
        /// Asynchronously prepares the command for execution.
        /// </summary>
        /// <param name="this">The <see cref="IDbCommand"/> to prepare.</param>
        /// <param name="cancellationToken">A token to monitor for cancellation requests.</param>
        /// <returns>A <see cref="Task"/> representing the asynchronous operation.</returns>
        public static Task PrepareAsync(this IDbCommand @this, CancellationToken cancellationToken = default)
        {
#if NETSTANDARD2_1_OR_GREATER
            if (@this is DbCommand command)
            {
                return command.PrepareAsync();
            }
#endif

            return AsyncUtility.ActionAsync(
                @this,
                x => x.Prepare(),
                cancellationToken
                );
        }

        /// <summary>
        /// Asynchronously executes the command and returns a data reader.
        /// </summary>
        /// <param name="this">The <see cref="IDbCommand"/> to execute.</param>
        /// <param name="behavior">The behavior of the command execution.</param>
        /// <param name="cancellationToken">A token to monitor for cancellation requests.</param>
        /// <returns>A <see cref="ValueTask{TResult}"/> representing the asynchronous operation, with a result of <see cref="IDataReader"/>.</returns>
        public static ValueTask<IDataReader> ExecuteReaderAsync(this IDbCommand @this, CommandBehavior behavior = CommandBehavior.Default, CancellationToken cancellationToken = default)
        {
#if NETSTANDARD2_1_OR_GREATER
            if (@this is DbCommand command)
            {
                return command.ExecuteReaderAsync(behavior, cancellationToken);
            }
#endif

            return AsyncUtility.FuncAsyncAsValueTask(
                (@this, behavior),
                x => x.@this.ExecuteReader(x.behavior),
                cancellationToken,
                s => s.@this.CancelIgnoreFailure()
                );
        }

        /// <summary>
        /// Asynchronously executes the command and returns the first column of the first row in the result set.
        /// </summary>
        /// <param name="this">The <see cref="IDbCommand"/> to execute.</param>
        /// <param name="cancellationToken">A token to monitor for cancellation requests.</param>
        /// <returns>A <see cref="ValueTask{TResult}"/> representing the asynchronous operation, with a result of <see cref="object"/>.</returns>
        public static ValueTask<object> ExecuteScalarAsync(this IDbCommand @this, CancellationToken cancellationToken = default)
        {
#if NETSTANDARD2_1_OR_GREATER
            if (@this is DbCommand command)
            {
                return command.ExecuteScalarAsync(behavior, cancellationToken);
            }
#endif

            return AsyncUtility.FuncAsyncAsValueTask(
                @this,
                x => x.ExecuteScalar(),
                cancellationToken,
                s => s.CancelIgnoreFailure()
                );
        }

        /// <summary>
        /// Asynchronously executes the command and returns the number of rows affected.
        /// </summary>
        /// <param name="this">The <see cref="IDbCommand"/> to execute.</param>
        /// <param name="cancellationToken">A token to monitor for cancellation requests.</param>
        /// <returns>A <see cref="ValueTask{TResult}"/> representing the asynchronous operation, with a result of <see cref="int"/>.</returns>
        public static ValueTask<int> ExecuteNonQueryAsync(this IDbCommand @this, CancellationToken cancellationToken = default)
        {
#if NETSTANDARD2_1_OR_GREATER
            if (@this is DbCommand command)
            {
                return command.ExecuteNonQueryAsync(cancellationToken);
            }
#endif

            return AsyncUtility.FuncAsyncAsValueTask(
                @this,
                x => x.ExecuteNonQuery(),
                cancellationToken,
                s => s.CancelIgnoreFailure()
                );
        }

        /// <summary>
        /// Attempts to cancel the execution of the command, ignoring any exceptions.
        /// </summary>
        /// <param name="this">The <see cref="IDbCommand"/> to cancel.</param>
        private static void CancelIgnoreFailure(this IDbCommand @this)
        {
            try
            {
                @this.Cancel();
            }
            catch (Exception)
            {
                // Ignore exceptions from Cancel
            }
        }

        #endregion

        #region AddParameter

        /// <summary>
        /// Adds a parameter to the command.
        /// </summary>
        /// <param name="this">The <see cref="IDbCommand"/> to which the parameter will be added.</param>
        /// <param name="name">The name of the parameter.</param>
        /// <param name="value">The value of the parameter. Default is <c>null</c>.</param>
        /// <param name="valueType">The <see cref="DbType"/> of the parameter. Default is <c>null</c>.</param>
        /// <param name="direction">The <see cref="ParameterDirection"/> of the parameter. Default is <see cref="ParameterDirection.Input"/>.</param>
        /// <returns>A tuple containing the index of the parameter and the created <see cref="IDbDataParameter"/>.</returns>  
        public static (int index, IDbDataParameter parameter) AddParameter(
            this IDbCommand @this,
            string name,
            object? value = null,
            DbType? valueType = null,
            ParameterDirection direction = ParameterDirection.Input
            )
        {
            var param = @this.CreateParameter();

            param.ParameterName = name;
            param.Value = value ?? DBNull.Value;
            if (valueType.HasValue) { param.DbType = valueType.Value; }
            param.Direction = direction;

            return (@this.Parameters.Add(param), param);
        }

        /// <summary>
        /// Adds a boolean parameter to the command.
        /// </summary>
        /// <param name="this">The <see cref="IDbCommand"/> to which the parameter will be added.</param>
        /// <param name="name">The name of the parameter.</param>
        /// <param name="value">The value of the parameter. Default is <c>null</c>.</param>
        /// <param name="valueType">The <see cref="DbType"/> of the parameter. Default is <see cref="DbType.Boolean"/>.</param>
        /// <param name="direction">The <see cref="ParameterDirection"/> of the parameter. Default is <see cref="ParameterDirection.Input"/>.</param>
        /// <returns>A tuple containing the index of the parameter and the created <see cref="IDbDataParameter"/>.</returns>  
        public static (int index, IDbDataParameter parameter) AddBooleanParameter(
            this IDbCommand @this,
            string name,
            bool? value = null,
            DbType valueType = DbType.Boolean,
            ParameterDirection direction = ParameterDirection.Input
            )
        {
            return AddParameter(@this, name, value.HasValue ? value.Value : null, valueType, direction);
        }

        /// <summary>
        /// Adds a byte parameter to the command.
        /// </summary>
        /// <param name="this">The <see cref="IDbCommand"/> to which the parameter will be added.</param>
        /// <param name="name">The name of the parameter.</param>
        /// <param name="value">The value of the parameter. Default is <c>null</c>.</param>
        /// <param name="valueType">The <see cref="DbType"/> of the parameter. Default is <see cref="DbType.Byte"/>.</param>
        /// <param name="direction">The <see cref="ParameterDirection"/> of the parameter. Default is <see cref="ParameterDirection.Input"/>.</param>
        /// <returns>A tuple containing the index of the parameter and the created <see cref="IDbDataParameter"/>.</returns>  
        public static (int index, IDbDataParameter parameter) AddByteParameter(
            this IDbCommand @this,
            string name,
            byte? value = null,
            DbType valueType = DbType.Byte,
            ParameterDirection direction = ParameterDirection.Input
            )
        {
            return AddParameter(@this, name, value.HasValue ? value.Value : null, valueType, direction);
        }

        /// <summary>
        /// Adds a 16-bit integer parameter to the command.
        /// </summary>
        /// <param name="this">The <see cref="IDbCommand"/> to which the parameter will be added.</param>
        /// <param name="name">The name of the parameter.</param>
        /// <param name="value">The value of the parameter. Default is <c>null</c>.</param>
        /// <param name="valueType">The <see cref="DbType"/> of the parameter. Default is <see cref="DbType.Int16"/>.</param>
        /// <param name="direction">The <see cref="ParameterDirection"/> of the parameter. Default is <see cref="ParameterDirection.Input"/>.</param>
        /// <returns>A tuple containing the index of the parameter and the created <see cref="IDbDataParameter"/>.</returns>  
        public static (int index, IDbDataParameter parameter) AddInt16Parameter(
            this IDbCommand @this,
            string name,
            short? value = null,
            DbType valueType = DbType.Int16,
            ParameterDirection direction = ParameterDirection.Input
            )
        {
            return AddParameter(@this, name, value.HasValue ? value.Value : null, valueType, direction);
        }

        /// <summary>
        /// Adds a 32-bit integer parameter to the command.
        /// </summary>
        /// <param name="this">The <see cref="IDbCommand"/> to which the parameter will be added.</param>
        /// <param name="name">The name of the parameter.</param>
        /// <param name="value">The value of the parameter. Default is <c>null</c>.</param>
        /// <param name="valueType">The <see cref="DbType"/> of the parameter. Default is <see cref="DbType.Int32"/>.</param>
        /// <param name="direction">The <see cref="ParameterDirection"/> of the parameter. Default is <see cref="ParameterDirection.Input"/>.</param>
        /// <returns>A tuple containing the index of the parameter and the created <see cref="IDbDataParameter"/>.</returns>  
        public static (int index, IDbDataParameter parameter) AddInt32Parameter(
            this IDbCommand @this,
            string name,
            int? value = null,
            DbType valueType = DbType.Int32,
            ParameterDirection direction = ParameterDirection.Input
            )
        {
            return AddParameter(@this, name, value.HasValue ? value.Value : null, valueType, direction);
        }

        /// <summary>
        /// Adds a 64-bit integer parameter to the command.
        /// </summary>
        /// <param name="this">The <see cref="IDbCommand"/> to which the parameter will be added.</param>
        /// <param name="name">The name of the parameter.</param>
        /// <param name="value">The value of the parameter. Default is <c>null</c>.</param>
        /// <param name="valueType">The <see cref="DbType"/> of the parameter. Default is <see cref="DbType.Int64"/>.</param>
        /// <param name="direction">The <see cref="ParameterDirection"/> of the parameter. Default is <see cref="ParameterDirection.Input"/>.</param>
        /// <returns>A tuple containing the index of the parameter and the created <see cref="IDbDataParameter"/>.</returns>  
        public static (int index, IDbDataParameter parameter) AddInt64Parameter(
            this IDbCommand @this,
            string name,
            long? value = null,
            DbType valueType = DbType.Int64,
            ParameterDirection direction = ParameterDirection.Input
            )
        {
            return AddParameter(@this, name, value.HasValue ? value.Value : null, valueType, direction);
        }

        /// <summary>
        /// Adds an 8-bit signed integer parameter to the command.
        /// </summary>
        /// <param name="this">The <see cref="IDbCommand"/> to which the parameter will be added.</param>
        /// <param name="name">The name of the parameter.</param>
        /// <param name="value">The value of the parameter. Default is <c>null</c>.</param>
        /// <param name="valueType">The <see cref="DbType"/> of the parameter. Default is <see cref="DbType.SByte"/>.</param>
        /// <param name="direction">The <see cref="ParameterDirection"/> of the parameter. Default is <see cref="ParameterDirection.Input"/>.</param>
        /// <returns>A tuple containing the index of the parameter and the created <see cref="IDbDataParameter"/>.</returns>
        /// <exception cref="ArgumentException">
        /// OdbcCommand does not support DbType.SByte.
        /// </exception>
        /// <exception cref="ArgumentException">
        /// SqlCommand does not support DbType.SByte.
        /// </exception>
        /// <exception cref="ArgumentException">
        /// OracleCommand does not support DbType.SByte.
        /// </exception>
        /// <exception cref="NotSupportedException">
        /// NpgsqlCommand does not support DbType.SByte.
        /// </exception>
        public static (int index, IDbDataParameter parameter) AddSByteParameter(
            this IDbCommand @this,
            string name,
            sbyte? value = null,
            DbType valueType = DbType.SByte,
            ParameterDirection direction = ParameterDirection.Input
            )
        {
            return AddParameter(@this, name, value.HasValue ? value.Value : null, valueType, direction);
        }

        /// <summary>
        /// Adds a 16-bit unsigned integer parameter to the command.
        /// </summary>
        /// <param name="this">The <see cref="IDbCommand"/> to which the parameter will be added.</param>
        /// <param name="name">The name of the parameter.</param>
        /// <param name="value">The value of the parameter. Default is <c>null</c>.</param>
        /// <param name="valueType">The <see cref="DbType"/> of the parameter. Default is <see cref="DbType.UInt16"/>.</param>
        /// <param name="direction">The <see cref="ParameterDirection"/> of the parameter. Default is <see cref="ParameterDirection.Input"/>.</param>
        /// <returns>A tuple containing the index of the parameter and the created <see cref="IDbDataParameter"/>.</returns>
        /// <exception cref="ArgumentException">
        /// OdbcCommand does not support DbType.UInt16.
        /// </exception>
        /// <exception cref="ArgumentException">
        /// SqlCommand does not support DbType.UInt16.
        /// </exception>
        /// <exception cref="ArgumentException">
        /// OracleCommand does not support DbType.UInt16.
        /// </exception>
        /// <exception cref="NotSupportedException">
        /// NpgsqlCommand does not support DbType.UInt16.
        /// </exception>
        public static (int index, IDbDataParameter parameter) AddUInt16Parameter(
            this IDbCommand @this,
            string name,
            ushort? value = null,
            DbType valueType = DbType.UInt16,
            ParameterDirection direction = ParameterDirection.Input
            )
        {
            return AddParameter(@this, name, value.HasValue ? value.Value : null, valueType, direction);
        }

        /// <summary>
        /// Adds a 32-bit unsigned integer parameter to the command.
        /// </summary>
        /// <param name="this">The <see cref="IDbCommand"/> to which the parameter will be added.</param>
        /// <param name="name">The name of the parameter.</param>
        /// <param name="value">The value of the parameter. Default is <c>null</c>.</param>
        /// <param name="valueType">The <see cref="DbType"/> of the parameter. Default is <see cref="DbType.UInt32"/>.</param>
        /// <param name="direction">The <see cref="ParameterDirection"/> of the parameter. Default is <see cref="ParameterDirection.Input"/>.</param>
        /// <returns>A tuple containing the index of the parameter and the created <see cref="IDbDataParameter"/>.</returns>
        /// <exception cref="ArgumentException">
        /// OdbcCommand does not support DbType.UInt32.
        /// </exception>
        /// <exception cref="ArgumentException">
        /// SqlCommand does not support DbType.UInt32.
        /// </exception>
        /// <exception cref="ArgumentException">
        /// OracleCommand does not support DbType.UInt32.
        /// </exception>
        /// <exception cref="NotSupportedException">
        /// NpgsqlCommand does not support DbType.UInt32.
        /// </exception>
        public static (int index, IDbDataParameter parameter) AddUInt32Parameter(
            this IDbCommand @this,
            string name,
            uint? value = null,
            DbType valueType = DbType.UInt32,
            ParameterDirection direction = ParameterDirection.Input
            )
        {
            return AddParameter(@this, name, value.HasValue ? value.Value : null, valueType, direction);
        }

        /// <summary>
        /// Adds a 64-bit unsigned integer parameter to the command.
        /// </summary>
        /// <param name="this">The <see cref="IDbCommand"/> to which the parameter will be added.</param>
        /// <param name="name">The name of the parameter.</param>
        /// <param name="value">The value of the parameter. Default is <c>null</c>.</param>
        /// <param name="valueType">The <see cref="DbType"/> of the parameter. Default is <see cref="DbType.UInt64"/>.</param>
        /// <param name="direction">The <see cref="ParameterDirection"/> of the parameter. Default is <see cref="ParameterDirection.Input"/>.</param>
        /// <returns>A tuple containing the index of the parameter and the created <see cref="IDbDataParameter"/>.</returns>
        /// <exception cref="ArgumentException">
        /// OdbcCommand does not support DbType.UInt64.
        /// </exception>
        /// <exception cref="ArgumentException">
        /// SqlCommand does not support DbType.UInt64.
        /// </exception>
        /// <exception cref="ArgumentException">
        /// OracleCommand does not support DbType.UInt64.
        /// </exception>
        /// <exception cref="NotSupportedException">
        /// NpgsqlCommand does not support DbType.UInt64.
        /// </exception>
        public static (int index, IDbDataParameter parameter) AddUInt64Parameter(
            this IDbCommand @this,
            string name,
            ulong? value = null,
            DbType valueType = DbType.UInt64,
            ParameterDirection direction = ParameterDirection.Input
            )
        {
            return AddParameter(@this, name, value.HasValue ? value.Value : null, valueType, direction);
        }

        /// <summary>
        /// Adds a single-precision floating-point parameter to the command.
        /// </summary>
        /// <param name="this">The <see cref="IDbCommand"/> to which the parameter will be added.</param>
        /// <param name="name">The name of the parameter.</param>
        /// <param name="value">The value of the parameter. Default is <c>null</c>.</param>
        /// <param name="valueType">The <see cref="DbType"/> of the parameter. Default is <see cref="DbType.Single"/>.</param>
        /// <param name="direction">The <see cref="ParameterDirection"/> of the parameter. Default is <see cref="ParameterDirection.Input"/>.</param>
        /// <returns>A tuple containing the index of the parameter and the created <see cref="IDbDataParameter"/>.</returns>  
        public static (int index, IDbDataParameter parameter) AddSingleParameter(
            this IDbCommand @this,
            string name,
            float? value = null,
            DbType valueType = DbType.Single,
            ParameterDirection direction = ParameterDirection.Input
            )
        {
            return AddParameter(@this, name, value.HasValue ? value.Value : null, valueType, direction);
        }

        /// <summary>
        /// Adds a double-precision floating-point parameter to the command.
        /// </summary>
        /// <param name="this">The <see cref="IDbCommand"/> to which the parameter will be added.</param>
        /// <param name="name">The name of the parameter.</param>
        /// <param name="value">The value of the parameter. Default is <c>null</c>.</param>
        /// <param name="valueType">The <see cref="DbType"/> of the parameter. Default is <see cref="DbType.Double"/>.</param>
        /// <param name="direction">The <see cref="ParameterDirection"/> of the parameter. Default is <see cref="ParameterDirection.Input"/>.</param>
        /// <returns>A tuple containing the index of the parameter and the created <see cref="IDbDataParameter"/>.</returns>  
        public static (int index, IDbDataParameter parameter) AddDoubleParameter(
            this IDbCommand @this,
            string name,
            double? value = null,
            DbType valueType = DbType.Double,
            ParameterDirection direction = ParameterDirection.Input
            )
        {
            return AddParameter(@this, name, value.HasValue ? value.Value : null, valueType, direction);
        }

        /// <summary>
        /// Adds a decimal parameter to the command.
        /// </summary>
        /// <param name="this">The <see cref="IDbCommand"/> to which the parameter will be added.</param>
        /// <param name="name">The name of the parameter.</param>
        /// <param name="value">The value of the parameter. Default is <c>null</c>.</param>
        /// <param name="valueType">The <see cref="DbType"/> of the parameter. Default is <see cref="DbType.Decimal"/>.</param>
        /// <param name="direction">The <see cref="ParameterDirection"/> of the parameter. Default is <see cref="ParameterDirection.Input"/>.</param>
        /// <returns>A tuple containing the index of the parameter and the created <see cref="IDbDataParameter"/>.</returns>  
        public static (int index, IDbDataParameter parameter) AddDecimalParameter(
            this IDbCommand @this,
            string name,
            decimal? value = null,
            DbType valueType = DbType.Decimal,
            ParameterDirection direction = ParameterDirection.Input
            )
        {
            return AddParameter(@this, name, value.HasValue ? value.Value : null, valueType, direction);
        }

        /// <summary>
        /// Adds a DateTime parameter to the command.
        /// </summary>
        /// <param name="this">The <see cref="IDbCommand"/> to which the parameter will be added.</param>
        /// <param name="name">The name of the parameter.</param>
        /// <param name="value">The value of the parameter. Default is <c>null</c>.</param>
        /// <param name="valueType">The <see cref="DbType"/> of the parameter. Default is <see cref="DbType.DateTime"/>.</param>
        /// <param name="direction">The <see cref="ParameterDirection"/> of the parameter. Default is <see cref="ParameterDirection.Input"/>.</param>
        /// <returns>A tuple containing the index of the parameter and the created <see cref="IDbDataParameter"/>.</returns>  
        public static (int index, IDbDataParameter parameter) AddDateTimeParameter(
            this IDbCommand @this,
            string name,
            DateTime? value = null,
            DbType valueType = DbType.DateTime,
            ParameterDirection direction = ParameterDirection.Input
            )
        {
            return AddParameter(@this, name, value.HasValue ? value.Value : null, valueType, direction);
        }

        /// <summary>
        /// Adds a DateTimeOffset parameter to the command.
        /// </summary>
        /// <param name="this">The <see cref="IDbCommand"/> to which the parameter will be added.</param>
        /// <param name="name">The name of the parameter.</param>
        /// <param name="value">The value of the parameter. Default is <c>null</c>.</param>
        /// <param name="valueType">The <see cref="DbType"/> of the parameter. Default is <see cref="DbType.DateTimeOffset"/>.</param>
        /// <param name="direction">The <see cref="ParameterDirection"/> of the parameter. Default is <see cref="ParameterDirection.Input"/>.</param>
        /// <returns>A tuple containing the index of the parameter and the created <see cref="IDbDataParameter"/>.</returns>  
        /// <exception cref="ArgumentException">
        /// OleDbCommand does not support DbType.DateTimeOffset.
        /// </exception>
        /// <exception cref="ArgumentException">
        /// OdbcCommand does not support DbType.DateTimeOffset.
        /// </exception>
        public static (int index, IDbDataParameter parameter) AddDateTimeOffsetParameter(
            this IDbCommand @this,
            string name,
            DateTimeOffset? value = null,
            DbType valueType = DbType.DateTimeOffset,
            ParameterDirection direction = ParameterDirection.Input
            )
        {
            return AddParameter(@this, name, value.HasValue ? value.Value : null, valueType, direction);
        }

        /// <summary>
        /// Adds a TimeSpan parameter to the command.
        /// </summary>
        /// <param name="this">The <see cref="IDbCommand"/> to which the parameter will be added.</param>
        /// <param name="name">The name of the parameter.</param>
        /// <param name="value">The value of the parameter. Default is <c>null</c>.</param>
        /// <param name="valueType">The <see cref="DbType"/> of the parameter. Default is <see cref="DbType.Time"/>.</param>
        /// <param name="direction">The <see cref="ParameterDirection"/> of the parameter. Default is <see cref="ParameterDirection.Input"/>.</param>
        /// <returns>A tuple containing the index of the parameter and the created <see cref="IDbDataParameter"/>.</returns>  
        public static (int index, IDbDataParameter parameter) AddTimeParameter(
            this IDbCommand @this,
            string name,
            TimeSpan? value = null,
            DbType valueType = DbType.Time,
            ParameterDirection direction = ParameterDirection.Input
            )
        {
            return AddParameter(@this, name, value.HasValue ? value.Value : null, valueType, direction);
        }

        /// <summary>
        /// Adds a string parameter to the command.
        /// </summary>
        /// <param name="this">The <see cref="IDbCommand"/> to which the parameter will be added.</param>
        /// <param name="name">The name of the parameter.</param>
        /// <param name="value">The value of the parameter. Default is <c>null</c>.</param>
        /// <param name="valueType">The <see cref="DbType"/> of the parameter. Default is <see cref="DbType.String"/>.</param>
        /// <param name="direction">The <see cref="ParameterDirection"/> of the parameter. Default is <see cref="ParameterDirection.Input"/>.</param>
        /// <returns>A tuple containing the index of the parameter and the created <see cref="IDbDataParameter"/>.</returns>  
        public static (int index, IDbDataParameter parameter) AddStringParameter(
            this IDbCommand @this,
            string name,
            string? value = null,
            DbType valueType = DbType.String,
            ParameterDirection direction = ParameterDirection.Input
            )
        {
            return AddParameter(@this, name, value, valueType, direction);
        }

        /// <summary>
        /// Adds a character parameter to the command.
        /// </summary>
        /// <param name="this">The <see cref="IDbCommand"/> to which the parameter will be added.</param>
        /// <param name="name">The name of the parameter.</param>
        /// <param name="value">The value of the parameter. Default is <c>null</c>.</param>
        /// <param name="valueType">The <see cref="DbType"/> of the parameter. Default is <see cref="DbType.String"/>.</param>
        /// <param name="direction">The <see cref="ParameterDirection"/> of the parameter. Default is <see cref="ParameterDirection.Input"/>.</param>
        /// <returns>A tuple containing the index of the parameter and the created <see cref="IDbDataParameter"/>.</returns>  
        public static (int index, IDbDataParameter parameter) AddCharParameter(
            this IDbCommand @this,
            string name,
            Char? value = null,
            DbType valueType = DbType.String,
            ParameterDirection direction = ParameterDirection.Input
            )
        {
            return AddParameter(@this, name, value.HasValue ? value.Value : null, valueType, direction);
        }

        /// <summary>
        /// Adds a GUID parameter to the command.
        /// </summary>
        /// <param name="this">The <see cref="IDbCommand"/> to which the parameter will be added.</param>
        /// <param name="name">The name of the parameter.</param>
        /// <param name="value">The value of the parameter. Default is <c>null</c>.</param>
        /// <param name="valueType">The <see cref="DbType"/> of the parameter. Default is <see cref="DbType.Guid"/>.</param>
        /// <param name="direction">The <see cref="ParameterDirection"/> of the parameter. Default is <see cref="ParameterDirection.Input"/>.</param>
        /// <returns>A tuple containing the index of the parameter and the created <see cref="IDbDataParameter"/>.</returns>  
        /// <exception cref="ArgumentException">
        /// OracleCommand does not support DbType.Guid.
        /// </exception>
        public static (int index, IDbDataParameter parameter) AddGuidParameter(
            this IDbCommand @this,
            string name,
            Guid? value = null,
            DbType valueType = DbType.Guid,
            ParameterDirection direction = ParameterDirection.Input
            )
        {
            return AddParameter(@this, name, value.HasValue ? value.Value : null, valueType, direction);
        }

        /// <summary>
        /// Adds a binary parameter to the command.
        /// </summary>
        /// <param name="this">The <see cref="IDbCommand"/> to which the parameter will be added.</param>
        /// <param name="name">The name of the parameter.</param>
        /// <param name="value">The value of the parameter. Default is <c>null</c>.</param>
        /// <param name="valueType">The <see cref="DbType"/> of the parameter. Default is <see cref="DbType.Binary"/>.</param>
        /// <param name="direction">The <see cref="ParameterDirection"/> of the parameter. Default is <see cref="ParameterDirection.Input"/>.</param>
        /// <returns>A tuple containing the index of the parameter and the created <see cref="IDbDataParameter"/>.</returns>  
        public static (int index, IDbDataParameter parameter) AddBinaryParameter(
            this IDbCommand @this,
            string name,
            byte[]? value = null,
            DbType valueType = DbType.Binary,
            ParameterDirection direction = ParameterDirection.Input
            )
        {
            return AddParameter(@this, name, value, valueType, direction);
        }

        #endregion

        #region GetParameter

        /// <summary>
        /// Retrieves a parameter from the command by its index.
        /// </summary>
        /// <param name="this">The <see cref="IDbCommand"/> from which the parameter will be retrieved.</param>
        /// <param name="index">The index of the parameter to retrieve.</param>
        /// <returns>
        /// The <see cref="IDbDataParameter"/> if found; otherwise, <c>null</c>.
        /// </returns>
        public static IDbDataParameter GetParameter(this IDbCommand @this, int index)
        {
            return (IDbDataParameter)@this.Parameters[index];
        }

        /// <summary>
        /// Retrieves a parameter from the command by its name.
        /// </summary>
        /// <param name="this">The <see cref="IDbCommand"/> from which the parameter will be retrieved.</param>
        /// <param name="name">The name of the parameter to retrieve.</param>
        /// <returns>
        /// The <see cref="IDbDataParameter"/> if found; otherwise, <c>null</c>.
        /// </returns>
        /// <exception cref="ArgumentException">
        /// The specified name not found in command parameters.
        /// </exception>
        public static IDbDataParameter GetParameter(this IDbCommand @this, string name)
        {
            if (@this.Parameters.Contains(name))
            {
                return (IDbDataParameter)@this.Parameters[name];
            }
            else
            {
                throw new ArgumentException($"Parameter '{name}' not found in command parameters.");
            }
        }

        #endregion

        #region SetParameterValue

        /// <summary>
        /// Sets the value of a parameter in the command.
        /// </summary>
        /// <param name="this">The <see cref="IDbCommand"/> containing the parameter.</param>
        /// <param name="index">The index of the parameter.</param>
        /// <param name="value">The value to set. If <c>null</c>, <see cref="DBNull.Value"/> is used.</param>
        public static void SetParameterValue(this IDbCommand @this, int index, object? value)
        {
            var parameter = GetParameter(@this, index);
            parameter.Value = value ?? DBNull.Value;
        }

        /// <summary>
        /// Sets the value of a parameter in the command.
        /// </summary>
        /// <param name="this">The <see cref="IDbCommand"/> containing the parameter.</param>
        /// <param name="name">The name of the parameter.</param>
        /// <param name="value">The value to set. If <c>null</c>, <see cref="DBNull.Value"/> is used.</param>
        public static void SetParameterValue(this IDbCommand @this, string name, object? value)
        {
            var parameter = GetParameter(@this, name);
            parameter.Value = value ?? DBNull.Value;
        }

        /// <summary>
        /// Sets the value of a boolean parameter in the command.
        /// </summary>
        /// <param name="this">The <see cref="IDbCommand"/> containing the parameter.</param>
        /// <param name="index">The index of the parameter.</param>
        /// <param name="value">The boolean value to set. If <c>null</c>, the parameter is set to <c>null</c>.</param>
        public static void SetBooleanParameterValue(this IDbCommand @this, int index, bool? value)
        {
            SetParameterValue(@this, index, value.HasValue ? value.Value : null);
        }

        /// <summary>
        /// Sets the value of a boolean parameter in the command.
        /// </summary>
        /// <param name="this">The <see cref="IDbCommand"/> containing the parameter.</param>
        /// <param name="name">The name of the parameter.</param>
        /// <param name="value">The boolean value to set. If <c>null</c>, the parameter is set to <c>null</c>.</param>
        public static void SetBooleanParameterValue(this IDbCommand @this, string name, bool? value)
        {
            SetParameterValue(@this, name, value.HasValue ? value.Value : null);
        }

        /// <summary>
        /// Sets the value of a byte parameter in the command.
        /// </summary>
        /// <param name="this">The <see cref="IDbCommand"/> containing the parameter.</param>
        /// <param name="index">The index of the parameter.</param>
        /// <param name="value">The byte value to set. If <c>null</c>, the parameter is set to <c>null</c>.</param>
        public static void SetByteParameterValue(this IDbCommand @this, int index, byte? value)
        {
            SetParameterValue(@this, index, value.HasValue ? value.Value : null);
        }

        /// <summary>
        /// Sets the value of a byte parameter in the command.
        /// </summary>
        /// <param name="this">The <see cref="IDbCommand"/> containing the parameter.</param>
        /// <param name="name">The name of the parameter.</param>
        /// <param name="value">The byte value to set. If <c>null</c>, the parameter is set to <c>null</c>.</param>
        public static void SetByteParameterValue(this IDbCommand @this, string name, byte? value)
        {
            SetParameterValue(@this, name, value.HasValue ? value.Value : null);
        }

        /// <summary>
        /// Sets the value of a 16-bit integer parameter in the command.
        /// </summary>
        /// <param name="this">The <see cref="IDbCommand"/> containing the parameter.</param>
        /// <param name="index">The index of the parameter.</param>
        /// <param name="value">The 16-bit integer value to set. If <c>null</c>, the parameter is set to <c>null</c>.</param>
        public static void SetInt16ParameterValue(this IDbCommand @this, int index, short? value)
        {
            SetParameterValue(@this, index, value.HasValue ? value.Value : null);
        }

        /// <summary>
        /// Sets the value of a 16-bit integer parameter in the command.
        /// </summary>
        /// <param name="this">The <see cref="IDbCommand"/> containing the parameter.</param>
        /// <param name="name">The name of the parameter.</param>
        /// <param name="value">The 16-bit integer value to set. If <c>null</c>, the parameter is set to <c>null</c>.</param>
        public static void SetInt16ParameterValue(this IDbCommand @this, string name, short? value)
        {
            SetParameterValue(@this, name, value.HasValue ? value.Value : null);
        }

        /// <summary>
        /// Sets the value of a 32-bit integer parameter in the command.
        /// </summary>
        /// <param name="this">The <see cref="IDbCommand"/> containing the parameter.</param>
        /// <param name="index">The index of the parameter.</param>
        /// <param name="value">The 32-bit integer value to set. If <c>null</c>, the parameter is set to <c>null</c>.</param>
        public static void SetInt32ParameterValue(this IDbCommand @this, int index, int? value)
        {
            SetParameterValue(@this, index, value.HasValue ? value.Value : null);
        }

        /// <summary>
        /// Sets the value of a 32-bit integer parameter in the command.
        /// </summary>
        /// <param name="this">The <see cref="IDbCommand"/> containing the parameter.</param>
        /// <param name="name">The name of the parameter.</param>
        /// <param name="value">The 32-bit integer value to set. If <c>null</c>, the parameter is set to <c>null</c>.</param>
        public static void SetInt32ParameterValue(this IDbCommand @this, string name, int? value)
        {
            SetParameterValue(@this, name, value.HasValue ? value.Value : null);
        }

        /// <summary>
        /// Sets the value of a 64-bit integer parameter in the command.
        /// </summary>
        /// <param name="this">The <see cref="IDbCommand"/> containing the parameter.</param>
        /// <param name="index">The index of the parameter.</param>
        /// <param name="value">The 64-bit integer value to set. If <c>null</c>, the parameter is set to <c>null</c>.</param>
        public static void SetInt64ParameterValue(this IDbCommand @this, int index, long? value)
        {
            SetParameterValue(@this, index, value.HasValue ? value.Value : null);
        }

        /// <summary>
        /// Sets the value of a 64-bit integer parameter in the command.
        /// </summary>
        /// <param name="this">The <see cref="IDbCommand"/> containing the parameter.</param>
        /// <param name="name">The name of the parameter.</param>
        /// <param name="value">The 64-bit integer value to set. If <c>null</c>, the parameter is set to <c>null</c>.</param>
        public static void SetInt64ParameterValue(this IDbCommand @this, string name, long? value)
        {
            SetParameterValue(@this, name, value.HasValue ? value.Value : null);
        }

        /// <summary>
        /// Sets the value of a signed byte parameter in the command.
        /// </summary>
        /// <param name="this">The <see cref="IDbCommand"/> containing the parameter.</param>
        /// <param name="index">The index of the parameter.</param>
        /// <param name="value">The byte value to set. If <c>null</c>, the parameter is set to <c>null</c>.</param>
        public static void SetSByteParameterValue(this IDbCommand @this, int index, sbyte? value)
        {
            SetParameterValue(@this, index, value.HasValue ? value.Value : null);
        }

        /// <summary>
        /// Sets the value of a signed byte parameter in the command.
        /// </summary>
        /// <param name="this">The <see cref="IDbCommand"/> containing the parameter.</param>
        /// <param name="name">The name of the parameter.</param>
        /// <param name="value">The byte value to set. If <c>null</c>, the parameter is set to <c>null</c>.</param>
        public static void SetSByteParameterValue(this IDbCommand @this, string name, sbyte? value)
        {
            SetParameterValue(@this, name, value.HasValue ? value.Value : null);
        }

        /// <summary>
        /// Sets the value of a 16-bit unsigned integer parameter in the command.
        /// </summary>
        /// <param name="this">The <see cref="IDbCommand"/> containing the parameter.</param>
        /// <param name="index">The index of the parameter.</param>
        /// <param name="value">The 16-bit integer value to set. If <c>null</c>, the parameter is set to <c>null</c>.</param>
        public static void SetUInt16ParameterValue(this IDbCommand @this, int index, ushort? value)
        {
            SetParameterValue(@this, index, value.HasValue ? value.Value : null);
        }

        /// <summary>
        /// Sets the value of a 16-bit unsigned integer parameter in the command.
        /// </summary>
        /// <param name="this">The <see cref="IDbCommand"/> containing the parameter.</param>
        /// <param name="name">The name of the parameter.</param>
        /// <param name="value">The 16-bit integer value to set. If <c>null</c>, the parameter is set to <c>null</c>.</param>
        public static void SetUInt16ParameterValue(this IDbCommand @this, string name, ushort? value)
        {
            SetParameterValue(@this, name, value.HasValue ? value.Value : null);
        }

        /// <summary>
        /// Sets the value of a 32-bit unsigned integer parameter in the command.
        /// </summary>
        /// <param name="this">The <see cref="IDbCommand"/> containing the parameter.</param>
        /// <param name="index">The index of the parameter.</param>
        /// <param name="value">The 32-bit integer value to set. If <c>null</c>, the parameter is set to <c>null</c>.</param>
        public static void SetUInt32ParameterValue(this IDbCommand @this, int index, uint? value)
        {
            SetParameterValue(@this, index, value.HasValue ? value.Value : null);
        }

        /// <summary>
        /// Sets the value of a 32-bit unsigned integer parameter in the command.
        /// </summary>
        /// <param name="this">The <see cref="IDbCommand"/> containing the parameter.</param>
        /// <param name="name">The name of the parameter.</param>
        /// <param name="value">The 32-bit integer value to set. If <c>null</c>, the parameter is set to <c>null</c>.</param>
        public static void SetUInt32ParameterValue(this IDbCommand @this, string name, uint? value)
        {
            SetParameterValue(@this, name, value.HasValue ? value.Value : null);
        }

        /// <summary>
        /// Sets the value of a 64-bit unsigned integer parameter in the command.
        /// </summary>
        /// <param name="this">The <see cref="IDbCommand"/> containing the parameter.</param>
        /// <param name="index">The index of the parameter.</param>
        /// <param name="value">The 64-bit integer value to set. If <c>null</c>, the parameter is set to <c>null</c>.</param>
        public static void SetUInt64ParameterValue(this IDbCommand @this, int index, ulong? value)
        {
            SetParameterValue(@this, index, value.HasValue ? value.Value : null);
        }

        /// <summary>
        /// Sets the value of a 64-bit unsigned integer parameter in the command.
        /// </summary>
        /// <param name="this">The <see cref="IDbCommand"/> containing the parameter.</param>
        /// <param name="name">The name of the parameter.</param>
        /// <param name="value">The 64-bit integer value to set. If <c>null</c>, the parameter is set to <c>null</c>.</param>
        public static void SetUInt64ParameterValue(this IDbCommand @this, string name, ulong? value)
        {
            SetParameterValue(@this, name, value.HasValue ? value.Value : null);
        }

        /// <summary>
        /// Sets the value of a single-precision floating-point parameter in the command.
        /// </summary>
        /// <param name="this">The <see cref="IDbCommand"/> containing the parameter.</param>
        /// <param name="index">The index of the parameter.</param>
        /// <param name="value">The single-precision floating-point value to set. If <c>null</c>, the parameter is set to <c>null</c>.</param>
        public static void SetSingleParameterValue(this IDbCommand @this, int index, float? value)
        {
            SetParameterValue(@this, index, value.HasValue ? value.Value : null);
        }

        /// <summary>
        /// Sets the value of a single-precision floating-point parameter in the command.
        /// </summary>
        /// <param name="this">The <see cref="IDbCommand"/> containing the parameter.</param>
        /// <param name="name">The name of the parameter.</param>
        /// <param name="value">The single-precision floating-point value to set. If <c>null</c>, the parameter is set to <c>null</c>.</param>
        public static void SetSingleParameterValue(this IDbCommand @this, string name, float? value)
        {
            SetParameterValue(@this, name, value.HasValue ? value.Value : null);
        }

        /// <summary>
        /// Sets the value of a double-precision floating-point parameter in the command.
        /// </summary>
        /// <param name="this">The <see cref="IDbCommand"/> containing the parameter.</param>
        /// <param name="index">The index of the parameter.</param>
        /// <param name="value">The double-precision floating-point value to set. If <c>null</c>, the parameter is set to <c>null</c>.</param>
        public static void SetDoubleParameterValue(this IDbCommand @this, int index, double? value)
        {
            SetParameterValue(@this, index, value.HasValue ? value.Value : null);
        }

        /// <summary>
        /// Sets the value of a double-precision floating-point parameter in the command.
        /// </summary>
        /// <param name="this">The <see cref="IDbCommand"/> containing the parameter.</param>
        /// <param name="name">The name of the parameter.</param>
        /// <param name="value">The double-precision floating-point value to set. If <c>null</c>, the parameter is set to <c>null</c>.</param>
        public static void SetDoubleParameterValue(this IDbCommand @this, string name, double? value)
        {
            SetParameterValue(@this, name, value.HasValue ? value.Value : null);
        }

        /// <summary>
        /// Sets the value of a decimal parameter in the command.
        /// </summary>
        /// <param name="this">The <see cref="IDbCommand"/> containing the parameter.</param>
        /// <param name="index">The index of the parameter.</param>
        /// <param name="value">The decimal value to set. If <c>null</c>, the parameter is set to <c>null</c>.</param>
        public static void SetDecimalParameterValue(this IDbCommand @this, int index, decimal? value)
        {
            SetParameterValue(@this, index, value.HasValue ? value.Value : null);
        }

        /// <summary>
        /// Sets the value of a decimal parameter in the command.
        /// </summary>
        /// <param name="this">The <see cref="IDbCommand"/> containing the parameter.</param>
        /// <param name="name">The name of the parameter.</param>
        /// <param name="value">The decimal value to set. If <c>null</c>, the parameter is set to <c>null</c>.</param>
        public static void SetDecimalParameterValue(this IDbCommand @this, string name, decimal? value)
        {
            SetParameterValue(@this, name, value.HasValue ? value.Value : null);
        }

        /// <summary>
        /// Sets the value of a DateTime parameter in the command.
        /// </summary>
        /// <param name="this">The <see cref="IDbCommand"/> containing the parameter.</param>
        /// <param name="index">The index of the parameter.</param>
        /// <param name="value">The DateTime value to set. If <c>null</c>, the parameter is set to <c>null</c>.</param>
        public static void SetDateTimeParameterValue(this IDbCommand @this, int index, DateTime? value)
        {
            SetParameterValue(@this, index, value.HasValue ? value.Value : null);
        }

        /// <summary>
        /// Sets the value of a DateTime parameter in the command.
        /// </summary>
        /// <param name="this">The <see cref="IDbCommand"/> containing the parameter.</param>
        /// <param name="name">The name of the parameter.</param>
        /// <param name="value">The DateTime value to set. If <c>null</c>, the parameter is set to <c>null</c>.</param>
        public static void SetDateTimeParameterValue(this IDbCommand @this, string name, DateTime? value)
        {
            SetParameterValue(@this, name, value.HasValue ? value.Value : null);
        }

        /// <summary>
        /// Sets the value of a DateTimeOffset parameter in the command.
        /// </summary>
        /// <param name="this">The <see cref="IDbCommand"/> containing the parameter.</param>
        /// <param name="index">The index of the parameter.</param>
        /// <param name="value">The DateTimeOffset value to set. If <c>null</c>, the parameter is set to <c>null</c>.</param>
        public static void SetDateTimeOffsetParameterValue(this IDbCommand @this, int index, DateTimeOffset? value)
        {
            SetParameterValue(@this, index, value.HasValue ? value.Value : null);
        }

        /// <summary>
        /// Sets the value of a DateTimeOffset parameter in the command.
        /// </summary>
        /// <param name="this">The <see cref="IDbCommand"/> containing the parameter.</param>
        /// <param name="name">The name of the parameter.</param>
        /// <param name="value">The DateTimeOffset value to set. If <c>null</c>, the parameter is set to <c>null</c>.</param>
        public static void SetDateTimeOffsetParameterValue(this IDbCommand @this, string name, DateTimeOffset? value)
        {
            SetParameterValue(@this, name, value.HasValue ? value.Value : null);
        }

        /// <summary>
        /// Sets the value of a TimeSpan parameter in the command.
        /// </summary>
        /// <param name="this">The <see cref="IDbCommand"/> containing the parameter.</param>
        /// <param name="index">The index of the parameter.</param>
        /// <param name="value">The TimeSpan value to set. If <c>null</c>, the parameter is set to <c>null</c>.</param>
        public static void SetTimeParameterValue(this IDbCommand @this, int index, TimeSpan? value)
        {
            SetParameterValue(@this, index, value.HasValue ? value.Value : null);
        }

        /// <summary>
        /// Sets the value of a TimeSpan parameter in the command.
        /// </summary>
        /// <param name="this">The <see cref="IDbCommand"/> containing the parameter.</param>
        /// <param name="name">The name of the parameter.</param>
        /// <param name="value">The TimeSpan value to set. If <c>null</c>, the parameter is set to <c>null</c>.</param>
        public static void SetTimeParameterValue(this IDbCommand @this, string name, TimeSpan? value)
        {
            SetParameterValue(@this, name, value.HasValue ? value.Value : null);
        }

        /// <summary>
        /// Sets the value of a string parameter in the command.
        /// </summary>
        /// <param name="this">The <see cref="IDbCommand"/> containing the parameter.</param>
        /// <param name="index">The index of the parameter.</param>
        /// <param name="value">The string value to set. If <c>null</c>, the parameter is set to <c>null</c>.</param>
        public static void SetStringParameterValue(this IDbCommand @this, int index, string? value)
        {
            SetParameterValue(@this, index, value);
        }

        /// <summary>
        /// Sets the value of a string parameter in the command.
        /// </summary>
        /// <param name="this">The <see cref="IDbCommand"/> containing the parameter.</param>
        /// <param name="name">The name of the parameter.</param>
        /// <param name="value">The string value to set. If <c>null</c>, the parameter is set to <c>null</c>.</param>
        public static void SetStringParameterValue(this IDbCommand @this, string name, string? value)
        {
            SetParameterValue(@this, name, value);
        }

        /// <summary>
        /// Sets the value of a character parameter in the command.
        /// </summary>
        /// <param name="this">The <see cref="IDbCommand"/> containing the parameter.</param>
        /// <param name="index">The index of the parameter.</param>
        /// <param name="value">The character value to set. If <c>null</c>, the parameter is set to <c>null</c>.</param>
        public static void SetCharParameterValue(this IDbCommand @this, int index, char? value)
        {
            SetParameterValue(@this, index, value.HasValue ? value.Value : null);
        }

        /// <summary>
        /// Sets the value of a character parameter in the command.
        /// </summary>
        /// <param name="this">The <see cref="IDbCommand"/> containing the parameter.</param>
        /// <param name="name">The name of the parameter.</param>
        /// <param name="value">The character value to set. If <c>null</c>, the parameter is set to <c>null</c>.</param>
        public static void SetCharParameterValue(this IDbCommand @this, string name, char? value)
        {
            SetParameterValue(@this, name, value.HasValue ? value.Value : null);
        }

        /// <summary>
        /// Sets the value of a GUID parameter in the command.
        /// </summary>
        /// <param name="this">The <see cref="IDbCommand"/> containing the parameter.</param>
        /// <param name="index">The index of the parameter.</param>
        /// <param name="value">The GUID value to set. If <c>null</c>, the parameter is set to <c>null</c>.</param>
        public static void SetGuidParameterValue(this IDbCommand @this, int index, Guid? value)
        {
            SetParameterValue(@this, index, value.HasValue ? value.Value : null);
        }

        /// <summary>
        /// Sets the value of a GUID parameter in the command.
        /// </summary>
        /// <param name="this">The <see cref="IDbCommand"/> containing the parameter.</param>
        /// <param name="name">The name of the parameter.</param>
        /// <param name="value">The GUID value to set. If <c>null</c>, the parameter is set to <c>null</c>.</param>
        public static void SetGuidParameterValue(this IDbCommand @this, string name, Guid? value)
        {
            SetParameterValue(@this, name, value.HasValue ? value.Value : null);
        }

        /// <summary>
        /// Sets the value of a binary parameter in the command.
        /// </summary>
        /// <param name="this">The <see cref="IDbCommand"/> containing the parameter.</param>
        /// <param name="index">The index of the parameter.</param>
        /// <param name="value">The binary value to set. If <c>null</c>, the parameter is set to <c>null</c>.</param>
        public static void SetBinaryParameterValue(this IDbCommand @this, int index, byte[]? value)
        {
            SetParameterValue(@this, index, value);
        }

        /// <summary>
        /// Sets the value of a binary parameter in the command.
        /// </summary>
        /// <param name="this">The <see cref="IDbCommand"/> containing the parameter.</param>
        /// <param name="name">The name of the parameter.</param>
        /// <param name="value">The binary value to set. If <c>null</c>, the parameter is set to <c>null</c>.</param>
        public static void SetBinaryParameterValue(this IDbCommand @this, string name, byte[]? value)
        {
            SetParameterValue(@this, name, value);
        }

        #endregion

        #region Cast

        /// <summary>
        /// Casts the current <see cref="IDbCommand"/> to the specified type and performs the given action.
        /// </summary>
        /// <typeparam name="TCommand">The type to cast the command to.</typeparam>
        /// <param name="this">The current <see cref="IDbCommand"/> instance.</param>
        /// <param name="action">The action to perform on the casted command.</param>
        /// <exception cref="InvalidCastException">Thrown if the command is not of the specified type.</exception>
        public static void Cast<TCommand>(this IDbCommand @this, Action<TCommand> action) where TCommand : IDbCommand
        {
            var command = @this.Find<TCommand>();

            if (command == null) { throw new InvalidCastException($"The command is not of type {typeof(TCommand).Name}."); }

            action(command);
        }

        /// <summary>
        /// Casts the current <see cref="IDbCommand"/> to the specified type and executes the given function.
        /// </summary>
        /// <typeparam name="TCommand">The type to cast the command to.</typeparam>
        /// <typeparam name="TResult">The return type of the function.</typeparam>
        /// <param name="this">The current <see cref="IDbCommand"/> instance.</param>
        /// <param name="func">The function to execute on the casted command.</param>
        /// <returns>The result of the function.</returns>
        /// <exception cref="InvalidCastException">Thrown if the command is not of the specified type.</exception>
        public static TResult Cast<TCommand, TResult>(this IDbCommand @this, Func<TCommand, TResult> func) where TCommand : IDbCommand
        {
            var command = @this.Find<TCommand>();

            if (command == null) { throw new InvalidCastException($"The command is not of type {typeof(TCommand).Name}."); }

            return func(command);
        }

        /// <summary>
        /// Finds the command of the specified type.
        /// </summary>
        /// <typeparam name="TCommand">The type of the command to find.</typeparam>
        /// <param name="this">The database command.</param>
        /// <returns>The command of the specified type, or <c>null</c> if not found.</returns>
        private static TCommand? Find<TCommand>(this IDbCommand @this) where TCommand : IDbCommand
        {
            if (@this is TCommand command)
            {
                return command;
            }

            if (@this is Wrappers.DbCommandWithFilter wrapper)
            {
                return wrapper.WrappedCommand.Find<TCommand>();
            }

            return default;
        }

        #endregion
    }
}