using System;
using System.Collections.Generic;
using System.Data;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace mxProject.Data
{
    /// <summary>
    /// Represents a filter for database command.
    /// </summary>
    public interface IDbCommandFilter
    {
        /// <summary>
        /// Gets the methods to filter.
        /// </summary>
        DbCommandFilterTargets TargetMethods { get; }

        /// <summary>
        /// Disposes the specified command.
        /// </summary>
        /// <param name="command">The database command to dispose.</param>
        /// <param name="continuation">The continuation action to execute after disposing.</param>
        void Dispose(IDbCommand command, Action<IDbCommand> continuation);

        /// <summary>
        /// Prepares the specified command.
        /// </summary>
        /// <param name="command">The database command to prepare.</param>
        /// <param name="continuation">The continuation action to execute after preparing.</param>
        void Prepare(IDbCommand command, Action<IDbCommand> continuation);

        /// <summary>
        /// Cancels the execution of the specified command.
        /// </summary>
        /// <param name="command">The database command to cancel.</param>
        /// <param name="continuation">The continuation action to execute after canceling.</param>
        void Cancel(IDbCommand command, Action<IDbCommand> continuation);

        /// <summary>
        /// Creates a parameter for the specified command.
        /// </summary>
        /// <param name="command">The database command for which to create a parameter.</param>
        /// <param name="continuation">The continuation function to execute after creating the parameter.</param>
        /// <returns>The created parameter.</returns>
        IDbDataParameter CreateParameter(IDbCommand command, Func<IDbCommand, IDbDataParameter> continuation);

        /// <summary>
        /// Executes the command and returns a data reader.
        /// </summary>
        /// <param name="command">The database command to execute.</param>
        /// <param name="behavior">The behavior of the command execution.</param>
        /// <param name="continuation">The continuation function to execute after reading.</param>
        /// <returns>The data reader.</returns>
        IDataReader ExecuteReader(IDbCommand command, CommandBehavior behavior, Func<IDbCommand, CommandBehavior, IDataReader> continuation);

        /// <summary>
        /// Executes the command and returns a scalar value.
        /// </summary>
        /// <param name="command">The database command to execute.</param>
        /// <param name="continuation">The continuation function to execute after execution.</param>
        /// <returns>The scalar value.</returns>
        object ExecuteScalar(IDbCommand command, Func<IDbCommand, object> continuation);

        /// <summary>
        /// Executes the command and returns the number of rows affected.
        /// </summary>
        /// <param name="command">The database command to execute.</param>
        /// <param name="continuation">The continuation function to execute after execution.</param>
        /// <returns>The number of rows affected.</returns>
        int ExecuteNonQuery(IDbCommand command, Func<IDbCommand, int> continuation);
    }
}
