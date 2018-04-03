// --------------------------------------------------------------------------------------------------------------------
// <copyright file="CubeCut.cs" company="James Jackson-South">
//   Copyright (c) James Jackson-South.
//   Licensed under the Apache License, Version 2.0.
// </copyright>
// <summary>
//   Represents a cube cut.
//   Adapted from <see href="https://github.com/drewnoakes" />
// </summary>
// --------------------------------------------------------------------------------------------------------------------

namespace Lenneth.Core.Framework.ImageProcessor.Imaging.Quantizers.WuQuantizer
{
    /// <summary>
    /// Represents a cube cut.
    /// Adapted from <see href="https://github.com/drewnoakes"/>
    /// </summary>
    internal struct CubeCut
    {
        /// <summary>
        /// The position.
        /// </summary>
        public readonly byte? Position;

        /// <summary>
        /// The value.
        /// </summary>
        public readonly float Value;

        /// <summary>
        /// Initializes a new instance of the <see cref="CubeCut"/> struct.
        /// </summary>
        /// <param name="cutPoint">
        /// The cut point.
        /// </param>
        /// <param name="result">
        /// The result.
        /// </param>
        public CubeCut(byte? cutPoint, float result)
        {
            Position = cutPoint;
            Value = result;
        }
    }
}