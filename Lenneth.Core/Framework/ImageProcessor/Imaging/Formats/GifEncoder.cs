// --------------------------------------------------------------------------------------------------------------------
// <copyright file="GifEncoder.cs" company="James Jackson-South">
//   Copyright (c) James Jackson-South.
//   Licensed under the Apache License, Version 2.0.
// </copyright>
// <summary>
//   Encodes multiple images as an animated gif to a stream.
//   <remarks>
//   Always wire this up in a using block.
//   Disposing the encoder will complete the file.
//   Uses default .NET GIF encoding and adds animation headers.
//   Adapted from <see href="http://github.com/DataDink/Bumpkit/blob/master/BumpKit/BumpKit/GifEncoder.cs"/>
//   </remarks>
// </summary>
// --------------------------------------------------------------------------------------------------------------------

using System;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;
using System.Linq;

namespace Lenneth.Core.Framework.ImageProcessor.Imaging.Formats
{
    /// <summary>
    /// Encodes multiple images as an animated gif to a stream.
    /// <remarks>
    /// Uses default .NET GIF encoding and adds animation headers.
    /// Adapted from <see href="http://github.com/DataDink/Bumpkit/blob/master/BumpKit/BumpKit/GifEncoder.cs"/>
    /// </remarks>
    /// </summary>
    public class GifEncoder : IDisposable
    {
        #region Constants
        /// <summary>
        /// The application block size.
        /// </summary>
        private const byte ApplicationBlockSize = 0x0b;

        /// <summary>
        /// The application extension block identifier.
        /// </summary>
        private const int ApplicationExtensionBlockIdentifier = 0xff21;

        /// <summary>
        /// The application identification.
        /// </summary>
        private const string ApplicationIdentification = "NETSCAPE2.0";

        /// <summary>
        /// The file trailer.
        /// </summary>
        private const byte FileTrailer = 0x3b;

        /// <summary>
        /// The file type.
        /// </summary>
        private const string FileType = "GIF";

        /// <summary>
        /// The file version.
        /// </summary>
        private const string FileVersion = "89a";

        /// <summary>
        /// The graphic control extension block identifier.
        /// </summary>
        private const int GraphicControlExtensionBlockIdentifier = 0xf921;

        /// <summary>
        /// The graphic control extension block size.
        /// </summary>
        private const byte GraphicControlExtensionBlockSize = 0x04;

        /// <summary>
        /// The source color block length.
        /// </summary>
        private const long SourceColorBlockLength = 768;

        /// <summary>
        /// The source color block position.
        /// </summary>
        private const long SourceColorBlockPosition = 13;

        /// <summary>
        /// The source global color info position.
        /// </summary>
        private const long SourceGlobalColorInfoPosition = 10;

        /// <summary>
        /// The source graphic control extension length.
        /// </summary>
        private const long SourceGraphicControlExtensionLength = 8;

        /// <summary>
        /// The source graphic control extension position.
        /// </summary>
        private const long SourceGraphicControlExtensionPosition = 781;

        /// <summary>
        /// The source image block header length.
        /// </summary>
        private const long SourceImageBlockHeaderLength = 11;

        /// <summary>
        /// The source image block position.
        /// </summary>
        private const long SourceImageBlockPosition = 789;
        #endregion

        #region Fields
        /// <summary>
        /// The converter for creating the output image from a byte array.
        /// </summary>
        private static readonly ImageConverter Converter = new ImageConverter();

        /// <summary>
        /// The stream.
        /// </summary>
        // ReSharper disable once FieldCanBeMadeReadOnly.Local
        private MemoryStream imageStream;

        /// <summary>
        /// The height.
        /// </summary>
        private int? height;

        /// <summary>
        /// The is first image.
        /// </summary>
        private bool isFirstImage = true;

        /// <summary>
        /// The repeat count.
        /// </summary>
        private int? repeatCount;

        /// <summary>
        /// The width.
        /// </summary>
        private int? width;

        /// <summary>
        /// Whether the gif has has the last terminated byte written.
        /// </summary>
        private bool terminated;
        #endregion

        #region Constructors
        /// <summary>
        /// Initializes a new instance of the <see cref="GifEncoder"/> class.
        /// </summary>
        /// <param name="width">
        /// Sets the width for this gif or null to use the first frame's width.
        /// </param>
        /// <param name="height">
        /// Sets the height for this gif or null to use the first frame's height.
        /// </param>
        /// <param name="repeatCount">
        /// The number of times to repeat the animation.
        /// </param>
        public GifEncoder(int? width = null, int? height = null, int? repeatCount = null)
        {
            imageStream = new MemoryStream();
            this.width = width;
            this.height = height;
            this.repeatCount = repeatCount;
        }
        #endregion

        /// <summary>
        /// Gets or sets the image bytes.
        /// </summary>
        public byte[] ImageBytes { get; set; }

        #region Public Methods and Operators
        /// <summary>
        /// Adds a frame to the gif.
        /// </summary>
        /// <param name="frame">
        /// The <see cref="GifFrame"/> containing the image.
        /// </param>
        public void AddFrame(GifFrame frame)
        {
            using (var gifStream = new MemoryStream())
            {
                frame.Image.Save(gifStream, ImageFormat.Gif);
                if (isFirstImage)
                {
                    // Steal the global color table info
                    WriteHeaderBlock(gifStream, frame.Image.Width, frame.Image.Height);
                }

                WriteGraphicControlBlock(gifStream, Convert.ToInt32(frame.Delay.TotalMilliseconds / 10F));
                WriteImageBlock(gifStream, !isFirstImage, frame.X, frame.Y, frame.Image.Width, frame.Image.Height);
            }

            isFirstImage = false;
        }

        /// <summary>
        /// Saves the completed gif to an <see cref="Image"/>
        /// </summary>
        /// <returns>The completed animated gif.</returns>
        public Image Save()
        {
            if (!terminated)
            {
                // Complete File
                WriteByte(FileTrailer);
                terminated = true;
            }

            // Push the data
            imageStream.Flush();
            imageStream.Position = 0;
            ImageBytes = imageStream.ToArray();
            imageStream.Dispose();
            return (Image)Converter.ConvertFrom(ImageBytes);
        }

        /// <summary>
        /// Saves the completed gif to an <see cref="Stream"/>
        /// </summary>
        /// <param name="stream">The stream.</param>
        public void Save(Stream stream)
        {
            if (!terminated)
            {
                // Complete File
                WriteByte(FileTrailer);
                terminated = true;
            }

            if (stream.CanSeek)
            {
                stream.Position = 0;
            }

            // Push the data
            imageStream.Flush();
            imageStream.Position = 0;

            imageStream.CopyTo(stream);

            imageStream.Position = 0;
        }
        #endregion

        #region Methods
        /// <summary>
        /// Writes the header block of the animated gif to the stream.
        /// </summary>
        /// <param name="sourceGif">
        /// The source gif.
        /// </param>
        /// <param name="w">
        /// The width of the image.
        /// </param>
        /// <param name="h">
        /// The height of the image.
        /// </param>
        private void WriteHeaderBlock(Stream sourceGif, int w, int h)
        {
            // File Header signature and version.
            WriteString(FileType);
            WriteString(FileVersion);

            // Write the logical screen descriptor.
            WriteShort(width.GetValueOrDefault(w)); // Initial Logical Width
            WriteShort(height.GetValueOrDefault(h)); // Initial Logical Height

            // Read the global color table info.
            sourceGif.Position = SourceGlobalColorInfoPosition;
            WriteByte(sourceGif.ReadByte());

            WriteByte(255); // Background Color Index
            WriteByte(0); // Pixel aspect ratio
            WriteColorTable(sourceGif);

            // Application Extension Header
            var count = repeatCount.GetValueOrDefault(0);
            if (count != 1)
            {
                // 0 means loop indefinitely. count is set as play n + 1 times.
                count = Math.Max(0, count - 1);
                WriteShort(ApplicationExtensionBlockIdentifier);
                WriteByte(ApplicationBlockSize);

                WriteString(ApplicationIdentification);
                WriteByte(3); // Application block length
                WriteByte(1);
                WriteShort(count); // Repeat count for images.

                WriteByte(0); // Terminator
            }
        }

        /// <summary>
        /// Writes the given integer as a byte to the stream.
        /// </summary>
        /// <param name="value">
        /// The value.
        /// </param>
        private void WriteByte(int value)
        {
            imageStream.WriteByte(Convert.ToByte(value));
        }

        /// <summary>
        /// Writes the color table.
        /// </summary>
        /// <param name="sourceGif">
        /// The source gif.
        /// </param>
        private void WriteColorTable(Stream sourceGif)
        {
            sourceGif.Position = SourceColorBlockPosition; // Locating the image color table
            var colorTable = new byte[SourceColorBlockLength];
            sourceGif.Read(colorTable, 0, colorTable.Length);
            imageStream.Write(colorTable, 0, colorTable.Length);
        }

        /// <summary>
        /// Writes graphic control block.
        /// </summary>
        /// <param name="sourceGif">The source gif.</param>
        /// <param name="frameDelay">The frame delay.</param>
        private void WriteGraphicControlBlock(Stream sourceGif, int frameDelay)
        {
            sourceGif.Position = SourceGraphicControlExtensionPosition; // Locating the source GCE
            var blockhead = new byte[SourceGraphicControlExtensionLength];
            sourceGif.Read(blockhead, 0, blockhead.Length); // Reading source GCE

            WriteShort(GraphicControlExtensionBlockIdentifier); // Identifier
            WriteByte(GraphicControlExtensionBlockSize); // Block Size
            WriteByte(blockhead[3] & 0xf7 | 0x08); // Setting disposal flag
            WriteShort(frameDelay); // Setting frame delay
            WriteByte(255); // Transparent color index
            WriteByte(0); // Terminator
        }

        /// <summary>
        /// Writes image block data.
        /// </summary>
        /// <param name="sourceGif">The source gif.</param>
        /// <param name="includeColorTable">The include color table.</param>
        /// <param name="x">The x position to write the image block.</param>
        /// <param name="y">The y position to write the image block.</param>
        /// <param name="h">The height of the image block.</param>
        /// <param name="w">
        /// The width of the image block.
        /// </param>
        private void WriteImageBlock(Stream sourceGif, bool includeColorTable, int x, int y, int h, int w)
        {
            // Local Image Descriptor
            sourceGif.Position = SourceImageBlockPosition; // Locating the image block
            var header = new byte[SourceImageBlockHeaderLength];
            sourceGif.Read(header, 0, header.Length);
            WriteByte(header[0]); // Separator
            WriteShort(x); // Position X
            WriteShort(y); // Position Y
            WriteShort(h); // Height
            WriteShort(w); // Width

            if (includeColorTable)
            {
                // If first frame, use global color table - else use local
                sourceGif.Position = SourceGlobalColorInfoPosition;
                WriteByte(sourceGif.ReadByte() & 0x3f | 0x80); // Enabling local color table
                WriteColorTable(sourceGif);
            }
            else
            {
                WriteByte(header[9] & 0x07 | 0x07); // Disabling local color table
            }

            WriteByte(header[10]); // LZW Min Code Size

            // Read/Write image data
            sourceGif.Position = SourceImageBlockPosition + SourceImageBlockHeaderLength;

            var dataLength = sourceGif.ReadByte();
            while (dataLength > 0)
            {
                var imgData = new byte[dataLength];
                sourceGif.Read(imgData, 0, dataLength);

                imageStream.WriteByte(Convert.ToByte(dataLength));
                imageStream.Write(imgData, 0, dataLength);
                dataLength = sourceGif.ReadByte();
            }

            imageStream.WriteByte(0); // Terminator
        }

        /// <summary>
        /// The write short.
        /// </summary>
        /// <param name="value">
        /// The value.
        /// </param>
        private void WriteShort(int value)
        {
            // Leave only one significant byte.
            imageStream.WriteByte(Convert.ToByte(value & 0xff));
            imageStream.WriteByte(Convert.ToByte((value >> 8) & 0xff));
        }

        /// <summary>
        /// The write string.
        /// </summary>
        /// <param name="value">
        /// The value.
        /// </param>
        private void WriteString(string value)
        {
            imageStream.Write(value.ToArray().Select(c => (byte)c).ToArray(), 0, value.Length);
        }
        #endregion

        private void ReleaseUnmanagedResources()
        {
            // TODO release unmanaged resources here
        }

        protected virtual void Dispose(bool disposing)
        {
            ReleaseUnmanagedResources();
            if (disposing)
            {
                imageStream?.Dispose();
            }
        }

        /// <summary>执行与释放或重置非托管资源关联的应用程序定义的任务。</summary>
        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        /// <summary>在垃圾回收将某一对象回收前允许该对象尝试释放资源并执行其他清理操作。</summary>
        ~GifEncoder()
        {
            Dispose(false);
        }
    }
}