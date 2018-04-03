// --------------------------------------------------------------------------------------------------------------------
// <copyright file="Pixelate.cs" company="James Jackson-South">
//   Copyright (c) James Jackson-South.
//   Licensed under the Apache License, Version 2.0.
// </copyright>
// <summary>
//   Encapsulates methods to pixelate an image.
// </summary>
// --------------------------------------------------------------------------------------------------------------------

using System;
using System.Collections.Generic;
using System.Drawing;
using System.Threading.Tasks;
using Lenneth.Core.Framework.ImageProcessor.Common.Exceptions;
using Lenneth.Core.Framework.ImageProcessor.Common.Extensions;
using Lenneth.Core.Framework.ImageProcessor.Imaging;

namespace Lenneth.Core.Framework.ImageProcessor.Processors
{
    /// <summary>
    /// Encapsulates methods to pixelate an image.
    /// </summary>
    public class Pixelate : IGraphicsProcessor
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="Pixelate"/> class.
        /// </summary>
        public Pixelate()
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
            Bitmap newImage = null;
            var image = factory.Image;

            try
            {
                Tuple<int, Rectangle?> parameters = DynamicParameter;
                var size = parameters.Item1;
                var rectangle = parameters.Item2 ?? new Rectangle(0, 0, image.Width, image.Height);
                var x = rectangle.X;
                var y = rectangle.Y;
                var offset = size / 2;
                var width = rectangle.Width;
                var height = rectangle.Height;
                var maxWidth = image.Width;
                var maxHeight = image.Height;

                newImage = new Bitmap(image);
                newImage.SetResolution(image.HorizontalResolution, image.VerticalResolution);

                using (var fastBitmap = new FastBitmap(newImage))
                {
                    // Get the range on the y-plane to choose from.
                    var range = EnumerableExtensions.SteppedRange(y, i => i < y + height && i < maxHeight, size);

                    Parallel.ForEach(
                        range,
                        j =>
                        {
                            for (var i = x; i < x + width && i < maxWidth; i += size)
                            {
                                var offsetX = offset;
                                var offsetY = offset;

                                // Make sure that the offset is within the boundary of the image.
                                while (j + offsetY >= maxHeight)
                                {
                                    offsetY--;
                                }

                                while (i + offsetX >= maxWidth)
                                {
                                    offsetX--;
                                }

                                // Get the pixel color in the centre of the soon to be pixelated area.
                                // ReSharper disable AccessToDisposedClosure
                                var pixel = fastBitmap.GetPixel(i + offsetX, j + offsetY);

                                // For each pixel in the pixelate size, set it to the centre color.
                                for (var l = j; l < j + size && l < maxHeight; l++)
                                {
                                    for (var k = i; k < i + size && k < maxWidth; k++)
                                    {
                                        fastBitmap.SetPixel(k, l, pixel);
                                    }
                                }
                                // ReSharper restore AccessToDisposedClosure
                            }
                        });
                }

                image.Dispose();
                image = newImage;
            }
            catch (Exception ex)
            {
                newImage?.Dispose();

                throw new ImageProcessingException("Error processing image with " + GetType().Name, ex);
            }

            return image;
        }
    }
}
