﻿// --------------------------------------------------------------------------------------------------------------------
// <copyright file="ImageLayer.cs" company="James Jackson-South">
//   Copyright (c) James Jackson-South.
//   Licensed under the Apache License, Version 2.0.
// </copyright>
// <summary>
//   Encapsulates the properties required to add an image layer to an image.
// </summary>
// --------------------------------------------------------------------------------------------------------------------

using System;
using System.Drawing;

namespace Lenneth.Core.Framework.ImageProcessor.Imaging
{
    /// <summary>
    /// Encapsulates the properties required to add an image layer to an image.
    /// </summary>
    public class ImageLayer : IDisposable
    {
        /// <summary>
        /// A value indicating whether this instance of the given entity has been disposed.
        /// </summary>
        /// <value><see langword="true"/> if this instance has been disposed; otherwise, <see langword="false"/>.</value>
        /// <remarks>
        /// If the entity is disposed, it must not be disposed a second
        /// time. The isDisposed field is set the first time the entity
        /// is disposed. If the isDisposed field is true, then the Dispose()
        /// method will not dispose again. This help not to prolong the entity's
        /// life in the Garbage Collector.
        /// </remarks>
        private bool isDisposed = false;

        /// <summary>
        /// Gets or sets the image.
        /// </summary>
        public Image Image { get; set; }

        /// <summary>
        /// Gets or sets the size.
        /// </summary>
        public Size Size { get; set; }

        /// <summary>
        /// Gets or sets the Opacity of the text layer.
        /// </summary>
        public int Opacity { get; set; } = 100;

        /// <summary>
        /// Gets or sets the Position of the text layer.
        /// </summary>
        public Point? Position { get; set; }

        /// <summary>
        /// Returns a value that indicates whether the specified object is an
        /// <see cref="ImageLayer"/> object that is equivalent to
        /// this <see cref="ImageLayer"/> object.
        /// </summary>
        /// <param name="obj">
        /// The object to test.
        /// </param>
        /// <returns>
        /// True if the given object  is an <see cref="ImageLayer"/> object that is equivalent to
        /// this <see cref="ImageLayer"/> object; otherwise, false.
        /// </returns>
        public override bool Equals(object obj)
        {
            var imageLayer = obj as ImageLayer;

            if (imageLayer == null)
            {
                return false;
            }

            return Image == imageLayer.Image
                && Size == imageLayer.Size
                && Opacity == imageLayer.Opacity
                && Position == imageLayer.Position;
        }

        /// <summary>
        /// Returns the hash code for this instance.
        /// </summary>
        /// <returns>
        /// A 32-bit signed integer that is the hash code for this instance.
        /// </returns>
        public override int GetHashCode()
        {
            unchecked
            {
                var hashCode = Image.GetHashCode();
                hashCode = (hashCode * 397) ^ Size.GetHashCode();
                hashCode = (hashCode * 397) ^ Opacity;
                hashCode = (hashCode * 397) ^ Position.GetHashCode();
                return hashCode;
            }
        }

        /// <summary>
        /// Disposes the object and frees resources for the Garbage Collector.
        /// </summary>
        public void Dispose()
        {
            Dispose(true);
        }

        /// <summary>
        /// Disposes the object and frees resources for the Garbage Collector.
        /// </summary>
        /// <param name="disposing">If true, the object gets disposed.</param>
        protected virtual void Dispose(bool disposing)
        {
            if (!isDisposed)
            {
                if (disposing)
                {
                    Image?.Dispose();
                }

                isDisposed = true;
            }
        }
    }
}