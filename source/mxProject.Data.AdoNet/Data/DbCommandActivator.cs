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
        internal DbCommandActivator(IDbConnection connection, IDbTransaction? transaction = null)
        {
            m_Connection = connection;
            m_Transaction = transaction;
        }

        /// <summary>
        /// Creates a new instance.
        /// </summary>
        /// <param name="transaction">The transaction.</param>
        internal DbCommandActivator(IDbTransaction transaction)
        {
            m_Connection = null;
            m_Transaction = transaction;
        }

        private readonly IDbConnection? m_Connection;
        private readonly IDbTransaction? m_Transaction;

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

            return command;
        }
    }
}
