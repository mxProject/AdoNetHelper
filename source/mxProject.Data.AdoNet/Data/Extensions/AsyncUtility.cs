using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using System.Threading;
using System.Data;

namespace mxProject.Data.Extensions
{
    /// <summary>  
    /// Provides utility methods for asynchronous operations.  
    /// </summary>  
    internal static class AsyncUtility
    {
        /// <summary>  
        /// Executes an action asynchronously.  
        /// </summary>  
        /// <typeparam name="TState">The type of the state object.</typeparam>  
        /// <param name="state">The state object to pass to the action.</param>  
        /// <param name="action">The action to execute.</param>  
        /// <param name="cancellationToken">The cancellation token to observe.</param>  
        /// <param name="onCancel">The action to execute if the operation is canceled.</param>  
        /// <returns>A task that represents the asynchronous operation.</returns>  
        internal static Task ActionAsync<TState>(TState state, Action<TState> action, CancellationToken cancellationToken = default, Action<TState>? onCancel = null)
        {
            if (cancellationToken.IsCancellationRequested)
            {
                return Task.FromCanceled(cancellationToken);
            }

            CancellationTokenRegistration registration = default;

            if (cancellationToken.CanBeCanceled && onCancel != null)
            {
                registration = cancellationToken.Register(s => onCancel((TState)s), state);
            }

            try
            {
                action(state);
                return Task.CompletedTask;
            }
            catch (Exception ex)
            {
                return Task.FromException(ex);
            }
            finally
            {
                registration.Dispose();
            }
        }

        /// <summary>  
        /// Executes a function asynchronously and returns a result.  
        /// </summary>  
        /// <typeparam name="TState">The type of the state object.</typeparam>  
        /// <typeparam name="TResult">The type of the result.</typeparam>  
        /// <param name="state">The state object to pass to the function.</param>  
        /// <param name="func">The function to execute.</param>  
        /// <param name="cancellationToken">The cancellation token to observe.</param>  
        /// <param name="onCancel">The action to execute if the operation is canceled.</param>  
        /// <returns>A task that represents the asynchronous operation and contains the result.</returns>  
        internal static Task<TResult> FuncAsync<TState, TResult>(TState state, Func<TState, TResult> func, CancellationToken cancellationToken = default, Action<TState>? onCancel = null)
        {
            if (cancellationToken.IsCancellationRequested)
            {
                return Task.FromCanceled<TResult>(cancellationToken);
            }

            CancellationTokenRegistration registration = default;

            if (cancellationToken.CanBeCanceled && onCancel != null)
            {
                registration = cancellationToken.Register(s => onCancel((TState)s), state);
            }

            try
            {
                return Task.FromResult(func(state));
            }
            catch (Exception ex)
            {
                return Task.FromException<TResult>(ex);
            }
            finally
            {
                registration.Dispose();
            }
        }

        /// <summary>  
        /// Executes an action asynchronously and returns a ValueTask.  
        /// </summary>  
        /// <typeparam name="TState">The type of the state object.</typeparam>  
        /// <param name="state">The state object to pass to the action.</param>  
        /// <param name="action">The action to execute.</param>  
        /// <param name="cancellationToken">The cancellation token to observe.</param>  
        /// <param name="onCancel">The action to execute if the operation is canceled.</param>  
        /// <returns>A ValueTask that represents the asynchronous operation.</returns>  
        internal static ValueTask ActionAsyncAsValueTask<TState>(TState state, Action<TState> action, CancellationToken cancellationToken = default, Action<TState>? onCancel = null)
        {
            if (cancellationToken.IsCancellationRequested)
            {
                return new ValueTask(Task.FromCanceled(cancellationToken));
            }

            CancellationTokenRegistration registration = default;

            if (cancellationToken.CanBeCanceled && onCancel != null)
            {
                registration = cancellationToken.Register(s => onCancel((TState)s), state);
            }

            try
            {
                action(state);
                return default;
            }
            catch (Exception ex)
            {
                return new ValueTask(Task.FromException(ex));
            }
            finally
            {
                registration.Dispose();
            }
        }

        /// <summary>  
        /// Executes a function asynchronously and returns a result as a ValueTask.  
        /// </summary>  
        /// <typeparam name="TState">The type of the state object.</typeparam>  
        /// <typeparam name="TResult">The type of the result.</typeparam>  
        /// <param name="state">The state object to pass to the function.</param>  
        /// <param name="func">The function to execute.</param>  
        /// <param name="cancellationToken">The cancellation token to observe.</param>  
        /// <param name="onCancel">The action to execute if the operation is canceled.</param>  
        /// <returns>A ValueTask that represents the asynchronous operation and contains the result.</returns>  
        internal static ValueTask<TResult> FuncAsyncAsValueTask<TState, TResult>(TState state, Func<TState, TResult> func, CancellationToken cancellationToken = default, Action<TState>? onCancel = null)
        {
            if (cancellationToken.IsCancellationRequested)
            {
                return new ValueTask<TResult>(Task.FromCanceled<TResult>(cancellationToken));
            }

            CancellationTokenRegistration registration = default;

            if (cancellationToken.CanBeCanceled && onCancel != null)
            {
                registration = cancellationToken.Register(s => onCancel((TState)s), state);
            }

            try
            {
                return new ValueTask<TResult>(func(state));
            }
            catch (Exception ex)
            {
                return new ValueTask<TResult>(Task.FromException<TResult>(ex));
            }
            finally
            {
                registration.Dispose();
            }
        }
    }
}
