using System;
using System.Collections.Generic;

namespace Lenneth.Core.Framework.DynamicData.Kernel
{
    /// <summary>
    /// Container for an item and it's index from a list
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public struct ItemWithIndex<T> : IEquatable<ItemWithIndex<T>>
    {
        /// <summary>
        /// Gets the item.
        /// </summary>
        public T Item { get; }

        /// <summary>
        /// Gets the index.
        /// </summary>
        public int Index { get; }

        /// <summary>
        /// Initializes a new instance of the <see cref="ItemWithIndex{T}"/> class.
        /// </summary>
        /// <param name="item">The item.</param>
        /// <param name="index">The index.</param>
        public ItemWithIndex(T item, int index)
        {
            Item = item;
            Index = index;
        }

        #region Equality 

        /// <inheritdoc />
        public bool Equals(ItemWithIndex<T> other)
        {
            return EqualityComparer<T>.Default.Equals(Item, other.Item);
        }

        /// <inheritdoc />
        public override bool Equals(object obj)
        {
            if (ReferenceEquals(null, obj)) return false;
            return obj is ItemWithIndex<T> && Equals((ItemWithIndex<T>) obj);
        }

        /// <summary>Returns the hash code for this instance.</summary>
        /// <returns>A 32-bit signed integer that is the hash code for this instance.</returns>
        public override int GetHashCode()
        {
            return EqualityComparer<T>.Default.GetHashCode(Item);
        }

        /// <summary>Returns a value that indicates whether the values of two <see cref="T:Lenneth.Core.Framework.DynamicData.Kernel.ItemWithIndex`1" /> objects are equal.</summary>
        /// <param name="left">The first value to compare.</param>
        /// <param name="right">The second value to compare.</param>
        /// <returns>true if the <paramref name="left" /> and <paramref name="right" /> parameters have the same value; otherwise, false.</returns>
        public static bool operator ==(ItemWithIndex<T> left, ItemWithIndex<T> right)
        {
            return left.Equals(right);
        }

        /// <summary>Returns a value that indicates whether two <see cref="T:Lenneth.Core.Framework.DynamicData.Kernel.ItemWithIndex`1" /> objects have different values.</summary>
        /// <param name="left">The first value to compare.</param>
        /// <param name="right">The second value to compare.</param>
        /// <returns>true if <paramref name="left" /> and <paramref name="right" /> are not equal; otherwise, false.</returns>
        public static bool operator !=(ItemWithIndex<T> left, ItemWithIndex<T> right)
        {
            return !left.Equals(right);
        }

        #endregion


        /// <inheritdoc />
        public override string ToString()
        {
            return $"{Item} ({Index})";
        }
    }
}
