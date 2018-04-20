using System;
using System.IO;
using System.Reflection;
using System.Runtime.Serialization;
using System.Runtime.Serialization.Formatters.Binary;

namespace Lenneth.Core.Extensions.Extra.CoreExtensions.ObjectExtensions
{
    public static class Extensions
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

        #endregion IsValidValueType

        #endregion Convert
    }
}