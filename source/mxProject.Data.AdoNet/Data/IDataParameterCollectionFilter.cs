using System;
using System.Collections.Generic;
using System.Data;
using System.Text;

namespace mxProject.Data
{
    /// <summary>  
    /// Provides methods to filter operations on an <see cref="IDataParameterCollection"/>.  
    /// </summary>  
    public interface IDataParameterCollectionFilter
    {
        /// <summary>
        /// Gets the methods to filter.
        /// </summary>
        DataParameterCollectionFilterTargets TargetMethods { get; }

        /// <summary>  
        /// Adds a parameter to the collection.  
        /// </summary>  
        /// <param name="parameters">The parameter collection.</param>  
        /// <param name="parameter">The parameter to add.</param>  
        /// <param name="continuation">The function to execute the addition.</param>  
        /// <returns>The index at which the parameter was added.</returns>  
        int Add(IDataParameterCollection parameters, object parameter, Func<IDataParameterCollection, object, int> continuation);

        /// <summary>  
        /// Inserts a parameter into the collection at the specified index.  
        /// </summary>  
        /// <param name="parameters">The parameter collection.</param>  
        /// <param name="index">The zero-based index at which the parameter should be inserted.</param>  
        /// <param name="parameter">The parameter to insert.</param>  
        /// <param name="continuation">The action to execute the insertion.</param>  
        void Insert(IDataParameterCollection parameters, int index, object parameter, Action<IDataParameterCollection, int, object> continuation);

        /// <summary>  
        /// Removes the parameter at the specified index from the collection.  
        /// </summary>  
        /// <param name="parameters">The parameter collection.</param>  
        /// <param name="index">The zero-based index of the parameter to remove.</param>  
        /// <param name="continuation">The action to execute the removal.</param>  
        void RemoveAt(IDataParameterCollection parameters, int index, Action<IDataParameterCollection, int> continuation);

        /// <summary>  
        /// Removes the parameter with the specified name from the collection.  
        /// </summary>  
        /// <param name="parameters">The parameter collection.</param>  
        /// <param name="parameterName">The name of the parameter to remove.</param>  
        /// <param name="continuation">The action to execute the removal.</param>  
        void RemoveAt(IDataParameterCollection parameters, string parameterName, Action<IDataParameterCollection, string> continuation);

        /// <summary>  
        /// Removes the specified parameter from the collection.  
        /// </summary>  
        /// <param name="parameters">The parameter collection.</param>  
        /// <param name="parameter">The parameter to remove.</param>  
        /// <param name="continuation">The action to execute the removal.</param>  
        void Remove(IDataParameterCollection parameters, object parameter, Action<IDataParameterCollection, object> continuation);

        /// <summary>  
        /// Clears all parameters from the collection.  
        /// </summary>  
        /// <param name="parameters">The parameter collection.</param>  
        /// <param name="continuation">The action to execute the clearing.</param>  
        void Clear(IDataParameterCollection parameters, Action<IDataParameterCollection> continuation);
    }
}

