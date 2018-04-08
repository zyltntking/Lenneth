using System;
using System.Diagnostics;
using System.IO;
using System.Reflection;

namespace Lenneth.Core.Extensions
{
    [DebuggerStepThrough]
    public static class StreamExtensions
    {
        /// <summary>
        /// Get stream for resource.
        /// </summary>
        /// <param name="aType"></param>
        /// <param name="aResName"></param>
        /// <param name="aResSubfolder"></param>
        /// <returns></returns>
        public static Stream FromResource(Type aType, string aResName, string aResSubfolder = "")
        {
            if (aResSubfolder != "")
                aResSubfolder = "." + aResSubfolder;

            var res = aType.GetParentFullName() + aResSubfolder + "." + aResName;
            return Assembly.GetAssembly(aType).GetManifestResourceStream(res);
        }

        /// <summary>
        /// Read all bytes from stream.
        /// </summary>
        /// <param name="aStream"></param>
        /// <returns></returns>
        public static byte[] ReadAll(this Stream aStream)
        {
            var res = new byte[aStream.Length];
            aStream.Read(res, 0, res.Length);
            return res;
        }

        public static Stream SeekToBegin(this Stream aStream)
        {
            aStream.Seek(0, SeekOrigin.Begin);
            return aStream;
        }

        public static Stream SeekToEnd(this Stream aStream)
        {
            aStream.Seek(0, SeekOrigin.End);
            return aStream;
        }
    }
}