using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;

namespace Lenneth.Core.Extensions
{
    [DebuggerStepThrough]
    public static class ListExtensions
    {
        public static int IndexOf<T>(this IList<T> aList, T aElement)
        {
            for (var i = 0; i < aList.Count; i++)
            {
                if (aElement.Equals(aList[i]))
                    return i;
            }

            return -1;
        }

        public static int IndexOf<T>(this IList<T> aList, T aElement, IEqualityComparer<T> aComparer)
        {
            for (var i = 0; i < aList.Count; i++)
            {
                if (aComparer.Equals(aList[i], aElement))
                    return i;
            }

            return -1;
        }

        public static T RemoveLast<T>(this IList<T> aList)
        {
            var last = aList.Last();
            aList.RemoveAt(aList.Count - 1);
            return last;
        }

        public static T RemoveFirst<T>(this IList<T> aList)
        {
            var first = aList.First();
            aList.RemoveAt(0);
            return first;
        }

        public static T Last<T>(this IList<T> aList)
        {
            return aList[aList.Count - 1];
        }

        public static void RemoveRange<T>(this IList<T> aList, IEnumerable<T> aElements)
        {
            foreach (var ele in aElements)
                aList.Remove(ele);
        }

        public static int GetHashCode<T>(IEnumerable<T> aList)
        {
            return aList.Aggregate(0, (current, el) => current ^ el.GetHashCode());
        }

        public static bool Replace<T>(this IList<T> aList, T aOld, T aNew)
        {
            var index = aList.IndexOf(aOld);

            if (index == -1)
                return false;

            aList[index] = aNew;
            return true;
        }
    }
}