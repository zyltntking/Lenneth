﻿// --------------------------------------------------------------------------------------------------------------------
// <copyright file="GaussianSharpen.cs" company="James Jackson-South">
//   Copyright (c) James Jackson-South.
//   Licensed under the Apache License, Version 2.0.
// </copyright>
// <summary>
//   Applies a Gaussian sharpen to the image.
// </summary>
// --------------------------------------------------------------------------------------------------------------------

using System;
using System.Collections.Generic;
using System.Drawing;
using Lenneth.Core.Framework.ImageProcessor.Common.Exceptions;
using Lenneth.Core.Framework.ImageProcessor.Imaging;

namespace Lenneth.Core.Framework.ImageProcessor.Processors
{
    /// <summary>
    /// Applies a Gaussian sharpen to the image.
    /// </summary>
    public class GaussianSharpen : IGraphicsProcessor
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="GaussianSharpen"/> class.
        /// </summary>
        public GaussianSharpen()
        {
            Settings = new Dictionary<string, string>();
        }

        /// <summary>
        /// Gets or sets the DynamicParameter.
        /// </summary>
        public dynamic DynamicParameter { get; set; }

        /// <summary>
        /// Gets or sets any additional settings required by the processor.
        /// </summary>
        public Dictionary<string, string> Settings { get; set; }

        /// <summary>
        /// Processes the image.
        /// </summary>
        /// <param name="factory">The current instance of the <see cref="T:ImageProcessor.ImageFactory" /> class containing
        /// the image to process.</param>
        /// <returns>
        /// The processed image from the current instance of the <see cref="T:ImageProcessor.ImageFactory" /> class.
        /// </returns>
        public Image ProcessImage(ImageFactory factory)
        {
            var image = (Bitmap)factory.Image;

            try
            {
                GaussianLayer gaussianLayer = DynamicParameter;
                var convolution = new Convolution(gaussianLayer.Sigma) { Threshold = gaussianLayer.Threshold };
                var kernel = convolution.CreateGuassianSharpenFilter(gaussianLayer.Size);

                return convolution.ProcessKernel(image, kernel, factory.FixGamma);
            }
            catch (Exception ex)
            {
                throw new ImageProcessingException("Error processing image with " + GetType().Name, ex);
            }
        }
    }
}
