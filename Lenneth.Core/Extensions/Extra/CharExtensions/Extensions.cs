using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;

namespace Lenneth.Core.Extensions.Extra.CharExtensions
{
    public static class Extensions
    {
        #region Object

        #region In

        /// <summary>
        ///     A T extension method to determines whether the object is equal to any of the provided values.
        /// </summary>
        /// <param name="this">The object to be compared.</param>
        /// <param name="values">The value list to compare with the object.</param>
        /// <returns>true if the values list contains the object, else false.</returns>
        public static bool In(this char @this, params char[] values)
        {
            return Array.IndexOf(values, @this) != -1;
        }

        #endregion In

        #region NotIn

        /// <summary>
        ///     A T extension method to determines whether the object is not equal to any of the provided values.
        /// </summary>
        /// <param name="this">The object to be compared.</param>
        /// <param name="values">The value list to compare with the object.</param>
        /// <returns>true if the values list doesn't contains the object, else false.</returns>
        public static bool NotIn(this char @this, params char[] values)
        {
            return Array.IndexOf(values, @this) == -1;
        }

        #endregion NotIn

        #endregion Object

        #region Char

        #region ConvertToUtf32

        /// <summary>
        ///     Converts the value of a UTF-16 encoded surrogate pair into a Unicode code point.
        /// </summary>
        /// <param name="highSurrogate">A high surrogate code unit (that is, a code unit ranging from U+D800 through U+DBFF).</param>
        /// <param name="lowSurrogate">A low surrogate code unit (that is, a code unit ranging from U+DC00 through U+DFFF).</param>
        /// <returns>The 21-bit Unicode code point represented by the  and  parameters.</returns>
        public static int ConvertToUtf32(this char highSurrogate, char lowSurrogate)
        {
            return char.ConvertToUtf32(highSurrogate, lowSurrogate);
        }

        #endregion ConvertToUtf32

        #region GetNumericValue

        /// <summary>
        ///     Converts the specified numeric Unicode character to a double-precision floating point number.
        /// </summary>
        /// <param name="c">The Unicode character to convert.</param>
        /// <returns>The numeric value of  if that character represents a number; otherwise, -1.0.</returns>
        public static double GetNumericValue(this char c)
        {
            return char.GetNumericValue(c);
        }

        #endregion GetNumericValue

        #region UnicodeCategory

        /// <summary>
        ///     Categorizes a specified Unicode character into a group identified by one of the  values.
        /// </summary>
        /// <param name="c">The Unicode character to categorize.</param>
        /// <returns>A  value that identifies the group that contains .</returns>
        public static UnicodeCategory GetUnicodeCategory(this char c)
        {
            return char.GetUnicodeCategory(c);
        }

        #endregion UnicodeCategory

        #region IsControl

        /// <summary>
        ///     Indicates whether the specified Unicode character is categorized as a control character.
        /// </summary>
        /// <param name="c">The Unicode character to evaluate.</param>
        /// <returns>true if  is a control character; otherwise, false.</returns>
        public static bool IsControl(this char c)
        {
            return char.IsControl(c);
        }

        #endregion IsControl

        #region IsDigit

        /// <summary>
        ///     Indicates whether the specified Unicode character is categorized as a decimal digit.
        /// </summary>
        /// <param name="c">The Unicode character to evaluate.</param>
        /// <returns>true if  is a decimal digit; otherwise, false.</returns>
        public static bool IsDigit(this char c)
        {
            return char.IsDigit(c);
        }

        #endregion IsDigit

        #region IsHighSurrogate

        /// <summary>
        ///     Indicates whether the specified  object is a high surrogate.
        /// </summary>
        /// <param name="c">The Unicode character to evaluate.</param>
        /// <returns>
        ///     true if the numeric value of the  parameter ranges from U+D800 through U+DBFF; otherwise, false.
        /// </returns>
        public static bool IsHighSurrogate(this char c)
        {
            return char.IsHighSurrogate(c);
        }

        #endregion IsHighSurrogate

        #region IsLetter

        /// <summary>
        ///     Indicates whether the specified Unicode character is categorized as a Unicode letter.
        /// </summary>
        /// <param name="c">The Unicode character to evaluate.</param>
        /// <returns>true if  is a letter; otherwise, false.</returns>
        public static bool IsLetter(this char c)
        {
            return char.IsLetter(c);
        }

        #endregion IsLetter

        #region IsLetterOrDigit

        /// <summary>
        ///     Indicates whether the specified Unicode character is categorized as a letter or a decimal digit.
        /// </summary>
        /// <param name="c">The Unicode character to evaluate.</param>
        /// <returns>true if  is a letter or a decimal digit; otherwise, false.</returns>
        public static bool IsLetterOrDigit(this char c)
        {
            return char.IsLetterOrDigit(c);
        }

        #endregion IsLetterOrDigit

        #region IsLower

        /// <summary>
        ///     Indicates whether the specified Unicode character is categorized as a lowercase letter.
        /// </summary>
        /// <param name="c">The Unicode character to evaluate.</param>
        /// <returns>true if  is a lowercase letter; otherwise, false.</returns>
        public static bool IsLower(this char c)
        {
            return char.IsLower(c);
        }

        #endregion IsLower

        #region IsLowSurrogate

        /// <summary>
        ///     Indicates whether the specified  object is a low surrogate.
        /// </summary>
        /// <param name="c">The character to evaluate.</param>
        /// <returns>
        ///     true if the numeric value of the  parameter ranges from U+DC00 through U+DFFF; otherwise, false.
        /// </returns>
        public static bool IsLowSurrogate(this char c)
        {
            return char.IsLowSurrogate(c);
        }

        #endregion IsLowSurrogate

        #region IsNumber

        /// <summary>
        ///     Indicates whether the specified Unicode character is categorized as a number.
        /// </summary>
        /// <param name="c">The Unicode character to evaluate.</param>
        /// <returns>true if  is a number; otherwise, false.</returns>
        public static bool IsNumber(this char c)
        {
            return char.IsNumber(c);
        }

        #endregion IsNumber

        #region IsPunctuation

        /// <summary>
        ///     Indicates whether the specified Unicode character is categorized as a punctuation mark.
        /// </summary>
        /// <param name="c">The Unicode character to evaluate.</param>
        /// <returns>true if  is a punctuation mark; otherwise, false.</returns>
        public static bool IsPunctuation(this char c)
        {
            return char.IsPunctuation(c);
        }

        #endregion IsPunctuation

        #region IsSeparator

        /// <summary>
        ///     Indicates whether the specified Unicode character is categorized as a separator character.
        /// </summary>
        /// <param name="c">The Unicode character to evaluate.</param>
        /// <returns>true if  is a separator character; otherwise, false.</returns>
        public static bool IsSeparator(this char c)
        {
            return char.IsSeparator(c);
        }

        #endregion IsSeparator

        #region IsSurrogate

        /// <summary>
        ///     Indicates whether the specified character has a surrogate code unit.
        /// </summary>
        /// <param name="c">The Unicode character to evaluate.</param>
        /// <returns>true if  is either a high surrogate or a low surrogate; otherwise, false.</returns>
        public static bool IsSurrogate(this char c)
        {
            return char.IsSurrogate(c);
        }

        #endregion IsSurrogate

        #region IsSurrogatePair

        /// <summary>
        ///     Indicates whether the two specified  objects form a surrogate pair.
        /// </summary>
        /// <param name="highSurrogate">The character to evaluate as the high surrogate of a surrogate pair.</param>
        /// <param name="lowSurrogate">The character to evaluate as the low surrogate of a surrogate pair.</param>
        /// <returns>
        ///     true if the numeric value of the  parameter ranges from U+D800 through U+DBFF, and the numeric value of the
        ///     parameter ranges from U+DC00 through U+DFFF; otherwise, false.
        /// </returns>
        public static bool IsSurrogatePair(this char highSurrogate, char lowSurrogate)
        {
            return char.IsSurrogatePair(highSurrogate, lowSurrogate);
        }

        #endregion IsSurrogatePair

        #region IsSymbol

        /// <summary>
        ///     Indicates whether the specified Unicode character is categorized as a symbol character.
        /// </summary>
        /// <param name="c">The Unicode character to evaluate.</param>
        /// <returns>true if  is a symbol character; otherwise, false.</returns>
        public static bool IsSymbol(this char c)
        {
            return char.IsSymbol(c);
        }

        #endregion IsSymbol

        #region IsUpper

        /// <summary>
        ///     Indicates whether the specified Unicode character is categorized as an uppercase letter.
        /// </summary>
        /// <param name="c">The Unicode character to evaluate.</param>
        /// <returns>true if  is an uppercase letter; otherwise, false.</returns>
        public static bool IsUpper(this char c)
        {
            return char.IsUpper(c);
        }

        #endregion IsUpper

        #region IsWhiteSpace

        /// <summary>
        ///     Indicates whether the specified Unicode character is categorized as white space.
        /// </summary>
        /// <param name="c">The Unicode character to evaluate.</param>
        /// <returns>true if  is white space; otherwise, false.</returns>
        public static bool IsWhiteSpace(this char c)
        {
            return char.IsWhiteSpace(c);
        }

        #endregion IsWhiteSpace

        #region ToLower

        /// <summary>
        ///     Converts the value of a specified Unicode character to its lowercase equivalent using specified culture-
        ///     specific formatting information.
        /// </summary>
        /// <param name="c">The Unicode character to convert.</param>
        /// <param name="culture">An object that supplies culture-specific casing rules.</param>
        /// <returns>
        ///     The lowercase equivalent of , modified according to , or the unchanged value of , if  is already lowercase or
        ///     not alphabetic.
        /// </returns>
        public static char ToLower(this char c, CultureInfo culture)
        {
            return char.ToLower(c, culture);
        }

        /// <summary>
        ///     Converts the value of a Unicode character to its lowercase equivalent.
        /// </summary>
        /// <param name="c">The Unicode character to convert.</param>
        /// <returns>
        ///     The lowercase equivalent of , or the unchanged value of , if  is already lowercase or not alphabetic.
        /// </returns>
        public static char ToLower(this char c)
        {
            return char.ToLower(c);
        }

        #endregion ToLower

        #region ToLowerInvariant

        /// <summary>
        ///     Converts the value of a Unicode character to its lowercase equivalent using the casing rules of the invariant
        ///     culture.
        /// </summary>
        /// <param name="c">The Unicode character to convert.</param>
        /// <returns>
        ///     The lowercase equivalent of the  parameter, or the unchanged value of , if  is already lowercase or not
        ///     alphabetic.
        /// </returns>
        public static char ToLowerInvariant(this char c)
        {
            return char.ToLowerInvariant(c);
        }

        #endregion ToLowerInvariant

        #region ToString

        /// <summary>
        ///     Converts the specified Unicode character to its equivalent string representation.
        /// </summary>
        /// <param name="c">The Unicode character to convert.</param>
        /// <returns>The string representation of the value of .</returns>
        public static string ToString(this char c)
        {
            return char.ToString(c);
        }

        #endregion ToString

        #region ToUpper

        /// <summary>
        ///     Converts the value of a specified Unicode character to its uppercase equivalent using specified culture-
        ///     specific formatting information.
        /// </summary>
        /// <param name="c">The Unicode character to convert.</param>
        /// <param name="culture">An object that supplies culture-specific casing rules.</param>
        /// <returns>
        ///     The uppercase equivalent of , modified according to , or the unchanged value of  if  is already uppercase,
        ///     has no uppercase equivalent, or is not alphabetic.
        /// </returns>
        public static char ToUpper(this char c, CultureInfo culture)
        {
            return char.ToUpper(c, culture);
        }

        /// <summary>
        ///     Converts the value of a Unicode character to its uppercase equivalent.
        /// </summary>
        /// <param name="c">The Unicode character to convert.</param>
        /// <returns>
        ///     The uppercase equivalent of , or the unchanged value of  if  is already uppercase, has no uppercase
        ///     equivalent, or is not alphabetic.
        /// </returns>
        public static char ToUpper(this char c)
        {
            return char.ToUpper(c);
        }

        #endregion ToUpper

        #region ToUpperInvariant

        /// <summary>
        ///     Converts the value of a Unicode character to its uppercase equivalent using the casing rules of the invariant
        ///     culture.
        /// </summary>
        /// <param name="c">The Unicode character to convert.</param>
        /// <returns>
        ///     The uppercase equivalent of the  parameter, or the unchanged value of , if  is already uppercase or not
        ///     alphabetic.
        /// </returns>
        public static char ToUpperInvariant(this char c)
        {
            return char.ToUpperInvariant(c);
        }

        #endregion ToUpperInvariant

        #endregion Char

        #region Other

        #region Repeat

        /// <summary>
        ///     A char extension method that repeats a character the specified number of times.
        /// </summary>
        /// <param name="this">The @this to act on.</param>
        /// <param name="repeatCount">Number of repeats.</param>
        /// <returns>The repeated char.</returns>
        public static string Repeat(this char @this, int repeatCount)
        {
            return new string(@this, repeatCount);
        }

        #endregion Repeat

        #region To

        /// <summary>
        ///     Enumerates from @this to toCharacter.
        /// </summary>
        /// <param name="this">The @this to act on.</param>
        /// <param name="toCharacter">to character.</param>
        /// <returns>An enumerator that allows foreach to be used to process @this to toCharacter.</returns>
        public static IEnumerable<char> To(this char @this, char toCharacter)
        {
            var reverseRequired = (@this > toCharacter);

            var first = reverseRequired ? toCharacter : @this;
            var last = reverseRequired ? @this : toCharacter;

            var result = Enumerable.Range(first, last - first + 1).Select(charCode => (char)charCode);

            if (reverseRequired)
            {
                result = result.Reverse();
            }

            return result;
        }

        #endregion To

        #endregion Other
    }
}