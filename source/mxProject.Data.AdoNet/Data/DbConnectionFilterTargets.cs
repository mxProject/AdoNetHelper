using System;
using System.Collections.Generic;
using System.Text;

namespace mxProject.Data
{
    /// <summary>
    /// Defines the targets for a <see cref="IDbConnectionFilter"/>.
    /// </summary>
    [Flags]
    public enum DbConnectionFilterTargets
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
        /// Open
        /// </summary>
        Open = 2,

        /// <summary>
        /// Close
        /// </summary>
        Close = 4,

        /// <summary>
        /// ChangeDatabase
        /// </summary>
        ChangeDatabase = 8,

        /// <summary>
        /// BeginTransaction
        /// </summary>
        BeginTransaction = 16,

        /// <summary>
        /// CreateCommand
        /// </summary>
        CreateCommand = 32,

        /// <summary>
        /// All
        /// </summary>
        All = Dispose | Open | Close | ChangeDatabase | BeginTransaction | CreateCommand
    }
}
