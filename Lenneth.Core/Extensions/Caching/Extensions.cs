using System;
using System.Linq.Expressions;
using System.Runtime.Caching;

namespace Lenneth.Core.Extensions.Caching
{
    public static class Extensions
    {
        #region FromCache

        /// <summary>A TKey extension method that from cache.</summary>
        /// <typeparam name="T">Generic type parameter.</typeparam>
        /// <typeparam name="TValue">Type of the value.</typeparam>
        /// <param name="this">The @this to act on.</param>
        /// <param name="cache">The cache.</param>
        /// <param name="key">The key.</param>
        /// <param name="value">The value.</param>
        /// <returns>A TValue.</returns>
        public static TValue FromCache<T, TValue>(this T @this, MemoryCache cache, string key, TValue value)
        {
            var item = cache.AddOrGetExisting(key, value, new CacheItemPolicy()) ?? value;

            return (TValue)item;
        }

        /// <summary>A TKey extension method that from cache.</summary>
        /// <typeparam name="T">Generic type parameter.</typeparam>
        /// <typeparam name="TValue">Type of the value.</typeparam>
        /// <param name="this">The @this to act on.</param>
        /// <param name="key">The key.</param>
        /// <param name="value">The value.</param>
        /// <returns>A TValue.</returns>
        public static TValue FromCache<T, TValue>(this T @this, string key, TValue value)
        {
            return @this.FromCache(MemoryCache.Default, key, value);
        }

        /// <summary>A TKey extension method that from cache.</summary>
        /// <typeparam name="T">Generic type parameter.</typeparam>
        /// <typeparam name="TValue">Type of the value.</typeparam>
        /// <param name="this">The @this to act on.</param>
        /// <param name="cache">The cache.</param>
        /// <param name="key">The key.</param>
        /// <param name="valueFactory">The value factory.</param>
        /// <returns>A TValue.</returns>
        public static TValue FromCache<T, TValue>(this T @this, MemoryCache cache, string key, Expression<Func<T, TValue>> valueFactory)
        {
            var lazy = new Lazy<TValue>(() => valueFactory.Compile()(@this));
            var item = (Lazy<TValue>)cache.AddOrGetExisting(key, lazy, new CacheItemPolicy()) ?? lazy;
            return item.Value;
        }

        /// <summary>A TKey extension method that from cache.</summary>
        /// <typeparam name="T">Generic type parameter.</typeparam>
        /// <typeparam name="TValue">Type of the value.</typeparam>
        /// <param name="this">The @this to act on.</param>
        /// <param name="key">The key.</param>
        /// <param name="valueFactory">The value factory.</param>
        /// <returns>A TValue.</returns>
        public static TValue FromCache<T, TValue>(this T @this, string key, Expression<Func<T, TValue>> valueFactory)
        {
            return @this.FromCache(MemoryCache.Default, key, valueFactory);
        }

        /// <summary>A TKey extension method that from cache.</summary>
        /// <typeparam name="TKey">Type of the key.</typeparam>
        /// <typeparam name="TValue">Type of the value.</typeparam>
        /// <param name="this">The @this to act on.</param>
        /// <param name="valueFactory">The value factory.</param>
        /// <returns>A TValue.</returns>
        public static TValue FromCache<TKey, TValue>(this TKey @this, Expression<Func<TKey, TValue>> valueFactory)
        {
            var key = string.Concat("Z.Caching.FromCache;", typeof(TKey).FullName, valueFactory.ToString());
            return @this.FromCache(MemoryCache.Default, key, valueFactory);
        }

        /// <summary>A TKey extension method that from cache.</summary>
        /// <typeparam name="TKey">Type of the key.</typeparam>
        /// <typeparam name="TValue">Type of the value.</typeparam>
        /// <param name="this">The @this to act on.</param>
        /// <param name="cache">The cache.</param>
        /// <param name="valueFactory">The value factory.</param>
        /// <returns>A TValue.</returns>
        public static TValue FromCache<TKey, TValue>(this TKey @this, MemoryCache cache, Expression<Func<TKey, TValue>> valueFactory)
        {
            var key = string.Concat("Z.Caching.FromCache;", typeof(TKey).FullName, valueFactory.ToString());
            return @this.FromCache(cache, key, valueFactory);
        }

        #endregion FromCache

        #region AddOrGetExisting

        /// <summary>A MemoryCache extension method that adds an or get existing.</summary>
        /// <typeparam name="TValue">Type of the value.</typeparam>
        /// <param name="cache">The cache to act on.</param>
        /// <param name="key">The key.</param>
        /// <param name="value">The value.</param>
        /// <returns>A TValue.</returns>
        public static TValue AddOrGetExisting<TValue>(this MemoryCache cache, string key, TValue value)
        {
            var item = cache.AddOrGetExisting(key, value, new CacheItemPolicy()) ?? value;

            return (TValue)item;
        }

        /// <summary>A MemoryCache extension method that adds an or get existing.</summary>
        /// <typeparam name="TValue">Type of the value.</typeparam>
        /// <param name="cache">The cache to act on.</param>
        /// <param name="key">The key.</param>
        /// <param name="valueFactory">The value factory.</param>
        /// <returns>A TValue.</returns>
        public static TValue AddOrGetExisting<TValue>(this MemoryCache cache, string key, Func<string, TValue> valueFactory)
        {
            var lazy = new Lazy<TValue>(() => valueFactory(key));

            var item = (Lazy<TValue>)cache.AddOrGetExisting(key, lazy, new CacheItemPolicy()) ?? lazy;

            return item.Value;
        }

        /// <summary>A MemoryCache extension method that adds an or get existing.</summary>
        /// <typeparam name="TValue">Type of the value.</typeparam>
        /// <param name="cache">The cache to act on.</param>
        /// <param name="key">The key.</param>
        /// <param name="valueFactory">The value factory.</param>
        /// <param name="policy">The policy.</param>
        /// <param name="regionName">(Optional) name of the region.</param>
        /// <returns>A TValue.</returns>
        public static TValue AddOrGetExisting<TValue>(this MemoryCache cache, string key, Func<string, TValue> valueFactory, CacheItemPolicy policy, string regionName = null)
        {
            var lazy = new Lazy<TValue>(() => valueFactory(key));

            var item = (Lazy<TValue>)cache.AddOrGetExisting(key, lazy, policy, regionName) ?? lazy;

            return item.Value;
        }

        /// <summary>A MemoryCache extension method that adds an or get existing.</summary>
        /// <typeparam name="TValue">Type of the value.</typeparam>
        /// <param name="cache">The cache to act on.</param>
        /// <param name="key">The key.</param>
        /// <param name="valueFactory">The value factory.</param>
        /// <param name="absoluteExpiration">The policy.</param>
        /// <param name="regionName">(Optional) name of the region.</param>
        /// <returns>A TValue.</returns>
        public static TValue AddOrGetExisting<TValue>(this MemoryCache cache, string key, Func<string, TValue> valueFactory, DateTimeOffset absoluteExpiration, string regionName = null)
        {
            var lazy = new Lazy<TValue>(() => valueFactory(key));

            var item = (Lazy<TValue>)cache.AddOrGetExisting(key, lazy, absoluteExpiration, regionName) ?? lazy;

            return item.Value;
        }

        #endregion AddOrGetExisting
    }
}