using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Data;
using System.Reflection;

namespace Lenneth.Core.Extensions.Extra.CoreExtensions
{
    public static partial class Extensions
    {
        #region Array

        #region AsReadOnly

        /// <summary>
        ///     A T[] extension method that converts an array to a read only.
        /// </summary>
        /// <typeparam name="T">Generic type parameter.</typeparam>
        /// <param name="array">The array to act on.</param>
        /// <returns>A list of.</returns>
        public static ReadOnlyCollection<T> AsReadOnly<T>(this T[] array)
        {
            return Array.AsReadOnly(array);
        }

        #endregion AsReadOnly

        #region Exists

        /// <summary>
        ///     A T[] extension method that exists.
        /// </summary>
        /// <typeparam name="T">Generic type parameter.</typeparam>
        /// <param name="array">The array to act on.</param>
        /// <param name="match">Specifies the match.</param>
        /// <returns>true if it succeeds, false if it fails.</returns>
        public static bool Exists<T>(this T[] array, Predicate<T> match)
        {
            return Array.Exists(array, match);
        }

        #endregion Exists

        #region Find

        /// <summary>
        ///     A T[] extension method that searches for the first match.
        /// </summary>
        /// <typeparam name="T">Generic type parameter.</typeparam>
        /// <param name="array">The array to act on.</param>
        /// <param name="match">Specifies the match.</param>
        /// <returns>A T.</returns>
        public static T Find<T>(this T[] array, Predicate<T> match)
        {
            return Array.Find(array, match);
        }

        #endregion Find

        #region FindAll

        /// <summary>
        ///     A T[] extension method that searches for the first all.
        /// </summary>
        /// <typeparam name="T">Generic type parameter.</typeparam>
        /// <param name="array">The array to act on.</param>
        /// <param name="match">Specifies the match.</param>
        /// <returns>The found all.</returns>
        public static T[] FindAll<T>(this T[] array, Predicate<T> match)
        {
            return Array.FindAll(array, match);
        }

        #endregion FindAll

        #region FindIndex

        /// <summary>
        ///     A T[] extension method that searches for the first index.
        /// </summary>
        /// <typeparam name="T">Generic type parameter.</typeparam>
        /// <param name="array">The array to act on.</param>
        /// <param name="match">Specifies the match.</param>
        /// <returns>The found index.</returns>
        public static int FindIndex<T>(this T[] array, Predicate<T> match)
        {
            return Array.FindIndex(array, match);
        }

        /// <summary>
        ///     A T[] extension method that searches for the first index.
        /// </summary>
        /// <typeparam name="T">Generic type parameter.</typeparam>
        /// <param name="array">The array to act on.</param>
        /// <param name="startIndex">The start index.</param>
        /// <param name="match">Specifies the match.</param>
        /// <returns>The found index.</returns>
        public static int FindIndex<T>(this T[] array, int startIndex, Predicate<T> match)
        {
            return Array.FindIndex(array, startIndex, match);
        }

        /// <summary>
        ///     A T[] extension method that searches for the first index.
        /// </summary>
        /// <typeparam name="T">Generic type parameter.</typeparam>
        /// <param name="array">The array to act on.</param>
        /// <param name="startIndex">The start index.</param>
        /// <param name="count">Number of.</param>
        /// <param name="match">Specifies the match.</param>
        /// <returns>The found index.</returns>
        public static int FindIndex<T>(this T[] array, int startIndex, int count, Predicate<T> match)
        {
            return Array.FindIndex(array, startIndex, count, match);
        }

        #endregion FindIndex

        #region FindLast

        /// <summary>
        ///     A T[] extension method that searches for the first last.
        /// </summary>
        /// <typeparam name="T">Generic type parameter.</typeparam>
        /// <param name="array">The array to act on.</param>
        /// <param name="match">Specifies the match.</param>
        /// <returns>The found last.</returns>
        public static T FindLast<T>(this T[] array, Predicate<T> match)
        {
            return Array.FindLast(array, match);
        }

        #endregion FindLast

        #region FindLastIndex

        /// <summary>
        ///     A T[] extension method that searches for the last index.
        /// </summary>
        /// <typeparam name="T">Generic type parameter.</typeparam>
        /// <param name="array">The array to act on.</param>
        /// <param name="match">Specifies the match.</param>
        /// <returns>The found index.</returns>
        public static int FindLastIndex<T>(this T[] array, Predicate<T> match)
        {
            return Array.FindLastIndex(array, match);
        }

        /// <summary>
        ///     A T[] extension method that searches for the last index.
        /// </summary>
        /// <typeparam name="T">Generic type parameter.</typeparam>
        /// <param name="array">The array to act on.</param>
        /// <param name="startIndex">The start index.</param>
        /// <param name="match">Specifies the match.</param>
        /// <returns>The found index.</returns>
        public static int FindLastIndex<T>(this T[] array, int startIndex, Predicate<T> match)
        {
            return Array.FindLastIndex(array, startIndex, match);
        }

        /// <summary>
        ///     A T[] extension method that searches for the last index.
        /// </summary>
        /// <typeparam name="T">Generic type parameter.</typeparam>
        /// <param name="array">The array to act on.</param>
        /// <param name="startIndex">The start index.</param>
        /// <param name="count">Number of.</param>
        /// <param name="match">Specifies the match.</param>
        /// <returns>The found index.</returns>
        public static int FindLastIndex<T>(this T[] array, int startIndex, int count, Predicate<T> match)
        {
            return Array.FindLastIndex(array, startIndex, count, match);
        }

        #endregion FindLastIndex

        #region ForEach

        /// <summary>
        ///     A T[] extension method that applies an operation to all items in this collection.
        /// </summary>
        /// <typeparam name="T">Generic type parameter.</typeparam>
        /// <param name="array">The array to act on.</param>
        /// <param name="action">The action.</param>
        public static void ForEach<T>(this T[] array, Action<T> action)
        {
            Array.ForEach(array, action);
        }

        #endregion ForEach

        #region TrueForAll

        /// <summary>
        ///     A T[] extension method that true for all.
        /// </summary>
        /// <typeparam name="T">Generic type parameter.</typeparam>
        /// <param name="array">The array to act on.</param>
        /// <param name="match">Specifies the match.</param>
        /// <returns>true if it succeeds, false if it fails.</returns>
        public static bool TrueForAll<T>(this T[] array, Predicate<T> match)
        {
            return Array.TrueForAll(array, match);
        }

        #endregion TrueForAll

        #endregion Array

        #region Other

        #region ClearAll

        /// <summary>
        ///     A T[] extension method that clears all described by @this.
        /// </summary>
        /// <typeparam name="T">Generic type parameter.</typeparam>
        /// <param name="this">The @this to act on.</param>
        /// ###
        /// <returns>.</returns>
        public static void ClearAll<T>(this T[] @this)
        {
            Array.Clear(@this, 0, @this.Length);
        }

        #endregion

        #region ClearAt

        /// <summary>
        ///     A T[] extension method that clears at.
        /// </summary>
        /// <typeparam name="T">Generic type parameter.</typeparam>
        /// <param name="this">The arrayToClear to act on.</param>
        /// <param name="at">at.</param>
        public static void ClearAt<T>(this T[] @this, int at)
        {
            Array.Clear(@this, at, 1);
        }

        #endregion

        #region ToDataTable

        /// <summary>
        ///     A T[] extension method that converts the @this to a data table.
        /// </summary>
        /// <typeparam name="T">Generic type parameter.</typeparam>
        /// <param name="this">The @this to act on.</param>
        /// <returns>@this as a DataTable.</returns>
        public static DataTable ToDataTable<T>(this IEnumerable<T> @this)
        {
            var type = typeof(T);

            var properties = type.GetProperties(BindingFlags.Public | BindingFlags.Instance);
            var fields = type.GetFields(BindingFlags.Public | BindingFlags.Instance);

            var dt = new DataTable();

            foreach (var property in properties)
            {
                dt.Columns.Add(property.Name, property.PropertyType);
            }

            foreach (var field in fields)
            {
                dt.Columns.Add(field.Name, field.FieldType);
            }

            foreach (var item in @this)
            {
                var dr = dt.NewRow();

                foreach (var property in properties)
                {
                    dr[property.Name] = property.GetValue(item, null);
                }

                foreach (var field in fields)
                {
                    dr[field.Name] = field.GetValue(item);
                }

                dt.Rows.Add(dr);
            }

            return dt;
        }

        #endregion

        #endregion
    }
}