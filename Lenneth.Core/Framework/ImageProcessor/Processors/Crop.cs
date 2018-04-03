// --------------------------------------------------------------------------------------------------------------------
// <copyright file="Crop.cs" company="James Jackson-South">
//   Copyright (c) James Jackson-South.
//   Licensed under the Apache License, Version 2.0.
// </copyright>
// <summary>
//   Crops an image to the given directions.
// </summary>
// --------------------------------------------------------------------------------------------------------------------

using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Drawing.Imaging;
using System.Linq;
using Lenneth.Core.Framework.ImageProcessor.Common.Exceptions;
using Lenneth.Core.Framework.ImageProcessor.Imaging;
using Lenneth.Core.Framework.ImageProcessor.Imaging.Helpers;
using Lenneth.Core.Framework.ImageProcessor.Imaging.MetaData;

namespace Lenneth.Core.Framework.ImageProcessor.Processors
{
    /// <summary>
    /// Crops an image to the given directions.
    /// </summary>
    public class Crop : IGraphicsProcessor
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="Crop"/> class.
        /// </summary>
        public Crop()
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
                var sourceWidth = image.Width;
                var sourceHeight = image.Height;
                RectangleF rectangleF;
                CropLayer cropLayer = DynamicParameter;

                if (cropLayer.CropMode == CropMode.Percentage)
                {
                    // Fix for whole numbers. 
                    var percentageLeft = cropLayer.Left > 1 ? cropLayer.Left / 100 : cropLayer.Left;
                    var percentageRight = cropLayer.Right > 1 ? cropLayer.Right / 100 : cropLayer.Right;
                    var percentageTop = cropLayer.Top > 1 ? cropLayer.Top / 100 : cropLayer.Top;
                    var percentageBottom = cropLayer.Bottom > 1 ? cropLayer.Bottom / 100 : cropLayer.Bottom;

                    // Work out the percentages.
                    var left = percentageLeft * sourceWidth;
                    var top = percentageTop * sourceHeight;
                    var width = percentageRight < 1 ? (1 - percentageLeft - percentageRight) * sourceWidth : sourceWidth;
                    var height = percentageBottom < 1 ? (1 - percentageTop - percentageBottom) * sourceHeight : sourceHeight;

                    rectangleF = new RectangleF(left, top, width, height);
                }
                else
                {
                    rectangleF = new RectangleF(cropLayer.Left, cropLayer.Top, cropLayer.Right, cropLayer.Bottom);
                }

                var rectangle = Rectangle.Round(rectangleF);

                if (rectangle.X < sourceWidth && rectangle.Y < sourceHeight)
                {
                    if (rectangle.Width > (sourceWidth - rectangle.X))
                    {
                        rectangle.Width = sourceWidth - rectangle.X;
                    }

                    if (rectangle.Height > (sourceHeight - rectangle.Y))
                    {
                        rectangle.Height = sourceHeight - rectangle.Y;
                    }

                    newImage = new Bitmap(rectangle.Width, rectangle.Height, PixelFormat.Format32bppPArgb);
                    newImage.SetResolution(image.HorizontalResolution, image.VerticalResolution);

                    var rotationValue = 0;
                    const int orientation = (int)ExifPropertyTag.Orientation;
                    var rotate = factory.PreserveExifData && factory.ExifPropertyItems.ContainsKey(orientation);
                    if (rotate)
                    {
                        rotationValue = factory.ExifPropertyItems[orientation].Value[0];
                        ForwardRotateFlip(rotationValue, ref image);
                    }

                    using (var graphics = Graphics.FromImage(newImage))
                    {
                        GraphicsHelper.SetGraphicsOptions(graphics);

                        // An unwanted border appears when using InterpolationMode.HighQualityBicubic to resize the image
                        // as the algorithm appears to be pulling averaging detail from surrounding pixels beyond the edge 
                        // of the image. Using the ImageAttributes class to specify that the pixels beyond are simply mirror 
                        // images of the pixels within solves this problem.
                        using (var wrapMode = new ImageAttributes())
                        {
                            wrapMode.SetWrapMode(WrapMode.TileFlipXY);

                            graphics.DrawImage(
                                image,
                                new Rectangle(0, 0, rectangle.Width, rectangle.Height),
                                rectangle.X,
                                rectangle.Y,
                                rectangle.Width,
                                rectangle.Height,
                                GraphicsUnit.Pixel,
                                wrapMode);
                        }
                    }

                    // Reassign the image.
                    image.Dispose();
                    image = newImage;

                    if (rotate)
                    {
                        ReverseRotateFlip(rotationValue, ref image);
                    }

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

                throw new ImageProcessingException("Error processing image with " + GetType().Name, ex);
            }

            return image;
        }

        /// <summary>
        /// Performs a forward rotation of an image
        /// </summary>
        /// <param name="orientation">The EXIF orientation value.</param>
        /// <param name="image">The image</param>
        private void ForwardRotateFlip(int orientation, ref Image image)
        {
            switch (orientation)
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

        /// <summary>
        /// Performs an inverse rotation of an image 
        /// </summary>
        /// <param name="orientation">The EXIF orientation value.</param>
        /// <param name="image">The image</param>
        private void ReverseRotateFlip(int orientation, ref Image image)
        {
            switch (orientation)
            {
                case 8:
                    // Rotated 90 right
                    image.RotateFlip(RotateFlipType.Rotate90FlipNone);
                    break;

                case 7: // Rotated 90 right, flip horizontally
                    image.RotateFlip(RotateFlipType.Rotate90FlipX);
                    break;

                case 6: // Rotated 90 left
                    image.RotateFlip(RotateFlipType.Rotate270FlipNone);
                    break;

                case 5: // Rotated 90 left, flip horizontally
                    image.RotateFlip(RotateFlipType.Rotate270FlipX);
                    break;

                case 3: // Rotate 180 left
                    image.RotateFlip(RotateFlipType.Rotate180FlipNone);
                    break;

                case 2: // Flip horizontally
                    image.RotateFlip(RotateFlipType.RotateNoneFlipX);
                    break;
            }
        }
    }
}