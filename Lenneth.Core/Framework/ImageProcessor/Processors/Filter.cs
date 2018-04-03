// --------------------------------------------------------------------------------------------------------------------
// <copyright file="Filter.cs" company="James Jackson-South">
//   Copyright (c) James Jackson-South.
//   Licensed under the Apache License, Version 2.0.
// </copyright>
// <summary>
//   Encapsulates methods with which to add filters to an image.
// </summary>
// --------------------------------------------------------------------------------------------------------------------

using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Imaging;
using Lenneth.Core.Framework.ImageProcessor.Common.Exceptions;
using Lenneth.Core.Framework.ImageProcessor.Imaging.Filters.Photo;

namespace Lenneth.Core.Framework.ImageProcessor.Processors
{
    /// <summary>
    /// Encapsulates methods with which to add filters to an image.
    /// </summary>
    public class Filter : IGraphicsProcessor
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="Filter"/> class.
        /// </summary>
        public Filter()
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
            Bitmap newImage = null;
            var image = factory.Image;

            try
            {
                // TODO: Optimize this one day when I can break the API.
                newImage = new Bitmap(image.Width, image.Height, PixelFormat.Format32bppPArgb);
                newImage.SetResolution(image.HorizontalResolution, image.VerticalResolution);
                IMatrixFilter matrix = DynamicParameter;
                newImage = matrix.TransformImage(image, newImage);
                
                image.Dispose();
                image = newImage;
            }
            catch (Exception ex)
            {
                newImage?.Dispose();

                throw new ImageProcessingException("Error processing image with " + GetType().Name, ex);
            }

            return image;
        }
    }
}
