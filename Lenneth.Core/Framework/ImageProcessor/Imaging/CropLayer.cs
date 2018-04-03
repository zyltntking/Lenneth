// --------------------------------------------------------------------------------------------------------------------
// <copyright file="CropLayer.cs" company="James Jackson-South">
//   Copyright (c) James Jackson-South.
//   Licensed under the Apache License, Version 2.0.
// </copyright>
// <summary>
//   Encapsulates the properties required to crop an image.
// </summary>
// --------------------------------------------------------------------------------------------------------------------

using System;

namespace Lenneth.Core.Framework.ImageProcessor.Imaging
{
    /// <summary>
    /// Encapsulates the properties required to crop an image.
    /// </summary>
    public class CropLayer
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="CropLayer"/> class.
        /// </summary>
        /// <param name="left">The left coordinate of the crop layer.</param>
        /// <param name="top">The top coordinate of the crop layer.</param>
        /// <param name="right">The right coordinate of the crop layer.</param>
        /// <param name="bottom">The bottom coordinate of the crop layer.</param>
        /// <param name="cropMode">The <see cref="CropMode"/>.</param>
        /// <remarks>
        /// If the <see cref="CropMode"/> is set to <value>CropMode.Percentage</value> then the four coordinates
        /// become percentages to reduce from each edge.
        /// </remarks>
        public CropLayer(float left, float top, float right, float bottom, CropMode cropMode = CropMode.Percentage)
        {
            if (left < 0 || top < 0 || right < 0 || bottom < 0)
            {
                throw new ArgumentOutOfRangeException();
            }

            Left = left;
            Top = top;
            Right = right;
            Bottom = bottom;
            CropMode = cropMode;
        }

        /// <summary>
        /// Gets or sets the left coordinate of the crop layer.
        /// </summary>
        public float Left { get; set; }

        /// <summary>
        /// Gets or sets the top coordinate of the crop layer.
        /// </summary>
        public float Top { get; set; }

        /// <summary>
        /// Gets or sets the right coordinate of the crop layer.
        /// </summary>
        public float Right { get; set; }

        /// <summary>
        /// Gets or sets the bottom coordinate of the crop layer.
        /// </summary>
        public float Bottom { get; set; }

        /// <summary>
        /// Gets or sets the <see cref="CropMode"/>.
        /// </summary>
        public CropMode CropMode { get; set; }

        /// <summary>
        /// Determines whether the specified <see cref="System.Object" />, is 
        /// equal to this instance.
        /// </summary>
        /// <param name="obj">
        /// The <see cref="System.Object" /> to compare with this instance.
        /// </param>
        /// <returns>
        ///   <c>true</c> if the specified <see cref="System.Object" /> is equal to 
        ///   this instance; otherwise, <c>false</c>.
        /// </returns>
        public override bool Equals(object obj)
        {
            var cropLayer = obj as CropLayer;

            if (cropLayer == null)
            {
                return false;
            }

            // Define the tolerance for variation in their values 
            return Math.Abs(Top - cropLayer.Top) <= Math.Abs(Top * .0001)
                   && Math.Abs(Right - cropLayer.Right) <= Math.Abs(Right * .0001)
                   && Math.Abs(Bottom - cropLayer.Bottom) <= Math.Abs(Bottom * .0001)
                   && Math.Abs(Left - cropLayer.Left) <= Math.Abs(Left * .0001)
                   && CropMode.Equals(cropLayer.CropMode);
        }

        /// <summary>
        /// Serves as a hash function for a particular type. 
        /// </summary>
        /// <returns>
        /// A hash code for the current <see cref="T:System.Object"/>.
        /// </returns>
        public override int GetHashCode()
        {
            unchecked
            {
                var hashCode = Left.GetHashCode();
                hashCode = (hashCode * 397) ^ Top.GetHashCode();
                hashCode = (hashCode * 397) ^ Right.GetHashCode();
                hashCode = (hashCode * 397) ^ Bottom.GetHashCode();
                hashCode = (hashCode * 397) ^ (int)CropMode;
                return hashCode;
            }
        }
    }
}
