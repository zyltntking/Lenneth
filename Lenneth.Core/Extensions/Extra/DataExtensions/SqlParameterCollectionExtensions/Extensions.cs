﻿using System.Collections.Generic;
using System.Data.SqlClient;

namespace Lenneth.Core.Extensions.Extra.DataExtensions.SqlParameterCollectionExtensions
{
    public static class Extensions
    {
        #region AddRangeWithValue

        /// <summary>
        ///     A SqlParameterCollection extension method that adds a range with value to 'values'.
        /// </summary>
        /// <param name="this">The @this to act on.</param>
        /// <param name="values">The values.</param>
        public static void AddRangeWithValue(this SqlParameterCollection @this, Dictionary<string, object> values)
        {
            foreach (var keyValuePair in values)
            {
                @this.AddWithValue(keyValuePair.Key, keyValuePair.Value);
            }
        }

        #endregion AddRangeWithValue
    }
}