using System;
using System.Collections.Generic;
using System.Data;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;

namespace mxProject.Data
{
    /// <summary>  
    /// Provides utility methods for invoking actions with filters.  
    /// </summary>  
    internal class FilterUtility
    {
        #region InvokeAction

        /// <summary>  
        /// Invokes an action with the specified filters and object.  
        /// </summary>  
        /// <typeparam name="TFilter">The type of the filter.</typeparam>  
        /// <typeparam name="TObj">The type of the object.</typeparam>  
        /// <param name="filters">The array of filters.</param>  
        /// <param name="obj">The object to process.</param>  
        /// <param name="action">The action to invoke.</param>  
        /// <param name="filterAction">The action to apply for each filter.</param>  
        internal static void InvokeAction<TFilter, TObj>(TFilter[] filters, TObj obj, Action<TObj> action, Action<TFilter, TObj, Action<TObj>> filterAction)
        {
            if (filters == null || filters.Length == 0)
            {
                action(obj);
                return;
            }

            static void main(TFilter[] filters, int index, TObj obj, Action<TObj> action, Action<TFilter, TObj, Action<TObj>> filterAction)
            {
                index += 1;

                if (index == filters.Length - 1)
                {
                    filterAction(filters[index], obj, action);
                }
                else
                {
                    filterAction(filters[index], obj, _obj => main(filters, index, _obj, action, filterAction));
                }
            }

            main(
                filters,
                -1,
                obj,
                action,
                filterAction
                );
        }

        /// <summary>  
        /// Invokes an action with the specified filters, object, and additional arguments.  
        /// </summary>  
        /// <typeparam name="TFilter">The type of the filter.</typeparam>  
        /// <typeparam name="TObj">The type of the object.</typeparam>  
        /// <typeparam name="TArgs">The type of the additional arguments.</typeparam>  
        /// <param name="filters">The array of filters.</param>  
        /// <param name="obj">The object to process.</param>  
        /// <param name="args">The additional arguments to pass.</param>  
        /// <param name="action">The action to invoke.</param>  
        /// <param name="filterAction">The action to apply for each filter.</param>  
        internal static void InvokeAction<TFilter, TObj, TArgs>(TFilter[] filters, TObj obj, TArgs args, Action<TObj, TArgs> action, Action<TFilter, TObj, TArgs, Action<TObj, TArgs>> filterAction)
        {
            if (filters == null || filters.Length == 0)
            {
                action(obj, args);
                return;
            }
            
            static void main(TFilter[] filters, int index, TObj obj, TArgs args, Action<TObj, TArgs> action, Action<TFilter, TObj, TArgs, Action<TObj, TArgs>> filterAction)
            {
                index += 1;

                if (index == filters.Length - 1)
                {
                    filterAction(filters[index], obj, args, action);
                }
                else
                {
                    filterAction(filters[index], obj, args, (_obj, _args) => main(filters, index, _obj, _args, action, filterAction));
                }
            }

            main(
                filters,
                -1,
                obj,
                args,
                action,
                filterAction
                );
        }

        #endregion

        #region InvokeActionAsync

        /// <summary>  
        /// Asynchronously invokes an action with the specified filters and object.  
        /// </summary>  
        /// <typeparam name="TFilter">The type of the filter.</typeparam>  
        /// <typeparam name="TObj">The type of the object.</typeparam>  
        /// <param name="filters">The array of filters.</param>  
        /// <param name="obj">The object to process.</param>  
        /// <param name="action">The asynchronous action to invoke.</param>  
        /// <param name="filterAction">The asynchronous action to apply for each filter.</param>  
        /// <returns>A task that represents the asynchronous operation.</returns>  
        internal static Task InvokeActionAsync<TFilter, TObj>(TFilter[] filters, TObj obj, Func<TObj, Task> action, Func<TFilter, TObj, Func<TObj, Task>, Task> filterAction)
        {
            if (filters == null || filters.Length == 0)
            {
                return action(obj);
            }

            static Task main(TFilter[] filters, int index, TObj obj, Func<TObj, Task> action, Func<TFilter, TObj, Func<TObj, Task>, Task> filterAction)
            {
                index += 1;

                if (index == filters.Length - 1)
                {
                    return filterAction(filters[index], obj, action);
                }
                else
                {
                    return filterAction(filters[index], obj, _obj => main(filters, index, _obj, action, filterAction));
                }
            }

            return main(
                filters,
                -1,
                obj,
                action,
                filterAction
                );
        }

        /// <summary>  
        /// Asynchronously invokes an action with the specified filters, object, and additional arguments.  
        /// </summary>  
        /// <typeparam name="TFilter">The type of the filter.</typeparam>  
        /// <typeparam name="TObj">The type of the object.</typeparam>  
        /// <typeparam name="TArgs">The type of the additional arguments.</typeparam>  
        /// <param name="filters">The array of filters.</param>  
        /// <param name="obj">The object to process.</param>  
        /// <param name="args">The additional arguments to pass.</param>  
        /// <param name="action">The asynchronous action to invoke.</param>  
        /// <param name="filterAction">The asynchronous action to apply for each filter.</param>  
        /// <returns>A task that represents the asynchronous operation.</returns>  
        internal static Task InvokeActionAsync<TFilter, TObj, TArgs>(TFilter[] filters, TObj obj, TArgs args, Func<TObj, TArgs, Task> action, Func<TFilter, TObj, TArgs, Func<TObj, TArgs, Task>, Task> filterAction)
        {
            if (filters == null || filters.Length == 0)
            {
                return action(obj, args);
            }

            static Task main(TFilter[] filters, int index, TObj obj, TArgs args, Func<TObj, TArgs, Task> action, Func<TFilter, TObj, TArgs, Func<TObj, TArgs, Task>, Task> filterAction)
            {
                index += 1;

                if (index == filters.Length - 1)
                {
                    return filterAction(filters[index], obj, args, action);
                }
                else
                {
                    return filterAction(filters[index], obj, args, (_obj, _state) => main(filters, index, _obj, _state, action, filterAction));
                }
            }

            return main(
                filters,
                -1,
                obj,
                args,
                action,
                filterAction
                );
        }

        /// <summary>  
        /// Asynchronously invokes an action with the specified filters and object.  
        /// </summary>  
        /// <typeparam name="TFilter">The type of the filter.</typeparam>  
        /// <typeparam name="TObj">The type of the object.</typeparam>  
        /// <param name="filters">The array of filters.</param>  
        /// <param name="obj">The object to process.</param>  
        /// <param name="action">The asynchronous action to invoke.</param>  
        /// <param name="filterAction">The asynchronous action to apply for each filter.</param>  
        /// <returns>A value task that represents the asynchronous operation.</returns>  
        internal static ValueTask InvokeActionAsync<TFilter, TObj>(TFilter[] filters, TObj obj, Func<TObj, ValueTask> action, Func<TFilter, TObj, Func<TObj, ValueTask>, ValueTask> filterAction)
        {
            if (filters == null || filters.Length == 0)
            {
                return action(obj);
            }

            static ValueTask main(TFilter[] filters, int index, TObj obj, Func<TObj, ValueTask> action, Func<TFilter, TObj, Func<TObj, ValueTask>, ValueTask> filterAction)
            {
                index += 1;

                if (index == filters.Length - 1)
                {
                    return filterAction(filters[index], obj, action);
                }
                else
                {
                    return filterAction(filters[index], obj, _obj => main(filters, index, _obj, action, filterAction));
                }
            }

            return main(
                filters,
                -1,
                obj,
                action,
                filterAction
                );
        }

        /// <summary>  
        /// Asynchronously invokes an action with the specified filters, object, and additional arguments.  
        /// </summary>  
        /// <typeparam name="TFilter">The type of the filter.</typeparam>  
        /// <typeparam name="TObj">The type of the object.</typeparam>  
        /// <typeparam name="TArgs">The type of the additional arguments.</typeparam>  
        /// <param name="filters">The array of filters.</param>  
        /// <param name="obj">The object to process.</param>  
        /// <param name="args">The additional arguments to pass.</param>  
        /// <param name="action">The asynchronous action to invoke.</param>  
        /// <param name="filterAction">The asynchronous action to apply for each filter.</param>  
        /// <returns>A value task that represents the asynchronous operation.</returns>  
        internal static ValueTask InvokeActionAsync<TFilter, TObj, TArgs>(TFilter[] filters, TObj obj, TArgs args, Func<TObj, TArgs, ValueTask> action, Func<TFilter, TObj, TArgs, Func<TObj, TArgs, ValueTask>, ValueTask> filterAction)
        {
            if (filters == null || filters.Length == 0)
            {
                return action(obj, args);
            }

            static ValueTask main(TFilter[] filters, int index, TObj obj, TArgs args, Func<TObj, TArgs, ValueTask> action, Func<TFilter, TObj, TArgs, Func<TObj, TArgs, ValueTask>, ValueTask> filterAction)
            {
                index += 1;

                if (index == filters.Length - 1)
                {
                    return filterAction(filters[index], obj, args, action);
                }
                else
                {
                    return filterAction(filters[index], obj, args, (_obj, _state) => main(filters, index, _obj, _state, action, filterAction));
                }
            }

            return main(
                filters,
                -1,
                obj,
                args,
                action,
                filterAction
                );
        }

        #endregion

        #region InvokeFunc

        /// <summary>  
        /// Invokes a function with the specified filters and object.  
        /// </summary>  
        /// <typeparam name="TFilter">The type of the filter.</typeparam>  
        /// <typeparam name="TObj">The type of the object.</typeparam>  
        /// <typeparam name="TResult">The type of the result.</typeparam>  
        /// <param name="filters">The array of filters.</param>  
        /// <param name="obj">The object to process.</param>  
        /// <param name="func">The function to invoke.</param>  
        /// <param name="filterFunc">The function to apply for each filter.</param>  
        /// <returns>The result of the function invocation.</returns>  
        internal static TResult InvokeFunc<TFilter, TObj, TResult>(TFilter[] filters, TObj obj, Func<TObj, TResult> func, Func<TFilter, TObj, Func<TObj, TResult>, TResult> filterFunc)
        {
            if (filters == null || filters.Length == 0)
            {
                return func(obj);
            }

            static TResult main(TFilter[] filters, int index, TObj obj, Func<TObj, TResult> func, Func<TFilter, TObj, Func<TObj, TResult>, TResult> filterFunc)
            {
                index += 1;

                if (index == filters.Length - 1)
                {
                    return filterFunc(filters[index], obj, func);
                }
                else
                {
                    return filterFunc(filters[index], obj, _obj => main(filters, index, _obj, func, filterFunc));
                }
            }

            return main(
                filters,
                -1,
                obj,
                func,
                filterFunc
                );
        }

        /// <summary>  
        /// Invokes a function with the specified filters, object, and additional arguments.  
        /// </summary>  
        /// <typeparam name="TFilter">The type of the filter.</typeparam>  
        /// <typeparam name="TObj">The type of the object.</typeparam>  
        /// <typeparam name="TArgs">The type of the additional arguments.</typeparam>  
        /// <typeparam name="TResult">The type of the result.</typeparam>  
        /// <param name="filters">The array of filters.</param>  
        /// <param name="obj">The object to process.</param>  
        /// <param name="args">The additional arguments to pass.</param>  
        /// <param name="func">The function to invoke.</param>  
        /// <param name="filterFunc">The function to apply for each filter.</param>  
        /// <returns>The result of the function invocation.</returns>  
        internal static TResult InvokeFunc<TFilter, TObj, TArgs, TResult>(TFilter[] filters, TObj obj, TArgs args, Func<TObj, TArgs, TResult> func, Func<TFilter, TObj, TArgs, Func<TObj, TArgs, TResult>, TResult> filterFunc)
        {
            if (filters == null || filters.Length == 0)
            {
                return func(obj, args);
            }

            static TResult main(TFilter[] filters, int index, TObj obj, TArgs args, Func<TObj, TArgs, TResult> func, Func<TFilter, TObj, TArgs, Func<TObj, TArgs, TResult>, TResult> filterFunc)
            {
                index += 1;

                if (index == filters.Length - 1)
                {
                    return filterFunc(filters[index], obj, args, func);
                }
                else
                {
                    return filterFunc(filters[index], obj, args, (_obj, _args) => main(filters, index, _obj, _args, func, filterFunc));
                }
            }

            return main(
                filters,
                -1,
                obj,
                args,
                func,
                filterFunc
                );
        }

        #endregion

        #region InvokeFuncAsync

        /// <summary>  
        /// Asynchronously invokes a function with the specified filters and object.  
        /// </summary>  
        /// <typeparam name="TFilter">The type of the filter.</typeparam>  
        /// <typeparam name="TObj">The type of the object.</typeparam>  
        /// <typeparam name="TResult">The type of the result.</typeparam>  
        /// <param name="filters">The array of filters.</param>  
        /// <param name="obj">The object to process.</param>  
        /// <param name="func">The asynchronous function to invoke.</param>  
        /// <param name="filterFunc">The asynchronous function to apply for each filter.</param>  
        /// <returns>A task that represents the asynchronous operation and contains the result of the function invocation.</returns>  
        internal static Task<TResult> InvokeFuncAsync<TFilter, TObj, TResult>(TFilter[] filters, TObj obj, Func<TObj, Task<TResult>> func, Func<TFilter, TObj, Func<TObj, Task<TResult>>, Task<TResult>> filterFunc)
        {
            if (filters == null || filters.Length == 0)
            {
                return func(obj);
            }

            static Task<TResult> main(TFilter[] filters, int index, TObj obj, Func<TObj, Task<TResult>> func, Func<TFilter, TObj, Func<TObj, Task<TResult>>, Task<TResult>> filterFunc)
            {
                index += 1;

                if (index == filters.Length - 1)
                {
                    return filterFunc(filters[index], obj, func);
                }
                else
                {
                    return filterFunc(filters[index], obj, _obj => main(filters, index, _obj, func, filterFunc));
                }
            }

            return main(
                filters,
                -1,
                obj,
                func,
                filterFunc
                );
        }

        /// <summary>  
        /// Asynchronously invokes a function with the specified filters, object, and additional arguments.  
        /// </summary>  
        /// <typeparam name="TFilter">The type of the filter.</typeparam>  
        /// <typeparam name="TObj">The type of the object.</typeparam>  
        /// <typeparam name="TArgs">The type of the additional arguments.</typeparam>  
        /// <typeparam name="TResult">The type of the result.</typeparam>  
        /// <param name="filters">The array of filters.</param>  
        /// <param name="obj">The object to process.</param>  
        /// <param name="args">The additional arguments to pass.</param>  
        /// <param name="func">The asynchronous function to invoke.</param>  
        /// <param name="filterFunc">The asynchronous function to apply for each filter.</param>  
        /// <returns>A task that represents the asynchronous operation and contains the result of the function invocation.</returns>  
        internal static Task<TResult> InvokeFuncAsync<TFilter, TObj, TArgs, TResult>(TFilter[] filters, TObj obj, TArgs args, Func<TObj, TArgs, Task<TResult>> func, Func<TFilter, TObj, TArgs, Func<TObj, TArgs, Task<TResult>>, Task<TResult>> filterFunc)
        {
            if (filters == null || filters.Length == 0)
            {
                return func(obj, args);
            }

            static Task<TResult> main(TFilter[] filters, int index, TObj obj, TArgs args, Func<TObj, TArgs, Task<TResult>> func, Func<TFilter, TObj, TArgs, Func<TObj, TArgs, Task<TResult>>, Task<TResult>> filterFunc)
            {
                index += 1;

                if (index == filters.Length - 1)
                {
                    return filterFunc(filters[index], obj, args, func);
                }
                else
                {
                    return filterFunc(filters[index], obj, args, (_obj, _args) => main(filters, index, _obj, _args, func, filterFunc));
                }
            }

            return main(
                filters,
                -1,
                obj,
                args,
                func,
                filterFunc
                );
        }

        /// <summary>  
        /// Asynchronously invokes a function with the specified filters and object.  
        /// </summary>  
        /// <typeparam name="TFilter">The type of the filter.</typeparam>  
        /// <typeparam name="TObj">The type of the object.</typeparam>  
        /// <typeparam name="TResult">The type of the result.</typeparam>  
        /// <param name="filters">The array of filters.</param>  
        /// <param name="obj">The object to process.</param>  
        /// <param name="func">The asynchronous function to invoke.</param>  
        /// <param name="filterFunc">The asynchronous function to apply for each filter.</param>  
        /// <returns>A value task that represents the asynchronous operation and contains the result of the function invocation.</returns>  
        internal static ValueTask<TResult> InvokeFuncAsync<TFilter, TObj, TResult>(TFilter[] filters, TObj obj, Func<TObj, ValueTask<TResult>> func, Func<TFilter, TObj, Func<TObj, ValueTask<TResult>>, ValueTask<TResult>> filterFunc)
        {
            if (filters == null || filters.Length == 0)
            {
                return func(obj);
            }

            static ValueTask<TResult> main(TFilter[] filters, int index, TObj obj, Func<TObj, ValueTask<TResult>> func, Func<TFilter, TObj, Func<TObj, ValueTask<TResult>>, ValueTask<TResult>> filterFunc)
            {
                index += 1;

                if (index == filters.Length - 1)
                {
                    return filterFunc(filters[index], obj, func);
                }
                else
                {
                    return filterFunc(filters[index], obj, _obj => main(filters, index, _obj, func, filterFunc));
                }
            }

            return main(
                filters,
                -1,
                obj,
                func,
                filterFunc
                );
        }

        /// <summary>  
        /// Asynchronously invokes a function with the specified filters, object, and additional arguments.  
        /// </summary>  
        /// <typeparam name="TFilter">The type of the filter.</typeparam>  
        /// <typeparam name="TObj">The type of the object.</typeparam>  
        /// <typeparam name="TArgs">The type of the additional arguments.</typeparam>  
        /// <typeparam name="TResult">The type of the result.</typeparam>  
        /// <param name="filters">The array of filters.</param>  
        /// <param name="obj">The object to process.</param>  
        /// <param name="args">The additional arguments to pass.</param>  
        /// <param name="func">The asynchronous function to invoke.</param>  
        /// <param name="filterFunc">The asynchronous function to apply for each filter.</param>  
        /// <returns>A value task that represents the asynchronous operation and contains the result of the function invocation.</returns>  
        internal static ValueTask<TResult> InvokeFuncAsync<TFilter, TObj, TArgs, TResult>(TFilter[] filters, TObj obj, TArgs args, Func<TObj, TArgs, ValueTask<TResult>> func, Func<TFilter, TObj, TArgs, Func<TObj, TArgs, ValueTask<TResult>>, ValueTask<TResult>> filterFunc)
        {
            if (filters == null || filters.Length == 0)
            {
                return func(obj, args);
            }

            static ValueTask<TResult> main(TFilter[] filters, int index, TObj obj, TArgs args, Func<TObj, TArgs, ValueTask<TResult>> func, Func<TFilter, TObj, TArgs, Func<TObj, TArgs, ValueTask<TResult>>, ValueTask<TResult>> filterFunc)
            {
                index += 1;

                if (index == filters.Length - 1)
                {
                    return filterFunc(filters[index], obj, args, func);
                }
                else
                {
                    return filterFunc(filters[index], obj, args, (_obj, _args) => main(filters, index, _obj, _args, func, filterFunc));
                }
            }

            return main(
                filters,
                -1,
                obj,
                args,
                func,
                filterFunc
                );
        }

        #endregion
    }
}
