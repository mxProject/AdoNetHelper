using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.Text;
using mxProject.Data.Filters;

namespace mxProject.Data.Wrappers
{
    /// <summary>  
    /// Represents a wrapper for <see cref="IDataParameterCollection"/> that applies filters to its operations.  
    /// </summary>  
    internal sealed class DataParameterCollectionWithFilter : IDataParameterCollection, IDataParameterCollectionWrapper
    {
        /// <summary>  
        /// Initializes a new instance of the <see cref="DataParameterCollectionWithFilter"/> class.  
        /// </summary>  
        /// <param name="paramters">The wrapped parameter collection.</param>  
        /// <param name="filters">The filters to apply to the collection operations.</param>  
        internal DataParameterCollectionWithFilter(IDataParameterCollection paramters, DbFilterSet<DataParameterCollectionFilterTargets, IDataParameterCollectionFilter> filters)
        {
            WrappedParameters = paramters;
            m_Filters = filters;
        }

        #region collection  

        /// <summary>  
        /// Gets the wrapped parameter collection.  
        /// </summary>  
        internal IDataParameterCollection WrappedParameters { get; }

        IDataParameterCollection IDataParameterCollectionWrapper.WrappedCollection => WrappedParameters;

        #endregion

        #region filter  

        /// <summary>  
        /// The filters applied to the collection operations.  
        /// </summary>  
        private readonly DbFilterSet<DataParameterCollectionFilterTargets, IDataParameterCollectionFilter> m_Filters;

        #endregion

        /// <inheritdoc/>  
        public object this[string parameterName]
        {
            get => WrappedParameters[parameterName];
            set => WrappedParameters[parameterName] = value;
        }

        /// <inheritdoc/>  
        public object this[int index]
        {
            get => WrappedParameters[index];
            set => WrappedParameters[index] = value;
        }

        /// <inheritdoc/>  
        public bool IsFixedSize => WrappedParameters.IsFixedSize;

        /// <inheritdoc/>  
        public bool IsReadOnly => WrappedParameters.IsReadOnly;

        /// <inheritdoc/>  
        public int Count => WrappedParameters.Count;

        /// <inheritdoc/>  
        public bool IsSynchronized => WrappedParameters.IsSynchronized;

        /// <inheritdoc/>  
        public object SyncRoot => WrappedParameters.SyncRoot;

        /// <inheritdoc/>  
        public void RemoveAt(string parameterName)
        {
            m_Filters[DataParameterCollectionFilterTargets.Remove].RemoveAt(WrappedParameters, parameterName);
        }

        /// <inheritdoc/>  
        public void RemoveAt(int index)
        {
            m_Filters[DataParameterCollectionFilterTargets.Remove].RemoveAt(WrappedParameters, index);
        }

        /// <inheritdoc/>  
        public void Clear()
        {
            m_Filters[DataParameterCollectionFilterTargets.Clear].Clear(WrappedParameters);
        }

        /// <inheritdoc/>  
        public void Insert(int index, object value)
        {
            m_Filters[DataParameterCollectionFilterTargets.Insert].Insert(WrappedParameters, index, value);
        }

        /// <inheritdoc/>  
        public void Remove(object value)
        {
            m_Filters[DataParameterCollectionFilterTargets.Remove].Remove(WrappedParameters, value);
        }

        /// <inheritdoc/>  
        public void CopyTo(Array array, int index)
        {
            WrappedParameters.CopyTo(array, index);
        }

        /// <inheritdoc/>  
        public bool Contains(string parameterName)
        {
            return WrappedParameters.Contains(parameterName);
        }

        /// <inheritdoc/>  
        public bool Contains(object value)
        {
            return WrappedParameters.Contains(value);
        }

        /// <inheritdoc/>  
        public int IndexOf(string parameterName)
        {
            return WrappedParameters.IndexOf(parameterName);
        }

        /// <inheritdoc/>  
        public int IndexOf(object value)
        {
            return WrappedParameters.IndexOf(value);
        }

        /// <inheritdoc/>  
        public int Add(object value)
        {
            return m_Filters[DataParameterCollectionFilterTargets.Add].Add(WrappedParameters, value);
        }

        /// <inheritdoc/>  
        public IEnumerator GetEnumerator()
        {
            return WrappedParameters.GetEnumerator();
        }
    }
}
