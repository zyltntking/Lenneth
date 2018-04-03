// --------------------------------------------------------------------------------------------------------------------
// <copyright file="BinaryThreshold.cs" company="James Jackson-South">
//   Copyright (c) James Jackson-South.
//   Licensed under the Apache License, Version 2.0.
// </copyright>
// <summary>
//   Performs binary threshold filtering against a given greyscale image.
// </summary>
// --------------------------------------------------------------------------------------------------------------------

using System.Drawing;
using System.Threading.Tasks;

namespace Lenneth.Core.Framework.ImageProcessor.Imaging.Filters.Binarization
{
    /// <summary>
    /// Performs binary threshold filtering against a given greyscale image.
    /// </summary>
    public class BinaryThreshold
    {
        /// <summary>
        /// The threshold value.
        /// </summary>
        private byte threshold;

        /// <summary>
        /// Initializes a new instance of the <see cref="BinaryThreshold"/> class.
        /// </summary>
        /// <param name="threshold">
        /// The threshold.
        /// </param>
        public BinaryThreshold(byte threshold = 10)
        {
            this.threshold = threshold;
        }

        /// <summary>
        /// Gets or sets the threshold.
        /// </summary>
        public byte Threshold
        {
            get
            {
                return threshold;
            }

            set
            {
                threshold = value;
            }
        }

        /// <summary>
        /// Processes the given bitmap to apply the threshold.
        /// </summary>
        /// <param name="source">
        /// The image to process.
        /// </param>
        /// <returns>
        /// A processed bitmap.
        /// </returns>
        public Bitmap ProcessFilter(Bitmap source)
        {
            var width = source.Width;
            var height = source.Height;

            using (var sourceBitmap = new FastBitmap(source))
            {
                Parallel.For(
                    0, 
                    height, 
                    y =>
                    {
                        for (var x = 0; x < width; x++)
                        {
                            // ReSharper disable AccessToDisposedClosure
                            var color = sourceBitmap.GetPixel(x, y);
                            sourceBitmap.SetPixel(x, y, color.B >= threshold ? Color.White : Color.Black);

                            // ReSharper restore AccessToDisposedClosure
                        }
                    });
            }

            return source;
        }
    }
}
