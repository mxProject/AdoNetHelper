using System;
using System.Collections.Generic;
using System.Text;

namespace mxProject.Data
{
    /// <summary>
    /// Defines the targets for a <see cref="IDbCommandFilter"/>.
    /// </summary>
    [Flags]
    public enum DbCommandFilterTargets
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
        Prepare = 2,

        /// <summary>
        /// Rollback
        /// </summary>
        Cancel = 4,

        /// <summary>
        /// Dispose
        /// </summary>
        CreateParameter = 8,

        /// <summary>
        /// ExecuteReader
        /// </summary>
        ExecuteReader = 16,

        /// <summary>
        /// ExecuteScalar
        /// </summary>
        ExecuteScalar = 32,

        /// <summary>
        /// ExecuteNonQuery
        /// </summary>
        ExecuteNonQuery = 64,

        /// <summary>
        /// All
        /// </summary>
        All = Dispose | Prepare | Cancel | CreateParameter | ExecuteReader | ExecuteScalar | ExecuteNonQuery
    }
}
