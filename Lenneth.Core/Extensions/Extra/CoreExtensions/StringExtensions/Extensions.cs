using System;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;

namespace Lenneth.Core.Extensions.Extra.CoreExtensions.StringExtensions
{
    public static class Extensions
    {
        #region Object

        #region In

        /// <summary>
        /// 判断字符串是否在数组中
        /// </summary>
        /// <param name="this">The object to be compared.</param>
        /// <param name="values">The value list to compare with the object.</param>
        /// <returns>true if the values list contains the object, else false.</returns>
        public static bool In(this string @this, params string[] values)
        {
            return Array.IndexOf(values, @this) != -1;
        }

        #endregion In

        #region IsNotNull

        /// <summary>
        /// 判断字符串是否不为null
        /// </summary>
        /// <param name="this">The @this to act on.</param>
        /// <returns>true if not null, false if not.</returns>
        public static bool IsNotNull(this string @this)
        {
            return @this != null;
        }

        #endregion IsNotNull

        #region IsNull

        /// <summary>
        /// 判断字符串是否为null
        /// </summary>
        /// <param name="this">The @this to act on.</param>
        /// <returns>true if null, false if not.</returns>
        public static bool IsNull(this string @this)
        {
            return @this == null;
        }

        #endregion IsNull

        #region NotIn

        /// <summary>
        /// 判断字符串是否不在数组中
        /// </summary>
        /// <param name="this">The object to be compared.</param>
        /// <param name="values">The value list to compare with the object.</param>
        /// <returns>true if the values list doesn't contains the object, else false.</returns>
        public static bool NotIn(this string @this, params string[] values)
        {
            return Array.IndexOf(values, @this) == -1;
        }

        #endregion NotIn

        #endregion Object

        #region ExtractValueType

        #region ExtractDecimal

        /// <summary>
        /// 提取decimal
        /// </summary>
        /// <param name="this">The @this to act on.</param>
        /// <returns>The extracted Decimal.</returns>
        public static decimal ExtractDecimal(this string @this)
        {
            var sb = new StringBuilder();
            for (var i = 0; i < @this.Length; i++)
            {
                if (char.IsDigit(@this[i]) || @this[i] == '.')
                {
                    if (sb.Length == 0 && i > 0 && @this[i - 1] == '-')
                    {
                        sb.Append('-');
                    }
                    sb.Append(@this[i]);
                }
            }

            return Convert.ToDecimal(sb.ToString());
        }

        #endregion ExtractDecimal

        #region ExtractDouble

        /// <summary>
        /// 提取double
        /// </summary>
        /// <param name="this">The @this to act on.</param>
        /// <returns>The extracted Double.</returns>
        public static double ExtractDouble(this string @this)
        {
            var sb = new StringBuilder();
            for (var i = 0; i < @this.Length; i++)
            {
                if (char.IsDigit(@this[i]) || @this[i] == '.')
                {
                    if (sb.Length == 0 && i > 0 && @this[i - 1] == '-')
                    {
                        sb.Append('-');
                    }
                    sb.Append(@this[i]);
                }
            }

            return Convert.ToDouble(sb.ToString());
        }

        #endregion ExtractDouble

        #region ExtractInt16

        /// <summary>
        /// 提取int16
        /// </summary>
        /// <param name="this">The @this to act on.</param>
        /// <returns>The extracted Int16.</returns>
        public static short ExtractInt16(this string @this)
        {
            var sb = new StringBuilder();
            for (var i = 0; i < @this.Length; i++)
            {
                if (char.IsDigit(@this[i]))
                {
                    if (sb.Length == 0 && i > 0 && @this[i - 1] == '-')
                    {
                        sb.Append('-');
                    }
                    sb.Append(@this[i]);
                }
            }

            return Convert.ToInt16(sb.ToString());
        }

        #endregion ExtractInt16

        #region ExtractInt32

        /// <summary>
        /// 提取int32
        /// </summary>
        /// <param name="this">The @this to act on.</param>
        /// <returns>The extracted Int32.</returns>
        public static int ExtractInt32(this string @this)
        {
            var sb = new StringBuilder();
            for (var i = 0; i < @this.Length; i++)
            {
                if (char.IsDigit(@this[i]))
                {
                    if (sb.Length == 0 && i > 0 && @this[i - 1] == '-')
                    {
                        sb.Append('-');
                    }
                    sb.Append(@this[i]);
                }
            }

            return Convert.ToInt32(sb.ToString());
        }

        #endregion ExtractInt32

        #region ExtractInt64

        /// <summary>
        /// 提取int64
        /// </summary>
        /// <param name="this">The @this to act on.</param>
        /// <returns>The extracted Int64.</returns>
        public static long ExtractInt64(this string @this)
        {
            var sb = new StringBuilder();
            for (var i = 0; i < @this.Length; i++)
            {
                if (char.IsDigit(@this[i]))
                {
                    if (sb.Length == 0 && i > 0 && @this[i - 1] == '-')
                    {
                        sb.Append('-');
                    }
                    sb.Append(@this[i]);
                }
            }

            return Convert.ToInt64(sb.ToString());
        }

        #endregion ExtractInt64

        #region ExtractManyDecimal

        /// <summary>
        /// 提取decimal数组
        /// </summary>
        /// <param name="this">The @this to act on.</param>
        /// <returns>All extracted Decimal.</returns>
        public static decimal[] ExtractManyDecimal(this string @this)
        {
            return Regex.Matches(@this, @"[-]?\d+(\.\d+)?")
                .Cast<Match>()
                .Select(x => Convert.ToDecimal(x.Value))
                .ToArray();
        }

        #endregion ExtractManyDecimal

        #region ExtractManyDouble

        /// <summary>
        /// 提取double数组
        /// </summary>
        /// <param name="this">The @this to act on.</param>
        /// <returns>All extracted Double.</returns>
        public static double[] ExtractManyDouble(this string @this)
        {
            return Regex.Matches(@this, @"[-]?\d+(\.\d+)?")
                .Cast<Match>()
                .Select(x => Convert.ToDouble(x.Value))
                .ToArray();
        }

        #endregion ExtractManyDouble

        #region ExtractManyInt16

        /// <summary>
        /// 提取int16数组
        /// </summary>
        /// <param name="this">The @this to act on.</param>
        /// <returns>All extracted Int16.</returns>
        public static short[] ExtractManyInt16(this string @this)
        {
            return Regex.Matches(@this, @"[-]?\d+")
                .Cast<Match>()
                .Select(x => Convert.ToInt16(x.Value))
                .ToArray();
        }

        #endregion ExtractManyInt16

        #region ExtractManyInt32

        /// <summary>
        /// 提取int32数组
        /// </summary>
        /// <param name="this">The @this to act on.</param>
        /// <returns>All extracted Int32.</returns>
        public static int[] ExtractManyInt32(this string @this)
        {
            return Regex.Matches(@this, @"[-]?\d+")
                .Cast<Match>()
                .Select(x => Convert.ToInt32(x.Value))
                .ToArray();
        }

        #endregion ExtractManyInt32

        #region ExtractManyInt64

        /// <summary>
        /// 提取int64数组
        /// </summary>
        /// <param name="this">The @this to act on.</param>
        /// <returns>All extracted Int64.</returns>
        public static long[] ExtractManyInt64(this string @this)
        {
            return Regex.Matches(@this, @"[-]?\d+")
                .Cast<Match>()
                .Select(x => Convert.ToInt64(x.Value))
                .ToArray();
        }

        #endregion ExtractManyInt64

        #region ExtractManyUInt16

        /// <summary>
        /// 提取uint16数组
        /// </summary>
        /// <param name="this">The @this to act on.</param>
        /// <returns>All extracted UInt16.</returns>
        public static ushort[] ExtractManyUInt16(this string @this)
        {
            return Regex.Matches(@this, @"\d+")
                .Cast<Match>()
                .Select(x => Convert.ToUInt16(x.Value))
                .ToArray();
        }

        #endregion ExtractManyUInt16

        #region ExtractManyUInt32

        /// <summary>
        /// 提取uint32数组
        /// </summary>
        /// <param name="this">The @this to act on.</param>
        /// <returns>All extracted UInt32.</returns>
        public static uint[] ExtractManyUInt32(this string @this)
        {
            return Regex.Matches(@this, @"\d+")
                .Cast<Match>()
                .Select(x => Convert.ToUInt32(x.Value))
                .ToArray();
        }

        #endregion ExtractManyUInt32

        #region ExtractManyUInt64

        /// <summary>
        /// 提取uint64数组
        /// </summary>
        /// <param name="this">The @this to act on.</param>
        /// <returns>All extracted UInt64.</returns>
        public static ulong[] ExtractManyUInt64(this string @this)
        {
            return Regex.Matches(@this, @"\d+")
                .Cast<Match>()
                .Select(x => Convert.ToUInt64(x.Value))
                .ToArray();
        }

        #endregion ExtractManyUInt64

        #region ExtractUInt16

        /// <summary>
        /// 提取uint16
        /// </summary>
        /// <param name="this">The @this to act on.</param>
        /// <returns>The extracted UInt16.</returns>
        public static ushort ExtractUInt16(this string @this)
        {
            var sb = new StringBuilder();
            foreach (var u in @this)
            {
                if (char.IsDigit(u))
                {
                    sb.Append(u);
                }
            }

            return Convert.ToUInt16(sb.ToString());
        }

        #endregion ExtractUInt16

        #region ExtractUInt32

        /// <summary>
        /// 提取uint32
        /// </summary>
        /// <param name="this">The @this to act on.</param>
        /// <returns>The extracted UInt32.</returns>
        public static uint ExtractUInt32(this string @this)
        {
            var sb = new StringBuilder();
            foreach (var u in @this)
            {
                if (char.IsDigit(u))
                {
                    sb.Append(u);
                }
            }

            return Convert.ToUInt32(sb.ToString());
        }

        #endregion ExtractUInt32

        #region ExtractUInt64

        /// <summary>
        /// 提取uint64
        /// </summary>
        /// <param name="this">The @this to act on.</param>
        /// <returns>The extracted UInt64.</returns>
        public static ulong ExtractUInt64(this string @this)
        {
            var sb = new StringBuilder();
            foreach (var u in @this)
            {
                if (char.IsDigit(u))
                {
                    sb.Append(u);
                }
            }

            return Convert.ToUInt64(sb.ToString());
        }

        #endregion ExtractUInt64

        #endregion ExtractValueType

        #region Char

        #region ConvertToUtf32

        /// <summary>
        /// 转换指定位置字符为utf32
        /// </summary>
        /// <param name="s">A string that contains a character or surrogate pair.</param>
        /// <param name="index">The index position of the character or surrogate pair in .</param>
        /// <returns>
        ///     The 21-bit Unicode code point represented by the character or surrogate pair at the position in the parameter
        ///     specified by the  parameter.
        /// </returns>
        public static int ConvertToUtf32(this string s, int index)
        {
            return char.ConvertToUtf32(s, index);
        }

        #endregion ConvertToUtf32

        #region GetNumericValue

        /// <summary>
        /// 转换指定位置字符为numeric
        /// </summary>
        /// <param name="s">A .</param>
        /// <param name="index">The character position in .</param>
        /// <returns>
        ///     The numeric value of the character at position  in  if that character represents a number; otherwise, -1.
        /// </returns>
        public static double GetNumericValue(this string s, int index)
        {
            return char.GetNumericValue(s, index);
        }

        #endregion GetNumericValue

        #region GetUnicodeCategory

        /// <summary>
        /// 把制定字符存入UnicodeCategory
        /// </summary>
        /// <param name="s">A .</param>
        /// <param name="index">The character position in .</param>
        /// <returns>A  enumerated constant that identifies the group that contains the character at position  in .</returns>
        public static UnicodeCategory GetUnicodeCategory(this string s, int index)
        {
            return char.GetUnicodeCategory(s, index);
        }

        #endregion GetUnicodeCategory

        #region IsControl

        /// <summary>
        /// 指定字符是否为控制字符
        /// </summary>
        /// <param name="s">A string.</param>
        /// <param name="index">The position of the character to evaluate in .</param>
        /// <returns>true if the character at position  in  is a control character; otherwise, false.</returns>
        public static bool IsControl(this string s, int index)
        {
            return char.IsControl(s, index);
        }

        #endregion IsControl

        #region IsDigit

        /// <summary>
        /// 指定字符是否digit
        /// </summary>
        /// <param name="s">A string.</param>
        /// <param name="index">The position of the character to evaluate in .</param>
        /// <returns>true if the character at position  in  is a decimal digit; otherwise, false.</returns>
        public static bool IsDigit(this string s, int index)
        {
            return char.IsDigit(s, index);
        }

        #endregion IsDigit

        #region IsHighSurrogate

        /// <summary>
        /// 指定字符是否为高代理
        /// </summary>
        /// <param name="s">A string.</param>
        /// <param name="index">The position of the character to evaluate in .</param>
        /// <returns>
        ///     true if the numeric value of the specified character in the  parameter ranges from U+D800 through U+DBFF;
        ///     otherwise, false.
        /// </returns>
        public static bool IsHighSurrogate(this string s, int index)
        {
            return char.IsHighSurrogate(s, index);
        }

        #endregion IsHighSurrogate

        #region IsLetter

        /// <summary>
        /// 指定字符是否为字母
        /// </summary>
        /// <param name="s">A string.</param>
        /// <param name="index">The position of the character to evaluate in .</param>
        /// <returns>true if the character at position  in  is a letter; otherwise, false.</returns>
        public static bool IsLetter(this string s, int index)
        {
            return char.IsLetter(s, index);
        }

        #endregion IsLetter

        #region IsLetterOrDigit

        /// <summary>
        /// 指定字符是否为字母或digit
        /// </summary>
        /// <param name="s">A string.</param>
        /// <param name="index">The position of the character to evaluate in .</param>
        /// <returns>true if the character at position  in  is a letter or a decimal digit; otherwise, false.</returns>
        public static bool IsLetterOrDigit(this string s, int index)
        {
            return char.IsLetterOrDigit(s, index);
        }

        #endregion IsLetterOrDigit

        #region IsLower

        /// <summary>
        /// 指定字符是否小写
        /// </summary>
        /// <param name="s">A string.</param>
        /// <param name="index">The position of the character to evaluate in .</param>
        /// <returns>true if the character at position  in  is a lowercase letter; otherwise, false.</returns>
        public static bool IsLower(this string s, int index)
        {
            return char.IsLower(s, index);
        }

        #endregion IsLower

        #region IsLowSurrogate

        /// <summary>
        /// 指定字符是否低代理
        /// </summary>
        /// <param name="s">A string.</param>
        /// <param name="index">The position of the character to evaluate in .</param>
        /// <returns>
        ///     true if the numeric value of the specified character in the  parameter ranges from U+DC00 through U+DFFF;
        ///     otherwise, false.
        /// </returns>
        public static bool IsLowSurrogate(this string s, int index)
        {
            return char.IsLowSurrogate(s, index);
        }

        #endregion IsLowSurrogate

        #region IsNumber

        /// <summary>
        /// 指定字符是否数字
        /// </summary>
        /// <param name="s">A string.</param>
        /// <param name="index">The position of the character to evaluate in .</param>
        /// <returns>true if the character at position  in  is a number; otherwise, false.</returns>
        public static bool IsNumber(this string s, int index)
        {
            return char.IsNumber(s, index);
        }

        #endregion IsNumber

        #region IsPunctuation

        /// <summary>
        /// 指定字符是否标点符号
        /// </summary>
        /// <param name="s">A string.</param>
        /// <param name="index">The position of the character to evaluate in .</param>
        /// <returns>true if the character at position  in  is a punctuation mark; otherwise, false.</returns>
        public static bool IsPunctuation(this string s, int index)
        {
            return char.IsPunctuation(s, index);
        }

        #endregion IsPunctuation

        #region IsSeparator

        /// <summary>
        /// 指定字符是否为分隔符
        /// </summary>
        /// <param name="s">A string.</param>
        /// <param name="index">The position of the character to evaluate in .</param>
        /// <returns>true if the character at position  in  is a separator character; otherwise, false.</returns>
        public static bool IsSeparator(this string s, int index)
        {
            return char.IsSeparator(s, index);
        }

        #endregion IsSeparator

        #region IsSurrogate

        /// <summary>
        /// 指定字符是否具有代理项代码单位
        /// </summary>
        /// <param name="s">A string.</param>
        /// <param name="index">The position of the character to evaluate in .</param>
        /// <returns>
        ///     true if the character at position  in  is a either a high surrogate or a low surrogate; otherwise, false.
        /// </returns>
        public static bool IsSurrogate(this string s, int index)
        {
            return char.IsSurrogate(s, index);
        }

        #endregion IsSurrogate

        #region IsSurrogatePair

        /// <summary>
        /// 指定字符相邻两位是否形成代理代码对
        /// </summary>
        /// <param name="s">A string.</param>
        /// <param name="index">The starting position of the pair of characters to evaluate within .</param>
        /// <returns>
        ///     true if the  parameter includes adjacent characters at positions  and  + 1, and the numeric value of the
        ///     character at position  ranges from U+D800 through U+DBFF, and the numeric value of the character at position
        ///     +1 ranges from U+DC00 through U+DFFF; otherwise, false.
        /// </returns>
        public static bool IsSurrogatePair(this string s, int index)
        {
            return char.IsSurrogatePair(s, index);
        }

        #endregion IsSurrogatePair

        #region IsSymbol

        /// <summary>
        /// 指定字符是否符号
        /// </summary>
        /// <param name="s">A string.</param>
        /// <param name="index">The position of the character to evaluate in .</param>
        /// <returns>true if the character at position  in  is a symbol character; otherwise, false.</returns>
        public static bool IsSymbol(this string s, int index)
        {
            return char.IsSymbol(s, index);
        }

        #endregion IsSymbol

        #region IsUpper

        /// <summary>
        /// 指定字符是否大写
        /// </summary>
        /// <param name="s">A string.</param>
        /// <param name="index">The position of the character to evaluate in .</param>
        /// <returns>true if the character at position  in  is an uppercase letter; otherwise, false.</returns>
        public static bool IsUpper(this string s, int index)
        {
            return char.IsUpper(s, index);
        }

        #endregion IsUpper

        #region IsWhiteSpace

        /// <summary>
        /// 指定字符是否空格
        /// </summary>
        /// <param name="s">A string.</param>
        /// <param name="index">The position of the character to evaluate in .</param>
        /// <returns>true if the character at position  in  is white space; otherwise, false.</returns>
        public static bool IsWhiteSpace(this string s, int index)
        {
            return char.IsWhiteSpace(s, index);
        }

        #endregion IsWhiteSpace

        #endregion Char

        #region String

        #region CompareOrdinal

        /// <summary>
        ///     Compares two specified  objects by evaluating the numeric values of the corresponding  objects in each string.
        /// </summary>
        /// <param name="strA">The first string to compare.</param>
        /// <param name="strB">The second string to compare.</param>
        /// <returns>
        ///     An integer that indicates the lexical relationship between the two comparands.ValueCondition Less than zero
        ///     is less than . Zero  and  are equal. Greater than zero  is greater than .
        /// </returns>
        public static Int32 CompareOrdinal(this String strA, String strB)
        {
            return String.CompareOrdinal(strA, strB);
        }

        /// <summary>
        ///     Compares substrings of two specified  objects by evaluating the numeric values of the corresponding  objects
        ///     in each substring.
        /// </summary>
        /// <param name="strA">The first string to use in the comparison.</param>
        /// <param name="indexA">The starting index of the substring in .</param>
        /// <param name="strB">The second string to use in the comparison.</param>
        /// <param name="indexB">The starting index of the substring in .</param>
        /// <param name="length">The maximum number of characters in the substrings to compare.</param>
        /// <returns>
        ///     A 32-bit signed integer that indicates the lexical relationship between the two comparands.ValueCondition
        ///     Less than zero The substring in  is less than the substring in . Zero The substrings are equal, or  is zero.
        ///     Greater than zero The substring in  is greater than the substring in .
        /// </returns>
        public static Int32 CompareOrdinal(this String strA, Int32 indexA, String strB, Int32 indexB, Int32 length)
        {
            return String.CompareOrdinal(strA, indexA, strB, indexB, length);
        }

        #endregion

        #region Concat



        #endregion

        #endregion
    }
}