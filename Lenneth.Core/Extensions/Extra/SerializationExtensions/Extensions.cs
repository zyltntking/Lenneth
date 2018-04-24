using System.IO;
using System.Runtime.Serialization.Formatters.Binary;
using System.Runtime.Serialization.Json;
using System.Text;
using System.Web.Script.Serialization;
using System.Xml.Serialization;

namespace Lenneth.Core.Extensions.Extra.SerializationExtensions
{
    public static class Extensions
    {
        #region Object

        #region SerializeBinary

        /// <summary>
        ///     An object extension method that serialize an object to binary.
        /// </summary>
        /// <typeparam name="T">Generic type parameter.</typeparam>
        /// <param name="this">The @this to act on.</param>
        /// <returns>A string.</returns>
        public static string SerializeBinary<T>(this T @this)
        {
            var binaryWrite = new BinaryFormatter();

            using (var memoryStream = new MemoryStream())
            {
                binaryWrite.Serialize(memoryStream, @this);
                return Encoding.Default.GetString(memoryStream.ToArray());
            }
        }

        /// <summary>
        ///     An object extension method that serialize an object to binary.
        /// </summary>
        /// <typeparam name="T">Generic type parameter.</typeparam>
        /// <param name="this">The @this to act on.</param>
        /// <param name="encoding">The encoding.</param>
        /// <returns>A string.</returns>
        public static string SerializeBinary<T>(this T @this, Encoding encoding)
        {
            var binaryWrite = new BinaryFormatter();

            using (var memoryStream = new MemoryStream())
            {
                binaryWrite.Serialize(memoryStream, @this);
                return encoding.GetString(memoryStream.ToArray());
            }
        }

        #endregion SerializeBinary

        #region SerializeJavaScript

        /// <summary>
        ///     A T extension method that serialize java script.
        /// </summary>
        /// <typeparam name="T">Generic type parameter.</typeparam>
        /// <param name="this">The @this to act on.</param>
        /// <returns>A string.</returns>
        public static string SerializeJavaScript<T>(this T @this)
        {
            var serializer = new JavaScriptSerializer();
            return serializer.Serialize(@this);
        }

        #endregion SerializeJavaScript

        #region SerializeJson

        /// <summary>
        ///     A T extension method that serialize an object to Json.
        /// </summary>
        /// <typeparam name="T">Generic type parameter.</typeparam>
        /// <param name="this">The @this to act on.</param>
        /// <returns>The Json string.</returns>
        public static string SerializeJson<T>(this T @this)
        {
            var serializer = new DataContractJsonSerializer(typeof(T));

            using (var memoryStream = new MemoryStream())
            {
                serializer.WriteObject(memoryStream, @this);
                return Encoding.Default.GetString(memoryStream.ToArray());
            }
        }

        /// <summary>
        ///     A T extension method that serialize an object to Json.
        /// </summary>
        /// <typeparam name="T">Generic type parameter.</typeparam>
        /// <param name="this">The @this to act on.</param>
        /// <param name="encoding">The encoding.</param>
        /// <returns>The Json string.</returns>
        public static string SerializeJson<T>(this T @this, Encoding encoding)
        {
            var serializer = new DataContractJsonSerializer(typeof(T));

            using (var memoryStream = new MemoryStream())
            {
                serializer.WriteObject(memoryStream, @this);
                return encoding.GetString(memoryStream.ToArray());
            }
        }

        #endregion SerializeJson

        #region SerializeXml

        /// <summary>
        ///     An object extension method that serialize a string to XML.
        /// </summary>
        /// <param name="this">The @this to act on.</param>
        /// <returns>The string representation of the Xml Serialization.</returns>
        public static string SerializeXml(this object @this)
        {
            var xmlSerializer = new XmlSerializer(@this.GetType());

            using (var stringWriter = new StringWriter())
            {
                xmlSerializer.Serialize(stringWriter, @this);
                using (var streamReader = new StringReader(stringWriter.GetStringBuilder().ToString()))
                {
                    return streamReader.ReadToEnd();
                }
            }
        }

        #endregion SerializeXml

        #endregion Object

        #region String

        #region DeserializeBinary

        /// <summary>
        ///     A string extension method that deserialize a string binary as &lt;T&gt;.
        /// </summary>
        /// <typeparam name="T">Generic type parameter.</typeparam>
        /// <param name="this">The @this to act on.</param>
        /// <returns>The desrialize binary as &lt;T&gt;</returns>
        public static T DeserializeBinary<T>(this string @this)
        {
            using (var stream = new MemoryStream(Encoding.Default.GetBytes(@this)))
            {
                var binaryRead = new BinaryFormatter();
                return (T)binaryRead.Deserialize(stream);
            }
        }

        /// <summary>
        ///     A string extension method that deserialize a string binary as &lt;T&gt;.
        /// </summary>
        /// <typeparam name="T">Generic type parameter.</typeparam>
        /// <param name="this">The @this to act on.</param>
        /// <param name="encoding">The encoding.</param>
        /// <returns>The desrialize binary as &lt;T&gt;</returns>
        public static T DeserializeBinary<T>(this string @this, Encoding encoding)
        {
            using (var stream = new MemoryStream(encoding.GetBytes(@this)))
            {
                var binaryRead = new BinaryFormatter();
                return (T)binaryRead.Deserialize(stream);
            }
        }

        #endregion DeserializeBinary

        #region DeserializeJavaScript

        /// <summary>
        ///     A string extension method that deserialize a string binary as &lt;T&gt;.
        /// </summary>
        /// <typeparam name="T">Generic type parameter.</typeparam>
        /// <param name="this">The @this to act on.</param>
        /// <returns>The desrialize binary as &lt;T&gt;</returns>
        public static T DeserializeJavaScript<T>(this string @this)
        {
            var serializer = new JavaScriptSerializer();
            return serializer.Deserialize<T>(@this);
        }

        #endregion DeserializeJavaScript

        #region DeserializeJson

        /// <summary>
        ///     A string extension method that deserialize a Json string to object.
        /// </summary>
        /// <typeparam name="T">Generic type parameter.</typeparam>
        /// <param name="this">The @this to act on.</param>
        /// <returns>The object deserialized.</returns>
        public static T DeserializeJson<T>(this string @this)
        {
            var serializer = new DataContractJsonSerializer(typeof(T));

            using (var stream = new MemoryStream(Encoding.Default.GetBytes(@this)))
            {
                return (T)serializer.ReadObject(stream);
            }
        }

        /// <summary>
        ///     A string extension method that deserialize a Json string to object.
        /// </summary>
        /// <typeparam name="T">Generic type parameter.</typeparam>
        /// <param name="this">The @this to act on.</param>
        /// <param name="encoding">The encoding.</param>
        /// <returns>The object deserialized.</returns>
        public static T DeserializeJson<T>(this string @this, Encoding encoding)
        {
            var serializer = new DataContractJsonSerializer(typeof(T));

            using (var stream = new MemoryStream(encoding.GetBytes(@this)))
            {
                return (T)serializer.ReadObject(stream);
            }
        }

        #endregion DeserializeJson

        #region DeserializeXml

        /// <summary>
        ///     A string extension method that deserialize an Xml as &lt;T&gt;.
        /// </summary>
        /// <typeparam name="T">Generic type parameter.</typeparam>
        /// <param name="this">The @this to act on.</param>
        /// <returns>The desieralize Xml as &lt;T&gt;</returns>
        public static T DeserializeXml<T>(this string @this)
        {
            var x = new XmlSerializer(typeof(T));
            var r = new StringReader(@this);

            return (T)x.Deserialize(r);
        }

        #endregion DeserializeXml

        #endregion String
    }
}