using System;
using System.Collections.Generic;
using System.Data;

namespace mxProject.Data.Extensions
{
    /// <summary>
    /// Provides extension methods for <see cref="IDataParameterCollection"/>.
    /// </summary>
    public static class DataParameterCollectionExtensions
    {
        #region Cast

        /// <summary>
        /// Casts the parameter collection to the specified type and performs the specified action.
        /// </summary>
        /// <typeparam name="TParameters">The type of the parameter collection to cast to.</typeparam>
        /// <param name="this">The parameter collection to cast.</param>
        /// <param name="action">The action to perform on the casted parameter collection.</param>
        /// <exception cref="InvalidCastException">
        /// Thrown if the parameter collection is not of the specified type.
        /// </exception>
        public static void Cast<TParameters>(this IDataParameterCollection @this, Action<TParameters> action) where TParameters : IDataParameterCollection
        {
            var paramters = @this.Find<TParameters>();

            if (paramters == null) { throw new InvalidCastException($"The paramter collection is not of type {typeof(TParameters).Name}."); }

            action(paramters);
        }

        /// <summary>
        /// Casts the parameter collection to the specified type and returns the result of the specified function.
        /// </summary>
        /// <typeparam name="TParameters">The type of the parameter collection to cast to.</typeparam>
        /// <typeparam name="TResult">The type of the result returned by the function.</typeparam>
        /// <param name="this">The parameter collection to cast.</param>
        /// <param name="func">The function to execute on the casted parameter collection.</param>
        /// <returns>The result of the function.</returns>
        /// <exception cref="InvalidCastException">
        /// Thrown if the parameter collection is not of the specified type.
        /// </exception>
        public static TResult Cast<TParameters, TResult>(this IDataParameterCollection @this, Func<TParameters, TResult> func) where TParameters : IDataParameterCollection
        {
            var paramters = @this.Find<TParameters>();

            if (paramters == null) { throw new InvalidCastException($"The paramter collection is not of type {typeof(TParameters).Name}."); }

            return func(paramters);
        }

        /// <summary>
        /// Finds the paramter collection of the specified type.
        /// </summary>
        /// <typeparam name="TParameters">The type of the paramter collection to find.</typeparam>
        /// <param name="this">The database paramter collection.</param>
        /// <returns>The paramter collection of the specified type, or <c>null</c> if not found.</returns>
        private static TParameters? Find<TParameters>(this IDataParameterCollection @this) where TParameters : IDataParameterCollection
        {
            if (@this is TParameters paramters)
            {
                return paramters;
            }

            if (@this is Wrappers.DataParameterCollectionWithFilter wrapper)
            {
                return wrapper.Find<TParameters>();
            }

            return default;
        }

        #endregion
    }
}
