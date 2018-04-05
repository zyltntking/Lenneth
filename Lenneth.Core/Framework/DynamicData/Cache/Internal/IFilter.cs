﻿using System;
using System.Collections.Generic;

namespace Lenneth.Core.Framework.DynamicData.Cache.Internal
{
    internal interface IFilter<TObject, TKey>
    {
        Func<TObject, bool> Filter { get; }
        IChangeSet<TObject, TKey> Refresh(IEnumerable<KeyValuePair<TKey, TObject>> items);
        IChangeSet<TObject, TKey> Update(IChangeSet<TObject, TKey> updates);
    }
}
