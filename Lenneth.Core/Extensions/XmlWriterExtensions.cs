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
    public static class XmlWriterExtensions
    {
        public static void WriteHexElement(this XmlWriter aWriter, string aName, byte aByte)
        {
            aWriter.WriteElementString(aName, Hex.ByteToHex(aByte));
        }

        public static void WriteHexElement(this XmlWriter aWriter, string aName, ushort aByte)
        {
            aWriter.WriteElementString(aName, Hex.UShortToHex(aByte));
        }

        public static void WriteHexElement(this XmlWriter aWriter, string aName, uint aByte)
        {
            aWriter.WriteElementString(aName, Hex.UIntToHex(aByte));
        }

        public static void WriteElement<T>(this XmlWriter aWriter, string aName, T aObj)
        {
            aWriter.WriteElementString(aName, aObj.ToString());
        }

        public static void WriteElement(this XmlWriter aWriter, string aName, double aValue)
        {
            aWriter.WriteElementString(aName, aValue.ToString(CultureInfo.InvariantCulture));
        }

        public static void WriteElementSize(this XmlWriter aWriter, string aName, Size aSize)
        {
            aWriter.WriteStartElement(aName);
            aWriter.WriteAttribute("Width", aSize.Width);
            aWriter.WriteAttribute("Height", aSize.Height);
            aWriter.WriteEndElement();
        }

        public static void WriteElementRectangle(this XmlWriter aWriter, string aName,
            Rectangle aRect)
        {
            aWriter.WriteStartElement(aName);
            aWriter.WriteAttribute("Left", aRect.Left);
            aWriter.WriteAttribute("Top", aRect.Top);
            aWriter.WriteAttribute("Width", aRect.Width);
            aWriter.WriteAttribute("Height", aRect.Height);
            aWriter.WriteEndElement();
        }

        public static void WriteElementEnum(this XmlWriter aWriter, string aName, Enum aEnum)
        {
            aWriter.WriteElementString(aName, aEnum.ToString().Replace(", ", " "));
        }

        public static void WriteHexAttribute(this XmlWriter aWriter, string aName, byte aByte)
        {
            aWriter.WriteAttributeString(aName, Hex.ByteToHex(aByte));
        }

        public static void WriteHexAttribute(this XmlWriter aWriter, string aName, ushort aByte)
        {
            aWriter.WriteAttributeString(aName, Hex.UShortToHex(aByte));
        }

        public static void WriteHexAttribute(this XmlWriter aWriter, string aName, uint aByte)
        {
            aWriter.WriteAttributeString(aName, Hex.UIntToHex(aByte));
        }

        public static void WriteAttribute<T>(this XmlWriter aWriter, string aName, T aObj)
        {
            aWriter.WriteAttributeString(aName, aObj.ToString());
        }

        public static void WriteAttributeEnum(this XmlWriter aWriter, string aName, Enum aEnum)
        {
            aWriter.WriteAttributeString(aName, aEnum.ToString().Replace(", ", " "));
        }

        /// <summary>
        /// Helper for xml writing.
        /// </summary>
        /// <param name="aStream"></param>
        /// <param name="aWriteFunc"></param>
        public static void WriteXml(Stream aStream, Action<XmlWriter> aWriteFunc)
        {
            var settings = new XmlWriterSettings {Indent = true};

            using (XmlWriter writer = new NoNamespacesXmlWriter(XmlWriter.Create(aStream, settings)))
            {
                writer.WriteStartDocument();
                aWriteFunc(writer);
                writer.WriteEndDocument();
            }
        }

        /// <summary>
        /// Helper for xml writing.
        /// </summary>
        /// <param name="aFileName"></param>
        /// <param name="aWriteFunc"></param>
        public static void WriteXml(string aFileName, Action<XmlWriter> aWriteFunc)
        {
            using (var fs = new FileStream(aFileName, FileMode.Create))
                WriteXml(fs, aWriteFunc);
        }
    }
}