﻿// --------------------------------------------------------------------------------------------------------------------
// <copyright file="EntropyCrop.cs" company="James Jackson-South">
//   Copyright (c) James Jackson-South.
//   Licensed under the Apache License, Version 2.0.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Imaging;
using System.Linq;
using Lenneth.Core.Framework.ImageProcessor.Common.Exceptions;
using Lenneth.Core.Framework.ImageProcessor.Imaging.Filters.Binarization;
using Lenneth.Core.Framework.ImageProcessor.Imaging.Filters.EdgeDetection;
using Lenneth.Core.Framework.ImageProcessor.Imaging.Helpers;
using Lenneth.Core.Framework.ImageProcessor.Imaging.MetaData;

namespace Lenneth.Core.Framework.ImageProcessor.Processors
{
    /// <summary>
    /// Performs a crop on an image to the area of greatest entropy.
    /// </summary>
    public class EntropyCrop : IGraphicsProcessor
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="EntropyCrop"/> class.
        /// </summary>
        public EntropyCrop()
        {
            Settings = new Dictionary<string, string>();
        }

        /// <summary>
        /// Gets or sets the dynamic parameter.
        /// </summary>
        public dynamic DynamicParameter { get; set; }

        /// <summary>
        /// Gets or sets any additional settings required by the processor.
        /// </summary>
        public Dictionary<string, string> Settings { get; set; }

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
            Bitmap newImage = null;
            Bitmap grey = null;
            var image = factory.Image;
            byte threshold = DynamicParameter;

            try
            {
                // Detect the edges then strip out middle shades.
                grey = new ConvolutionFilter(new SobelEdgeFilter(), true).Process2DFilter(image);
                grey = new BinaryThreshold(threshold).ProcessFilter(grey);

                // Search for the first white pixels
                var rectangle = ImageMaths.GetFilteredBoundingRectangle(grey, 0);
                grey.Dispose();

                newImage = new Bitmap(rectangle.Width, rectangle.Height, PixelFormat.Format32bppPArgb);
                newImage.SetResolution(image.HorizontalResolution, image.VerticalResolution);
                using (var graphics = Graphics.FromImage(newImage))
                {
                    graphics.DrawImage(
                                     image,
                                     new Rectangle(0, 0, rectangle.Width, rectangle.Height),
                                     rectangle.X,
                                     rectangle.Y,
                                     rectangle.Width,
                                     rectangle.Height,
                                     GraphicsUnit.Pixel);
                }

                // Reassign the image.
                image.Dispose();
                image = newImage;

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
                grey?.Dispose();

                newImage?.Dispose();

                throw new ImageProcessingException("Error processing image with " + GetType().Name, ex);
            }

            return image;
        }
    }
}