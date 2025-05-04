using System;
using System.Collections.Generic;
using System.Text;

namespace mxProject.Data
{
    /// <summary>
    /// Defines the targets for a <see cref="IDbTransactionFilter"/>.
    /// </summary>
    [Flags]
    public enum DbTransactionFilterTargets
    {
        /// <summary>
        /// None
        /// </summary>
        None = 0,

        /// <summary>
        /// Dispose
        /// </summary>
        Dispose = 1,

        /// <summary>
        /// Commmit
        /// </summary>
        Commmit = 2,

        /// <summary>
        /// Rollback
        /// </summary>
        Rollback = 4,

        /// <summary>
        /// All
        /// </summary>
        All = Dispose | Commmit | Rollback
    }
}
