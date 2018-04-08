using System.Collections.Generic;
using System.Diagnostics;
using System.IO;

namespace Lenneth.Core.Extensions
{
    [DebuggerStepThrough]
    public static class DirectoryInfoExtensions
    {
        public static void DeleteAll(this DirectoryInfo aDirInfo)
        {
            if (!aDirInfo.Exists)
                return;

            foreach (var fileInfo in aDirInfo.GetFiles())
                fileInfo.Delete();

            foreach (var dirInfo in aDirInfo.GetDirectories())
                dirInfo.DeleteAll();

            aDirInfo.Delete(false);
        }

        public static void DeleteContent(this DirectoryInfo aDirInfo)
        {
            if (!aDirInfo.Exists)
                return;

            foreach (var fileInfo in aDirInfo.GetFiles())
                fileInfo.Delete();

            foreach (var dirInfo in aDirInfo.GetDirectories())
                dirInfo.DeleteAll();
        }

        public static void CreateOrEmpty(this DirectoryInfo aDirInfo)
        {
            aDirInfo.DeleteContent();
            aDirInfo.Create();
        }

        public static FileInfo[] FindFilesRecursive(this DirectoryInfo aDirInfo, string aPattern)
        {
            var foundFiles = new List<FileInfo>();
            FindFilesRecursive(aDirInfo, aPattern, foundFiles);
            return foundFiles.ToArray();
        }

        private static void FindFilesRecursive(DirectoryInfo aDirInfo, string aPattern,
            List<FileInfo> aFiles)
        {
            aFiles.AddRange(aDirInfo.GetFiles(aPattern));

            foreach (var dir in aDirInfo.GetDirectories())
                FindFilesRecursive(dir, aPattern, aFiles);
        }

        public static string FindExistingDirectory(this DirectoryInfo aDi)
        {
            var str = aDi.FullName;

            for (; ; )
            {
                var di = new DirectoryInfo(str);

                if (di.Exists)
                    return str;

                if (di.Parent == null)
                    return "";

                str = di.Parent.FullName;
            }
        }

        public static DirectoryInfo Append(this DirectoryInfo aDi, string aDir)
        {
            return new DirectoryInfo(aDi.FullName + Path.DirectorySeparatorChar + aDir);
        }
    }
}