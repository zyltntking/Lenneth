using System.Data.SqlClient;
using System.Reflection;

namespace Lenneth.Core.Extensions.Extra.DataExtensions.SqlBulkCopyExtensions
{
    public static class Extensions
    {
        #region GetConnection

        /// <summary>A SqlBulkCopy extension method that gets a connection.</summary>
        /// <param name="this">The @this to act on.</param>
        /// <returns>The connection.</returns>
        public static SqlConnection GetConnection(this SqlBulkCopy @this)
        {
            var type = @this.GetType();
            var field = type.GetField("_connection", BindingFlags.NonPublic | BindingFlags.Instance);
            return field?.GetValue(@this) as SqlConnection;
        }

        #endregion GetConnection

        #region GetTransaction

        /// <summary>A SqlBulkCopy extension method that gets a transaction.</summary>
        /// <param name="this">The @this to act on.</param>
        /// <returns>The transaction.</returns>
        public static SqlTransaction GetTransaction(this SqlBulkCopy @this)
        {
            var type = @this.GetType();
            var field = type.GetField("_externalTransaction", BindingFlags.NonPublic | BindingFlags.Instance);
            return field?.GetValue(@this) as SqlTransaction;
        }

        #endregion GetTransaction
    }
}