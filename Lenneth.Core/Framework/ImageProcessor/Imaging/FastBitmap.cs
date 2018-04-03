// --------------------------------------------------------------------------------------------------------------------
// <copyright file="FastBitmap.cs" company="James Jackson-South">
//   Copyright (c) James Jackson-South.
//   Licensed under the Apache License, Version 2.0.
// </copyright>
// <summary>
//   Allows fast access to <see cref="System.Drawing.Bitmap" />'s pixel data.
// </summary>
// --------------------------------------------------------------------------------------------------------------------

using System;
using System.Drawing;
using System.Drawing.Imaging;
using System.Runtime.InteropServices;
using Lenneth.Core.Framework.ImageProcessor.Imaging.Colors;

namespace Lenneth.Core.Framework.ImageProcessor.Imaging
{
    /// <summary>
    /// Allows fast access to <see cref="System.Drawing.Bitmap"/>'s pixel data.
    /// </summary>
    public unsafe class FastBitmap : IDisposable
    {
        /// <summary>
        /// The integral representation of the 8bppIndexed pixel format.
        /// </summary>
        // ReSharper disable once InconsistentNaming
        private const int Format8bppIndexed = (int)PixelFormat.Format8bppIndexed;

        /// <summary>
        /// The integral representation of the 24bppRgb pixel format.
        /// </summary>
        // ReSharper disable once InconsistentNaming
        private const int Format24bppRgb = (int)PixelFormat.Format24bppRgb;

        /// <summary>
        /// The integral representation of the 32bppArgb pixel format.
        /// </summary>
        // ReSharper disable once InconsistentNaming
        private const int Format32bppArgb = (int)PixelFormat.Format32bppArgb;

        /// <summary>
        /// The integral representation of the 32bppPArgb pixel format.
        /// </summary>
        // ReSharper disable once InconsistentNaming
        private const int Format32bppPArgb = (int)PixelFormat.Format32bppPArgb;

        /// <summary>
        /// The bitmap.
        /// </summary>
        private readonly Bitmap bitmap;

        /// <summary>
        /// The width of the bitmap.
        /// </summary>
        private readonly int width;

        /// <summary>
        /// The height of the bitmap.
        /// </summary>
        private readonly int height;

        /// <summary>
        /// The color channel - blue, green, red, alpha.
        /// </summary>
        private readonly int channel;

        /// <summary>
        /// Whether to compute integral rectangles.
        /// </summary>
        private readonly bool computeIntegrals;

        /// <summary>
        /// Whether to compute tilted integral rectangles.
        /// </summary>
        private readonly bool computeTilted;

        /// <summary>
        /// The normal integral image.
        /// </summary>
        private long[,] normalSumImage;

        /// <summary>
        /// The squared integral image.
        /// </summary>
        private long[,] squaredSumImage;

        /// <summary>
        /// The tilted sum image.
        /// </summary>
        private long[,] tiltedSumImage;

        /// <summary>
        /// The normal width.
        /// </summary>
        private int normalWidth;

        /// <summary>
        /// The tilted width.
        /// </summary>
        private int tiltedWidth;

        /// <summary>
        /// The number of bytes in a row.
        /// </summary>
        private int bytesInARow;

        /// <summary>
        /// The normal integral sum.
        /// </summary>
        private long* normalSum;

        /// <summary>
        /// The squared integral sum.
        /// </summary>
        private long* squaredSum;

        /// <summary>
        /// The tilted integral sum.
        /// </summary>
        private long* tiltedSum;

        /// <summary>
        /// The normal sum handle.
        /// </summary>
        private GCHandle normalSumHandle;

        /// <summary>
        /// The squared sum handle.
        /// </summary>
        private GCHandle squaredSumHandle;

        /// <summary>
        /// The tilted sum handle.
        /// </summary>
        private GCHandle tiltedSumHandle;

        /// <summary>
        /// The size of the color32 structure.
        /// </summary>
        private int pixelSize;

        /// <summary>
        /// The bitmap data.
        /// </summary>
        private BitmapData bitmapData;

        /// <summary>
        /// The position of the first pixel in the bitmap.
        /// </summary>
        private byte* pixelBase;

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
        private bool isDisposed;

        /// <summary>
        /// Initializes a new instance of the <see cref="FastBitmap"/> class.
        /// </summary>
        /// <param name="bitmap">The input bitmap.</param>
        public FastBitmap(Image bitmap)
            : this(bitmap, false)
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="FastBitmap"/> class.
        /// </summary>
        /// <param name="bitmap">The input bitmap.</param>
        /// <param name="computeIntegrals">
        /// Whether to compute integral rectangles.
        /// </param>
        public FastBitmap(Image bitmap, bool computeIntegrals)
            : this(bitmap, computeIntegrals, false)
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="FastBitmap"/> class.
        /// </summary>
        /// <param name="bitmap">The input bitmap.</param>
        /// <param name="computeIntegrals">
        /// Whether to compute integral rectangles.
        /// </param>
        /// <param name="computeTilted">
        /// Whether to compute tilted integral rectangles.
        /// </param>
        public FastBitmap(Image bitmap, bool computeIntegrals, bool computeTilted)
        {
            var pixelFormat = (int)bitmap.PixelFormat;

            // Check image format
            if (!(pixelFormat == Format8bppIndexed ||
                  pixelFormat == Format24bppRgb ||
                  pixelFormat == Format32bppArgb ||
                  pixelFormat == Format32bppPArgb))
            {
                throw new ArgumentException("Only 8bpp, 24bpp and 32bpp images are supported.");
            }

            this.bitmap = (Bitmap)bitmap;
            width = this.bitmap.Width;
            height = this.bitmap.Height;

            channel = pixelFormat == Format8bppIndexed ? 0 : 2;
            this.computeIntegrals = computeIntegrals;
            this.computeTilted = computeTilted;

            LockBitmap();
        }

        /// <summary>
        /// Gets the width, in pixels of the <see cref="System.Drawing.Bitmap"/>.
        /// </summary>
        public int Width => width;

        /// <summary>
        /// Gets the height, in pixels of the <see cref="System.Drawing.Bitmap"/>.
        /// </summary>
        public int Height => height;

        /// <summary>
        /// Gets the Integral Image for values' sum.
        /// </summary>
        public long[,] NormalImage => normalSumImage;

        /// <summary>
        /// Gets the Integral Image for values' squared sum.
        /// </summary>
        public long[,] SquaredImage => squaredSumImage;

        /// <summary>
        /// Gets the Integral Image for tilted values' sum.
        /// </summary>
        public long[,] TiltedImage => tiltedSumImage;

        /// <summary>
        /// Gets the pixel data for the given position.
        /// </summary>
        /// <param name="x">
        /// The x position of the pixel.
        /// </param>
        /// <param name="y">
        /// The y position of the pixel.
        /// </param>
        /// <returns>
        /// The <see cref="Color32"/>.
        /// </returns>
        private Color32* this[int x, int y] => (Color32*)(pixelBase + (y * bytesInARow) + (x * 4));

        /// <summary>
        /// Allows the implicit conversion of an instance of <see cref="FastBitmap"/> to a 
        /// <see cref="System.Drawing.Image"/>.
        /// </summary>
        /// <param name="fastBitmap">
        /// The instance of <see cref="FastBitmap"/> to convert.
        /// </param>
        /// <returns>
        /// An instance of <see cref="System.Drawing.Image"/>.
        /// </returns>
        public static implicit operator Image(FastBitmap fastBitmap)
        {
            return fastBitmap.bitmap;
        }

        /// <summary>
        /// Allows the implicit conversion of an instance of <see cref="FastBitmap"/> to a 
        /// <see cref="System.Drawing.Bitmap"/>.
        /// </summary>
        /// <param name="fastBitmap">
        /// The instance of <see cref="FastBitmap"/> to convert.
        /// </param>
        /// <returns>
        /// An instance of <see cref="System.Drawing.Bitmap"/>.
        /// </returns>
        public static implicit operator Bitmap(FastBitmap fastBitmap)
        {
            return fastBitmap.bitmap;
        }

        /// <summary>
        /// Gets the color at the specified pixel of the <see cref="System.Drawing.Bitmap"/>.
        /// </summary>
        /// <param name="x">The x-coordinate of the pixel to retrieve.</param>
        /// <param name="y">The y-coordinate of the pixel to retrieve.</param>
        /// <returns>The <see cref="System.Drawing.Color"/> at the given pixel.</returns>
        public Color GetPixel(int x, int y)
        {
#if DEBUG
            if ((x < 0) || (x >= width))
            {
                throw new ArgumentOutOfRangeException("x", "Value cannot be less than zero or greater than the bitmap width.");
            }

            if ((y < 0) || (y >= height))
            {
                throw new ArgumentOutOfRangeException("y", "Value cannot be less than zero or greater than the bitmap height.");
            }
#endif
            var data = this[x, y];
            return Color.FromArgb(data->A, data->R, data->G, data->B);
        }

        /// <summary>
        /// Sets the color of the specified pixel of the <see cref="System.Drawing.Bitmap"/>.
        /// </summary>
        /// <param name="x">The x-coordinate of the pixel to set.</param>
        /// <param name="y">The y-coordinate of the pixel to set.</param>
        /// <param name="color">
        /// A <see cref="System.Drawing.Color"/> color structure that represents the 
        /// color to set the specified pixel.
        /// </param>
        public void SetPixel(int x, int y, Color color)
        {
#if DEBUG
            if ((x < 0) || (x >= width))
            {
                throw new ArgumentOutOfRangeException("x", "Value cannot be less than zero or greater than the bitmap width.");
            }

            if ((y < 0) || (y >= height))
            {
                throw new ArgumentOutOfRangeException("y", "Value cannot be less than zero or greater than the bitmap height.");
            }
#endif
            var data = this[x, y];
            data->Argb = color.ToArgb();
        }

        /// <summary>
        /// Gets the sum of the pixels in a rectangle of the Integral image.
        /// </summary>
        /// <param name="x">The horizontal position of the rectangle <c>x</c>.</param>
        /// <param name="y">The vertical position of the rectangle <c>y</c>.</param>
        /// <param name="rectangleWidth">The rectangle's width <c>w</c>.</param>
        /// <param name="rectangleHeight">The rectangle's height <c>h</c>.</param>
        /// <returns>
        /// The sum of all pixels contained in the rectangle, computed
        /// as I[y, x] + I[y + h, x + w] - I[y + h, x] - I[y, x + w].
        /// </returns>
        public long GetSum(int x, int y, int rectangleWidth, int rectangleHeight)
        {
            var a = (normalWidth * y) + x;
            var b = (normalWidth * (y + rectangleHeight)) + (x + rectangleWidth);
            var c = (normalWidth * (y + rectangleHeight)) + x;
            var d = (normalWidth * y) + (x + rectangleWidth);

            return normalSum[a] + normalSum[b] - normalSum[c] - normalSum[d];
        }

        /// <summary>
        /// Gets the sum of the squared pixels in a rectangle of the Integral image.
        /// </summary>
        /// <param name="x">The horizontal position of the rectangle <c>x</c>.</param>
        /// <param name="y">The vertical position of the rectangle <c>y</c>.</param>
        /// <param name="rectangleWidth">The rectangle's width <c>w</c>.</param>
        /// <param name="rectangleHeight">The rectangle's height <c>h</c>.</param>
        /// <returns>
        /// The sum of all pixels contained in the rectangle, computed
        /// as I²[y, x] + I²[y + h, x + w] - I²[y + h, x] - I²[y, x + w].
        /// </returns>
        public long GetSum2(int x, int y, int rectangleWidth, int rectangleHeight)
        {
            var a = (normalWidth * y) + x;
            var b = (normalWidth * (y + rectangleHeight)) + (x + rectangleWidth);
            var c = (normalWidth * (y + rectangleHeight)) + x;
            var d = (normalWidth * y) + (x + rectangleWidth);

            return squaredSum[a] + squaredSum[b] - squaredSum[c] - squaredSum[d];
        }

        /// <summary>
        /// Gets the sum of the pixels in a tilted rectangle of the Integral image.
        /// </summary>
        /// <param name="x">The horizontal position of the rectangle <c>x</c>.</param>
        /// <param name="y">The vertical position of the rectangle <c>y</c>.</param>
        /// <param name="rectangleWidth">The rectangle's width <c>w</c>.</param>
        /// <param name="rectangleHeight">The rectangle's height <c>h</c>.</param>
        /// <returns>
        /// The sum of all pixels contained in the rectangle, computed
        /// as T[y + w, x + w + 1] + T[y + h, x - h + 1] - T[y, x + 1] - T[y + w + h, x + w - h + 1].
        /// </returns>
        public long GetSumT(int x, int y, int rectangleWidth, int rectangleHeight)
        {
            var a = (tiltedWidth * (y + rectangleWidth)) + (x + rectangleWidth + 1);
            var b = (tiltedWidth * (y + rectangleHeight)) + (x - rectangleHeight + 1);
            var c = (tiltedWidth * y) + (x + 1);
            var d = (tiltedWidth * (y + rectangleWidth + rectangleHeight)) + (x + rectangleWidth - rectangleHeight + 1);

            return tiltedSum[a] + tiltedSum[b] - tiltedSum[c] - tiltedSum[d];
        }

        /// <summary>
        /// Disposes the object and frees resources for the Garbage Collector.
        /// </summary>
        public void Dispose()
        {
            Dispose(true);

            // This object will be cleaned up by the Dispose method.
            // Therefore, you should call GC.SuppressFinalize to
            // take this object off the finalization queue 
            // and prevent finalization code for this object
            // from executing a second time.
            GC.SuppressFinalize(this);
        }

        /// <summary>
        /// Determines whether the specified <see cref="T:System.Object"/> is equal to the current <see cref="T:System.Object"/>.
        /// </summary>
        /// <returns>
        /// true if the specified object  is equal to the current object; otherwise, false.
        /// </returns>
        /// <param name="obj">The object to compare with the current object. </param>
        public override bool Equals(object obj)
        {
            var fastBitmap = obj as FastBitmap;

            if (fastBitmap == null)
            {
                return false;
            }

            return bitmap == fastBitmap.bitmap;
        }

        /// <summary>
        /// Serves as a hash function for a particular type. 
        /// </summary>
        /// <returns>
        /// A hash code for the current <see cref="T:System.Object"/>.
        /// </returns>
        public override int GetHashCode()
        {
            return bitmap.GetHashCode();
        }

        /// <summary>
        /// Disposes the object and frees resources for the Garbage Collector.
        /// </summary>
        /// <param name="disposing">If true, the object gets disposed.</param>
        protected virtual void Dispose(bool disposing)
        {
            if (isDisposed)
            {
                return;
            }

            if (disposing)
            {
                // Dispose of any managed resources here.
                UnlockBitmap();
            }

            // Call the appropriate methods to clean up
            // unmanaged resources here.
            if (normalSumHandle.IsAllocated)
            {
                normalSumHandle.Free();
                normalSum = null;
            }

            if (squaredSumHandle.IsAllocated)
            {
                squaredSumHandle.Free();
                squaredSum = null;
            }

            if (tiltedSumHandle.IsAllocated)
            {
                tiltedSumHandle.Free();
                tiltedSum = null;
            }

            // Note disposing is done.
            isDisposed = true;
        }

        /// <summary>
        /// Locks the bitmap into system memory.
        /// </summary>
        private void LockBitmap()
        {
            var bounds = new Rectangle(Point.Empty, bitmap.Size);

            // Figure out the number of bytes in a row. This is rounded up to be a multiple
            // of 4 bytes, since a scan line in an image must always be a multiple of 4 bytes
            // in length.
            pixelSize = Image.GetPixelFormatSize(bitmap.PixelFormat) / 8;
            bytesInARow = bounds.Width * pixelSize;
            if (bytesInARow % 4 != 0)
            {
                bytesInARow = 4 * ((bytesInARow / 4) + 1);
            }

            // Lock the bitmap
            bitmapData = bitmap.LockBits(bounds, ImageLockMode.ReadWrite, bitmap.PixelFormat);

            // Set the value to the first scan line
            pixelBase = (byte*)bitmapData.Scan0.ToPointer();

            if (computeIntegrals)
            {
                // Allocate values for integral image calculation.
                normalWidth = width + 1;
                var normalHeight = height + 1;

                tiltedWidth = width + 2;
                var tiltedHeight = height + 2;

                normalSumImage = new long[normalHeight, normalWidth];
                normalSumHandle = GCHandle.Alloc(normalSumImage, GCHandleType.Pinned);
                normalSum = (long*)normalSumHandle.AddrOfPinnedObject().ToPointer();

                squaredSumImage = new long[normalHeight, normalWidth];
                squaredSumHandle = GCHandle.Alloc(squaredSumImage, GCHandleType.Pinned);
                squaredSum = (long*)squaredSumHandle.AddrOfPinnedObject().ToPointer();

                if (computeTilted)
                {
                    tiltedSumImage = new long[tiltedHeight, tiltedWidth];
                    tiltedSumHandle = GCHandle.Alloc(tiltedSumImage, GCHandleType.Pinned);
                    tiltedSum = (long*)tiltedSumHandle.AddrOfPinnedObject().ToPointer();
                }

                CalculateIntegrals();
            }
        }

        /// <summary>
        /// Computes all possible rectangular areas in the image.
        /// </summary>
        private void CalculateIntegrals()
        {
            // Calculate integral and integral squared values.
            var stride = bitmapData.Stride;
            var offset = stride - bytesInARow;
            var srcStart = pixelBase + channel;

            // Do the job
            var src = srcStart;

            // For each line
            for (var y = 1; y <= height; y++)
            {
                var yy = normalWidth * y;
                var y1 = normalWidth * (y - 1);

                // For each pixel
                for (var x = 1; x <= width; x++, src += pixelSize)
                {
                    int pixel = *src;
                    var pixelSquared = pixel * pixel;

                    var r = yy + x;
                    var a = yy + (x - 1);
                    var b = y1 + x;
                    var g = y1 + (x - 1);

                    normalSum[r] = pixel + normalSum[a] + normalSum[b] - normalSum[g];
                    squaredSum[r] = pixelSquared + squaredSum[a] + squaredSum[b] - squaredSum[g];
                }

                src += offset;
            }

            if (computeTilted)
            {
                src = srcStart;

                // Left-to-right, top-to-bottom pass
                for (var y = 1; y <= height; y++, src += offset)
                {
                    var yy = tiltedWidth * y;
                    var y1 = tiltedWidth * (y - 1);

                    for (var x = 2; x < width + 2; x++, src += pixelSize)
                    {
                        var a = y1 + (x - 1);
                        var b = yy + (x - 1);
                        var g = y1 + (x - 2);
                        var r = yy + x;

                        tiltedSum[r] = *src + tiltedSum[a] + tiltedSum[b] - tiltedSum[g];
                    }
                }

                {
                    var yy = tiltedWidth * height;
                    var y1 = tiltedWidth * (height + 1);

                    for (var x = 2; x < width + 2; x++, src += pixelSize)
                    {
                        var a = yy + (x - 1);
                        var c = yy + (x - 2);
                        var b = y1 + (x - 1);
                        var r = y1 + x;

                        tiltedSum[r] = tiltedSum[a] + tiltedSum[b] - tiltedSum[c];
                    }
                }

                // Right-to-left, bottom-to-top pass
                for (var y = height; y >= 0; y--)
                {
                    var yy = tiltedWidth * y;
                    var y1 = tiltedWidth * (y + 1);

                    for (var x = width + 1; x >= 1; x--)
                    {
                        var r = yy + x;
                        var b = y1 + (x - 1);

                        tiltedSum[r] += tiltedSum[b];
                    }
                }

                for (var y = height + 1; y >= 0; y--)
                {
                    var yy = tiltedWidth * y;

                    for (var x = width + 1; x >= 2; x--)
                    {
                        var r = yy + x;
                        var b = yy + (x - 2);

                        tiltedSum[r] -= tiltedSum[b];
                    }
                }
            }
        }

        /// <summary>
        /// Unlocks the bitmap from system memory.
        /// </summary>
        private void UnlockBitmap()
        {
            // Copy the RGB values back to the bitmap and unlock the bitmap.
            bitmap.UnlockBits(bitmapData);
            bitmapData = null;
            pixelBase = null;
        }
    }
}