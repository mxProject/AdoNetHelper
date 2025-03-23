using System;
using System.Collections.Generic;
using System.Text;
using System.Transactions;

namespace mxProject.Data
{
    internal static class TransactionScopeUtility
    {
        /// <summary>
        /// Determines whether there is an active TransactionScope.
        /// </summary>
        /// <returns>True if there is an active TransactionScope; otherwise, false.</returns>
        internal static bool ExistsActiveTransactionScope()
        {
            if (Transaction.Current == null) { return false; }

            return Transaction.Current.TransactionInformation.Status == TransactionStatus.Active;
        }
    }
}
