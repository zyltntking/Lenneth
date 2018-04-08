using System.Xml;

namespace Lenneth.Core.Extensions.Utils
{
    public class NoNamespacesXmlWriter : XmlWriter
    {
        private readonly XmlWriter _mWriter;

        public NoNamespacesXmlWriter(XmlWriter aWriter)
        {
            if (aWriter is NoNamespacesXmlWriter)
                _mWriter = ((NoNamespacesXmlWriter) aWriter)._mWriter;
            else
                _mWriter = aWriter;
        }

        public override void Close()
        {
            _mWriter.Close();
        }

        public override void Flush()
        {
            _mWriter.Flush();
        }

        public override string LookupPrefix(string aNs)
        {
            if (aNs == "http://www.w3.org/2001/XMLSchema")
                return "";
            else if (aNs == "http://www.w3.org/2001/XMLSchema-instance")
                return "";

            return _mWriter.LookupPrefix(aNs);
        }

        public override void WriteBase64(byte[] aBuffer, int aIndex, int aCount)
        {
            _mWriter.WriteBase64(aBuffer, aIndex, aCount);
        }

        public override void WriteCData(string aText)
        {
            _mWriter.WriteCData(aText);
        }

        public override void WriteCharEntity(char aCh)
        {
            _mWriter.WriteCharEntity(aCh);
        }

        public override void WriteChars(char[] aBuffer, int aIndex, int aCount)
        {
            _mWriter.WriteChars(aBuffer, aIndex, aCount);
        }

        public override void WriteComment(string aText)
        {
            _mWriter.WriteComment(aText);
        }

        public override void WriteDocType(string aName, string aPubid,
            string aSysid, string aSubset)
        {
            _mWriter.WriteDocType(aName, aPubid, aSysid, aSubset);
        }

        public override void WriteEndAttribute()
        {
            _mWriter.WriteEndAttribute();
        }

        public override void WriteEndDocument()
        {
            _mWriter.WriteEndDocument();
        }

        public override void WriteEndElement()
        {
            _mWriter.WriteEndElement();
        }

        public override void WriteEntityRef(string aName)
        {
            _mWriter.WriteEntityRef(aName);
        }

        public override void WriteFullEndElement()
        {
            _mWriter.WriteFullEndElement();
        }

        public override void WriteProcessingInstruction(string aName, string aText)
        {
            _mWriter.WriteProcessingInstruction(aName, aText);
        }

        public override void WriteRaw(string aData)
        {
            _mWriter.WriteRaw(aData);
        }

        public override void WriteRaw(char[] aBuffer, int aIndex, int aCount)
        {
            _mWriter.WriteRaw(aBuffer, aIndex, aCount);
        }

        public override void WriteStartAttribute(string aPrefix, string aLocalName, string aNs)
        {
            _mWriter.WriteStartAttribute(aPrefix, aLocalName, aNs);
        }

        public override void WriteStartDocument(bool aStandalone)
        {
            _mWriter.WriteStartDocument(aStandalone);
        }

        public override void WriteStartDocument()
        {
            _mWriter.WriteStartDocument();
        }

        public override void WriteStartElement(string aPrefix, string aLocalName, string aNs)
        {
            _mWriter.WriteStartElement(aPrefix, aLocalName, aNs);
        }

        public override WriteState WriteState => _mWriter.WriteState;

        public override void WriteString(string aText)
        {
            _mWriter.WriteString(aText);
        }

        public override void WriteSurrogateCharEntity(char aLowChar, char aHighChar)
        {
            _mWriter.WriteSurrogateCharEntity(aLowChar, aHighChar);
        }

        public override void WriteWhitespace(string aWs)
        {
            _mWriter.WriteWhitespace(aWs);
        }

        public override bool Equals(object aObj)
        {
            return _mWriter.Equals(aObj);
        }

        public override int GetHashCode()
        {
            return _mWriter.GetHashCode();
        }

        public override string ToString()
        {
            return _mWriter.ToString();
        }

        public override string XmlLang => _mWriter.XmlLang;

        public override XmlSpace XmlSpace => _mWriter.XmlSpace;

        public override XmlWriterSettings Settings => _mWriter.Settings;
    }
}