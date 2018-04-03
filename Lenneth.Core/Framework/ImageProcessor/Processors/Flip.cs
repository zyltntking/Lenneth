﻿// --------------------------------------------------------------------------------------------------------------------
// <copyright file="Flip.cs" company="James Jackson-South">
//   Copyright (c) James Jackson-South.
//   Licensed under the Apache License, Version 2.0.
// </copyright>
// <summary>
//   Flips an image horizontally or vertically.
// </summary>
// --------------------------------------------------------------------------------------------------------------------

using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using Lenneth.Core.Framework.ImageProcessor.Common.Exceptions;
using Lenneth.Core.Framework.ImageProcessor.Imaging.MetaData;

namespace Lenneth.Core.Framework.ImageProcessor.Processors
{
    /// <summary>
    /// Flips an image horizontally or vertically.
    /// </summary>
    public class Flip : IGraphicsProcessor
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="Flip"/> class.
        /// </summary>
        public Flip()
        {
            Settings = new Dictionary<string, string>();
        }

        /// <summary>
        /// Gets or sets DynamicParameter.
        /// </summary>
        public dynamic DynamicParameter
        {
            get;
            set;
        }

        /// <summary>
        /// Gets or sets any additional settings required by the processor.
        /// </summary>
        public Dictionary<string, string> Settings
        {
            get;
            set;
        }

        /// <summary>
        /// Processes the image.
        /// </summary>
        /// <param name="factory">
        /// The current instance of the <see cref="T:ImageProcessor.ImageFactory"/> class containing
        /// the image to process.
        /// </param>
        /// <returns>
        /// The processed image from the current instance of the <see cref="T:ImageProcessor.ImageFactory"/> class.
        /// </returns>
        public Image ProcessImage(ImageFactory factory)
        {
            var image = factory.Image;

            try
            {
                RotateFlipType rotateFlipType = DynamicParameter;

                // Flip
                image.RotateFlip(rotateFlipType);

                if (factory.PreserveExifData && factory.ExifPropertyItems.Any())
                {
                    // Set the width EXIF data.
                    factory.SetPropertyItem(ExifPropertyTag.ImageWidth, (ushort)image.Width);

                    // Set the height EXIF data.
                    factory.SetPropertyItem(ExifPropertyTag.ImageHeight, (ushort)image.Height);
                }
            }
            catch (Exception ex)
            {
                throw new ImageProcessingException("Error processing image with " + GetType().Name, ex);
            }

            return image;
        }
    }
}
