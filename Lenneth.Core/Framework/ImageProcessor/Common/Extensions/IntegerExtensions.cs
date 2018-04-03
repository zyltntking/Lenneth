﻿// --------------------------------------------------------------------------------------------------------------------
// <copyright file="IntegerExtensions.cs" company="James Jackson-South">
//   Copyright (c) James Jackson-South.
//   Licensed under the Apache License, Version 2.0.
// </copyright>
// <summary>
//   Encapsulates a series of time saving extension methods to the <see cref="T:System.Int32" /> class.
// </summary>
// --------------------------------------------------------------------------------------------------------------------

using System;
using System.Globalization;
using Lenneth.Core.Framework.ImageProcessor.Imaging.Helpers;

namespace Lenneth.Core.Framework.ImageProcessor.Common.Extensions
{
    /// <summary>
    /// Encapsulates a series of time saving extension methods to the <see cref="T:System.Int32"/> class.
    /// </summary>
    public static class IntegerExtensions
    {
        /// <summary>
        /// Converts an <see cref="T:System.Int32"/> value into a valid <see cref="T:System.Byte"/>.
        /// <remarks>
        /// If the value given is less than 0 or greater than 255, the value will be constrained into
        /// those restricted ranges.
        /// </remarks>
        /// </summary>
        /// <param name="value">
        /// The <see cref="T:System.Int32"/> to convert.
        /// </param>
        /// <returns>
        /// The <see cref="T:System.Byte"/>.
        /// </returns>
        public static byte ToByte(this int value)
        {
            return Convert.ToByte(ImageMaths.Clamp(value, 0, 255));
        }

        /// <summary>
        /// Converts the string representation of a number in a specified culture-specific format to its 
        /// 32-bit signed integer equivalent using invariant culture.
        /// </summary>
        /// <param name="value">The integer.</param>
        /// <param name="toParse">A string containing a number to convert.</param>
        /// <returns>A 32-bit signed integer equivalent to the number specified in toParse.</returns>
        public static int ParseInvariant(this int value, string toParse)
        {
            return int.Parse(toParse, CultureInfo.InvariantCulture);
        }
    }
}
