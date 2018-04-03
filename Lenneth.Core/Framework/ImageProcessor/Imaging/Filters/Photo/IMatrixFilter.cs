﻿// --------------------------------------------------------------------------------------------------------------------
// <copyright file="IMatrixFilter.cs" company="James Jackson-South">
//   Copyright (c) James Jackson-South.
//   Licensed under the Apache License, Version 2.0.
// </copyright>
// <summary>
//   Defines properties and methods for ColorMatrix based filters.
// </summary>
// --------------------------------------------------------------------------------------------------------------------

using System.Drawing;
using System.Drawing.Imaging;

namespace Lenneth.Core.Framework.ImageProcessor.Imaging.Filters.Photo
{
    /// <summary>
    /// Defines properties and methods for ColorMatrix based filters.
    /// </summary>
    public interface IMatrixFilter
    {
        /// <summary>
        /// Gets the <see cref="T:System.Drawing.Imaging.ColorMatrix"/> for this filter instance.
        /// </summary>
        ColorMatrix Matrix { get; }

        /// <summary>
        /// Processes the image.
        /// </summary>
        /// <param name="source">The current image to process</param>
        /// <param name="destination">The new Image to return</param>
        /// <returns>
        /// The processed <see cref="System.Drawing.Bitmap"/>.
        /// </returns>
        Bitmap TransformImage(Image source, Image destination);
    }
}
