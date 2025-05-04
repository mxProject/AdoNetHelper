using System;
using System.Collections.Generic;
using System.Data;

namespace mxProject.Data
{
    /// <summary>
    /// Represents a wrapper for an <see cref="IDataParameterCollection"/>.
    /// </summary>
    public interface IDataParameterCollectionWrapper
    {
        /// <summary>
        /// Gets the wrapped <see cref="IDataParameterCollection"/> instance.
        /// </summary>
        IDataParameterCollection WrappedCollection { get; }
    }
}
