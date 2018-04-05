﻿using Lenneth.Core.Framework.DynamicData.Operators;

namespace Lenneth.Core.Framework.DynamicData
{
    /// <summary>
    /// A paged update collection
    /// </summary>
    /// <typeparam name="TObject">The type of the object.</typeparam>
    /// <typeparam name="TKey">The type of the key.</typeparam>
    public interface IPagedChangeSet<TObject, TKey> : ISortedChangeSet<TObject, TKey>
    {
        /// <summary>
        /// The paramaters used to virtualise the stream
        /// </summary>
        IPageResponse Response { get; }
    }
}
