// --------------------------------------------------------------------------------------------------------------------
// <copyright file="ConvolutionFilter.cs" company="James Jackson-South">
//   Copyright (c) James Jackson-South.
//   Licensed under the Apache License, Version 2.0.
// </copyright>
// <summary>
//   The convolution filter for applying gradient operators to detect edges within an image.
// </summary>
// --------------------------------------------------------------------------------------------------------------------

using System;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Drawing.Imaging;
using System.Threading.Tasks;
using Lenneth.Core.Framework.ImageProcessor.Common.Extensions;
using Lenneth.Core.Framework.ImageProcessor.Imaging.Filters.Photo;

namespace Lenneth.Core.Framework.ImageProcessor.Imaging.Filters.EdgeDetection
{
    /// <summary>
    /// The convolution filter for applying gradient operators to detect edges within an image.
    /// </summary>
    public class ConvolutionFilter
    {
        /// <summary>
        /// The edge filter.
        /// </summary>
        private readonly IEdgeFilter _edgeFilter;

        /// <summary>
        /// Whether to produce a greyscale output.
        /// </summary>
        private readonly bool _greyscale;

        /// <summary>
        /// Initializes a new instance of the <see cref="ConvolutionFilter"/> class.
        /// </summary>
        /// <param name="edgeFilter">
        /// The <see cref="IEdgeFilter"/> to apply.
        /// </param>
        /// <param name="greyscale">
        /// Whether to produce a greyscale output.
        /// </param>
        public ConvolutionFilter(IEdgeFilter edgeFilter, bool greyscale)
        {
            _edgeFilter = edgeFilter;
            _greyscale = greyscale;
        }

        /// <summary>
        /// Processes the given bitmap to apply the current instance of <see cref="IEdgeFilter"/>.
        /// </summary>
        /// <param name="source">The image to process.</param>
        /// <returns>A processed bitmap.</returns>
        public Bitmap ProcessFilter(Image source)
        {
            var width = source.Width;
            var height = source.Height;
            var maxWidth = width + 1;
            var maxHeight = height + 1;
            var bufferedWidth = width + 2;
            var bufferedHeight = height + 2;

            var destination = new Bitmap(width, height, PixelFormat.Format32bppPArgb);
            var input = new Bitmap(bufferedWidth, bufferedHeight, PixelFormat.Format32bppPArgb);
            destination.SetResolution(source.HorizontalResolution, source.VerticalResolution);
            input.SetResolution(source.HorizontalResolution, source.VerticalResolution);

            using (var graphics = Graphics.FromImage(input))
            {
                // Fixes an issue with transparency not converting properly.
                graphics.Clear(Color.Transparent);

                var destinationRectangle = new Rectangle(0, 0, bufferedWidth, bufferedHeight);
                var rectangle = new Rectangle(0, 0, width, height);

                // If it's greyscale apply a colormatrix to the image.
                using (var attributes = new ImageAttributes())
                {
                    if (_greyscale)
                    {
                        attributes.SetColorMatrix(ColorMatrixes.GreyScale);
                    }

                    // We use a trick here to detect right to the edges of the image.
                    // flip/tile the image with a pixel in excess in each direction to duplicate pixels.
                    // Later on we draw pixels without that excess.
                    using (var tb = new TextureBrush(source, rectangle, attributes))
                    {
                        tb.WrapMode = WrapMode.TileFlipXY;
                        tb.TranslateTransform(1, 1);

                        graphics.FillRectangle(tb, destinationRectangle);
                    }
                }
            }

            try
            {
                var horizontalFilter = _edgeFilter.HorizontalGradientOperator;

                var kernelLength = horizontalFilter.GetLength(0);
                var radius = kernelLength >> 1;

                using (var sourceBitmap = new FastBitmap(input))
                {
                    using (var destinationBitmap = new FastBitmap(destination))
                    {
                        // Loop through the pixels.
                        Parallel.For(
                            0,
                            bufferedHeight,
                            y =>
                            {
                                for (var x = 0; x < bufferedWidth; x++)
                                {
                                    double rX = 0;
                                    double gX = 0;
                                    double bX = 0;

                                    // Apply each matrix multiplier to the color components for each pixel.
                                    for (var fy = 0; fy < kernelLength; fy++)
                                    {
                                        var fyr = fy - radius;
                                        var offsetY = y + fyr;

                                        // Skip the current row
                                        if (offsetY < 0)
                                        {
                                            continue;
                                        }

                                        // Outwith the current bounds so break.
                                        if (offsetY >= bufferedHeight)
                                        {
                                            break;
                                        }

                                        for (var fx = 0; fx < kernelLength; fx++)
                                        {
                                            var fxr = fx - radius;
                                            var offsetX = x + fxr;

                                            // Skip the column
                                            if (offsetX < 0)
                                            {
                                                continue;
                                            }

                                            if (offsetX < bufferedWidth)
                                            {
                                                // ReSharper disable once AccessToDisposedClosure
                                                var currentColor = sourceBitmap.GetPixel(offsetX, offsetY);
                                                double r = currentColor.R;
                                                double g = currentColor.G;
                                                double b = currentColor.B;

                                                rX += horizontalFilter[fy, fx] * r;

                                                gX += horizontalFilter[fy, fx] * g;

                                                bX += horizontalFilter[fy, fx] * b;
                                            }
                                        }
                                    }

                                    // Apply the equation and sanitize.
                                    var red = rX.ToByte();
                                    var green = gX.ToByte();
                                    var blue = bX.ToByte();

                                    var newColor = Color.FromArgb(red, green, blue);
                                    if (y > 0 && x > 0 && y < maxHeight && x < maxWidth)
                                    {
                                        // ReSharper disable once AccessToDisposedClosure
                                        destinationBitmap.SetPixel(x - 1, y - 1, newColor);
                                    }
                                }
                            });
                    }
                }
            }
            finally
            {
                // We created a new image. Cleanup.
                input.Dispose();
            }

            return destination;
        }

        /// <summary>
        /// Processes the given bitmap to apply the current instance of <see cref="I2DEdgeFilter"/>.
        /// </summary>
        /// <param name="source">The image to process.</param>
        /// <returns>A processed bitmap.</returns>
        public Bitmap Process2DFilter(Image source)
        {
            var width = source.Width;
            var height = source.Height;
            var maxWidth = width + 1;
            var maxHeight = height + 1;
            var bufferedWidth = width + 2;
            var bufferedHeight = height + 2;

            var destination = new Bitmap(width, height, PixelFormat.Format32bppPArgb);
            var input = new Bitmap(bufferedWidth, bufferedHeight, PixelFormat.Format32bppPArgb);
            destination.SetResolution(source.HorizontalResolution, source.VerticalResolution);
            input.SetResolution(source.HorizontalResolution, source.VerticalResolution);

            using (var graphics = Graphics.FromImage(input))
            {
                // Fixes an issue with transparency not converting properly.
                graphics.Clear(Color.Transparent);

                var destinationRectangle = new Rectangle(0, 0, bufferedWidth, bufferedHeight);
                var rectangle = new Rectangle(0, 0, width, height);

                // If it's greyscale apply a colormatrix to the image.
                using (var attributes = new ImageAttributes())
                {
                    if (_greyscale)
                    {
                        attributes.SetColorMatrix(ColorMatrixes.GreyScale);
                    }

                    // We use a trick here to detect right to the edges of the image.
                    // flip/tile the image with a pixel in excess in each direction to duplicate pixels.
                    // Later on we draw pixels without that excess.
                    using (var tb = new TextureBrush(source, rectangle, attributes))
                    {
                        tb.WrapMode = WrapMode.TileFlipXY;
                        tb.TranslateTransform(1, 1);

                        graphics.FillRectangle(tb, destinationRectangle);
                    }
                }
            }

            try
            {
                var horizontalFilter = _edgeFilter.HorizontalGradientOperator;
                var verticalFilter = ((I2DEdgeFilter)_edgeFilter).VerticalGradientOperator;

                var kernelLength = horizontalFilter.GetLength(0);
                var radius = kernelLength >> 1;

                using (var sourceBitmap = new FastBitmap(input))
                {
                    using (var destinationBitmap = new FastBitmap(destination))
                    {
                        // Loop through the pixels.
                        Parallel.For(
                            0,
                            bufferedHeight,
                            y =>
                            {
                                for (var x = 0; x < bufferedWidth; x++)
                                {
                                    double rX = 0;
                                    double rY = 0;
                                    double gX = 0;
                                    double gY = 0;
                                    double bX = 0;
                                    double bY = 0;

                                    // Apply each matrix multiplier to the color components for each pixel.
                                    for (var fy = 0; fy < kernelLength; fy++)
                                    {
                                        var fyr = fy - radius;
                                        var offsetY = y + fyr;

                                        // Skip the current row
                                        if (offsetY < 0)
                                        {
                                            continue;
                                        }

                                        // Outwith the current bounds so break.
                                        if (offsetY >= bufferedHeight)
                                        {
                                            break;
                                        }

                                        for (var fx = 0; fx < kernelLength; fx++)
                                        {
                                            var fxr = fx - radius;
                                            var offsetX = x + fxr;

                                            // Skip the column
                                            if (offsetX < 0)
                                            {
                                                continue;
                                            }

                                            if (offsetX < bufferedWidth)
                                            {
                                                // ReSharper disable once AccessToDisposedClosure
                                                var currentColor = sourceBitmap.GetPixel(offsetX, offsetY);
                                                double r = currentColor.R;
                                                double g = currentColor.G;
                                                double b = currentColor.B;

                                                rX += horizontalFilter[fy, fx] * r;
                                                rY += verticalFilter[fy, fx] * r;

                                                gX += horizontalFilter[fy, fx] * g;
                                                gY += verticalFilter[fy, fx] * g;

                                                bX += horizontalFilter[fy, fx] * b;
                                                bY += verticalFilter[fy, fx] * b;
                                            }
                                        }
                                    }

                                    // Apply the equation and sanitize.
                                    var red = Math.Sqrt((rX * rX) + (rY * rY)).ToByte();
                                    var green = Math.Sqrt((gX * gX) + (gY * gY)).ToByte();
                                    var blue = Math.Sqrt((bX * bX) + (bY * bY)).ToByte();

                                    var newColor = Color.FromArgb(red, green, blue);
                                    if (y > 0 && x > 0 && y < maxHeight && x < maxWidth)
                                    {
                                        // ReSharper disable once AccessToDisposedClosure
                                        destinationBitmap.SetPixel(x - 1, y - 1, newColor);
                                    }
                                }
                            });
                    }
                }
            }
            finally
            {
                // We created a new image. Cleanup.
                input.Dispose();
            }

            return destination;
        }
    }
}
