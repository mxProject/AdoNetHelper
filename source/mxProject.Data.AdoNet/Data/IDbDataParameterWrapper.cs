using System;
using System.Collections.Generic;
using System.Data;

namespace mxProject.Data
{
    /// <summary>
    /// Represents a wrapper for an <see cref="IDbDataParameter"/>.
    /// </summary>
    public interface IDbDataParameterWrapper
    {
        /// <summary>
        /// Gets the wrapped <see cref="IDbDataParameter"/> instance.
        /// </summary>
        IDbDataParameter WrappedParameter { get; }
    }
}
