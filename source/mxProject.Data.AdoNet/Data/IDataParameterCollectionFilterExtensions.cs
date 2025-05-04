using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Common;
using System.Reflection;
using System.Threading;
using System.Threading.Tasks;
using System.Xml.Linq;
using mxProject.Data.Extensions;
using mxProject.Data.Wrappers;

namespace mxProject.Data
{
    /// <summary>  
    /// Provides extension methods for <see cref="IDataParameterCollectionFilter"/> arrays to handle parameter collection operations.  
    /// </summary>  
    public static class IDataParameterCollectionFilterExtensions
    {
        #region Add  

        /// <summary>  
        /// Adds a parameter to the collection.  
        /// </summary>  
        /// <param name="this">The array of filters.</param>  
        /// <param name="parameters">The parameter collection.</param>  
        /// <param name="parameter">The parameter to add.</param>  
        /// <returns>The index at which the parameter was added.</returns>  
        public static int Add(this IDataParameterCollectionFilter[] @this, IDataParameterCollection parameters, object parameter)
        {
            if (@this == null || @this.Length == 0)
            {
                return parameters.Add(parameter);
            }
            else
            {
                return FilterUtility.InvokeFunc(
                    @this,
                    parameters,
                    parameter,
                    (_parameters, _parameter) => _parameters.Add(_parameter),
                    (_filter, _parameters, _parameter, _continuation) => _filter.Add(_parameters, _parameter, _continuation)
                    );
            }
        }

        #endregion

        #region Index  

        /// <summary>  
        /// Inserts a parameter into the collection at the specified index.  
        /// </summary>  
        /// <param name="this">The array of filters.</param>  
        /// <param name="parameters">The parameter collection.</param>  
        /// <param name="index">The zero-based index at which the parameter should be inserted.</param>  
        /// <param name="parameter">The parameter to insert.</param>  
        public static void Insert(this IDataParameterCollectionFilter[] @this, IDataParameterCollection parameters, int index, object parameter)
        {
            if (@this == null || @this.Length == 0)
            {
                parameters.Insert(index, parameter);
            }
            else
            {
                FilterUtility.InvokeAction(
                    @this,
                    parameters,
                    (index, parameter),
                    (_parameters, args) => _parameters.Insert(args.index, args.parameter),
                    (_filter, _parameters, args, _continuation) => _filter.Insert(_parameters, args.index, args.parameter, (parameters, idx, param) => _continuation(parameters, (idx, param)))
                    );
            }
        }

        #endregion

        #region Remove  

        /// <summary>  
        /// Removes the parameter at the specified index from the collection.  
        /// </summary>  
        /// <param name="this">The array of filters.</param>  
        /// <param name="parameters">The parameter collection.</param>  
        /// <param name="index">The zero-based index of the parameter to remove.</param>  
        public static void RemoveAt(this IDataParameterCollectionFilter[] @this, IDataParameterCollection parameters, int index)
        {
            if (@this == null || @this.Length == 0)
            {
                parameters.RemoveAt(index);
            }
            else
            {
                FilterUtility.InvokeAction(
                    @this,
                    parameters,
                    index,
                    (_parameters, _index) => _parameters.RemoveAt(_index),
                    (_filter, _parameters, _index, _continuation) => _filter.RemoveAt(_parameters, _index, _continuation)
                    );
            }
        }

        /// <summary>  
        /// Removes the parameter with the specified name from the collection.  
        /// </summary>  
        /// <param name="this">The array of filters.</param>  
        /// <param name="parameters">The parameter collection.</param>  
        /// <param name="name">The name of the parameter to remove.</param>  
        public static void RemoveAt(this IDataParameterCollectionFilter[] @this, IDataParameterCollection parameters, string name)
        {
            if (@this == null || @this.Length == 0)
            {
                parameters.RemoveAt(name);
            }
            else
            {
                FilterUtility.InvokeAction(
                    @this,
                    parameters,
                    name,
                    (_parameters, _name) => _parameters.RemoveAt(_name),
                    (_filter, _parameters, _name, _continuation) => _filter.RemoveAt(_parameters, _name, _continuation)
                    );
            }
        }

        /// <summary>  
        /// Removes the specified parameter from the collection.  
        /// </summary>  
        /// <param name="this">The array of filters.</param>  
        /// <param name="parameters">The parameter collection.</param>  
        /// <param name="parameter">The parameter to remove.</param>  
        public static void Remove(this IDataParameterCollectionFilter[] @this, IDataParameterCollection parameters, object parameter)
        {
            if (@this == null || @this.Length == 0)
            {
                parameters.Remove(parameter);
            }
            else
            {
                FilterUtility.InvokeAction(
                    @this,
                    parameters,
                    parameter,
                    (_parameters, _parameter) => _parameters.Remove(_parameter),
                    (_filter, _parameters, _parameter, _continuation) => _filter.Remove(_parameters, _parameter, _continuation)
                    );
            }
        }

        #endregion

        #region Clear  

        /// <summary>  
        /// Clears all parameters from the collection.  
        /// </summary>  
        /// <param name="this">The array of filters.</param>  
        /// <param name="parameters">The parameter collection.</param>  
        public static void Clear(this IDataParameterCollectionFilter[] @this, IDataParameterCollection parameters)
        {
            if (@this == null || @this.Length == 0)
            {
                parameters.Clear();
            }
            else
            {
                FilterUtility.InvokeAction(
                    @this,
                    parameters,
                    _parameters => _parameters.Clear(),
                    (_filter, _parameters, _continuation) => _filter.Clear(_parameters, _continuation)
                    );
            }
        }

        #endregion

    }
}