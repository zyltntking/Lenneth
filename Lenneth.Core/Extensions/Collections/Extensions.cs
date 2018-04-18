using System;
using System.Collections.Generic;
using System.Linq;

namespace Lenneth.Core.Extensions.Collections
{
    public static class Extensions
    {
        #region ICollection AddIf

        /// <summary>
        ///     An ICollection&lt;T&gt; extension method that adds only if the value satisfies the predicate.
        /// </summary>
        /// <typeparam name="T">Generic type parameter.</typeparam>
        /// <param name="this">The @this to act on.</param>
        /// <param name="predicate">The predicate.</param>
        /// <param name="value">The value.</param>
        /// <returns>true if it succeeds, false if it fails.</returns>
        public static bool AddIf<T>(this ICollection<T> @this, Func<T, bool> predicate, T value)
        {
            if (predicate(value))
            {
                @this.Add(value);
                return true;
            }

            return false;
        }

        #endregion ICollection AddIf

        #region ICollection AddIfNotContains

        /// <summary>
        ///     An ICollection&lt;T&gt; extension method that add value if the ICollection doesn't contains it already.
        /// </summary>
        /// <typeparam name="T">Generic type parameter.</typeparam>
        /// <param name="this">The @this to act on.</param>
        /// <param name="value">The value.</param>
        /// <returns>true if it succeeds, false if it fails.</returns>
        public static bool AddIfNotContains<T>(this ICollection<T> @this, T value)
        {
            if (!@this.Contains(value))
            {
                @this.Add(value);
                return true;
            }

            return false;
        }

        #endregion ICollection AddIfNotContains

        #region ICollection AddRange

        /// <summary>
        ///     An ICollection&lt;T&gt; extension method that adds a range to 'values'.
        /// </summary>
        /// <typeparam name="T">Generic type parameter.</typeparam>
        /// <param name="this">The @this to act on.</param>
        /// <param name="values">A variable-length parameters list containing values.</param>
        public static void AddRange<T>(this ICollection<T> @this, params T[] values)
        {
            foreach (var value in values)
            {
                @this.Add(value);
            }
        }

        #endregion ICollection AddRange

        #region ICollection AddRangeIf

        /// <summary>
        ///     An ICollection&lt;T&gt; extension method that adds a collection of objects to the end of this collection only
        ///     for value who satisfies the predicate.
        /// </summary>
        /// <typeparam name="T">Generic type parameter.</typeparam>
        /// <param name="this">The @this to act on.</param>
        /// <param name="predicate">The predicate.</param>
        /// <param name="values">A variable-length parameters list containing values.</param>
        public static void AddRangeIf<T>(this ICollection<T> @this, Func<T, bool> predicate, params T[] values)
        {
            foreach (var value in values)
            {
                if (predicate(value))
                {
                    @this.Add(value);
                }
            }
        }

        #endregion ICollection AddRangeIf

        #region ICollection AddRangeIfNotContains

        /// <summary>
        ///     An ICollection&lt;T&gt; extension method that adds a range of values that's not already in the ICollection.
        /// </summary>
        /// <typeparam name="T">Generic type parameter.</typeparam>
        /// <param name="this">The @this to act on.</param>
        /// <param name="values">A variable-length parameters list containing values.</param>
        public static void AddRangeIfNotContains<T>(this ICollection<T> @this, params T[] values)
        {
            foreach (var value in values)
            {
                if (!@this.Contains(value))
                {
                    @this.Add(value);
                }
            }
        }

        #endregion ICollection AddRangeIfNotContains

        #region ICollection ContainsAll

        /// <summary>
        ///     An ICollection&lt;T&gt; extension method that query if '@this' contains all values.
        /// </summary>
        /// <typeparam name="T">Generic type parameter.</typeparam>
        /// <param name="this">The @this to act on.</param>
        /// <param name="values">A variable-length parameters list containing values.</param>
        /// <returns>true if it succeeds, false if it fails.</returns>
        public static bool ContainsAll<T>(this ICollection<T> @this, params T[] values)
        {
            return values.All(@this.Contains);
        }

        #endregion ICollection ContainsAll

        #region ICollection ContainsAny

        /// <summary>
        ///     An ICollection&lt;T&gt; extension method that query if '@this' contains any value.
        /// </summary>
        /// <typeparam name="T">Generic type parameter.</typeparam>
        /// <param name="this">The @this to act on.</param>
        /// <param name="values">A variable-length parameters list containing values.</param>
        /// <returns>true if it succeeds, false if it fails.</returns>
        public static bool ContainsAny<T>(this ICollection<T> @this, params T[] values)
        {
            foreach (T value in values)
            {
                if (@this.Contains(value))
                {
                    return true;
                }
            }

            return false;
        }

        #endregion ICollection ContainsAny
    }
}