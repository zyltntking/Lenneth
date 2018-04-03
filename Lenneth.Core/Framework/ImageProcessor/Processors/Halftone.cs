// --------------------------------------------------------------------------------------------------------------------
// <copyright file="Halftone.cs" company="James Jackson-South">
//   Copyright (c) James Jackson-South.
//   Licensed under the Apache License, Version 2.0.
// </copyright>
// <summary>
//   The halftone processor applies a classical CMYK halftone to the given image.
// </summary>
// --------------------------------------------------------------------------------------------------------------------

using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Imaging;
using Lenneth.Core.Framework.ImageProcessor.Common.Exceptions;
using Lenneth.Core.Framework.ImageProcessor.Imaging.Filters.Artistic;
using Lenneth.Core.Framework.ImageProcessor.Imaging.Helpers;

namespace Lenneth.Core.Framework.ImageProcessor.Processors
{
    /// <summary>
    /// The halftone processor applies a classical CMYK halftone to the given image.
    /// </summary>
    public class Halftone : IGraphicsProcessor
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="Halftone"/> class.
        /// </summary>
        public Halftone()
        {
            Settings = new Dictionary<string, string>();
        }

        /// <summary>
        /// Gets or sets the dynamic parameter.
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
            var width = image.Width;
            var height = image.Height;
            Bitmap newImage = null;
            Bitmap edgeBitmap = null;
            try
            {
                var filter = new HalftoneFilter(5);
                newImage = new Bitmap(image);
                newImage.SetResolution(image.HorizontalResolution, image.VerticalResolution);
                newImage = filter.ApplyFilter(newImage);
                bool comicMode = DynamicParameter;

                if (comicMode)
                {
                    // Draw the edges.
                    edgeBitmap = new Bitmap(width, height, PixelFormat.Format32bppPArgb);
                    edgeBitmap.SetResolution(image.HorizontalResolution, image.VerticalResolution);
                    edgeBitmap = Effects.Trace(image, edgeBitmap, 120);

                    using (var graphics = Graphics.FromImage(newImage))
                    {
                        // Overlay the image.
                        graphics.DrawImage(edgeBitmap, 0, 0);
                        var rectangle = new Rectangle(0, 0, width, height);

                        // Draw an edge around the image.
                        using (var blackPen = new Pen(Color.Black))
                        {
                            blackPen.Width = 4;
                            graphics.DrawRectangle(blackPen, rectangle);
                        }
                    }

                    edgeBitmap.Dispose();
                }

                image.Dispose();
                image = newImage;
            }
            catch (Exception ex)
            {
                edgeBitmap?.Dispose();

                newImage?.Dispose();

                throw new ImageProcessingException("Error processing image with " + GetType().Name, ex);
            }

            return image;
        }
    }
}
