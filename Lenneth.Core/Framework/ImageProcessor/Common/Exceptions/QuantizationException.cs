﻿// --------------------------------------------------------------------------------------------------------------------
// <copyright file="QuantizationException.cs" company="James Jackson-South">
//   Copyright (c) James Jackson-South.
//   Licensed under the Apache License, Version 2.0.
// </copyright>
// <summary>
//   The exception that is thrown when quantizing an image has failed.
// </summary>
// --------------------------------------------------------------------------------------------------------------------

using System;

namespace Lenneth.Core.Framework.ImageProcessor.Common.Exceptions
{
    /// <summary>
    /// The exception that is thrown when quantizing an image has failed.
    /// </summary>
    [Serializable]
    public class QuantizationException : Exception
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="QuantizationException"/> class.
        /// </summary>
        /// <param name="message">
        /// The message.
        /// </param>
        public QuantizationException(string message)
            : base(message)
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="QuantizationException"/> class.
        /// </summary>
        /// <param name="message">The error message that explains the reason for the exception.</param>
        /// <param name="innerException">The exception that is the cause of the current exception, or a null reference (Nothing in Visual Basic) if no inner exception is specified.</param>
        public QuantizationException(string message, Exception innerException)
            : base(message, innerException)
        {
        }
    }
}
