// --------------------------------------------------------------------------------------------------------------------
// <copyright file="Resolution.cs" company="James Jackson-South">
//   Copyright (c) James Jackson-South.
//   Licensed under the Apache License, Version 2.0.
// </copyright>
// <summary>
//   Encapsulates methods to change the resolution of the image.
// </summary>
// --------------------------------------------------------------------------------------------------------------------

using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;

namespace Lenneth.Core.Framework.ImageProcessor.Processors
{
    using Common.Exceptions;
    using Imaging.MetaData;

    /// <summary>
    /// Encapsulates methods to change the resolution of the image.
    /// </summary>
    public class Resolution : IGraphicsProcessor
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="Resolution"/> class.
        /// </summary>
        public Resolution()
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
            const float inchInCm = 0.3937007874015748f;
            var image = factory.Image;

            try
            {
                Tuple<int, int, PropertyTagResolutionUnit> resolution = DynamicParameter;

                // Set the bitmap resolution data.
                // Ensure that the resolution is recalculated for bitmap since it only
                // supports inches.
                if (resolution.Item3 == PropertyTagResolutionUnit.Cm)
                {
                    var horizontal = resolution.Item1 / inchInCm;
                    var vertical = resolution.Item2 / inchInCm;
                    ((Bitmap)image).SetResolution(horizontal, vertical);
                }
                else
                {
                    ((Bitmap)image).SetResolution(resolution.Item1, resolution.Item2);
                }

                if (factory.PreserveExifData && factory.ExifPropertyItems.Any())
                {
                    // Set the horizontal EXIF data.
                    var horizontalRational = new Rational<uint>((uint)resolution.Item1, 1);
                    factory.SetPropertyItem(ExifPropertyTag.XResolution, horizontalRational);

                    // Set the vertical EXIF data.
                    var verticalRational = new Rational<uint>((uint)resolution.Item2, 1);
                    factory.SetPropertyItem(ExifPropertyTag.YResolution, verticalRational);

                    // Set the unit EXIF data
                    var units = (ushort)resolution.Item3;
                    factory.SetPropertyItem(ExifPropertyTag.ResolutionUnit, units);
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