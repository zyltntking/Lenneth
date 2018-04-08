using System.IO;
using System.Xml;

namespace Lenneth.Core.Extensions.Utils
{
    /// <summary>
    /// Base class suitable for all classes that can be saved and loaded from xml.
    /// </summary>
    public abstract class XmlBase
    {
        /// <summary>
        /// Constructor.
        /// </summary>
        public XmlBase()
        {
        }

        /// <summary>
        /// Constructor.
        /// </summary>
        /// <param name="aSource"></param>
        public XmlBase(XmlBase aSource)
        {
            var ms = new MemoryStream();
            aSource.Save(ms);
            ms.Position = 0;
            Load(ms);
        }

        /// <summary>
        /// Constructor.
        /// </summary>
        /// <param name="aReader"></param>
        public XmlBase(XmlReader aReader)
        {
            ReadXml(aReader);
        }

        /// <summary>
        /// Load object from xml file.
        /// </summary>
        /// <param name="aFileName"></param>
        protected virtual void Load(string aFileName)
        {
            using (var fs = new FileStream(aFileName, FileMode.Open, FileAccess.Read))
                Load(fs);
        }

        /// <summary>
        /// Load object from xml stream.
        /// </summary>
        /// <param name="aStream"></param>
        protected virtual void Load(Stream aStream)
        {
            XmlReaderExtensions.ReadXml(aStream, (reader) => ReadXml(reader));
        }

        /// <summary>
        /// Load object from xml.
        /// </summary>
        /// <param name="aReader"></param>
        protected abstract void ReadXml(XmlReader aReader);

        /// <summary>
        /// Save object to xml file.
        /// </summary>
        /// <param name="aFileName"></param>
        public virtual void Save(string aFileName)
        {
            using (var fs = new FileStream(aFileName, FileMode.Create))
                Save(fs);
        }

        /// <summary>
        /// Save object to xml stream.
        /// </summary>
        /// <param name="aStream"></param>
        public virtual void Save(Stream aStream)
        {
            XmlWriterExtensions.WriteXml(aStream, WriteXml);
        }

        /// <summary>
        /// Save object to xml.
        /// </summary>
        /// <param name="aWriter"></param>
        internal protected abstract void WriteXml(XmlWriter aWriter);

        /// <summary>
        /// Compare objects through xml.
        /// </summary>
        /// <param name="aObj"></param>
        /// <returns></returns>
        public override bool Equals(object aObj)
        {
            if (aObj == null)
                return false;

            if (!(aObj is XmlBase xmlBase))
                return false;

            var ms1 = new MemoryStream();
            xmlBase.Save(ms1);

            var ms2 = new MemoryStream();
            Save(ms2);

            return ms1.ToArray().AreSame(ms2.ToArray());
        }

        /// <summary>
        /// Calculate hash through xml.
        /// </summary>
        /// <returns></returns>
        public override int GetHashCode()
        {
            var ms = new MemoryStream();
            Save(ms);
            return ArrayExtensions.GetHashCode(ms.ToArray());
        }
    }
}