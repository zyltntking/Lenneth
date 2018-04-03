// --------------------------------------------------------------------------------------------------------------------
// <copyright file="AutoRotate.cs" company="James Jackson-South">
//   Copyright (c) James Jackson-South.
//   Licensed under the Apache License, Version 2.0.
// </copyright>
// <summary>
//   Performs auto-rotation to ensure that EXIF defined rotation is reflected in
//   the final image.
//   <remarks>
//   <see href="http://sylvana.net/jpegcrop/exif_orientation.html" />
//   </remarks>
// </summary>
// --------------------------------------------------------------------------------------------------------------------

using System;
using System.Collections.Generic;
using System.Drawing;
using Lenneth.Core.Framework.ImageProcessor.Common.Exceptions;
using Lenneth.Core.Framework.ImageProcessor.Imaging.MetaData;

namespace Lenneth.Core.Framework.ImageProcessor.Processors
{
    /// <summary>
    /// Performs auto-rotation to ensure that EXIF defined rotation is reflected in 
    /// the final image.
    /// <remarks>
    /// <see href="http://sylvana.net/jpegcrop/exif_orientation.html"/>
    /// </remarks>
    /// </summary>
    public class AutoRotate : IGraphicsProcessor
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="AutoRotate"/> class.
        /// </summary>
        public AutoRotate()
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
        /// <param name="factory">The current instance of the 
        /// <see cref="T:ImageProcessor.ImageFactory" /> class containing
        /// the image to process.</param>
        /// <returns>
        /// The processed image from the current instance of the <see cref="T:ImageProcessor.ImageFactory" /> class.
        /// </returns>
        public Image ProcessImage(ImageFactory factory)
        {
            var image = factory.Image;

            try
            {
                const int Orientation = (int)ExifPropertyTag.Orientation;
                if (!factory.PreserveExifData && factory.ExifPropertyItems.ContainsKey(Orientation))
                {
                    int rotationValue = factory.ExifPropertyItems[Orientation].Value[0];
                    switch (rotationValue)
                    {
                        case 8:
                            // Rotated 90 right
                            image.RotateFlip(RotateFlipType.Rotate270FlipNone);
                            break;

                        case 7: // Rotated 90 right, flip horizontally
                            image.RotateFlip(RotateFlipType.Rotate270FlipX);
                            break;

                        case 6: // Rotated 90 left
                            image.RotateFlip(RotateFlipType.Rotate90FlipNone);
                            break;

                        case 5: // Rotated 90 left, flip horizontally
                            image.RotateFlip(RotateFlipType.Rotate90FlipX);
                            break;

                        case 3: // Rotate 180 left
                            image.RotateFlip(RotateFlipType.Rotate180FlipNone);
                            break;

                        case 2: // Flip horizontally
                            image.RotateFlip(RotateFlipType.RotateNoneFlipX);
                            break;
                    }
                }

                return image;
            }
            catch (Exception ex)
            {
                throw new ImageProcessingException("Error processing image with " + GetType().Name, ex);
            }
        }
    }
}