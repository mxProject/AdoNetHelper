using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using System.Data;

namespace mxProject.Data.Extensions
{
    /// <summary>
    /// Provides extension methods for <see cref="IDataReader"/>.
    /// </summary>
    public static class DataReaderExtensions
    {
        #region async support

        // https://github.com/dotnet/runtime/blob/main/src/libraries/System.Data.Common/src/System/Data/Common/DbDataReader.cs

        /// <summary>
        /// Asynchronously disposes the <see cref="IDataReader"/>.
        /// </summary>
        /// <param name="this">The <see cref="IDataReader"/> instance.</param>
        /// <returns>A <see cref="ValueTask"/> representing the asynchronous operation.</returns>
        public static ValueTask DisposeAsync(this IDataReader @this)
        {
#if NETSTANDARD2_1_OR_GREATER
                if (@this is DbDataReader reader)
                {
                    return reader.DisposeAsync();
                }
#endif

            return AsyncUtility.ActionAsyncAsValueTask(@this, x => x.Dispose());
        }

        /// <summary>
        /// Asynchronously closes the <see cref="IDataReader"/>.
        /// </summary>
        /// <param name="this">The <see cref="IDataReader"/> instance.</param>
        /// <returns>A <see cref="Task"/> representing the asynchronous operation.</returns>
        public static Task CloseAsync(this IDataReader @this)
        {
#if NETSTANDARD2_1_OR_GREATER
                if (@this is DbDataReader reader)
                {
                    return reader.CloseAsync();
                }
#endif

            return AsyncUtility.ActionAsync(@this, x => x.Close());
        }

        /// <summary>
        /// Asynchronously retrieves the schema table for the <see cref="IDataReader"/>.
        /// </summary>
        /// <param name="this">The <see cref="IDataReader"/> instance.</param>
        /// <param name="cancellationToken">A token to monitor for cancellation requests.</param>
        /// <returns>A <see cref="Task{TResult}"/> representing the asynchronous operation, with a <see cref="DataTable"/> result.</returns>
        public static Task<DataTable> GetSchemaTableAsync(this IDataReader @this, CancellationToken cancellationToken = default)
        {
#if NETSTANDARD2_1_OR_GREATER
                if (@this is DbDataReader reader)
                {
                    return reader.GetSchemaTableAsync(cancellationToken);
                }
#endif

            return AsyncUtility.FuncAsync(@this, x => x.GetSchemaTable(), cancellationToken);
        }

        /// <summary>
        /// Asynchronously reads the next record from the <see cref="IDataReader"/>.
        /// </summary>
        /// <param name="this">The <see cref="IDataReader"/> instance.</param>
        /// <param name="cancellationToken">A token to monitor for cancellation requests.</param>
        /// <returns>A <see cref="Task{TResult}"/> representing the asynchronous operation, with a <see cref="bool"/> result indicating whether there are more rows.</returns>
        public static Task<bool> ReadAsync(this IDataReader @this, CancellationToken cancellationToken = default)
        {
#if NETSTANDARD2_1_OR_GREATER
                if (@this is DbDataReader reader)
                {
                    return reader.ReadAsync(cancellationToken);
                }
#endif

            return AsyncUtility.FuncAsync(@this, x => x.Read(), cancellationToken);
        }

        /// <summary>
        /// Asynchronously advances the <see cref="IDataReader"/> to the next result set.
        /// </summary>
        /// <param name="this">The <see cref="IDataReader"/> instance.</param>
        /// <param name="cancellationToken">A token to monitor for cancellation requests.</param>
        /// <returns>A <see cref="Task{TResult}"/> representing the asynchronous operation, with a <see cref="bool"/> result indicating whether there are more result sets.</returns>
        public static Task<bool> NextResultAsync(this IDataReader @this, CancellationToken cancellationToken = default)
        {
#if NETSTANDARD2_1_OR_GREATER
                if (@this is DbDataReader reader)
                {
                    return reader.NextResultAsync(cancellationToken);
                }
#endif

            return AsyncUtility.FuncAsync(@this, x => x.NextResult(), cancellationToken);
        }

        #endregion

        #region Configure

        ///// <summary>
        ///// Configures the datareader by applying the specified action.
        ///// </summary>
        ///// <typeparam name="TReader">The type of the datareader.</typeparam>
        ///// <param name="this">The database datareader.</param>
        ///// <param name="configure">The action to apply to the datareader.</param>
        ///// <exception cref="InvalidOperationException">Thrown if the datareader is not of the specified type.</exception>
        //public static void Configure<TReader>(this IDataReader @this, Action<TReader> configure) where TReader : IDataReader
        //{
        //    var reader = @this.Find<TReader>();

        //    if (reader == null) { throw new InvalidOperationException($"The datareader is not of type {typeof(TReader).Name}."); }

        //    configure(reader);
        //}

        /// <summary>
        /// Finds the datareader of the specified type.
        /// </summary>
        /// <typeparam name="TReader">The type of the datareader to find.</typeparam>
        /// <param name="this">The database datareader.</param>
        /// <returns>The datareader of the specified type, or <c>null</c> if not found.</returns>
        private static TReader? Find<TReader>(this IDataReader @this) where TReader : IDataReader
        {
            if (@this is TReader reader)
            {
                return reader;
            }

            if (@this is Wrappers.DataReaderWithFilter wrapper)
            {
                return wrapper.WrappedDataReader.Find<TReader>();
            }

            return default;
        }

        #endregion
    }
}