using System;
using System.Collections.Generic;
using System.Data;
using System.Text;

namespace mxProject.Data
{
    /// <summary>
    /// DbCommand Activator.
    /// </summary>
    internal readonly struct DbCommandActivator
    {
        /// <summary>
        /// Creates a new instance.
        /// </summary>
        /// <param name="connection">The connection.</param>
        /// <param name="transaction">The transaction.</param>
        /// <param name="configureCommand">A method to configure a command before execution.</param>
        internal DbCommandActivator(IDbConnection connection, IDbTransaction? transaction = null, Action<IDbCommand>? configureCommand = null)
        {
            m_Connection = connection;
            m_Transaction = transaction;
            m_ConfigureCommand = configureCommand;
        }

        /// <summary>
        /// Creates a new instance.
        /// </summary>
        /// <param name="transaction">The transaction.</param>
        /// <param name="configureCommand">A method to configure a command before execution.</param>
        internal DbCommandActivator(IDbTransaction transaction, Action<IDbCommand>? configureCommand = null)
        {
            m_Connection = null;
            m_Transaction = transaction;
            m_ConfigureCommand = configureCommand;
        }

        private readonly IDbConnection? m_Connection;
        private readonly IDbTransaction? m_Transaction;
        private readonly Action<IDbCommand>? m_ConfigureCommand;

        /// <summary>
        /// Creates a new command.
        /// </summary>
        /// <returns>The command.</returns>
        /// <exception cref="NullReferenceException">
        /// "The connection is null."
        /// </exception>
        internal IDbCommand CreateCommand()
        {
            var connection = m_Connection ?? m_Transaction?.Connection;

            if (connection == null)
            {
                throw new NullReferenceException("The connection is null.");
            }

            var command = connection.CreateCommand();

            if (m_Transaction != null)
            {
                command.Transaction = m_Transaction;
            }

            if (m_ConfigureCommand != null)
            {
                m_ConfigureCommand(command);
            }

            return command;
        }
    }
}
