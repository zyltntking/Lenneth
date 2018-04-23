﻿using Lenneth.Core.Extensions.Extra.CoreExtensions.ObjectExtensions;
using Lenneth.Core.Extensions.Extra.DataExtensions.IDataReaderExtensions;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Common;

namespace Lenneth.Core.Extensions.Extra.DataExtensions.DbCommandExtensions
{
    public static class Extensions
    {
        #region ExecuteEntities

        /// <summary>
        ///     Enumerates execute entities in this collection.
        /// </summary>
        /// <typeparam name="T">Generic type parameter.</typeparam>
        /// <param name="this">The @this to act on.</param>
        /// <returns>An enumerator that allows foreach to be used to process execute entities in this collection.</returns>
        public static IEnumerable<T> ExecuteEntities<T>(this DbCommand @this) where T : new()
        {
            using (IDataReader reader = @this.ExecuteReader())
            {
                return reader.ToEntities<T>();
            }
        }

        #endregion ExecuteEntities

        #region ExecuteEntity

        /// <summary>
        ///     A DbCommand extension method that executes the entity operation.
        /// </summary>
        /// <typeparam name="T">Generic type parameter.</typeparam>
        /// <param name="this">The @this to act on.</param>
        /// <returns>A T.</returns>
        public static T ExecuteEntity<T>(this DbCommand @this) where T : new()
        {
            using (IDataReader reader = @this.ExecuteReader())
            {
                reader.Read();
                return reader.ToEntity<T>();
            }
        }

        #endregion ExecuteEntity

        #region ExecuteExpandoObject

        /// <summary>
        ///     A DbCommand extension method that executes the expando object operation.
        /// </summary>
        /// <param name="this">The @this to act on.</param>
        /// <returns>A dynamic.</returns>
        public static dynamic ExecuteExpandoObject(this DbCommand @this)
        {
            using (IDataReader reader = @this.ExecuteReader())
            {
                reader.Read();
                return reader.ToExpandoObject();
            }
        }

        #endregion ExecuteExpandoObject

        #region ExecuteExpandoObjects

        /// <summary>
        ///     Enumerates execute expando objects in this collection.
        /// </summary>
        /// <param name="this">The @this to act on.</param>
        /// <returns>
        ///     An enumerator that allows foreach to be used to process execute expando objects in this collection.
        /// </returns>
        public static IEnumerable<dynamic> ExecuteExpandoObjects(this DbCommand @this)
        {
            using (IDataReader reader = @this.ExecuteReader())
            {
                return reader.ToExpandoObjects();
            }
        }

        #endregion ExecuteExpandoObjects

        #region ExecuteScalarAs

        /// <summary>
        ///     A DbCommand extension method that executes the scalar as operation.
        /// </summary>
        /// <typeparam name="T">Generic type parameter.</typeparam>
        /// <param name="this">The @this to act on.</param>
        /// <returns>A T.</returns>
        public static T ExecuteScalarAs<T>(this DbCommand @this)
        {
            return (T)@this.ExecuteScalar();
        }

        #endregion ExecuteScalarAs

        #region ExecuteScalarAsOrDefault

        /// <summary>
        ///     A DbCommand extension method that executes the scalar as or default operation.
        /// </summary>
        /// <typeparam name="T">Generic type parameter.</typeparam>
        /// <param name="this">The @this to act on.</param>
        /// <returns>A T.</returns>
        public static T ExecuteScalarAsOrDefault<T>(this DbCommand @this)
        {
            try
            {
                return (T)@this.ExecuteScalar();
            }
            catch (Exception)
            {
                return default(T);
            }
        }

        /// <summary>
        ///     A DbCommand extension method that executes the scalar as or default operation.
        /// </summary>
        /// <typeparam name="T">Generic type parameter.</typeparam>
        /// <param name="this">The @this to act on.</param>
        /// <param name="defaultValue">The default value.</param>
        /// <returns>A T.</returns>
        public static T ExecuteScalarAsOrDefault<T>(this DbCommand @this, T defaultValue)
        {
            try
            {
                return (T)@this.ExecuteScalar();
            }
            catch (Exception)
            {
                return defaultValue;
            }
        }

        /// <summary>
        ///     A DbCommand extension method that executes the scalar as or default operation.
        /// </summary>
        /// <typeparam name="T">Generic type parameter.</typeparam>
        /// <param name="this">The @this to act on.</param>
        /// <param name="defaultValueFactory">The default value factory.</param>
        /// <returns>A T.</returns>
        public static T ExecuteScalarAsOrDefault<T>(this DbCommand @this, Func<DbCommand, T> defaultValueFactory)
        {
            try
            {
                return (T)@this.ExecuteScalar();
            }
            catch (Exception)
            {
                return defaultValueFactory(@this);
            }
        }

        #endregion ExecuteScalarAsOrDefault

        #region ExecuteScalarTo

        /// <summary>
        ///     A DbCommand extension method that executes the scalar to operation.
        /// </summary>
        /// <typeparam name="T">Generic type parameter.</typeparam>
        /// <param name="this">The @this to act on.</param>
        /// <returns>A T.</returns>
        public static T ExecuteScalarTo<T>(this DbCommand @this)
        {
            return @this.ExecuteScalar().To<T>();
        }

        #endregion ExecuteScalarTo

        #region ExecuteScalarToOrDefault

        /// <summary>
        ///     A DbCommand extension method that executes the scalar to or default operation.
        /// </summary>
        /// <typeparam name="T">Generic type parameter.</typeparam>
        /// <param name="this">The @this to act on.</param>
        /// <returns>A T.</returns>
        public static T ExecuteScalarToOrDefault<T>(this DbCommand @this)
        {
            try
            {
                return @this.ExecuteScalar().To<T>();
            }
            catch (Exception)
            {
                return default(T);
            }
        }

        /// <summary>
        ///     A DbCommand extension method that executes the scalar to or default operation.
        /// </summary>
        /// <typeparam name="T">Generic type parameter.</typeparam>
        /// <param name="this">The @this to act on.</param>
        /// <param name="defaultValue">The default value.</param>
        /// <returns>A T.</returns>
        public static T ExecuteScalarToOrDefault<T>(this DbCommand @this, T defaultValue)
        {
            try
            {
                return @this.ExecuteScalar().To<T>();
            }
            catch (Exception)
            {
                return defaultValue;
            }
        }

        /// <summary>
        ///     A DbCommand extension method that executes the scalar to or default operation.
        /// </summary>
        /// <typeparam name="T">Generic type parameter.</typeparam>
        /// <param name="this">The @this to act on.</param>
        /// <param name="defaultValueFactory">The default value factory.</param>
        /// <returns>A T.</returns>
        public static T ExecuteScalarToOrDefault<T>(this DbCommand @this, Func<DbCommand, T> defaultValueFactory)
        {
            try
            {
                return @this.ExecuteScalar().To<T>();
            }
            catch (Exception)
            {
                return defaultValueFactory(@this);
            }
        }

        #endregion ExecuteScalarToOrDefault
    }
}