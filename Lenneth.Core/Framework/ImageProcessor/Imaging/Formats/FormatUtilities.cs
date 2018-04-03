﻿// --------------------------------------------------------------------------------------------------------------------
// <copyright file="FormatUtilities.cs" company="James Jackson-South">
//   Copyright (c) James Jackson-South.
//   Licensed under the Apache License, Version 2.0.
// </copyright>
// <summary>
//   Utility methods for working with supported image formats.
// </summary>
// --------------------------------------------------------------------------------------------------------------------

using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;
using System.Linq;
using System.Reflection;
using Lenneth.Core.Framework.ImageProcessor.Configuration;

namespace Lenneth.Core.Framework.ImageProcessor.Imaging.Formats
{
    /// <summary>
    /// Utility methods for working with supported image formats.
    /// </summary>
    public static class FormatUtilities
    {
        /// <summary>
        /// Gets the correct <see cref="ISupportedImageFormat"/> from the given stream.
        /// <see href="http://stackoverflow.com/questions/55869/determine-file-type-of-an-image"/>
        /// </summary>
        /// <param name="stream">
        /// The <see cref="System.IO.Stream"/> to read from.
        /// </param>
        /// <returns>
        /// The <see cref="ISupportedImageFormat"/>.
        /// </returns>
        public static ISupportedImageFormat GetFormat(Stream stream)
        {
            // Reset the position of the stream to ensure we're reading the correct part.
            if (stream.CanSeek)
            {
                stream.Position = 0;
            }

            var supportedImageFormats =
                ImageProcessorBootstrapper.Instance.SupportedImageFormats;

            // It's actually a list.
            // ReSharper disable once PossibleMultipleEnumeration
            var numberOfBytesToRead = supportedImageFormats.Max(f => f.FileHeaders.Max(h=>h.Length));

            var buffer = new byte[numberOfBytesToRead];
            stream.Read(buffer, 0, buffer.Length);

            // ReSharper disable once PossibleMultipleEnumeration
            foreach (var supportedImageFormat in supportedImageFormats)
            {
                var headers = supportedImageFormat.FileHeaders;

                // ReSharper disable once LoopCanBeConvertedToQuery
                foreach (var header in headers)
                {
                    if (header.SequenceEqual(buffer.Take(header.Length)))
                    {
                        if (stream.CanSeek)
                        {
                            stream.Position = 0;
                        }

                        // Return a new instance as we want to use instance properties.
                        return Activator.CreateInstance(supportedImageFormat.GetType()) as ISupportedImageFormat;
                    }
                }
            }

            if (stream.CanSeek)
            {
                stream.Position = 0;
            }

            return null;
        }

        /// <summary>
        /// Returns a value indicating whether the given image is indexed.
        /// </summary>
        /// <param name="image">
        /// The <see cref="System.Drawing.Image"/> to test.
        /// </param>
        /// <returns>
        /// The true if the image is indexed; otherwise, false.
        /// </returns>
        public static bool IsIndexed(Image image)
        {
            // Test value of flags using bitwise AND.
            // ReSharper disable once BitwiseOperatorOnEnumWithoutFlags
            return (image.PixelFormat & PixelFormat.Indexed) != 0;
        }

        /// <summary>
        /// Returns a value indicating whether the given image has an alpha channel.
        /// </summary>
        /// <param name="image">
        /// The <see cref="System.Drawing.Image"/> to test.
        /// </param>
        /// <returns>
        /// The true if the image has an alpha channel; otherwise, false.
        /// </returns>
        public static bool HasAlpha(Image image)
        {
            return ((ImageFlags)image.Flags & ImageFlags.HasAlpha) == ImageFlags.HasAlpha;
        }

        /// <summary>
        /// Returns a value indicating whether the given image is animated.
        /// </summary>
        /// <param name="image">
        /// The <see cref="System.Drawing.Image"/> to test.
        /// </param>
        /// <returns>
        /// The true if the image is animated; otherwise, false.
        /// </returns>
        public static bool IsAnimated(Image image)
        {
            return ImageAnimator.CanAnimate(image);
        }

        /// <summary>
        /// Returns an instance of EncodingParameters for jpeg compression.
        /// </summary>
        /// <param name="quality">The quality to return the image at.</param>
        /// <returns>The encodingParameters for jpeg compression. </returns>
        public static EncoderParameters GetEncodingParameters(int quality)
        {
            EncoderParameters encoderParameters = null;
            try
            {
                // Create a series of encoder parameters.
                encoderParameters = new EncoderParameters(1)
                {
                    // Set the quality.
                    Param = { [0] = new EncoderParameter(Encoder.Quality, quality) }
                };
            }
            catch
            {
                encoderParameters?.Dispose();
            }

            return encoderParameters;
        }

        /// <summary>
        /// Uses reflection to allow the creation of an instance of <see cref="PropertyItem"/>.
        /// </summary>
        /// <returns>
        /// The <see cref="PropertyItem"/>.
        /// </returns>
        public static PropertyItem CreatePropertyItem()
        {
            var type = typeof(PropertyItem);
            var constructor = type.GetConstructor(BindingFlags.NonPublic | BindingFlags.Instance | BindingFlags.Public, null, new Type[] { }, null);

            return (PropertyItem)constructor.Invoke(null);
        }
    }
}