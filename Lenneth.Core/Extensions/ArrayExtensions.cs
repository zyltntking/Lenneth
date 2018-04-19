using System;
using System.Diagnostics;
using System.Linq;
using System.Collections.Generic;
using Lenneth.Core.Extensions.Utils;

namespace Lenneth.Core.Extensions
{
    [DebuggerStepThrough]
    public static class ArrayExtensions
    {
        /// <summary>
        /// /// True if array are exactly the same.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="aAr1"></param>
        /// <param name="aAr2"></param>
        /// <returns></returns>
        public static bool AreSame<T>(this T[] aAr1, T[] aAr2)
        {
            if (ReferenceEquals(aAr1, aAr2))
                return true;

            if (aAr1.Length != aAr2.Length)
                return false;

            return !aAr1.Where((t, i) => !t.Equals(aAr2[i])).Any();
        }

        /// <summary>
        /// /// True if array are exactly the same.
        /// </summary>
        /// <param name="aAr1"></param>
        /// <param name="aAr2"></param>
        /// <returns></returns>
        public static bool AreSame(this byte[] aAr1, byte[] aAr2)
        {
            if (ReferenceEquals(aAr1, aAr2))
                return true;

            if (aAr1.Length != aAr2.Length)
                return false;

            return !aAr1.Where((t, i) => t != aAr2[i]).Any();
        }

        /// <summary>
        /// True if array are exactly the same.
        /// </summary>
        /// <param name="aAr1"></param>
        /// <param name="aAr2"></param>
        /// <returns></returns>
        public static bool AreSame(this byte[,] aAr1, byte[,] aAr2)
        {
            if (ReferenceEquals(aAr1, aAr2))
                return true;

            if (aAr1.GetLength(0) != aAr2.GetLength(1))
                return false;

            for (var x = 0; x < aAr1.GetLength(0); x++)
            {
                for (var y = 0; y < aAr1.GetLength(1); y++)
                {
                    if (aAr1[x, y] != aAr2[x, y])
                        return false;
                }
            }

            return true;
        }

        /// <summary>
        /// True if array are exactly the same.
        /// </summary>
        /// <param name="aAr1"></param>
        /// <param name="aAr2"></param>
        /// <returns></returns>
        public static bool AreSame(this ushort[] aAr1, ushort[] aAr2)
        {
            if (ReferenceEquals(aAr1, aAr2))
                return true;

            if (aAr1.Length != aAr2.Length)
                return false;

            return !aAr1.Where((t, i) => t != aAr2[i]).Any();
        }

        /// <summary>
        /// Return hash code for array. Result is xor sum of elements GetHashCode() functions.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="aAr"></param>
        /// <returns></returns>
        public static int GetHashCode<T>(IEnumerable<T> aAr)
        {
            return aAr.Aggregate(0, (current, t) => current ^ t.GetHashCode());
        }

        /// <summary>
        /// Check that this is valid index.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="aArray"></param>
        /// <param name="aIndex"></param>
        /// <returns></returns>
        public static bool InRange<T>(this T[] aArray, int aIndex)
        {
            return (aIndex >= aArray.GetLowerBound(0)) && (aIndex <= aArray.GetUpperBound(0));
        }

        /// <summary>
        /// Clear array with zeroes.
        /// </summary>
        /// <param name="aArray"></param>
        /// <param name="aValue"></param>
        public static void Clear<T>(this T[] aArray, T aValue = default(T))
        {
            for (var i = 0; i < aArray.Length; i++)
                aArray[i] = aValue;
        }

        /// <summary>
        /// Clear array with zeroes.
        /// </summary>
        /// <param name="aArray"></param>
        /// <param name="aValue"></param>
        public static void Clear<T>(this T[,] aArray, T aValue = default(T))
        {
            for (var x = 0; x < aArray.GetLength(0); x++)
            {
                for (var y = 0; y < aArray.GetLength(1); y++)
                {
                    aArray[x, y] = aValue;
                }
            }
        }

        /// <summary>
        /// Return array stated from a_index and with a_count legth.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="aArray"></param>
        /// <param name="aIndex"></param>
        /// <param name="aCount"></param>
        /// <returns></returns>
        public static T[] SubArray<T>(this T[] aArray, int aIndex, int aCount = -1)
        {
            if (aCount == -1)
                aCount = aArray.Length - aIndex;

            var result = new T[aCount];
            Array.Copy(aArray, aIndex, result, 0, aCount);
            return result;
        }

        /// <summary>
        /// Find index of a_element within a_array.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="aArray"></param>
        /// <param name="aElement"></param>
        /// <returns>
        /// Index of element or -1 if not find.
        /// </returns>
        public static int IndexOf<T>(this T[] aArray, T aElement)
        {
            for (var i = 0; i < aArray.Length; i++)
                if (ReferenceEquals(aElement, aArray[i]))
                    return i;
            return -1;
        }

        /// <summary>
        /// Return first occurence of a_sun_array in a_array.
        /// </summary>
        /// <param name="aArray"></param>
        /// <param name="aSubArray"></param>
        /// <returns></returns>
        public static int FindArrayInArray(this byte[] aArray, byte[] aSubArray)
        {
            int j;

            for (j = 0; j < aArray.Length - aSubArray.Length; j++)
            {
                int i;
                for (i = 0; i < aSubArray.Length; i++)
                {
                    if (aArray[j + i] != aSubArray[i])
                        break;
                }

                if (i == aSubArray.Length)
                    return j;
            }

            return -1;
        }

        public static T[] Shuffle<T>(this T[] aArray)
        {
            return Shuffle(aArray, Environment.TickCount);
        }

        public static T[] Shuffle<T>(this T[] aArray, int aSeed)
        {
            var mt = new MersenneTwister((uint)aSeed);

            return (from gr in
                        from el in aArray
                        select new { index = mt.NextInt(), el }
                    orderby gr.index
                    select gr.el).ToArray();
        }

        public static void Fill<T>(this T[,] aAr, T aValue)
        {
            for (var x = 0; x < aAr.GetLength(0); x++)
            {
                for (var y = 0; y < aAr.GetLength(1); y++)
                {
                    aAr[x, y] = aValue;
                }
            }
        }

        public static void Fill<T>(this T[] aAr, T aValue)
        {
            for (var i = 0; i < aAr.GetLength(0); i++)
                aAr[i] = aValue;
        }

        public static IEnumerable<T> ToEnumerable<T>(this T[,] aAr)
        {
            return aAr.Cast<T>();
        }
    }
}