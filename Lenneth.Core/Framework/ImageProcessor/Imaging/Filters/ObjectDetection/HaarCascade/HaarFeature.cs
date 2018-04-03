﻿// Accord Vision Library
// The Accord.NET Framework (LGPL)
// http://accord-framework.net
//
// Copyright © César Souza, 2009-2015
// cesarsouza at gmail.com
//
//    This library is free software; you can redistribute it and/or
//    modify it under the terms of the GNU Lesser General Public
//    License as published by the Free Software Foundation; either
//    version 2.1 of the License, or (at your option) any later version.
//
//    This library is distributed in the hope that it will be useful,
//    but WITHOUT ANY WARRANTY; without even the implied warranty of
//    MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the GNU
//    Lesser General Public License for more details.
//
//    You should have received a copy of the GNU Lesser General Public
//    License along with this library; if not, write to the Free Software
//    Foundation, Inc., 51 Franklin St, Fifth Floor, Boston, MA  02110-1301  USA
//

using System;
using System.Collections.Generic;
using System.Xml;
using System.Xml.Schema;
using System.Xml.Serialization;

namespace Lenneth.Core.Framework.ImageProcessor.Imaging.Filters.ObjectDetection.HaarCascade
{
    /// <summary>
    ///   Rectangular Haar-like feature container.
    /// </summary>
    /// <remarks>
    ///   References:
    ///   - http://en.wikipedia.org/wiki/Haar-like_features#Rectangular_Haar-like_features
    /// </remarks>
    [Serializable]
    public sealed class HaarFeature : IXmlSerializable, ICloneable
    {
        /// <summary>
        ///   Gets or sets whether this feature is tilted.
        /// </summary>
        /// 
        public bool Tilted { get; set; }

        /// <summary>
        ///   Gets or sets the Haar rectangles for this feature.
        /// </summary>
        /// 
        public HaarRectangle[] Rectangles { get; set; }


        /// <summary>
        ///   Constructs a new Haar-like feature.
        /// </summary>
        /// 
        public HaarFeature()
        {
            Rectangles = new HaarRectangle[2];
        }

        /// <summary>
        ///   Constructs a new Haar-like feature.
        /// </summary>
        /// 
        public HaarFeature(params HaarRectangle[] rectangles)
        {
            Rectangles = rectangles;
        }

        /// <summary>
        ///   Constructs a new Haar-like feature.
        /// </summary>
        /// 
        public HaarFeature(params int[][] rectangles)
            : this(false, rectangles) { }

        /// <summary>
        ///   Constructs a new Haar-like feature.
        /// </summary>
        /// 
        public HaarFeature(bool tilted, params int[][] rectangles)
        {
            Tilted = tilted;
            Rectangles = new HaarRectangle[rectangles.Length];
            for (var i = 0; i < rectangles.Length; i++)
                Rectangles[i] = new HaarRectangle(rectangles[i]);
        }

        /// <summary>
        /// Gets the sum of the areas of the rectangular features in an integral image.
        /// </summary>
        /// <param name="image">The <see cref="FastBitmap"/> containing integral rectangle information.</param>
        /// <param name="x">The x coordinate.</param>
        /// <param name="y">The y coordinate.</param>
        /// <returns>
        /// The <see cref="double"/> representing the sum.
        /// </returns>
        public double GetSum(FastBitmap image, int x, int y)
        {
            var sum = 0.0;

            if (!Tilted)
            {
                // Compute the sum for a standard feature
                foreach (var rect in Rectangles)
                {
                    sum += image.GetSum(x + rect.ScaledX, y + rect.ScaledY,
                        rect.ScaledWidth, rect.ScaledHeight) * rect.ScaledWeight;
                }
            }
            else
            {
                // Compute the sum for a rotated feature
                foreach (var rect in Rectangles)
                {
                    sum += image.GetSumT(x + rect.ScaledX, y + rect.ScaledY,
                        rect.ScaledWidth, rect.ScaledHeight) * rect.ScaledWeight;
                }
            }

            return sum;
        }

        /// <summary>
        ///   Sets the scale and weight of a Haar-like rectangular feature container.
        /// </summary>
        /// 
        public void SetScaleAndWeight(float scale, float weight)
        {
            // Manual loop unfolding
            if (Rectangles.Length == 2)
            {
                var a = Rectangles[0];
                var b = Rectangles[1];

                b.ScaleRectangle(scale);
                b.ScaleWeight(weight);

                a.ScaleRectangle(scale);
                a.ScaledWeight = -(b.Area * b.ScaledWeight) / a.Area;
            }
            else // rectangles.Length == 3
            {
                var a = Rectangles[0];
                var b = Rectangles[1];
                var c = Rectangles[2];

                c.ScaleRectangle(scale);
                c.ScaleWeight(weight);

                b.ScaleRectangle(scale);
                b.ScaleWeight(weight);

                a.ScaleRectangle(scale);
                a.ScaledWeight = -(b.Area * b.ScaledWeight
                    + c.Area * c.ScaledWeight) / (a.Area);
            }
        }


        #region IXmlSerializable Members

        XmlSchema IXmlSerializable.GetSchema()
        {
            throw new NotSupportedException();
        }

        void IXmlSerializable.ReadXml(XmlReader reader)
        {
            reader.ReadStartElement("feature");

            reader.ReadToFollowing("rects");
            reader.ReadToFollowing("_");

            var rec = new List<HaarRectangle>();
            while (reader.Name == "_")
            {
                var str = reader.ReadElementContentAsString();
                rec.Add(HaarRectangle.Parse(str));

                while (reader.Name != "_" && reader.Name != "tilted" &&
                    reader.NodeType != XmlNodeType.EndElement)
                    reader.Read();
            }

            Rectangles = rec.ToArray();

            reader.ReadToFollowing("tilted", reader.BaseURI);
            Tilted = reader.ReadElementContentAsInt() == 1;

            reader.ReadEndElement();
        }

        void IXmlSerializable.WriteXml(XmlWriter writer)
        {
            throw new NotSupportedException();
        }

        #endregion



        /// <summary>
        ///   Creates a new object that is a copy of the current instance.
        /// </summary>
        /// <returns>
        ///   A new object that is a copy of this instance.
        /// </returns>
        public object Clone()
        {
            var newRectangles = new HaarRectangle[Rectangles.Length];
            for (var i = 0; i < newRectangles.Length; i++)
            {
                var rect = Rectangles[i];
                newRectangles[i] = new HaarRectangle(rect.X, rect.Y, rect.Width, rect.Height, rect.Weight);
            }

            return new HaarFeature { Rectangles = newRectangles, Tilted = Tilted };
        }
    }
}