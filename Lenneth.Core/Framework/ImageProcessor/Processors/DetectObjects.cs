// --------------------------------------------------------------------------------------------------------------------
// <copyright file="DetectObjects.cs" company="James South">
//   Copyright (c) James South.
//   Licensed under the Apache License, Version 2.0.
// </copyright>
// <summary>
//   Encapsulates methods to change the DetectObjects component of the image to effect its transparency.
// </summary>
// --------------------------------------------------------------------------------------------------------------------

using System;
using System.Collections.Generic;
using System.Drawing;
using Lenneth.Core.Framework.ImageProcessor.Common.Exceptions;
using Lenneth.Core.Framework.ImageProcessor.Imaging.Filters.ObjectDetection;
using Lenneth.Core.Framework.ImageProcessor.Imaging.Filters.ObjectDetection.HaarCascade;
using Lenneth.Core.Framework.ImageProcessor.Imaging.Filters.Photo;

namespace Lenneth.Core.Framework.ImageProcessor.Processors
{
    /// <summary>
    /// Encapsulates methods to change the DetectObjects component of the image to effect its transparency.
    /// </summary>
    public class DetectObjects : IGraphicsProcessor
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="DetectObjects"/> class.
        /// </summary>
        public DetectObjects()
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
            Bitmap grey = null;
            var image = factory.Image;

            try
            {
                HaarCascade cascade = DynamicParameter;
                grey = new Bitmap(image.Width, image.Height);
                grey.SetResolution(image.HorizontalResolution, image.VerticalResolution);
                grey = MatrixFilters.GreyScale.TransformImage(image, grey);

                var detector = new HaarObjectDetector(cascade)
                {
                    SearchMode = ObjectDetectorSearchMode.NoOverlap,
                    ScalingMode = ObjectDetectorScalingMode.GreaterToSmaller,
                    ScalingFactor = 1.5f
                };

                // Process frame to detect objects
                var rectangles = detector.ProcessFrame(grey);
                grey.Dispose();

                newImage = new Bitmap(image);
                newImage.SetResolution(image.HorizontalResolution, image.VerticalResolution);
                using (var graphics = Graphics.FromImage(newImage))
                {
                    using (var blackPen = new Pen(Color.White))
                    {
                        blackPen.Width = 4;
                        graphics.DrawRectangles(blackPen, rectangles);
                    }
                }

                image.Dispose();
                image = newImage;
            }
            catch (Exception ex)
            {
                grey?.Dispose();

                newImage?.Dispose();

                throw new ImageProcessingException("Error processing image with " + GetType().Name, ex);
            }

            return image;
        }
    }
}
