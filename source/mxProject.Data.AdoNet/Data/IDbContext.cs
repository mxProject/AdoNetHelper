using System;
using System.Collections.Generic;
using System.Data;
using System.Text;

namespace mxProject.Data
{
    /// <summary>
    /// It provides the functionality required for a data context that uses a database.
    /// </summary>
    public interface IDbContext
    {
        /// <summary>
        /// Gets the current connection.
        /// </summary>
        /// <returns>The connection.</returns>
        IDbConnection GetCurrentConnection();

        /// <summary>
        /// Gets the current transaction.
        /// </summary>
        /// <returns>The transaction.</returns>
        IDbTransaction? GetCurrentTransaction();
    }
}
