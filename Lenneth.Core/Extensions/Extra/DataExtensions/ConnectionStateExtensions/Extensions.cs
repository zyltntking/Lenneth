﻿using System.Data;

namespace Lenneth.Core.Extensions.Extra.DataExtensions
{
    public static partial class Extensions
    {
        #region In

        /// <summary>
        ///     A ConnectionState extension method that insert.
        /// </summary>
        /// <param name="this">The @this to act on.</param>
        /// <param name="values">A variable-length parameters list containing values.</param>
        /// <returns>true if it succeeds, false if it fails.</returns>
        public static bool In(this ConnectionState @this, params ConnectionState[] values)
        {
            return values.IndexOf(@this) != -1;
        }

        #endregion In

        #region NotIn

        /// <summary>
        ///     A ConnectionState extension method that not in.
        /// </summary>
        /// <param name="this">The @this to act on.</param>
        /// <param name="values">A variable-length parameters list containing values.</param>
        /// <returns>true if it succeeds, false if it fails.</returns>
        public static bool NotIn(this ConnectionState @this, params ConnectionState[] values)
        {
            return values.IndexOf(@this) == -1;
        }

        #endregion NotIn
    }
}