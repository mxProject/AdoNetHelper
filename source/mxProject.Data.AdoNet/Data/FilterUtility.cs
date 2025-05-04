using System;
using System.Collections.Generic;
using System.Data;
using System.Threading.Tasks;

namespace mxProject.Data
{
    [Flags]
    internal enum FilterAlgorithms
    {
        Default = 0,
        Recursive = 1,
    }

    /// <summary>  
    /// Provides utility methods for invoking actions with filters.  
    /// </summary>  
    internal class FilterUtility
    {
        internal static readonly FilterAlgorithms Algorithm = FilterAlgorithms.Default;

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
        internal static void InvokeAction<TFilter, TObj>(
            TFilter[] filters,
            TObj obj,
            Action<TObj> action,
            Action<TFilter, TObj, Action<TObj>> filterAction
            )
        {
            if (filters == null || filters.Length == 0)
            {
                action(obj);
                return;
            }
            else if (filters.Length == 1)
            {
                filterAction(filters[0], obj, action);
                return;
            }

            if ((Algorithm & FilterAlgorithms.Recursive) == 0)
            {
                if (filters.Length == 2)
                {
                    filterAction(filters[0], obj,
                        obj => filterAction(filters[1], obj, action)
                        );
                    return;
                }
                else if (filters.Length == 3)
                {
                    filterAction(filters[0], obj,
                        obj => filterAction(filters[1], obj,
                        obj => filterAction(filters[2], obj, action)
                        ));
                    return;
                }
                else if (filters.Length == 4)
                {
                    filterAction(filters[0], obj,
                        obj => filterAction(filters[1], obj,
                        obj => filterAction(filters[2], obj,
                        obj => filterAction(filters[3], obj, action)
                        )));
                    return;
                }
                else if (filters.Length == 5)
                {
                    filterAction(filters[0], obj,
                        obj => filterAction(filters[1], obj,
                        obj => filterAction(filters[2], obj,
                        obj => filterAction(filters[3], obj,
                        obj => filterAction(filters[4], obj, action)
                        ))));
                    return;
                }
                else if (filters.Length == 6)
                {
                    filterAction(filters[0], obj,
                        obj => filterAction(filters[1], obj,
                        obj => filterAction(filters[2], obj,
                        obj => filterAction(filters[3], obj,
                        obj => filterAction(filters[4], obj,
                        obj => filterAction(filters[5], obj, action)
                        )))));
                    return;
                }
                else if (filters.Length == 7)
                {
                    filterAction(filters[0], obj,
                        obj => filterAction(filters[1], obj,
                        obj => filterAction(filters[2], obj,
                        obj => filterAction(filters[3], obj,
                        obj => filterAction(filters[4], obj,
                        obj => filterAction(filters[5], obj,
                        obj => filterAction(filters[6], obj, action)
                        ))))));
                    return;
                }
                else if (filters.Length == 8)
                {
                    filterAction(filters[0], obj,
                        obj => filterAction(filters[1], obj,
                        obj => filterAction(filters[2], obj,
                        obj => filterAction(filters[3], obj,
                        obj => filterAction(filters[4], obj,
                        obj => filterAction(filters[5], obj,
                        obj => filterAction(filters[6], obj,
                        obj => filterAction(filters[7], obj, action)
                        )))))));
                    return;
                }
            }

            static void recursive(
                int index,
                TFilter[] filters,
                TObj obj,
                Action<TObj> action,
                Action<TFilter, TObj, Action<TObj>> filterAction
                )
            {
                index += 1;

                if (index == filters.Length - 1)
                {
                    filterAction(filters[index], obj, action);
                }
                else
                {
                    filterAction(filters[index], obj, _obj => recursive(index, filters, _obj, action, filterAction));
                }
            }

            recursive(
                -1,
                filters,
                obj,
                action,
                filterAction
                );
        }

        #endregion

        #region InvokeAction(args)

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
        internal static void InvokeAction<TFilter, TObj, TArgs>(
            TFilter[] filters,
            TObj obj,
            TArgs args,
            Action<TObj, TArgs> action,
            Action<TFilter, TObj, TArgs, Action<TObj, TArgs>> filterAction
            )
        {
            if (filters == null || filters.Length == 0)
            {
                action(obj, args);
                return;
            }
            else if (filters.Length == 1)
            {
                filterAction(filters[0], obj, args, action);
                return;
            }

            if ((Algorithm & FilterAlgorithms.Recursive) == 0)
            {
                if (filters.Length == 2)
                {
                    filterAction(filters[0], obj, args,
                        (obj, args) => filterAction(filters[1], obj, args, action)
                        );
                    return;
                }
                else if (filters.Length == 3)
                {
                    filterAction(filters[0], obj, args,
                        (obj, args) => filterAction(filters[1], obj, args,
                        (obj, args) => filterAction(filters[2], obj, args, action)
                        ));
                    return;
                }
                else if (filters.Length == 4)
                {
                    filterAction(filters[0], obj, args,
                        (obj, args) => filterAction(filters[1], obj, args,
                        (obj, args) => filterAction(filters[2], obj, args,
                        (obj, args) => filterAction(filters[3], obj, args, action)
                        )));
                    return;
                }
                else if (filters.Length == 5)
                {
                    filterAction(filters[0], obj, args,
                        (obj, args) => filterAction(filters[1], obj, args,
                        (obj, args) => filterAction(filters[2], obj, args,
                        (obj, args) => filterAction(filters[3], obj, args,
                        (obj, args) => filterAction(filters[4], obj, args, action)
                        ))));
                    return;
                }
                else if (filters.Length == 6)
                {
                    filterAction(filters[0], obj, args,
                        (obj, args) => filterAction(filters[1], obj, args,
                        (obj, args) => filterAction(filters[2], obj, args,
                        (obj, args) => filterAction(filters[3], obj, args,
                        (obj, args) => filterAction(filters[4], obj, args,
                        (obj, args) => filterAction(filters[5], obj, args, action)
                        )))));
                    return;
                }
                else if (filters.Length == 7)
                {
                    filterAction(filters[0], obj, args,
                        (obj, args) => filterAction(filters[1], obj, args,
                        (obj, args) => filterAction(filters[2], obj, args,
                        (obj, args) => filterAction(filters[3], obj, args,
                        (obj, args) => filterAction(filters[4], obj, args,
                        (obj, args) => filterAction(filters[5], obj, args,
                        (obj, args) => filterAction(filters[6], obj, args, action)
                        ))))));
                    return;
                }
                else if (filters.Length == 8)
                {
                    filterAction(filters[0], obj, args,
                        (obj, args) => filterAction(filters[1], obj, args,
                        (obj, args) => filterAction(filters[2], obj, args,
                        (obj, args) => filterAction(filters[3], obj, args,
                        (obj, args) => filterAction(filters[4], obj, args,
                        (obj, args) => filterAction(filters[5], obj, args,
                        (obj, args) => filterAction(filters[6], obj, args,
                        (obj, args) => filterAction(filters[7], obj, args, action)
                        )))))));
                    return;
                }
            }

            static void recursive(
                int index,
                TFilter[] filters,
                TObj obj,
                TArgs args,
                Action<TObj, TArgs> action,
                Action<TFilter, TObj, TArgs, Action<TObj, TArgs>> filterAction
                )
            {
                index += 1;

                if (index == filters.Length - 1)
                {
                    filterAction(filters[index], obj, args, action);
                }
                else
                {
                    filterAction(filters[index], obj, args, (_obj, _args) => recursive(index, filters, _obj, _args, action, filterAction));
                }
            }

            recursive(
                -1,
                filters,
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
        internal static Task InvokeActionAsync<TFilter, TObj>(
            TFilter[] filters,
            TObj obj,
            Func<TObj, Task> action,
            Func<TFilter, TObj, Func<TObj, Task>, Task> filterAction
            )
        {
            if (filters == null || filters.Length == 0)
            {
                return action(obj);
            }
            else if (filters.Length == 1)
            {
                return filterAction(filters[0], obj, action);
            }

            if ((Algorithm & FilterAlgorithms.Recursive) == 0)
            {
                if (filters.Length == 2)
                {
                    return filterAction(filters[0], obj,
                        obj => filterAction(filters[1], obj, action)
                        );
                }
                else if (filters.Length == 3)
                {
                    return filterAction(filters[0], obj,
                        obj => filterAction(filters[1], obj,
                        obj => filterAction(filters[2], obj, action)
                        ));
                }
                else if (filters.Length == 4)
                {
                    return filterAction(filters[0], obj,
                        obj => filterAction(filters[1], obj,
                        obj => filterAction(filters[2], obj,
                        obj => filterAction(filters[3], obj, action)
                        )));
                }
                else if (filters.Length == 5)
                {
                    return filterAction(filters[0], obj,
                        obj => filterAction(filters[1], obj,
                        obj => filterAction(filters[2], obj,
                        obj => filterAction(filters[3], obj,
                        obj => filterAction(filters[4], obj, action)
                        ))));
                }
                else if (filters.Length == 6)
                {
                    return filterAction(filters[0], obj,
                        obj => filterAction(filters[1], obj,
                        obj => filterAction(filters[2], obj,
                        obj => filterAction(filters[3], obj,
                        obj => filterAction(filters[4], obj,
                        obj => filterAction(filters[5], obj, action)
                        )))));
                }
                else if (filters.Length == 7)
                {
                    return filterAction(filters[0], obj,
                        obj => filterAction(filters[1], obj,
                        obj => filterAction(filters[2], obj,
                        obj => filterAction(filters[3], obj,
                        obj => filterAction(filters[4], obj,
                        obj => filterAction(filters[5], obj,
                        obj => filterAction(filters[6], obj, action)
                        ))))));
                }
                else if (filters.Length == 8)
                {
                    return filterAction(filters[0], obj,
                        obj => filterAction(filters[1], obj,
                        obj => filterAction(filters[2], obj,
                        obj => filterAction(filters[3], obj,
                        obj => filterAction(filters[4], obj,
                        obj => filterAction(filters[5], obj,
                        obj => filterAction(filters[6], obj,
                        obj => filterAction(filters[7], obj, action)
                        )))))));
                }
            }

            static Task recursive(
                int index,
                TFilter[] filters,
                TObj obj,
                Func<TObj, Task> action,
                Func<TFilter, TObj, Func<TObj, Task>, Task> filterAction
                )
            {
                index += 1;

                if (index == filters.Length - 1)
                {
                    return filterAction(filters[index], obj, action);
                }
                else
                {
                    return filterAction(filters[index], obj, _obj => recursive(index, filters, _obj, action, filterAction));
                }
            }

            return recursive(
                -1,
                filters,
                obj,
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
        internal static ValueTask InvokeActionAsync<TFilter, TObj>(
            TFilter[] filters,
            TObj obj,
            Func<TObj, ValueTask> action,
            Func<TFilter, TObj, Func<TObj, ValueTask>, ValueTask> filterAction
            )
        {
            if (filters == null || filters.Length == 0)
            {
                return action(obj);
            }
            else if (filters.Length == 1)
            {
                return filterAction(filters[0], obj, action);
            }

            if ((Algorithm & FilterAlgorithms.Recursive) == 0)
            {
                if (filters.Length == 2)
                {
                    return filterAction(filters[0], obj,
                        obj => filterAction(filters[1], obj, action)
                        );
                }
                else if (filters.Length == 3)
                {
                    return filterAction(filters[0], obj,
                        obj => filterAction(filters[1], obj,
                        obj => filterAction(filters[2], obj, action)
                        ));
                }
                else if (filters.Length == 4)
                {
                    return filterAction(filters[0], obj,
                        obj => filterAction(filters[1], obj,
                        obj => filterAction(filters[2], obj,
                        obj => filterAction(filters[3], obj, action)
                        )));
                }
                else if (filters.Length == 5)
                {
                    return filterAction(filters[0], obj,
                        obj => filterAction(filters[1], obj,
                        obj => filterAction(filters[2], obj,
                        obj => filterAction(filters[3], obj,
                        obj => filterAction(filters[4], obj, action)
                        ))));
                }
                else if (filters.Length == 6)
                {
                    return filterAction(filters[0], obj,
                        obj => filterAction(filters[1], obj,
                        obj => filterAction(filters[2], obj,
                        obj => filterAction(filters[3], obj,
                        obj => filterAction(filters[4], obj,
                        obj => filterAction(filters[5], obj, action)
                        )))));
                }
                else if (filters.Length == 7)
                {
                    return filterAction(filters[0], obj,
                        obj => filterAction(filters[1], obj,
                        obj => filterAction(filters[2], obj,
                        obj => filterAction(filters[3], obj,
                        obj => filterAction(filters[4], obj,
                        obj => filterAction(filters[5], obj,
                        obj => filterAction(filters[6], obj, action)
                        ))))));
                }
                else if (filters.Length == 8)
                {
                    return filterAction(filters[0], obj,
                        obj => filterAction(filters[1], obj,
                        obj => filterAction(filters[2], obj,
                        obj => filterAction(filters[3], obj,
                        obj => filterAction(filters[4], obj,
                        obj => filterAction(filters[5], obj,
                        obj => filterAction(filters[6], obj,
                        obj => filterAction(filters[7], obj, action)
                        )))))));
                }
            }

            static ValueTask recursive(
                int index,
                TFilter[] filters,
                TObj obj,
                Func<TObj, ValueTask> action,
                Func<TFilter, TObj, Func<TObj, ValueTask>, ValueTask> filterAction
                )
            {
                index += 1;

                if (index == filters.Length - 1)
                {
                    return filterAction(filters[index], obj, action);
                }
                else
                {
                    return filterAction(filters[index], obj, _obj => recursive(index, filters, _obj, action, filterAction));
                }
            }

            return recursive(
                -1,
                filters,
                obj,
                action,
                filterAction
                );
        }

        #endregion

        #region InvokeActionAsync(args)

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
        internal static Task InvokeActionAsync<TFilter, TObj, TArgs>(
            TFilter[] filters,
            TObj obj,
            TArgs args,
            Func<TObj, TArgs, Task> action,
            Func<TFilter, TObj, TArgs, Func<TObj, TArgs, Task>, Task> filterAction
            )
        {
            if (filters == null || filters.Length == 0)
            {
                return action(obj, args);
            }
            else if (filters.Length == 1)
            {
                return filterAction(filters[0], obj, args, action);
            }

            if ((Algorithm & FilterAlgorithms.Recursive) == 0)
            {
                if (filters.Length == 2)
                {
                    return filterAction(filters[0], obj, args,
                        (obj, args) => filterAction(filters[1], obj, args, action)
                        );
                }
                else if (filters.Length == 3)
                {
                    return filterAction(filters[0], obj, args,
                        (obj, args) => filterAction(filters[1], obj, args,
                        (obj, args) => filterAction(filters[2], obj, args, action)
                        ));
                }
                else if (filters.Length == 4)
                {
                    return filterAction(filters[0], obj, args,
                        (obj, args) => filterAction(filters[1], obj, args,
                        (obj, args) => filterAction(filters[2], obj, args,
                        (obj, args) => filterAction(filters[3], obj, args, action)
                        )));
                }
                else if (filters.Length == 5)
                {
                    return filterAction(filters[0], obj, args,
                        (obj, args) => filterAction(filters[1], obj, args,
                        (obj, args) => filterAction(filters[2], obj, args,
                        (obj, args) => filterAction(filters[3], obj, args,
                        (obj, args) => filterAction(filters[4], obj, args, action)
                        ))));
                }
                else if (filters.Length == 6)
                {
                    return filterAction(filters[0], obj, args,
                        (obj, args) => filterAction(filters[1], obj, args,
                        (obj, args) => filterAction(filters[2], obj, args,
                        (obj, args) => filterAction(filters[3], obj, args,
                        (obj, args) => filterAction(filters[4], obj, args,
                        (obj, args) => filterAction(filters[5], obj, args, action)
                        )))));
                }
                else if (filters.Length == 7)
                {
                    return filterAction(filters[0], obj, args,
                        (obj, args) => filterAction(filters[1], obj, args,
                        (obj, args) => filterAction(filters[2], obj, args,
                        (obj, args) => filterAction(filters[3], obj, args,
                        (obj, args) => filterAction(filters[4], obj, args,
                        (obj, args) => filterAction(filters[5], obj, args,
                        (obj, args) => filterAction(filters[6], obj, args, action)
                        ))))));
                }
                else if (filters.Length == 8)
                {
                    return filterAction(filters[0], obj, args,
                        (obj, args) => filterAction(filters[1], obj, args,
                        (obj, args) => filterAction(filters[2], obj, args,
                        (obj, args) => filterAction(filters[3], obj, args,
                        (obj, args) => filterAction(filters[4], obj, args,
                        (obj, args) => filterAction(filters[5], obj, args,
                        (obj, args) => filterAction(filters[6], obj, args,
                        (obj, args) => filterAction(filters[7], obj, args, action)
                        )))))));
                }
            }

            static Task recursive(
                int index,
                TFilter[] filters,
                TObj obj,
                TArgs args,
                Func<TObj, TArgs, Task> action,
                Func<TFilter, TObj, TArgs, Func<TObj, TArgs, Task>, Task> filterAction
                )
            {
                index += 1;

                if (index == filters.Length - 1)
                {
                    return filterAction(filters[index], obj, args, action);
                }
                else
                {
                    return filterAction(filters[index], obj, args, (_obj, _args) => recursive(index, filters, _obj, _args, action, filterAction));
                }
            }

            return recursive(
                -1,
                filters,
                obj,
                args,
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
        internal static ValueTask InvokeActionAsync<TFilter, TObj, TArgs>(
            TFilter[] filters,
            TObj obj, TArgs args,
            Func<TObj, TArgs, ValueTask> action,
            Func<TFilter, TObj, TArgs, Func<TObj, TArgs, ValueTask>, ValueTask> filterAction
            )
        {
            if (filters == null || filters.Length == 0)
            {
                return action(obj, args);
            }
            else if (filters.Length == 1)
            {
                return filterAction(filters[0], obj, args, action);
            }

            if ((Algorithm & FilterAlgorithms.Recursive) == 0)
            {
                if (filters.Length == 2)
                {
                    return filterAction(filters[0], obj, args,
                        (obj, args) => filterAction(filters[1], obj, args, action)
                        );
                }
                else if (filters.Length == 3)
                {
                    return filterAction(filters[0], obj, args,
                        (obj, args) => filterAction(filters[1], obj, args,
                        (obj, args) => filterAction(filters[2], obj, args, action)
                        ));
                }
                else if (filters.Length == 4)
                {
                    return filterAction(filters[0], obj, args,
                        (obj, args) => filterAction(filters[1], obj, args,
                        (obj, args) => filterAction(filters[2], obj, args,
                        (obj, args) => filterAction(filters[3], obj, args, action)
                        )));
                }
                else if (filters.Length == 5)
                {
                    return filterAction(filters[0], obj, args,
                        (obj, args) => filterAction(filters[1], obj, args,
                        (obj, args) => filterAction(filters[2], obj, args,
                        (obj, args) => filterAction(filters[3], obj, args,
                        (obj, args) => filterAction(filters[4], obj, args, action)
                        ))));
                }
                else if (filters.Length == 6)
                {
                    return filterAction(filters[0], obj, args,
                        (obj, args) => filterAction(filters[1], obj, args,
                        (obj, args) => filterAction(filters[2], obj, args,
                        (obj, args) => filterAction(filters[3], obj, args,
                        (obj, args) => filterAction(filters[4], obj, args,
                        (obj, args) => filterAction(filters[5], obj, args, action)
                        )))));
                }
                else if (filters.Length == 7)
                {
                    return filterAction(filters[0], obj, args,
                        (obj, args) => filterAction(filters[1], obj, args,
                        (obj, args) => filterAction(filters[2], obj, args,
                        (obj, args) => filterAction(filters[3], obj, args,
                        (obj, args) => filterAction(filters[4], obj, args,
                        (obj, args) => filterAction(filters[5], obj, args,
                        (obj, args) => filterAction(filters[6], obj, args, action)
                        ))))));
                }
                else if (filters.Length == 8)
                {
                    return filterAction(filters[0], obj, args,
                        (obj, args) => filterAction(filters[1], obj, args,
                        (obj, args) => filterAction(filters[2], obj, args,
                        (obj, args) => filterAction(filters[3], obj, args,
                        (obj, args) => filterAction(filters[4], obj, args,
                        (obj, args) => filterAction(filters[5], obj, args,
                        (obj, args) => filterAction(filters[6], obj, args,
                        (obj, args) => filterAction(filters[7], obj, args, action)
                        )))))));
                }
            }

            static ValueTask recursive(
                int index,
                TFilter[] filters,
                TObj obj,
                TArgs args,
                Func<TObj, TArgs, ValueTask> action,
                Func<TFilter, TObj, TArgs, Func<TObj, TArgs, ValueTask>, ValueTask> filterAction
                )
            {
                index += 1;

                if (index == filters.Length - 1)
                {
                    return filterAction(filters[index], obj, args, action);
                }
                else
                {
                    return filterAction(filters[index], obj, args, (_obj, _args) => recursive(index, filters, _obj, _args, action, filterAction));
                }
            }

            return recursive(
                -1,
                filters,
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
        internal static TResult InvokeFunc<TFilter, TObj, TResult>(
            TFilter[] filters,
            TObj obj,
            Func<TObj, TResult> func,
            Func<TFilter, TObj, Func<TObj, TResult>, TResult> filterFunc
            )
        {
            if (filters == null || filters.Length == 0)
            {
                return func(obj);
            }
            else if (filters.Length == 1)
            {
                return filterFunc(filters[0], obj, func);
            }

            if ((Algorithm & FilterAlgorithms.Recursive) == 0)
            {
                if (filters.Length == 2)
                {
                    return filterFunc(filters[0], obj,
                        obj => filterFunc(filters[1], obj, func)
                        );
                }
                else if (filters.Length == 3)
                {
                    return filterFunc(filters[0], obj,
                        obj => filterFunc(filters[1], obj,
                        obj => filterFunc(filters[2], obj, func)
                        ));
                }
                else if (filters.Length == 4)
                {
                    return filterFunc(filters[0], obj,
                        obj => filterFunc(filters[1], obj,
                        obj => filterFunc(filters[2], obj,
                        obj => filterFunc(filters[3], obj, func)
                        )));
                }
                else if (filters.Length == 5)
                {
                    return filterFunc(filters[0], obj,
                        obj => filterFunc(filters[1], obj,
                        obj => filterFunc(filters[2], obj,
                        obj => filterFunc(filters[3], obj,
                        obj => filterFunc(filters[4], obj, func)
                        ))));
                }
                else if (filters.Length == 6)
                {
                    return filterFunc(filters[0], obj,
                        obj => filterFunc(filters[1], obj,
                        obj => filterFunc(filters[2], obj,
                        obj => filterFunc(filters[3], obj,
                        obj => filterFunc(filters[4], obj,
                        obj => filterFunc(filters[5], obj, func)
                        )))));
                }
                else if (filters.Length == 7)
                {
                    return filterFunc(filters[0], obj,
                        obj => filterFunc(filters[1], obj,
                        obj => filterFunc(filters[2], obj,
                        obj => filterFunc(filters[3], obj,
                        obj => filterFunc(filters[4], obj,
                        obj => filterFunc(filters[5], obj,
                        obj => filterFunc(filters[6], obj, func)
                        ))))));
                }
                else if (filters.Length == 8)
                {
                    return filterFunc(filters[0], obj,
                        obj => filterFunc(filters[1], obj,
                        obj => filterFunc(filters[2], obj,
                        obj => filterFunc(filters[3], obj,
                        obj => filterFunc(filters[4], obj,
                        obj => filterFunc(filters[5], obj,
                        obj => filterFunc(filters[6], obj,
                        obj => filterFunc(filters[7], obj, func)
                        )))))));
                }
            }

            static TResult recursive(
                int index,
                TFilter[] filters,
                TObj obj,
                Func<TObj, TResult> func,
                Func<TFilter, TObj, Func<TObj, TResult>, TResult> filterFunc
                )
            {
                index += 1;

                if (index == filters.Length - 1)
                {
                    return filterFunc(filters[index], obj, func);
                }
                else
                {
                    return filterFunc(filters[index], obj, _obj => recursive(index, filters, _obj, func, filterFunc));
                }
            }

            return recursive(
                -1,
                filters,
                obj,
                func,
                filterFunc
                );
        }

        #endregion

        #region InvokeFunc(args)

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
        internal static TResult InvokeFunc<TFilter, TObj, TArgs, TResult>(
            TFilter[] filters,
            TObj obj,
            TArgs args,
            Func<TObj, TArgs, TResult> func,
            Func<TFilter, TObj, TArgs, Func<TObj, TArgs, TResult>, TResult> filterFunc
            )
        {
            if (filters == null || filters.Length == 0)
            {
                return func(obj, args);
            }
            else if (filters.Length == 1)
            {
                return filterFunc(filters[0], obj, args, func);
            }

            if ((Algorithm & FilterAlgorithms.Recursive) == 0)
            {
                if (filters.Length == 2)
                {
                    return filterFunc(filters[0], obj, args,
                        (obj, args) => filterFunc(filters[1], obj, args, func)
                        );
                }
                else if (filters.Length == 3)
                {
                    return filterFunc(filters[0], obj, args,
                        (obj, args) => filterFunc(filters[1], obj, args,
                        (obj, args) => filterFunc(filters[2], obj, args, func)
                        ));
                }
                else if (filters.Length == 4)
                {
                    return filterFunc(filters[0], obj, args,
                        (obj, args) => filterFunc(filters[1], obj, args,
                        (obj, args) => filterFunc(filters[2], obj, args,
                        (obj, args) => filterFunc(filters[3], obj, args, func)
                        )));
                }
                else if (filters.Length == 5)
                {
                    return filterFunc(filters[0], obj, args,
                        (obj, args) => filterFunc(filters[1], obj, args,
                        (obj, args) => filterFunc(filters[2], obj, args,
                        (obj, args) => filterFunc(filters[3], obj, args,
                        (obj, args) => filterFunc(filters[4], obj, args, func)
                        ))));
                }
                else if (filters.Length == 6)
                {
                    return filterFunc(filters[0], obj, args,
                        (obj, args) => filterFunc(filters[1], obj, args,
                        (obj, args) => filterFunc(filters[2], obj, args,
                        (obj, args) => filterFunc(filters[3], obj, args,
                        (obj, args) => filterFunc(filters[4], obj, args,
                        (obj, args) => filterFunc(filters[5], obj, args, func)
                        )))));
                }
                else if (filters.Length == 7)
                {
                    return filterFunc(filters[0], obj, args,
                        (obj, args) => filterFunc(filters[1], obj, args,
                        (obj, args) => filterFunc(filters[2], obj, args,
                        (obj, args) => filterFunc(filters[3], obj, args,
                        (obj, args) => filterFunc(filters[4], obj, args,
                        (obj, args) => filterFunc(filters[5], obj, args,
                        (obj, args) => filterFunc(filters[6], obj, args, func)
                        ))))));
                }
                else if (filters.Length == 8)
                {
                    return filterFunc(filters[0], obj, args,
                        (obj, args) => filterFunc(filters[1], obj, args,
                        (obj, args) => filterFunc(filters[2], obj, args,
                        (obj, args) => filterFunc(filters[3], obj, args,
                        (obj, args) => filterFunc(filters[4], obj, args,
                        (obj, args) => filterFunc(filters[5], obj, args,
                        (obj, args) => filterFunc(filters[6], obj, args,
                        (obj, args) => filterFunc(filters[7], obj, args, func)
                        )))))));
                }
            }

            static TResult recursive(
                int index,
                TFilter[] filters,
                TObj obj,
                TArgs args,
                Func<TObj, TArgs, TResult> func,
                Func<TFilter, TObj, TArgs, Func<TObj, TArgs, TResult>, TResult> filterFunc
                )
            {
                index += 1;

                if (index == filters.Length - 1)
                {
                    return filterFunc(filters[index], obj, args, func);
                }
                else
                {
                    return filterFunc(filters[index], obj, args, (_obj, _args) => recursive(index, filters, _obj, _args, func, filterFunc));
                }
            }

            return recursive(
                -1,
                filters,
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
        internal static Task<TResult> InvokeFuncAsync<TFilter, TObj, TResult>(
            TFilter[] filters,
            TObj obj,
            Func<TObj, Task<TResult>> func,
            Func<TFilter, TObj, Func<TObj, Task<TResult>>, Task<TResult>> filterFunc
            )
        {
            if (filters == null || filters.Length == 0)
            {
                return func(obj);
            }
            else if (filters.Length == 1)
            {
                return filterFunc(filters[0], obj, func);
            }

            if ((Algorithm & FilterAlgorithms.Recursive) == 0)
            {
                if (filters.Length == 2)
                {
                    return filterFunc(filters[0], obj,
                        obj => filterFunc(filters[1], obj, func)
                        );
                }
                else if (filters.Length == 3)
                {
                    return filterFunc(filters[0], obj,
                        obj => filterFunc(filters[1], obj,
                        obj => filterFunc(filters[2], obj, func)
                        ));
                }
                else if (filters.Length == 4)
                {
                    return filterFunc(filters[0], obj,
                        obj => filterFunc(filters[1], obj,
                        obj => filterFunc(filters[2], obj,
                        obj => filterFunc(filters[3], obj, func)
                        )));
                }
                else if (filters.Length == 5)
                {
                    return filterFunc(filters[0], obj,
                        obj => filterFunc(filters[1], obj,
                        obj => filterFunc(filters[2], obj,
                        obj => filterFunc(filters[3], obj,
                        obj => filterFunc(filters[4], obj, func)
                        ))));
                }
                else if (filters.Length == 6)
                {
                    return filterFunc(filters[0], obj,
                        obj => filterFunc(filters[1], obj,
                        obj => filterFunc(filters[2], obj,
                        obj => filterFunc(filters[3], obj,
                        obj => filterFunc(filters[4], obj,
                        obj => filterFunc(filters[5], obj, func)
                        )))));
                }
                else if (filters.Length == 7)
                {
                    return filterFunc(filters[0], obj,
                        obj => filterFunc(filters[1], obj,
                        obj => filterFunc(filters[2], obj,
                        obj => filterFunc(filters[3], obj,
                        obj => filterFunc(filters[4], obj,
                        obj => filterFunc(filters[5], obj,
                        obj => filterFunc(filters[6], obj, func)
                        ))))));
                }
                else if (filters.Length == 8)
                {
                    return filterFunc(filters[0], obj,
                        obj => filterFunc(filters[1], obj,
                        obj => filterFunc(filters[2], obj,
                        obj => filterFunc(filters[3], obj,
                        obj => filterFunc(filters[4], obj,
                        obj => filterFunc(filters[5], obj,
                        obj => filterFunc(filters[6], obj,
                        obj => filterFunc(filters[7], obj, func)
                        )))))));
                }
            }

            static Task<TResult> recursive(
                int index,
                TFilter[] filters,
                TObj obj,
                Func<TObj, Task<TResult>> func,
                Func<TFilter, TObj, Func<TObj, Task<TResult>>, Task<TResult>> filterFunc
                )
            {
                index += 1;

                if (index == filters.Length - 1)
                {
                    return filterFunc(filters[index], obj, func);
                }
                else
                {
                    return filterFunc(filters[index], obj, _obj => recursive(index, filters, _obj, func, filterFunc));
                }
            }

            return recursive(
                -1,
                filters,
                obj,
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
        internal static ValueTask<TResult> InvokeFuncAsync<TFilter, TObj, TResult>(
            TFilter[] filters,
            TObj obj,
            Func<TObj, ValueTask<TResult>> func,
            Func<TFilter, TObj, Func<TObj, ValueTask<TResult>>, ValueTask<TResult>> filterFunc
            )
        {
            if (filters == null || filters.Length == 0)
            {
                return func(obj);
            }
            else if (filters.Length == 1)
            {
                return filterFunc(filters[0], obj, func);
            }

            if ((Algorithm & FilterAlgorithms.Recursive) == 0)
            {
                if (filters.Length == 2)
                {
                    return filterFunc(filters[0], obj,
                        obj => filterFunc(filters[1], obj, func)
                        );
                }
                else if (filters.Length == 3)
                {
                    return filterFunc(filters[0], obj,
                        obj => filterFunc(filters[1], obj,
                        obj => filterFunc(filters[2], obj, func)
                        ));
                }
                else if (filters.Length == 4)
                {
                    return filterFunc(filters[0], obj,
                        obj => filterFunc(filters[1], obj,
                        obj => filterFunc(filters[2], obj,
                        obj => filterFunc(filters[3], obj, func)
                        )));
                }
                else if (filters.Length == 5)
                {
                    return filterFunc(filters[0], obj,
                        obj => filterFunc(filters[1], obj,
                        obj => filterFunc(filters[2], obj,
                        obj => filterFunc(filters[3], obj,
                        obj => filterFunc(filters[4], obj, func)
                        ))));
                }
                else if (filters.Length == 6)
                {
                    return filterFunc(filters[0], obj,
                        obj => filterFunc(filters[1], obj,
                        obj => filterFunc(filters[2], obj,
                        obj => filterFunc(filters[3], obj,
                        obj => filterFunc(filters[4], obj,
                        obj => filterFunc(filters[5], obj, func)
                        )))));
                }
                else if (filters.Length == 7)
                {
                    return filterFunc(filters[0], obj,
                        obj => filterFunc(filters[1], obj,
                        obj => filterFunc(filters[2], obj,
                        obj => filterFunc(filters[3], obj,
                        obj => filterFunc(filters[4], obj,
                        obj => filterFunc(filters[5], obj,
                        obj => filterFunc(filters[6], obj, func)
                        ))))));
                }
                else if (filters.Length == 8)
                {
                    return filterFunc(filters[0], obj,
                        obj => filterFunc(filters[1], obj,
                        obj => filterFunc(filters[2], obj,
                        obj => filterFunc(filters[3], obj,
                        obj => filterFunc(filters[4], obj,
                        obj => filterFunc(filters[5], obj,
                        obj => filterFunc(filters[6], obj,
                        obj => filterFunc(filters[7], obj, func)
                        )))))));
                }
            }

            static ValueTask<TResult> recursive(
                int index,
                TFilter[] filters,
                TObj obj,
                Func<TObj, ValueTask<TResult>> func,
                Func<TFilter, TObj, Func<TObj, ValueTask<TResult>>, ValueTask<TResult>> filterFunc
                )
            {
                index += 1;

                if (index == filters.Length - 1)
                {
                    return filterFunc(filters[index], obj, func);
                }
                else
                {
                    return filterFunc(filters[index], obj, _obj => recursive(index, filters, _obj, func, filterFunc));
                }
            }

            return recursive(
                -1,
                filters,
                obj,
                func,
                filterFunc
                );
        }

        #endregion

        #region InvokeFuncAsync(args)

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
        internal static Task<TResult> InvokeFuncAsync<TFilter, TObj, TArgs, TResult>(
            TFilter[] filters,
            TObj obj,
            TArgs args,
            Func<TObj, TArgs, Task<TResult>> func,
            Func<TFilter, TObj, TArgs, Func<TObj, TArgs, Task<TResult>>, Task<TResult>> filterFunc
            )
        {
            if (filters == null || filters.Length == 0)
            {
                return func(obj,args);
            }
            else if (filters.Length == 1)
            {
                return filterFunc(filters[0], obj, args, func);
            }

            if ((Algorithm & FilterAlgorithms.Recursive) == 0)
            {
                if (filters.Length == 2)
                {
                    return filterFunc(filters[0], obj, args,
                        (obj, args) => filterFunc(filters[1], obj, args, func)
                        );
                }
                else if (filters.Length == 3)
                {
                    return filterFunc(filters[0], obj, args,
                        (obj, args) => filterFunc(filters[1], obj, args,
                        (obj, args) => filterFunc(filters[2], obj, args, func)
                        ));
                }
                else if (filters.Length == 4)
                {
                    return filterFunc(filters[0], obj, args,
                        (obj, args) => filterFunc(filters[1], obj, args,
                        (obj, args) => filterFunc(filters[2], obj, args,
                        (obj, args) => filterFunc(filters[3], obj, args, func)
                        )));
                }
                else if (filters.Length == 5)
                {
                    return filterFunc(filters[0], obj, args,
                        (obj, args) => filterFunc(filters[1], obj, args,
                        (obj, args) => filterFunc(filters[2], obj, args,
                        (obj, args) => filterFunc(filters[3], obj, args,
                        (obj, args) => filterFunc(filters[4], obj, args, func)
                        ))));
                }
                else if (filters.Length == 6)
                {
                    return filterFunc(filters[0], obj, args,
                        (obj, args) => filterFunc(filters[1], obj, args,
                        (obj, args) => filterFunc(filters[2], obj, args,
                        (obj, args) => filterFunc(filters[3], obj, args,
                        (obj, args) => filterFunc(filters[4], obj, args,
                        (obj, args) => filterFunc(filters[5], obj, args, func)
                        )))));
                }
                else if (filters.Length == 7)
                {
                    return filterFunc(filters[0], obj, args,
                        (obj, args) => filterFunc(filters[1], obj, args,
                        (obj, args) => filterFunc(filters[2], obj, args,
                        (obj, args) => filterFunc(filters[3], obj, args,
                        (obj, args) => filterFunc(filters[4], obj, args,
                        (obj, args) => filterFunc(filters[5], obj, args,
                        (obj, args) => filterFunc(filters[6], obj, args, func)
                        ))))));
                }
                else if (filters.Length == 8)
                {
                    return filterFunc(filters[0], obj, args,
                        (obj, args) => filterFunc(filters[1], obj, args,
                        (obj, args) => filterFunc(filters[2], obj, args,
                        (obj, args) => filterFunc(filters[3], obj, args,
                        (obj, args) => filterFunc(filters[4], obj, args,
                        (obj, args) => filterFunc(filters[5], obj, args,
                        (obj, args) => filterFunc(filters[6], obj, args,
                        (obj, args) => filterFunc(filters[7], obj, args, func)
                        )))))));
                }
            }

            static Task<TResult> recursive(
                int index,
                TFilter[] filters,
                TObj obj,
                TArgs args,
                Func<TObj, TArgs, Task<TResult>> func,
                Func<TFilter, TObj, TArgs, Func<TObj, TArgs, Task<TResult>>, Task<TResult>> filterFunc
                )
            {
                index += 1;

                if (index == filters.Length - 1)
                {
                    return filterFunc(filters[index], obj, args, func);
                }
                else
                {
                    return filterFunc(filters[index], obj, args, (_obj, _args) => recursive(index, filters, _obj, _args, func, filterFunc));
                }
            }

            return recursive(
                -1,
                filters,
                obj,
                args,
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
        internal static ValueTask<TResult> InvokeFuncAsync<TFilter, TObj, TArgs, TResult>(
            TFilter[] filters,
            TObj obj,
            TArgs args,
            Func<TObj, TArgs, ValueTask<TResult>> func,
            Func<TFilter, TObj, TArgs, Func<TObj, TArgs, ValueTask<TResult>>, ValueTask<TResult>> filterFunc
            )
        {
            if (filters == null || filters.Length == 0)
            {
                return func(obj, args);
            }
            else if (filters.Length == 1)
            {
                return filterFunc(filters[0], obj, args, func);
            }

            if ((Algorithm & FilterAlgorithms.Recursive) == 0)
            {
                if (filters.Length == 2)
                {
                    return filterFunc(filters[0], obj, args,
                        (obj, args) => filterFunc(filters[1], obj, args, func)
                        );
                }
                else if (filters.Length == 3)
                {
                    return filterFunc(filters[0], obj, args,
                        (obj, args) => filterFunc(filters[1], obj, args,
                        (obj, args) => filterFunc(filters[2], obj, args, func)
                        ));
                }
                else if (filters.Length == 4)
                {
                    return filterFunc(filters[0], obj, args,
                        (obj, args) => filterFunc(filters[1], obj, args,
                        (obj, args) => filterFunc(filters[2], obj, args,
                        (obj, args) => filterFunc(filters[3], obj, args, func)
                        )));
                }
                else if (filters.Length == 5)
                {
                    return filterFunc(filters[0], obj, args,
                        (obj, args) => filterFunc(filters[1], obj, args,
                        (obj, args) => filterFunc(filters[2], obj, args,
                        (obj, args) => filterFunc(filters[3], obj, args,
                        (obj, args) => filterFunc(filters[4], obj, args, func)
                        ))));
                }
                else if (filters.Length == 6)
                {
                    return filterFunc(filters[0], obj, args,
                        (obj, args) => filterFunc(filters[1], obj, args,
                        (obj, args) => filterFunc(filters[2], obj, args,
                        (obj, args) => filterFunc(filters[3], obj, args,
                        (obj, args) => filterFunc(filters[4], obj, args,
                        (obj, args) => filterFunc(filters[5], obj, args, func)
                        )))));
                }
                else if (filters.Length == 7)
                {
                    return filterFunc(filters[0], obj, args,
                        (obj, args) => filterFunc(filters[1], obj, args,
                        (obj, args) => filterFunc(filters[2], obj, args,
                        (obj, args) => filterFunc(filters[3], obj, args,
                        (obj, args) => filterFunc(filters[4], obj, args,
                        (obj, args) => filterFunc(filters[5], obj, args,
                        (obj, args) => filterFunc(filters[6], obj, args, func)
                        ))))));
                }
                else if (filters.Length == 8)
                {
                    return filterFunc(filters[0], obj, args,
                        (obj, args) => filterFunc(filters[1], obj, args,
                        (obj, args) => filterFunc(filters[2], obj, args,
                        (obj, args) => filterFunc(filters[3], obj, args,
                        (obj, args) => filterFunc(filters[4], obj, args,
                        (obj, args) => filterFunc(filters[5], obj, args,
                        (obj, args) => filterFunc(filters[6], obj, args,
                        (obj, args) => filterFunc(filters[7], obj, args, func)
                        )))))));
                }
            }

            static ValueTask<TResult> recursive(
                int index,
                TFilter[] filters,
                TObj obj,
                TArgs args,
                Func<TObj, TArgs, ValueTask<TResult>> func,
                Func<TFilter, TObj, TArgs, Func<TObj, TArgs, ValueTask<TResult>>, ValueTask<TResult>> filterFunc
                )
            {
                index += 1;

                if (index == filters.Length - 1)
                {
                    return filterFunc(filters[index], obj, args, func);
                }
                else
                {
                    return filterFunc(filters[index], obj, args, (_obj, _args) => recursive(index, filters, _obj, _args, func, filterFunc));
                }
            }

            return recursive(
                -1,
                filters,
                obj,
                args,
                func,
                filterFunc
                );
        }

        #endregion
    }
}
