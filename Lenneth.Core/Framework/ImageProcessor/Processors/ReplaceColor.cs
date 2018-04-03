// --------------------------------------------------------------------------------------------------------------------
// <copyright file="ReplaceColor.cs" company="James Jackson-South">
//   Copyright (c) James Jackson-South.
//   Licensed under the Apache License, Version 2.0.
// </copyright>
// <summary>
//   Encapsulates methods allowing the replacement of a color within an image.
// </summary>
// --------------------------------------------------------------------------------------------------------------------

using System;
using System.Collections.Generic;
using System.Drawing;
using System.Threading.Tasks;
using Lenneth.Core.Framework.ImageProcessor.Common.Exceptions;
using Lenneth.Core.Framework.ImageProcessor.Common.Extensions;
using Lenneth.Core.Framework.ImageProcessor.Imaging;
using Lenneth.Core.Framework.ImageProcessor.Imaging.Helpers;

namespace Lenneth.Core.Framework.ImageProcessor.Processors
{
    /// <summary>
    /// Encapsulates methods allowing the replacement of a color within an image.
    /// </summary>
    public class ReplaceColor : IGraphicsProcessor
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="ReplaceColor"/> class.
        /// </summary>
        public ReplaceColor()
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
                Tuple<Color, Color, int> parameters = DynamicParameter;
                var original = parameters.Item1;
                var replacement = parameters.Item2;

                var originalR = original.R;
                var originalG = original.G;
                var originalB = original.B;
                var originalA = original.A;

                var replacementR = replacement.R;
                var replacementG = replacement.G;
                var replacementB = replacement.B;
                var replacementA = replacement.A;

                var fuzziness = parameters.Item3;

                var minR = (originalR - fuzziness).ToByte();
                var minG = (originalG - fuzziness).ToByte();
                var minB = (originalB - fuzziness).ToByte();

                var maxR = (originalR + fuzziness).ToByte();
                var maxG = (originalG + fuzziness).ToByte();
                var maxB = (originalB + fuzziness).ToByte();

                newImage = new Bitmap(image);
                newImage.SetResolution(image.HorizontalResolution, image.VerticalResolution);
                var width = image.Width;
                var height = image.Height;

                using (var fastBitmap = new FastBitmap(newImage))
                {
                    Parallel.For(
                        0,
                        height,
                        y =>
                        {
                            for (var x = 0; x < width; x++)
                            {
                                // Get the pixel color.
                                // ReSharper disable once AccessToDisposedClosure
                                var currentColor = fastBitmap.GetPixel(x, y);

                                var currentR = currentColor.R;
                                var currentG = currentColor.G;
                                var currentB = currentColor.B;
                                var currentA = currentColor.A;

                                // Test whether it is in the expected range.
                                if (ImageMaths.InRange(currentR, minR, maxR))
                                {
                                    if (ImageMaths.InRange(currentG, minG, maxG))
                                    {
                                        if (ImageMaths.InRange(currentB, minB, maxB))
                                        {
                                            // Ensure the values are within an acceptable byte range
                                            // and set the new value.
                                            var r = (originalR - currentR + replacementR).ToByte();
                                            var g = (originalG - currentG + replacementG).ToByte();
                                            var b = (originalB - currentB + replacementB).ToByte();

                                            // Allow replacement with transparent color.
                                            var a = currentA;
                                            if (originalA != replacementA)
                                            {
                                                a = replacementA;
                                            }

                                            // ReSharper disable once AccessToDisposedClosure
                                            fastBitmap.SetPixel(x, y, Color.FromArgb(a, r, g, b));
                                        }
                                    }
                                }
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
