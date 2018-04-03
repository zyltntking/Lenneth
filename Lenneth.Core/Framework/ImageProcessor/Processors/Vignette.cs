// --------------------------------------------------------------------------------------------------------------------
// <copyright file="Vignette.cs" company="James Jackson-South">
//   Copyright (c) James Jackson-South.
//   Licensed under the Apache License, Version 2.0.
// </copyright>
// <summary>
//   Encapsulates methods with which to add a vignette image effect to an image.
// </summary>
// --------------------------------------------------------------------------------------------------------------------

using System;
using System.Collections.Generic;
using System.Drawing;

namespace Lenneth.Core.Framework.ImageProcessor.Processors
{
    using Common.Exceptions;
    using Imaging.Helpers;

    /// <summary>
    /// Encapsulates methods with which to add a vignette image effect to an image.
    /// </summary>
    public class Vignette : IGraphicsProcessor
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="Vignette"/> class.
        /// </summary>
        public Vignette()
        {
            DynamicParameter = Color.Black;
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
                var baseColor = (Color)DynamicParameter;
                return Effects.Vignette(image, baseColor);
            }
            catch (Exception ex)
            {
                throw new ImageProcessingException("Error processing image with " + GetType().Name, ex);
            }
        }
    }
}