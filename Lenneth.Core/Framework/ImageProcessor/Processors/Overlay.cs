// --------------------------------------------------------------------------------------------------------------------
// <copyright file="Overlay.cs" company="James Jackson-South">
//   Copyright (c) James Jackson-South.
//   Licensed under the Apache License, Version 2.0.
// </copyright>
// <summary>
//   Adds an image overlay to the current image.
// </summary>
// --------------------------------------------------------------------------------------------------------------------

using System;
using System.Collections.Generic;
using System.Drawing;
using Lenneth.Core.Framework.ImageProcessor.Common.Exceptions;
using Lenneth.Core.Framework.ImageProcessor.Imaging;
using Lenneth.Core.Framework.ImageProcessor.Imaging.Helpers;

namespace Lenneth.Core.Framework.ImageProcessor.Processors
{
    /// <summary>
    /// Adds an image overlay to the current image.
    /// If the overlay is larger than the image it will be scaled to match the image.
    /// </summary>
    public class Overlay : IGraphicsProcessor
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="Overlay"/> class.
        /// </summary>
        public Overlay()
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
            var image = factory.Image;

            Bitmap overlay = null;
            try
            {
                ImageLayer imageLayer = DynamicParameter;
                overlay = new Bitmap(imageLayer.Image);

                // Set the resolution of the overlay and the image to match.
                overlay.SetResolution(image.HorizontalResolution, image.VerticalResolution);

                var size = imageLayer.Size;
                var width = image.Width;
                var height = image.Height;
                var overlayWidth = size != Size.Empty ? Math.Min(width, size.Width) : Math.Min(width, overlay.Width);
                var overlayHeight = size != Size.Empty ? Math.Min(height, size.Height) : Math.Min(height, overlay.Height);

                var position = imageLayer.Position;
                var opacity = imageLayer.Opacity;

                if (image.Size != overlay.Size || image.Size != new Size(overlayWidth, overlayHeight))
                {
                    // Find the maximum possible dimensions and resize the image.
                    var layer = new ResizeLayer(new Size(overlayWidth, overlayHeight), ResizeMode.Max);
                    var resizer = new Resizer(layer) { AnimationProcessMode = factory.AnimationProcessMode };
                    overlay = resizer.ResizeImage(overlay, factory.FixGamma);
                    overlayWidth = overlay.Width;
                    overlayHeight = overlay.Height;
                }

                // Figure out bounds.
                var parent = new Rectangle(0, 0, width, height);
                var child = new Rectangle(0, 0, overlayWidth, overlayHeight);

                // Apply opacity.
                if (opacity < 100)
                {
                    overlay = Adjustments.Alpha(overlay, opacity);
                }

                using (var graphics = Graphics.FromImage(image))
                {
                    GraphicsHelper.SetGraphicsOptions(graphics, true);

                    if (position != null)
                    {
                        // Draw the image in position catering for overflow.
                        graphics.DrawImage(overlay, new Point(Math.Min(position.Value.X, width - overlayWidth), Math.Min(position.Value.Y, height - overlayHeight)));
                    }
                    else
                    {
                        var centered = ImageMaths.CenteredRectangle(parent, child);
                        graphics.DrawImage(overlay, new PointF(centered.X, centered.Y));
                    }
                }
            }
            catch (Exception ex)
            {
                throw new ImageProcessingException("Error processing image with " + GetType().Name, ex);
            }
            finally
            {
                overlay?.Dispose();
            }

            return image;
        }
    }
}
