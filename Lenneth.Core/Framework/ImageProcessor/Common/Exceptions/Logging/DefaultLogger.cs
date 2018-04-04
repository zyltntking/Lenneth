// --------------------------------------------------------------------------------------------------------------------
// <copyright file="DefaultLogger.cs" company="James Jackson-South">
//   Copyright (c) James Jackson-South.
//   Licensed under the Apache License, Version 2.0.
// </copyright>
// <summary>
//   The default logger which logs messages to the trace listeners.
// </summary>
// --------------------------------------------------------------------------------------------------------------------

using System;
using System.Diagnostics;
using System.Runtime.CompilerServices;

namespace Lenneth.Core.Framework.ImageProcessor.Common.Exceptions.Logging
{
    /// <summary>
    /// The default logger which logs messages to the trace listeners.
    /// </summary>
    /// <seealso cref="ILogger" />
    public class DefaultLogger : ILogger
    {
        /// <summary>
        /// Logs the specified message.
        /// </summary>
        /// <typeparam name="T">The type calling the logger.</typeparam>
        /// <param name="text">The message to log.</param>
        /// <param name="callerName">The property or method name calling the log.</param>
        /// <param name="lineNumber">The line number where the method is called.</param>
        public void Log<T>(string text, [CallerMemberName] string callerName = null, [CallerLineNumber] int lineNumber = 0) => LogInternal(typeof(T), text, callerName, lineNumber);

        /// <summary>
        /// Logs the specified message.
        /// </summary>
        /// <param name="type">The type calling the logger.</param>
        /// <param name="text">The message to log.</param>
        /// <param name="callerName">The property or method name calling the log.</param>
        /// <param name="lineNumber">The line number where the method is called.</param>
        public void Log(Type type, string text, [CallerMemberName] string callerName = null, [CallerLineNumber] int lineNumber = 0) => LogInternal(type, text, callerName, lineNumber);

        /// <summary>
		/// Logs the specified message.
		/// </summary>
		/// <param name="type">The type calling the logger.</param>
		/// <param name="text">The message to log.</param>
		/// <param name="callerName">The property or method name calling the log.</param>
		/// <param name="lineNumber">The line number where the method is called.</param>
		[Conditional("TRACE")]
		private void LogInternal(Type type, string text, string callerName = null, int lineNumber = 0)
        {
            var message = $"{DateTime.UtcNow.ToString("s")} - {type.Name}: {callerName} {lineNumber}:{text}";

            Trace.WriteLine(message);
        }
    }
}