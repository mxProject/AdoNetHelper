using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Common;
using System.Threading;
using System.Threading.Tasks;
using mxProject.Data.Extensions;
using mxProject.Data.Wrappers;

namespace mxProject.Data
{
    /// <summary>
    /// Provides extension methods for <see cref="IDbCommandFilter"/> arrays to handle database command operations.
    /// </summary>
    public static class IDbCommandFilterExtensions
    {
        #region Dispose

        /// <summary>
        /// Disposes the specified database command using the provided filters.
        /// </summary>
        /// <param name="this">The array of <see cref="IDbCommandFilter"/>.</param>
        /// <param name="command">The database command to dispose.</param>
        public static void Dispose(this IDbCommandFilter[] @this, IDbCommand command)
        {
            if (@this == null || @this.Length == 0)
            {
                command.Dispose();
            }
            else
            {
                FilterUtility.InvokeAction(
                    @this,
                    command,
                    _command => _command.Dispose(),
                    (_filter, _command, _continuation) => _filter.Dispose(_command, _continuation)
                    );
            }
        }

        #endregion

        #region Prepare

        /// <summary>
        /// Prepares the specified database command using the provided filters.
        /// </summary>
        /// <param name="this">The array of <see cref="IDbCommandFilter"/>.</param>
        /// <param name="command">The database command to prepare.</param>
        public static void Prepare(this IDbCommandFilter[] @this, IDbCommand command)
        {
            if (@this == null || @this.Length == 0)
            {
                command.Prepare();
            }
            else
            {
                FilterUtility.InvokeAction(
                    @this,
                    command,
                    _command => _command.Prepare(),
                    (_filter, _command, _continuation) => _filter.Prepare(_command, _continuation)
                    );
            }
        }

        #endregion

        #region Cancel

        /// <summary>
        /// Cancels the execution of the specified database command using the provided filters.
        /// </summary>
        /// <param name="this">The array of <see cref="IDbCommandFilter"/>.</param>
        /// <param name="command">The database command to cancel.</param>
        public static void Cancel(this IDbCommandFilter[] @this, IDbCommand command)
        {
            if (@this == null || @this.Length == 0)
            {
                command.Cancel();
            }
            else
            {
                FilterUtility.InvokeAction(
                    @this,
                    command,
                    _command => _command.Cancel(),
                    (_filter, _command, _continuation) => _filter.Cancel(_command, _continuation)
                    );
            }
        }

        #endregion

        #region CreateParameter

        /// <summary>
        /// Creates a new parameter for the specified database command using the provided filters.
        /// </summary>
        /// <param name="this">The array of <see cref="IDbCommandFilter"/>.</param>
        /// <param name="command">The database command for which to create the parameter.</param>
        /// <returns>A new <see cref="IDbDataParameter"/> object.</returns>
        public static IDbDataParameter CreateParameter(this IDbCommandFilter[] @this, IDbCommand command)
        {
            if (@this == null || @this.Length == 0)
            {
                return command.CreateParameter();
            }
            else
            {
                return FilterUtility.InvokeFunc(
                    @this,
                    command,
                    _command => _command.CreateParameter(),
                    (_filter, _command, _continuation) => _filter.CreateParameter(_command, _continuation)
                );
            }
        }

        #endregion

        #region ExecuteReader

        /// <summary>
        /// Executes the command text against the connection and builds an <see cref="IDataReader"/> using the provided filters.
        /// </summary>
        /// <param name="this">The array of <see cref="IDbCommandFilter"/>.</param>
        /// <param name="command">The database command to execute.</param>
        /// <param name="behavior">The behavior of the command.</param>
        /// <returns>An <see cref="IDataReader"/> object.</returns>
        public static IDataReader ExecuteReader(this IDbCommandFilter[] @this, IDbCommand command, CommandBehavior behavior)
        {
            if (@this == null || @this.Length == 0)
            {
                return command.ExecuteReader(behavior);
            }
            else
            {
                return FilterUtility.InvokeFunc(
                    @this,
                    command,
                    behavior,
                    (_command, _behavior) => _command.ExecuteReader(behavior),
                    (_filter, _command, _behavior, _continuation) => _filter.ExecuteReader(_command, _behavior, _continuation)
                    );
            }
        }

        #endregion

        #region ExecuteScalar

        /// <summary>
        /// Executes the query and returns the first column of the first row in the result set returned by the query using the provided filters.
        /// </summary>
        /// <param name="this">The array of <see cref="IDbCommandFilter"/>.</param>
        /// <param name="command">The database command to execute.</param>
        /// <returns>The first column of the first row in the result set.</returns>
        public static object ExecuteScalar(this IDbCommandFilter[] @this, IDbCommand command)
        {
            if (@this == null || @this.Length == 0)
            {
                return command.ExecuteScalar();
            }
            else
            {
                return FilterUtility.InvokeFunc(
                    @this,
                    command,
                    _command => _command.ExecuteScalar(),
                    (_filter, _command, _continuation) => _filter.ExecuteScalar(_command, _continuation)
                    );
            }
        }

        #endregion

        #region ExecuteNonQuery

        /// <summary>
        /// Executes a SQL statement against the connection and returns the number of rows affected using the provided filters.
        /// </summary>
        /// <param name="this">The array of <see cref="IDbCommandFilter"/>.</param>
        /// <param name="command">The database command to execute.</param>
        /// <returns>The number of rows affected.</returns>
        public static int ExecuteNonQuery(this IDbCommandFilter[] @this, IDbCommand command)
        {
            if (@this == null || @this.Length == 0)
            {
                return command.ExecuteNonQuery();
            }
            else
            {
                return FilterUtility.InvokeFunc(
                    @this,
                    command,
                    _command => _command.ExecuteNonQuery(),
                    (_filter, _command, _continuation) => _filter.ExecuteNonQuery(_command, _continuation)
                    );
            }
        }

        #endregion
    }
}