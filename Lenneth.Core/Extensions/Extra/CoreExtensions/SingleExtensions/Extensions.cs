﻿using System;

namespace Lenneth.Core.Extensions.Extra.CoreExtensions
{
    public static partial class Extensions
    {
        #region Object

        #region Between

        /// <summary>
        ///     A T extension method that check if the value is between (exclusif) the minValue and maxValue.
        /// </summary>
        /// <param name="this">The @this to act on.</param>
        /// <param name="minValue">The minimum value.</param>
        /// <param name="maxValue">The maximum value.</param>
        /// <returns>true if the value is between the minValue and maxValue, otherwise false.</returns>
        public static bool Between(this float @this, float minValue, float maxValue)
        {
            return minValue.CompareTo(@this) == -1 && @this.CompareTo(maxValue) == -1;
        }

        #endregion Between

        #region In

        /// <summary>
        ///     A T extension method to determines whether the object is equal to any of the provided values.
        /// </summary>
        /// <param name="this">The object to be compared.</param>
        /// <param name="values">The value list to compare with the object.</param>
        /// <returns>true if the values list contains the object, else false.</returns>
        public static bool In(this float @this, params float[] values)
        {
            return Array.IndexOf(values, @this) != -1;
        }

        #endregion In

        #region InRange

        /// <summary>
        ///     A T extension method that check if the value is between inclusively the minValue and maxValue.
        /// </summary>
        /// <param name="this">The @this to act on.</param>
        /// <param name="minValue">The minimum value.</param>
        /// <param name="maxValue">The maximum value.</param>
        /// <returns>true if the value is between inclusively the minValue and maxValue, otherwise false.</returns>
        public static bool InRange(this float @this, float minValue, float maxValue)
        {
            return @this.CompareTo(minValue) >= 0 && @this.CompareTo(maxValue) <= 0;
        }

        #endregion InRange

        #region NotIn

        /// <summary>
        ///     A T extension method to determines whether the object is not equal to any of the provided values.
        /// </summary>
        /// <param name="this">The object to be compared.</param>
        /// <param name="values">The value list to compare with the object.</param>
        /// <returns>true if the values list doesn't contains the object, else false.</returns>
        public static bool NotIn(this float @this, params float[] values)
        {
            return Array.IndexOf(values, @this) == -1;
        }

        #endregion NotIn

        #endregion Object

        #region Math

        #region Abs

        /// <summary>
        ///     Returns the absolute value of a single-precision floating-point number.
        /// </summary>
        /// <param name="value">A number that is greater than or equal to , but less than or equal to .</param>
        /// <returns>A single-precision floating-point number, x, such that 0 ? x ?.</returns>
        public static float Abs(this float value)
        {
            return Math.Abs(value);
        }

        #endregion Abs

        #region Max

        /// <summary>
        ///     Returns the larger of two single-precision floating-point numbers.
        /// </summary>
        /// <param name="val1">The first of two single-precision floating-point numbers to compare.</param>
        /// <param name="val2">The second of two single-precision floating-point numbers to compare.</param>
        /// <returns>Parameter  or , whichever is larger. If , or , or both  and  are equal to ,  is returned.</returns>
        public static float Max(this float val1, float val2)
        {
            return Math.Max(val1, val2);
        }

        #endregion Max

        #region Min

        /// <summary>
        ///     Returns the smaller of two single-precision floating-point numbers.
        /// </summary>
        /// <param name="val1">The first of two single-precision floating-point numbers to compare.</param>
        /// <param name="val2">The second of two single-precision floating-point numbers to compare.</param>
        /// <returns>Parameter  or , whichever is smaller. If , , or both  and  are equal to ,  is returned.</returns>
        public static float Min(this float val1, float val2)
        {
            return Math.Min(val1, val2);
        }

        #endregion Min

        #region Sign

        /// <summary>
        ///     Returns a value indicating the sign of a single-precision floating-point number.
        /// </summary>
        /// <param name="value">A signed number.</param>
        /// <returns>
        ///     A number that indicates the sign of , as shown in the following table.Return value Meaning -1  is less than
        ///     zero. 0  is equal to zero. 1  is greater than zero.
        /// </returns>
        public static int Sign(this float value)
        {
            return Math.Sign(value);
        }

        #endregion Sign

        #endregion Math

        #region Single

        #region IsInfinity

        /// <summary>
        ///     Returns a value indicating whether the specified number evaluates to negative or positive infinity.
        /// </summary>
        /// <param name="f">A single-precision floating-point number.</param>
        /// <returns>true if  evaluates to  or ; otherwise, false.</returns>
        public static bool IsInfinity(this float f)
        {
            return float.IsInfinity(f);
        }

        #endregion IsInfinity

        #region IsNaN

        /// <summary>
        ///     Returns a value that indicates whether the specified value is not a number ().
        /// </summary>
        /// <param name="f">A single-precision floating-point number.</param>
        /// <returns>true if  evaluates to not a number (); otherwise, false.</returns>
        public static bool IsNaN(this float f)
        {
            return float.IsNaN(f);
        }

        #endregion IsNaN

        #region IsNegativeInfinity

        /// <summary>
        ///     Returns a value indicating whether the specified number evaluates to negative infinity.
        /// </summary>
        /// <param name="f">A single-precision floating-point number.</param>
        /// <returns>true if  evaluates to ; otherwise, false.</returns>
        public static bool IsNegativeInfinity(this float f)
        {
            return float.IsNegativeInfinity(f);
        }

        #endregion IsNegativeInfinity

        #region IsPositiveInfinity

        /// <summary>
        ///     Returns a value indicating whether the specified number evaluates to positive infinity.
        /// </summary>
        /// <param name="f">A single-precision floating-point number.</param>
        /// <returns>true if  evaluates to ; otherwise, false.</returns>
        public static bool IsPositiveInfinity(this float f)
        {
            return float.IsPositiveInfinity(f);
        }

        #endregion IsPositiveInfinity

        #endregion Single
    }
}