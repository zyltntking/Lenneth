﻿// --------------------------------------------------------------------------------------------------------------------
// <copyright file="GraphicsHelper.cs" company="James Jackson-South">
//   Copyright (c) James Jackson-South.
//   Licensed under the Apache License, Version 2.0.
// </copyright>
// <summary>
//   Defines the GraphicsHelper type.
// </summary>
// --------------------------------------------------------------------------------------------------------------------

using System.Drawing;
using System.Drawing.Drawing2D;

namespace Lenneth.Core.Framework.ImageProcessor.Imaging.Helpers
{
    /// <summary>
    /// The graphics helper.
    /// </summary>
    internal static class GraphicsHelper
    {
        /// <summary>
        /// Set the default graphics options for drawing an image.
        /// </summary>
        /// <param name="graphics">The graphics.</param>
        /// <param name="blending">Whether the graphics object will be blending pixels.</param>
        /// <param name="smoothing">Whether the graphics object will be smoothing pixels.</param>
        public static void SetGraphicsOptions(Graphics graphics, bool blending = false, bool smoothing = false)
        {
            // Highest quality resampling algorithm.
            graphics.InterpolationMode = InterpolationMode.HighQualityBicubic;

            // Ensure pixel offset is set.
            graphics.PixelOffsetMode = PixelOffsetMode.Half;

            if (smoothing)
            {
                // We want smooth edges when drawing.
                graphics.SmoothingMode = SmoothingMode.AntiAlias;
            }
            if (blending || smoothing)
            {
                // Best combination for blending pixels.
                graphics.CompositingMode = CompositingMode.SourceOver;
                graphics.CompositingQuality = CompositingQuality.GammaCorrected;
            }
            else
            {
                // We're not blending pixels here so use the faster option.
                graphics.CompositingMode = CompositingMode.SourceCopy;
            }
        }
    }
}
