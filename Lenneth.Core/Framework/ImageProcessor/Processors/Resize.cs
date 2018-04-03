// --------------------------------------------------------------------------------------------------------------------
// <copyright file="Resize.cs" company="James Jackson-South">
//   Copyright (c) James Jackson-South.
//   Licensed under the Apache License, Version 2.0.
// </copyright>
// <summary>
//   Resizes an image to the given dimensions.
// </summary>
// --------------------------------------------------------------------------------------------------------------------

using System;
using System.Collections.Generic;
using System.Drawing;
using System.Globalization;
using System.Linq;
using Lenneth.Core.Framework.ImageProcessor.Common.Exceptions;
using Lenneth.Core.Framework.ImageProcessor.Imaging;
using Lenneth.Core.Framework.ImageProcessor.Imaging.MetaData;

namespace Lenneth.Core.Framework.ImageProcessor.Processors
{
    /// <summary>
    /// Resizes an image to the given dimensions.
    /// </summary>
    public class Resize : IGraphicsProcessor
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="Resize"/> class.
        /// </summary>
        public Resize()
        {
            this.RestrictedSizes = new List<Size>();
            this.Settings = new Dictionary<string, string>();
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
        /// Gets or sets the list of sizes to restrict resizing methods to.
        /// </summary>
        public List<Size> RestrictedSizes { get; set; }

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
            Image image = factory.Image;

            try
            {
                ResizeLayer resizeLayer = this.DynamicParameter;

                // Augment the layer with the extra information.
                resizeLayer.RestrictedSizes = this.RestrictedSizes;
                Size maxSize = new Size();

                int maxWidth;
                int maxHeight;
                int.TryParse(this.Settings["MaxWidth"], NumberStyles.Any, CultureInfo.InvariantCulture, out maxWidth);
                int.TryParse(this.Settings["MaxHeight"], NumberStyles.Any, CultureInfo.InvariantCulture, out maxHeight);

                maxSize.Width = maxWidth;
                maxSize.Height = maxHeight;

                resizeLayer.MaxSize = maxSize;
                Resizer resizer = new Resizer(resizeLayer) { ImageFormat = factory.CurrentImageFormat, AnimationProcessMode = factory.AnimationProcessMode };
                newImage = resizer.ResizeImage(image, factory.FixGamma);

                // Check that the original image has not been returned.
                if (newImage != image)
                {
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
            }
            catch (Exception ex)
            {
                newImage?.Dispose();

                throw new ImageProcessingException("Error processing image with " + this.GetType().Name, ex);
            }

            return image;
        }
    }
}