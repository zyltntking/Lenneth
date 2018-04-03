// --------------------------------------------------------------------------------------------------------------------
// <copyright file="PaletteLookup.cs" company="James Jackson-South">
//   Copyright (c) James Jackson-South.
//   Licensed under the Apache License, Version 2.0.
// </copyright>
// <summary>
//   Stores the indexed color palette of an image for fast access.
//   Adapted from <see href="https://github.com/drewnoakes" />
// </summary>
// --------------------------------------------------------------------------------------------------------------------

using System;
using System.Collections.Generic;
using System.Linq;
using Lenneth.Core.Framework.ImageProcessor.Imaging.Colors;

namespace Lenneth.Core.Framework.ImageProcessor.Imaging.Quantizers.WuQuantizer
{
    /// <summary>
    /// Stores the indexed color palette of an image for fast access.
    /// Adapted from <see href="https://github.com/drewnoakes" />
    /// </summary>
    internal class PaletteLookup
    {
        /// <summary>
        /// The dictionary for caching lookup nodes.
        /// </summary>
        private Dictionary<int, LookupNode[]> lookupNodes;

        /// <summary>
        /// The palette mask.
        /// </summary>
        private int paletteMask;

        /// <summary>
        /// Initializes a new instance of the <see cref="PaletteLookup"/> class.
        /// </summary>
        /// <param name="palette">
        /// The palette.
        /// </param>
        public PaletteLookup(Color32[] palette)
        {
            Palette = new LookupNode[palette.Length];
            for (var paletteIndex = 0; paletteIndex < palette.Length; paletteIndex++)
            {
                Palette[paletteIndex] = new LookupNode
                {
                    Color32 = palette[paletteIndex],
                    PaletteIndex = (byte)paletteIndex
                };
            }

            BuildLookup(palette);
        }

        /// <summary>
        /// Gets the palette.
        /// </summary>
        private LookupNode[] Palette { get; }

        /// <summary>
        /// Gets palette index for the given pixel.
        /// </summary>
        /// <param name="pixel">
        /// The pixel to return the index for.
        /// </param>
        /// <returns>
        /// The <see cref="byte"/> representing the index.
        /// </returns>
        public byte GetPaletteIndex(Color32 pixel)
        {
            var pixelKey = pixel.Argb & paletteMask;
            LookupNode[] bucket;
            if (!lookupNodes.TryGetValue(pixelKey, out bucket))
            {
                bucket = Palette;
            }

            if (bucket.Length == 1)
            {
                return bucket[0].PaletteIndex;
            }

            var bestDistance = int.MaxValue;
            byte bestMatch = 0;
            foreach (var lookup in bucket)
            {
                var lookupPixel = lookup.Color32;

                var deltaAlpha = pixel.A - lookupPixel.A;
                var distance = deltaAlpha * deltaAlpha;

                var deltaRed = pixel.R - lookupPixel.R;
                distance += deltaRed * deltaRed;

                var deltaGreen = pixel.G - lookupPixel.G;
                distance += deltaGreen * deltaGreen;

                var deltaBlue = pixel.B - lookupPixel.B;
                distance += deltaBlue * deltaBlue;

                if (distance >= bestDistance)
                {
                    continue;
                }

                bestDistance = distance;
                bestMatch = lookup.PaletteIndex;
            }

            if ((bucket == Palette) && (pixelKey != 0))
            {
                lookupNodes[pixelKey] = new[] { bucket[bestMatch] };
            }

            return bestMatch;
        }

        /// <summary>
        /// Computes the bit mask.
        /// </summary>
        /// <param name="max">
        /// The maximum byte value.
        /// </param>
        /// <param name="bits">
        /// The number of bits.
        /// </param>
        /// <returns>
        /// The <see cref="byte"/>.
        /// </returns>
        private static byte ComputeBitMask(byte max, int bits)
        {
            byte mask = 0;

            if (bits != 0)
            {
                var highestSetBitIndex = HighestSetBitIndex(max);

                for (var i = 0; i < bits; i++)
                {
                    mask <<= 1;
                    mask++;
                }

                for (var i = 0; i <= highestSetBitIndex - bits; i++)
                {
                    mask <<= 1;
                }
            }

            return mask;
        }

        /// <summary>
        /// Gets the mask value from the palette.
        /// </summary>
        /// <param name="palette">
        /// The palette.
        /// </param>
        /// <returns>
        /// The <see cref="int"/> representing the component value of the mask.
        /// </returns>
        private static int GetMask(Color32[] palette)
        {
            IEnumerable<byte> alphas = palette.Select(p => p.A).ToArray();
            var maxAlpha = alphas.Max();
            var uniqueAlphas = alphas.Distinct().Count();

            IEnumerable<byte> reds = palette.Select(p => p.R).ToArray();
            var maxRed = reds.Max();
            var uniqueReds = reds.Distinct().Count();

            IEnumerable<byte> greens = palette.Select(p => p.G).ToArray();
            var maxGreen = greens.Max();
            var uniqueGreens = greens.Distinct().Count();

            IEnumerable<byte> blues = palette.Select(p => p.B).ToArray();
            var maxBlue = blues.Max();
            var uniqueBlues = blues.Distinct().Count();

            double totalUniques = uniqueAlphas + uniqueReds + uniqueGreens + uniqueBlues;

            var availableBits = 1.0 + Math.Log(uniqueAlphas * uniqueReds * uniqueGreens * uniqueBlues);

            var alphaMask = ComputeBitMask(maxAlpha, Convert.ToInt32(Math.Round(uniqueAlphas / totalUniques * availableBits)));
            var redMask = ComputeBitMask(maxRed, Convert.ToInt32(Math.Round(uniqueReds / totalUniques * availableBits)));
            var greenMask = ComputeBitMask(maxGreen, Convert.ToInt32(Math.Round(uniqueGreens / totalUniques * availableBits)));
            var blueMask = ComputeBitMask(maxBlue, Convert.ToInt32(Math.Round(uniqueBlues / totalUniques * availableBits)));

            var maskedPixel = new Color32(alphaMask, redMask, greenMask, blueMask);
            return maskedPixel.Argb;
        }

        /// <summary>
        /// Gets the highest set bit index.
        /// </summary>
        /// <param name="value">
        /// The value.
        /// </param>
        /// <returns>
        /// The <see cref="byte"/>.
        /// </returns>
        private static byte HighestSetBitIndex(byte value)
        {
            byte index = 0;
            for (var i = 0; i < 8; i++)
            {
                if (0 != (value & 1))
                {
                    index = (byte)i;
                }

                value >>= 1;
            }

            return index;
        }

        /// <summary>
        /// The build lookup.
        /// </summary>
        /// <param name="palette">
        /// The palette.
        /// </param>
        private void BuildLookup(Color32[] palette)
        {
            var mask = GetMask(palette);
            var tempLookup = new Dictionary<int, List<LookupNode>>();
            foreach (var lookup in Palette)
            {
                var pixelKey = lookup.Color32.Argb & mask;

                List<LookupNode> bucket;
                if (!tempLookup.TryGetValue(pixelKey, out bucket))
                {
                    bucket = new List<LookupNode>();
                    tempLookup[pixelKey] = bucket;
                }

                bucket.Add(lookup);
            }

            lookupNodes = new Dictionary<int, LookupNode[]>(tempLookup.Count);
            foreach (var key in tempLookup.Keys)
            {
                lookupNodes[key] = tempLookup[key].ToArray();
            }

            paletteMask = mask;
        }

        /// <summary>
        /// Represents a single node containing the index and pixel.
        /// </summary>
        private struct LookupNode
        {
            /// <summary>
            /// The palette index.
            /// </summary>
            public byte PaletteIndex;

            /// <summary>
            /// The pixel.
            /// </summary>
            public Color32 Color32;
        }
    }
}