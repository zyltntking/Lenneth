// --------------------------------------------------------------------------------------------------------------------
// <copyright file="Histogram.cs" company="James Jackson-South">
//   Copyright (c) James Jackson-South.
//   Licensed under the Apache License, Version 2.0.
// </copyright>
// <summary>
//   The histogram representing the distribution of color data.
//   Adapted from <see href="https://github.com/drewnoakes" />
// </summary>
// --------------------------------------------------------------------------------------------------------------------

using System;

namespace Lenneth.Core.Framework.ImageProcessor.Imaging.Quantizers.WuQuantizer
{
    /// <summary>
    /// The histogram representing the distribution of color data.
    /// Adapted from <see href="https://github.com/drewnoakes" />
    /// </summary>
    public class Histogram
    {
        /// <summary>
        /// The moments.
        /// </summary>
        internal readonly ColorMoment[,,,] Moments;

        /// <summary>
        /// The side size.
        /// </summary>
        private const int SideSize = 33;

        /// <summary>
        /// Initializes a new instance of the <see cref="Histogram"/> class.
        /// </summary>
        public Histogram()
        {
            // 47,436,840 bytes
            Moments = new ColorMoment[SideSize, SideSize, SideSize, SideSize];
        }

        /// <summary>
        /// The clear.
        /// </summary>
        internal void Clear()
        {
            Array.Clear(Moments, 0, SideSize * SideSize * SideSize * SideSize);
        }
    }
}