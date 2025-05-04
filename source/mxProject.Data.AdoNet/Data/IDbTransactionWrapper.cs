using System;
using System.Collections.Generic;
using System.Data;

namespace mxProject.Data
{
    /// <summary>
    /// Represents a wrapper for an <see cref="IDbTransaction"/>.
    /// </summary>
    public interface IDbTransactionWrapper
    {
        /// <summary>
        /// Gets the wrapped <see cref="IDbTransaction"/> instance.
        /// </summary>
        IDbTransaction WrappedTransaction { get; }
    }
}
