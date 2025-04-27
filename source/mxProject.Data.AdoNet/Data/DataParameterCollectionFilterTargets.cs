using System;
using System.Collections.Generic;
using System.Text;

namespace mxProject.Data
{
    /// <summary>
    /// Defines the targets for a <see cref="IDataParameterCollectionFilter"/>.
    /// </summary>
    [Flags]
    public enum DataParameterCollectionFilterTargets
    {
        /// <summary>
        /// None
        /// </summary>
        None = 0,

        /// <summary>
        /// Add
        /// </summary>
        Add = 1,

        /// <summary>
        /// Insert
        /// </summary>
        Insert = 2,

        /// <summary>
        /// Remove
        /// </summary>
        Remove = 4,

        /// <summary>
        /// Clear
        /// </summary>
        Clear = 8,

        /// <summary>
        /// All
        /// </summary>
        All = Add | Insert | Remove | Clear
    }
}
