// --------------------------------------------------------------------------------------------------------------------
// <copyright file="Rotate.cs" company="James Jackson-South">
//   Copyright (c) James Jackson-South.
//   Licensed under the Apache License, Version 2.0.
// </copyright>
// <summary>
//   Encapsulates methods to rotate an image.
// </summary>
// --------------------------------------------------------------------------------------------------------------------

using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Imaging;
using System.Linq;

namespace Lenneth.Core.Framework.ImageProcessor.Processors
{
    using Common.Exceptions;
    using Imaging.Helpers;
    using Imaging.MetaData;

    /// <summary>
    /// Encapsulates methods to rotate an image.
    /// </summary>
    public class Rotate : IGraphicsProcessor
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="Rotate"/> class.
        /// </summary>
        public Rotate()
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
                float angle = DynamicParameter;

                // Center of the image
                float rotateAtX = Math.Abs(image.Width / 2);
                float rotateAtY = Math.Abs(image.Height / 2);

                // Create a rotated image.
                image = RotateImage(image, rotateAtX, rotateAtY, angle);

                if (factory.PreserveExifData && factory.ExifPropertyItems.Any())
                {
                    // Set the width EXIF data.
                    factory.SetPropertyItem(ExifPropertyTag.ImageWidth, (ushort)image.Width);

                    // Set the height EXIF data.
                    factory.SetPropertyItem(ExifPropertyTag.ImageHeight, (ushort)image.Height);
                }

                return image;
            }
            catch (Exception ex)
            {
                throw new ImageProcessingException("Error processing image with " + GetType().Name, ex);
            }
        }

        /// <summary>
        /// Rotates an image to the given angle at the given position.
        /// </summary>
        /// <param name="image">The image to rotate</param>
        /// <param name="rotateAtX">The horizontal pixel coordinate at which to rotate the image.</param>
        /// <param name="rotateAtY">The vertical pixel coordinate at which to rotate the image.</param>
        /// <param name="angle">The angle in degrees at which to rotate the image.</param>
        /// <returns>The image rotated to the given angle at the given position.</returns>
        /// <remarks>
        /// Based on <see href="http://www.codeproject.com/Articles/58815/C-Image-PictureBox-Rotations?msg=4155374#xx4155374xx"/>
        /// </remarks>
        private Bitmap RotateImage(Image image, float rotateAtX, float rotateAtY, float angle)
        {
            var newSize = ImageMaths.GetBoundingRotatedRectangle(image.Width, image.Height, angle);

            var x = (newSize.Width - image.Width) / 2;
            var y = (newSize.Height - image.Height) / 2;

            // Create a new empty bitmap to hold rotated image
            var newImage = new Bitmap(newSize.Width, newSize.Height, PixelFormat.Format32bppPArgb);
            newImage.SetResolution(image.HorizontalResolution, image.VerticalResolution);

            // Make a graphics object from the empty bitmap
            using (var graphics = Graphics.FromImage(newImage))
            {
                // Reduce the jagged edge.
                GraphicsHelper.SetGraphicsOptions(graphics);

                // Put the rotation point in the "center" of the image
                graphics.TranslateTransform(rotateAtX + x, rotateAtY + y);

                // Rotate the image
                graphics.RotateTransform(angle);

                // Move the image back
                graphics.TranslateTransform(-rotateAtX - x, -rotateAtY - y);

                // Draw passed in image onto graphics object
                graphics.DrawImage(image, new PointF(x, y));
            }

            image.Dispose();
            return newImage;
        }
    }
}