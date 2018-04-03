﻿// --------------------------------------------------------------------------------------------------------------------
// <copyright file="NativeMethods.cs" company="James Jackson-South">
//   Copyright (c) James Jackson-South.
//   Licensed under the Apache License, Version 2.0.
// </copyright>
// <summary>
//   Provides access to unmanaged native methods.
// </summary>
// --------------------------------------------------------------------------------------------------------------------

using System;
using System.Runtime.InteropServices;

namespace Lenneth.Core.Framework.ImageProcessor.Configuration
{
    /// <summary>
    /// Provides access to unmanaged native methods.
    /// </summary>
    internal class NativeMethods
    {
        /// <summary>
        /// Loads the specified module into the address space of the calling process. 
        /// The specified module may cause other modules to be loaded.
        /// </summary>
        /// <param name="libname">
        /// The name of the module. This can be either a library module or 
        /// an executable module. 
        /// </param>
        /// <returns>If the function succeeds, the return value is a handle to the module; otherwise null.</returns>
        [DllImport("kernel32", SetLastError = true, CharSet = CharSet.Auto)]
        public static extern IntPtr LoadLibrary(string libname);

        /// <summary>
        /// Frees the loaded dynamic-link library (DLL) module and, if necessary, decrements its reference count. 
        /// When the reference count reaches zero, the module is unloaded from the address space of the calling 
        /// process and the handle is no longer valid.
        /// </summary>
        /// <param name="hModule">A handle to the loaded library module. 
        /// The LoadLibrary, LoadLibraryEx, GetModuleHandle, or GetModuleHandleEx function returns this handle.</param>
        /// <returns>If the function succeeds, the return value is nonzero; otherwise zero.</returns>
        [DllImport("kernel32", SetLastError = true)]
        public static extern bool FreeLibrary(IntPtr hModule);

        /// <summary>
        /// Loads the specified module into the address space of the calling process. 
        /// The specified module may cause other modules to be loaded.
        /// </summary>
        /// <param name="libname">
        /// The name of the module. This can be either a library module or 
        /// an executable module. 
        /// </param>
        /// <param name="flags">
        /// The flag indicating whether to load the library immediately or lazily.
        /// </param>
        /// <returns>
        /// If the function succeeds, the return value is a handle to the module; otherwise null.
        /// </returns>
        [DllImport("libdl")]
        public static extern IntPtr dlopen(string libname, int flags);

        /// <summary>
        /// Frees the loaded dynamic-link library (DLL) module and, if necessary, decrements its reference count. 
        /// When the reference count reaches zero, the module is unloaded from the address space of the calling 
        /// process and the handle is no longer valid.
        /// </summary>
        /// <param name="hModule">A handle to the loaded library module. 
        /// The LoadLibrary, LoadLibraryEx, GetModuleHandle, or GetModuleHandleEx function returns this handle.</param>
        /// <returns>If the function succeeds, the return value is nonzero; otherwise zero.</returns>
        [DllImport("libdl")]
        public static extern int dlclose(IntPtr hModule);
    }
}
