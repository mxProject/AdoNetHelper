using System;
using System.Collections.Generic;
using System.Data;

namespace mxProject.Data
{
    /// <summary>
    /// Represents a wrapper for an <see cref="IDbConnection"/>.
    /// </summary>
    public interface IDbConnectionWrapper
    {
        /// <summary>
        /// Gets the wrapped <see cref="IDbConnection"/> instance.
        /// </summary>
        IDbConnection WrappedConnection { get; }
    }
}
