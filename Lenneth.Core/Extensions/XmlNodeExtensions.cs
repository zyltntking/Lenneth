using System.Diagnostics;
using System.Xml;

namespace Lenneth.Core.Extensions
{
    [DebuggerStepThrough]
    public static class XmlNodeExtensions
    {
        public static XmlNode CreateChildNode(this XmlNode aNode, string aName)
        {
            var document = (aNode is XmlDocument xmlDocument ? xmlDocument : aNode.OwnerDocument);
            XmlNode node = document.CreateElement(aName);
            aNode.AppendChild(node);
            return node;
        }
    }
}