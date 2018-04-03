// --------------------------------------------------------------------------------------------------------------------
// <copyright file="ImageBuffer.cs" company="James Jackson-South">
//   Copyright (c) James Jackson-South.
//   Licensed under the Apache License, Version 2.0.
// </copyright>
// <summary>
//   The image buffer for storing and manipulating pixel information.
//   Adapted from <see href="https://github.com/drewnoakes" />
// </summary>
// --------------------------------------------------------------------------------------------------------------------

using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Imaging;
using System.Runtime.InteropServices;
using Lenneth.Core.Framework.ImageProcessor.Common.Exceptions;
using Lenneth.Core.Framework.ImageProcessor.Imaging.Colors;

namespace Lenneth.Core.Framework.ImageProcessor.Imaging.Quantizers.WuQuantizer
{
    /// <summary>
    /// The image buffer for storing and manipulating pixel information.
    /// Adapted from <see href="https://github.com/drewnoakes"/>
    /// </summary>
    internal class ImageBuffer
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="ImageBuffer"/> class.
        /// </summary>
        /// <param name="image">
        /// The image to store.
        /// </param>
        public ImageBuffer(Bitmap image)
        {
            Image = image;
        }

        /// <summary>
        /// Gets the image.
        /// </summary>
        public Bitmap Image { get; }

        /// <summary>
        /// Gets the enumerable pixel array representing each row of pixels.
        /// </summary>
        /// <exception cref="QuantizationException">
        /// Thrown if the given image is not a 32 bit per pixel image.
        /// </exception>
        public IEnumerable<Color32[]> PixelLines
        {
            get
            {
                var width = Image.Width;
                var height = Image.Height;
                var pixels = new Color32[width];

                using (var bitmap = new FastBitmap(Image))
                {
                    for (var y = 0; y < height; y++)
                    {
                        for (var x = 0; x < width; x++)
                        {
                            var color = bitmap.GetPixel(x, y);
                            pixels[x] = new Color32(color.A, color.R, color.G, color.B);
                        }

                        yield return pixels;
                    }
                }
            }
        }

        /// <summary>
        /// Updates the pixel indexes.
        /// </summary>
        /// <param name="lineIndexes">
        /// The enumerable byte array representing each row of pixels.
        /// </param>
        public void UpdatePixelIndexes(IEnumerable<byte[]> lineIndexes)
        {
            var width = Image.Width;
            var height = Image.Height;
            var rowIndex = 0;

            var data = Image.LockBits(Rectangle.FromLTRB(0, 0, width, height), ImageLockMode.WriteOnly, PixelFormat.Format8bppIndexed);
            try
            {
                var pixelBase = data.Scan0;
                var scanWidth = data.Stride;
                foreach (var scanLine in lineIndexes)
                {
                    // TODO: Use unsafe code
                    Marshal.Copy(scanLine, 0, IntPtr.Add(pixelBase, scanWidth * rowIndex), width);

                    if (++rowIndex >= height)
                    {
                        break;
                    }
                }
            }
            finally
            {
                Image.UnlockBits(data);
            }
        }
    }
}
