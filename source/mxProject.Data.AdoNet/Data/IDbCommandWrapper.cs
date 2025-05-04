using System;
using System.Collections.Generic;
using System.Data;

namespace mxProject.Data
{
    /// <summary>
    /// Represents a wrapper for an <see cref="IDbCommand"/>.
    /// </summary>
    public interface IDbCommandWrapper
    {
        /// <summary>
        /// Gets the wrapped <see cref="IDbCommand"/> instance.
        /// </summary>
        IDbCommand WrappedCommand { get; }
    }
}
