// --------------------------------------------------------------------------------------------------------------------
// <copyright file="PaletteColorHistory.cs" company="James Jackson-South">
//   Copyright (c) James Jackson-South.
//   Licensed under the Apache License, Version 2.0.
// </copyright>
// <summary>
//   The palette color history containing the sum of all pixel data.
//   Adapted from <see href="https://github.com/drewnoakes" />
// </summary>
// --------------------------------------------------------------------------------------------------------------------

using System.Drawing;
using Lenneth.Core.Framework.ImageProcessor.Imaging.Colors;

namespace Lenneth.Core.Framework.ImageProcessor.Imaging.Quantizers.WuQuantizer
{
    /// <summary>
    /// The palette color history containing the sum of all pixel data.
    /// Adapted from <see href="https://github.com/drewnoakes" />
    /// </summary>
    internal struct PaletteColorHistory
    {
        /// <summary>
        /// The alpha component.
        /// </summary>
        public ulong Alpha;

        /// <summary>
        /// The red component.
        /// </summary>
        public ulong Red;

        /// <summary>
        /// The green component.
        /// </summary>
        public ulong Green;

        /// <summary>
        /// The blue component.
        /// </summary>
        public ulong Blue;

        /// <summary>
        /// The sum of the color components.
        /// </summary>
        public ulong Sum;

        /// <summary>
        /// Normalizes the color.
        /// </summary>
        /// <returns>
        /// The normalized <see cref="Color"/>.
        /// </returns>
        public Color ToNormalizedColor()
        {
            return (Sum != 0) ? Color.FromArgb((int)(Alpha /= Sum), (int)(Red /= Sum), (int)(Green /= Sum), (int)(Blue /= Sum)) : Color.Empty;
        }

        /// <summary>
        /// Adds a pixel to the color history.
        /// </summary>
        /// <param name="pixel">
        /// The pixel to add.
        /// </param>
        public void AddPixel(Color32 pixel)
        {
            Alpha += pixel.A;
            Red += pixel.R;
            Green += pixel.G;
            Blue += pixel.B;
            Sum++;
        }
    }
}
