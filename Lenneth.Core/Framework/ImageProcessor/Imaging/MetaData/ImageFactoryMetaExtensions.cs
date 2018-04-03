﻿// --------------------------------------------------------------------------------------------------------------------
// <copyright file="ImageFactoryMetaExtensions.cs" company="James Jackson-South">
//   Copyright (c) James Jackson-South.
//   Licensed under the Apache License, Version 2.0.
// </copyright>
// <summary>
//   Extension methods for writing EXIF metadata to the image.
// </summary>
// --------------------------------------------------------------------------------------------------------------------

using Lenneth.Core.Framework.ImageProcessor.Imaging.Formats;

namespace Lenneth.Core.Framework.ImageProcessor.Imaging.MetaData
{
    /// <summary>
    /// Extension methods for writing EXIF metadata to the image.
    /// </summary>
    internal static class ImageFactoryMetaExtensions
    {
        /// <summary>
        /// The converter for converting inputs into byte arrays.
        /// </summary>
        private static readonly ExifBitConverter BitConverter = new ExifBitConverter(new ComputerArchitectureInfo());

        /// <summary>
        /// Sets a property item with the given id to the collection within the current
        /// <see cref="ImageFactory"/> instance.
        /// </summary>
        /// <param name="imageFactory">The image factory.</param>
        /// <param name="id">The id to assign to the property item.</param>
        /// <param name="value">The value to assign to the property item.</param>
        /// <returns>
        /// The <see cref="ImageFactory"/>.
        /// </returns>
        public static ImageFactory SetPropertyItem(this ImageFactory imageFactory, ExifPropertyTag id, byte value)
        {
            byte[] bytes = { value };
            return imageFactory.SetPropertyItem(id, ExifPropertyTagType.Byte, bytes.Length, bytes);
        }

        /// <summary>
        /// Sets a property item with the given id to the collection within the current
        /// <see cref="ImageFactory"/> instance.
        /// </summary>
        /// <param name="imageFactory">The image factory.</param>
        /// <param name="id">The id to assign to the property item.</param>
        /// <param name="value">The value to assign to the property item.</param>
        /// <returns>
        /// The <see cref="ImageFactory"/>.
        /// </returns>
        public static ImageFactory SetPropertyItem(this ImageFactory imageFactory, ExifPropertyTag id, string value)
        {
            // TODO: Cover the different encoding types for different tags.
            var bytes = BitConverter.GetBytes(value);
            return imageFactory.SetPropertyItem(id, ExifPropertyTagType.ASCII, bytes.Length, bytes);
        }

        /// <summary>
        /// Sets a property item with the given id to the collection within the current
        /// <see cref="ImageFactory"/> instance.
        /// </summary>
        /// <param name="imageFactory">The image factory.</param>
        /// <param name="id">The id to assign to the property item.</param>
        /// <param name="value">The value to assign to the property item.</param>
        /// <returns>
        /// The <see cref="ImageFactory"/>.
        /// </returns>
        public static ImageFactory SetPropertyItem(this ImageFactory imageFactory, ExifPropertyTag id, ushort value)
        {
            var bytes = BitConverter.GetBytes(value);
            return imageFactory.SetPropertyItem(id, ExifPropertyTagType.UShort, bytes.Length, bytes);
        }

        /// <summary>
        /// Sets a property item with the given id to the collection within the current
        /// <see cref="ImageFactory"/> instance.
        /// </summary>
        /// <param name="imageFactory">The image factory.</param>
        /// <param name="id">The id to assign to the property item.</param>
        /// <param name="value">The value to assign to the property item.</param>
        /// <returns>
        /// The <see cref="ImageFactory"/>.
        /// </returns>
        public static ImageFactory SetPropertyItem(this ImageFactory imageFactory, ExifPropertyTag id, uint value)
        {
            var bytes = BitConverter.GetBytes(value);
            return imageFactory.SetPropertyItem(id, ExifPropertyTagType.ULong, bytes.Length, bytes);
        }

        /// <summary>
        /// Sets a property item with the given id to the collection within the current
        /// <see cref="ImageFactory"/> instance.
        /// </summary>
        /// <param name="imageFactory">The image factory.</param>
        /// <param name="id">The id to assign to the property item.</param>
        /// <param name="value">The value to assign to the property item.</param>
        /// <returns>
        /// The <see cref="ImageFactory"/>.
        /// </returns>
        public static ImageFactory SetPropertyItem(this ImageFactory imageFactory, ExifPropertyTag id, Rational<uint> value)
        {
            var bytes = BitConverter.GetBytes(value);
            return imageFactory.SetPropertyItem(id, ExifPropertyTagType.Rational, bytes.Length, bytes);
        }

        /// <summary>
        /// Sets a property item with the given id to the collection within the current
        /// <see cref="ImageFactory"/> instance.
        /// </summary>
        /// <param name="imageFactory">The image factory.</param>
        /// <param name="id">The id to assign to the property item.</param>
        /// <param name="value">The value to assign to the property item.</param>
        /// <returns>
        /// The <see cref="ImageFactory"/>.
        /// </returns>
        public static ImageFactory SetPropertyItem(this ImageFactory imageFactory, ExifPropertyTag id, byte[] value)
        {
            return imageFactory.SetPropertyItem(id, ExifPropertyTagType.Undefined, value.Length, value);
        }

        /// <summary>
        /// Sets a property item with the given id to the collection within the current
        /// <see cref="ImageFactory"/> instance.
        /// </summary>
        /// <param name="imageFactory">The image factory.</param>
        /// <param name="id">The id to assign to the property item.</param>
        /// <param name="value">The value to assign to the property item.</param>
        /// <returns>
        /// The <see cref="ImageFactory"/>.
        /// </returns>
        public static ImageFactory SetPropertyItem(this ImageFactory imageFactory, ExifPropertyTag id, int value)
        {
            var bytes = BitConverter.GetBytes(value);
            return imageFactory.SetPropertyItem(id, ExifPropertyTagType.SLong, bytes.Length, bytes);
        }

        /// <summary>
        /// Sets a property item with the given id to the collection within the current
        /// <see cref="ImageFactory"/> instance.
        /// </summary>
        /// <param name="imageFactory">The image factory.</param>
        /// <param name="id">The id to assign to the property item.</param>
        /// <param name="value">The value to assign to the property item.</param>
        /// <returns>
        /// The <see cref="ImageFactory"/>.
        /// </returns>
        public static ImageFactory SetPropertyItem(this ImageFactory imageFactory, ExifPropertyTag id, Rational<int> value)
        {
            var bytes = BitConverter.GetBytes(value);
            return imageFactory.SetPropertyItem(id, ExifPropertyTagType.SRational, bytes.Length, bytes);
        }

        /// <summary>
        /// Sets a property item with the given id to the collection within the current
        /// <see cref="ImageFactory"/> instance.
        /// </summary>
        /// <param name="imageFactory">The image factory.</param>
        /// <param name="id">The id to assign to the property item.</param>
        /// <param name="type">The type to assign to the property item.</param>
        /// <param name="length">The length to assign to the property item.</param>
        /// <param name="value">The value to assign to the property item.</param>
        /// <returns>
        /// The <see cref="ImageFactory"/>.
        /// </returns>
        private static ImageFactory SetPropertyItem(
            this ImageFactory imageFactory,
            ExifPropertyTag id,
            ExifPropertyTagType type,
            int length,
            byte[] value)
        {
            var item = FormatUtilities.CreatePropertyItem();

            item.Id = (int)id;
            item.Type = (short)type;
            item.Len = length;
            item.Value = value;

            imageFactory.ExifPropertyItems[item.Id] = item;
            return imageFactory;
        }
    }
}
