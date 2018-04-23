using System;
using System.Collections.Generic;
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
        /// <param name="this">当前字符串</param>
        /// <param name="values">待比对字符串数组</param>
        /// <returns>true-存在,false-不存在</returns>
        public static bool In(this string @this, params string[] values)
        {
            return Array.IndexOf(values, @this) != -1;
        }

        #endregion In

        #region NotIn

        /// <summary>
        /// 判断字符串是否不在数组中
        /// </summary>
        /// <param name="this">当前字符串</param>
        /// <param name="values">待比对字符串数组</param>
        /// <returns>true-不存在,false-存在</returns>
        public static bool NotIn(this string @this, params string[] values)
        {
            return Array.IndexOf(values, @this) == -1;
        }

        #endregion NotIn

        #region IsNull

        /// <summary>
        /// 判断字符串是否为null
        /// </summary>
        /// <param name="this">当前字符串</param>
        /// <returns>true-为null,false-不为null</returns>
        public static bool IsNull(this string @this)
        {
            return @this == null;
        }

        #endregion IsNull

        #region IsNotNull

        /// <summary>
        /// 判断字符串是否不为null
        /// </summary>
        /// <param name="this">当前字符串</param>
        /// <returns>true-不为null,false-为null</returns>
        public static bool IsNotNull(this string @this)
        {
            return @this != null;
        }

        #endregion IsNotNull

        #endregion Object

        #region ExtractValueType

        #region ExtractDecimal

        /// <summary>
        /// 提取decimal
        /// </summary>
        /// <param name="this">当前字符串</param>
        /// <returns>导出的decimal</returns>
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
        /// <param name="this">当前字符串</param>
        /// <returns>导出的double</returns>
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
        /// <param name="this">当前字符串</param>
        /// <returns>导出的int16</returns>
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
        /// <param name="this">当前字符串</param>
        /// <returns>导出的int32</returns>
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
        /// <param name="this">当前字符串</param>
        /// <returns>导出的int64</returns>
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

        #region ExtractUInt16

        /// <summary>
        /// 提取uint16
        /// </summary>
        /// <param name="this">当前字符串</param>
        /// <returns>导出的uInt16</returns>
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
        /// <param name="this">当前字符串</param>
        /// <returns>导出的uInt32</returns>
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
        /// <param name="this">当前字符串</param>
        /// <returns>导出的uInt64</returns>
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

        #region ExtractManyDecimal

        /// <summary>
        /// 提取decimal数组
        /// </summary>
        /// <param name="this">当前字符串</param>
        /// <returns>导出的decimal数组</returns>
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
        /// <param name="this">当前字符串</param>
        /// <returns>导出的double数组</returns>
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
        /// <param name="this">当前字符串</param>
        /// <returns>导出的int16数组</returns>
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
        /// <param name="this">当前字符串</param>
        /// <returns>导出的int32数组</returns>
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
        /// <param name="this">当前字符串</param>
        /// <returns>导出的int64数组</returns>
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
        /// <param name="this">当前字符串</param>
        /// <returns>导出的uInt16数组</returns>
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
        /// <param name="this">当前字符串</param>
        /// <returns>导出的uInt32数组</returns>
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
        /// <param name="this">当前字符串</param>
        /// <returns>导出的uInt64数组</returns>
        public static ulong[] ExtractManyUInt64(this string @this)
        {
            return Regex.Matches(@this, @"\d+")
                .Cast<Match>()
                .Select(x => Convert.ToUInt64(x.Value))
                .ToArray();
        }

        #endregion ExtractManyUInt64

        #endregion ExtractValueType

        #region Char

        #region ConvertToUtf32

        /// <summary>
        /// 转换指定位置字符为utf-32字符编码
        /// </summary>
        /// <param name="s">当前字符串</param>
        /// <param name="index">转换位置</param>
        /// <returns>字符编码</returns>
        public static int ConvertToUtf32(this string s, int index)
        {
            return char.ConvertToUtf32(s, index);
        }

        #endregion ConvertToUtf32

        #region GetNumericValue

        /// <summary>
        /// 获取指定位置的numeric字符
        /// </summary>
        /// <param name="s">当前字符串</param>
        /// <param name="index">获取位置</param>
        /// <returns>获取结果</returns>
        public static double GetNumericValue(this string s, int index)
        {
            return char.GetNumericValue(s, index);
        }

        #endregion GetNumericValue

        #region GetUnicodeCategory

        /// <summary>
        /// 把指定定字符存入UnicodeCategory
        /// </summary>
        /// <param name="s">当前字符串</param>
        /// <param name="index">指定字符位置</param>
        /// <returns>存有指定字符的UnicodeCategory</returns>
        public static UnicodeCategory GetUnicodeCategory(this string s, int index)
        {
            return char.GetUnicodeCategory(s, index);
        }

        #endregion GetUnicodeCategory

        #region IsControl

        /// <summary>
        /// 判断指定字符是否为控制字符
        /// </summary>
        /// <param name="s">当前字符串</param>
        /// <param name="index">指定字符位置</param>
        /// <returns>true-属于,false-不属于</returns>
        public static bool IsControl(this string s, int index)
        {
            return char.IsControl(s, index);
        }

        #endregion IsControl

        #region IsDigit

        /// <summary>
        /// 判断指定字符是否属于十进制数字字符
        /// </summary>
        /// <param name="s">当前字符串</param>
        /// <param name="index">指定字符位置</param>
        /// <returns>true-属于,false-不属于</returns>
        public static bool IsDigit(this string s, int index)
        {
            return char.IsDigit(s, index);
        }

        #endregion IsDigit

        #region IsHighSurrogate

        /// <summary>
        /// 判断指定字符是否为高代理项
        /// </summary>
        /// <param name="s">当前字符串</param>
        /// <param name="index">指定字符位置</param>
        /// <returns>true-属于,false-不属于</returns>
        public static bool IsHighSurrogate(this string s, int index)
        {
            return char.IsHighSurrogate(s, index);
        }

        #endregion IsHighSurrogate

        #region IsLetter

        /// <summary>
        /// 判断指定字符是否属于Unicode字母
        /// </summary>
        /// <param name="s">当前字符串</param>
        /// <param name="index">指定字符位置</param>
        /// <returns>true-属于,false-不属于</returns>
        public static bool IsLetter(this string s, int index)
        {
            return char.IsLetter(s, index);
        }

        #endregion IsLetter

        #region IsLetterOrDigit

        /// <summary>
        /// 指定字符是否为字母或十进制数字
        /// </summary>
        /// <param name="s">当前字符串</param>
        /// <param name="index">指定字符位置</param>
        /// <returns>true-属于,false-不属于</returns>
        public static bool IsLetterOrDigit(this string s, int index)
        {
            return char.IsLetterOrDigit(s, index);
        }

        #endregion IsLetterOrDigit

        #region IsLower

        /// <summary>
        /// 指定字符是否属于小写字母
        /// </summary>
        /// <param name="s">当前字符串</param>
        /// <param name="index">指定字符位置</param>
        /// <returns>true-属于,false-不属于</returns>
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
        public static int CompareOrdinal(this string strA, string strB)
        {
            return string.CompareOrdinal(strA, strB);
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
        public static int CompareOrdinal(this string strA, int indexA, string strB, int indexB, int length)
        {
            return string.CompareOrdinal(strA, indexA, strB, indexB, length);
        }

        #endregion CompareOrdinal

        #region Concat

        /// <summary>
        ///     Concatenates two specified instances of .
        /// </summary>
        /// <param name="str0">The first string to concatenate.</param>
        /// <param name="str1">The second string to concatenate.</param>
        /// <returns>The concatenation of  and .</returns>
        public static string Concat(this string str0, string str1)
        {
            return string.Concat(str0, str1);
        }

        /// <summary>
        ///     Concatenates three specified instances of .
        /// </summary>
        /// <param name="str0">The first string to concatenate.</param>
        /// <param name="str1">The second string to concatenate.</param>
        /// <param name="str2">The third string to concatenate.</param>
        /// <returns>The concatenation of , , and .</returns>
        public static string Concat(this string str0, string str1, string str2)
        {
            return string.Concat(str0, str1, str2);
        }

        /// <summary>
        ///     Concatenates four specified instances of .
        /// </summary>
        /// <param name="str0">The first string to concatenate.</param>
        /// <param name="str1">The second string to concatenate.</param>
        /// <param name="str2">The third string to concatenate.</param>
        /// <param name="str3">The fourth string to concatenate.</param>
        /// <returns>The concatenation of , , , and .</returns>
        public static string Concat(this string str0, string str1, string str2, string str3)
        {
            return string.Concat(str0, str1, str2, str3);
        }

        #endregion Concat

        #region Copy

        /// <summary>
        ///     Creates a new instance of  with the same value as a specified .
        /// </summary>
        /// <param name="str">The string to copy.</param>
        /// <returns>A new string with the same value as .</returns>
        public static string Copy(this string str)
        {
            return string.Copy(str);
        }

        #endregion Copy

        #region Format

        /// <summary>
        ///     Replaces one or more format items in a specified string with the string representation of a specified object.
        /// </summary>
        /// <param name="format">A composite format string.</param>
        /// <param name="arg0">The object to format.</param>
        /// <returns>A copy of  in which any format items are replaced by the string representation of .</returns>
        public static string Format(this string format, object arg0)
        {
            return string.Format(format, arg0);
        }

        /// <summary>
        ///     Replaces the format items in a specified string with the string representation of two specified objects.
        /// </summary>
        /// <param name="format">A composite format string.</param>
        /// <param name="arg0">The first object to format.</param>
        /// <param name="arg1">The second object to format.</param>
        /// <returns>A copy of  in which format items are replaced by the string representations of  and .</returns>
        public static string Format(this string format, object arg0, object arg1)
        {
            return string.Format(format, arg0, arg1);
        }

        /// <summary>
        ///     Replaces the format items in a specified string with the string representation of three specified objects.
        /// </summary>
        /// <param name="format">A composite format string.</param>
        /// <param name="arg0">The first object to format.</param>
        /// <param name="arg1">The second object to format.</param>
        /// <param name="arg2">The third object to format.</param>
        /// <returns>
        ///     A copy of  in which the format items have been replaced by the string representations of , , and .
        /// </returns>
        public static string Format(this string format, object arg0, object arg1, object arg2)
        {
            return string.Format(format, arg0, arg1, arg2);
        }

        /// <summary>
        ///     Replaces the format item in a specified string with the string representation of a corresponding object in a
        ///     specified array.
        /// </summary>
        /// <param name="format">A composite format string.</param>
        /// <param name="args">An object array that contains zero or more objects to format.</param>
        /// <returns>
        ///     A copy of  in which the format items have been replaced by the string representation of the corresponding
        ///     objects in .
        /// </returns>
        public static string Format(this string format, object[] args)
        {
            return string.Format(format, args);
        }

        #endregion Format

        #region Intern

        /// <summary>
        ///     Retrieves the system&#39;s reference to the specified .
        /// </summary>
        /// <param name="str">A string to search for in the intern pool.</param>
        /// <returns>
        ///     The system&#39;s reference to , if it is interned; otherwise, a new reference to a string with the value of .
        /// </returns>
        public static string Intern(this string str)
        {
            return string.Intern(str);
        }

        #endregion Intern

        #region IsInterned

        /// <summary>
        ///     Retrieves a reference to a specified .
        /// </summary>
        /// <param name="str">The string to search for in the intern pool.</param>
        /// <returns>A reference to  if it is in the common language runtime intern pool; otherwise, null.</returns>
        public static string IsInterned(this string str)
        {
            return string.IsInterned(str);
        }

        #endregion IsInterned

        #region IsNullOrWhiteSpace

        /// <summary>
        ///     Indicates whether a specified string is null, empty, or consists only of white-space characters.
        /// </summary>
        /// <param name="value">The string to test.</param>
        /// <returns>true if the  parameter is null or , or if  consists exclusively of white-space characters.</returns>
        public static bool IsNullOrWhiteSpace(this string value)
        {
            return string.IsNullOrWhiteSpace(value);
        }

        #endregion IsNullOrWhiteSpace

        #region Join

        /// <summary>
        ///     Concatenates all the elements of a string array, using the specified separator between each element.
        /// </summary>
        /// <param name="separator">
        ///     The string to use as a separator.  is included in the returned string only if  has more
        ///     than one element.
        /// </param>
        /// <param name="value">An array that contains the elements to concatenate.</param>
        /// <returns>
        ///     A string that consists of the elements in  delimited by the  string. If  is an empty array, the method
        ///     returns .
        /// </returns>
        public static string Join(this string separator, string[] value)
        {
            return string.Join(separator, value);
        }

        /// <summary>
        ///     Concatenates the elements of an object array, using the specified separator between each element.
        /// </summary>
        /// <param name="separator">
        ///     The string to use as a separator.  is included in the returned string only if  has more
        ///     than one element.
        /// </param>
        /// <param name="values">An array that contains the elements to concatenate.</param>
        /// <returns>
        ///     A string that consists of the elements of  delimited by the  string. If  is an empty array, the method
        ///     returns .
        /// </returns>
        public static string Join(this string separator, object[] values)
        {
            return string.Join(separator, values);
        }

        /// <summary>
        ///     A String extension method that joins.
        /// </summary>
        /// <typeparam name="T">Generic type parameter.</typeparam>
        /// <param name="separator">
        ///     The string to use as a separator.  is included in the returned string only if  has more
        ///     than one element.
        /// </param>
        /// <param name="values">An array that contains the elements to concatenate.</param>
        /// <returns>A String.</returns>
        public static string Join<T>(this string separator, IEnumerable<T> values)
        {
            return string.Join(separator, values);
        }

        /// <summary>
        ///     Concatenates all the elements of a string array, using the specified separator between each element.
        /// </summary>
        /// <param name="separator">
        ///     The string to use as a separator.  is included in the returned string only if  has more
        ///     than one element.
        /// </param>
        /// <param name="values">An array that contains the elements to concatenate.</param>
        /// <returns>
        ///     A string that consists of the elements in  delimited by the  string. If  is an empty array, the method
        ///     returns .
        /// </returns>
        public static string Join(this string separator, IEnumerable<string> values)
        {
            return string.Join(separator, values);
        }

        /// <summary>
        ///     Concatenates the specified elements of a string array, using the specified separator between each element.
        /// </summary>
        /// <param name="separator">
        ///     The string to use as a separator.  is included in the returned string only if  has more
        ///     than one element.
        /// </param>
        /// <param name="value">An array that contains the elements to concatenate.</param>
        /// <param name="startIndex">The first element in  to use.</param>
        /// <param name="count">The number of elements of  to use.</param>
        /// <returns>
        ///     A string that consists of the strings in  delimited by the  string. -or- if  is zero,  has no elements, or
        ///     and all the elements of  are .
        /// </returns>
        public static string Join(this string separator, string[] value, int startIndex, int count)
        {
            return string.Join(separator, value, startIndex, count);
        }

        #endregion Join

        #endregion String
    }
}