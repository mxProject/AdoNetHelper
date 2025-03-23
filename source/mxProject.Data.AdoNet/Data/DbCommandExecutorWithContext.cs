using System;
using System.Collections.Generic;
using System.Data;
using System.Text;
using System.Transactions;

namespace mxProject.Data
{
    /// <summary>
    /// Perform the database operation.
    /// </summary>
    /// <typeparam name="TContext">The context type.</typeparam>
    public class DbCommandExecutorWithContext<TContext>
    {
        /// <summary>
        /// Creates a new instance.
        /// </summary>
        /// <param name="connectionActivator">The method to activate a connection.</param>
        /// <param name="useTransactionScope">A value that indicates whether to use ambient transactions using TransactionScope.</param>
        public DbCommandExecutorWithContext(Func<IDbConnection> connectionActivator, bool useTransactionScope)
        {
            m_ConnectionActivator = connectionActivator;
            UseTransactionScope = useTransactionScope;
        }

        private readonly Func<IDbConnection> m_ConnectionActivator;

        #region TransactionScope

        /// <summary>
        /// Gets a value that indicates whether to use ambient transactions using TransactionScope.
        /// </summary>
        public bool UseTransactionScope { get; }

        /// <summary>
        /// Gets whether there is an active TransactionScope.
        /// </summary>
        /// <returns></returns>
        private bool ExistsActiveTransactionScope()
        {
            return TransactionScopeUtility.ExistsActiveTransactionScope();
        }

        #endregion

        #region ExecuteOnNewConnection

        /// <summary>
        /// Executes the specified action on the new connection.
        /// </summary>
        /// <param name="action">The action.</param>
        /// <param name="context">The current context.</param>
        public void ExecuteOnNewConnection(Action<Func<IDbCommand>, TContext> action, TContext context)
        {
            using var connection = m_ConnectionActivator();

            connection.Open();

            if (UseTransactionScope && ExistsActiveTransactionScope())
            {
                // Use ambient transaction.
                action(new DbCommandActivator(connection).CreateCommand, context);
            }
            else
            {
                using var transaction = connection.BeginTransaction();

                action(new DbCommandActivator(connection, transaction).CreateCommand, context);

                transaction.Commit();
            }
        }

        /// <summary>
        /// Executes the specified function on the new connection.
        /// </summary>
        /// <typeparam name="TResult">The result type of the function.</typeparam>
        /// <param name="func">The function.</param>
        /// <param name="context">The current context.</param>
        /// <returns>The result of the function.</returns>
        public TResult ExecuteOnNewConnection<TResult>(Func<Func<IDbCommand>, TContext, TResult> func, TContext context)
        {
            using var connection = m_ConnectionActivator();

            connection.Open();

            if (UseTransactionScope && ExistsActiveTransactionScope())
            {
                // Use ambient transaction.
                return func(new DbCommandActivator(connection).CreateCommand, context);
            }
            else
            {
                using var transaction = connection.BeginTransaction();

                var result = func(new DbCommandActivator(connection, transaction).CreateCommand, context);

                transaction.Commit();

                return result;
            }
        }

        /// <summary>
        /// Executes the specified function on the new connection.
        /// </summary>
        /// <typeparam name="TResult">The result type of the function.</typeparam>
        /// <param name="func">The function.</param>
        /// <param name="context">The current context.</param>
        /// <returns>The result of the function.</returns>
        public IEnumerable<TResult> ExecuteIteratorOnNewConnection<TResult>(Func<Func<IDbCommand>, TContext, IEnumerable<TResult>> func, TContext context)
        {
            using var connection = m_ConnectionActivator();

            connection.Open();

            if (UseTransactionScope && ExistsActiveTransactionScope())
            {
                // Use ambient transaction.
                foreach (var result in func(new DbCommandActivator(connection).CreateCommand, context))
                {
                    yield return result;
                }
            }
            else
            {
                using var transaction = connection.BeginTransaction();

                foreach (var result in func(new DbCommandActivator(connection, transaction).CreateCommand, context))
                {
                    yield return result;
                }

                transaction.Commit();
            }
        }

        /// <summary>
        /// Executes the specified action on the new connection.
        /// </summary>
        /// <typeparam name="TState">The type of the state.</typeparam>
        /// <param name="state">The state.</param>
        /// <param name="action">The action.</param>
        /// <param name="context">The current context.</param>
        public void ExecuteOnNewConnection<TState>(TState state, Action<TState, Func<IDbCommand>, TContext> action, TContext context)
        {
            using var connection = m_ConnectionActivator();

            connection.Open();

            if (UseTransactionScope && ExistsActiveTransactionScope())
            {
                // Use ambient transaction.
                action(state, new DbCommandActivator(connection).CreateCommand, context);
            }
            else
            {
                using var transaction = connection.BeginTransaction();

                action(state, new DbCommandActivator(connection, transaction).CreateCommand, context);

                transaction.Commit();
            }
        }

        /// <summary>
        /// Executes the specified function on the new connection.
        /// </summary>
        /// <typeparam name="TState">The type of the state.</typeparam>
        /// <typeparam name="TResult">The result type of the function.</typeparam>
        /// <param name="state">The state.</param>
        /// <param name="func">The function.</param>
        /// <param name="context">The current context.</param>
        /// <returns>The result of the function.</returns>
        public TResult ExecuteOnNewConnection<TState, TResult>(TState state, Func<TState, Func<IDbCommand>, TContext, TResult> func, TContext context)
        {
            using var connection = m_ConnectionActivator();

            connection.Open();

            if (UseTransactionScope && ExistsActiveTransactionScope())
            {
                // Use ambient transaction.
                return func(state, new DbCommandActivator(connection).CreateCommand, context);
            }
            else
            {
                using var transaction = connection.BeginTransaction();

                var result = func(state, new DbCommandActivator(connection, transaction).CreateCommand, context);

                transaction.Commit();

                return result;
            }
        }

        /// <summary>
        /// Executes the specified function on the new connection.
        /// </summary>
        /// <typeparam name="TState">The type of the state.</typeparam>
        /// <typeparam name="TResult">The result type of the function.</typeparam>
        /// <param name="state">The state.</param>
        /// <param name="func">The function.</param>
        /// <param name="context">The current context.</param>
        /// <returns>The result of the function.</returns>
        public IEnumerable<TResult> ExecuteIteratorOnNewConnection<TState, TResult>(TState state, Func<TState, Func<IDbCommand>, TContext, IEnumerable<TResult>> func, TContext context)
        {
            using var connection = m_ConnectionActivator();

            connection.Open();

            if (UseTransactionScope && ExistsActiveTransactionScope())
            {
                // Use ambient transaction.
                foreach (var result in func(state, new DbCommandActivator(connection).CreateCommand, context))
                {
                    yield return result;
                }
            }
            else
            {
                using var transaction = connection.BeginTransaction();

                foreach( var result in func(state, new DbCommandActivator(connection, transaction).CreateCommand, context))
                {
                    yield return result;
                }

                transaction.Commit();
            }
        }

        #endregion

        #region ExecuteOnConnection

        /// <summary>
        /// Executes the specified action on the specified connection.
        /// </summary>
        /// <param name="connection">The currernt connection.</param>
        /// <param name="action">The action.</param>
        /// <param name="context">The current context.</param>
        public void ExecuteOnConnection(IDbConnection connection, Action<Func<IDbCommand>, TContext> action, TContext context)
        {
            action(new DbCommandActivator(connection).CreateCommand, context);
        }

        /// <summary>
        /// Executes the specified function on the specified connection.
        /// </summary>
        /// <typeparam name="TResult">The result type of the function.</typeparam>
        /// <param name="connection">The currernt connection.</param>
        /// <param name="func">The function.</param>
        /// <param name="context">The current context.</param>
        /// <returns>The result of the function.</returns>
        public TResult ExecuteOnConnection<TResult>(IDbConnection connection, Func<Func<IDbCommand>, TContext, TResult> func, TContext context)
        {
            return func(new DbCommandActivator(connection).CreateCommand, context);
        }

        /// <summary>
        /// Executes the specified action on the specified connection.
        /// </summary>
        /// <typeparam name="TState">The type of the state.</typeparam>
        /// <param name="state">The state.</param>
        /// <param name="connection">The currernt connection.</param>
        /// <param name="action">The action.</param>
        /// <param name="context">The current context.</param>
        public void ExecuteOnConnection<TState>(TState state, IDbConnection connection, Action<TState, Func<IDbCommand>, TContext> action, TContext context)
        {
            action(state, new DbCommandActivator(connection).CreateCommand, context);
        }

        /// <summary>
        /// Executes the specified function on the specified connection.
        /// </summary>
        /// <typeparam name="TState">The type of the state.</typeparam>
        /// <typeparam name="TResult">The result type of the function.</typeparam>
        /// <param name="state">The state.</param>
        /// <param name="connection">The currernt connection.</param>
        /// <param name="func">The function.</param>
        /// <param name="context">The current context.</param>
        /// <returns>The result of the function.</returns>
        public TResult ExecuteOnConnection<TState, TResult>(TState state, IDbConnection connection, Func<TState, Func<IDbCommand>, TContext, TResult> func, TContext context)
        {
            return func(state, new DbCommandActivator(connection).CreateCommand, context);
        }

        #endregion

        #region ExecuteOnTransaction

        /// <summary>
        /// Executes the specified action on the specified transaction.
        /// </summary>
        /// <param name="transaction">The currernt transaction.</param>
        /// <param name="action">The action.</param>
        /// <param name="context">The current context.</param>
        public void ExecuteOnTransaction(IDbTransaction transaction, Action<Func<IDbCommand>, TContext> action, TContext context)
        {
            action(new DbCommandActivator(transaction).CreateCommand, context);
        }

        /// <summary>
        /// Executes the specified function on the specified transaction.
        /// </summary>
        /// <typeparam name="TResult">The result type of the function.</typeparam>
        /// <param name="transaction">The currernt transaction.</param>
        /// <param name="func">The function.</param>
        /// <param name="context">The current context.</param>
        /// <returns>The result of the function.</returns>
        public TResult ExecuteOnTransaction<TResult>(IDbTransaction transaction, Func<Func<IDbCommand>, TContext, TResult> func, TContext context)
        {
            return func(new DbCommandActivator(transaction).CreateCommand, context);
        }

        /// <summary>
        /// Executes the specified action on the specified transaction.
        /// </summary>
        /// <typeparam name="TState">The type of the state.</typeparam>
        /// <param name="state">The state.</param>
        /// <param name="transaction">The currernt transaction.</param>
        /// <param name="action">The action.</param>
        /// <param name="context">The current context.</param>
        public void ExecuteOnTransaction<TState>(TState state, IDbTransaction transaction, Action<TState, Func<IDbCommand>, TContext> action, TContext context)
        {
            action(state, new DbCommandActivator(transaction).CreateCommand, context);
        }

        /// <summary>
        /// Executes the specified function on the specified transaction.
        /// </summary>
        /// <typeparam name="TState">The type of the state.</typeparam>
        /// <typeparam name="TResult">The result type of the function.</typeparam>
        /// <param name="state">The state.</param>
        /// <param name="transaction">The currernt transaction.</param>
        /// <param name="func">The function.</param>
        /// <param name="context">The current context.</param>
        /// <returns>The result of the function.</returns>
        public TResult ExecuteOnTransaction<TState, TResult>(TState state, IDbTransaction transaction, Func<TState, Func<IDbCommand>, TContext, TResult> func, TContext context)
        {
            return func(state, new DbCommandActivator(transaction).CreateCommand, context);
        }

        #endregion
    }
}
