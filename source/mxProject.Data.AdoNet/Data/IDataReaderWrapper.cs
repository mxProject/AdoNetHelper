using System;
using System.Collections.Generic;
using System.Data;

namespace mxProject.Data
{
    /// <summary>
    /// Represents a wrapper for an <see cref="IDataReader"/>.
    /// </summary>
    public interface IDataReaderWrapper
    {
        /// <summary>
        /// Gets the wrapped <see cref="IDataReader"/> instance.
        /// </summary>
        IDataReader WrappedReader { get; }
    }
}
