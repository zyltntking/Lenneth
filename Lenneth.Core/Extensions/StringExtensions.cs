using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Globalization;
using System.Linq;
using System.Text;

namespace Lenneth.Core.Extensions
{
    [DebuggerStepThrough]
    public static class StringExtensions
    {
        public static double ToDouble(this string aStr)
        {
            if (aStr.ToLower() == double.NegativeInfinity.ToString().ToLower())
                return double.NegativeInfinity;
            if (aStr.ToLower() == double.PositiveInfinity.ToString().ToLower())
                return double.PositiveInfinity;
            if (aStr.ToLower() == double.NaN.ToString().ToLower())
                return double.NaN;
            else
                return double.Parse(aStr, CultureInfo.InvariantCulture);
        }

        public static float ToSingle(this string aStr)
        {
            if (aStr.ToLower() == float.NegativeInfinity.ToString().ToLower())
                return float.NegativeInfinity;
            if (aStr.ToLower() == float.PositiveInfinity.ToString().ToLower())
                return float.PositiveInfinity;
            if (aStr.ToLower() == float.NaN.ToString().ToLower())
                return float.NaN;
            else
                return float.Parse(aStr, CultureInfo.InvariantCulture);
        }

        public static int ToInt(this string aStr)
        {
            return int.Parse(aStr);
        }

        public static bool ToBool(this string aStr)
        {
            return bool.Parse(aStr);
        }

        public static string RemoveFromRight(this string aStr, int aChars)
        {
            return aStr.Remove(aStr.Length - aChars);
        }

        public static string RemoveFromLeft(this string aStr, int aChars)
        {
            return aStr.Remove(0, aChars);
        }

        public static string Left(this string aStr, int aCount)
        {
            return aStr.Substring(0, aCount);
        }

        public static string Right(this string aStr, int aCount)
        {
            return aStr.Substring(aStr.Length - aCount, aCount);
        }

        public static string EnsureStartsWith(this string aStr, string aPrefix)
        {
            return aStr.StartsWith(aPrefix) ? aStr : string.Concat(aPrefix, aStr);
        }

        public static string Repeat(this string aStr, int aCount)
        {
            var sb = new StringBuilder(aStr.Length * aCount);

            for (var i = 0; i < aCount; i++)
                sb.Append(aStr);

            return sb.ToString();
        }

        public static string GetBefore(this string aStr, string aPattern)
        {
            var index = aStr.IndexOf(aPattern);
            return (index == -1) ? string.Empty : aStr.Substring(0, index);
        }

        public static string GetAfter(this string aStr, string aPattern)
        {
            var lastPos = aStr.LastIndexOf(aPattern);

            if (lastPos == -1)
                return string.Empty;

            var start = lastPos + aPattern.Length;
            return start >= aStr.Length ? string.Empty : aStr.Substring(start).Trim();
        }

        public static string GetBetween(this string aStr, string aLeft, string aRight)
        {
            return aStr.GetBefore(aRight).GetAfter(aLeft);
        }

        public static string ToTitleCase(this string value)
        {
            return System.Globalization.CultureInfo.CurrentUICulture.TextInfo.ToTitleCase(value);
        }

        public static string FindUniqueName(this string aPattern,
            IEnumerable<string> aNames)
        {
            if (!aNames.Contains(aPattern))
                return aPattern;

            var ar = aPattern.Split(' ');

            string left;
            uint index;
            if (!uint.TryParse(ar.Last(), out index))
            {
                index = 1;
                left = aPattern + " ";
            }
            else
            {
                left = string.Join(" ", ar.SkipLast(1)) + " ";
                index++;
            }

            for (; ; )
            {
                var result = (left + index.ToString()).Trim();

                if (aNames.Contains(result))
                {
                    index++;
                    continue;
                }

                return result;
            }
        }

        public static IEnumerable<string> Split(this string aStr, string aSplit)
        {
            var startIndex = 0;

            for (; ; )
            {
                var splitIndex = aStr.IndexOf(aSplit, startIndex);

                if (splitIndex == -1)
                    break;

                yield return aStr.Substring(startIndex, splitIndex - startIndex);

                startIndex = splitIndex + aSplit.Length;
            }

            yield return aStr.Substring(startIndex);
        }
    }
}