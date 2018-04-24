using System;

namespace Lenneth.Core.Extensions.Extra.CoreExtensions
{
    public static partial class Extensions
    {
        #region Object

        #region In

        /// <summary>
        ///     A T extension method to determines whether the object is equal to any of the provided values.
        /// </summary>
        /// <param name="this">The object to be compared.</param>
        /// <param name="values">The value list to compare with the object.</param>
        /// <returns>true if the values list contains the object, else false.</returns>
        public static bool In(this sbyte @this, params sbyte[] values)
        {
            return Array.IndexOf(values, @this) != -1;
        }

        #endregion In

        #region NotIn

        /// <summary>
        ///     A T extension method to determines whether the object is not equal to any of the provided values.
        /// </summary>
        /// <param name="this">The object to be compared.</param>
        /// <param name="values">The value list to compare with the object.</param>
        /// <returns>true if the values list doesn't contains the object, else false.</returns>
        public static bool NotIn(this sbyte @this, params sbyte[] values)
        {
            return Array.IndexOf(values, @this) == -1;
        }

        #endregion NotIn

        #endregion Object

        #region Math

        #region Abs

        /// <summary>
        ///     Returns the absolute value of an 8-bit signed integer.
        /// </summary>
        /// <param name="value">A number that is greater than , but less than or equal to .</param>
        /// <returns>An 8-bit signed integer, x, such that 0 ? x ?.</returns>
        public static sbyte Abs(this sbyte value)
        {
            return Math.Abs(value);
        }

        #endregion Abs

        #region Max

        /// <summary>
        ///     Returns the larger of two 8-bit signed integers.
        /// </summary>
        /// <param name="val1">The first of two 8-bit signed integers to compare.</param>
        /// <param name="val2">The second of two 8-bit signed integers to compare.</param>
        /// <returns>Parameter  or , whichever is larger.</returns>
        public static sbyte Max(this sbyte val1, sbyte val2)
        {
            return Math.Max(val1, val2);
        }

        #endregion Max

        #region Min

        /// <summary>
        ///     Returns the smaller of two 8-bit signed integers.
        /// </summary>
        /// <param name="val1">The first of two 8-bit signed integers to compare.</param>
        /// <param name="val2">The second of two 8-bit signed integers to compare.</param>
        /// <returns>Parameter  or , whichever is smaller.</returns>
        public static sbyte Min(this sbyte val1, sbyte val2)
        {
            return Math.Min(val1, val2);
        }

        #endregion Min

        #region Sign

        /// <summary>
        ///     Returns a value indicating the sign of an 8-bit signed integer.
        /// </summary>
        /// <param name="value">A signed number.</param>
        /// <returns>
        ///     A number that indicates the sign of , as shown in the following table.Return value Meaning -1  is less than
        ///     zero. 0  is equal to zero. 1  is greater than zero.
        /// </returns>
        public static int Sign(this sbyte value)
        {
            return Math.Sign(value);
        }

        #endregion Sign

        #endregion Math
    }
}