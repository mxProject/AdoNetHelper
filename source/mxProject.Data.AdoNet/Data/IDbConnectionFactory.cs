using System;
using System.Collections.Generic;
using System.Data;
using mxProject.Data.Filters;

namespace mxProject.Data
{
    /// <summary>
    /// Represents a factory for creating <see cref="IDbConnection"/> instances.
    /// </summary>
    public interface IDbConnectionFactory
    {
        /// <summary>  
        /// Creates a new <see cref="IDbConnection"/> instance.  
        /// </summary>  
        /// <returns>A new <see cref="IDbConnection"/> instance.</returns>  
        IDbConnection CreateConnection();
    }
}
