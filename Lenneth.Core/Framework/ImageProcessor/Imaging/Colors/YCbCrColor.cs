﻿// --------------------------------------------------------------------------------------------------------------------
// <copyright file="YCbCrColor.cs" company="James Jackson-South">
//   Copyright (c) James Jackson-South.
//   Licensed under the Apache License, Version 2.0.
// </copyright>
// <summary>
//   Represents an YCbCr (luminance, chroma, chroma) color conforming to the ITU-R BT.601 standard used in digital imaging systems.
//   <see href="http://en.wikipedia.org/wiki/YCbCr" />
// </summary>
// --------------------------------------------------------------------------------------------------------------------

using System;
using System.Drawing;
using Lenneth.Core.Framework.ImageProcessor.Imaging.Helpers;

namespace Lenneth.Core.Framework.ImageProcessor.Imaging.Colors
{
    /// <summary>
    /// Represents an YCbCr (luminance, chroma, chroma) color conforming to the ITU-R BT.601 standard used in digital imaging systems.
    /// <see href="http://en.wikipedia.org/wiki/YCbCr"/>
    /// </summary>
    public struct YCbCrColor : IEquatable<YCbCrColor>
    {
        /// <summary>
        /// Represents a <see cref="YCbCrColor"/> that is null.
        /// </summary>
        public static readonly YCbCrColor Empty = new YCbCrColor();

        /// <summary>
        /// The y luminance component.
        /// </summary>
        private readonly float _y;

        /// <summary>
        /// The u chroma component.
        /// </summary>
        private readonly float _cb;

        /// <summary>
        /// The v chroma component.
        /// </summary>
        private readonly float _cr;

        /// <summary>
        /// Initializes a new instance of the <see cref="YCbCrColor"/> struct.
        /// </summary>
        /// <param name="y">The y luminance component.</param>
        /// <param name="cb">The u chroma component.</param>
        /// <param name="cr">The v chroma component.</param> 
        private YCbCrColor(float y, float cb, float cr)
        {
            _y = ImageMaths.Clamp(y, 0, 255);
            _cb = ImageMaths.Clamp(cb, 0, 255);
            _cr = ImageMaths.Clamp(cr, 0, 255);
        }

        /// <summary>
        /// Gets the Y luminance component.
        /// <remarks>A value ranging between 0 and 255.</remarks>
        /// </summary>
        public float Y => _y;

        /// <summary>
        /// Gets the U chroma component.
        /// <remarks>A value ranging between 0 and 255.</remarks>
        /// </summary>
        public float Cb => _cb;

        /// <summary>
        /// Gets the V chroma component.
        /// <remarks>A value ranging between 0 and 255.</remarks>
        /// </summary>
        public float Cr => _cr;

        /// <summary>
        /// Creates a <see cref="YCbCrColor"/> structure from the three 32-bit YCbCr 
        /// components (luminance, chroma, and chroma) values.
        /// </summary>
        /// <param name="y">The y luminance component.</param>
        /// <param name="cb">The u chroma component.</param>
        /// <param name="cr">The v chroma component.</param> 
        /// <returns>
        /// The <see cref="YCbCrColor"/>.
        /// </returns>
        public static YCbCrColor FromYCbCr(float y, float cb, float cr) => new YCbCrColor(y, cb, cr);

        /// <summary>
        /// Creates a <see cref="YCbCrColor"/> structure from the specified <see cref="System.Drawing.Color"/> structure
        /// </summary>
        /// <param name="color">
        /// The <see cref="System.Drawing.Color"/> from which to create the new <see cref="YCbCrColor"/>.
        /// </param>
        /// <returns>
        /// The <see cref="YCbCrColor"/>.
        /// </returns>
        public static YCbCrColor FromColor(Color color)
        {
            var r = color.R;
            var g = color.G;
            var b = color.B;

            var y = (float)((0.299 * r) + (0.587 * g) + (0.114 * b));
            var cb = 128 + (float)((-0.168736 * r) - (0.331264 * g) + (0.5 * b));
            var cr = 128 + (float)((0.5 * r) - (0.418688 * g) - (0.081312 * b));

            return new YCbCrColor(y, cb, cr);
        }

        /// <summary>
        /// Allows the implicit conversion of an instance of <see cref="System.Drawing.Color"/> to a 
        /// <see cref="YCbCrColor"/>.
        /// </summary>
        /// <param name="color">
        /// The instance of <see cref="System.Drawing.Color"/> to convert.
        /// </param>
        /// <returns>
        /// An instance of <see cref="YCbCrColor"/>.
        /// </returns>
        public static implicit operator YCbCrColor(Color color) => FromColor(color);

        /// <summary>
        /// Allows the implicit conversion of an instance of <see cref="RgbaColor"/> to a 
        /// <see cref="YCbCrColor"/>.
        /// </summary>
        /// <param name="rgbaColor">
        /// The instance of <see cref="RgbaColor"/> to convert.
        /// </param>
        /// <returns>
        /// An instance of <see cref="YCbCrColor"/>.
        /// </returns>
        public static implicit operator YCbCrColor(RgbaColor rgbaColor) => FromColor(rgbaColor);

        /// <summary>
        /// Allows the implicit conversion of an instance of <see cref="HslaColor"/> to a 
        /// <see cref="YCbCrColor"/>.
        /// </summary>
        /// <param name="hslaColor">
        /// The instance of <see cref="HslaColor"/> to convert.
        /// </param>
        /// <returns>
        /// An instance of <see cref="YCbCrColor"/>.
        /// </returns>
        public static implicit operator YCbCrColor(HslaColor hslaColor) => FromColor(hslaColor);

        /// <summary>
        /// Allows the implicit conversion of an instance of <see cref="YCbCrColor"/> to a 
        /// <see cref="System.Drawing.Color"/>.
        /// </summary>
        /// <param name="ycbcrColor">
        /// The instance of <see cref="YCbCrColor"/> to convert.
        /// </param>
        /// <returns>
        /// An instance of <see cref="System.Drawing.Color"/>.
        /// </returns>
        public static implicit operator Color(YCbCrColor ycbcrColor)
        {
            var y = ycbcrColor.Y;
            var cb = ycbcrColor.Cb - 128;
            var cr = ycbcrColor.Cr - 128;

            var r = Convert.ToByte(ImageMaths.Clamp(y + (1.402 * cr), 0, 255));
            var g = Convert.ToByte(ImageMaths.Clamp(y - (0.34414 * cb) - (0.71414 * cr), 0, 255));
            var b = Convert.ToByte(ImageMaths.Clamp(y + (1.772 * cb), 0, 255));

            return Color.FromArgb(255, r, g, b);
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
                return "YCbCrColor [ Empty ]";
            }

            return $"YCbCrColor [ Y={Y:#0.##}, Cb={Cb:#0.##}, Cr={Cr:#0.##}]";
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
            if (obj is YCbCrColor)
            {
                return Equals((YCbCrColor)obj);
            }

            return false;
        }

        #region override operator

        public static bool operator ==(YCbCrColor lhs, YCbCrColor rhs)
        {
            return lhs.Equals(rhs);
        }

        public static bool operator !=(YCbCrColor lhs, YCbCrColor rhs)
        {
            return !lhs.Equals(rhs);
        }

        #endregion

        /// <summary>
        /// Indicates whether the current object is equal to another object of the same type.
        /// </summary>
        /// <param name="other">An object to compare with this object.</param>
        /// <returns>true if the current object is equal to the <paramref name="other" /> parameter; otherwise, false.</returns>
        public bool Equals(YCbCrColor other)
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
        /// Returns a value indicating whether the current instance is empty.
        /// </summary>
        /// <returns>
        /// The true if this instance is empty; otherwise, false.
        /// </returns>
        private bool IsEmpty()
        {
            const float epsilon = .0001f;
            return Math.Abs(_y - 0) <= epsilon && Math.Abs(_cb - 0) <= epsilon &&
                   Math.Abs(_cr - 0) <= epsilon;
        }
    }
}
