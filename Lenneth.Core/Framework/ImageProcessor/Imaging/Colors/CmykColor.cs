﻿// --------------------------------------------------------------------------------------------------------------------
// <copyright file="CmykColor.cs" company="James Jackson-South">
//   Copyright (c) James Jackson-South.
//   Licensed under the Apache License, Version 2.0.
// </copyright>
// <summary>
//   Represents an CMYK (cyan, magenta, yellow, keyline) color.
// </summary>
// --------------------------------------------------------------------------------------------------------------------

using System;
using System.Drawing;
using Lenneth.Core.Framework.ImageProcessor.Common.Extensions;
using Lenneth.Core.Framework.ImageProcessor.Imaging.Helpers;

namespace Lenneth.Core.Framework.ImageProcessor.Imaging.Colors
{
    /// <summary>
    /// Represents an CMYK (cyan, magenta, yellow, keyline) color.
    /// </summary>
    public struct CmykColor : IEquatable<CmykColor>
    {
        /// <summary>
        /// Represents a <see cref="CmykColor"/> that is null.
        /// </summary>
        public static readonly CmykColor Empty = new CmykColor();

        /// <summary>
        /// The cyan color component.
        /// </summary>
        private readonly float c;

        /// <summary>
        /// The magenta color component.
        /// </summary>
        private readonly float m;

        /// <summary>
        /// The yellow color component.
        /// </summary>
        private readonly float y;

        /// <summary>
        /// The keyline black color component.
        /// </summary>
        private readonly float k;

        /// <summary>
        /// Initializes a new instance of the <see cref="CmykColor"/> struct.
        /// </summary>
        /// <param name="cyan">
        /// The cyan component.
        /// </param>
        /// <param name="magenta">
        /// The magenta component.
        /// </param>
        /// <param name="yellow">
        /// The yellow component.
        /// </param>
        /// <param name="keyline">
        /// The keyline black component.
        /// </param>
        private CmykColor(float cyan, float magenta, float yellow, float keyline)
        {
            c = Clamp(cyan);
            m = Clamp(magenta);
            y = Clamp(yellow);
            k = Clamp(keyline);
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="CmykColor"/> struct.
        /// </summary>
        /// <param name="color">
        /// The <see cref="System.Drawing.Color"/> to initialize from.
        /// </param>
        private CmykColor(Color color)
        {
            CmykColor cmykColor = color;
            c = cmykColor.c;
            m = cmykColor.m;
            y = cmykColor.y;
            k = cmykColor.k;
        }

        /// <summary>
        /// Gets the cyan component.
        /// <remarks>A value ranging between 0 and 100.</remarks>
        /// </summary>
        public float C => c;

        /// <summary>
        /// Gets the magenta component.
        /// <remarks>A value ranging between 0 and 100.</remarks>
        /// </summary>
        public float M => m;

        /// <summary>
        /// Gets the yellow component.
        /// <remarks>A value ranging between 0 and 100.</remarks>
        /// </summary>
        public float Y => y;

        /// <summary>
        /// Gets the keyline black component.
        /// <remarks>A value ranging between 0 and 100.</remarks>
        /// </summary>
        public float K => k;

        /// <summary>
        /// Creates a <see cref="CmykColor"/> structure from the four 32-bit CMYK 
        /// components (cyan, magenta, yellow, and keyline) values.
        /// </summary>
        /// <param name="cyan">
        /// The cyan component.
        /// </param>
        /// <param name="magenta">
        /// The magenta component.
        /// </param>
        /// <param name="yellow">
        /// The yellow component.
        /// </param>
        /// <param name="keyline">
        /// The keyline black component.
        /// </param>
        /// <returns>
        /// The <see cref="CmykColor"/>.
        /// </returns>
        public static CmykColor FromCmykColor(float cyan, float magenta, float yellow, float keyline)
        {
            return new CmykColor(cyan, magenta, yellow, keyline);
        }

        /// <summary>
        /// Creates a <see cref="CmykColor"/> structure from the specified <see cref="System.Drawing.Color"/> structure
        /// </summary>
        /// <param name="color">
        /// The <see cref="System.Drawing.Color"/> from which to create the new <see cref="CmykColor"/>.
        /// </param>
        /// <returns>
        /// The <see cref="CmykColor"/>.
        /// </returns>
        public static CmykColor FromColor(Color color)
        {
            return new CmykColor(color);
        }

        /// <summary>
        /// Allows the implicit conversion of an instance of <see cref="System.Drawing.Color"/> to a 
        /// <see cref="CmykColor"/>.
        /// </summary>
        /// <param name="color">
        /// The instance of <see cref="System.Drawing.Color"/> to convert.
        /// </param>
        /// <returns>
        /// An instance of <see cref="CmykColor"/>.
        /// </returns>
        public static implicit operator CmykColor(Color color)
        {
            var c = (255f - color.R) / 255;
            var m = (255f - color.G) / 255;
            var y = (255f - color.B) / 255;

            var k = Math.Min(c, Math.Min(m, y));

            if (Math.Abs(k - 1.0) <= .0001f)
            {
                return new CmykColor(0, 0, 0, 100);
            }

            c = ((c - k) / (1 - k)) * 100;
            m = ((m - k) / (1 - k)) * 100;
            y = ((y - k) / (1 - k)) * 100;

            return new CmykColor(c, m, y, k * 100);
        }

        /// <summary>
        /// Allows the implicit conversion of an instance of <see cref="RgbaColor"/> to a 
        /// <see cref="CmykColor"/>.
        /// </summary>
        /// <param name="rgbaColor">
        /// The instance of <see cref="RgbaColor"/> to convert.
        /// </param>
        /// <returns>
        /// An instance of <see cref="CmykColor"/>.
        /// </returns>
        public static implicit operator CmykColor(RgbaColor rgbaColor)
        {
            return FromColor(rgbaColor);
        }

        /// <summary>
        /// Allows the implicit conversion of an instance of <see cref="YCbCrColor"/> to a 
        /// <see cref="HslaColor"/>.
        /// </summary>
        /// <param name="ycbcrColor">
        /// The instance of <see cref="YCbCrColor"/> to convert.
        /// </param>
        /// <returns>
        /// An instance of <see cref="YCbCrColor"/>.
        /// </returns>
        public static implicit operator CmykColor(YCbCrColor ycbcrColor)
        {
            Color color = ycbcrColor;
            return FromColor(color);
        }

        /// <summary>
        /// Allows the implicit conversion of an instance of <see cref="CmykColor"/> to a 
        /// <see cref="System.Drawing.Color"/>.
        /// </summary>
        /// <param name="cmykColor">
        /// The instance of <see cref="CmykColor"/> to convert.
        /// </param>
        /// <returns>
        /// An instance of <see cref="System.Drawing.Color"/>.
        /// </returns>
        public static implicit operator Color(CmykColor cmykColor)
        {
            var red = Convert.ToInt32((1 - (cmykColor.c / 100)) * (1 - (cmykColor.k / 100)) * 255.0);
            var green = Convert.ToInt32((1 - (cmykColor.m / 100)) * (1 - (cmykColor.k / 100)) * 255.0);
            var blue = Convert.ToInt32((1 - (cmykColor.y / 100)) * (1 - (cmykColor.k / 100)) * 255.0);
            return Color.FromArgb(red.ToByte(), green.ToByte(), blue.ToByte());
        }

        /// <summary>
        /// Allows the implicit conversion of an instance of <see cref="CmykColor"/> to a 
        /// <see cref="RgbaColor"/>.
        /// </summary>
        /// <param name="cmykColor">
        /// The instance of <see cref="CmykColor"/> to convert.
        /// </param>
        /// <returns>
        /// An instance of <see cref="RgbaColor"/>.
        /// </returns>
        public static implicit operator RgbaColor(CmykColor cmykColor)
        {
            var red = Convert.ToInt32((1 - (cmykColor.c / 100)) * (1 - (cmykColor.k / 100)) * 255.0);
            var green = Convert.ToInt32((1 - (cmykColor.m / 100)) * (1 - (cmykColor.k / 100)) * 255.0);
            var blue = Convert.ToInt32((1 - (cmykColor.y / 100)) * (1 - (cmykColor.k / 100)) * 255.0);
            return RgbaColor.FromRgba(red.ToByte(), green.ToByte(), blue.ToByte());
        }

        /// <summary>
        /// Allows the implicit conversion of an instance of <see cref="CmykColor"/> to a 
        /// <see cref="YCbCrColor"/>.
        /// </summary>
        /// <param name="cmykColor">
        /// The instance of <see cref="CmykColor"/> to convert.
        /// </param>
        /// <returns>
        /// An instance of <see cref="YCbCrColor"/>.
        /// </returns>
        public static implicit operator YCbCrColor(CmykColor cmykColor)
        {
            return YCbCrColor.FromColor(cmykColor);
        }

        /// <summary>
        /// Allows the implicit conversion of an instance of <see cref="CmykColor"/> to a 
        /// <see cref="HslaColor"/>.
        /// </summary>
        /// <param name="cmykColor">
        /// The instance of <see cref="CmykColor"/> to convert.
        /// </param>
        /// <returns>
        /// An instance of <see cref="HslaColor"/>.
        /// </returns>
        public static implicit operator HslaColor(CmykColor cmykColor)
        {
            return HslaColor.FromColor(cmykColor);
        }

        /// <summary>
        /// Returns a <see cref="System.String" /> that represents this instance.
        /// </summary>
        /// <returns>
        /// A <see cref="System.String" /> that represents this instance.
        /// </returns>
        public override string ToString()
        {
            if (IsEmpty())
            {
                return "CmykColor [ Empty ]";
            }

            return string.Format("CmykColor [ C={0:#0.##}, M={1:#0.##}, Y={2:#0.##}, K={3:#0.##}]", C, M, Y, K);
        }

        /// <summary>
        /// Indicates whether this instance and a specified object are equal.
        /// </summary>
        /// <returns>
        /// true if <paramref name="obj"/> and this instance are the same type and represent the same value; otherwise, false.
        /// </returns>
        /// <param name="obj">Another object to compare to. </param>
        public override bool Equals(object obj)
        {
            if (obj is CmykColor)
            {
                return Equals((CmykColor)obj);
            }

            return false;
        }

        /// <summary>
        /// Indicates whether the current object is equal to another object of the same type.
        /// </summary>
        /// <param name="other">An object to compare with this object.</param>
        /// <returns>true if the current object is equal to the <paramref name="other" /> parameter; otherwise, false.</returns>
        public bool Equals(CmykColor other)
        {
            Color thisColor = this;
            Color otherColor = other;
            return thisColor.Equals(otherColor);
        }

        /// <summary>
        /// Returns the hash code for this instance.
        /// </summary>
        /// <returns>
        /// A 32-bit signed integer that is the hash code for this instance.
        /// </returns>
        public override int GetHashCode()
        {
            Color thisColor = this;
            return thisColor.GetHashCode();
        }

        /// <summary>
        /// Checks the range of the given value to ensure that it remains within the acceptable boundaries.
        /// </summary>
        /// <param name="value">
        /// The value to check.
        /// </param>
        /// <returns>
        /// The sanitized <see cref="float"/>.
        /// </returns>
        private static float Clamp(float value)
        {
            return ImageMaths.Clamp(value, 0, 100);
        }

        /// <summary>
        /// Returns a value indicating whether the current instance is empty.
        /// </summary>
        /// <returns>
        /// The true if this instance is empty; otherwise, false.
        /// </returns>
        private bool IsEmpty()
        {
            const float Epsilon = .0001f;
            return Math.Abs(c - 0) <= Epsilon && Math.Abs(m - 0) <= Epsilon &&
                   Math.Abs(y - 0) <= Epsilon && Math.Abs(k - 0) <= Epsilon;
        }
    }
}
