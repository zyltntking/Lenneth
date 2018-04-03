// --------------------------------------------------------------------------------------------------------------------
// <copyright file="HaarRectangle.cs" company="James South">
//   Copyright (c) James South.
//   Licensed under the Apache License, Version 2.0.
// </copyright>
// <summary>
//   Represents a rectangle which can be at any position and scale within the original image.
//   Based on original code found in the Accord Framework at <see href="https://github.com/accord-net/framework" />
// </summary>
// --------------------------------------------------------------------------------------------------------------------

using System;
using System.Globalization;

namespace Lenneth.Core.Framework.ImageProcessor.Imaging.Filters.ObjectDetection.HaarCascade
{
    /// <summary>
    /// Represents a rectangle which can be at any position and scale within the original image.
    /// Based on original code found in the Accord Framework at <see href="https://github.com/accord-net/framework"/>
    /// </summary>
    [Serializable]
    public class HaarRectangle : ICloneable
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="HaarRectangle"/> class.
        /// </summary>
        /// <param name="values">
        /// The values for this rectangle.
        /// </param>
        public HaarRectangle(int[] values)
        {
            X = values[0];
            Y = values[1];
            Width = values[2];
            Height = values[3];
            Weight = values[4];
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="HaarRectangle"/> class.
        /// </summary>
        /// <param name="x">
        /// The x coordinate marking the top-left point to apply to this rectangle.
        /// </param>
        /// <param name="y">
        /// The y coordinate marking the top-left point to apply to this rectangle.
        /// </param>
        /// <param name="width">
        /// The width to apply to this rectangle.
        /// </param>
        /// <param name="height">
        /// The height to apply to this rectangle.
        /// </param>
        /// <param name="weight">
        /// The weight to apply to this rectangle.
        /// </param>
        public HaarRectangle(int x, int y, int width, int height, float weight)
        {
            X = x;
            Y = y;
            Width = width;
            Height = height;
            Weight = weight;
        }

        /// <summary>
        /// Prevents a default instance of the <see cref="HaarRectangle"/> class from being created.
        /// </summary>
        private HaarRectangle()
        {
        }

        /// <summary>
        /// Gets or sets the x-coordinate of this Haar feature rectangle.
        /// </summary>
        public int X { get; set; }

        /// <summary>
        /// Gets or sets the y-coordinate of this Haar feature rectangle.
        /// </summary>
        public int Y { get; set; }

        /// <summary>
        /// Gets or sets the width of this Haar feature rectangle.
        /// </summary>
        public int Width { get; set; }

        /// <summary>
        /// Gets or sets the height of this Haar feature rectangle.
        /// </summary>
        public int Height { get; set; }

        /// <summary>
        /// Gets or sets the weight of this Haar feature rectangle.
        /// </summary>
        public float Weight { get; set; }

        /// <summary>
        /// Gets or sets the scaled x-coordinate of this Haar feature rectangle.
        /// </summary>
        public int ScaledX { get; set; }

        /// <summary>
        /// Gets or sets the scaled y-coordinate of this Haar feature rectangle.
        /// </summary>
        public int ScaledY { get; set; }

        /// <summary>
        ///   Gets or sets the scaled width of this Haar feature rectangle.
        /// </summary>
        public int ScaledWidth { get; set; }

        /// <summary>
        ///   Gets or sets the scaled height of this Haar feature rectangle.
        /// </summary>
        public int ScaledHeight { get; set; }

        /// <summary>
        /// Gets or sets the scaled weight of this Haar feature rectangle.
        /// </summary>
        public float ScaledWeight { get; set; }

        /// <summary>
        /// Gets the area of this rectangle.
        /// </summary>
        public int Area
        {
            get { return ScaledWidth * ScaledHeight; }
        }

        /// <summary>
        /// Converts a <see cref="HaarRectangle"/> from a string representation.
        /// </summary>
        /// <param name="value">
        /// The value to parse.
        /// </param>
        /// <returns>
        /// The <see cref="HaarRectangle"/>.
        /// </returns>
        public static HaarRectangle Parse(string value)
        {
            var values = value.Trim().Split(' ');

            var x = int.Parse(values[0], CultureInfo.InvariantCulture);
            var y = int.Parse(values[1], CultureInfo.InvariantCulture);
            var w = int.Parse(values[2], CultureInfo.InvariantCulture);
            var h = int.Parse(values[3], CultureInfo.InvariantCulture);
            var weight = float.Parse(values[4], CultureInfo.InvariantCulture);

            return new HaarRectangle(x, y, w, h, weight);
        }

        /// <summary>
        /// Scales the values of this rectangle.
        /// </summary>
        /// <param name="scale">
        /// The scale.
        /// </param>
        public void ScaleRectangle(float scale)
        {
            ScaledX = (int)(X * scale);
            ScaledY = (int)(Y * scale);
            ScaledWidth = (int)(Width * scale);
            ScaledHeight = (int)(Height * scale);
        }

        /// <summary>
        /// Scales the weight of this rectangle.
        /// </summary>
        /// <param name="scale">
        /// The scale.
        /// </param>
        public void ScaleWeight(float scale)
        {
            ScaledWeight = Weight * scale;
        }

        /// <summary>
        /// Creates a new object that is a copy of the current instance.
        /// </summary>
        /// <returns>
        /// A new object that is a copy of this instance.
        /// </returns>
        public object Clone()
        {
            var r = new HaarRectangle
            {
                Height = Height,
                ScaledHeight = ScaledHeight,
                ScaledWeight = ScaledWeight,
                ScaledWidth = ScaledWidth,
                ScaledX = ScaledX,
                ScaledY = ScaledY,
                Weight = Weight,
                Width = Width,
                X = X,
                Y = Y
            };

            return r;
        }
    }
}
