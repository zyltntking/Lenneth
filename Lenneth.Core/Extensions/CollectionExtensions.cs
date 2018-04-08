using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;

namespace Lenneth.Core.Extensions
{
    [DebuggerStepThrough]
    public static class CollectionExtensions
    {
        public static void RemoveRange<T>(this ICollection<T> aCollection, IEnumerable<T> aElements)
        {
            foreach (var ele in aElements)
                aCollection.Remove(ele);
        }

        public static void RemoveAll<T>(this ICollection<T> aCollection, Predicate<T> aPredicate)
        {
            var deleteList = aCollection.Where(child => aPredicate(child)).ToList();
            deleteList.ForEach(t => aCollection.Remove(t));
        }

        public static bool AddUnique<T>(this ICollection<T> aCollection, T aValue)
        {
            if (aCollection.Contains(aValue) == false)
            {
                aCollection.Add(aValue);
                return true;
            }
            return false;
        }

        public static int AddRangeUnique<T>(this ICollection<T> aCollection, IEnumerable<T> aValues)
        {
            var count = 0;
            foreach (var value in aValues)
            {
                if (aCollection.AddUnique(value))
                    count++;
            }
            return count;
        }
    }
}