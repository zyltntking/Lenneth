using System;
using System.IO;

namespace Lenneth.Core.Extensions.Utils
{
    public static class FileUtils
    {
        public static bool IsFilePathValid(string aPath)
        {
            if (string.IsNullOrEmpty(aPath.Trim()))
            {
                return false;
            }

            string pathname;
            string filename;

            try
            {
                pathname = Path.GetPathRoot(aPath);
                filename = Path.GetFileName(aPath);
            }
            catch (ArgumentException)
            {
                // GetPathRoot() and GetFileName() above will throw exceptions
                // if pathname/filename could not be parsed.

                return false;
            }

            // Make sure the filename part was actually specified
            if (string.IsNullOrEmpty(filename.Trim()))
            {
                return false;
            }

            // Not sure if additional checking below is needed, but no harm done
            if (pathname.IndexOfAny(Path.GetInvalidPathChars()) >= 0)
            {
                return false;
            }

            if (filename.IndexOfAny(Path.GetInvalidFileNameChars()) >= 0)
            {
                return false;
            }

            return true;
        }

        public static byte[] ReadFile(string aPath)
        {
            return File.ReadAllBytes(aPath);
        }
    }
}