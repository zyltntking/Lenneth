using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;

namespace Lenneth.Core.Extensions
{
    [DebuggerStepThrough]
    public static class EnumerableExtensions
    {
        public static void ForEachWithIndex<T>(this IEnumerable<T> aEnumerable,
            Action<T, int> aHandler)
        {
            var index = 0;
            foreach (var item in aEnumerable)
                aHandler(item, index++);
        }

        public static void ForEach<T>(this IEnumerable<T> aEnumerable, Action<T> aHandler)
        {
            foreach (var item in aEnumerable)
                aHandler(item);
        }

        public static int IndexOf<T>(this IEnumerable<T> aEnum, T aEl)
        {
            var i = 0;

            foreach (var el in aEnum)
            {
                if (aEl.Equals(el))
                    return i;
                i++;
            }

            return -1;
        }

        public static bool Unique<T>(this IEnumerable<T> aEnum)
        {
            return aEnum.Distinct().Count() == aEnum.Count();
        }

        public static IEnumerable<T> Except<T>(this IEnumerable<T> aEnumerable, T aElement)
        {
            foreach (var ele in aEnumerable)
            {
                if (!ele.Equals(aElement))
                    yield return ele;
            }
        }

        public static IEnumerable<T> Except<T>(this IEnumerable<T> aEnumerable, T aElement,
            IEqualityComparer<T> aComparer)
        {
            foreach (var ele in aEnumerable)
            {
                if (!aComparer.Equals(ele, aElement))
                    yield return ele;
            }
        }

        public static IEnumerable<T> ExceptExact<T>(this IEnumerable<T> aEnumerable,
            IEnumerable<T> aValues)
        {
            if (aValues.FirstOrDefault() == null)
                return aEnumerable;

            var list = new List<T>(aEnumerable);

            foreach (var ele in aValues)
            {
                var index = list.IndexOf(ele);
                if (index != -1)
                    list.RemoveAt(index);
            }

            return list;
        }

        public static IEnumerable<T> Except<T>(this IEnumerable<T> aEnumerable,
                                               Predicate<T> aPredicate)
        {
            foreach (var ele in aEnumerable)
            {
                if (!aPredicate(ele))
                    yield return ele;
            }
        }

        public static bool ContainsAny<T>(this IEnumerable<T> aEnumerable, IEnumerable<T> aValues)
        {
            return aEnumerable.Intersect(aValues).Any();
        }

        public static bool ContainsAny<T>(this IEnumerable<T> aEnumerable,
            IEnumerable<T> aValues, IEqualityComparer<T> aComparer)
        {
            return aEnumerable.Intersect(aValues, aComparer).Any();
        }

        public static bool Contains<T>(this IEnumerable<T> aEnumerable, IEnumerable<T> aValues)
        {
            if (aValues.FirstOrDefault() == null)
                return false;

            foreach (var ele in aValues)
            {
                if (!aEnumerable.Contains(ele))
                    return false;
            }

            return true;
        }

        public static bool Contains<T>(this IEnumerable<T> aEnumerable, IEnumerable<T> aValues,
            IEqualityComparer<T> aComparer)
        {
            if (aValues.FirstOrDefault() == null)
                return false;

            foreach (var ele in aValues)
            {
                if (!aEnumerable.Contains(ele, aComparer))
                    return false;
            }

            return true;
        }

        public static bool Exact<T>(this IEnumerable<T> aEnumerable, IEnumerable<T> aValues)
        {
            var list = new List<T>(aValues);

            var initCount = list.Count;
            var count = 0;

            foreach (var ele in aEnumerable)
            {
                count++;

                if (count > initCount)
                    return false;

                var index = list.IndexOf(ele);
                if (index == -1)
                    return false;
                else
                    list.RemoveAt(index);
            }

            return count == initCount;
        }

        public static bool Exact<T>(this IEnumerable<T> aEnumerable, IEnumerable<T> aValues,
            IEqualityComparer<T> aComparer)
        {
            var list = new List<T>(aValues);

            var initCount = list.Count;
            var count = 0;

            foreach (var ele in aEnumerable)
            {
                count++;

                if (count > initCount)
                    return false;

                var index = list.IndexOf(ele, aComparer);
                if (index == -1)
                    return false;
                else
                    list.RemoveAt(index);
            }

            return count == initCount;
        }

        public static IEnumerable<T> Substract<T>(this IEnumerable<T> aEnumerable,
            IEnumerable<T> aValues)
        {
            var list = new List<T>(aValues);

            foreach (var ele in aEnumerable)
            {
                var index = list.IndexOf(ele);
                if (index != -1)
                    list.RemoveAt(index);
                else
                    yield return ele;
            }
        }

        public static IEnumerable<T> Substract<T>(this IEnumerable<T> aEnumerable,
            IEnumerable<T> aValues, IEqualityComparer<T> aComparer)
        {
            var list = new List<T>(aValues);

            foreach (var ele in aEnumerable)
            {
                var index = list.IndexOf(ele, aComparer);
                if (index != -1)
                    list.RemoveAt(index);
                else
                    yield return ele;
            }
        }

        public static bool ContainsExact<T>(this IEnumerable<T> aEnumerable,
            IEnumerable<T> aValues)
        {
            if (aValues.FirstOrDefault() == null)
                return false;

            var list = new List<T>(aEnumerable);

            foreach (var ele in aValues)
            {
                var index = list.IndexOf(ele);
                if (index == -1)
                    return false;
                else
                    list.RemoveAt(index);
            }

            return true;
        }

        public static bool ContainsExact<T>(this IEnumerable<T> aEnumerable,
            IEnumerable<T> aValues, IEqualityComparer<T> aComparer)
        {
            var list = new List<T>(aEnumerable);

            if (aValues.FirstOrDefault() == null)
                return false;

            foreach (var ele in aValues)
            {
                var index = list.IndexOf(ele, aComparer);
                if (index == -1)
                    return false;
                else
                    list.RemoveAt(index);
            }

            return true;
        }

        public static IEnumerable<T> Repeat<T>(this IEnumerable<T> aEnum, int aTimes = 1)
        {
            if (aTimes == 0)
                yield break;

            for (var i = 0; i < aTimes; i++)
            {
                foreach (var el in aEnum)
                    yield return el;
            }
        }

        public static IEnumerable<T> SkipLast<T>(this IEnumerable<T> aEnum, int aSkip = 1)
        {
            return aEnum.Reverse().Skip(aSkip).Reverse();
        }

        public static IEnumerable<T> SelectMany<T>(this IEnumerable<IEnumerable<T>> aEnum)
        {
            return aEnum.SelectMany(obj => obj);
        }

        public static IEnumerable<T> Take<T>(this IEnumerable<T> aEnumerable, int aFrom,
            int aCount)
        {
            return aEnumerable.Skip(aFrom).Take(aCount);
        }

        public static IEnumerable<T> TakeAllOrOne<T>(this IEnumerable<T> aEnum, bool aAll,
            Func<T, bool> aTake)
        {
            var first = false;

            foreach (var el in aEnum)
            {
                if (first && !aAll)
                    yield break;

                if (!aTake(el))
                    continue;

                first = true;
                yield return el;
            }
        }
    }
}