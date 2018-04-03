// --------------------------------------------------------------------------------------------------------------------
// <copyright file="OilPaintingFilter.cs" company="James Jackson-South">
//   Copyright (c) James Jackson-South.
//   Licensed under the Apache License, Version 2.0.
// </copyright>
// <summary>
//   The oil painting filter.
// </summary>
// --------------------------------------------------------------------------------------------------------------------

using System;
using System.Drawing;
using System.Drawing.Imaging;
using System.Threading.Tasks;
using Lenneth.Core.Framework.ImageProcessor.Common.Extensions;

namespace Lenneth.Core.Framework.ImageProcessor.Imaging.Filters.Artistic
{
    /// <summary>
    /// The oil painting filter.
    /// </summary>
    public class OilPaintingFilter
    {
        /// <summary>
        /// The levels.
        /// </summary>
        private int levels;

        /// <summary>
        /// The brush size.
        /// </summary>
        private int brushSize;

        /// <summary>
        /// Initializes a new instance of the <see cref="OilPaintingFilter"/> class.
        /// </summary>
        /// <param name="levels">
        /// The number of levels.
        /// </param>
        /// <param name="brushSize">
        /// The brush size.
        /// </param>
        public OilPaintingFilter(int levels, int brushSize)
        {
            this.levels = levels;
            this.brushSize = brushSize;
        }

        /// <summary>
        /// Gets or sets the number of levels.
        /// </summary>
        public int Levels
        {
            get
            {
                return levels;
            }

            set
            {
                if (value > 0)
                {
                    levels = value;
                }
            }
        }

        /// <summary>
        /// Gets or sets the brush size.
        /// </summary>
        public int BrushSize
        {
            get
            {
                return brushSize;
            }

            set
            {
                if (value > 0)
                {
                    brushSize = value;
                }
            }
        }

        /// <summary>
        /// Applies the oil painting filter. 
        /// </summary>
        /// <param name="source">
        /// The source.
        /// </param>
        /// <returns>
        /// The <see cref="Bitmap"/>.
        /// </returns>
        public Bitmap ApplyFilter(Bitmap source)
        {
            // TODO: Make this class implement an interface?
            var width = source.Width;
            var height = source.Height;

            var radius = brushSize >> 1;

            var destination = new Bitmap(width, height, PixelFormat.Format32bppPArgb);
            destination.SetResolution(source.HorizontalResolution, source.VerticalResolution);
            using (var sourceBitmap = new FastBitmap(source))
            {
                using (var destinationBitmap = new FastBitmap(destination))
                {
                    Parallel.For(
                        0,
                        height,
                        y =>
                        {
                            for (var x = 0; x < width; x++)
                            {
                                var maxIntensity = 0;
                                var maxIndex = 0;
                                var intensityBin = new int[levels];
                                var blueBin = new int[levels];
                                var greenBin = new int[levels];
                                var redBin = new int[levels];
                                byte sourceAlpha = 255;

                                for (var i = 0; i <= radius; i++)
                                {
                                    var ir = i - radius;
                                    var offsetY = y + ir;

                                    // Skip the current row
                                    if (offsetY < 0)
                                    {
                                        continue;
                                    }

                                    // Outwith the current bounds so break.
                                    if (offsetY >= height)
                                    {
                                        break;
                                    }

                                    for (var fx = 0; fx <= radius; fx++)
                                    {
                                        var jr = fx - radius;
                                        var offsetX = x + jr;

                                        // Skip the column
                                        if (offsetX < 0)
                                        {
                                            continue;
                                        }

                                        if (offsetX < width)
                                        {
                                            // ReSharper disable once AccessToDisposedClosure
                                            var color = sourceBitmap.GetPixel(offsetX, offsetY);

                                            var sourceBlue = color.B;
                                            var sourceGreen = color.G;
                                            var sourceRed = color.R;
                                            sourceAlpha = color.A;

                                            var currentIntensity = (int)Math.Round(((sourceBlue + sourceGreen + sourceRed) / 3.0 * (levels - 1)) / 255.0);

                                            intensityBin[currentIntensity] += 1;
                                            blueBin[currentIntensity] += sourceBlue;
                                            greenBin[currentIntensity] += sourceGreen;
                                            redBin[currentIntensity] += sourceRed;

                                            if (intensityBin[currentIntensity] > maxIntensity)
                                            {
                                                maxIntensity = intensityBin[currentIntensity];
                                                maxIndex = currentIntensity;
                                            }
                                        }
                                    }
                                }

                                var blue = Math.Abs(blueBin[maxIndex] / maxIntensity).ToByte();
                                var green = Math.Abs(greenBin[maxIndex] / maxIntensity).ToByte();
                                var red = Math.Abs(redBin[maxIndex] / maxIntensity).ToByte();

                                // ReSharper disable once AccessToDisposedClosure
                                destinationBitmap.SetPixel(x, y, Color.FromArgb(sourceAlpha, red, green, blue));
                            }
                        });
                }
            }

            return destination;
        }
    }
}
