using System;
using System.Collections.Generic;
using System.Data;
using System.Text;

namespace mxProject.Data.Filters
{
    /// <summary>  
    /// Base class for filtering operations on an <see cref="IDataParameterCollection"/>.  
    /// </summary>  
    public abstract class DataParameterCollectionFilterBase : IDataParameterCollectionFilter
    {
        /// <summary>  
        /// Initializes a new instance of the <see cref="DataParameterCollectionFilterBase"/> class.  
        /// </summary>  
        protected DataParameterCollectionFilterBase()
        {
        }

        /// <inheritdoc/>  
        public virtual DataParameterCollectionFilterTargets TargetMethods => DataParameterCollectionFilterTargets.None;

        /// <inheritdoc/>  
        public virtual int Add(IDataParameterCollection parameters, object parameter, Func<IDataParameterCollection, object, int> continuation)
        {
            return continuation(parameters, parameter);
        }

        /// <inheritdoc/>  
        public virtual void Clear(IDataParameterCollection parameters, Action<IDataParameterCollection> continuation)
        {
            continuation(parameters);
        }

        /// <inheritdoc/>  
        public virtual void Insert(IDataParameterCollection parameters, int index, object parameter, Action<IDataParameterCollection, int, object> continuation)
        {
            continuation(parameters, index, parameter);
        }

        /// <inheritdoc/>  
        public virtual void Remove(IDataParameterCollection parameters, object parameter, Action<IDataParameterCollection, object> continuation)
        {
            continuation(parameters, parameter);
        }

        /// <inheritdoc/>  
        public virtual void RemoveAt(IDataParameterCollection parameters, int index, Action<IDataParameterCollection, int> continuation)
        {
            continuation(parameters, index);
        }

        /// <inheritdoc/>  
        public virtual void RemoveAt(IDataParameterCollection parameters, string parameterName, Action<IDataParameterCollection, string> continuation)
        {
            continuation(parameters, parameterName);
        }
    }
}
