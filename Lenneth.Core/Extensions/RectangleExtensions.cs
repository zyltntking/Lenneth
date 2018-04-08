using System.Collections.Generic;
using System.Diagnostics;
using System.Drawing;
using System.Xml.Linq;

namespace Lenneth.Core.Extensions
{
    [DebuggerStepThrough]
    public static class RectangleExtensions
    {
        public static Rectangle Scale(this Rectangle aRect, int aRatio)
        {
            return new Rectangle(aRect.Left * aRatio, aRect.Top * aRatio,
                aRect.Width * aRatio, aRect.Height * aRatio);
        }

        /// <summary>
        /// Round mode: Math.Round()
        /// </summary>
        /// <param name="aRect"></param>
        /// <param name="aRatio"></param>
        /// <returns></returns>
        public static Rectangle Scale(this Rectangle aRect, double aRatio)
        {
             return Rectangle.FromLTRB(
               (aRect.Left * aRatio).Round(),
               (aRect.Top * aRatio).Round(),
               (aRect.Right * aRatio).Round(),
               (aRect.Bottom * aRatio).Round());
        }

        public static IEnumerable<Point> EnumPixels(this Rectangle aRect)
        {
            for (var y = aRect.Top; y < aRect.Bottom; y++)
            {
                for (var x = aRect.Left; x < aRect.Right; x++)
                {
                    yield return new Point(x, y);
                }
            }
        }

        public static XElement GetAsXml(this Rectangle aRect, string aName)
        {
            return new XElement(aName,
                new XElement("Left", aRect.Left),
                new XElement("Top", aRect.Top),
                new XElement("Width", aRect.Width),
                new XElement("Height", aRect.Height));
        }

        public static Rectangle FromXml(XElement aElement)
        {
            return new Rectangle(
                aElement.Element("Left").Value.ToInt(),
                aElement.Element("Top").Value.ToInt(),
                aElement.Element("Width").Value.ToInt(),
                aElement.Element("Height").Value.ToInt());
        }
    }
}