﻿// --------------------------------------------------------------------------------------------------------------------
// <copyright file="ResizeLayer.cs" company="James Jackson-South">
//   Copyright (c) James Jackson-South.
//   Licensed under the Apache License, Version 2.0.
// </copyright>
// <summary>
//   Encapsulates the properties required to resize an image.
// </summary>
// --------------------------------------------------------------------------------------------------------------------

using System.Collections.Generic;
using System.Drawing;
using System.Linq;

namespace Lenneth.Core.Framework.ImageProcessor.Imaging
{
    #region Using

    #endregion

    /// <summary>
    /// Encapsulates the properties required to resize an image.
    /// </summary>
    public class ResizeLayer
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="ResizeLayer"/> class.
        /// </summary>
        /// <param name="size">
        /// The <see cref="T:System.Drawing.Size"/> containing the width and height to set the image to.
        /// </param>
        /// <param name="resizeMode">
        /// The resize mode to apply to resized image. (Default ResizeMode.Pad)
        /// </param>
        /// <param name="anchorPosition">
        /// The <see cref="AnchorPosition"/> to apply to resized image. (Default AnchorPosition.Center)
        /// </param>
        /// <param name="upscale">
        /// Whether to allow up-scaling of images. (Default true)
        /// </param>
        /// <param name="centerCoordinates">
        /// The center coordinates (Default null)
        /// </param>
        /// <param name="maxSize">
        /// The maximum size to resize an image to. 
        /// Used to restrict resizing based on calculated resizing
        /// </param>
        /// <param name="restrictedSizes">
        /// The range of sizes to restrict resizing an image to. 
        /// Used to restrict resizing based on calculated resizing
        /// </param>
        /// <param name="anchorPoint">
        /// The anchor point (Default null)
        /// </param>
        public ResizeLayer(
            Size size,
            ResizeMode resizeMode = ResizeMode.Pad,
            AnchorPosition anchorPosition = AnchorPosition.Center,
            bool upscale = true,
            float[] centerCoordinates = null,
            Size? maxSize = null,
            List<Size> restrictedSizes = null,
            Point? anchorPoint = null)
        {
            Size = size;
            Upscale = upscale;
            ResizeMode = resizeMode;
            AnchorPosition = anchorPosition;
            CenterCoordinates = centerCoordinates ?? new float[] { };
            MaxSize = maxSize;
            RestrictedSizes = restrictedSizes ?? new List<Size>();
            AnchorPoint = anchorPoint;
        }

        /// <summary>
        /// Gets or sets the size.
        /// </summary>
        public Size Size { get; set; }

        /// <summary>
        /// Gets or sets the max size.
        /// </summary>
        public Size? MaxSize { get; set; }

        /// <summary>
        /// Gets or sets the restricted range of sizes. to restrict resizing methods to.
        /// </summary>
        public List<Size> RestrictedSizes { get; set; }

        /// <summary>
        /// Gets or sets the resize mode.
        /// </summary>
        public ResizeMode ResizeMode { get; set; }

        /// <summary>
        /// Gets or sets the anchor position.
        /// </summary>
        public AnchorPosition AnchorPosition { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether to allow up-scaling of images.
        /// For <see cref="T:ResizeMode.BoxPad"/> this is always true.
        /// </summary>
        public bool Upscale { get; set; }

        /// <summary>
        /// Gets or sets the center coordinates.
        /// </summary>
        public float[] CenterCoordinates { get; set; }

        /// <summary>
        /// Gets or sets the anchor point.
        /// </summary>
        public Point? AnchorPoint { get; set; }

        /// <summary>
        /// Returns a value that indicates whether the specified object is an 
        /// <see cref="ResizeLayer"/> object that is equivalent to 
        /// this <see cref="ResizeLayer"/> object.
        /// </summary>
        /// <param name="obj">
        /// The object to test.
        /// </param>
        /// <returns>
        /// True if the given object  is an <see cref="ResizeLayer"/> object that is equivalent to 
        /// this <see cref="ResizeLayer"/> object; otherwise, false.
        /// </returns>
        public override bool Equals(object obj)
        {
            var resizeLayer = obj as ResizeLayer;

            if (resizeLayer == null)
            {
                return false;
            }

            return Size == resizeLayer.Size
                && ResizeMode == resizeLayer.ResizeMode
                && AnchorPosition == resizeLayer.AnchorPosition
                && Upscale == resizeLayer.Upscale
                && ((CenterCoordinates != null
                    && resizeLayer.CenterCoordinates != null
                    && CenterCoordinates.SequenceEqual(resizeLayer.CenterCoordinates))
                    || (CenterCoordinates == resizeLayer.CenterCoordinates))
                && MaxSize == resizeLayer.MaxSize
                && ((RestrictedSizes != null
                    && resizeLayer.RestrictedSizes != null
                    && RestrictedSizes.SequenceEqual(resizeLayer.RestrictedSizes))
                    || (RestrictedSizes == resizeLayer.RestrictedSizes))
                && AnchorPoint == resizeLayer.AnchorPoint;
        }

        /// <summary>
        /// Returns the hash code for this instance.
        /// </summary>
        /// <returns>
        /// A 32-bit signed integer that is the hash code for this instance.
        /// </returns>
        public override int GetHashCode()
        {
            unchecked
            {
                var hashCode = Size.GetHashCode();
                hashCode = (hashCode * 397) ^ MaxSize.GetHashCode();
                hashCode = (hashCode * 397) ^ (RestrictedSizes?.GetHashCode() ?? 0);
                hashCode = (hashCode * 397) ^ (int)ResizeMode;
                hashCode = (hashCode * 397) ^ (int)AnchorPosition;
                hashCode = (hashCode * 397) ^ Upscale.GetHashCode();
                hashCode = (hashCode * 397) ^ (CenterCoordinates?.GetHashCode() ?? 0);
                hashCode = (hashCode * 397) ^ AnchorPoint.GetHashCode();
                return hashCode;
            }
        }
    }
}