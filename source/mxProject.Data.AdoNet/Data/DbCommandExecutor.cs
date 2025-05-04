using System;
using System.Collections.Generic;
using System.Data;
using System.Text;
using System.Transactions;
using System.Threading.Tasks;

namespace mxProject.Data
{
    /// <summary>
    /// Perform the database operation.
    /// </summary>
    public class DbCommandExecutor
    {
        /// <summary>
        /// Creates a new instance.
        /// </summary>
        /// <param name="connectionActivator">A method to activate a connection.</param>
        /// <param name="useTransactionScope">A value that indicates whether to use ambient transactions using TransactionScope.</param>
        public DbCommandExecutor(Func<IDbConnection> connectionActivator, bool useTransactionScope)
        {
            m_ConnectionActivator = connectionActivator;
            UseTransactionScope = useTransactionScope;
            m_ConfigureCommand = null;
        }

        /// <summary>
        /// Creates a new instance.
        /// </summary>
        /// <param name="connectionActivator">A method to activate a connection.</param>
        /// <param name="useTransactionScope">A value that indicates whether to use ambient transactions using TransactionScope.</param>
        /// <param name="configureCommand">A method to configure a command before execution.</param>
        public DbCommandExecutor(Func<IDbConnection> connectionActivator, bool useTransactionScope, Action<IDbCommand> configureCommand)
        {
            m_ConnectionActivator = connectionActivator;
            UseTransactionScope = useTransactionScope;
            m_ConfigureCommand = configureCommand;
        }

        private readonly Func<IDbConnection> m_ConnectionActivator;
        private readonly Action<IDbCommand>? m_ConfigureCommand;

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
        public void ExecuteOnNewConnection(Action<Func<IDbCommand>> action)
        {
            using var connection = m_ConnectionActivator();

            connection.Open();

            if (UseTransactionScope && ExistsActiveTransactionScope())
            {
                // Use ambient transaction.
                action(new DbCommandActivator(connection, configureCommand: m_ConfigureCommand).CreateCommand);
            }
            else
            {
                using var transaction = connection.BeginTransaction();

                action(new DbCommandActivator(connection, transaction, m_ConfigureCommand).CreateCommand);

                transaction.Commit();
            }
        }

        /// <summary>
        /// Executes the specified function on the new connection.
        /// </summary>
        /// <typeparam name="TResult">The result type of the function.</typeparam>
        /// <param name="func">The function.</param>
        /// <returns>The result of the function.</returns>
        public TResult ExecuteOnNewConnection<TResult>(Func<Func<IDbCommand>, TResult> func)
        {
            using var connection = m_ConnectionActivator();

            connection.Open();

            if (UseTransactionScope && ExistsActiveTransactionScope())
            {
                // Use ambient transaction.
                return func(new DbCommandActivator(connection, configureCommand: m_ConfigureCommand).CreateCommand);
            }
            else
            {
                using var transaction = connection.BeginTransaction();

                var result = func(new DbCommandActivator(connection, transaction, m_ConfigureCommand).CreateCommand);

                transaction.Commit();

                return result;
            }
        }

        /// <summary>
        /// Executes the specified function on the new connection.
        /// </summary>
        /// <typeparam name="TResult">The result type of the function.</typeparam>
        /// <param name="func">The function.</param>
        /// <returns>The result of the function.</returns>
        public IEnumerable<TResult> ExecuteIteratorOnNewConnection<TResult>(Func<Func<IDbCommand>, IEnumerable<TResult>> func)
        {
            using var connection = m_ConnectionActivator();

            connection.Open();

            if (UseTransactionScope && ExistsActiveTransactionScope())
            {
                // Use ambient transaction.
                foreach (var result in func(new DbCommandActivator(connection, configureCommand: m_ConfigureCommand).CreateCommand))
                {
                    yield return result;
                }
            }
            else
            {
                using var transaction = connection.BeginTransaction();

                foreach(var result in func(new DbCommandActivator(connection, transaction, m_ConfigureCommand).CreateCommand))
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
        public void ExecuteOnNewConnection<TState>(TState state, Action<TState, Func<IDbCommand>> action)
        {
            using var connection = m_ConnectionActivator();

            connection.Open();

            if (UseTransactionScope && ExistsActiveTransactionScope())
            {
                // Use ambient transaction.
                action(state, new DbCommandActivator(connection, configureCommand: m_ConfigureCommand).CreateCommand);
            }
            else
            {
                using var transaction = connection.BeginTransaction();

                action(state, new DbCommandActivator(connection, transaction, m_ConfigureCommand).CreateCommand);

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
        /// <returns>The result of the function.</returns>
        public TResult ExecuteOnNewConnection<TState, TResult>(TState state, Func<TState, Func<IDbCommand>, TResult> func)
        {
            using var connection = m_ConnectionActivator();

            connection.Open();

            if (UseTransactionScope && ExistsActiveTransactionScope())
            {
                // Use ambient transaction.
                return func(state, new DbCommandActivator(connection, configureCommand: m_ConfigureCommand).CreateCommand);
            }
            else
            {
                using var transaction = connection.BeginTransaction();

                var result = func(state, new DbCommandActivator(connection, transaction, m_ConfigureCommand).CreateCommand);

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
        /// <returns>The result of the function.</returns>
        public IEnumerable<TResult> ExecuteIteratorOnNewConnection<TState, TResult>(TState state, Func<TState, Func<IDbCommand>, IEnumerable<TResult>> func)
        {
            using var connection = m_ConnectionActivator();

            connection.Open();

            if (UseTransactionScope && ExistsActiveTransactionScope())
            {
                // Use ambient transaction.
                foreach (var result in func(state, new DbCommandActivator(connection, configureCommand: m_ConfigureCommand).CreateCommand))
                {
                    yield return result;
                }
            }
            else
            {
                using var transaction = connection.BeginTransaction();

                foreach (var result in func(state, new DbCommandActivator(connection, transaction, m_ConfigureCommand).CreateCommand))
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
        public void ExecuteOnConnection(IDbConnection connection, Action<Func<IDbCommand>> action)
        {
            action(new DbCommandActivator(connection, configureCommand: m_ConfigureCommand).CreateCommand);
        }

        /// <summary>
        /// Executes the specified function on the specified connection.
        /// </summary>
        /// <typeparam name="TResult">The result type of the function.</typeparam>
        /// <param name="connection">The currernt connection.</param>
        /// <param name="func">The function.</param>
        /// <returns>The result of the function.</returns>
        public TResult ExecuteOnConnection<TResult>(IDbConnection connection, Func<Func<IDbCommand>, TResult> func)
        {
            return func(new DbCommandActivator(connection, configureCommand: m_ConfigureCommand).CreateCommand);
        }

        /// <summary>
        /// Executes the specified action on the specified connection.
        /// </summary>
        /// <typeparam name="TState">The type of the state.</typeparam>
        /// <param name="state">The state.</param>
        /// <param name="connection">The currernt connection.</param>
        /// <param name="action">The action.</param>
        public void ExecuteOnConnection<TState>(TState state, IDbConnection connection, Action<TState, Func<IDbCommand>> action)
        {
            action(state, new DbCommandActivator(connection, configureCommand: m_ConfigureCommand).CreateCommand);
        }

        /// <summary>
        /// Executes the specified function on the specified connection.
        /// </summary>
        /// <typeparam name="TState">The type of the state.</typeparam>
        /// <typeparam name="TResult">The result type of the function.</typeparam>
        /// <param name="state">The state.</param>
        /// <param name="connection">The currernt connection.</param>
        /// <param name="func">The function.</param>
        /// <returns>The result of the function.</returns>
        public TResult ExecuteOnConnection<TState, TResult>(TState state, IDbConnection connection, Func<TState, Func<IDbCommand>, TResult> func)
        {
            return func(state, new DbCommandActivator(connection, configureCommand: m_ConfigureCommand).CreateCommand);
        }

        #endregion

        #region ExecuteOnTransaction

        /// <summary>
        /// Executes the specified action on the specified transaction.
        /// </summary>
        /// <param name="transaction">The currernt transaction.</param>
        /// <param name="action">The action.</param>
        public void ExecuteOnTransaction(IDbTransaction transaction, Action<Func<IDbCommand>> action)
        {
            action(new DbCommandActivator(transaction, m_ConfigureCommand).CreateCommand);
        }

        /// <summary>
        /// Executes the specified function on the specified transaction.
        /// </summary>
        /// <typeparam name="TResult">The result type of the function.</typeparam>
        /// <param name="transaction">The currernt transaction.</param>
        /// <param name="func">The function.</param>
        /// <returns>The result of the function.</returns>
        public TResult ExecuteOnTransaction<TResult>(IDbTransaction transaction, Func<Func<IDbCommand>, TResult> func)
        {
            return func(new DbCommandActivator(transaction, m_ConfigureCommand).CreateCommand);
        }

        /// <summary>
        /// Executes the specified action on the specified transaction.
        /// </summary>
        /// <typeparam name="TState">The type of the state.</typeparam>
        /// <param name="state">The state.</param>
        /// <param name="transaction">The currernt transaction.</param>
        /// <param name="action">The action.</param>
        public void ExecuteOnTransaction<TState>(TState state, IDbTransaction transaction, Action<TState, Func<IDbCommand>> action)
        {
            action(state, new DbCommandActivator(transaction, m_ConfigureCommand).CreateCommand);
        }

        /// <summary>
        /// Executes the specified function on the specified transaction.
        /// </summary>
        /// <typeparam name="TState">The type of the state.</typeparam>
        /// <typeparam name="TResult">The result type of the function.</typeparam>
        /// <param name="state">The state.</param>
        /// <param name="transaction">The currernt transaction.</param>
        /// <param name="func">The function.</param>
        /// <returns>The result of the function.</returns>
        public TResult ExecuteOnTransaction<TState, TResult>(TState state, IDbTransaction transaction, Func<TState, Func<IDbCommand>, TResult> func)
        {
            return func(state, new DbCommandActivator(transaction, m_ConfigureCommand).CreateCommand);
        }

        #endregion

        #region ExecuteOnNewConnectionAsync

        /// <summary>
        /// Asynchronously executes the specified action on the new connection.
        /// </summary>
        /// <param name="asyncAction">The action.</param>
        public async Task ExecuteOnNewConnectionAsync(Func<Func<IDbCommand>, Task> asyncAction)
        {
            using var connection = m_ConnectionActivator();

            connection.Open();

            if (UseTransactionScope && ExistsActiveTransactionScope())
            {
                // Use ambient transaction.
                await asyncAction(new DbCommandActivator(connection, configureCommand: m_ConfigureCommand).CreateCommand).ConfigureAwait(false);
            }
            else
            {
                using var transaction = connection.BeginTransaction();

                await asyncAction(new DbCommandActivator(connection, transaction, m_ConfigureCommand).CreateCommand).ConfigureAwait(false);

                transaction.Commit();
            }
        }
        
        /// <summary>
        /// Asynchronously executes the specified action on the new connection.
        /// </summary>
        /// <param name="asyncAction">The action.</param>
        public async ValueTask ExecuteOnNewConnectionAsync(Func<Func<IDbCommand>, ValueTask> asyncAction)
        {
            using var connection = m_ConnectionActivator();

            connection.Open();

            if (UseTransactionScope && ExistsActiveTransactionScope())
            {
                // Use ambient transaction.
                await asyncAction(new DbCommandActivator(connection, configureCommand: m_ConfigureCommand).CreateCommand).ConfigureAwait(false);
            }
            else
            {
                using var transaction = connection.BeginTransaction();

                await asyncAction(new DbCommandActivator(connection, transaction, m_ConfigureCommand).CreateCommand).ConfigureAwait(false);

                transaction.Commit();
            }
        }

        /// <summary>
        /// Asynchronously executes the specified function on the new connection.
        /// </summary>
        /// <typeparam name="TResult">The result type of the function.</typeparam>
        /// <param name="asyncFunc">The function.</param>
        /// <returns>The result of the function.</returns>
        public async Task<TResult> ExecuteOnNewConnectionAsync<TResult>(Func<Func<IDbCommand>, Task<TResult>> asyncFunc)
        {
            using var connection = m_ConnectionActivator();

            connection.Open();

            if (UseTransactionScope && ExistsActiveTransactionScope())
            {
                // Use ambient transaction.
                return await asyncFunc(new DbCommandActivator(connection, configureCommand: m_ConfigureCommand).CreateCommand).ConfigureAwait(false);
            }
            else
            {
                using var transaction = connection.BeginTransaction();

                var result = await asyncFunc(new DbCommandActivator(connection, transaction, m_ConfigureCommand).CreateCommand).ConfigureAwait(false);

                transaction.Commit();

                return result;
            }
        }

        /// <summary>
        /// Asynchronously executes the specified function on the new connection.
        /// </summary>
        /// <typeparam name="TResult">The result type of the function.</typeparam>
        /// <param name="asyncFunc">The function.</param>
        /// <returns>The result of the function.</returns>
        public async ValueTask<TResult> ExecuteOnNewConnectionAsync<TResult>(Func<Func<IDbCommand>, ValueTask<TResult>> asyncFunc)
        {
            using var connection = m_ConnectionActivator();

            connection.Open();

            if (UseTransactionScope && ExistsActiveTransactionScope())
            {
                // Use ambient transaction.
                return await asyncFunc(new DbCommandActivator(connection, configureCommand: m_ConfigureCommand).CreateCommand).ConfigureAwait(false);
            }
            else
            {
                using var transaction = connection.BeginTransaction();

                var result = await asyncFunc(new DbCommandActivator(connection, transaction, m_ConfigureCommand).CreateCommand).ConfigureAwait(false);

                transaction.Commit();

                return result;
            }
        }

        /// <summary>
        /// Asynchronously executes the specified function on the new connection.
        /// </summary>
        /// <typeparam name="TResult">The result type of the function.</typeparam>
        /// <param name="asyncFunc">The function.</param>
        /// <returns>The result of the function.</returns>
        public async IAsyncEnumerable<TResult> ExecuteIteratorOnNewConnectionAsync<TResult>(Func<Func<IDbCommand>, IAsyncEnumerable<TResult>> asyncFunc)
        {
            using var connection = m_ConnectionActivator();

            connection.Open();

            if (UseTransactionScope && ExistsActiveTransactionScope())
            {
                // Use ambient transaction.
                await foreach(var result in asyncFunc(new DbCommandActivator(connection, configureCommand: m_ConfigureCommand).CreateCommand).ConfigureAwait(false))
                {
                    yield return result;
                }
            }
            else
            {
                using var transaction = connection.BeginTransaction();

                await foreach(var result in asyncFunc(new DbCommandActivator(connection, transaction, m_ConfigureCommand).CreateCommand).ConfigureAwait(false))
                {
                    yield return result;
                }

                transaction.Commit();
            }
        }

        /// <summary>
        /// Asynchronously executes the specified action on the new connection.
        /// </summary>
        /// <typeparam name="TState">The type of the state.</typeparam>
        /// <param name="state">The state.</param>
        /// <param name="asyncAction">The action.</param>
        public async Task ExecuteOnNewConnectionAsync<TState>(TState state, Func<TState, Func<IDbCommand>, Task> asyncAction)
        {
            using var connection = m_ConnectionActivator();

            connection.Open();

            if (UseTransactionScope && ExistsActiveTransactionScope())
            {
                // Use ambient transaction.
                await asyncAction(state, new DbCommandActivator(connection, configureCommand: m_ConfigureCommand).CreateCommand).ConfigureAwait(false);
            }
            else
            {
                using var transaction = connection.BeginTransaction();

                await asyncAction(state, new DbCommandActivator(connection, transaction, m_ConfigureCommand).CreateCommand).ConfigureAwait(false);

                transaction.Commit();
            }
        }

        /// <summary>
        /// Asynchronously executes the specified action on the new connection.
        /// </summary>
        /// <typeparam name="TState">The type of the state.</typeparam>
        /// <param name="state">The state.</param>
        /// <param name="asyncAction">The action.</param>
        public async ValueTask ExecuteOnNewConnectionAsync<TState>(TState state, Func<TState, Func<IDbCommand>, ValueTask> asyncAction)
        {
            using var connection = m_ConnectionActivator();

            connection.Open();

            if (UseTransactionScope && ExistsActiveTransactionScope())
            {
                // Use ambient transaction.
                await asyncAction(state, new DbCommandActivator(connection, configureCommand: m_ConfigureCommand).CreateCommand).ConfigureAwait(false);
            }
            else
            {
                using var transaction = connection.BeginTransaction();

                await asyncAction(state, new DbCommandActivator(connection, transaction, m_ConfigureCommand).CreateCommand).ConfigureAwait(false);

                transaction.Commit();
            }
        }

        /// <summary>
        /// Asynchronously executes the specified function on the new connection.
        /// </summary>
        /// <typeparam name="TState">The type of the state.</typeparam>
        /// <typeparam name="TResult">The result type of the function.</typeparam>
        /// <param name="state">The state.</param>
        /// <param name="asyncFunc">The function.</param>
        /// <returns>The result of the function.</returns>
        public async Task<TResult> ExecuteOnNewConnectionAsync<TState, TResult>(TState state, Func<TState, Func<IDbCommand>, Task<TResult>> asyncFunc)
        {
            using var connection = m_ConnectionActivator();

            connection.Open();

            if (UseTransactionScope && ExistsActiveTransactionScope())
            {
                // Use ambient transaction.
                return await asyncFunc(state, new DbCommandActivator(connection, configureCommand: m_ConfigureCommand).CreateCommand).ConfigureAwait(false);
            }
            else
            {
                using var transaction = connection.BeginTransaction();

                var result = await asyncFunc(state, new DbCommandActivator(connection, transaction, m_ConfigureCommand).CreateCommand).ConfigureAwait(false);

                transaction.Commit();

                return result;
            }
        }

        /// <summary>
        /// Asynchronously executes the specified function on the new connection.
        /// </summary>
        /// <typeparam name="TState">The type of the state.</typeparam>
        /// <typeparam name="TResult">The result type of the function.</typeparam>
        /// <param name="state">The state.</param>
        /// <param name="asyncFunc">The function.</param>
        /// <returns>The result of the function.</returns>
        public async ValueTask<TResult> ExecuteOnNewConnectionAsync<TState, TResult>(TState state, Func<TState, Func<IDbCommand>, ValueTask<TResult>> asyncFunc)
        {
            using var connection = m_ConnectionActivator();

            connection.Open();

            if (UseTransactionScope && ExistsActiveTransactionScope())
            {
                // Use ambient transaction.
                return await asyncFunc(state, new DbCommandActivator(connection, configureCommand: m_ConfigureCommand).CreateCommand).ConfigureAwait(false);
            }
            else
            {
                using var transaction = connection.BeginTransaction();

                var result = await asyncFunc(state, new DbCommandActivator(connection, transaction, m_ConfigureCommand).CreateCommand).ConfigureAwait(false);

                transaction.Commit();

                return result;
            }
        }

        /// <summary>
        /// Asynchronously executes the specified function on the new connection.
        /// </summary>
        /// <typeparam name="TState">The type of the state.</typeparam>
        /// <typeparam name="TResult">The result type of the function.</typeparam>
        /// <param name="state">The state.</param>
        /// <param name="asyncFunc">The function.</param>
        /// <returns>The result of the function.</returns>
        public async IAsyncEnumerable<TResult> ExecuteIteratorOnNewConnectionAsync<TState, TResult>(TState state, Func<TState, Func<IDbCommand>, IAsyncEnumerable<TResult>> asyncFunc)
        {
            using var connection = m_ConnectionActivator();

            connection.Open();

            if (UseTransactionScope && ExistsActiveTransactionScope())
            {
                // Use ambient transaction.
                await foreach (var result in asyncFunc(state, new DbCommandActivator(connection, configureCommand: m_ConfigureCommand).CreateCommand).ConfigureAwait(false))
                {
                    yield return result;
                }
            }
            else
            {
                using var transaction = connection.BeginTransaction();

                await foreach (var result in asyncFunc(state, new DbCommandActivator(connection, transaction, m_ConfigureCommand).CreateCommand).ConfigureAwait(false))
                {
                    yield return result;
                }

                transaction.Commit();
            }
        }

        #endregion

        #region ExecuteOnConnectionAsync

        /// <summary>
        /// Asynchronously executes the specified action on the specified connection.
        /// </summary>
        /// <param name="connection">The currernt connection.</param>
        /// <param name="asyncAction">The action.</param>
        public Task ExecuteOnConnectionAsync(IDbConnection connection, Func<Func<IDbCommand>, Task> asyncAction)
        {
            return asyncAction(new DbCommandActivator(connection, configureCommand: m_ConfigureCommand).CreateCommand);
        }

        /// <summary>
        /// Asynchronously executes the specified action on the specified connection.
        /// </summary>
        /// <param name="connection">The currernt connection.</param>
        /// <param name="asyncAction">The action.</param>
        public ValueTask ExecuteOnConnectionAsync(IDbConnection connection, Func<Func<IDbCommand>, ValueTask> asyncAction)
        {
            return asyncAction(new DbCommandActivator(connection, configureCommand: m_ConfigureCommand).CreateCommand);
        }

        /// <summary>
        /// Asynchronously executes the specified function on the specified connection.
        /// </summary>
        /// <typeparam name="TResult">The result type of the function.</typeparam>
        /// <param name="connection">The currernt connection.</param>
        /// <param name="asyncFunc">The function.</param>
        /// <returns>The result of the function.</returns>
        public Task<TResult> ExecuteOnConnectionAsync<TResult>(IDbConnection connection, Func<Func<IDbCommand>, Task<TResult>> asyncFunc)
        {
            return asyncFunc(new DbCommandActivator(connection, configureCommand: m_ConfigureCommand).CreateCommand);
        }

        /// <summary>
        /// Asynchronously executes the specified function on the specified connection.
        /// </summary>
        /// <typeparam name="TResult">The result type of the function.</typeparam>
        /// <param name="connection">The currernt connection.</param>
        /// <param name="asyncFunc">The function.</param>
        /// <returns>The result of the function.</returns>
        public ValueTask<TResult> ExecuteOnConnectionAsync<TResult>(IDbConnection connection, Func<Func<IDbCommand>, ValueTask<TResult>> asyncFunc)
        {
            return asyncFunc(new DbCommandActivator(connection, configureCommand: m_ConfigureCommand).CreateCommand);
        }

        /// <summary>
        /// Asynchronously executes the specified function on the specified connection.
        /// </summary>
        /// <typeparam name="TResult">The result type of the function.</typeparam>
        /// <param name="connection">The currernt connection.</param>
        /// <param name="asyncFunc">The function.</param>
        /// <returns>The result of the function.</returns>
        public IAsyncEnumerable<TResult> ExecuteIteratorOnConnectionAsync<TResult>(IDbConnection connection, Func<Func<IDbCommand>, IAsyncEnumerable<TResult>> asyncFunc)
        {
            return asyncFunc(new DbCommandActivator(connection, configureCommand: m_ConfigureCommand).CreateCommand);
        }

        /// <summary>
        /// Asynchronously executes the specified action on the specified connection.
        /// </summary>
        /// <typeparam name="TState">The type of the state.</typeparam>
        /// <param name="state">The state.</param>
        /// <param name="connection">The currernt connection.</param>
        /// <param name="asyncAction">The action.</param>
        public Task ExecuteOnConnectionAsync<TState>(TState state, IDbConnection connection, Func<TState, Func<IDbCommand>, Task> asyncAction)
        {
            return asyncAction(state, new DbCommandActivator(connection, configureCommand: m_ConfigureCommand).CreateCommand);
        }

        /// <summary>
        /// Asynchronously executes the specified action on the specified connection.
        /// </summary>
        /// <typeparam name="TState">The type of the state.</typeparam>
        /// <param name="state">The state.</param>
        /// <param name="connection">The currernt connection.</param>
        /// <param name="asyncAction">The action.</param>
        public ValueTask ExecuteOnConnectionAsync<TState>(TState state, IDbConnection connection, Func<TState, Func<IDbCommand>, ValueTask> asyncAction)
        {
            return asyncAction(state, new DbCommandActivator(connection, configureCommand: m_ConfigureCommand).CreateCommand);
        }

        /// <summary>
        /// Asynchronously executes the specified function on the specified connection.
        /// </summary>
        /// <typeparam name="TState">The type of the state.</typeparam>
        /// <typeparam name="TResult">The result type of the function.</typeparam>
        /// <param name="state">The state.</param>
        /// <param name="connection">The currernt connection.</param>
        /// <param name="asyncFunc">The function.</param>
        /// <returns>The result of the function.</returns>
        public Task<TResult> ExecuteOnConnectionAsync<TState, TResult>(TState state, IDbConnection connection, Func<TState, Func<IDbCommand>, Task<TResult>> asyncFunc)
        {
            return asyncFunc(state, new DbCommandActivator(connection, configureCommand: m_ConfigureCommand).CreateCommand);
        }

        /// <summary>
        /// Asynchronously executes the specified function on the specified connection.
        /// </summary>
        /// <typeparam name="TState">The type of the state.</typeparam>
        /// <typeparam name="TResult">The result type of the function.</typeparam>
        /// <param name="state">The state.</param>
        /// <param name="connection">The currernt connection.</param>
        /// <param name="asyncFunc">The function.</param>
        /// <returns>The result of the function.</returns>
        public ValueTask<TResult> ExecuteOnConnectionAsync<TState, TResult>(TState state, IDbConnection connection, Func<TState, Func<IDbCommand>,  ValueTask<TResult>> asyncFunc)
        {
            return asyncFunc(state, new DbCommandActivator(connection, configureCommand: m_ConfigureCommand).CreateCommand);
        }

        /// <summary>
        /// Asynchronously executes the specified function on the specified connection.
        /// </summary>
        /// <typeparam name="TState">The type of the state.</typeparam>
        /// <typeparam name="TResult">The result type of the function.</typeparam>
        /// <param name="state">The state.</param>
        /// <param name="connection">The currernt connection.</param>
        /// <param name="asyncFunc">The function.</param>
        /// <returns>The result of the function.</returns>
        public IAsyncEnumerable<TResult> ExecuteIteratorOnConnectionAsync<TState, TResult>(TState state, IDbConnection connection, Func<TState, Func<IDbCommand>, IAsyncEnumerable<TResult>> asyncFunc)
        {
            return asyncFunc(state, new DbCommandActivator(connection, configureCommand: m_ConfigureCommand).CreateCommand);
        }

        #endregion

        #region ExecuteOnTransactionAsync

        /// <summary>
        /// Asynchronously executes the specified action on the specified transaction.
        /// </summary>
        /// <param name="transaction">The currernt transaction.</param>
        /// <param name="asyncAction">The action.</param>
        public Task ExecuteOnTransactionAsync(IDbTransaction transaction, Func<Func<IDbCommand>, Task> asyncAction)
        {
            return asyncAction(new DbCommandActivator(transaction, m_ConfigureCommand).CreateCommand);
        }

        /// <summary>
        /// Asynchronously executes the specified action on the specified transaction.
        /// </summary>
        /// <param name="transaction">The currernt transaction.</param>
        /// <param name="asyncAction">The action.</param>
        public ValueTask ExecuteOnTransactionAsync(IDbTransaction transaction, Func<Func<IDbCommand>, ValueTask> asyncAction)
        {
            return asyncAction(new DbCommandActivator(transaction, m_ConfigureCommand).CreateCommand);
        }

        /// <summary>
        /// Asynchronously executes the specified function on the specified transaction.
        /// </summary>
        /// <typeparam name="TResult">The result type of the function.</typeparam>
        /// <param name="transaction">The currernt transaction.</param>
        /// <param name="asyncFunc">The function.</param>
        /// <returns>The result of the function.</returns>
        public Task<TResult> ExecuteOnTransactionAsync<TResult>(IDbTransaction transaction, Func<Func<IDbCommand>, Task<TResult>> asyncFunc)
        {
            return asyncFunc(new DbCommandActivator(transaction, m_ConfigureCommand).CreateCommand);
        }

        /// <summary>
        /// Asynchronously executes the specified function on the specified transaction.
        /// </summary>
        /// <typeparam name="TResult">The result type of the function.</typeparam>
        /// <param name="transaction">The currernt transaction.</param>
        /// <param name="asyncFunc">The function.</param>
        /// <returns>The result of the function.</returns>
        public ValueTask<TResult> ExecuteOnTransactionAsync<TResult>(IDbTransaction transaction, Func<Func<IDbCommand>, ValueTask<TResult>> asyncFunc)
        {
            return asyncFunc(new DbCommandActivator(transaction, m_ConfigureCommand).CreateCommand);
        }

        /// <summary>
        /// Asynchronously executes the specified function on the specified transaction.
        /// </summary>
        /// <typeparam name="TResult">The result type of the function.</typeparam>
        /// <param name="transaction">The currernt transaction.</param>
        /// <param name="asyncFunc">The function.</param>
        /// <returns>The result of the function.</returns>
        public IAsyncEnumerable<TResult> ExecuteIteratorOnTransactionAsync<TResult>(IDbTransaction transaction, Func<Func<IDbCommand>, IAsyncEnumerable<TResult>> asyncFunc)
        {
            return asyncFunc(new DbCommandActivator(transaction, m_ConfigureCommand).CreateCommand);
        }

        /// <summary>
        /// Asynchronously executes the specified action on the specified transaction.
        /// </summary>
        /// <typeparam name="TState">The type of the state.</typeparam>
        /// <param name="state">The state.</param>
        /// <param name="transaction">The currernt transaction.</param>
        /// <param name="asyncAction">The action.</param>
        public Task ExecuteOnTransactionAsync<TState>(TState state, IDbTransaction transaction, Func<TState, Func<IDbCommand>, Task> asyncAction)
        {
            return asyncAction(state, new DbCommandActivator(transaction, m_ConfigureCommand).CreateCommand);
        }

        /// <summary>
        /// Asynchronously executes the specified action on the specified transaction.
        /// </summary>
        /// <typeparam name="TState">The type of the state.</typeparam>
        /// <param name="state">The state.</param>
        /// <param name="transaction">The currernt transaction.</param>
        /// <param name="asyncAction">The action.</param>
        public ValueTask ExecuteOnTransactionAsync<TState>(TState state, IDbTransaction transaction, Func<TState, Func<IDbCommand>, ValueTask> asyncAction)
        {
            return asyncAction(state, new DbCommandActivator(transaction, m_ConfigureCommand).CreateCommand);
        }

        /// <summary>
        /// Asynchronously executes the specified function on the specified transaction.
        /// </summary>
        /// <typeparam name="TState">The type of the state.</typeparam>
        /// <typeparam name="TResult">The result type of the function.</typeparam>
        /// <param name="state">The state.</param>
        /// <param name="transaction">The currernt transaction.</param>
        /// <param name="asyncFunc">The function.</param>
        /// <returns>The result of the function.</returns>
        public Task<TResult> ExecuteOnTransactionAsync<TState, TResult>(TState state, IDbTransaction transaction, Func<TState, Func<IDbCommand>, Task<TResult>> asyncFunc)
        {
            return asyncFunc(state, new DbCommandActivator(transaction, m_ConfigureCommand).CreateCommand);
        }

        /// <summary>
        /// Asynchronously executes the specified function on the specified transaction.
        /// </summary>
        /// <typeparam name="TState">The type of the state.</typeparam>
        /// <typeparam name="TResult">The result type of the function.</typeparam>
        /// <param name="state">The state.</param>
        /// <param name="transaction">The currernt transaction.</param>
        /// <param name="asyncFunc">The function.</param>
        /// <returns>The result of the function.</returns>
        public ValueTask<TResult> ExecuteOnTransactionAsync<TState, TResult>(TState state, IDbTransaction transaction, Func<TState, Func<IDbCommand>, ValueTask<TResult>> asyncFunc)
        {
            return asyncFunc(state, new DbCommandActivator(transaction, m_ConfigureCommand).CreateCommand);
        }

        /// <summary>
        /// Asynchronously executes the specified function on the specified transaction.
        /// </summary>
        /// <typeparam name="TState">The type of the state.</typeparam>
        /// <typeparam name="TResult">The result type of the function.</typeparam>
        /// <param name="state">The state.</param>
        /// <param name="transaction">The currernt transaction.</param>
        /// <param name="asyncFunc">The function.</param>
        /// <returns>The result of the function.</returns>
        public IAsyncEnumerable<TResult> ExecuteIteratorOnTransactionAsync<TState, TResult>(TState state, IDbTransaction transaction, Func<TState, Func<IDbCommand>, IAsyncEnumerable<TResult>> asyncFunc)
        {
            return asyncFunc(state, new DbCommandActivator(transaction, m_ConfigureCommand).CreateCommand);
        }

        #endregion
    }
}
