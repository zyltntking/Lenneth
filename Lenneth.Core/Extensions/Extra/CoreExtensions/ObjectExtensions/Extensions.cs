﻿using System;
using System.ComponentModel;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Runtime.Serialization;
using System.Runtime.Serialization.Formatters.Binary;

namespace Lenneth.Core.Extensions.Extra.CoreExtensions
{
    public static partial class Extensions
    {
        #region Casting

        #region As

        /// <summary>
        ///     An object extension method that cast anonymous type to the specified type.
        /// </summary>
        /// <typeparam name="T">Generic type parameter. The specified type.</typeparam>
        /// <param name="this">The @this to act on.</param>
        /// <returns>The object as the specified type.</returns>
        public static T As<T>(this object @this)
        {
            return (T)@this;
        }

        #endregion As

        #region AsOrDefault

        /// <summary>
        ///     An object extension method that converts the @this to an or default.
        /// </summary>
        /// <typeparam name="T">Generic type parameter.</typeparam>
        /// <param name="this">The @this to act on.</param>
        /// <returns>A T.</returns>
        public static T AsOrDefault<T>(this object @this)
        {
            try
            {
                return (T)@this;
            }
            catch (Exception)
            {
                return default(T);
            }
        }

        /// <summary>
        ///     An object extension method that converts the @this to an or default.
        /// </summary>
        /// <typeparam name="T">Generic type parameter.</typeparam>
        /// <param name="this">The @this to act on.</param>
        /// <param name="defaultValue">The default value.</param>
        /// <returns>A T.</returns>
        public static T AsOrDefault<T>(this object @this, T defaultValue)
        {
            try
            {
                return (T)@this;
            }
            catch (Exception)
            {
                return defaultValue;
            }
        }

        /// <summary>
        ///     An object extension method that converts the @this to an or default.
        /// </summary>
        /// <typeparam name="T">Generic type parameter.</typeparam>
        /// <param name="this">The @this to act on.</param>
        /// <param name="defaultValueFactory">The default value factory.</param>
        /// <returns>A T.</returns>
        /// <example>
        ///     <code>
        ///       using Microsoft.VisualStudio.TestTools.UnitTesting;
        ///       using ExtensionMethods.Object;
        ///
        ///       namespace ExtensionMethods.Examples
        ///       {
        ///           [TestClass]
        ///           public class System_Object_AsOrDefault
        ///           {
        ///               [TestMethod]
        ///               public void AsOrDefault()
        ///               {
        ///                   // Type
        ///                   object intValue = 1;
        ///                   object invalidValue = &quot;Fizz&quot;;
        ///
        ///                   // Exemples
        ///                   var result1 = intValue.AsOrDefault&lt;int&gt;(); // return 1;
        ///                   var result2 = invalidValue.AsOrDefault&lt;int&gt;(); // return 0;
        ///                   int result3 = invalidValue.AsOrDefault(3); // return 3;
        ///                   int result4 = invalidValue.AsOrDefault(() =&gt; 4); // return 4;
        ///
        ///                   // Unit Test
        ///                   Assert.AreEqual(1, result1);
        ///                   Assert.AreEqual(0, result2);
        ///                   Assert.AreEqual(3, result3);
        ///                   Assert.AreEqual(4, result4);
        ///               }
        ///           }
        ///       }
        /// </code>
        /// </example>
        /// <example>
        ///     <code>
        ///       using Microsoft.VisualStudio.TestTools.UnitTesting;
        ///       using ExtensionMethods.Object;
        ///
        ///       namespace ExtensionMethods.Examples
        ///       {
        ///           [TestClass]
        ///           public class System_Object_AsOrDefault
        ///           {
        ///               [TestMethod]
        ///               public void AsOrDefault()
        ///               {
        ///                   // Type
        ///                   object intValue = 1;
        ///                   object invalidValue = &quot;Fizz&quot;;
        ///
        ///                   // Exemples
        ///                   var result1 = intValue.AsOrDefault&lt;int&gt;(); // return 1;
        ///                   var result2 = invalidValue.AsOrDefault&lt;int&gt;(); // return 0;
        ///                   int result3 = invalidValue.AsOrDefault(3); // return 3;
        ///                   int result4 = invalidValue.AsOrDefault(() =&gt; 4); // return 4;
        ///
        ///                   // Unit Test
        ///                   Assert.AreEqual(1, result1);
        ///                   Assert.AreEqual(0, result2);
        ///                   Assert.AreEqual(3, result3);
        ///                   Assert.AreEqual(4, result4);
        ///               }
        ///           }
        ///       }
        /// </code>
        /// </example>
        /// <example>
        ///     <code>
        ///           using Microsoft.VisualStudio.TestTools.UnitTesting;
        ///           using ExtensionMethods.Object;
        ///
        ///           namespace ExtensionMethods.Examples
        ///           {
        ///               [TestClass]
        ///               public class System_Object_AsOrDefault
        ///               {
        ///                   [TestMethod]
        ///                   public void AsOrDefault()
        ///                   {
        ///                       // Type
        ///                       object intValue = 1;
        ///                       object invalidValue = &quot;Fizz&quot;;
        ///
        ///                       // Exemples
        ///                       var result1 = intValue.AsOrDefault&lt;int&gt;(); // return 1;
        ///                       var result2 = invalidValue.AsOrDefault&lt;int&gt;(); // return 0;
        ///                       int result3 = invalidValue.AsOrDefault(3); // return 3;
        ///                       int result4 = invalidValue.AsOrDefault(() =&gt; 4); // return 4;
        ///
        ///                       // Unit Test
        ///                       Assert.AreEqual(1, result1);
        ///                       Assert.AreEqual(0, result2);
        ///                       Assert.AreEqual(3, result3);
        ///                       Assert.AreEqual(4, result4);
        ///                   }
        ///               }
        ///           }
        ///     </code>
        /// </example>
        public static T AsOrDefault<T>(this object @this, Func<T> defaultValueFactory)
        {
            try
            {
                return (T)@this;
            }
            catch (Exception)
            {
                return defaultValueFactory();
            }
        }

        /// <summary>
        ///     An object extension method that converts the @this to an or default.
        /// </summary>
        /// <typeparam name="T">Generic type parameter.</typeparam>
        /// <param name="this">The @this to act on.</param>
        /// <param name="defaultValueFactory">The default value factory.</param>
        /// <returns>A T.</returns>
        /// <example>
        ///     <code>
        ///       using Microsoft.VisualStudio.TestTools.UnitTesting;
        ///       using ExtensionMethods.Object;
        ///
        ///       namespace ExtensionMethods.Examples
        ///       {
        ///           [TestClass]
        ///           public class System_Object_AsOrDefault
        ///           {
        ///               [TestMethod]
        ///               public void AsOrDefault()
        ///               {
        ///                   // Type
        ///                   object intValue = 1;
        ///                   object invalidValue = &quot;Fizz&quot;;
        ///
        ///                   // Exemples
        ///                   var result1 = intValue.AsOrDefault&lt;int&gt;(); // return 1;
        ///                   var result2 = invalidValue.AsOrDefault&lt;int&gt;(); // return 0;
        ///                   int result3 = invalidValue.AsOrDefault(3); // return 3;
        ///                   int result4 = invalidValue.AsOrDefault(() =&gt; 4); // return 4;
        ///
        ///                   // Unit Test
        ///                   Assert.AreEqual(1, result1);
        ///                   Assert.AreEqual(0, result2);
        ///                   Assert.AreEqual(3, result3);
        ///                   Assert.AreEqual(4, result4);
        ///               }
        ///           }
        ///       }
        /// </code>
        /// </example>
        /// <example>
        ///     <code>
        ///       using Microsoft.VisualStudio.TestTools.UnitTesting;
        ///       using ExtensionMethods.Object;
        ///
        ///       namespace ExtensionMethods.Examples
        ///       {
        ///           [TestClass]
        ///           public class System_Object_AsOrDefault
        ///           {
        ///               [TestMethod]
        ///               public void AsOrDefault()
        ///               {
        ///                   // Type
        ///                   object intValue = 1;
        ///                   object invalidValue = &quot;Fizz&quot;;
        ///
        ///                   // Exemples
        ///                   var result1 = intValue.AsOrDefault&lt;int&gt;(); // return 1;
        ///                   var result2 = invalidValue.AsOrDefault&lt;int&gt;(); // return 0;
        ///                   int result3 = invalidValue.AsOrDefault(3); // return 3;
        ///                   int result4 = invalidValue.AsOrDefault(() =&gt; 4); // return 4;
        ///
        ///                   // Unit Test
        ///                   Assert.AreEqual(1, result1);
        ///                   Assert.AreEqual(0, result2);
        ///                   Assert.AreEqual(3, result3);
        ///                   Assert.AreEqual(4, result4);
        ///               }
        ///           }
        ///       }
        /// </code>
        /// </example>
        /// <example>
        ///     <code>
        ///           using Microsoft.VisualStudio.TestTools.UnitTesting;
        ///           using ExtensionMethods.Object;
        ///
        ///           namespace ExtensionMethods.Examples
        ///           {
        ///               [TestClass]
        ///               public class System_Object_AsOrDefault
        ///               {
        ///                   [TestMethod]
        ///                   public void AsOrDefault()
        ///                   {
        ///                       // Type
        ///                       object intValue = 1;
        ///                       object invalidValue = &quot;Fizz&quot;;
        ///
        ///                       // Exemples
        ///                       var result1 = intValue.AsOrDefault&lt;int&gt;(); // return 1;
        ///                       var result2 = invalidValue.AsOrDefault&lt;int&gt;(); // return 0;
        ///                       int result3 = invalidValue.AsOrDefault(3); // return 3;
        ///                       int result4 = invalidValue.AsOrDefault(() =&gt; 4); // return 4;
        ///
        ///                       // Unit Test
        ///                       Assert.AreEqual(1, result1);
        ///                       Assert.AreEqual(0, result2);
        ///                       Assert.AreEqual(3, result3);
        ///                       Assert.AreEqual(4, result4);
        ///                   }
        ///               }
        ///           }
        ///     </code>
        /// </example>
        public static T AsOrDefault<T>(this object @this, Func<object, T> defaultValueFactory)
        {
            try
            {
                return (T)@this;
            }
            catch (Exception)
            {
                return defaultValueFactory(@this);
            }
        }

        #endregion AsOrDefault

        #region IsAssignableFrom

        /// <summary>
        ///     An object extension method that query if '@this' is assignable from.
        /// </summary>
        /// <typeparam name="T">Generic type parameter.</typeparam>
        /// <param name="this">The @this to act on.</param>
        /// <returns>true if assignable from, false if not.</returns>
        public static bool IsAssignableFrom<T>(this object @this)
        {
            var type = @this.GetType();
            return type.IsAssignableFrom(typeof(T));
        }

        /// <summary>
        ///     An object extension method that query if '@this' is assignable from.
        /// </summary>
        /// <param name="this">The @this to act on.</param>
        /// <param name="targetType">Type of the target.</param>
        /// <returns>true if assignable from, false if not.</returns>
        public static bool IsAssignableFrom(this object @this, Type targetType)
        {
            var type = @this.GetType();
            return type.IsAssignableFrom(targetType);
        }

        #endregion IsAssignableFrom

        #endregion Casting

        #region Chaining

        #region Chain

        /// <summary>
        ///     A T extension method that chains actions.
        /// </summary>
        /// <typeparam name="T">Generic type parameter.</typeparam>
        /// <param name="this">The @this to act on.</param>
        /// <param name="action">The action.</param>
        /// <returns>The @this acted on.</returns>
        public static T Chain<T>(this T @this, Action<T> action)
        {
            action(@this);

            return @this;
        }

        #endregion Chain

        #endregion Chaining

        #region Cloning

        #region DeepClone

        /// <summary>
        ///     A T extension method that makes a deep copy of '@this' object.
        /// </summary>
        /// <typeparam name="T">Generic type parameter.</typeparam>
        /// <param name="this">The @this to act on.</param>
        /// <returns>the copied object.</returns>
        public static T DeepClone<T>(this T @this)
        {
            IFormatter formatter = new BinaryFormatter();
            using (var stream = new MemoryStream())
            {
                formatter.Serialize(stream, @this);
                stream.Seek(0, SeekOrigin.Begin);
                return (T)formatter.Deserialize(stream);
            }
        }

        #endregion DeepClone

        #region ShallowCopy

        /// <summary>
        ///     A T extension method that shallow copy.
        /// </summary>
        /// <typeparam name="T">Generic type parameter.</typeparam>
        /// <param name="this">The @this to act on.</param>
        /// <returns>A T.</returns>
        public static T ShallowCopy<T>(this T @this)
        {
            var method = @this.GetType().GetMethod("MemberwiseClone", BindingFlags.NonPublic | BindingFlags.Instance);
            return (T)method?.Invoke(@this, null);
        }

        #endregion ShallowCopy

        #endregion Cloning

        #region Convert

        #region IsValidValueType

        #region IsValidBoolean

        /// <summary>
        ///     An object extension method that query if '@this' is valid bool.
        /// </summary>
        /// <param name="this">The @this to act on.</param>
        /// <returns>true if valid bool, false if not.</returns>
        public static bool IsValidBoolean(this object @this)
        {
            return @this == null || bool.TryParse(@this.ToString(), out _);
        }

        #endregion IsValidBoolean

        #region IsValidByte

        /// <summary>
        ///     An object extension method that query if '@this' is valid byte.
        /// </summary>
        /// <param name="this">The @this to act on.</param>
        /// <returns>true if valid byte, false if not.</returns>
        public static bool IsValidByte(this object @this)
        {
            return @this == null || byte.TryParse(@this.ToString(), out _);
        }

        #endregion IsValidByte

        #region IsValidChar

        /// <summary>
        ///     An object extension method that query if '@this' is valid char.
        /// </summary>
        /// <param name="this">The @this to act on.</param>
        /// <returns>true if valid char, false if not.</returns>
        public static bool IsValidChar(this object @this)
        {
            return char.TryParse(@this.ToString(), out _);
        }

        #endregion IsValidChar

        #region IsValidDateTime

        /// <summary>
        ///     An object extension method that query if '@this' is valid System.DateTime.
        /// </summary>
        /// <param name="this">The @this to act on.</param>
        /// <returns>true if valid System.DateTime, false if not.</returns>
        public static bool IsValidDateTime(this object @this)
        {
            return @this == null || DateTime.TryParse(@this.ToString(), out _);
        }

        #endregion IsValidDateTime

        #region IsValidDateTimeOffSet

        /// <summary>
        ///     An object extension method that query if '@this' is valid System.DateTimeOffset.
        /// </summary>
        /// <param name="this">The @this to act on.</param>
        /// <returns>true if valid System.DateTimeOffset, false if not.</returns>
        public static bool IsValidDateTimeOffSet(this object @this)
        {
            return @this == null || DateTimeOffset.TryParse(@this.ToString(), out _);
        }

        #endregion IsValidDateTimeOffSet

        #region IsValidDecimal

        /// <summary>
        ///     An object extension method that query if '@this' is valid decimal.
        /// </summary>
        /// <param name="this">The @this to act on.</param>
        /// <returns>true if valid decimal, false if not.</returns>
        public static bool IsValidDecimal(this object @this)
        {
            return @this == null || decimal.TryParse(@this.ToString(), out _);
        }

        #endregion IsValidDecimal

        #region IsValidDouble

        /// <summary>
        ///     An object extension method that query if '@this' is valid double.
        /// </summary>
        /// <param name="this">The @this to act on.</param>
        /// <returns>true if valid double, false if not.</returns>
        public static bool IsValidDouble(this object @this)
        {
            return @this == null || double.TryParse(@this.ToString(), out _);
        }

        #endregion IsValidDouble

        #region IsValidFloat

        /// <summary>
        ///     An object extension method that query if '@this' is valid float.
        /// </summary>
        /// <param name="this">The @this to act on.</param>
        /// <returns>true if valid float, false if not.</returns>
        public static bool IsValidFloat(this object @this)
        {
            return @this == null || float.TryParse(@this.ToString(), out _);
        }

        #endregion IsValidFloat

        #region IsValidGuid

        /// <summary>
        ///     An object extension method that query if '@this' is valid System.Guid.
        /// </summary>
        /// <param name="this">The @this to act on.</param>
        /// <returns>true if valid System.Guid, false if not.</returns>
        public static bool IsValidGuid(this object @this)
        {
            return Guid.TryParse(@this.ToString(), out _);
        }

        #endregion IsValidGuid

        #region IsValidInt16

        /// <summary>
        ///     An object extension method that query if '@this' is valid short.
        /// </summary>
        /// <param name="this">The @this to act on.</param>
        /// <returns>true if valid short, false if not.</returns>
        public static bool IsValidInt16(this object @this)
        {
            return @this == null || short.TryParse(@this.ToString(), out _);
        }

        #endregion IsValidInt16

        #region IsValidInt32

        /// <summary>
        ///     An object extension method that query if '@this' is valid int.
        /// </summary>
        /// <param name="this">The @this to act on.</param>
        /// <returns>true if valid int, false if not.</returns>
        public static bool IsValidInt32(this object @this)
        {
            return @this == null || int.TryParse(@this.ToString(), out _);
        }

        #endregion IsValidInt32

        #region IsValidInt64

        /// <summary>
        ///     An object extension method that query if '@this' is valid long.
        /// </summary>
        /// <param name="this">The @this to act on.</param>
        /// <returns>true if valid long, false if not.</returns>
        public static bool IsValidInt64(this object @this)
        {
            return @this == null || long.TryParse(@this.ToString(), out _);
        }

        #endregion IsValidInt64

        #region IsValidLong

        /// <summary>
        ///     An object extension method that query if '@this' is valid long.
        /// </summary>
        /// <param name="this">The @this to act on.</param>
        /// <returns>true if valid long, false if not.</returns>
        public static bool IsValidLong(this object @this)
        {
            return @this == null || long.TryParse(@this.ToString(), out _);
        }

        #endregion IsValidLong

        #region IsValidSByte

        /// <summary>
        ///     An object extension method that query if '@this' is valid sbyte.
        /// </summary>
        /// <param name="this">The @this to act on.</param>
        /// <returns>true if valid sbyte, false if not.</returns>
        public static bool IsValidSByte(this object @this)
        {
            return @this == null || sbyte.TryParse(@this.ToString(), out _);
        }

        #endregion IsValidSByte

        #region IsValidShort

        /// <summary>
        ///     An object extension method that query if '@this' is valid short.
        /// </summary>
        /// <param name="this">The @this to act on.</param>
        /// <returns>true if valid short, false if not.</returns>
        public static bool IsValidShort(this object @this)
        {
            return @this == null || short.TryParse(@this.ToString(), out _);
        }

        #endregion IsValidShort

        #region IsValidSingle

        /// <summary>
        ///     An object extension method that query if '@this' is valid float.
        /// </summary>
        /// <param name="this">The @this to act on.</param>
        /// <returns>true if valid float, false if not.</returns>
        public static bool IsValidSingle(this object @this)
        {
            return @this == null || float.TryParse(@this.ToString(), out _);
        }

        #endregion IsValidSingle

        #region IsValidString

        /// <summary>
        ///     An object extension method that query if '@this' is valid string.
        /// </summary>
        /// <param name="this">The @this to act on.</param>
        /// <returns>true if valid string, false if not.</returns>
        public static bool IsValidString(this object @this)
        {
            return true;
        }

        #endregion IsValidString

        #region IsValidUInt16

        /// <summary>
        ///     An object extension method that query if '@this' is valid ushort.
        /// </summary>
        /// <param name="this">The @this to act on.</param>
        /// <returns>true if valid ushort, false if not.</returns>
        public static bool IsValidUInt16(this object @this)
        {
            return @this == null || ushort.TryParse(@this.ToString(), out _);
        }

        #endregion IsValidUInt16

        #region IsValidUInt32

        /// <summary>
        ///     An object extension method that query if '@this' is valid uint.
        /// </summary>
        /// <param name="this">The @this to act on.</param>
        /// <returns>true if valid uint, false if not.</returns>
        public static bool IsValidUInt32(this object @this)
        {
            return @this == null || uint.TryParse(@this.ToString(), out _);
        }

        #endregion IsValidUInt32

        #region IsValidUInt64

        /// <summary>
        ///     An object extension method that query if '@this' is valid ulong.
        /// </summary>
        /// <param name="this">The @this to act on.</param>
        /// <returns>true if valid ulong, false if not.</returns>
        public static bool IsValidUInt64(this object @this)
        {
            return @this == null || ulong.TryParse(@this.ToString(), out _);
        }

        #endregion IsValidUInt64

        #region IsValidULong

        /// <summary>
        ///     An object extension method that query if '@this' is valid ulong.
        /// </summary>
        /// <param name="this">The @this to act on.</param>
        /// <returns>true if valid ulong, false if not.</returns>
        public static bool IsValidULong(this object @this)
        {
            return @this == null || ulong.TryParse(@this.ToString(), out _);
        }

        #endregion IsValidULong

        #region IsValidUShort

        /// <summary>
        ///     An object extension method that query if '@this' is valid ushort.
        /// </summary>
        /// <param name="this">The @this to act on.</param>
        /// <returns>true if valid ushort, false if not.</returns>
        public static bool IsValidUShort(this object @this)
        {
            return @this == null || ushort.TryParse(@this.ToString(), out _);
        }

        #endregion IsValidUShort

        #endregion IsValidValueType

        #region ToValueType

        #region ToBoolean

        /// <summary>
        ///     An object extension method that converts the @this to a boolean.
        /// </summary>
        /// <param name="this">The @this to act on.</param>
        /// <returns>@this as a bool.</returns>
        public static bool ToBoolean(this object @this)
        {
            return Convert.ToBoolean(@this);
        }

        #endregion ToBoolean

        #region ToBooleanOrDefault

        /// <summary>
        ///     An object extension method that converts this object to a boolean or default.
        /// </summary>
        /// <param name="this">The @this to act on.</param>
        /// <returns>The given data converted to a bool.</returns>
        public static bool ToBooleanOrDefault(this object @this)
        {
            try
            {
                return Convert.ToBoolean(@this);
            }
            catch (Exception)
            {
                return default(bool);
            }
        }

        /// <summary>
        ///     An object extension method that converts this object to a boolean or default.
        /// </summary>
        /// <param name="this">The @this to act on.</param>
        /// <param name="defaultValue">true to default value.</param>
        /// <returns>The given data converted to a bool.</returns>
        public static bool ToBooleanOrDefault(this object @this, bool defaultValue)
        {
            try
            {
                return Convert.ToBoolean(@this);
            }
            catch (Exception)
            {
                return defaultValue;
            }
        }

        /// <summary>
        /// An object extension method that converts this object to a boolean or default.
        /// </summary>
        /// <param name="this">The @this to act on.</param>
        /// <param name="defaultValue">true to default value.</param>
        /// <param name="useDefaultIfNull">true to use default if null.</param>
        /// <returns>The given data converted to a bool.</returns>
        public static bool ToBooleanOrDefault(this object @this, bool defaultValue, bool useDefaultIfNull)
        {
            if (useDefaultIfNull && @this == null)
            {
                return defaultValue;
            }

            try
            {
                return Convert.ToBoolean(@this);
            }
            catch (Exception)
            {
                return defaultValue;
            }
        }

        /// <summary>
        ///     An object extension method that converts this object to a boolean or default.
        /// </summary>
        /// <param name="this">The @this to act on.</param>
        /// <param name="defaultValueFactory">The default value factory.</param>
        /// <returns>The given data converted to a bool.</returns>
        public static bool ToBooleanOrDefault(this object @this, Func<bool> defaultValueFactory)
        {
            try
            {
                return Convert.ToBoolean(@this);
            }
            catch (Exception)
            {
                return defaultValueFactory();
            }
        }

        /// <summary>
        /// An object extension method that converts this object to a boolean or default.
        /// </summary>
        /// <param name="this">The @this to act on.</param>
        /// <param name="defaultValueFactory">The default value factory.</param>
        /// <param name="useDefaultIfNull">true to use default if null.</param>
        /// <returns>The given data converted to a bool.</returns>
        public static bool ToBooleanOrDefault(this object @this, Func<bool> defaultValueFactory, bool useDefaultIfNull)
        {
            if (useDefaultIfNull && @this == null)
            {
                return defaultValueFactory();
            }

            try
            {
                return Convert.ToBoolean(@this);
            }
            catch (Exception)
            {
                return defaultValueFactory();
            }
        }

        #endregion ToBooleanOrDefault

        #region ToByte

        /// <summary>
        ///     An object extension method that converts the @this to a byte.
        /// </summary>
        /// <param name="this">The @this to act on.</param>
        /// <returns>@this as a byte.</returns>
        public static byte ToByte(this object @this)
        {
            return Convert.ToByte(@this);
        }

        #endregion ToByte

        #region ToByteOrDefault

        /// <summary>
        ///     An object extension method that converts this object to a byte or default.
        /// </summary>
        /// <param name="this">The @this to act on.</param>
        /// <returns>The given data converted to a byte.</returns>
        public static byte ToByteOrDefault(this object @this)
        {
            try
            {
                return Convert.ToByte(@this);
            }
            catch (Exception)
            {
                return default(byte);
            }
        }

        /// <summary>
        ///     An object extension method that converts this object to a byte or default.
        /// </summary>
        /// <param name="this">The @this to act on.</param>
        /// <param name="defaultValue">The default value.</param>
        /// <returns>The given data converted to a byte.</returns>
        public static byte ToByteOrDefault(this object @this, byte defaultValue)
        {
            try
            {
                return Convert.ToByte(@this);
            }
            catch (Exception)
            {
                return defaultValue;
            }
        }

        /// <summary>An object extension method that converts this object to a byte or default.</summary>
        /// <param name="this">The @this to act on.</param>
        /// <param name="defaultValue">The default value.</param>
        /// <param name="useDefaultIfNull">true to use default if null.</param>
        /// <returns>The given data converted to a byte.</returns>
        public static byte ToByteOrDefault(this object @this, byte defaultValue, bool useDefaultIfNull)
        {
            if (useDefaultIfNull && @this == null)
            {
                return defaultValue;
            }

            try
            {
                return Convert.ToByte(@this);
            }
            catch (Exception)
            {
                return defaultValue;
            }
        }

        /// <summary>
        ///     An object extension method that converts this object to a byte or default.
        /// </summary>
        /// <param name="this">The @this to act on.</param>
        /// <param name="defaultValueFactory">The default value factory.</param>
        /// <returns>The given data converted to a byte.</returns>
        public static byte ToByteOrDefault(this object @this, Func<byte> defaultValueFactory)
        {
            try
            {
                return Convert.ToByte(@this);
            }
            catch (Exception)
            {
                return defaultValueFactory();
            }
        }

        /// <summary>An object extension method that converts this object to a byte or default.</summary>
        /// <param name="this">The @this to act on.</param>
        /// <param name="defaultValueFactory">The default value factory.</param>
        /// <param name="useDefaultIfNull">true to use default if null.</param>
        /// <returns>The given data converted to a byte.</returns>
        public static byte ToByteOrDefault(this object @this, Func<byte> defaultValueFactory, bool useDefaultIfNull)
        {
            if (useDefaultIfNull && @this == null)
            {
                return defaultValueFactory();
            }

            try
            {
                return Convert.ToByte(@this);
            }
            catch (Exception)
            {
                return defaultValueFactory();
            }
        }

        #endregion ToByteOrDefault

        #region ToChar

        /// <summary>
        ///     An object extension method that converts the @this to a character.
        /// </summary>
        /// <param name="this">The @this to act on.</param>
        /// <returns>@this as a char.</returns>
        public static char ToChar(this object @this)
        {
            return Convert.ToChar(@this);
        }

        #endregion ToChar

        #region ToCharOrDefault

        /// <summary>
        ///     An object extension method that converts this object to a character or default.
        /// </summary>
        /// <param name="this">The @this to act on.</param>
        /// <returns>The given data converted to a char.</returns>
        public static char ToCharOrDefault(this object @this)
        {
            try
            {
                return Convert.ToChar(@this);
            }
            catch (Exception)
            {
                return default(char);
            }
        }

        /// <summary>
        ///     An object extension method that converts this object to a character or default.
        /// </summary>
        /// <param name="this">The @this to act on.</param>
        /// <param name="defaultValue">The default value.</param>
        /// <returns>The given data converted to a char.</returns>
        public static char ToCharOrDefault(this object @this, char defaultValue)
        {
            try
            {
                return Convert.ToChar(@this);
            }
            catch (Exception)
            {
                return defaultValue;
            }
        }

        /// <summary>
        /// An object extension method that converts this object to a character or default.
        /// </summary>
        /// <param name="this">The @this to act on.</param>
        /// <param name="defaultValue">The default value.</param>
        /// <param name="useDefaultIfNull">true to use default if null.</param>
        /// <returns>The given data converted to a char.</returns>
        public static char ToCharOrDefault(this object @this, char defaultValue, bool useDefaultIfNull)
        {
            if (useDefaultIfNull && @this == null)
            {
                return defaultValue;
            }

            try
            {
                return Convert.ToChar(@this);
            }
            catch (Exception)
            {
                return defaultValue;
            }
        }

        /// <summary>
        ///     An object extension method that converts this object to a character or default.
        /// </summary>
        /// <param name="this">The @this to act on.</param>
        /// <param name="defaultValueFactory">The default value factory.</param>
        /// <returns>The given data converted to a char.</returns>
        public static char ToCharOrDefault(this object @this, Func<char> defaultValueFactory)
        {
            try
            {
                return Convert.ToChar(@this);
            }
            catch (Exception)
            {
                return defaultValueFactory();
            }
        }

        /// <summary>
        /// An object extension method that converts this object to a character or default.
        /// </summary>
        /// <param name="this">The @this to act on.</param>
        /// <param name="defaultValueFactory">The default value factory.</param>
        /// <param name="useDefaultIfNull">true to use default if null.</param>
        /// <returns>The given data converted to a char.</returns>
        public static char ToCharOrDefault(this object @this, Func<char> defaultValueFactory, bool useDefaultIfNull)
        {
            if (useDefaultIfNull && @this == null)
            {
                return defaultValueFactory();
            }

            try
            {
                return Convert.ToChar(@this);
            }
            catch (Exception)
            {
                return defaultValueFactory();
            }
        }

        #endregion ToCharOrDefault

        #region ToDateTime

        /// <summary>
        ///     An object extension method that converts the @this to a date time.
        /// </summary>
        /// <param name="this">The @this to act on.</param>
        /// <returns>@this as a DateTime.</returns>
        public static DateTime ToDateTime(this object @this)
        {
            return Convert.ToDateTime(@this);
        }

        #endregion ToDateTime

        #region ToDateTimeOrDefault

        /// <summary>
        ///     An object extension method that converts this object to a date time or default.
        /// </summary>
        /// <param name="this">The @this to act on.</param>
        /// <returns>The given data converted to a DateTime.</returns>
        public static DateTime ToDateTimeOrDefault(this object @this)
        {
            try
            {
                return Convert.ToDateTime(@this);
            }
            catch (Exception)
            {
                return default(DateTime);
            }
        }

        /// <summary>
        ///     An object extension method that converts this object to a date time or default.
        /// </summary>
        /// <param name="this">The @this to act on.</param>
        /// <param name="defaultValue">The default value.</param>
        /// <returns>The given data converted to a DateTime.</returns>
        public static DateTime ToDateTimeOrDefault(this object @this, DateTime defaultValue)
        {
            try
            {
                return Convert.ToDateTime(@this);
            }
            catch (Exception)
            {
                return defaultValue;
            }
        }

        /// <summary>
        /// An object extension method that converts this object to a date time or default.
        /// </summary>
        /// <param name="this">The @this to act on.</param>
        /// <param name="defaultValue">The default value.</param>
        /// <param name="useDefaultIfNull">true to use default if null.</param>
        /// <returns>The given data converted to a DateTime.</returns>
        public static DateTime ToDateTimeOrDefault(this object @this, DateTime defaultValue, bool useDefaultIfNull)
        {
            if (useDefaultIfNull && @this == null)
            {
                return defaultValue;
            }

            try
            {
                return Convert.ToDateTime(@this);
            }
            catch (Exception)
            {
                return defaultValue;
            }
        }

        /// <summary>
        ///     An object extension method that converts this object to a date time or default.
        /// </summary>
        /// <param name="this">The @this to act on.</param>
        /// <param name="defaultValueFactory">The default value factory.</param>
        /// <returns>The given data converted to a DateTime.</returns>
        public static DateTime ToDateTimeOrDefault(this object @this, Func<DateTime> defaultValueFactory)
        {
            try
            {
                return Convert.ToDateTime(@this);
            }
            catch (Exception)
            {
                return defaultValueFactory();
            }
        }

        /// <summary>
        /// An object extension method that converts this object to a date time or default.
        /// </summary>
        /// <param name="this">The @this to act on.</param>
        /// <param name="defaultValueFactory">The default value factory.</param>
        /// <param name="useDefaultIfNull">true to use default if null.</param>
        /// <returns>The given data converted to a DateTime.</returns>
        public static DateTime ToDateTimeOrDefault(this object @this, Func<DateTime> defaultValueFactory, bool useDefaultIfNull)
        {
            if (useDefaultIfNull && @this == null)
            {
                return defaultValueFactory();
            }

            try
            {
                return Convert.ToDateTime(@this);
            }
            catch (Exception)
            {
                return defaultValueFactory();
            }
        }

        #endregion ToDateTimeOrDefault

        #region ToDateTimeOffSet

        /// <summary>
        ///     An object extension method that converts the @this to a date time off set.
        /// </summary>
        /// <param name="this">The @this to act on.</param>
        /// <returns>@this as a DateTimeOffset.</returns>
        public static DateTimeOffset ToDateTimeOffSet(this object @this)
        {
            return new DateTimeOffset(Convert.ToDateTime(@this), TimeSpan.Zero);
        }

        #endregion ToDateTimeOffSet

        #region ToDateTimeOffSetOrDefault

        /// <summary>
        ///     An object extension method that converts this object to a date time off set or default.
        /// </summary>
        /// <param name="this">The @this to act on.</param>
        /// <returns>The given data converted to a DateTimeOffset.</returns>
        public static DateTimeOffset ToDateTimeOffSetOrDefault(this object @this)
        {
            try
            {
                return new DateTimeOffset(Convert.ToDateTime(@this), TimeSpan.Zero);
            }
            catch (Exception)
            {
                return default(DateTimeOffset);
            }
        }

        /// <summary>
        ///     An object extension method that converts this object to a date time off set or default.
        /// </summary>
        /// <param name="this">The @this to act on.</param>
        /// <param name="defaultValue">The default value.</param>
        /// <returns>The given data converted to a DateTimeOffset.</returns>
        public static DateTimeOffset ToDateTimeOffSetOrDefault(this object @this, DateTimeOffset defaultValue)
        {
            try
            {
                return new DateTimeOffset(Convert.ToDateTime(@this), TimeSpan.Zero);
            }
            catch (Exception)
            {
                return defaultValue;
            }
        }

        /// <summary>
        /// An object extension method that converts this object to a date time off set or default.
        /// </summary>
        /// <param name="this">The @this to act on.</param>
        /// <param name="defaultValue">The default value.</param>
        /// <param name="useDefaultIfNull">true to use default if null.</param>
        /// <returns>The given data converted to a DateTimeOffset.</returns>
        public static DateTimeOffset ToDateTimeOffSetOrDefault(this object @this, DateTimeOffset defaultValue, bool useDefaultIfNull)
        {
            if (useDefaultIfNull && @this == null)
            {
                return defaultValue;
            }

            try
            {
                return new DateTimeOffset(Convert.ToDateTime(@this), TimeSpan.Zero);
            }
            catch (Exception)
            {
                return defaultValue;
            }
        }

        /// <summary>
        ///     An object extension method that converts this object to a date time off set or default.
        /// </summary>
        /// <param name="this">The @this to act on.</param>
        /// <param name="defaultValueFactory">The default value factory.</param>
        /// <returns>The given data converted to a DateTimeOffset.</returns>
        public static DateTimeOffset ToDateTimeOffSetOrDefault(this object @this, Func<DateTimeOffset> defaultValueFactory)
        {
            try
            {
                return new DateTimeOffset(Convert.ToDateTime(@this), TimeSpan.Zero);
            }
            catch (Exception)
            {
                return defaultValueFactory();
            }
        }

        /// <summary>
        /// An object extension method that converts this object to a date time off set or default.
        /// </summary>
        /// <param name="this">The @this to act on.</param>
        /// <param name="defaultValueFactory">The default value factory.</param>
        /// <param name="useDefaultIfNull">true to use default if null.</param>
        /// <returns>The given data converted to a DateTimeOffset.</returns>
        public static DateTimeOffset ToDateTimeOffSetOrDefault(this object @this, Func<DateTimeOffset> defaultValueFactory, bool useDefaultIfNull)
        {
            if (useDefaultIfNull && @this == null)
            {
                return defaultValueFactory();
            }

            try
            {
                return new DateTimeOffset(Convert.ToDateTime(@this), TimeSpan.Zero);
            }
            catch (Exception)
            {
                return defaultValueFactory();
            }
        }

        #endregion ToDateTimeOffSetOrDefault

        #region ToDecimal

        /// <summary>
        ///     An object extension method that converts the @this to a decimal.
        /// </summary>
        /// <param name="this">The @this to act on.</param>
        /// <returns>@this as a decimal.</returns>
        public static decimal ToDecimal(this object @this)
        {
            return Convert.ToDecimal(@this);
        }

        #endregion ToDecimal

        #region ToDecimalOrDefault

        /// <summary>
        ///     An object extension method that converts this object to a decimal or default.
        /// </summary>
        /// <param name="this">The @this to act on.</param>
        /// <returns>The given data converted to a decimal.</returns>
        public static decimal ToDecimalOrDefault(this object @this)
        {
            try
            {
                return Convert.ToDecimal(@this);
            }
            catch (Exception)
            {
                return default(decimal);
            }
        }

        /// <summary>
        ///     An object extension method that converts this object to a decimal or default.
        /// </summary>
        /// <param name="this">The @this to act on.</param>
        /// <param name="defaultValue">The default value.</param>
        /// <returns>The given data converted to a decimal.</returns>
        public static decimal ToDecimalOrDefault(this object @this, decimal defaultValue)
        {
            try
            {
                return Convert.ToDecimal(@this);
            }
            catch (Exception)
            {
                return defaultValue;
            }
        }

        /// <summary>
        /// An object extension method that converts this object to a decimal or default.
        /// </summary>
        /// <param name="this">The @this to act on.</param>
        /// <param name="defaultValue">The default value.</param>
        /// <param name="useDefaultIfNull">true to use default if null.</param>
        /// <returns>The given data converted to a decimal.</returns>
        public static decimal ToDecimalOrDefault(this object @this, decimal defaultValue, bool useDefaultIfNull)
        {
            if (useDefaultIfNull && @this == null)
            {
                return defaultValue;
            }

            try
            {
                return Convert.ToDecimal(@this);
            }
            catch (Exception)
            {
                return defaultValue;
            }
        }

        /// <summary>
        ///     An object extension method that converts this object to a decimal or default.
        /// </summary>
        /// <param name="this">The @this to act on.</param>
        /// <param name="defaultValueFactory">The default value factory.</param>
        /// <returns>The given data converted to a decimal.</returns>
        public static decimal ToDecimalOrDefault(this object @this, Func<decimal> defaultValueFactory)
        {
            try
            {
                return Convert.ToDecimal(@this);
            }
            catch (Exception)
            {
                return defaultValueFactory();
            }
        }

        /// <summary>
        /// An object extension method that converts this object to a decimal or default.
        /// </summary>
        /// <param name="this">The @this to act on.</param>
        /// <param name="defaultValueFactory">The default value factory.</param>
        /// <param name="useDefaultIfNull">true to use default if null.</param>
        /// <returns>The given data converted to a decimal.</returns>
        public static decimal ToDecimalOrDefault(this object @this, Func<decimal> defaultValueFactory, bool useDefaultIfNull)
        {
            if (useDefaultIfNull && @this == null)
            {
                return defaultValueFactory();
            }

            try
            {
                return Convert.ToDecimal(@this);
            }
            catch (Exception)
            {
                return defaultValueFactory();
            }
        }

        #endregion ToDecimalOrDefault

        #region ToDouble

        /// <summary>
        ///     An object extension method that converts the @this to a double.
        /// </summary>
        /// <param name="this">The @this to act on.</param>
        /// <returns>@this as a double.</returns>
        public static double ToDouble(this object @this)
        {
            return Convert.ToDouble(@this);
        }

        #endregion ToDouble

        #region ToDoubleOrDefault

        /// <summary>
        ///     An object extension method that converts this object to a double or default.
        /// </summary>
        /// <param name="this">The @this to act on.</param>
        /// <returns>The given data converted to a double.</returns>
        public static double ToDoubleOrDefault(this object @this)
        {
            try
            {
                return Convert.ToDouble(@this);
            }
            catch (Exception)
            {
                return default(double);
            }
        }

        /// <summary>
        ///     An object extension method that converts this object to a double or default.
        /// </summary>
        /// <param name="this">The @this to act on.</param>
        /// <param name="defaultValue">The default value.</param>
        /// <returns>The given data converted to a double.</returns>
        public static double ToDoubleOrDefault(this object @this, double defaultValue)
        {
            try
            {
                return Convert.ToDouble(@this);
            }
            catch (Exception)
            {
                return defaultValue;
            }
        }

        /// <summary>
        /// An object extension method that converts this object to a double or default.
        /// </summary>
        /// <param name="this">The @this to act on.</param>
        /// <param name="defaultValue">The default value.</param>
        /// <param name="useDefaultIfNull">true to use default if null.</param>
        /// <returns>The given data converted to a double.</returns>
        public static double ToDoubleOrDefault(this object @this, double defaultValue, bool useDefaultIfNull)
        {
            if (useDefaultIfNull && @this == null)
            {
                return defaultValue;
            }

            try
            {
                return Convert.ToDouble(@this);
            }
            catch (Exception)
            {
                return defaultValue;
            }
        }

        /// <summary>
        ///     An object extension method that converts this object to a double or default.
        /// </summary>
        /// <param name="this">The @this to act on.</param>
        /// <param name="defaultValueFactory">The default value factory.</param>
        /// <returns>The given data converted to a double.</returns>
        public static double ToDoubleOrDefault(this object @this, Func<double> defaultValueFactory)
        {
            try
            {
                return Convert.ToDouble(@this);
            }
            catch (Exception)
            {
                return defaultValueFactory();
            }
        }

        /// <summary>
        /// An object extension method that converts this object to a double or default.
        /// </summary>
        /// <param name="this">The @this to act on.</param>
        /// <param name="defaultValueFactory">The default value factory.</param>
        /// <param name="useDefaultIfNull">true to use default if null.</param>
        /// <returns>The given data converted to a double.</returns>
        public static double ToDoubleOrDefault(this object @this, Func<double> defaultValueFactory, bool useDefaultIfNull)
        {
            if (useDefaultIfNull && @this == null)
            {
                return defaultValueFactory();
            }

            try
            {
                return Convert.ToDouble(@this);
            }
            catch (Exception)
            {
                return defaultValueFactory();
            }
        }

        #endregion ToDoubleOrDefault

        #region ToFloat

        /// <summary>
        ///     An object extension method that converts the @this to a float.
        /// </summary>
        /// <param name="this">The @this to act on.</param>
        /// <returns>@this as a float.</returns>
        public static float ToFloat(this object @this)
        {
            return Convert.ToSingle(@this);
        }

        #endregion ToFloat

        #region ToFloatOrDefault

        /// <summary>
        ///     An object extension method that converts this object to a float or default.
        /// </summary>
        /// <param name="this">The @this to act on.</param>
        /// <returns>The given data converted to a float.</returns>
        public static float ToFloatOrDefault(this object @this)
        {
            try
            {
                return Convert.ToSingle(@this);
            }
            catch (Exception)
            {
                return default(float);
            }
        }

        /// <summary>
        ///     An object extension method that converts this object to a float or default.
        /// </summary>
        /// <param name="this">The @this to act on.</param>
        /// <param name="defaultValue">The default value.</param>
        /// <returns>The given data converted to a float.</returns>
        public static float ToFloatOrDefault(this object @this, float defaultValue)
        {
            try
            {
                return Convert.ToSingle(@this);
            }
            catch (Exception)
            {
                return defaultValue;
            }
        }

        /// <summary>
        /// An object extension method that converts this object to a float or default.
        /// </summary>
        /// <param name="this">The @this to act on.</param>
        /// <param name="defaultValue">The default value.</param>
        /// <param name="useDefaultIfNull">true to use default if null.</param>
        /// <returns>The given data converted to a float.</returns>
        public static float ToFloatOrDefault(this object @this, float defaultValue, bool useDefaultIfNull)
        {
            if (useDefaultIfNull && @this == null)
            {
                return defaultValue;
            }

            try
            {
                return Convert.ToSingle(@this);
            }
            catch (Exception)
            {
                return defaultValue;
            }
        }

        /// <summary>
        ///     An object extension method that converts this object to a float or default.
        /// </summary>
        /// <param name="this">The @this to act on.</param>
        /// <param name="defaultValueFactory">The default value factory.</param>
        /// <returns>The given data converted to a float.</returns>
        public static float ToFloatOrDefault(this object @this, Func<float> defaultValueFactory)
        {
            try
            {
                return Convert.ToSingle(@this);
            }
            catch (Exception)
            {
                return defaultValueFactory();
            }
        }

        /// <summary>
        /// An object extension method that converts this object to a float or default.
        /// </summary>
        /// <param name="this">The @this to act on.</param>
        /// <param name="defaultValueFactory">The default value factory.</param>
        /// <param name="useDefaultIfNull">true to use default if null.</param>
        /// <returns>The given data converted to a float.</returns>
        public static float ToFloatOrDefault(this object @this, Func<float> defaultValueFactory, bool useDefaultIfNull)
        {
            if (useDefaultIfNull && @this == null)
            {
                return defaultValueFactory();
            }

            try
            {
                return Convert.ToSingle(@this);
            }
            catch (Exception)
            {
                return defaultValueFactory();
            }
        }

        #endregion ToFloatOrDefault

        #region ToGuid

        /// <summary>
        ///     An object extension method that converts the @this to a unique identifier.
        /// </summary>
        /// <param name="this">The @this to act on.</param>
        /// <returns>@this as a GUID.</returns>
        public static Guid ToGuid(this object @this)
        {
            return new Guid(@this.ToString());
        }

        #endregion ToGuid

        #region ToGuidOrDefault

        /// <summary>
        ///     An object extension method that converts this object to a unique identifier or default.
        /// </summary>
        /// <param name="this">The @this to act on.</param>
        /// <returns>The given data converted to a GUID.</returns>
        public static Guid ToGuidOrDefault(this object @this)
        {
            try
            {
                return new Guid(@this.ToString());
            }
            catch (Exception)
            {
                return Guid.Empty;
            }
        }

        /// <summary>
        ///     An object extension method that converts this object to a unique identifier or default.
        /// </summary>
        /// <param name="this">The @this to act on.</param>
        /// <param name="defaultValue">The default value.</param>
        /// <returns>The given data converted to a GUID.</returns>
        public static Guid ToGuidOrDefault(this object @this, Guid defaultValue)
        {
            try
            {
                return new Guid(@this.ToString());
            }
            catch (Exception)
            {
                return defaultValue;
            }
        }

        /// <summary>
        /// An object extension method that converts this object to a unique identifier or default.
        /// </summary>
        /// <param name="this">The @this to act on.</param>
        /// <param name="defaultValue">The default value.</param>
        /// <param name="useDefaultIfNull">true to use default if null.</param>
        /// <returns>The given data converted to a GUID.</returns>
        public static Guid ToGuidOrDefault(this object @this, Guid defaultValue, bool useDefaultIfNull)
        {
            if (useDefaultIfNull && @this == null)
            {
                return defaultValue;
            }

            try
            {
                return new Guid(@this.ToString());
            }
            catch (Exception)
            {
                return defaultValue;
            }
        }

        /// <summary>
        ///     An object extension method that converts this object to a unique identifier or default.
        /// </summary>
        /// <param name="this">The @this to act on.</param>
        /// <param name="defaultValueFactory">The default value factory.</param>
        /// <returns>The given data converted to a GUID.</returns>
        public static Guid ToGuidOrDefault(this object @this, Func<Guid> defaultValueFactory)
        {
            try
            {
                return new Guid(@this.ToString());
            }
            catch (Exception)
            {
                return defaultValueFactory();
            }
        }

        /// <summary>
        /// An object extension method that converts this object to a unique identifier or default.
        /// </summary>
        /// <param name="this">The @this to act on.</param>
        /// <param name="defaultValueFactory">The default value factory.</param>
        /// <param name="useDefaultIfNull">true to use default if null.</param>
        /// <returns>The given data converted to a GUID.</returns>
        public static Guid ToGuidOrDefault(this object @this, Func<Guid> defaultValueFactory, bool useDefaultIfNull)
        {
            if (useDefaultIfNull && @this == null)
            {
                return defaultValueFactory();
            }

            try
            {
                return new Guid(@this.ToString());
            }
            catch (Exception)
            {
                return defaultValueFactory();
            }
        }

        #endregion ToGuidOrDefault

        #region ToInt16

        /// <summary>
        ///     An object extension method that converts the @this to an int 16.
        /// </summary>
        /// <param name="this">The @this to act on.</param>
        /// <returns>@this as a short.</returns>
        public static short ToInt16(this object @this)
        {
            return Convert.ToInt16(@this);
        }

        #endregion ToInt16

        #region ToInt16OrDefault

        /// <summary>
        ///     An object extension method that converts this object to an int 16 or default.
        /// </summary>
        /// <param name="this">The @this to act on.</param>
        /// <returns>The given data converted to a short.</returns>
        public static short ToInt16OrDefault(this object @this)
        {
            try
            {
                return Convert.ToInt16(@this);
            }
            catch (Exception)
            {
                return default(short);
            }
        }

        /// <summary>
        ///     An object extension method that converts this object to an int 16 or default.
        /// </summary>
        /// <param name="this">The @this to act on.</param>
        /// <param name="defaultValue">The default value.</param>
        /// <returns>The given data converted to a short.</returns>
        public static short ToInt16OrDefault(this object @this, short defaultValue)
        {
            try
            {
                return Convert.ToInt16(@this);
            }
            catch (Exception)
            {
                return defaultValue;
            }
        }

        /// <summary>
        /// An object extension method that converts this object to an int 16 or default.
        /// </summary>
        /// <param name="this">The @this to act on.</param>
        /// <param name="defaultValue">The default value.</param>
        /// <param name="useDefaultIfNull">true to use default if null.</param>
        /// <returns>The given data converted to a short.</returns>
        public static short ToInt16OrDefault(this object @this, short defaultValue, bool useDefaultIfNull)
        {
            if (useDefaultIfNull && @this == null)
            {
                return defaultValue;
            }

            try
            {
                return Convert.ToInt16(@this);
            }
            catch (Exception)
            {
                return defaultValue;
            }
        }

        /// <summary>
        ///     An object extension method that converts this object to an int 16 or default.
        /// </summary>
        /// <param name="this">The @this to act on.</param>
        /// <param name="defaultValueFactory">The default value factory.</param>
        /// <returns>The given data converted to a short.</returns>
        public static short ToInt16OrDefault(this object @this, Func<short> defaultValueFactory)
        {
            try
            {
                return Convert.ToInt16(@this);
            }
            catch (Exception)
            {
                return defaultValueFactory();
            }
        }

        /// <summary>
        /// An object extension method that converts this object to an int 16 or default.
        /// </summary>
        /// <param name="this">The @this to act on.</param>
        /// <param name="defaultValueFactory">The default value factory.</param>
        /// <param name="useDefaultIfNull">true to use default if null.</param>
        /// <returns>The given data converted to a short.</returns>
        public static short ToInt16OrDefault(this object @this, Func<short> defaultValueFactory, bool useDefaultIfNull)
        {
            if (useDefaultIfNull && @this == null)
            {
                return defaultValueFactory();
            }

            try
            {
                return Convert.ToInt16(@this);
            }
            catch (Exception)
            {
                return defaultValueFactory();
            }
        }

        #endregion ToInt16OrDefault

        #region ToInt32

        /// <summary>
        ///     An object extension method that converts the @this to an int 32.
        /// </summary>
        /// <param name="this">The @this to act on.</param>
        /// <returns>@this as an int.</returns>
        public static int ToInt32(this object @this)
        {
            return Convert.ToInt32(@this);
        }

        #endregion ToInt32

        #region ToInt32OrDefault

        /// <summary>
        ///     An object extension method that converts this object to an int 32 or default.
        /// </summary>
        /// <param name="this">The @this to act on.</param>
        /// <returns>The given data converted to an int.</returns>
        public static int ToInt32OrDefault(this object @this)
        {
            try
            {
                return Convert.ToInt32(@this);
            }
            catch (Exception)
            {
                return default(int);
            }
        }

        /// <summary>
        ///     An object extension method that converts this object to an int 32 or default.
        /// </summary>
        /// <param name="this">The @this to act on.</param>
        /// <param name="defaultValue">The default value.</param>
        /// <returns>The given data converted to an int.</returns>
        public static int ToInt32OrDefault(this object @this, int defaultValue)
        {
            try
            {
                return Convert.ToInt32(@this);
            }
            catch (Exception)
            {
                return defaultValue;
            }
        }

        /// <summary>
        /// An object extension method that converts this object to an int 32 or default.
        /// </summary>
        /// <param name="this">The @this to act on.</param>
        /// <param name="defaultValue">The default value.</param>
        /// <param name="useDefaultIfNull">true to use default if null.</param>
        /// <returns>The given data converted to an int.</returns>
        public static int ToInt32OrDefault(this object @this, int defaultValue, bool useDefaultIfNull)
        {
            if (useDefaultIfNull && @this == null)
            {
                return defaultValue;
            }

            try
            {
                return Convert.ToInt32(@this);
            }
            catch (Exception)
            {
                return defaultValue;
            }
        }

        /// <summary>
        ///     An object extension method that converts this object to an int 32 or default.
        /// </summary>
        /// <param name="this">The @this to act on.</param>
        /// <param name="defaultValueFactory">The default value factory.</param>
        /// <returns>The given data converted to an int.</returns>
        public static int ToInt32OrDefault(this object @this, Func<int> defaultValueFactory)
        {
            try
            {
                return Convert.ToInt32(@this);
            }
            catch (Exception)
            {
                return defaultValueFactory();
            }
        }

        /// <summary>
        /// An object extension method that converts this object to an int 32 or default.
        /// </summary>
        /// <param name="this">The @this to act on.</param>
        /// <param name="defaultValueFactory">The default value factory.</param>
        /// <param name="useDefaultIfNull">true to use default if null.</param>
        /// <returns>The given data converted to an int.</returns>
        public static int ToInt32OrDefault(this object @this, Func<int> defaultValueFactory, bool useDefaultIfNull)
        {
            if (useDefaultIfNull && @this == null)
            {
                return defaultValueFactory();
            }

            try
            {
                return Convert.ToInt32(@this);
            }
            catch (Exception)
            {
                return defaultValueFactory();
            }
        }

        #endregion ToInt32OrDefault

        #region ToInt64

        /// <summary>
        ///     An object extension method that converts the @this to an int 64.
        /// </summary>
        /// <param name="this">The @this to act on.</param>
        /// <returns>@this as a long.</returns>
        public static long ToInt64(this object @this)
        {
            return Convert.ToInt64(@this);
        }

        #endregion ToInt64

        #region ToInt64OrDefault

        /// <summary>
        ///     An object extension method that converts this object to an int 64 or default.
        /// </summary>
        /// <param name="this">The @this to act on.</param>
        /// <returns>The given data converted to a long.</returns>
        public static long ToInt64OrDefault(this object @this)
        {
            try
            {
                return Convert.ToInt64(@this);
            }
            catch (Exception)
            {
                return default(long);
            }
        }

        /// <summary>
        ///     An object extension method that converts this object to an int 64 or default.
        /// </summary>
        /// <param name="this">The @this to act on.</param>
        /// <param name="defaultValue">The default value.</param>
        /// <returns>The given data converted to a long.</returns>
        public static long ToInt64OrDefault(this object @this, long defaultValue)
        {
            try
            {
                return Convert.ToInt64(@this);
            }
            catch (Exception)
            {
                return defaultValue;
            }
        }

        /// <summary>
        /// An object extension method that converts this object to an int 64 or default.
        /// </summary>
        /// <param name="this">The @this to act on.</param>
        /// <param name="defaultValue">The default value.</param>
        /// <param name="useDefaultIfNull">true to use default if null.</param>
        /// <returns>The given data converted to a long.</returns>
        public static long ToInt64OrDefault(this object @this, long defaultValue, bool useDefaultIfNull)
        {
            if (useDefaultIfNull && @this == null)
            {
                return defaultValue;
            }

            try
            {
                return Convert.ToInt64(@this);
            }
            catch (Exception)
            {
                return defaultValue;
            }
        }

        /// <summary>
        ///     An object extension method that converts this object to an int 64 or default.
        /// </summary>
        /// <param name="this">The @this to act on.</param>
        /// <param name="defaultValueFactory">The default value factory.</param>
        /// <returns>The given data converted to a long.</returns>
        public static long ToInt64OrDefault(this object @this, Func<long> defaultValueFactory)
        {
            try
            {
                return Convert.ToInt64(@this);
            }
            catch (Exception)
            {
                return defaultValueFactory();
            }
        }

        /// <summary>
        /// An object extension method that converts this object to an int 64 or default.
        /// </summary>
        /// <param name="this">The @this to act on.</param>
        /// <param name="defaultValueFactory">The default value factory.</param>
        /// <param name="useDefaultIfNull">true to use default if null.</param>
        /// <returns>The given data converted to a long.</returns>
        public static long ToInt64OrDefault(this object @this, Func<long> defaultValueFactory, bool useDefaultIfNull)
        {
            if (useDefaultIfNull && @this == null)
            {
                return defaultValueFactory();
            }

            try
            {
                return Convert.ToInt64(@this);
            }
            catch (Exception)
            {
                return defaultValueFactory();
            }
        }

        #endregion ToInt64OrDefault

        #region ToLong

        /// <summary>
        ///     An object extension method that converts the @this to a long.
        /// </summary>
        /// <param name="this">The @this to act on.</param>
        /// <returns>@this as a long.</returns>
        public static long ToLong(this object @this)
        {
            return Convert.ToInt64(@this);
        }

        #endregion ToLong

        #region ToLongOrDefault

        /// <summary>
        ///     An object extension method that converts this object to a long or default.
        /// </summary>
        /// <param name="this">The @this to act on.</param>
        /// <returns>The given data converted to a long.</returns>
        public static long ToLongOrDefault(this object @this)
        {
            try
            {
                return Convert.ToInt64(@this);
            }
            catch (Exception)
            {
                return default(long);
            }
        }

        /// <summary>
        ///     An object extension method that converts this object to a long or default.
        /// </summary>
        /// <param name="this">The @this to act on.</param>
        /// <param name="defaultValue">The default value.</param>
        /// <returns>The given data converted to a long.</returns>
        public static long ToLongOrDefault(this object @this, long defaultValue)
        {
            try
            {
                return Convert.ToInt64(@this);
            }
            catch (Exception)
            {
                return defaultValue;
            }
        }

        /// <summary>An object extension method that converts this object to a long or default.</summary>
        /// <param name="this">The @this to act on.</param>
        /// <param name="defaultValue">The default value.</param>
        /// <param name="useDefaultIfNull">true to use default if null.</param>
        /// <returns>The given data converted to a long.</returns>
        public static long ToLongOrDefault(this object @this, long defaultValue, bool useDefaultIfNull)
        {
            if (useDefaultIfNull && @this == null)
            {
                return defaultValue;
            }

            try
            {
                return Convert.ToInt64(@this);
            }
            catch (Exception)
            {
                return defaultValue;
            }
        }

        /// <summary>
        ///     An object extension method that converts this object to a long or default.
        /// </summary>
        /// <param name="this">The @this to act on.</param>
        /// <param name="defaultValueFactory">The default value factory.</param>
        /// <returns>The given data converted to a long.</returns>
        public static long ToLongOrDefault(this object @this, Func<long> defaultValueFactory)
        {
            try
            {
                return Convert.ToInt64(@this);
            }
            catch (Exception)
            {
                return defaultValueFactory();
            }
        }

        /// <summary>An object extension method that converts this object to a long or default.</summary>
        /// <param name="this">The @this to act on.</param>
        /// <param name="defaultValueFactory">The default value factory.</param>
        /// <param name="useDefaultIfNull">true to use default if null.</param>
        /// <returns>The given data converted to a long.</returns>
        public static long ToLongOrDefault(this object @this, Func<long> defaultValueFactory, bool useDefaultIfNull)
        {
            if (useDefaultIfNull && @this == null)
            {
                return defaultValueFactory();
            }

            try
            {
                return Convert.ToInt64(@this);
            }
            catch (Exception)
            {
                return defaultValueFactory();
            }
        }

        #endregion ToLongOrDefault

        #region ToNullableBoolean

        /// <summary>
        ///     An object extension method that converts the @this to a nullable boolean.
        /// </summary>
        /// <param name="this">The @this to act on.</param>
        /// <returns>@this as a bool?</returns>
        public static bool? ToNullableBoolean(this object @this)
        {
            if (@this == null || @this == DBNull.Value)
            {
                return null;
            }

            return Convert.ToBoolean(@this);
        }

        #endregion ToNullableBoolean

        #region ToNullableBooleanOrDefault

        /// <summary>
        ///     An object extension method that converts this object to a nullable boolean or default.
        /// </summary>
        /// <param name="this">The @this to act on.</param>
        /// <returns>The given data converted to a bool?</returns>
        public static bool? ToNullableBooleanOrDefault(this object @this)
        {
            try
            {
                if (@this == null || @this == DBNull.Value)
                {
                    return null;
                }

                return Convert.ToBoolean(@this);
            }
            catch (Exception)
            {
                return default(bool);
            }
        }

        /// <summary>
        ///     An object extension method that converts this object to a nullable boolean or default.
        /// </summary>
        /// <param name="this">The @this to act on.</param>
        /// <param name="defaultValue">The default value.</param>
        /// <returns>The given data converted to a bool?</returns>
        public static bool? ToNullableBooleanOrDefault(this object @this, bool? defaultValue)
        {
            try
            {
                if (@this == null || @this == DBNull.Value)
                {
                    return null;
                }

                return Convert.ToBoolean(@this);
            }
            catch (Exception)
            {
                return defaultValue;
            }
        }

        /// <summary>
        ///     An object extension method that converts this object to a nullable boolean or default.
        /// </summary>
        /// <param name="this">The @this to act on.</param>
        /// <param name="defaultValueFactory">The default value factory.</param>
        /// <returns>The given data converted to a bool?</returns>
        public static bool? ToNullableBooleanOrDefault(this object @this, Func<bool?> defaultValueFactory)
        {
            try
            {
                if (@this == null || @this == DBNull.Value)
                {
                    return null;
                }

                return Convert.ToBoolean(@this);
            }
            catch (Exception)
            {
                return defaultValueFactory();
            }
        }

        #endregion ToNullableBooleanOrDefault

        #region ToNullableByte

        /// <summary>
        ///     An object extension method that converts the @this to a nullable byte.
        /// </summary>
        /// <param name="this">The @this to act on.</param>
        /// <returns>@this as a byte?</returns>
        public static byte? ToNullableByte(this object @this)
        {
            if (@this == null || @this == DBNull.Value)
            {
                return null;
            }

            return Convert.ToByte(@this);
        }

        #endregion ToNullableByte

        #region ToNullableByteOrDefault

        /// <summary>
        ///     An object extension method that converts this object to a nullable byte or default.
        /// </summary>
        /// <param name="this">The @this to act on.</param>
        /// <returns>The given data converted to a byte?</returns>
        public static byte? ToNullableByteOrDefault(this object @this)
        {
            try
            {
                if (@this == null || @this == DBNull.Value)
                {
                    return null;
                }

                return Convert.ToByte(@this);
            }
            catch (Exception)
            {
                return default(byte);
            }
        }

        /// <summary>
        ///     An object extension method that converts this object to a nullable byte or default.
        /// </summary>
        /// <param name="this">The @this to act on.</param>
        /// <param name="defaultValue">The default value.</param>
        /// <returns>The given data converted to a byte?</returns>
        public static byte? ToNullableByteOrDefault(this object @this, byte? defaultValue)
        {
            try
            {
                if (@this == null || @this == DBNull.Value)
                {
                    return null;
                }

                return Convert.ToByte(@this);
            }
            catch (Exception)
            {
                return defaultValue;
            }
        }

        /// <summary>
        ///     An object extension method that converts this object to a nullable byte or default.
        /// </summary>
        /// <param name="this">The @this to act on.</param>
        /// <param name="defaultValueFactory">The default value factory.</param>
        /// <returns>The given data converted to a byte?</returns>
        public static byte? ToNullableByteOrDefault(this object @this, Func<byte?> defaultValueFactory)
        {
            try
            {
                if (@this == null || @this == DBNull.Value)
                {
                    return null;
                }

                return Convert.ToByte(@this);
            }
            catch (Exception)
            {
                return defaultValueFactory();
            }
        }

        #endregion ToNullableByteOrDefault

        #region ToNullableChar

        /// <summary>
        ///     An object extension method that converts the @this to a nullable character.
        /// </summary>
        /// <param name="this">The @this to act on.</param>
        /// <returns>@this as a char?</returns>
        public static char? ToNullableChar(this object @this)
        {
            if (@this == null || @this == DBNull.Value)
            {
                return null;
            }

            return Convert.ToChar(@this);
        }

        #endregion ToNullableChar

        #region ToNullableCharOrDefault

        /// <summary>
        ///     An object extension method that converts this object to a nullable character or default.
        /// </summary>
        /// <param name="this">The @this to act on.</param>
        /// <returns>The given data converted to a char?</returns>
        public static char? ToNullableCharOrDefault(this object @this)
        {
            try
            {
                if (@this == null || @this == DBNull.Value)
                {
                    return null;
                }

                return Convert.ToChar(@this);
            }
            catch (Exception)
            {
                return default(char);
            }
        }

        /// <summary>
        ///     An object extension method that converts this object to a nullable character or default.
        /// </summary>
        /// <param name="this">The @this to act on.</param>
        /// <param name="defaultValue">The default value.</param>
        /// <returns>The given data converted to a char?</returns>
        public static char? ToNullableCharOrDefault(this object @this, char? defaultValue)
        {
            try
            {
                if (@this == null || @this == DBNull.Value)
                {
                    return null;
                }

                return Convert.ToChar(@this);
            }
            catch (Exception)
            {
                return defaultValue;
            }
        }

        /// <summary>
        ///     An object extension method that converts this object to a nullable character or default.
        /// </summary>
        /// <param name="this">The @this to act on.</param>
        /// <param name="defaultValueFactory">The default value factory.</param>
        /// <returns>The given data converted to a char?</returns>
        public static char? ToNullableCharOrDefault(this object @this, Func<char?> defaultValueFactory)
        {
            try
            {
                if (@this == null || @this == DBNull.Value)
                {
                    return null;
                }

                return Convert.ToChar(@this);
            }
            catch (Exception)
            {
                return defaultValueFactory();
            }
        }

        #endregion ToNullableCharOrDefault

        #region ToNullableDateTime

        /// <summary>
        ///     An object extension method that converts the @this to a nullable date time.
        /// </summary>
        /// <param name="this">The @this to act on.</param>
        /// <returns>@this as a DateTime?</returns>
        public static DateTime? ToNullableDateTime(this object @this)
        {
            if (@this == null || @this == DBNull.Value)
            {
                return null;
            }

            return Convert.ToDateTime(@this);
        }

        #endregion ToNullableDateTime

        #region ToNullableDateTimeOrDefault

        /// <summary>
        ///     An object extension method that converts this object to a nullable date time or default.
        /// </summary>
        /// <param name="this">The @this to act on.</param>
        /// <returns>The given data converted to a DateTime?</returns>
        public static DateTime? ToNullableDateTimeOrDefault(this object @this)
        {
            try
            {
                if (@this == null || @this == DBNull.Value)
                {
                    return null;
                }

                return Convert.ToDateTime(@this);
            }
            catch (Exception)
            {
                return default(DateTime);
            }
        }

        /// <summary>
        ///     An object extension method that converts this object to a nullable date time or default.
        /// </summary>
        /// <param name="this">The @this to act on.</param>
        /// <param name="defaultValue">The default value.</param>
        /// <returns>The given data converted to a DateTime?</returns>
        public static DateTime? ToNullableDateTimeOrDefault(this object @this, DateTime? defaultValue)
        {
            try
            {
                if (@this == null || @this == DBNull.Value)
                {
                    return null;
                }

                return Convert.ToDateTime(@this);
            }
            catch (Exception)
            {
                return defaultValue;
            }
        }

        /// <summary>
        ///     An object extension method that converts this object to a nullable date time or default.
        /// </summary>
        /// <param name="this">The @this to act on.</param>
        /// <param name="defaultValueFactory">The default value factory.</param>
        /// <returns>The given data converted to a DateTime?</returns>
        public static DateTime? ToNullableDateTimeOrDefault(this object @this, Func<DateTime?> defaultValueFactory)
        {
            try
            {
                if (@this == null || @this == DBNull.Value)
                {
                    return null;
                }

                return Convert.ToDateTime(@this);
            }
            catch (Exception)
            {
                return defaultValueFactory();
            }
        }

        #endregion ToNullableDateTimeOrDefault

        #region ToNullableDateTimeOffSet

        /// <summary>
        ///     An object extension method that converts the @this to a nullable date time off set.
        /// </summary>
        /// <param name="this">The @this to act on.</param>
        /// <returns>@this as a DateTimeOffset?</returns>
        public static DateTimeOffset? ToNullableDateTimeOffSet(this object @this)
        {
            if (@this == null || @this == DBNull.Value)
            {
                return null;
            }

            return new DateTimeOffset(Convert.ToDateTime(@this), TimeSpan.Zero);
        }

        #endregion ToNullableDateTimeOffSet

        #region ToNullableDateTimeOffSetOrDefault

        /// <summary>
        ///     An object extension method that converts this object to a nullable date time off set or default.
        /// </summary>
        /// <param name="this">The @this to act on.</param>
        /// <returns>The given data converted to a DateTimeOffset?</returns>
        public static DateTimeOffset? ToNullableDateTimeOffSetOrDefault(this object @this)
        {
            try
            {
                if (@this == null || @this == DBNull.Value)
                {
                    return null;
                }

                return new DateTimeOffset(Convert.ToDateTime(@this), TimeSpan.Zero);
            }
            catch (Exception)
            {
                return default(DateTimeOffset);
            }
        }

        /// <summary>
        ///     An object extension method that converts this object to a nullable date time off set or default.
        /// </summary>
        /// <param name="this">The @this to act on.</param>
        /// <param name="defaultValue">The default value.</param>
        /// <returns>The given data converted to a DateTimeOffset?</returns>
        public static DateTimeOffset? ToNullableDateTimeOffSetOrDefault(this object @this, DateTimeOffset? defaultValue)
        {
            try
            {
                if (@this == null || @this == DBNull.Value)
                {
                    return null;
                }

                return new DateTimeOffset(Convert.ToDateTime(@this), TimeSpan.Zero);
            }
            catch (Exception)
            {
                return defaultValue;
            }
        }

        /// <summary>
        ///     An object extension method that converts this object to a nullable date time off set or default.
        /// </summary>
        /// <param name="this">The @this to act on.</param>
        /// <param name="defaultValueFactory">The default value factory.</param>
        /// <returns>The given data converted to a DateTimeOffset?</returns>
        public static DateTimeOffset? ToNullableDateTimeOffSetOrDefault(this object @this, Func<DateTimeOffset?> defaultValueFactory)
        {
            try
            {
                if (@this == null || @this == DBNull.Value)
                {
                    return null;
                }

                return new DateTimeOffset(Convert.ToDateTime(@this), TimeSpan.Zero);
            }
            catch (Exception)
            {
                return defaultValueFactory();
            }
        }

        #endregion ToNullableDateTimeOffSetOrDefault

        #region ToNullableDecimal

        /// <summary>
        ///     An object extension method that converts the @this to a nullable decimal.
        /// </summary>
        /// <param name="this">The @this to act on.</param>
        /// <returns>@this as a decimal?</returns>
        public static decimal? ToNullableDecimal(this object @this)
        {
            if (@this == null || @this == DBNull.Value)
            {
                return null;
            }

            return Convert.ToDecimal(@this);
        }

        #endregion ToNullableDecimal

        #region ToNullableDecimalOrDefault

        /// <summary>
        ///     An object extension method that converts this object to a nullable decimal or default.
        /// </summary>
        /// <param name="this">The @this to act on.</param>
        /// <returns>The given data converted to a decimal?</returns>
        public static decimal? ToNullableDecimalOrDefault(this object @this)
        {
            try
            {
                if (@this == null || @this == DBNull.Value)
                {
                    return null;
                }

                return Convert.ToDecimal(@this);
            }
            catch (Exception)
            {
                return default(decimal);
            }
        }

        /// <summary>
        ///     An object extension method that converts this object to a nullable decimal or default.
        /// </summary>
        /// <param name="this">The @this to act on.</param>
        /// <param name="defaultValue">The default value.</param>
        /// <returns>The given data converted to a decimal?</returns>
        public static decimal? ToNullableDecimalOrDefault(this object @this, decimal? defaultValue)
        {
            try
            {
                if (@this == null || @this == DBNull.Value)
                {
                    return null;
                }

                return Convert.ToDecimal(@this);
            }
            catch (Exception)
            {
                return defaultValue;
            }
        }

        /// <summary>
        ///     An object extension method that converts this object to a nullable decimal or default.
        /// </summary>
        /// <param name="this">The @this to act on.</param>
        /// <param name="defaultValueFactory">The default value factory.</param>
        /// <returns>The given data converted to a decimal?</returns>
        public static decimal? ToNullableDecimalOrDefault(this object @this, Func<decimal?> defaultValueFactory)
        {
            try
            {
                if (@this == null || @this == DBNull.Value)
                {
                    return null;
                }

                return Convert.ToDecimal(@this);
            }
            catch (Exception)
            {
                return defaultValueFactory();
            }
        }

        #endregion ToNullableDecimalOrDefault

        #region ToNullableDouble

        /// <summary>
        ///     An object extension method that converts the @this to a nullable double.
        /// </summary>
        /// <param name="this">The @this to act on.</param>
        /// <returns>@this as a double?</returns>
        public static double? ToNullableDouble(this object @this)
        {
            if (@this == null || @this == DBNull.Value)
            {
                return null;
            }

            return Convert.ToDouble(@this);
        }

        #endregion ToNullableDouble

        #region ToNullableDoubleOrDefault

        /// <summary>
        ///     An object extension method that converts this object to a nullable double or default.
        /// </summary>
        /// <param name="this">The @this to act on.</param>
        /// <returns>The given data converted to a double?</returns>
        public static double? ToNullableDoubleOrDefault(this object @this)
        {
            try
            {
                if (@this == null || @this == DBNull.Value)
                {
                    return null;
                }

                return Convert.ToDouble(@this);
            }
            catch (Exception)
            {
                return default(double);
            }
        }

        /// <summary>
        ///     An object extension method that converts this object to a nullable double or default.
        /// </summary>
        /// <param name="this">The @this to act on.</param>
        /// <param name="defaultValue">The default value.</param>
        /// <returns>The given data converted to a double?</returns>
        public static double? ToNullableDoubleOrDefault(this object @this, double? defaultValue)
        {
            try
            {
                if (@this == null || @this == DBNull.Value)
                {
                    return null;
                }

                return Convert.ToDouble(@this);
            }
            catch (Exception)
            {
                return defaultValue;
            }
        }

        /// <summary>
        ///     An object extension method that converts this object to a nullable double or default.
        /// </summary>
        /// <param name="this">The @this to act on.</param>
        /// <param name="defaultValueFactory">The default value factory.</param>
        /// <returns>The given data converted to a double?</returns>
        public static double? ToNullableDoubleOrDefault(this object @this, Func<double?> defaultValueFactory)
        {
            try
            {
                if (@this == null || @this == DBNull.Value)
                {
                    return null;
                }

                return Convert.ToDouble(@this);
            }
            catch (Exception)
            {
                return defaultValueFactory();
            }
        }

        #endregion ToNullableDoubleOrDefault

        #region ToNullableFloat

        /// <summary>
        ///     An object extension method that converts the @this to a nullable float.
        /// </summary>
        /// <param name="this">The @this to act on.</param>
        /// <returns>@this as a float?</returns>
        public static float? ToNullableFloat(this object @this)
        {
            if (@this == null || @this == DBNull.Value)
            {
                return null;
            }

            return Convert.ToSingle(@this);
        }

        #endregion ToNullableFloat

        #region ToNullableFloatOrDefault

        /// <summary>
        ///     An object extension method that converts this object to a nullable float or default.
        /// </summary>
        /// <param name="this">The @this to act on.</param>
        /// <returns>The given data converted to a float?</returns>
        public static float? ToNullableFloatOrDefault(this object @this)
        {
            try
            {
                if (@this == null || @this == DBNull.Value)
                {
                    return null;
                }

                return Convert.ToSingle(@this);
            }
            catch (Exception)
            {
                return default(float);
            }
        }

        /// <summary>
        ///     An object extension method that converts this object to a nullable float or default.
        /// </summary>
        /// <param name="this">The @this to act on.</param>
        /// <param name="defaultValue">The default value.</param>
        /// <returns>The given data converted to a float?</returns>
        public static float? ToNullableFloatOrDefault(this object @this, float? defaultValue)
        {
            try
            {
                if (@this == null || @this == DBNull.Value)
                {
                    return null;
                }

                return Convert.ToSingle(@this);
            }
            catch (Exception)
            {
                return defaultValue;
            }
        }

        /// <summary>
        ///     An object extension method that converts this object to a nullable float or default.
        /// </summary>
        /// <param name="this">The @this to act on.</param>
        /// <param name="defaultValueFactory">The default value factory.</param>
        /// <returns>The given data converted to a float?</returns>
        public static float? ToNullableFloatOrDefault(this object @this, Func<float?> defaultValueFactory)
        {
            try
            {
                if (@this == null || @this == DBNull.Value)
                {
                    return null;
                }

                return Convert.ToSingle(@this);
            }
            catch (Exception)
            {
                return defaultValueFactory();
            }
        }

        #endregion ToNullableFloatOrDefault

        #region ToNullableGuid

        /// <summary>
        ///     An object extension method that converts the @this to a nullable unique identifier.
        /// </summary>
        /// <param name="this">The @this to act on.</param>
        /// <returns>@this as a Guid?</returns>
        public static Guid? ToNullableGuid(this object @this)
        {
            if (@this == null || @this == DBNull.Value)
            {
                return null;
            }

            return new Guid(@this.ToString());
        }

        #endregion ToNullableGuid

        #region ToNullableGuidOrDefault

        /// <summary>
        ///     An object extension method that converts this object to a nullable unique identifier or default.
        /// </summary>
        /// <param name="this">The @this to act on.</param>
        /// <returns>The given data converted to a Guid?</returns>
        public static Guid? ToNullableGuidOrDefault(this object @this)
        {
            try
            {
                if (@this == null || @this == DBNull.Value)
                {
                    return null;
                }

                return new Guid(@this.ToString());
            }
            catch (Exception)
            {
                return Guid.Empty;
            }
        }

        /// <summary>
        ///     An object extension method that converts this object to a nullable unique identifier or default.
        /// </summary>
        /// <param name="this">The @this to act on.</param>
        /// <param name="defaultValue">The default value.</param>
        /// <returns>The given data converted to a Guid?</returns>
        public static Guid? ToNullableGuidOrDefault(this object @this, Guid? defaultValue)
        {
            try
            {
                if (@this == null || @this == DBNull.Value)
                {
                    return null;
                }

                return new Guid(@this.ToString());
            }
            catch (Exception)
            {
                return defaultValue;
            }
        }

        /// <summary>
        ///     An object extension method that converts this object to a nullable unique identifier or default.
        /// </summary>
        /// <param name="this">The @this to act on.</param>
        /// <param name="defaultValueFactory">The default value factory.</param>
        /// <returns>The given data converted to a Guid?</returns>
        public static Guid? ToNullableGuidOrDefault(this object @this, Func<Guid?> defaultValueFactory)
        {
            try
            {
                if (@this == null || @this == DBNull.Value)
                {
                    return null;
                }

                return new Guid(@this.ToString());
            }
            catch (Exception)
            {
                return defaultValueFactory();
            }
        }

        #endregion ToNullableGuidOrDefault

        #region ToNullableInt16

        /// <summary>
        ///     An object extension method that converts the @this to a nullable int 16.
        /// </summary>
        /// <param name="this">The @this to act on.</param>
        /// <returns>@this as a short?</returns>
        public static short? ToNullableInt16(this object @this)
        {
            if (@this == null || @this == DBNull.Value)
            {
                return null;
            }

            return Convert.ToInt16(@this);
        }

        #endregion ToNullableInt16

        #region ToNullableInt16OrDefault

        /// <summary>
        ///     An object extension method that converts this object to a nullable int 16 or default.
        /// </summary>
        /// <param name="this">The @this to act on.</param>
        /// <returns>The given data converted to a short?</returns>
        public static short? ToNullableInt16OrDefault(this object @this)
        {
            try
            {
                if (@this == null || @this == DBNull.Value)
                {
                    return null;
                }

                return Convert.ToInt16(@this);
            }
            catch (Exception)
            {
                return default(short);
            }
        }

        /// <summary>
        ///     An object extension method that converts this object to a nullable int 16 or default.
        /// </summary>
        /// <param name="this">The @this to act on.</param>
        /// <param name="defaultValue">The default value.</param>
        /// <returns>The given data converted to a short?</returns>
        public static short? ToNullableInt16OrDefault(this object @this, short? defaultValue)
        {
            try
            {
                if (@this == null || @this == DBNull.Value)
                {
                    return null;
                }

                return Convert.ToInt16(@this);
            }
            catch (Exception)
            {
                return defaultValue;
            }
        }

        /// <summary>
        ///     An object extension method that converts this object to a nullable int 16 or default.
        /// </summary>
        /// <param name="this">The @this to act on.</param>
        /// <param name="defaultValueFactory">The default value factory.</param>
        /// <returns>The given data converted to a short?</returns>
        public static short? ToNullableInt16OrDefault(this object @this, Func<short?> defaultValueFactory)
        {
            try
            {
                if (@this == null || @this == DBNull.Value)
                {
                    return null;
                }

                return Convert.ToInt16(@this);
            }
            catch (Exception)
            {
                return defaultValueFactory();
            }
        }

        #endregion ToNullableInt16OrDefault

        #region ToNullableInt32

        /// <summary>
        ///     An object extension method that converts the @this to a nullable int 32.
        /// </summary>
        /// <param name="this">The @this to act on.</param>
        /// <returns>@this as an int?</returns>
        public static int? ToNullableInt32(this object @this)
        {
            if (@this == null || @this == DBNull.Value)
            {
                return null;
            }

            return Convert.ToInt32(@this);
        }

        #endregion ToNullableInt32

        #region ToNullableInt32OrDefault

        /// <summary>
        ///     An object extension method that converts this object to a nullable int 32 or default.
        /// </summary>
        /// <param name="this">The @this to act on.</param>
        /// <returns>The given data converted to an int?</returns>
        public static int? ToNullableInt32OrDefault(this object @this)
        {
            try
            {
                if (@this == null || @this == DBNull.Value)
                {
                    return null;
                }

                return Convert.ToInt32(@this);
            }
            catch (Exception)
            {
                return default(int);
            }
        }

        /// <summary>
        ///     An object extension method that converts this object to a nullable int 32 or default.
        /// </summary>
        /// <param name="this">The @this to act on.</param>
        /// <param name="defaultValue">The default value.</param>
        /// <returns>The given data converted to an int?</returns>
        public static int? ToNullableInt32OrDefault(this object @this, int? defaultValue)
        {
            try
            {
                if (@this == null || @this == DBNull.Value)
                {
                    return null;
                }

                return Convert.ToInt32(@this);
            }
            catch (Exception)
            {
                return defaultValue;
            }
        }

        /// <summary>
        ///     An object extension method that converts this object to a nullable int 32 or default.
        /// </summary>
        /// <param name="this">The @this to act on.</param>
        /// <param name="defaultValueFactory">The default value factory.</param>
        /// <returns>The given data converted to an int?</returns>
        public static int? ToNullableInt32OrDefault(this object @this, Func<int?> defaultValueFactory)
        {
            try
            {
                if (@this == null || @this == DBNull.Value)
                {
                    return null;
                }

                return Convert.ToInt32(@this);
            }
            catch (Exception)
            {
                return defaultValueFactory();
            }
        }

        #endregion ToNullableInt32OrDefault

        #region ToNullableInt64

        /// <summary>
        ///     An object extension method that converts the @this to a nullable int 64.
        /// </summary>
        /// <param name="this">The @this to act on.</param>
        /// <returns>@this as a long?</returns>
        public static long? ToNullableInt64(this object @this)
        {
            if (@this == null || @this == DBNull.Value)
            {
                return null;
            }

            return Convert.ToInt64(@this);
        }

        #endregion ToNullableInt64

        #region ToNullableInt64OrDefault

        /// <summary>
        ///     An object extension method that converts this object to a nullable int 64 or default.
        /// </summary>
        /// <param name="this">The @this to act on.</param>
        /// <returns>The given data converted to a long?</returns>
        public static long? ToNullableInt64OrDefault(this object @this)
        {
            try
            {
                if (@this == null || @this == DBNull.Value)
                {
                    return null;
                }

                return Convert.ToInt64(@this);
            }
            catch (Exception)
            {
                return default(long);
            }
        }

        /// <summary>
        ///     An object extension method that converts this object to a nullable int 64 or default.
        /// </summary>
        /// <param name="this">The @this to act on.</param>
        /// <param name="defaultValue">The default value.</param>
        /// <returns>The given data converted to a long?</returns>
        public static long? ToNullableInt64OrDefault(this object @this, long? defaultValue)
        {
            try
            {
                if (@this == null || @this == DBNull.Value)
                {
                    return null;
                }

                return Convert.ToInt64(@this);
            }
            catch (Exception)
            {
                return defaultValue;
            }
        }

        /// <summary>
        ///     An object extension method that converts this object to a nullable int 64 or default.
        /// </summary>
        /// <param name="this">The @this to act on.</param>
        /// <param name="defaultValueFactory">The default value factory.</param>
        /// <returns>The given data converted to a long?</returns>
        public static long? ToNullableInt64OrDefault(this object @this, Func<long?> defaultValueFactory)
        {
            try
            {
                if (@this == null || @this == DBNull.Value)
                {
                    return null;
                }

                return Convert.ToInt64(@this);
            }
            catch (Exception)
            {
                return defaultValueFactory();
            }
        }

        #endregion ToNullableInt64OrDefault

        #region ToNullableLong

        /// <summary>
        ///     An object extension method that converts the @this to a nullable long.
        /// </summary>
        /// <param name="this">The @this to act on.</param>
        /// <returns>@this as a long?</returns>
        public static long? ToNullableLong(this object @this)
        {
            if (@this == null || @this == DBNull.Value)
            {
                return null;
            }

            return Convert.ToInt64(@this);
        }

        #endregion ToNullableLong

        #region ToNullableLongOrDefault

        /// <summary>
        ///     An object extension method that converts this object to a nullable long or default.
        /// </summary>
        /// <param name="this">The @this to act on.</param>
        /// <returns>The given data converted to a long?</returns>
        public static long? ToNullableLongOrDefault(this object @this)
        {
            try
            {
                if (@this == null || @this == DBNull.Value)
                {
                    return null;
                }

                return Convert.ToInt64(@this);
            }
            catch (Exception)
            {
                return default(long);
            }
        }

        /// <summary>
        ///     An object extension method that converts this object to a nullable long or default.
        /// </summary>
        /// <param name="this">The @this to act on.</param>
        /// <param name="defaultValue">The default value.</param>
        /// <returns>The given data converted to a long?</returns>
        public static long? ToNullableLongOrDefault(this object @this, long? defaultValue)
        {
            try
            {
                if (@this == null || @this == DBNull.Value)
                {
                    return null;
                }

                return Convert.ToInt64(@this);
            }
            catch (Exception)
            {
                return defaultValue;
            }
        }

        /// <summary>
        ///     An object extension method that converts this object to a nullable long or default.
        /// </summary>
        /// <param name="this">The @this to act on.</param>
        /// <param name="defaultValueFactory">The default value factory.</param>
        /// <returns>The given data converted to a long?</returns>
        public static long? ToNullableLongOrDefault(this object @this, Func<long?> defaultValueFactory)
        {
            try
            {
                if (@this == null || @this == DBNull.Value)
                {
                    return null;
                }

                return Convert.ToInt64(@this);
            }
            catch (Exception)
            {
                return defaultValueFactory();
            }
        }

        #endregion ToNullableLongOrDefault

        #region ToNullableSByte

        /// <summary>
        ///     An object extension method that converts the @this to a nullable s byte.
        /// </summary>
        /// <param name="this">The @this to act on.</param>
        /// <returns>@this as a sbyte?</returns>
        public static sbyte? ToNullableSByte(this object @this)
        {
            if (@this == null || @this == DBNull.Value)
            {
                return null;
            }

            return Convert.ToSByte(@this);
        }

        #endregion ToNullableSByte

        #region ToNullableSByteOrDefault

        /// <summary>
        ///     An object extension method that converts this object to a nullable s byte or default.
        /// </summary>
        /// <param name="this">The @this to act on.</param>
        /// <returns>The given data converted to a sbyte?</returns>
        public static sbyte? ToNullableSByteOrDefault(this object @this)
        {
            try
            {
                if (@this == null || @this == DBNull.Value)
                {
                    return null;
                }

                return Convert.ToSByte(@this);
            }
            catch (Exception)
            {
                return default(sbyte);
            }
        }

        /// <summary>
        ///     An object extension method that converts this object to a nullable s byte or default.
        /// </summary>
        /// <param name="this">The @this to act on.</param>
        /// <param name="defaultValue">The default value.</param>
        /// <returns>The given data converted to a sbyte?</returns>
        public static sbyte? ToNullableSByteOrDefault(this object @this, sbyte? defaultValue)
        {
            try
            {
                if (@this == null || @this == DBNull.Value)
                {
                    return null;
                }

                return Convert.ToSByte(@this);
            }
            catch (Exception)
            {
                return defaultValue;
            }
        }

        /// <summary>
        ///     An object extension method that converts this object to a nullable s byte or default.
        /// </summary>
        /// <param name="this">The @this to act on.</param>
        /// <param name="defaultValueFactory">The default value factory.</param>
        /// <returns>The given data converted to a sbyte?</returns>
        public static sbyte? ToNullableSByteOrDefault(this object @this, Func<sbyte?> defaultValueFactory)
        {
            try
            {
                if (@this == null || @this == DBNull.Value)
                {
                    return null;
                }

                return Convert.ToSByte(@this);
            }
            catch (Exception)
            {
                return defaultValueFactory();
            }
        }

        #endregion ToNullableSByteOrDefault

        #region ToNullableShort

        /// <summary>
        ///     An object extension method that converts the @this to a nullable short.
        /// </summary>
        /// <param name="this">The @this to act on.</param>
        /// <returns>@this as a short?</returns>
        public static short? ToNullableShort(this object @this)
        {
            if (@this == null || @this == DBNull.Value)
            {
                return null;
            }

            return Convert.ToInt16(@this);
        }

        #endregion ToNullableShort

        #region ToNullableShortOrDefault

        /// <summary>
        ///     An object extension method that converts this object to a nullable short or default.
        /// </summary>
        /// <param name="this">The @this to act on.</param>
        /// <returns>The given data converted to a short?</returns>
        public static short? ToNullableShortOrDefault(this object @this)
        {
            try
            {
                if (@this == null || @this == DBNull.Value)
                {
                    return null;
                }

                return Convert.ToInt16(@this);
            }
            catch (Exception)
            {
                return default(short);
            }
        }

        /// <summary>
        ///     An object extension method that converts this object to a nullable short or default.
        /// </summary>
        /// <param name="this">The @this to act on.</param>
        /// <param name="defaultValue">The default value.</param>
        /// <returns>The given data converted to a short?</returns>
        public static short? ToNullableShortOrDefault(this object @this, short? defaultValue)
        {
            try
            {
                if (@this == null || @this == DBNull.Value)
                {
                    return null;
                }

                return Convert.ToInt16(@this);
            }
            catch (Exception)
            {
                return defaultValue;
            }
        }

        /// <summary>
        ///     An object extension method that converts this object to a nullable short or default.
        /// </summary>
        /// <param name="this">The @this to act on.</param>
        /// <param name="defaultValueFactory">The default value factory.</param>
        /// <returns>The given data converted to a short?</returns>
        public static short? ToNullableShortOrDefault(this object @this, Func<short?> defaultValueFactory)
        {
            try
            {
                if (@this == null || @this == DBNull.Value)
                {
                    return null;
                }

                return Convert.ToInt16(@this);
            }
            catch (Exception)
            {
                return defaultValueFactory();
            }
        }

        #endregion ToNullableShortOrDefault

        #region ToNullableSingle

        /// <summary>
        ///     An object extension method that converts the @this to a nullable single.
        /// </summary>
        /// <param name="this">The @this to act on.</param>
        /// <returns>@this as a float?</returns>
        public static float? ToNullableSingle(this object @this)
        {
            if (@this == null || @this == DBNull.Value)
            {
                return null;
            }

            return Convert.ToSingle(@this);
        }

        #endregion ToNullableSingle

        #region ToNullableSingleOrDefault

        /// <summary>
        ///     An object extension method that converts this object to a nullable single or default.
        /// </summary>
        /// <param name="this">The @this to act on.</param>
        /// <returns>The given data converted to a float?</returns>
        public static float? ToNullableSingleOrDefault(this object @this)
        {
            try
            {
                if (@this == null || @this == DBNull.Value)
                {
                    return null;
                }

                return Convert.ToSingle(@this);
            }
            catch (Exception)
            {
                return default(float);
            }
        }

        /// <summary>
        ///     An object extension method that converts this object to a nullable single or default.
        /// </summary>
        /// <param name="this">The @this to act on.</param>
        /// <param name="defaultValue">The default value.</param>
        /// <returns>The given data converted to a float?</returns>
        public static float? ToNullableSingleOrDefault(this object @this, float? defaultValue)
        {
            try
            {
                if (@this == null || @this == DBNull.Value)
                {
                    return null;
                }

                return Convert.ToSingle(@this);
            }
            catch (Exception)
            {
                return defaultValue;
            }
        }

        /// <summary>
        ///     An object extension method that converts this object to a nullable single or default.
        /// </summary>
        /// <param name="this">The @this to act on.</param>
        /// <param name="defaultValueFactory">The default value factory.</param>
        /// <returns>The given data converted to a float?</returns>
        public static float? ToNullableSingleOrDefault(this object @this, Func<float?> defaultValueFactory)
        {
            try
            {
                if (@this == null || @this == DBNull.Value)
                {
                    return null;
                }

                return Convert.ToSingle(@this);
            }
            catch (Exception)
            {
                return defaultValueFactory();
            }
        }

        #endregion ToNullableSingleOrDefault

        #region ToNullableUInt16

        /// <summary>
        ///     An object extension method that converts the @this to a nullable u int 16.
        /// </summary>
        /// <param name="this">The @this to act on.</param>
        /// <returns>@this as an ushort?</returns>
        public static ushort? ToNullableUInt16(this object @this)
        {
            if (@this == null || @this == DBNull.Value)
            {
                return null;
            }

            return Convert.ToUInt16(@this);
        }

        #endregion ToNullableUInt16

        #region ToNullableUInt16OrDefault

        /// <summary>
        ///     An object extension method that converts this object to a nullable u int 16 or default.
        /// </summary>
        /// <param name="this">The @this to act on.</param>
        /// <returns>The given data converted to an ushort?</returns>
        public static ushort? ToNullableUInt16OrDefault(this object @this)
        {
            try
            {
                if (@this == null || @this == DBNull.Value)
                {
                    return null;
                }

                return Convert.ToUInt16(@this);
            }
            catch (Exception)
            {
                return default(ushort);
            }
        }

        /// <summary>
        ///     An object extension method that converts this object to a nullable u int 16 or default.
        /// </summary>
        /// <param name="this">The @this to act on.</param>
        /// <param name="defaultValue">The default value.</param>
        /// <returns>The given data converted to an ushort?</returns>
        public static ushort? ToNullableUInt16OrDefault(this object @this, ushort? defaultValue)
        {
            try
            {
                if (@this == null || @this == DBNull.Value)
                {
                    return null;
                }

                return Convert.ToUInt16(@this);
            }
            catch (Exception)
            {
                return defaultValue;
            }
        }

        /// <summary>
        ///     An object extension method that converts this object to a nullable u int 16 or default.
        /// </summary>
        /// <param name="this">The @this to act on.</param>
        /// <param name="defaultValueFactory">The default value factory.</param>
        /// <returns>The given data converted to an ushort?</returns>
        public static ushort? ToNullableUInt16OrDefault(this object @this, Func<ushort?> defaultValueFactory)
        {
            try
            {
                if (@this == null || @this == DBNull.Value)
                {
                    return null;
                }

                return Convert.ToUInt16(@this);
            }
            catch (Exception)
            {
                return defaultValueFactory();
            }
        }

        #endregion ToNullableUInt16OrDefault

        #region ToNullableUInt32

        /// <summary>
        ///     An object extension method that converts the @this to a nullable u int 32.
        /// </summary>
        /// <param name="this">The @this to act on.</param>
        /// <returns>@this as an uint?</returns>
        public static uint? ToNullableUInt32(this object @this)
        {
            if (@this == null || @this == DBNull.Value)
            {
                return null;
            }

            return Convert.ToUInt32(@this);
        }

        #endregion ToNullableUInt32

        #region ToNullableUInt32OrDefault

        /// <summary>
        ///     An object extension method that converts this object to a nullable u int 32 or default.
        /// </summary>
        /// <param name="this">The @this to act on.</param>
        /// <returns>The given data converted to an uint?</returns>
        public static uint? ToNullableUInt32OrDefault(this object @this)
        {
            try
            {
                if (@this == null || @this == DBNull.Value)
                {
                    return null;
                }

                return Convert.ToUInt32(@this);
            }
            catch (Exception)
            {
                return default(uint);
            }
        }

        /// <summary>
        ///     An object extension method that converts this object to a nullable u int 32 or default.
        /// </summary>
        /// <param name="this">The @this to act on.</param>
        /// <param name="defaultValue">The default value.</param>
        /// <returns>The given data converted to an uint?</returns>
        public static uint? ToNullableUInt32OrDefault(this object @this, uint? defaultValue)
        {
            try
            {
                if (@this == null || @this == DBNull.Value)
                {
                    return null;
                }

                return Convert.ToUInt32(@this);
            }
            catch (Exception)
            {
                return defaultValue;
            }
        }

        /// <summary>
        ///     An object extension method that converts this object to a nullable u int 32 or default.
        /// </summary>
        /// <param name="this">The @this to act on.</param>
        /// <param name="defaultValueFactory">The default value factory.</param>
        /// <returns>The given data converted to an uint?</returns>
        public static uint? ToNullableUInt32OrDefault(this object @this, Func<uint?> defaultValueFactory)
        {
            try
            {
                if (@this == null || @this == DBNull.Value)
                {
                    return null;
                }

                return Convert.ToUInt32(@this);
            }
            catch (Exception)
            {
                return defaultValueFactory();
            }
        }

        #endregion ToNullableUInt32OrDefault

        #region ToNullableUInt64

        /// <summary>
        ///     An object extension method that converts the @this to a nullable u int 64.
        /// </summary>
        /// <param name="this">The @this to act on.</param>
        /// <returns>@this as an ulong?</returns>
        public static ulong? ToNullableUInt64(this object @this)
        {
            if (@this == null || @this == DBNull.Value)
            {
                return null;
            }

            return Convert.ToUInt64(@this);
        }

        #endregion ToNullableUInt64

        #region ToNullableUInt64OrDefault

        /// <summary>
        ///     An object extension method that converts this object to a nullable u int 64 or default.
        /// </summary>
        /// <param name="this">The @this to act on.</param>
        /// <returns>The given data converted to an ulong?</returns>
        public static ulong? ToNullableUInt64OrDefault(this object @this)
        {
            try
            {
                if (@this == null || @this == DBNull.Value)
                {
                    return null;
                }

                return Convert.ToUInt64(@this);
            }
            catch (Exception)
            {
                return default(ulong);
            }
        }

        /// <summary>
        ///     An object extension method that converts this object to a nullable u int 64 or default.
        /// </summary>
        /// <param name="this">The @this to act on.</param>
        /// <param name="defaultValue">The default value.</param>
        /// <returns>The given data converted to an ulong?</returns>
        public static ulong? ToNullableUInt64OrDefault(this object @this, ulong? defaultValue)
        {
            try
            {
                if (@this == null || @this == DBNull.Value)
                {
                    return null;
                }

                return Convert.ToUInt64(@this);
            }
            catch (Exception)
            {
                return defaultValue;
            }
        }

        /// <summary>
        ///     An object extension method that converts this object to a nullable u int 64 or default.
        /// </summary>
        /// <param name="this">The @this to act on.</param>
        /// <param name="defaultValueFactory">The default value factory.</param>
        /// <returns>The given data converted to an ulong?</returns>
        public static ulong? ToNullableUInt64OrDefault(this object @this, Func<ulong?> defaultValueFactory)
        {
            try
            {
                if (@this == null || @this == DBNull.Value)
                {
                    return null;
                }

                return Convert.ToUInt64(@this);
            }
            catch (Exception)
            {
                return defaultValueFactory();
            }
        }

        #endregion ToNullableUInt64OrDefault

        #region ToNullableULong

        /// <summary>
        ///     An object extension method that converts the @this to a nullable u long.
        /// </summary>
        /// <param name="this">The @this to act on.</param>
        /// <returns>@this as an ulong?</returns>
        public static ulong? ToNullableULong(this object @this)
        {
            if (@this == null || @this == DBNull.Value)
            {
                return null;
            }

            return Convert.ToUInt64(@this);
        }

        #endregion ToNullableULong

        #region ToNullableULongOrDefault

        /// <summary>
        ///     An object extension method that converts this object to a nullable u long or default.
        /// </summary>
        /// <param name="this">The @this to act on.</param>
        /// <returns>The given data converted to an ulong?</returns>
        public static ulong? ToNullableULongOrDefault(this object @this)
        {
            try
            {
                if (@this == null || @this == DBNull.Value)
                {
                    return null;
                }

                return Convert.ToUInt64(@this);
            }
            catch (Exception)
            {
                return default(ulong);
            }
        }

        /// <summary>
        ///     An object extension method that converts this object to a nullable u long or default.
        /// </summary>
        /// <param name="this">The @this to act on.</param>
        /// <param name="defaultValue">The default value.</param>
        /// <returns>The given data converted to an ulong?</returns>
        public static ulong? ToNullableULongOrDefault(this object @this, ulong? defaultValue)
        {
            try
            {
                if (@this == null || @this == DBNull.Value)
                {
                    return null;
                }

                return Convert.ToUInt64(@this);
            }
            catch (Exception)
            {
                return defaultValue;
            }
        }

        /// <summary>
        ///     An object extension method that converts this object to a nullable u long or default.
        /// </summary>
        /// <param name="this">The @this to act on.</param>
        /// <param name="defaultValueFactory">The default value factory.</param>
        /// <returns>The given data converted to an ulong?</returns>
        public static ulong? ToNullableULongOrDefault(this object @this, Func<ulong?> defaultValueFactory)
        {
            try
            {
                if (@this == null || @this == DBNull.Value)
                {
                    return null;
                }

                return Convert.ToUInt64(@this);
            }
            catch (Exception)
            {
                return defaultValueFactory();
            }
        }

        #endregion ToNullableULongOrDefault

        #region ToNullableUShort

        /// <summary>
        ///     An object extension method that converts the @this to a nullable u short.
        /// </summary>
        /// <param name="this">The @this to act on.</param>
        /// <returns>@this as an ushort?</returns>
        public static ushort? ToNullableUShort(this object @this)
        {
            if (@this == null || @this == DBNull.Value)
            {
                return null;
            }

            return Convert.ToUInt16(@this);
        }

        #endregion ToNullableUShort

        #region ToNullableUShortOrDefault

        /// <summary>
        ///     An object extension method that converts this object to a nullable u short or default.
        /// </summary>
        /// <param name="this">The @this to act on.</param>
        /// <returns>The given data converted to an ushort?</returns>
        public static ushort? ToNullableUShortOrDefault(this object @this)
        {
            try
            {
                if (@this == null || @this == DBNull.Value)
                {
                    return null;
                }

                return Convert.ToUInt16(@this);
            }
            catch (Exception)
            {
                return default(ushort);
            }
        }

        /// <summary>
        ///     An object extension method that converts this object to a nullable u short or default.
        /// </summary>
        /// <param name="this">The @this to act on.</param>
        /// <param name="defaultValue">The default value.</param>
        /// <returns>The given data converted to an ushort?</returns>
        public static ushort? ToNullableUShortOrDefault(this object @this, ushort? defaultValue)
        {
            try
            {
                if (@this == null || @this == DBNull.Value)
                {
                    return null;
                }

                return Convert.ToUInt16(@this);
            }
            catch (Exception)
            {
                return defaultValue;
            }
        }

        /// <summary>
        ///     An object extension method that converts this object to a nullable u short or default.
        /// </summary>
        /// <param name="this">The @this to act on.</param>
        /// <param name="defaultValueFactory">The default value factory.</param>
        /// <returns>The given data converted to an ushort?</returns>
        public static ushort? ToNullableUShortOrDefault(this object @this, Func<ushort?> defaultValueFactory)
        {
            try
            {
                if (@this == null || @this == DBNull.Value)
                {
                    return null;
                }

                return Convert.ToUInt16(@this);
            }
            catch (Exception)
            {
                return defaultValueFactory();
            }
        }

        #endregion ToNullableUShortOrDefault

        #region ToSByte

        /// <summary>
        ///     An object extension method that converts the @this to the s byte.
        /// </summary>
        /// <param name="this">The @this to act on.</param>
        /// <returns>@this as a sbyte.</returns>
        public static sbyte ToSByte(this object @this)
        {
            return Convert.ToSByte(@this);
        }

        #endregion ToSByte

        #region ToSByteOrDefault

        /// <summary>
        ///     An object extension method that converts this object to the s byte or default.
        /// </summary>
        /// <param name="this">The @this to act on.</param>
        /// <returns>The given data converted to a sbyte.</returns>
        public static sbyte ToSByteOrDefault(this object @this)
        {
            try
            {
                return Convert.ToSByte(@this);
            }
            catch (Exception)
            {
                return default(sbyte);
            }
        }

        /// <summary>
        ///     An object extension method that converts this object to the s byte or default.
        /// </summary>
        /// <param name="this">The @this to act on.</param>
        /// <param name="defaultValue">The default value.</param>
        /// <returns>The given data converted to a sbyte.</returns>
        public static sbyte ToSByteOrDefault(this object @this, sbyte defaultValue)
        {
            try
            {
                return Convert.ToSByte(@this);
            }
            catch (Exception)
            {
                return defaultValue;
            }
        }

        /// <summary>
        /// An object extension method that converts this object to the s byte or default.
        /// </summary>
        /// <param name="this">The @this to act on.</param>
        /// <param name="defaultValue">The default value.</param>
        /// <param name="useDefaultIfNull">true to use default if null.</param>
        /// <returns>The given data converted to a sbyte.</returns>
        public static sbyte ToSByteOrDefault(this object @this, sbyte defaultValue, bool useDefaultIfNull)
        {
            if (useDefaultIfNull && @this == null)
            {
                return defaultValue;
            }

            try
            {
                return Convert.ToSByte(@this);
            }
            catch (Exception)
            {
                return defaultValue;
            }
        }

        /// <summary>
        ///     An object extension method that converts this object to the s byte or default.
        /// </summary>
        /// <param name="this">The @this to act on.</param>
        /// <param name="defaultValueFactory">The default value factory.</param>
        /// <returns>The given data converted to a sbyte.</returns>
        public static sbyte ToSByteOrDefault(this object @this, Func<sbyte> defaultValueFactory)
        {
            try
            {
                return Convert.ToSByte(@this);
            }
            catch (Exception)
            {
                return defaultValueFactory();
            }
        }

        /// <summary>
        /// An object extension method that converts this object to the s byte or default.
        /// </summary>
        /// <param name="this">The @this to act on.</param>
        /// <param name="defaultValueFactory">The default value factory.</param>
        /// <param name="useDefaultIfNull">true to use default if null.</param>
        /// <returns>The given data converted to a sbyte.</returns>
        public static sbyte ToSByteOrDefault(this object @this, Func<sbyte> defaultValueFactory, bool useDefaultIfNull)
        {
            if (useDefaultIfNull && @this == null)
            {
                return defaultValueFactory();
            }

            try
            {
                return Convert.ToSByte(@this);
            }
            catch (Exception)
            {
                return defaultValueFactory();
            }
        }

        #endregion ToSByteOrDefault

        #region ToShort

        /// <summary>
        ///     An object extension method that converts the @this to a short.
        /// </summary>
        /// <param name="this">The @this to act on.</param>
        /// <returns>@this as a short.</returns>
        public static short ToShort(this object @this)
        {
            return Convert.ToInt16(@this);
        }

        #endregion ToShort

        #region ToShortOrDefault

        /// <summary>
        ///     An object extension method that converts this object to a short or default.
        /// </summary>
        /// <param name="this">The @this to act on.</param>
        /// <returns>The given data converted to a short.</returns>
        public static short ToShortOrDefault(this object @this)
        {
            try
            {
                return Convert.ToInt16(@this);
            }
            catch (Exception)
            {
                return default(short);
            }
        }

        /// <summary>
        ///     An object extension method that converts this object to a short or default.
        /// </summary>
        /// <param name="this">The @this to act on.</param>
        /// <param name="defaultValue">The default value.</param>
        /// <returns>The given data converted to a short.</returns>
        public static short ToShortOrDefault(this object @this, short defaultValue)
        {
            try
            {
                return Convert.ToInt16(@this);
            }
            catch (Exception)
            {
                return defaultValue;
            }
        }

        /// <summary>
        /// An object extension method that converts this object to a short or default.
        /// </summary>
        /// <param name="this">The @this to act on.</param>
        /// <param name="defaultValue">The default value.</param>
        /// <param name="useDefaultIfNull">true to use default if null.</param>
        /// <returns>The given data converted to a short.</returns>
        public static short ToShortOrDefault(this object @this, short defaultValue, bool useDefaultIfNull)
        {
            if (useDefaultIfNull && @this == null)
            {
                return defaultValue;
            }

            try
            {
                return Convert.ToInt16(@this);
            }
            catch (Exception)
            {
                return defaultValue;
            }
        }

        /// <summary>
        ///     An object extension method that converts this object to a short or default.
        /// </summary>
        /// <param name="this">The @this to act on.</param>
        /// <param name="defaultValueFactory">The default value factory.</param>
        /// <returns>The given data converted to a short.</returns>
        public static short ToShortOrDefault(this object @this, Func<short> defaultValueFactory)
        {
            try
            {
                return Convert.ToInt16(@this);
            }
            catch (Exception)
            {
                return defaultValueFactory();
            }
        }

        /// <summary>
        /// An object extension method that converts this object to a short or default.
        /// </summary>
        /// <param name="this">The @this to act on.</param>
        /// <param name="defaultValueFactory">The default value factory.</param>
        /// <param name="useDefaultIfNull">true to use default if null.</param>
        /// <returns>The given data converted to a short.</returns>
        public static short ToShortOrDefault(this object @this, Func<short> defaultValueFactory, bool useDefaultIfNull)
        {
            if (useDefaultIfNull && @this == null)
            {
                return defaultValueFactory();
            }

            try
            {
                return Convert.ToInt16(@this);
            }
            catch (Exception)
            {
                return defaultValueFactory();
            }
        }

        #endregion ToShortOrDefault

        #region ToSingle

        /// <summary>
        ///     An object extension method that converts the @this to a single.
        /// </summary>
        /// <param name="this">The @this to act on.</param>
        /// <returns>@this as a float.</returns>
        public static float ToSingle(this object @this)
        {
            return Convert.ToSingle(@this);
        }

        #endregion ToSingle

        #region ToSingleOrDefault

        /// <summary>
        ///     An object extension method that converts this object to a single or default.
        /// </summary>
        /// <param name="this">The @this to act on.</param>
        /// <returns>The given data converted to a float.</returns>
        public static float ToSingleOrDefault(this object @this)
        {
            try
            {
                return Convert.ToSingle(@this);
            }
            catch (Exception)
            {
                return default(float);
            }
        }

        /// <summary>
        ///     An object extension method that converts this object to a single or default.
        /// </summary>
        /// <param name="this">The @this to act on.</param>
        /// <param name="defaultValue">The default value.</param>
        /// <returns>The given data converted to a float.</returns>
        public static float ToSingleOrDefault(this object @this, float defaultValue)
        {
            try
            {
                return Convert.ToSingle(@this);
            }
            catch (Exception)
            {
                return defaultValue;
            }
        }

        /// <summary>
        /// An object extension method that converts this object to a single or default.
        /// </summary>
        /// <param name="this">The @this to act on.</param>
        /// <param name="defaultValue">The default value.</param>
        /// <param name="useDefaultIfNull">true to use default if null.</param>
        /// <returns>The given data converted to a float.</returns>
        public static float ToSingleOrDefault(this object @this, float defaultValue, bool useDefaultIfNull)
        {
            if (useDefaultIfNull && @this == null)
            {
                return defaultValue;
            }

            try
            {
                return Convert.ToSingle(@this);
            }
            catch (Exception)
            {
                return defaultValue;
            }
        }

        /// <summary>
        ///     An object extension method that converts this object to a single or default.
        /// </summary>
        /// <param name="this">The @this to act on.</param>
        /// <param name="defaultValueFactory">The default value factory.</param>
        /// <returns>The given data converted to a float.</returns>
        public static float ToSingleOrDefault(this object @this, Func<float> defaultValueFactory)
        {
            try
            {
                return Convert.ToSingle(@this);
            }
            catch (Exception)
            {
                return defaultValueFactory();
            }
        }

        /// <summary>
        /// An object extension method that converts this object to a single or default.
        /// </summary>
        /// <param name="this">The @this to act on.</param>
        /// <param name="defaultValueFactory">The default value factory.</param>
        /// <param name="useDefaultIfNull">true to use default if null.</param>
        /// <returns>The given data converted to a float.</returns>
        public static float ToSingleOrDefault(this object @this, Func<float> defaultValueFactory, bool useDefaultIfNull)
        {
            if (useDefaultIfNull && @this == null)
            {
                return defaultValueFactory();
            }

            try
            {
                return Convert.ToSingle(@this);
            }
            catch (Exception)
            {
                return defaultValueFactory();
            }
        }

        #endregion ToSingleOrDefault

        #region ToString

        /// <summary>
        ///     An object extension method that convert this object into a string representation.
        /// </summary>
        /// <param name="this">The @this to act on.</param>
        /// <returns>A string that represents this object.</returns>
        public static string ToString(this object @this)
        {
            return Convert.ToString(@this);
        }

        #endregion ToString

        #region ToStringOrDefault

        /// <summary>
        ///     An object extension method that converts this object to a string or default.
        /// </summary>
        /// <param name="this">The @this to act on.</param>
        /// <returns>The given data converted to a string.</returns>
        public static string ToStringOrDefault(this object @this)
        {
            try
            {
                return Convert.ToString(@this);
            }
            catch (Exception)
            {
                return default(string);
            }
        }

        /// <summary>
        ///     An object extension method that converts this object to a string or default.
        /// </summary>
        /// <param name="this">The @this to act on.</param>
        /// <param name="defaultValue">The default value.</param>
        /// <returns>The given data converted to a string.</returns>
        public static string ToStringOrDefault(this object @this, string defaultValue)
        {
            try
            {
                return Convert.ToString(@this);
            }
            catch (Exception)
            {
                return defaultValue;
            }
        }

        /// <summary>
        /// An object extension method that converts this object to a string or default.
        /// </summary>
        /// <param name="this">The @this to act on.</param>
        /// <param name="defaultValue">The default value.</param>
        /// <param name="useDefaultIfNull">true to use default if null.</param>
        /// <returns>The given data converted to a string.</returns>
        public static string ToStringOrDefault(this object @this, string defaultValue, bool useDefaultIfNull)
        {
            if (useDefaultIfNull && @this == null)
            {
                return defaultValue;
            }

            try
            {
                return Convert.ToString(@this);
            }
            catch (Exception)
            {
                return defaultValue;
            }
        }

        /// <summary>
        ///     An object extension method that converts this object to a string or default.
        /// </summary>
        /// <param name="this">The @this to act on.</param>
        /// <param name="defaultValueFactory">The default value factory.</param>
        /// <returns>The given data converted to a string.</returns>
        public static string ToStringOrDefault(this object @this, Func<string> defaultValueFactory)
        {
            try
            {
                return Convert.ToString(@this);
            }
            catch (Exception)
            {
                return defaultValueFactory();
            }
        }

        /// <summary>
        /// An object extension method that converts this object to a string or default.
        /// </summary>
        /// <param name="this">The @this to act on.</param>
        /// <param name="defaultValueFactory">The default value factory.</param>
        /// <param name="useDefaultIfNull">true to use default if null.</param>
        /// <returns>The given data converted to a string.</returns>
        public static string ToStringOrDefault(this object @this, Func<string> defaultValueFactory, bool useDefaultIfNull)
        {
            if (useDefaultIfNull && @this == null)
            {
                return defaultValueFactory();
            }

            try
            {
                return Convert.ToString(@this);
            }
            catch (Exception)
            {
                return defaultValueFactory();
            }
        }

        #endregion ToStringOrDefault

        #region ToUInt16

        /// <summary>
        ///     An object extension method that converts the @this to an u int 16.
        /// </summary>
        /// <param name="this">The @this to act on.</param>
        /// <returns>@this as an ushort.</returns>
        public static ushort ToUInt16(this object @this)
        {
            return Convert.ToUInt16(@this);
        }

        #endregion ToUInt16

        #region ToUInt16OrDefault

        /// <summary>
        ///     An object extension method that converts this object to an u int 16 or default.
        /// </summary>
        /// <param name="this">The @this to act on.</param>
        /// <returns>The given data converted to an ushort.</returns>
        public static ushort ToUInt16OrDefault(this object @this)
        {
            try
            {
                return Convert.ToUInt16(@this);
            }
            catch (Exception)
            {
                return default(ushort);
            }
        }

        /// <summary>
        ///     An object extension method that converts this object to an u int 16 or default.
        /// </summary>
        /// <param name="this">The @this to act on.</param>
        /// <param name="defaultValue">The default value.</param>
        /// <returns>The given data converted to an ushort.</returns>
        public static ushort ToUInt16OrDefault(this object @this, ushort defaultValue)
        {
            try
            {
                return Convert.ToUInt16(@this);
            }
            catch (Exception)
            {
                return defaultValue;
            }
        }

        /// <summary>
        /// An object extension method that converts this object to an u int 16 or default.
        /// </summary>
        /// <param name="this">The @this to act on.</param>
        /// <param name="defaultValue">The default value.</param>
        /// <param name="useDefaultIfNull">true to use default if null.</param>
        /// <returns>The given data converted to an ushort.</returns>
        public static ushort ToUInt16OrDefault(this object @this, ushort defaultValue, bool useDefaultIfNull)
        {
            if (useDefaultIfNull && @this == null)
            {
                return defaultValue;
            }

            try
            {
                return Convert.ToUInt16(@this);
            }
            catch (Exception)
            {
                return defaultValue;
            }
        }

        /// <summary>
        ///     An object extension method that converts this object to an u int 16 or default.
        /// </summary>
        /// <param name="this">The @this to act on.</param>
        /// <param name="defaultValueFactory">The default value factory.</param>
        /// <returns>The given data converted to an ushort.</returns>
        public static ushort ToUInt16OrDefault(this object @this, Func<ushort> defaultValueFactory)
        {
            try
            {
                return Convert.ToUInt16(@this);
            }
            catch (Exception)
            {
                return defaultValueFactory();
            }
        }

        /// <summary>
        /// An object extension method that converts this object to an u int 16 or default.
        /// </summary>
        /// <param name="this">The @this to act on.</param>
        /// <param name="defaultValueFactory">The default value factory.</param>
        /// <param name="useDefaultIfNull">true to use default if null.</param>
        /// <returns>The given data converted to an ushort.</returns>
        public static ushort ToUInt16OrDefault(this object @this, Func<ushort> defaultValueFactory, bool useDefaultIfNull)
        {
            if (useDefaultIfNull && @this == null)
            {
                return defaultValueFactory();
            }

            try
            {
                return Convert.ToUInt16(@this);
            }
            catch (Exception)
            {
                return defaultValueFactory();
            }
        }

        #endregion ToUInt16OrDefault

        #region ToUInt32

        /// <summary>
        ///     An object extension method that converts the @this to an u int 32.
        /// </summary>
        /// <param name="this">The @this to act on.</param>
        /// <returns>@this as an uint.</returns>
        public static uint ToUInt32(this object @this)
        {
            return Convert.ToUInt32(@this);
        }

        #endregion ToUInt32

        #region ToUInt32OrDefault

        /// <summary>
        ///     An object extension method that converts this object to an u int 32 or default.
        /// </summary>
        /// <param name="this">The @this to act on.</param>
        /// <returns>The given data converted to an uint.</returns>
        public static uint ToUInt32OrDefault(this object @this)
        {
            try
            {
                return Convert.ToUInt32(@this);
            }
            catch (Exception)
            {
                return default(uint);
            }
        }

        /// <summary>
        ///     An object extension method that converts this object to an u int 32 or default.
        /// </summary>
        /// <param name="this">The @this to act on.</param>
        /// <param name="defaultValue">The default value.</param>
        /// <returns>The given data converted to an uint.</returns>
        public static uint ToUInt32OrDefault(this object @this, uint defaultValue)
        {
            try
            {
                return Convert.ToUInt32(@this);
            }
            catch (Exception)
            {
                return defaultValue;
            }
        }

        /// <summary>
        /// An object extension method that converts this object to an u int 32 or default.
        /// </summary>
        /// <param name="this">The @this to act on.</param>
        /// <param name="defaultValue">The default value.</param>
        /// <param name="useDefaultIfNull">true to use default if null.</param>
        /// <returns>The given data converted to an uint.</returns>
        public static uint ToUInt32OrDefault(this object @this, uint defaultValue, bool useDefaultIfNull)
        {
            if (useDefaultIfNull && @this == null)
            {
                return defaultValue;
            }

            try
            {
                return Convert.ToUInt32(@this);
            }
            catch (Exception)
            {
                return defaultValue;
            }
        }

        /// <summary>
        ///     An object extension method that converts this object to an u int 32 or default.
        /// </summary>
        /// <param name="this">The @this to act on.</param>
        /// <param name="defaultValueFactory">The default value factory.</param>
        /// <returns>The given data converted to an uint.</returns>
        public static uint ToUInt32OrDefault(this object @this, Func<uint> defaultValueFactory)
        {
            try
            {
                return Convert.ToUInt32(@this);
            }
            catch (Exception)
            {
                return defaultValueFactory();
            }
        }

        /// <summary>
        /// An object extension method that converts this object to an u int 32 or default.
        /// </summary>
        /// <param name="this">The @this to act on.</param>
        /// <param name="defaultValueFactory">The default value factory.</param>
        /// <param name="useDefaultIfNull">true to use default if null.</param>
        /// <returns>The given data converted to an uint.</returns>
        public static uint ToUInt32OrDefault(this object @this, Func<uint> defaultValueFactory, bool useDefaultIfNull)
        {
            if (useDefaultIfNull && @this == null)
            {
                return defaultValueFactory();
            }

            try
            {
                return Convert.ToUInt32(@this);
            }
            catch (Exception)
            {
                return defaultValueFactory();
            }
        }

        #endregion ToUInt32OrDefault

        #region ToUInt64

        /// <summary>
        ///     An object extension method that converts the @this to an u int 64.
        /// </summary>
        /// <param name="this">The @this to act on.</param>
        /// <returns>@this as an ulong.</returns>
        public static ulong ToUInt64(this object @this)
        {
            return Convert.ToUInt64(@this);
        }

        #endregion ToUInt64

        #region ToUInt64OrDefault

        /// <summary>
        ///     An object extension method that converts this object to an u int 64 or default.
        /// </summary>
        /// <param name="this">The @this to act on.</param>
        /// <returns>The given data converted to an ulong.</returns>
        public static ulong ToUInt64OrDefault(this object @this)
        {
            try
            {
                return Convert.ToUInt64(@this);
            }
            catch (Exception)
            {
                return default(ulong);
            }
        }

        /// <summary>
        ///     An object extension method that converts this object to an u int 64 or default.
        /// </summary>
        /// <param name="this">The @this to act on.</param>
        /// <param name="defaultValue">The default value.</param>
        /// <returns>The given data converted to an ulong.</returns>
        public static ulong ToUInt64OrDefault(this object @this, ulong defaultValue)
        {
            try
            {
                return Convert.ToUInt64(@this);
            }
            catch (Exception)
            {
                return defaultValue;
            }
        }

        /// <summary>
        /// An object extension method that converts this object to an u int 64 or default.
        /// </summary>
        /// <param name="this">The @this to act on.</param>
        /// <param name="defaultValue">The default value.</param>
        /// <param name="useDefaultIfNull">true to use default if null.</param>
        /// <returns>The given data converted to an ulong.</returns>
        public static ulong ToUInt64OrDefault(this object @this, ulong defaultValue, bool useDefaultIfNull)
        {
            if (useDefaultIfNull && @this == null)
            {
                return defaultValue;
            }

            try
            {
                return Convert.ToUInt64(@this);
            }
            catch (Exception)
            {
                return defaultValue;
            }
        }

        /// <summary>
        ///     An object extension method that converts this object to an u int 64 or default.
        /// </summary>
        /// <param name="this">The @this to act on.</param>
        /// <param name="defaultValueFactory">The default value factory.</param>
        /// <returns>The given data converted to an ulong.</returns>
        public static ulong ToUInt64OrDefault(this object @this, Func<ulong> defaultValueFactory)
        {
            try
            {
                return Convert.ToUInt64(@this);
            }
            catch (Exception)
            {
                return defaultValueFactory();
            }
        }

        /// <summary>
        /// An object extension method that converts this object to an u int 64 or default.
        /// </summary>
        /// <param name="this">The @this to act on.</param>
        /// <param name="defaultValueFactory">The default value factory.</param>
        /// <param name="useDefaultIfNull">true to use default if null.</param>
        /// <returns>The given data converted to an ulong.</returns>
        public static ulong ToUInt64OrDefault(this object @this, Func<ulong> defaultValueFactory, bool useDefaultIfNull)
        {
            if (useDefaultIfNull && @this == null)
            {
                return defaultValueFactory();
            }

            try
            {
                return Convert.ToUInt64(@this);
            }
            catch (Exception)
            {
                return defaultValueFactory();
            }
        }

        #endregion ToUInt64OrDefault

        #region ToULong

        /// <summary>
        ///     An object extension method that converts the @this to an u long.
        /// </summary>
        /// <param name="this">The @this to act on.</param>
        /// <returns>@this as an ulong.</returns>
        public static ulong ToULong(this object @this)
        {
            return Convert.ToUInt64(@this);
        }

        #endregion ToULong

        #region ToULongOrDefault

        /// <summary>
        ///     An object extension method that converts this object to an u long or default.
        /// </summary>
        /// <param name="this">The @this to act on.</param>
        /// <returns>The given data converted to an ulong.</returns>
        public static ulong ToULongOrDefault(this object @this)
        {
            try
            {
                return Convert.ToUInt64(@this);
            }
            catch (Exception)
            {
                return default(ulong);
            }
        }

        /// <summary>
        ///     An object extension method that converts this object to an u long or default.
        /// </summary>
        /// <param name="this">The @this to act on.</param>
        /// <param name="defaultValue">The default value.</param>
        /// <returns>The given data converted to an ulong.</returns>
        public static ulong ToULongOrDefault(this object @this, ulong defaultValue)
        {
            try
            {
                return Convert.ToUInt64(@this);
            }
            catch (Exception)
            {
                return defaultValue;
            }
        }

        /// <summary>
        /// An object extension method that converts this object to an u long or default.
        /// </summary>
        /// <param name="this">The @this to act on.</param>
        /// <param name="defaultValue">The default value.</param>
        /// <param name="useDefaultIfNull">true to use default if null.</param>
        /// <returns>The given data converted to an ulong.</returns>
        public static ulong ToULongOrDefault(this object @this, ulong defaultValue, bool useDefaultIfNull)
        {
            if (useDefaultIfNull && @this == null)
            {
                return defaultValue;
            }

            try
            {
                return Convert.ToUInt64(@this);
            }
            catch (Exception)
            {
                return defaultValue;
            }
        }

        /// <summary>
        ///     An object extension method that converts this object to an u long or default.
        /// </summary>
        /// <param name="this">The @this to act on.</param>
        /// <param name="defaultValueFactory">The default value factory.</param>
        /// <returns>The given data converted to an ulong.</returns>
        public static ulong ToULongOrDefault(this object @this, Func<ulong> defaultValueFactory)
        {
            try
            {
                return Convert.ToUInt64(@this);
            }
            catch (Exception)
            {
                return defaultValueFactory();
            }
        }

        /// <summary>
        /// An object extension method that converts this object to an u long or default.
        /// </summary>
        /// <param name="this">The @this to act on.</param>
        /// <param name="defaultValueFactory">The default value factory.</param>
        /// <param name="useDefaultIfNull">true to use default if null.</param>
        /// <returns>The given data converted to an ulong.</returns>
        public static ulong ToULongOrDefault(this object @this, Func<ulong> defaultValueFactory, bool useDefaultIfNull)
        {
            if (useDefaultIfNull && @this == null)
            {
                return defaultValueFactory();
            }

            try
            {
                return Convert.ToUInt64(@this);
            }
            catch (Exception)
            {
                return defaultValueFactory();
            }
        }

        #endregion ToULongOrDefault

        #region ToUShort

        /// <summary>
        ///     An object extension method that converts the @this to an u short.
        /// </summary>
        /// <param name="this">The @this to act on.</param>
        /// <returns>@this as an ushort.</returns>
        public static ushort ToUShort(this object @this)
        {
            return Convert.ToUInt16(@this);
        }

        #endregion ToUShort

        #region ToUShortOrDefault

        /// <summary>
        ///     An object extension method that converts this object to an u short or default.
        /// </summary>
        /// <param name="this">The @this to act on.</param>
        /// <returns>The given data converted to an ushort.</returns>
        public static ushort ToUShortOrDefault(this object @this)
        {
            try
            {
                return Convert.ToUInt16(@this);
            }
            catch (Exception)
            {
                return default(ushort);
            }
        }

        /// <summary>
        ///     An object extension method that converts this object to an u short or default.
        /// </summary>
        /// <param name="this">The @this to act on.</param>
        /// <param name="defaultValue">The default value.</param>
        /// <returns>The given data converted to an ushort.</returns>
        public static ushort ToUShortOrDefault(this object @this, ushort defaultValue)
        {
            try
            {
                return Convert.ToUInt16(@this);
            }
            catch (Exception)
            {
                return defaultValue;
            }
        }

        /// <summary>
        /// An object extension method that converts this object to an u short or default.
        /// </summary>
        /// <param name="this">The @this to act on.</param>
        /// <param name="defaultValue">The default value.</param>
        /// <param name="useDefaultIfNull">true to use default if null.</param>
        /// <returns>The given data converted to an ushort.</returns>
        public static ushort ToUShortOrDefault(this object @this, ushort defaultValue, bool useDefaultIfNull)
        {
            if (useDefaultIfNull && @this == null)
            {
                return defaultValue;
            }

            try
            {
                return Convert.ToUInt16(@this);
            }
            catch (Exception)
            {
                return defaultValue;
            }
        }

        /// <summary>
        ///     An object extension method that converts this object to an u short or default.
        /// </summary>
        /// <param name="this">The @this to act on.</param>
        /// <param name="defaultValueFactory">The default value factory.</param>
        /// <returns>The given data converted to an ushort.</returns>
        public static ushort ToUShortOrDefault(this object @this, Func<ushort> defaultValueFactory)
        {
            try
            {
                return Convert.ToUInt16(@this);
            }
            catch (Exception)
            {
                return defaultValueFactory();
            }
        }

        /// <summary>
        /// An object extension method that converts this object to an u short or default.
        /// </summary>
        /// <param name="this">The @this to act on.</param>
        /// <param name="defaultValueFactory">The default value factory.</param>
        /// <param name="useDefaultIfNull">true to use default if null.</param>
        /// <returns>The given data converted to an ushort.</returns>
        public static ushort ToUShortOrDefault(this object @this, Func<ushort> defaultValueFactory, bool useDefaultIfNull)
        {
            if (useDefaultIfNull && @this == null)
            {
                return defaultValueFactory();
            }

            try
            {
                return Convert.ToUInt16(@this);
            }
            catch (Exception)
            {
                return defaultValueFactory();
            }
        }

        #endregion ToUShortOrDefault

        #endregion ToValueType

        #region Other

        #region ChangeType

        /// <summary>
        ///     Returns an object of the specified type whose value is equivalent to the specified object.
        /// </summary>
        /// <param name="value">An object that implements the  interface.</param>
        /// <param name="typeCode">The type of object to return.</param>
        /// <returns>
        ///     An object whose underlying type is  and whose value is equivalent to .-or-A null reference (Nothing in Visual
        ///     Basic), if  is null and  is , , or .
        /// </returns>
        public static object ChangeType(this object value, TypeCode typeCode)
        {
            return Convert.ChangeType(value, typeCode);
        }

        /// <summary>
        ///     Returns an object of the specified type whose value is equivalent to the specified object. A parameter
        ///     supplies culture-specific formatting information.
        /// </summary>
        /// <param name="value">An object that implements the  interface.</param>
        /// <param name="typeCode">The type of object to return.</param>
        /// <param name="provider">An object that supplies culture-specific formatting information.</param>
        /// <returns>
        ///     An object whose underlying type is  and whose value is equivalent to .-or- A null reference (Nothing in
        ///     Visual Basic), if  is null and  is , , or .
        /// </returns>
        public static object ChangeType(this object value, TypeCode typeCode, IFormatProvider provider)
        {
            return Convert.ChangeType(value, typeCode, provider);
        }

        /// <summary>
        ///     Returns an object of the specified type and whose value is equivalent to the specified object.
        /// </summary>
        /// <param name="value">An object that implements the  interface.</param>
        /// <param name="conversionType">The type of object to return.</param>
        /// <returns>
        ///     An object whose type is  and whose value is equivalent to .-or-A null reference (Nothing in Visual Basic), if
        ///     is null and  is not a value type.
        /// </returns>
        public static object ChangeType(this object value, Type conversionType)
        {
            return Convert.ChangeType(value, conversionType);
        }

        /// <summary>
        ///     Returns an object of the specified type whose value is equivalent to the specified object. A parameter
        ///     supplies culture-specific formatting information.
        /// </summary>
        /// <param name="value">An object that implements the  interface.</param>
        /// <param name="conversionType">The type of object to return.</param>
        /// <param name="provider">An object that supplies culture-specific formatting information.</param>
        /// <returns>
        ///     An object whose type is  and whose value is equivalent to .-or- , if the  of  and  are equal.-or- A null
        ///     reference (Nothing in Visual Basic), if  is null and  is not a value type.
        /// </returns>
        public static object ChangeType(this object value, Type conversionType, IFormatProvider provider)
        {
            return Convert.ChangeType(value, conversionType, provider);
        }

        /// <summary>
        ///     Returns an object of the specified type and whose value is equivalent to the specified object.
        /// </summary>
        /// <typeparam name="T">Generic type parameter.</typeparam>
        /// <param name="value">An object that implements the  interface.</param>
        /// <returns>
        ///     An object whose type is  and whose value is equivalent to .-or-A null reference (Nothing in Visual Basic), if
        ///     is null and  is not a value type.
        /// </returns>
        public static object ChangeType<T>(this object value)
        {
            return (T)Convert.ChangeType(value, typeof(T));
        }

        /// <summary>
        ///     Returns an object of the specified type whose value is equivalent to the specified object. A parameter
        ///     supplies culture-specific formatting information.
        /// </summary>
        /// <typeparam name="T">Generic type parameter.</typeparam>
        /// <param name="value">An object that implements the  interface.</param>
        /// <param name="provider">An object that supplies culture-specific formatting information.</param>
        /// <returns>
        ///     An object whose type is  and whose value is equivalent to .-or- , if the  of  and  are equal.-or- A null
        ///     reference (Nothing in Visual Basic), if  is null and  is not a value type.
        /// </returns>
        public static object ChangeType<T>(this object value, IFormatProvider provider)
        {
            return (T)Convert.ChangeType(value, typeof(T), provider);
        }

        #endregion ChangeType

        #region GetTypeCode

        /// <summary>
        ///     Returns the  for the specified object.
        /// </summary>
        /// <param name="value">An object that implements the  interface.</param>
        /// <returns>The  for , or  if  is null.</returns>
        public static TypeCode GetTypeCode(this object value)
        {
            return Convert.GetTypeCode(value);
        }

        #endregion GetTypeCode

        #region To

        /// <summary>
        ///     A System.Object extension method that toes the given this.
        /// </summary>
        /// <typeparam name="T">Generic type parameter.</typeparam>
        /// <param name="this">this.</param>
        /// <returns>A T.</returns>
        /// <example>
        ///     <code>
        ///       using System;
        ///       using Microsoft.VisualStudio.TestTools.UnitTesting;
        ///
        ///
        ///       namespace ExtensionMethods.Examples
        ///       {
        ///           [TestClass]
        ///           public class System_Object_To
        ///           {
        ///               [TestMethod]
        ///               public void To()
        ///               {
        ///                   string nullValue = null;
        ///                   string value = &quot;1&quot;;
        ///                   object dbNullValue = DBNull.Value;
        ///
        ///                   // Exemples
        ///                   var result1 = value.To&lt;int&gt;(); // return 1;
        ///                   var result2 = value.To&lt;int?&gt;(); // return 1;
        ///                   var result3 = nullValue.To&lt;int?&gt;(); // return null;
        ///                   var result4 = dbNullValue.To&lt;int?&gt;(); // return null;
        ///
        ///                   // Unit Test
        ///                   Assert.AreEqual(1, result1);
        ///                   Assert.AreEqual(1, result2.Value);
        ///                   Assert.IsFalse(result3.HasValue);
        ///                   Assert.IsFalse(result4.HasValue);
        ///               }
        ///           }
        ///       }
        /// </code>
        /// </example>
        /// <example>
        ///     <code>
        ///       using System;
        ///       using Microsoft.VisualStudio.TestTools.UnitTesting;
        ///       using ExtensionMethods.Object;
        ///
        ///       namespace ExtensionMethods.Examples
        ///       {
        ///           [TestClass]
        ///           public class System_Object_To
        ///           {
        ///               [TestMethod]
        ///               public void To()
        ///               {
        ///                   string nullValue = null;
        ///                   string value = &quot;1&quot;;
        ///                   object dbNullValue = DBNull.Value;
        ///
        ///                   // Exemples
        ///                   var result1 = value.To&lt;int&gt;(); // return 1;
        ///                   var result2 = value.To&lt;int?&gt;(); // return 1;
        ///                   var result3 = nullValue.To&lt;int?&gt;(); // return null;
        ///                   var result4 = dbNullValue.To&lt;int?&gt;(); // return null;
        ///
        ///                   // Unit Test
        ///                   Assert.AreEqual(1, result1);
        ///                   Assert.AreEqual(1, result2.Value);
        ///                   Assert.IsFalse(result3.HasValue);
        ///                   Assert.IsFalse(result4.HasValue);
        ///               }
        ///           }
        ///       }
        /// </code>
        /// </example>
        public static T To<T>(this object @this)
        {
            if (@this != null)
            {
                var targetType = typeof(T);

                if (@this.GetType() == targetType)
                {
                    return (T)@this;
                }

                var converter = TypeDescriptor.GetConverter(@this);
                if (converter.CanConvertTo(targetType))
                {
                    return (T)converter.ConvertTo(@this, targetType);
                }

                converter = TypeDescriptor.GetConverter(targetType);
                if (converter.CanConvertFrom(@this.GetType()))
                {
                    return (T)converter.ConvertFrom(@this);
                }

                if (@this == DBNull.Value)
                {
                    return (T)(object)null;
                }
            }

            return (T)@this;
        }

        /// <summary>
        ///     A System.Object extension method that toes the given this.
        /// </summary>
        /// <param name="this">this.</param>
        /// <param name="type">The type.</param>
        /// <returns>An object.</returns>
        /// <example>
        ///     <code>
        ///       using System;
        ///       using Microsoft.VisualStudio.TestTools.UnitTesting;
        ///
        ///
        ///       namespace ExtensionMethods.Examples
        ///       {
        ///           [TestClass]
        ///           public class System_Object_To
        ///           {
        ///               [TestMethod]
        ///               public void To()
        ///               {
        ///                   string nullValue = null;
        ///                   string value = &quot;1&quot;;
        ///                   object dbNullValue = DBNull.Value;
        ///
        ///                   // Exemples
        ///                   var result1 = value.To&lt;int&gt;(); // return 1;
        ///                   var result2 = value.To&lt;int?&gt;(); // return 1;
        ///                   var result3 = nullValue.To&lt;int?&gt;(); // return null;
        ///                   var result4 = dbNullValue.To&lt;int?&gt;(); // return null;
        ///
        ///                   // Unit Test
        ///                   Assert.AreEqual(1, result1);
        ///                   Assert.AreEqual(1, result2.Value);
        ///                   Assert.IsFalse(result3.HasValue);
        ///                   Assert.IsFalse(result4.HasValue);
        ///               }
        ///           }
        ///       }
        /// </code>
        /// </example>
        /// <example>
        ///     <code>
        ///       using System;
        ///       using Microsoft.VisualStudio.TestTools.UnitTesting;
        ///       using ExtensionMethods.Object;
        ///
        ///       namespace ExtensionMethods.Examples
        ///       {
        ///           [TestClass]
        ///           public class System_Object_To
        ///           {
        ///               [TestMethod]
        ///               public void To()
        ///               {
        ///                   string nullValue = null;
        ///                   string value = &quot;1&quot;;
        ///                   object dbNullValue = DBNull.Value;
        ///
        ///                   // Exemples
        ///                   var result1 = value.To&lt;int&gt;(); // return 1;
        ///                   var result2 = value.To&lt;int?&gt;(); // return 1;
        ///                   var result3 = nullValue.To&lt;int?&gt;(); // return null;
        ///                   var result4 = dbNullValue.To&lt;int?&gt;(); // return null;
        ///
        ///                   // Unit Test
        ///                   Assert.AreEqual(1, result1);
        ///                   Assert.AreEqual(1, result2.Value);
        ///                   Assert.IsFalse(result3.HasValue);
        ///                   Assert.IsFalse(result4.HasValue);
        ///               }
        ///           }
        ///       }
        /// </code>
        /// </example>
        public static object To(this object @this, Type type)
        {
            if (@this != null)
            {
                var targetType = type;

                if (@this.GetType() == targetType)
                {
                    return @this;
                }

                var converter = TypeDescriptor.GetConverter(@this);
                if (converter.CanConvertTo(targetType))
                {
                    return converter.ConvertTo(@this, targetType);
                }

                converter = TypeDescriptor.GetConverter(targetType);
                if (converter.CanConvertFrom(@this.GetType()))
                {
                    return converter.ConvertFrom(@this);
                }

                if (@this == DBNull.Value)
                {
                    return null;
                }
            }

            return @this;
        }

        #endregion To

        #region ToOrDefault

        /// <summary>
        ///     A System.Object extension method that converts this object to an or default.
        /// </summary>
        /// <typeparam name="T">Generic type parameter.</typeparam>
        /// <param name="this">this.</param>
        /// <param name="defaultValueFactory">The default value factory.</param>
        /// <returns>The given data converted to a T.</returns>
        /// <example>
        ///     <code>
        ///       using Microsoft.VisualStudio.TestTools.UnitTesting;
        ///       using Z.ExtensionMethods.Object;
        ///
        ///       namespace ExtensionMethods.Examples
        ///       {
        ///           [TestClass]
        ///           public class System_Object_ToOrDefault
        ///           {
        ///               [TestMethod]
        ///               public void ToOrDefault()
        ///               {
        ///                   // Type
        ///                   object intValue = &quot;1&quot;;
        ///                   object invalidValue = &quot;Fizz&quot;;
        ///
        ///                   // Exemples
        ///                   var result1 = intValue.ToOrDefault&lt;int&gt;(); // return 1;
        ///                   var result2 = invalidValue.ToOrDefault&lt;int&gt;(); // return 0;
        ///                   int result3 = invalidValue.ToOrDefault(3); // return 3;
        ///                   int result4 = invalidValue.ToOrDefault(() =&gt; 4); // return 4;
        ///
        ///                   // Unit Test
        ///                   Assert.AreEqual(1, result1);
        ///                   Assert.AreEqual(0, result2);
        ///                   Assert.AreEqual(3, result3);
        ///                   Assert.AreEqual(4, result4);
        ///               }
        ///           }
        ///       }
        /// </code>
        /// </example>
        /// <example>
        ///     <code>
        ///       using Microsoft.VisualStudio.TestTools.UnitTesting;
        ///       using ExtensionMethods.Object;
        ///
        ///       namespace ExtensionMethods.Examples
        ///       {
        ///           [TestClass]
        ///           public class System_Object_ToOrDefault
        ///           {
        ///               [TestMethod]
        ///               public void ToOrDefault()
        ///               {
        ///                   // Type
        ///                   object intValue = &quot;1&quot;;
        ///                   object invalidValue = &quot;Fizz&quot;;
        ///
        ///                   // Exemples
        ///                   var result1 = intValue.ToOrDefault&lt;int&gt;(); // return 1;
        ///                   var result2 = invalidValue.ToOrDefault&lt;int&gt;(); // return 0;
        ///                   int result3 = invalidValue.ToOrDefault(3); // return 3;
        ///                   int result4 = invalidValue.ToOrDefault(() =&gt; 4); // return 4;
        ///
        ///                   // Unit Test
        ///                   Assert.AreEqual(1, result1);
        ///                   Assert.AreEqual(0, result2);
        ///                   Assert.AreEqual(3, result3);
        ///                   Assert.AreEqual(4, result4);
        ///               }
        ///           }
        ///       }
        /// </code>
        /// </example>
        /// <example>
        ///     <code>
        ///           using Microsoft.VisualStudio.TestTools.UnitTesting;
        ///           using ExtensionMethods.Object;
        ///
        ///           namespace ExtensionMethods.Examples
        ///           {
        ///               [TestClass]
        ///               public class System_Object_ToOrDefault
        ///               {
        ///                   [TestMethod]
        ///                   public void ToOrDefault()
        ///                   {
        ///                       // Type
        ///                       object intValue = &quot;1&quot;;
        ///                       object invalidValue = &quot;Fizz&quot;;
        ///
        ///                       // Exemples
        ///                       var result1 = intValue.ToOrDefault&lt;int&gt;(); // return 1;
        ///                       var result2 = invalidValue.ToOrDefault&lt;int&gt;(); // return 0;
        ///                       int result3 = invalidValue.ToOrDefault(3); // return 3;
        ///                       int result4 = invalidValue.ToOrDefault(() =&gt; 4); // return 4;
        ///
        ///                       // Unit Test
        ///                       Assert.AreEqual(1, result1);
        ///                       Assert.AreEqual(0, result2);
        ///                       Assert.AreEqual(3, result3);
        ///                       Assert.AreEqual(4, result4);
        ///                   }
        ///               }
        ///           }
        ///     </code>
        /// </example>
        public static T ToOrDefault<T>(this object @this, Func<object, T> defaultValueFactory)
        {
            try
            {
                if (@this != null)
                {
                    var targetType = typeof(T);

                    if (@this.GetType() == targetType)
                    {
                        return (T)@this;
                    }

                    var converter = TypeDescriptor.GetConverter(@this);
                    if (converter.CanConvertTo(targetType))
                    {
                        return (T)converter.ConvertTo(@this, targetType);
                    }

                    converter = TypeDescriptor.GetConverter(targetType);
                    if (converter.CanConvertFrom(@this.GetType()))
                    {
                        return (T)converter.ConvertFrom(@this);
                    }

                    if (@this == DBNull.Value)
                    {
                        return (T)(object)null;
                    }
                }

                return (T)@this;
            }
            catch (Exception)
            {
                return defaultValueFactory(@this);
            }
        }

        /// <summary>
        ///     A System.Object extension method that converts this object to an or default.
        /// </summary>
        /// <typeparam name="T">Generic type parameter.</typeparam>
        /// <param name="this">this.</param>
        /// <param name="defaultValueFactory">The default value factory.</param>
        /// <returns>The given data converted to a T.</returns>
        /// <example>
        ///     <code>
        ///       using Microsoft.VisualStudio.TestTools.UnitTesting;
        ///       using ExtensionMethods.Object;
        ///
        ///       namespace ExtensionMethods.Examples
        ///       {
        ///           [TestClass]
        ///           public class System_Object_ToOrDefault
        ///           {
        ///               [TestMethod]
        ///               public void ToOrDefault()
        ///               {
        ///                   // Type
        ///                   object intValue = &quot;1&quot;;
        ///                   object invalidValue = &quot;Fizz&quot;;
        ///
        ///                   // Exemples
        ///                   var result1 = intValue.ToOrDefault&lt;int&gt;(); // return 1;
        ///                   var result2 = invalidValue.ToOrDefault&lt;int&gt;(); // return 0;
        ///                   int result3 = invalidValue.ToOrDefault(3); // return 3;
        ///                   int result4 = invalidValue.ToOrDefault(() =&gt; 4); // return 4;
        ///
        ///                   // Unit Test
        ///                   Assert.AreEqual(1, result1);
        ///                   Assert.AreEqual(0, result2);
        ///                   Assert.AreEqual(3, result3);
        ///                   Assert.AreEqual(4, result4);
        ///               }
        ///           }
        ///       }
        /// </code>
        /// </example>
        /// <example>
        ///     <code>
        ///       using Microsoft.VisualStudio.TestTools.UnitTesting;
        ///       using ExtensionMethods.Object;
        ///
        ///       namespace ExtensionMethods.Examples
        ///       {
        ///           [TestClass]
        ///           public class System_Object_ToOrDefault
        ///           {
        ///               [TestMethod]
        ///               public void ToOrDefault()
        ///               {
        ///                   // Type
        ///                   object intValue = &quot;1&quot;;
        ///                   object invalidValue = &quot;Fizz&quot;;
        ///
        ///                   // Exemples
        ///                   var result1 = intValue.ToOrDefault&lt;int&gt;(); // return 1;
        ///                   var result2 = invalidValue.ToOrDefault&lt;int&gt;(); // return 0;
        ///                   int result3 = invalidValue.ToOrDefault(3); // return 3;
        ///                   int result4 = invalidValue.ToOrDefault(() =&gt; 4); // return 4;
        ///
        ///                   // Unit Test
        ///                   Assert.AreEqual(1, result1);
        ///                   Assert.AreEqual(0, result2);
        ///                   Assert.AreEqual(3, result3);
        ///                   Assert.AreEqual(4, result4);
        ///               }
        ///           }
        ///       }
        /// </code>
        /// </example>
        /// <example>
        ///     <code>
        ///           using Microsoft.VisualStudio.TestTools.UnitTesting;
        ///           using ExtensionMethods.Object;
        ///
        ///           namespace ExtensionMethods.Examples
        ///           {
        ///               [TestClass]
        ///               public class System_Object_ToOrDefault
        ///               {
        ///                   [TestMethod]
        ///                   public void ToOrDefault()
        ///                   {
        ///                       // Type
        ///                       object intValue = &quot;1&quot;;
        ///                       object invalidValue = &quot;Fizz&quot;;
        ///
        ///                       // Exemples
        ///                       var result1 = intValue.ToOrDefault&lt;int&gt;(); // return 1;
        ///                       var result2 = invalidValue.ToOrDefault&lt;int&gt;(); // return 0;
        ///                       int result3 = invalidValue.ToOrDefault(3); // return 3;
        ///                       int result4 = invalidValue.ToOrDefault(() =&gt; 4); // return 4;
        ///
        ///                       // Unit Test
        ///                       Assert.AreEqual(1, result1);
        ///                       Assert.AreEqual(0, result2);
        ///                       Assert.AreEqual(3, result3);
        ///                       Assert.AreEqual(4, result4);
        ///                   }
        ///               }
        ///           }
        ///     </code>
        /// </example>
        public static T ToOrDefault<T>(this object @this, Func<T> defaultValueFactory)
        {
            return @this.ToOrDefault(x => defaultValueFactory());
        }

        /// <summary>
        ///     A System.Object extension method that converts this object to an or default.
        /// </summary>
        /// <typeparam name="T">Generic type parameter.</typeparam>
        /// <param name="this">this.</param>
        /// <returns>The given data converted to a T.</returns>
        public static T ToOrDefault<T>(this object @this)
        {
            return @this.ToOrDefault(x => default(T));
        }

        /// <summary>
        ///     A System.Object extension method that converts this object to an or default.
        /// </summary>
        /// <typeparam name="T">Generic type parameter.</typeparam>
        /// <param name="this">this.</param>
        /// <param name="defaultValue">The default value.</param>
        /// <returns>The given data converted to a T.</returns>
        public static T ToOrDefault<T>(this object @this, T defaultValue)
        {
            return @this.ToOrDefault(x => defaultValue);
        }

        #endregion ToOrDefault

        #endregion Other

        #endregion Convert

        #region Utility

        #region Coalesce

        /// <summary>
        ///     A T extension method that that return the first not null value (including the @this).
        /// </summary>
        /// <typeparam name="T">Generic type parameter.</typeparam>
        /// <param name="this">The @this to act on.</param>
        /// <param name="values">A variable-length parameters list containing values.</param>
        /// <returns>The first not null value.</returns>
        public static T Coalesce<T>(this T @this, params T[] values) where T : class
        {
            return @this ?? values.FirstOrDefault(value => value != null);
        }

        #endregion Coalesce

        #region CoalesceOrDefault

        /// <summary>
        ///     A T extension method that that return the first not null value (including the @this) or a default value.
        /// </summary>
        /// <typeparam name="T">Generic type parameter.</typeparam>
        /// <param name="this">The @this to act on.</param>
        /// <param name="values">A variable-length parameters list containing values.</param>
        /// <returns>The first not null value or a default value.</returns>
        public static T CoalesceOrDefault<T>(this T @this, params T[] values) where T : class
        {
            return @this ?? values.FirstOrDefault(value => value != null);
        }

        /// <summary>
        ///     A T extension method that that return the first not null value (including the @this) or a default value.
        /// </summary>
        /// <typeparam name="T">Generic type parameter.</typeparam>
        /// <param name="this">The @this to act on.</param>
        /// <param name="defaultValueFactory">The default value factory.</param>
        /// <param name="values">A variable-length parameters list containing values.</param>
        /// <returns>The first not null value or a default value.</returns>
        /// <example>
        ///     <code>
        ///       using Microsoft.VisualStudio.TestTools.UnitTesting;
        ///
        ///
        ///       namespace ExtensionMethods.Examples
        ///       {
        ///           [TestClass]
        ///           public class System_Object_CoalesceOrDefault
        ///           {
        ///               [TestMethod]
        ///               public void CoalesceOrDefault()
        ///               {
        ///                   // Varable
        ///                   object nullObject = null;
        ///
        ///                   // Type
        ///                   object @thisNull = null;
        ///                   object @thisNotNull = &quot;Fizz&quot;;
        ///
        ///                   // Exemples
        ///                   object result1 = @thisNull.CoalesceOrDefault(nullObject, nullObject, &quot;Buzz&quot;); // return &quot;Buzz&quot;;
        ///                   object result2 = @thisNull.CoalesceOrDefault(() =&gt; &quot;Buzz&quot;, null, null); // return &quot;Buzz&quot;;
        ///                   object result3 = @thisNull.CoalesceOrDefault((x) =&gt; &quot;Buzz&quot;, null, null); // return &quot;Buzz&quot;;
        ///                   object result4 = @thisNotNull.CoalesceOrDefault(nullObject, nullObject, &quot;Buzz&quot;); // return &quot;Fizz&quot;;
        ///
        ///                   // Unit Test
        ///                   Assert.AreEqual(&quot;Buzz&quot;, result1);
        ///                   Assert.AreEqual(&quot;Buzz&quot;, result2);
        ///                   Assert.AreEqual(&quot;Buzz&quot;, result3);
        ///                   Assert.AreEqual(&quot;Fizz&quot;, result4);
        ///               }
        ///           }
        ///       }
        /// </code>
        /// </example>
        /// <example>
        ///     <code>
        ///       using Microsoft.VisualStudio.TestTools.UnitTesting;
        ///       using ExtensionMethods.Object;
        ///
        ///       namespace ExtensionMethods.Examples
        ///       {
        ///           [TestClass]
        ///           public class System_Object_CoalesceOrDefault
        ///           {
        ///               [TestMethod]
        ///               public void CoalesceOrDefault()
        ///               {
        ///                   // Varable
        ///                   object nullObject = null;
        ///
        ///                   // Type
        ///                   object @thisNull = null;
        ///                   object @thisNotNull = &quot;Fizz&quot;;
        ///
        ///                   // Exemples
        ///                   object result1 = @thisNull.CoalesceOrDefault(nullObject, nullObject, &quot;Buzz&quot;); // return &quot;Buzz&quot;;
        ///                   object result2 = @thisNull.CoalesceOrDefault(() =&gt; &quot;Buzz&quot;, null, null); // return &quot;Buzz&quot;;
        ///                   object result3 = @thisNull.CoalesceOrDefault(x =&gt; &quot;Buzz&quot;, null, null); // return &quot;Buzz&quot;;
        ///                   object result4 = @thisNotNull.CoalesceOrDefault(nullObject, nullObject, &quot;Buzz&quot;); // return &quot;Fizz&quot;;
        ///
        ///                   // Unit Test
        ///                   Assert.AreEqual(&quot;Buzz&quot;, result1);
        ///                   Assert.AreEqual(&quot;Buzz&quot;, result2);
        ///                   Assert.AreEqual(&quot;Buzz&quot;, result3);
        ///                   Assert.AreEqual(&quot;Fizz&quot;, result4);
        ///               }
        ///           }
        ///       }
        /// </code>
        /// </example>
        /// <example>
        ///     <code>
        ///           using Microsoft.VisualStudio.TestTools.UnitTesting;
        ///           using ExtensionMethods.Object;
        ///
        ///           namespace ExtensionMethods.Examples
        ///           {
        ///               [TestClass]
        ///               public class System_Object_CoalesceOrDefault
        ///               {
        ///                   [TestMethod]
        ///                   public void CoalesceOrDefault()
        ///                   {
        ///                       // Varable
        ///                       object nullObject = null;
        ///
        ///                       // Type
        ///                       object @thisNull = null;
        ///                       object @thisNotNull = &quot;Fizz&quot;;
        ///
        ///                       // Exemples
        ///                       object result1 = @thisNull.CoalesceOrDefault(nullObject, nullObject, &quot;Buzz&quot;); // return &quot;Buzz&quot;;
        ///                       object result2 = @thisNull.CoalesceOrDefault(() =&gt; &quot;Buzz&quot;, null, null); // return &quot;Buzz&quot;;
        ///                       object result3 = @thisNull.CoalesceOrDefault(x =&gt; &quot;Buzz&quot;, null, null); // return &quot;Buzz&quot;;
        ///                       object result4 = @thisNotNull.CoalesceOrDefault(nullObject, nullObject, &quot;Buzz&quot;); // return &quot;Fizz&quot;;
        ///
        ///                       // Unit Test
        ///                       Assert.AreEqual(&quot;Buzz&quot;, result1);
        ///                       Assert.AreEqual(&quot;Buzz&quot;, result2);
        ///                       Assert.AreEqual(&quot;Buzz&quot;, result3);
        ///                       Assert.AreEqual(&quot;Fizz&quot;, result4);
        ///                   }
        ///               }
        ///           }
        ///     </code>
        /// </example>
        public static T CoalesceOrDefault<T>(this T @this, Func<T> defaultValueFactory, params T[] values) where T : class
        {
            if (@this != null)
            {
                return @this;
            }

            foreach (var value in values)
            {
                if (value != null)
                {
                    return value;
                }
            }

            return defaultValueFactory();
        }

        /// <summary>
        ///     A T extension method that that return the first not null value (including the @this) or a default value.
        /// </summary>
        /// <typeparam name="T">Generic type parameter.</typeparam>
        /// <param name="this">The @this to act on.</param>
        /// <param name="defaultValueFactory">The default value factory.</param>
        /// <param name="values">A variable-length parameters list containing values.</param>
        /// <returns>The first not null value or a default value.</returns>
        /// <example>
        ///     <code>
        ///       using Microsoft.VisualStudio.TestTools.UnitTesting;
        ///
        ///
        ///       namespace ExtensionMethods.Examples
        ///       {
        ///           [TestClass]
        ///           public class System_Object_CoalesceOrDefault
        ///           {
        ///               [TestMethod]
        ///               public void CoalesceOrDefault()
        ///               {
        ///                   // Varable
        ///                   object nullObject = null;
        ///
        ///                   // Type
        ///                   object @thisNull = null;
        ///                   object @thisNotNull = &quot;Fizz&quot;;
        ///
        ///                   // Exemples
        ///                   object result1 = @thisNull.CoalesceOrDefault(nullObject, nullObject, &quot;Buzz&quot;); // return &quot;Buzz&quot;;
        ///                   object result2 = @thisNull.CoalesceOrDefault(() =&gt; &quot;Buzz&quot;, null, null); // return &quot;Buzz&quot;;
        ///                   object result3 = @thisNull.CoalesceOrDefault((x) =&gt; &quot;Buzz&quot;, null, null); // return &quot;Buzz&quot;;
        ///                   object result4 = @thisNotNull.CoalesceOrDefault(nullObject, nullObject, &quot;Buzz&quot;); // return &quot;Fizz&quot;;
        ///
        ///                   // Unit Test
        ///                   Assert.AreEqual(&quot;Buzz&quot;, result1);
        ///                   Assert.AreEqual(&quot;Buzz&quot;, result2);
        ///                   Assert.AreEqual(&quot;Buzz&quot;, result3);
        ///                   Assert.AreEqual(&quot;Fizz&quot;, result4);
        ///               }
        ///           }
        ///       }
        /// </code>
        /// </example>
        /// <example>
        ///     <code>
        ///       using Microsoft.VisualStudio.TestTools.UnitTesting;
        ///       using ExtensionMethods.Object;
        ///
        ///       namespace ExtensionMethods.Examples
        ///       {
        ///           [TestClass]
        ///           public class System_Object_CoalesceOrDefault
        ///           {
        ///               [TestMethod]
        ///               public void CoalesceOrDefault()
        ///               {
        ///                   // Varable
        ///                   object nullObject = null;
        ///
        ///                   // Type
        ///                   object @thisNull = null;
        ///                   object @thisNotNull = &quot;Fizz&quot;;
        ///
        ///                   // Exemples
        ///                   object result1 = @thisNull.CoalesceOrDefault(nullObject, nullObject, &quot;Buzz&quot;); // return &quot;Buzz&quot;;
        ///                   object result2 = @thisNull.CoalesceOrDefault(() =&gt; &quot;Buzz&quot;, null, null); // return &quot;Buzz&quot;;
        ///                   object result3 = @thisNull.CoalesceOrDefault(x =&gt; &quot;Buzz&quot;, null, null); // return &quot;Buzz&quot;;
        ///                   object result4 = @thisNotNull.CoalesceOrDefault(nullObject, nullObject, &quot;Buzz&quot;); // return &quot;Fizz&quot;;
        ///
        ///                   // Unit Test
        ///                   Assert.AreEqual(&quot;Buzz&quot;, result1);
        ///                   Assert.AreEqual(&quot;Buzz&quot;, result2);
        ///                   Assert.AreEqual(&quot;Buzz&quot;, result3);
        ///                   Assert.AreEqual(&quot;Fizz&quot;, result4);
        ///               }
        ///           }
        ///       }
        /// </code>
        /// </example>
        /// <example>
        ///     <code>
        ///           using Microsoft.VisualStudio.TestTools.UnitTesting;
        ///           using ExtensionMethods.Object;
        ///
        ///           namespace ExtensionMethods.Examples
        ///           {
        ///               [TestClass]
        ///               public class System_Object_CoalesceOrDefault
        ///               {
        ///                   [TestMethod]
        ///                   public void CoalesceOrDefault()
        ///                   {
        ///                       // Varable
        ///                       object nullObject = null;
        ///
        ///                       // Type
        ///                       object @thisNull = null;
        ///                       object @thisNotNull = &quot;Fizz&quot;;
        ///
        ///                       // Exemples
        ///                       object result1 = @thisNull.CoalesceOrDefault(nullObject, nullObject, &quot;Buzz&quot;); // return &quot;Buzz&quot;;
        ///                       object result2 = @thisNull.CoalesceOrDefault(() =&gt; &quot;Buzz&quot;, null, null); // return &quot;Buzz&quot;;
        ///                       object result3 = @thisNull.CoalesceOrDefault(x =&gt; &quot;Buzz&quot;, null, null); // return &quot;Buzz&quot;;
        ///                       object result4 = @thisNotNull.CoalesceOrDefault(nullObject, nullObject, &quot;Buzz&quot;); // return &quot;Fizz&quot;;
        ///
        ///                       // Unit Test
        ///                       Assert.AreEqual(&quot;Buzz&quot;, result1);
        ///                       Assert.AreEqual(&quot;Buzz&quot;, result2);
        ///                       Assert.AreEqual(&quot;Buzz&quot;, result3);
        ///                       Assert.AreEqual(&quot;Fizz&quot;, result4);
        ///                   }
        ///               }
        ///           }
        ///     </code>
        /// </example>
        public static T CoalesceOrDefault<T>(this T @this, Func<T, T> defaultValueFactory, params T[] values) where T : class
        {
            if (@this != null)
            {
                return @this;
            }

            foreach (var value in values)
            {
                if (value != null)
                {
                    return value;
                }
            }

            return defaultValueFactory(null);
        }

        #endregion CoalesceOrDefault

        #region GetValueOrDefault

        /// <summary>
        ///     A T extension method that gets value or default.
        /// </summary>
        /// <typeparam name="T">Generic type parameter.</typeparam>
        /// <typeparam name="TResult">Type of the result.</typeparam>
        /// <param name="this">The @this to act on.</param>
        /// <param name="func">The function.</param>
        /// <returns>The value or default.</returns>
        public static TResult GetValueOrDefault<T, TResult>(this T @this, Func<T, TResult> func)
        {
            try
            {
                return func(@this);
            }
            catch (Exception)
            {
                return default(TResult);
            }
        }

        /// <summary>
        ///     A T extension method that gets value or default.
        /// </summary>
        /// <typeparam name="T">Generic type parameter.</typeparam>
        /// <typeparam name="TResult">Type of the result.</typeparam>
        /// <param name="this">The @this to act on.</param>
        /// <param name="func">The function.</param>
        /// <param name="defaultValue">The default value.</param>
        /// <returns>The value or default.</returns>
        public static TResult GetValueOrDefault<T, TResult>(this T @this, Func<T, TResult> func, TResult defaultValue)
        {
            try
            {
                return func(@this);
            }
            catch (Exception)
            {
                return defaultValue;
            }
        }

        /// <summary>
        ///     A T extension method that gets value or default.
        /// </summary>
        /// <typeparam name="T">Generic type parameter.</typeparam>
        /// <typeparam name="TResult">Type of the result.</typeparam>
        /// <param name="this">The @this to act on.</param>
        /// <param name="func">The function.</param>
        /// <param name="defaultValueFactory">The default value factory.</param>
        /// <returns>The value or default.</returns>
        /// <example>
        ///     <code>
        ///       using System.Xml;
        ///       using Microsoft.VisualStudio.TestTools.UnitTesting;
        ///
        ///
        ///       namespace ExtensionMethods.Examples
        ///       {
        ///           [TestClass]
        ///           public class System_Object_GetValueOrDefault
        ///           {
        ///               [TestMethod]
        ///               public void GetValueOrDefault()
        ///               {
        ///                   // Type
        ///                   var @this = new XmlDocument();
        ///
        ///                   // Exemples
        ///                   string result1 = @this.GetValueOrDefault(x =&gt; x.FirstChild.InnerXml, &quot;FizzBuzz&quot;); // return &quot;FizzBuzz&quot;;
        ///                   string result2 = @this.GetValueOrDefault(x =&gt; x.FirstChild.InnerXml, () =&gt; &quot;FizzBuzz&quot;); // return &quot;FizzBuzz&quot;
        ///
        ///                   // Unit Test
        ///                   Assert.AreEqual(&quot;FizzBuzz&quot;, result1);
        ///                   Assert.AreEqual(&quot;FizzBuzz&quot;, result2);
        ///               }
        ///           }
        ///       }
        /// </code>
        /// </example>
        /// <example>
        ///     <code>
        ///       using System.Xml;
        ///       using Microsoft.VisualStudio.TestTools.UnitTesting;
        ///       using ExtensionMethods.Object;
        ///
        ///       namespace ExtensionMethods.Examples
        ///       {
        ///           [TestClass]
        ///           public class System_Object_GetValueOrDefault
        ///           {
        ///               [TestMethod]
        ///               public void GetValueOrDefault()
        ///               {
        ///                   // Type
        ///                   var @this = new XmlDocument();
        ///
        ///                   // Exemples
        ///                   string result1 = @this.GetValueOrDefault(x =&gt; x.FirstChild.InnerXml, &quot;FizzBuzz&quot;); // return &quot;FizzBuzz&quot;;
        ///                   string result2 = @this.GetValueOrDefault(x =&gt; x.FirstChild.InnerXml, () =&gt; &quot;FizzBuzz&quot;); // return &quot;FizzBuzz&quot;
        ///
        ///                   // Unit Test
        ///                   Assert.AreEqual(&quot;FizzBuzz&quot;, result1);
        ///                   Assert.AreEqual(&quot;FizzBuzz&quot;, result2);
        ///               }
        ///           }
        ///       }
        /// </code>
        /// </example>
        /// <example>
        ///     <code>
        ///           using System.Xml;
        ///           using Microsoft.VisualStudio.TestTools.UnitTesting;
        ///           using Z.ExtensionMethods.Object;
        ///
        ///           namespace ExtensionMethods.Examples
        ///           {
        ///               [TestClass]
        ///               public class System_Object_GetValueOrDefault
        ///               {
        ///                   [TestMethod]
        ///                   public void GetValueOrDefault()
        ///                   {
        ///                       // Type
        ///                       var @this = new XmlDocument();
        ///
        ///                       // Exemples
        ///                       string result1 = @this.GetValueOrDefault(x =&gt; x.FirstChild.InnerXml, &quot;FizzBuzz&quot;); // return &quot;FizzBuzz&quot;;
        ///                       string result2 = @this.GetValueOrDefault(x =&gt; x.FirstChild.InnerXml, () =&gt; &quot;FizzBuzz&quot;); // return &quot;FizzBuzz&quot;
        ///
        ///                       // Unit Test
        ///                       Assert.AreEqual(&quot;FizzBuzz&quot;, result1);
        ///                       Assert.AreEqual(&quot;FizzBuzz&quot;, result2);
        ///                   }
        ///               }
        ///           }
        ///     </code>
        /// </example>
        public static TResult GetValueOrDefault<T, TResult>(this T @this, Func<T, TResult> func, Func<TResult> defaultValueFactory)
        {
            try
            {
                return func(@this);
            }
            catch (Exception)
            {
                return defaultValueFactory();
            }
        }

        /// <summary>
        ///     A T extension method that gets value or default.
        /// </summary>
        /// <typeparam name="T">Generic type parameter.</typeparam>
        /// <typeparam name="TResult">Type of the result.</typeparam>
        /// <param name="this">The @this to act on.</param>
        /// <param name="func">The function.</param>
        /// <param name="defaultValueFactory">The default value factory.</param>
        /// <returns>The value or default.</returns>
        /// <example>
        ///     <code>
        ///       using System.Xml;
        ///       using Microsoft.VisualStudio.TestTools.UnitTesting;
        ///
        ///
        ///       namespace ExtensionMethods.Examples
        ///       {
        ///           [TestClass]
        ///           public class System_Object_GetValueOrDefault
        ///           {
        ///               [TestMethod]
        ///               public void GetValueOrDefault()
        ///               {
        ///                   // Type
        ///                   var @this = new XmlDocument();
        ///
        ///                   // Exemples
        ///                   string result1 = @this.GetValueOrDefault(x =&gt; x.FirstChild.InnerXml, &quot;FizzBuzz&quot;); // return &quot;FizzBuzz&quot;;
        ///                   string result2 = @this.GetValueOrDefault(x =&gt; x.FirstChild.InnerXml, () =&gt; &quot;FizzBuzz&quot;); // return &quot;FizzBuzz&quot;
        ///
        ///                   // Unit Test
        ///                   Assert.AreEqual(&quot;FizzBuzz&quot;, result1);
        ///                   Assert.AreEqual(&quot;FizzBuzz&quot;, result2);
        ///               }
        ///           }
        ///       }
        /// </code>
        /// </example>
        /// <example>
        ///     <code>
        ///       using System.Xml;
        ///       using Microsoft.VisualStudio.TestTools.UnitTesting;
        ///       using Z.ExtensionMethods.Object;
        ///
        ///       namespace ExtensionMethods.Examples
        ///       {
        ///           [TestClass]
        ///           public class System_Object_GetValueOrDefault
        ///           {
        ///               [TestMethod]
        ///               public void GetValueOrDefault()
        ///               {
        ///                   // Type
        ///                   var @this = new XmlDocument();
        ///
        ///                   // Exemples
        ///                   string result1 = @this.GetValueOrDefault(x =&gt; x.FirstChild.InnerXml, &quot;FizzBuzz&quot;); // return &quot;FizzBuzz&quot;;
        ///                   string result2 = @this.GetValueOrDefault(x =&gt; x.FirstChild.InnerXml, () =&gt; &quot;FizzBuzz&quot;); // return &quot;FizzBuzz&quot;
        ///
        ///                   // Unit Test
        ///                   Assert.AreEqual(&quot;FizzBuzz&quot;, result1);
        ///                   Assert.AreEqual(&quot;FizzBuzz&quot;, result2);
        ///               }
        ///           }
        ///       }
        /// </code>
        /// </example>
        /// <example>
        ///     <code>
        ///           using System.Xml;
        ///           using Microsoft.VisualStudio.TestTools.UnitTesting;
        ///           using ExtensionMethods.Object;
        ///
        ///           namespace ExtensionMethods.Examples
        ///           {
        ///               [TestClass]
        ///               public class System_Object_GetValueOrDefault
        ///               {
        ///                   [TestMethod]
        ///                   public void GetValueOrDefault()
        ///                   {
        ///                       // Type
        ///                       var @this = new XmlDocument();
        ///
        ///                       // Exemples
        ///                       string result1 = @this.GetValueOrDefault(x =&gt; x.FirstChild.InnerXml, &quot;FizzBuzz&quot;); // return &quot;FizzBuzz&quot;;
        ///                       string result2 = @this.GetValueOrDefault(x =&gt; x.FirstChild.InnerXml, () =&gt; &quot;FizzBuzz&quot;); // return &quot;FizzBuzz&quot;
        ///
        ///                       // Unit Test
        ///                       Assert.AreEqual(&quot;FizzBuzz&quot;, result1);
        ///                       Assert.AreEqual(&quot;FizzBuzz&quot;, result2);
        ///                   }
        ///               }
        ///           }
        ///     </code>
        /// </example>
        public static TResult GetValueOrDefault<T, TResult>(this T @this, Func<T, TResult> func, Func<T, TResult> defaultValueFactory)
        {
            try
            {
                return func(@this);
            }
            catch (Exception)
            {
                return defaultValueFactory(@this);
            }
        }

        #endregion GetValueOrDefault

        #region IfNotNull

        /// <summary>A T extension method that execute an action when the value is not null.</summary>
        /// <typeparam name="T">Generic type parameter.</typeparam>
        /// <param name="this">The @this to act on.</param>
        /// <param name="action">The action.</param>
        public static void IfNotNull<T>(this T @this, Action<T> action) where T : class
        {
            if (@this != null)
            {
                action(@this);
            }
        }

        /// <summary>
        ///     A T extension method that the function result if not null otherwise default value.
        /// </summary>
        /// <typeparam name="T">Generic type parameter.</typeparam>
        /// <typeparam name="TResult">Type of the result.</typeparam>
        /// <param name="this">The @this to act on.</param>
        /// <param name="func">The function.</param>
        /// <returns>The function result if @this is not null otherwise default value.</returns>
        public static TResult IfNotNull<T, TResult>(this T @this, Func<T, TResult> func) where T : class
        {
            return @this != null ? func(@this) : default(TResult);
        }

        /// <summary>
        ///     A T extension method that the function result if not null otherwise default value.
        /// </summary>
        /// <typeparam name="T">Generic type parameter.</typeparam>
        /// <typeparam name="TResult">Type of the result.</typeparam>
        /// <param name="this">The @this to act on.</param>
        /// <param name="func">The function.</param>
        /// <param name="defaultValue">The default value.</param>
        /// <returns>The function result if @this is not null otherwise default value.</returns>
        public static TResult IfNotNull<T, TResult>(this T @this, Func<T, TResult> func, TResult defaultValue) where T : class
        {
            return @this != null ? func(@this) : defaultValue;
        }

        /// <summary>
        ///     A T extension method that the function result if not null otherwise default value.
        /// </summary>
        /// <typeparam name="T">Generic type parameter.</typeparam>
        /// <typeparam name="TResult">Type of the result.</typeparam>
        /// <param name="this">The @this to act on.</param>
        /// <param name="func">The function.</param>
        /// <param name="defaultValueFactory">The default value factory.</param>
        /// <returns>The function result if @this is not null otherwise default value.</returns>
        public static TResult IfNotNull<T, TResult>(this T @this, Func<T, TResult> func, Func<TResult> defaultValueFactory) where T : class
        {
            return @this != null ? func(@this) : defaultValueFactory();
        }

        #endregion IfNotNull

        #region NullIf

        /// <summary>
        ///     A T extension method that null if.
        /// </summary>
        /// <typeparam name="T">Generic type parameter.</typeparam>
        /// <param name="this">The @this to act on.</param>
        /// <param name="predicate">The predicate.</param>
        /// <returns>A T.</returns>
        public static T NullIf<T>(this T @this, Func<T, bool> predicate) where T : class
        {
            return predicate(@this) ? null : @this;
        }

        #endregion NullIf

        #region NullIfEquals

        /// <summary>
        ///     A T extension method that null if equals.
        /// </summary>
        /// <typeparam name="T">Generic type parameter.</typeparam>
        /// <param name="this">The @this to act on.</param>
        /// <param name="value">The value.</param>
        /// <returns>A T.</returns>
        public static T NullIfEquals<T>(this T @this, T value) where T : class
        {
            return @this.Equals(value) ? null : @this;
        }

        #endregion NullIfEquals

        #region NullIfEqualsAny

        /// <summary>
        ///     A T extension method that null if equals any.
        /// </summary>
        /// <typeparam name="T">Generic type parameter.</typeparam>
        /// <param name="this">The @this to act on.</param>
        /// <param name="values">A variable-length parameters list containing values.</param>
        /// <returns>A T.</returns>
        public static T NullIfEqualsAny<T>(this T @this, params T[] values) where T : class
        {
            return Array.IndexOf(values, @this) != -1 ? null : @this;
        }

        #endregion NullIfEqualsAny

        #region ToStringSafe

        /// <summary>
        ///     An object extension method that converts the @this to string or return an empty string if the value is null.
        /// </summary>
        /// <param name="this">The @this to act on.</param>
        /// <returns>@this as a string or empty if the value is null.</returns>
        public static string ToStringSafe(this object @this)
        {
            return @this == null ? "" : @this.ToString();
        }

        #endregion ToStringSafe

        #region Try

        /// <summary>A TType extension method that tries.</summary>
        /// <typeparam name="TType">Type of the type.</typeparam>
        /// <typeparam name="TResult">Type of the result.</typeparam>
        /// <param name="this">The @this to act on.</param>
        /// <param name="tryFunction">The try function.</param>
        /// <returns>A TResult.</returns>
        public static TResult Try<TType, TResult>(this TType @this, Func<TType, TResult> tryFunction)
        {
            try
            {
                return tryFunction(@this);
            }
            catch
            {
                return default(TResult);
            }
        }

        /// <summary>A TType extension method that tries.</summary>
        /// <typeparam name="TType">Type of the type.</typeparam>
        /// <typeparam name="TResult">Type of the result.</typeparam>
        /// <param name="this">The @this to act on.</param>
        /// <param name="tryFunction">The try function.</param>
        /// <param name="catchValue">The catch value.</param>
        /// <returns>A TResult.</returns>
        public static TResult Try<TType, TResult>(this TType @this, Func<TType, TResult> tryFunction, TResult catchValue)
        {
            try
            {
                return tryFunction(@this);
            }
            catch
            {
                return catchValue;
            }
        }

        /// <summary>A TType extension method that tries.</summary>
        /// <typeparam name="TType">Type of the type.</typeparam>
        /// <typeparam name="TResult">Type of the result.</typeparam>
        /// <param name="this">The @this to act on.</param>
        /// <param name="tryFunction">The try function.</param>
        /// <param name="catchValueFactory">The catch value factory.</param>
        /// <returns>A TResult.</returns>
        public static TResult Try<TType, TResult>(this TType @this, Func<TType, TResult> tryFunction, Func<TType, TResult> catchValueFactory)
        {
            try
            {
                return tryFunction(@this);
            }
            catch
            {
                return catchValueFactory(@this);
            }
        }

        /// <summary>A TType extension method that tries.</summary>
        /// <typeparam name="TType">Type of the type.</typeparam>
        /// <typeparam name="TResult">Type of the result.</typeparam>
        /// <param name="this">The @this to act on.</param>
        /// <param name="tryFunction">The try function.</param>
        /// <param name="result">[out] The result.</param>
        /// <returns>A TResult.</returns>
        public static bool Try<TType, TResult>(this TType @this, Func<TType, TResult> tryFunction, out TResult result)
        {
            try
            {
                result = tryFunction(@this);
                return true;
            }
            catch
            {
                result = default(TResult);
                return false;
            }
        }

        /// <summary>A TType extension method that tries.</summary>
        /// <typeparam name="TType">Type of the type.</typeparam>
        /// <typeparam name="TResult">Type of the result.</typeparam>
        /// <param name="this">The @this to act on.</param>
        /// <param name="tryFunction">The try function.</param>
        /// <param name="catchValue">The catch value.</param>
        /// <param name="result">[out] The result.</param>
        /// <returns>A TResult.</returns>
        public static bool Try<TType, TResult>(this TType @this, Func<TType, TResult> tryFunction, TResult catchValue, out TResult result)
        {
            try
            {
                result = tryFunction(@this);
                return true;
            }
            catch
            {
                result = catchValue;
                return false;
            }
        }

        /// <summary>A TType extension method that tries.</summary>
        /// <typeparam name="TType">Type of the type.</typeparam>
        /// <typeparam name="TResult">Type of the result.</typeparam>
        /// <param name="this">The @this to act on.</param>
        /// <param name="tryFunction">The try function.</param>
        /// <param name="catchValueFactory">The catch value factory.</param>
        /// <param name="result">[out] The result.</param>
        /// <returns>A TResult.</returns>
        public static bool Try<TType, TResult>(this TType @this, Func<TType, TResult> tryFunction, Func<TType, TResult> catchValueFactory, out TResult result)
        {
            try
            {
                result = tryFunction(@this);
                return true;
            }
            catch
            {
                result = catchValueFactory(@this);
                return false;
            }
        }

        /// <summary>A TType extension method that attempts to action from the given data.</summary>
        /// <typeparam name="TType">Type of the type.</typeparam>
        /// <param name="this">The @this to act on.</param>
        /// <param name="tryAction">The try action.</param>
        /// <returns>true if it succeeds, false if it fails.</returns>
        public static bool Try<TType>(this TType @this, Action<TType> tryAction)
        {
            try
            {
                tryAction(@this);
                return true;
            }
            catch
            {
                return false;
            }
        }

        /// <summary>A TType extension method that attempts to action from the given data.</summary>
        /// <typeparam name="TType">Type of the type.</typeparam>
        /// <param name="this">The @this to act on.</param>
        /// <param name="tryAction">The try action.</param>
        /// <param name="catchAction">The catch action.</param>
        /// <returns>true if it succeeds, false if it fails.</returns>
        public static bool Try<TType>(this TType @this, Action<TType> tryAction, Action<TType> catchAction)
        {
            try
            {
                tryAction(@this);
                return true;
            }
            catch
            {
                catchAction(@this);
                return false;
            }
        }

        #endregion Try

        #endregion Utility

        #region ValueComparison

        #region Between

        /// <summary>
        ///     A T extension method that check if the value is between (exclusif) the minValue and maxValue.
        /// </summary>
        /// <typeparam name="T">Generic type parameter.</typeparam>
        /// <param name="this">The @this to act on.</param>
        /// <param name="minValue">The minimum value.</param>
        /// <param name="maxValue">The maximum value.</param>
        /// <returns>true if the value is between the minValue and maxValue, otherwise false.</returns>
        public static bool Between<T>(this T @this, T minValue, T maxValue) where T : IComparable<T>
        {
            return minValue.CompareTo(@this) == -1 && @this.CompareTo(maxValue) == -1;
        }

        #endregion Between

        #region In

        /// <summary>
        ///     A T extension method to determines whether the object is equal to any of the provided values.
        /// </summary>
        /// <typeparam name="T">Generic type parameter.</typeparam>
        /// <param name="this">The object to be compared.</param>
        /// <param name="values">The value list to compare with the object.</param>
        /// <returns>true if the values list contains the object, else false.</returns>
        public static bool In<T>(this T @this, params T[] values)
        {
            return Array.IndexOf(values, @this) != -1;
        }

        #endregion In

        #region InRange

        /// <summary>
        ///     A T extension method that check if the value is between inclusively the minValue and maxValue.
        /// </summary>
        /// <typeparam name="T">Generic type parameter.</typeparam>
        /// <param name="this">The @this to act on.</param>
        /// <param name="minValue">The minimum value.</param>
        /// <param name="maxValue">The maximum value.</param>
        /// <returns>true if the value is between inclusively the minValue and maxValue, otherwise false.</returns>
        public static bool InRange<T>(this T @this, T minValue, T maxValue) where T : IComparable<T>
        {
            return @this.CompareTo(minValue) >= 0 && @this.CompareTo(maxValue) <= 0;
        }

        #endregion InRange

        #region IsDBNull

        /// <summary>
        ///     Returns an indication whether the specified object is of type .
        /// </summary>
        /// <typeparam name="T">Generic type parameter.</typeparam>
        /// <param name="value">An object.</param>
        /// <returns>true if  is of type ; otherwise, false.</returns>
        public static bool IsDbNull<T>(this T value) where T : class
        {
            return Convert.IsDBNull(value);
        }

        #endregion IsDBNull

        #region IsDefault

        /// <summary>
        ///     A T extension method that query if 'source' is the default value.
        /// </summary>
        /// <typeparam name="T">Generic type parameter.</typeparam>
        /// <param name="source">The source to act on.</param>
        /// <returns>true if default, false if not.</returns>
        public static bool IsDefault<T>(this T source)
        {
            return source.Equals(default(T));
        }

        #endregion IsDefault

        #region IsNotNull

        /// <summary>
        ///     A T extension method that query if '@this' is not null.
        /// </summary>
        /// <typeparam name="T">Generic type parameter.</typeparam>
        /// <param name="this">The @this to act on.</param>
        /// <returns>true if not null, false if not.</returns>
        public static bool IsNotNull<T>(this T @this) where T : class
        {
            return @this != null;
        }

        #endregion IsNotNull

        #region IsNull

        /// <summary>
        ///     A T extension method that query if '@this' is null.
        /// </summary>
        /// <typeparam name="T">Generic type parameter.</typeparam>
        /// <param name="this">The @this to act on.</param>
        /// <returns>true if null, false if not.</returns>
        public static bool IsNull<T>(this T @this) where T : class
        {
            return @this == null;
        }

        #endregion IsNull

        #region NotIn

        /// <summary>
        ///     A T extension method to determines whether the object is not equal to any of the provided values.
        /// </summary>
        /// <typeparam name="T">Generic type parameter.</typeparam>
        /// <param name="this">The object to be compared.</param>
        /// <param name="values">The value list to compare with the object.</param>
        /// <returns>true if the values list doesn't contains the object, else false.</returns>
        public static bool NotIn<T>(this T @this, params T[] values)
        {
            return Array.IndexOf(values, @this) == -1;
        }

        #endregion NotIn

        #region ReferenceEquals

        /// <summary>
        ///     Determines whether the specified  instances are the same instance.
        /// </summary>
        /// <param name="objA">The first object to compare.</param>
        /// <param name="objB">The second object  to compare.</param>
        /// <returns>true if  is the same instance as  or if both are null; otherwise, false.</returns>
        public static bool ExReferenceEquals(this object objA, object objB)
        {
            return ReferenceEquals(objA, objB);
        }

        #endregion ReferenceEquals

        #endregion ValueComparison
    }
}