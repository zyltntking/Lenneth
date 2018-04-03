// --------------------------------------------------------------------------------------------------------------------
// <copyright file="IOHelper.cs" company="James Jackson-South">
//   Copyright (c) James Jackson-South.
//   Licensed under the Apache License, Version 2.0.
// </copyright>
// <summary>
//   Provides helper method for traversing the file system.
//   <remarks>
//   Adapted from identically named class within <see href="https://github.com/umbraco/Umbraco-CMS" />
//   </remarks>
// </summary>
// --------------------------------------------------------------------------------------------------------------------

using System;
using System.Globalization;
using System.IO;
using System.Reflection;
using Lenneth.Core.Framework.ImageProcessor.Common.Extensions;

namespace Lenneth.Core.Framework.ImageProcessor.Common.Helpers
{
    /// <summary>
    /// Provides helper method for traversing the file system.
    /// <remarks>
    /// Adapted from identically named class within <see href="https://github.com/umbraco/Umbraco-CMS"/>
    /// </remarks>
    /// </summary>
    internal class IOHelper
    {
        /// <summary>
        /// The root directory.
        /// </summary>
        private static string _rootDirectory;

        /// <summary>
        /// Maps a virtual path to a physical path.
        /// </summary>
        /// <param name="virtualPath">
        /// The virtual path to map.
        /// </param>
        /// <returns>
        /// The <see cref="string"/> representing the physical path.
        /// </returns>
        public static string MapPath(string virtualPath)
        {
            // Check if the path is already mapped
            // UNC Paths start with "\\". If the site is running off a network drive mapped paths 
            // will look like "\\Whatever\Boo\Bar"
            if ((virtualPath.Length >= 2 && virtualPath[1] == Path.VolumeSeparatorChar)
                || virtualPath.StartsWith(@"\\"))
            {
                return virtualPath;
            }

            var separator = Path.DirectorySeparatorChar;
            var root = GetRootDirectorySafe();
            var newPath = virtualPath.TrimStart('~', '/').Replace('/', separator);
            return root + separator.ToString(CultureInfo.InvariantCulture) + newPath;
        }

        /// <summary>
        /// Gets the root directory bin folder for the currently running application.
        /// </summary>
        /// <returns>
        /// The <see cref="string"/> representing the root directory bin folder.
        /// </returns>
        public static string GetRootDirectoryBinFolder()
        {
            var binFolder = string.Empty;
            if (string.IsNullOrEmpty(_rootDirectory))
            {
                var directoryInfo = Assembly.GetExecutingAssembly().GetAssemblyFile().Directory;
                if (directoryInfo != null)
                {
                    binFolder = directoryInfo.FullName;
                }

                return binFolder;
            }

            binFolder = Path.Combine(GetRootDirectorySafe(), "bin");

#if DEBUG
            var debugFolder = Path.Combine(binFolder, "debug");
            if (Directory.Exists(debugFolder))
            {
                return debugFolder;
            }
#endif
            var releaseFolder = Path.Combine(binFolder, "release");
            if (Directory.Exists(releaseFolder))
            {
                return releaseFolder;
            }

            if (Directory.Exists(binFolder))
            {
                return binFolder;
            }

            return _rootDirectory;
        }

        /// <summary>
        /// Returns the path to the root of the application, by getting the path to where the assembly where this
        /// method is included is present, then traversing until it's past the /bin directory. I.e. this makes it work
        /// even if the assembly is in a /bin/debug or /bin/release folder
        /// </summary>
        /// <returns>
        /// The <see cref="string"/> representing the root path of the currently running application.</returns>
        internal static string GetRootDirectorySafe()
        {
            if (string.IsNullOrEmpty(_rootDirectory) == false)
            {
                return _rootDirectory;
            }

            var codeBase = Assembly.GetExecutingAssembly().CodeBase;
            var uri = new Uri(codeBase);
            var path = uri.LocalPath;
            var baseDirectory = Path.GetDirectoryName(path);
            if (string.IsNullOrEmpty(baseDirectory))
            {
                throw new Exception(
                    "No root directory could be resolved. Please ensure that your solution is correctly configured.");
            }

            _rootDirectory = baseDirectory.Contains("bin")
                           ? baseDirectory.Substring(0, baseDirectory.LastIndexOf("bin", StringComparison.OrdinalIgnoreCase) - 1)
                           : baseDirectory;

            return _rootDirectory;
        }
    }
}
