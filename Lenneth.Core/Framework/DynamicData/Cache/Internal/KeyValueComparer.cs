﻿using System.Collections.Generic;

namespace Lenneth.Core.Framework.DynamicData.Cache.Internal
{
    internal class KeyValueComparer<TObject, TKey> : IComparer<KeyValuePair<TKey, TObject>>
    {
        private readonly IComparer<TObject> _comparer;

        public KeyValueComparer(IComparer<TObject> comparer = null)
        {
            _comparer = comparer;
        }

        public int Compare(KeyValuePair<TKey, TObject> x, KeyValuePair<TKey, TObject> y)
        {
            if (_comparer != null)
            {
                var result = _comparer.Compare(x.Value, y.Value);

                if (result != 0)
                    return result;
            }

            return x.Key.GetHashCode().CompareTo(y.Key.GetHashCode());
        }
    }
}
