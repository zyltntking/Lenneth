using System;
using System.Collections.Generic;
using System.Linq;

namespace Lenneth.Core.Extensions.Collections.IEnumerable
{
    public static class Extensions
    {
        #region IEnumerable<IEnumerable<T>

        #region MergeDistinctInnerEnumerable

        /// <summary>Enumerates merge distinct inner enumerable in this collection.</summary>
        /// <typeparam name="T">Generic type parameter.</typeparam>
        /// <param name="this">The @this to act on.</param>
        /// <returns>
        ///     An enumerator that allows foreach to be used to process merge distinct inner
        ///     enumerable in this collection.
        /// </returns>
        public static IEnumerable<T> MergeDistinctInnerEnumerable<T>(this IEnumerable<IEnumerable<T>> @this)
        {
            var listItem = @this.ToList();

            var list = new List<T>();

            foreach (var item in listItem)
            {
                list = list.Union(item).ToList();
            }

            return list;
        }

        #endregion MergeDistinctInnerEnumerable

        #region MergeInnerEnumerable

        /// <summary>Enumerates merge inner enumerable in this collection.</summary>
        /// <typeparam name="T">Generic type parameter.</typeparam>
        /// <param name="this">The @this to act on.</param>
        /// <returns>
        ///     An enumerator that allows foreach to be used to process merge inner enumerable in
        ///     this collection.
        /// </returns>
        public static IEnumerable<T> MergeInnerEnumerable<T>(this IEnumerable<IEnumerable<T>> @this)
        {
            var listItem = @this.ToList();

            var list = new List<T>();

            foreach (var item in listItem)
            {
                list.AddRange(item);
            }

            return list;
        }

        #endregion MergeInnerEnumerable

        #endregion IEnumerable<IEnumerable<T>

        #region IEnumerable<T>

        #region ContainsAll

        /// <summary>
        ///     An IEnumerable&lt;T&gt; extension method that query if '@this' contains all.
        /// </summary>
        /// <typeparam name="T">Generic type parameter.</typeparam>
        /// <param name="this">The @this to act on.</param>
        /// <param name="values">A variable-length parameters list containing values.</param>
        /// <returns>true if it succeeds, false if it fails.</returns>
        public static bool ContainsAll<T>(this IEnumerable<T> @this, params T[] values)
        {
            var list = @this.ToArray();
            return values.All(value => list.Contains(value));
        }

        #endregion ContainsAll

        #region ContainsAny

        /// <summary>
        ///     An IEnumerable&lt;T&gt; extension method that query if '@this' contains any.
        /// </summary>
        /// <typeparam name="T">Generic type parameter.</typeparam>
        /// <param name="this">The @this to act on.</param>
        /// <param name="values">A variable-length parameters list containing values.</param>
        /// <returns>true if it succeeds, false if it fails.</returns>
        public static bool ContainsAny<T>(this IEnumerable<T> @this, params T[] values)
        {
            var list = @this.ToArray();
            return values.Any(value => list.Contains(value));
        }

        #endregion ContainsAny

        #region ForEach

        /// <summary>
        ///     Enumerates for each in this collection.
        /// </summary>
        /// <typeparam name="T">Generic type parameter.</typeparam>
        /// <param name="this">The @this to act on.</param>
        /// <param name="action">The action.</param>
        /// <returns>An enumerator that allows foreach to be used to process for each in this collection.</returns>
        public static IEnumerable<T> ForEach<T>(this IEnumerable<T> @this, Action<T> action)
        {
            T[] array = @this.ToArray();
            foreach (T t in array)
            {
                action(t);
            }
            return array;
        }

        /// <summary>Enumerates for each in this collection.</summary>
        /// <typeparam name="T">Generic type parameter.</typeparam>
        /// <param name="this">The @this to act on.</param>
        /// <param name="action">The action.</param>
        /// <returns>An enumerator that allows foreach to be used to process for each in this collection.</returns>
        public static IEnumerable<T> ForEach<T>(this IEnumerable<T> @this, Action<T, int> action)
        {
            var array = @this.ToArray();

            for (var i = 0; i < array.Length; i++)
            {
                action(array[i], i);
            }

            return array;
        }

        #endregion ForEach

        #region IsEmpty

        /// <summary>
        ///     An IEnumerable&lt;T&gt; extension method that query if 'collection' is empty.
        /// </summary>
        /// <typeparam name="T">Generic type parameter.</typeparam>
        /// <param name="this">The collection to act on.</param>
        /// <returns>true if empty, false if not.</returns>
        public static bool IsEmpty<T>(this IEnumerable<T> @this)
        {
            return !@this.Any();
        }

        #endregion IsEmpty

        #region IsNotEmpty

        /// <summary>
        ///     An IEnumerable&lt;T&gt; extension method that queries if a not is empty.
        /// </summary>
        /// <typeparam name="T">Generic type parameter.</typeparam>
        /// <param name="this">The collection to act on.</param>
        /// <returns>true if a not is t>, false if not.</returns>
        public static bool IsNotEmpty<T>(this IEnumerable<T> @this)
        {
            return @this.Any();
        }

        #endregion IsNotEmpty

        #region IsNotNullOrEmpty

        /// <summary>
        ///     An IEnumerable&lt;T&gt; extension method that queries if a not null or is empty.
        /// </summary>
        /// <typeparam name="T">Generic type parameter.</typeparam>
        /// <param name="this">The collection to act on.</param>
        /// <returns>true if a not null or is t>, false if not.</returns>
        public static bool IsNotNullOrEmpty<T>(this IEnumerable<T> @this)
        {
            return @this != null && @this.Any();
        }

        #endregion IsNotNullOrEmpty

        #region IsNullOrEmpty

        /// <summary>
        ///     An IEnumerable&lt;T&gt; extension method that queries if a null or is empty.
        /// </summary>
        /// <typeparam name="T">Generic type parameter.</typeparam>
        /// <param name="this">The collection to act on.</param>
        /// <returns>true if a null or is t>, false if not.</returns>
        public static bool IsNullOrEmpty<T>(this IEnumerable<T> @this)
        {
            return @this == null || !@this.Any();
        }

        #endregion IsNullOrEmpty

        #region StringJoin

        /// <summary>
        ///     Concatenates all the elements of a IEnumerable, using the specified separator between each element.
        /// </summary>
        /// <typeparam name="T">Generic type parameter.</typeparam>
        /// <param name="this">An IEnumerable that contains the elements to concatenate.</param>
        /// <param name="separator">
        ///     The string to use as a separator. separator is included in the returned string only if
        ///     value has more than one element.
        /// </param>
        /// <returns>
        ///     A string that consists of the elements in value delimited by the separator string. If value is an empty array,
        ///     the method returns String.Empty.
        /// </returns>
        public static string StringJoin<T>(this IEnumerable<T> @this, string separator)
        {
            return string.Join(separator, @this);
        }

        /// <summary>
        ///     Concatenates all the elements of a IEnumerable, using the specified separator between
        ///     each element.
        /// </summary>
        /// <typeparam name="T">Generic type parameter.</typeparam>
        /// <param name="this">The @this to act on.</param>
        /// <param name="separator">
        ///     The string to use as a separator. separator is included in the
        ///     returned string only if value has more than one element.
        /// </param>
        /// <returns>
        ///     A string that consists of the elements in value delimited by the separator string. If
        ///     value is an empty array, the method returns String.Empty.
        /// </returns>
        public static string StringJoin<T>(this IEnumerable<T> @this, char separator)
        {
            return string.Join(separator.ToString(), @this);
        }

        #endregion StringJoin

        #endregion IEnumerable<T>
    }
}