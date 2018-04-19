using System;
using System.Diagnostics;
using System.Drawing;
using System.Globalization;
using System.IO;
using System.Xml;
using Lenneth.Core.Extensions.Utils;

namespace Lenneth.Core.Extensions
{
    [DebuggerStepThrough]
    public static class XmlReaderExtensions
    {
        public static byte ReadElementContentAsHexByte(this XmlReader aReader, string aName)
        {
            return Hex.HexToByte(aReader.ReadElementString(aName));
        }

        public static ushort ReadElementContentAsHexUShort(this XmlReader aReader, string aName)
        {
            return Hex.HexToUShort(aReader.ReadElementString(aName));
        }

        public static bool ReadElementContentAsBoolean(this XmlReader aReader, string aName)
        {
            return bool.Parse(aReader.ReadElementString(aName));
        }

        public static string ReadElementContentAsString(this XmlReader aReader, string aName)
        {
            return aReader.ReadElementContentAsString(aName, "");
        }

        public static double ReadElementContentAsDouble(this XmlReader aReader, string aName)
        {
            return double.Parse(aReader.ReadElementString(aName.Replace(',', '.')),
                CultureInfo.InvariantCulture);
        }

        public static uint ReadElementContentAsHexUInt(this XmlReader aReader, string aName)
        {
            return Hex.HexToUInt(aReader.ReadElementString(aName));
        }

        public static byte ReadElementContentAsByte(this XmlReader aReader, string aName)
        {
            return byte.Parse(aReader.ReadElementString(aName));
        }

        public static ushort ReadElementContentAsUShort(this XmlReader aReader, string aName)
        {
            return ushort.Parse(aReader.ReadElementString(aName));
        }

        public static uint ReadElementContentAsUInt(this XmlReader aReader, string aName)
        {
            return uint.Parse(aReader.ReadElementString(aName));
        }

        public static Size ReadElementContentAsSize(this XmlReader aReader, string aName)
        {
            var size = new Size(
                aReader.GetAttributeInt("Width"),
                aReader.GetAttributeInt("Height"));
            aReader.MoveToNextElement(aName);
            return size;
        }

        public static Rectangle ReadElementContentAsRectangle(this XmlReader aReader, string aName)
        {
            var rect = new Rectangle(
                aReader.GetAttributeInt("Left"),
                aReader.GetAttributeInt("Top"),
                aReader.GetAttributeInt("Width"),
                aReader.GetAttributeInt("Height"));
            aReader.MoveToNextElement(aName);
            return rect;
        }

        public static ulong ReadElementContentAsULong(this XmlReader aReader, string aName)
        {
            return ulong.Parse(aReader.ReadElementString(aName));
        }

        public static int ReadElementContentAsInt(this XmlReader aReader, string aName)
        {
            return int.Parse(aReader.ReadElementString(aName));
        }

        public static Guid ReadElementContentAsGuid(this XmlReader aReader, string aName)
        {
            return Guid.Parse(aReader.ReadElementString(aName));
        }

        public static T ReadElementContentAsEnum<T>(this XmlReader aReader, string aName)
        {
            return (T)Enum.Parse(typeof(T), aReader.ReadElementString(aName).Replace(" ", ", "));
        }

        public static string GetAttributeDef(this XmlReader aReader, string aName,
            string aDefault = "")
        {
            return aReader.MoveToAttribute(aName) ? aReader.GetAttribute(aName) : aDefault;
        }

        public static byte GetAttributeHexByte(this XmlReader aReader, string aName)
        {
            return Hex.HexToByte(aReader.GetAttribute(aName));
        }

        public static ushort GetAttributeHexUShort(this XmlReader aReader, string aName)
        {
            return Hex.HexToUShort(aReader.GetAttribute(aName));
        }

        public static uint GetAttributeHexUInt(this XmlReader aReader, string aName)
        {
            return Hex.HexToUInt(aReader.GetAttribute(aName));
        }

        public static byte GetAttributeByte(this XmlReader aReader, string aName)
        {
            return byte.Parse(aReader.GetAttribute(aName));
        }

        public static ushort GetAttributeUShort(this XmlReader aReader, string aName)
        {
            return ushort.Parse(aReader.GetAttribute(aName));
        }

        public static uint GetAttributeUInt(this XmlReader aReader, string aName)
        {
            return uint.Parse(aReader.GetAttribute(aName));
        }

        public static ulong GetAttributeULong(this XmlReader aReader, string aName)
        {
            return ulong.Parse(aReader.GetAttribute(aName));
        }

        public static int GetAttributeInt(this XmlReader aReader, string aName)
        {
            return int.Parse(aReader.GetAttribute(aName));
        }

        public static int GetAttributeIntDef(this XmlReader aReader, string aName, int aDefault = 0)
        {
            return aReader.MoveToAttribute(aName) ? int.Parse(aReader.GetAttribute(aName)) : aDefault;
        }

        public static bool GetAttributeBool(this XmlReader aReader, string aName)
        {
            return bool.Parse(aReader.GetAttribute(aName));
        }

        public static bool GetAttributeBoolDef(this XmlReader aReader, string aName,
            bool aDefault = false)
        {
            return aReader.MoveToAttribute(aName) ? bool.Parse(aReader.GetAttribute(aName)) : aDefault;
        }

        public static Guid GetAttributeGuid(this XmlReader aReader, string aName)
        {
            return Guid.Parse(aReader.GetAttribute(aName));
        }

        public static T GetAttributeEnum<T>(this XmlReader aReader, string aName)
        {
            return (T)Enum.Parse(typeof(T), aReader.GetAttribute(aName).Replace(" ", ", "));
        }

        /// <summary>
        /// Use to start read xml.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="a_type"></param>
        /// <param name="aStream"></param>
        /// <param name="aReadFunc"></param>
        public static void ReadXml(Stream aStream, Action<XmlReader> aReadFunc)
        {
            var settings = new XmlReaderSettings {IgnoreWhitespace = true};

            using (var reader = XmlReader.Create(aStream, settings))
            {
                reader.Read();

                if (reader.NodeType == XmlNodeType.XmlDeclaration)
                    reader.Skip();

                aReadFunc(reader);
            }
        }

        /// <summary>
        /// Move to next element. Current element must be empty.
        /// </summary>
        /// <param name="aReader"></param>
        /// <param name="aName"></param>
        public static void MoveToNextElement(this XmlReader aReader, string aName)
        {
            if (aReader.NodeType == XmlNodeType.Attribute)
                aReader.MoveToElement();

            if (aReader.IsEmptyElement)
                aReader.ReadStartElement(aName);
            else
            {
                aReader.ReadStartElement(aName);

                if (aReader.IsStartElement())
                    throw new XmlException();

                aReader.ReadEndElement();
            }
        }
    }
}