using System.Diagnostics;
using System.IO;

namespace Lenneth.Core.Extensions
{
    [DebuggerStepThrough]
    public static class FileInfoExtensions
    {
        public static void Rename(this FileInfo aFileInfo, string aFileName)
        {
            var filePath = Path.Combine(Path.GetDirectoryName(aFileInfo.FullName), aFileName);
            aFileInfo.MoveTo(filePath);
        }

        public static void RenameFileWithoutExtension(this FileInfo aFileInfo, string aFileName)
        {
            var fileName = string.Concat(aFileName, aFileInfo.Extension);
            aFileInfo.Rename(fileName);
        }

        public static void ChangeExtension(this FileInfo aFileInfo, string aFileExt)
        {
            aFileExt = aFileExt.EnsureStartsWith(".");
            var fileName = string.Concat(Path.GetFileNameWithoutExtension(aFileInfo.FullName), aFileExt);
            aFileInfo.Rename(fileName);
        }
    }
}