using System.IO;
using System.IO.Compression;
using System.Text;

namespace Lenneth.Core.Extensions.Compression.ZipFile
{
    public static class Extensions
    {
        #region DirectoryInfo

        #region CreateZipFile

        /// <summary>
        ///     Creates a zip archive that contains the files and directories from the specified
        ///     directory.
        /// </summary>
        /// <param name="this">The @this to act on.</param>
        /// <param name="destinationArchiveFileName">
        ///     The path of the archive to be created, specified as a
        ///     relative or absolute path. A relative path is interpreted as relative to the current working
        ///     directory.
        /// </param>
        public static void CreateZipFile(this DirectoryInfo @this, string destinationArchiveFileName)
        {
            System.IO.Compression.ZipFile.CreateFromDirectory(@this.FullName, destinationArchiveFileName);
        }

        /// <summary>
        ///     Creates a zip archive that contains the files and directories from the specified
        ///     directory, uses the specified compression level, and optionally includes the base directory.
        /// </summary>
        /// <param name="this">The @this to act on.</param>
        /// <param name="destinationArchiveFileName">
        ///     The path of the archive to be created, specified as a
        ///     relative or absolute path. A relative path is interpreted as relative to the current working
        ///     directory.
        /// </param>
        /// <param name="compressionLevel">
        ///     One of the enumeration values that indicates whether to
        ///     emphasize speed or compression effectiveness when creating the entry.
        /// </param>
        /// <param name="includeBaseDirectory">
        ///     true to include the directory name from
        ///     sourceDirectoryName at the root of the archive; false to include only the contents of the
        ///     directory.
        /// </param>
        public static void CreateZipFile(this DirectoryInfo @this, string destinationArchiveFileName, CompressionLevel compressionLevel, bool includeBaseDirectory)
        {
            System.IO.Compression.ZipFile.CreateFromDirectory(@this.FullName, destinationArchiveFileName, compressionLevel, includeBaseDirectory);
        }

        /// <summary>
        ///     Creates a zip archive that contains the files and directories from the specified directory, uses the specified
        ///     compression level and character encoding for entry names, and optionally includes the base directory.
        /// </summary>
        /// <param name="this">
        ///     The path to the directory to be archived, specified as a relative or absolute path. A relative path
        ///     is interpreted as relative to the current working directory.
        /// </param>
        /// <param name="destinationArchiveFileName">
        ///     The path of the archive to be created, specified as a relative or absolute
        ///     path. A relative path is interpreted as relative to the current working directory.
        /// </param>
        /// <param name="compressionLevel">
        ///     One of the enumeration values that indicates whether to emphasize speed or compression
        ///     effectiveness when creating the entry.
        /// </param>
        /// <param name="includeBaseDirectory">
        ///     true to include the directory name from sourceDirectoryName at the root of the
        ///     archive; false to include only the contents of the directory.
        /// </param>
        /// <param name="entryNameEncoding">
        ///     The encoding to use when reading or writing entry names in this archive. Specify a
        ///     value for this parameter only when an encoding is required for interoperability with zip archive tools and
        ///     libraries that do not support UTF-8 encoding for entry names.
        /// </param>
        public static void CreateZipFile(this DirectoryInfo @this, string destinationArchiveFileName, CompressionLevel compressionLevel, bool includeBaseDirectory, Encoding entryNameEncoding)
        {
            System.IO.Compression.ZipFile.CreateFromDirectory(@this.FullName, destinationArchiveFileName, compressionLevel, includeBaseDirectory, entryNameEncoding);
        }

        /// <summary>
        ///     Creates a zip archive that contains the files and directories from the specified
        ///     directory.
        /// </summary>
        /// <param name="this">The @this to act on.</param>
        /// <param name="destinationArchiveFile">
        ///     The path of the archive to be created, specified as a
        ///     relative or absolute path. A relative path is interpreted as relative to the current working
        ///     directory.
        /// </param>
        public static void CreateZipFile(this DirectoryInfo @this, FileInfo destinationArchiveFile)
        {
            System.IO.Compression.ZipFile.CreateFromDirectory(@this.FullName, destinationArchiveFile.FullName);
        }

        /// <summary>
        ///     Creates a zip archive that contains the files and directories from the specified
        ///     directory, uses the specified compression level, and optionally includes the base directory.
        /// </summary>
        /// <param name="this">The @this to act on.</param>
        /// <param name="destinationArchiveFile">
        ///     The path of the archive to be created, specified as a
        ///     relative or absolute path. A relative path is interpreted as relative to the current working
        ///     directory.
        /// </param>
        /// <param name="compressionLevel">
        ///     One of the enumeration values that indicates whether to
        ///     emphasize speed or compression effectiveness when creating the entry.
        /// </param>
        /// <param name="includeBaseDirectory">
        ///     true to include the directory name from
        ///     sourceDirectoryName at the root of the archive; false to include only the contents of the
        ///     directory.
        /// </param>
        public static void CreateZipFile(this DirectoryInfo @this, FileInfo destinationArchiveFile, CompressionLevel compressionLevel, bool includeBaseDirectory)
        {
            System.IO.Compression.ZipFile.CreateFromDirectory(@this.FullName, destinationArchiveFile.FullName, compressionLevel, includeBaseDirectory);
        }

        /// <summary>
        ///     Creates a zip archive that contains the files and directories from the specified
        ///     directory, uses the specified compression level and character encoding for entry names, and
        ///     optionally includes the base directory.
        /// </summary>
        /// <param name="this">
        ///     The path to the directory to be archived, specified as a relative or
        ///     absolute path. A relative path is interpreted as relative to the current working directory.
        /// </param>
        /// <param name="destinationArchiveFile">
        ///     The path of the archive to be created, specified as a
        ///     relative or absolute path. A relative path is interpreted as relative to the current working
        ///     directory.
        /// </param>
        /// <param name="compressionLevel">
        ///     One of the enumeration values that indicates whether to
        ///     emphasize speed or compression effectiveness when creating the entry.
        /// </param>
        /// <param name="includeBaseDirectory">
        ///     true to include the directory name from
        ///     sourceDirectoryName at the root of the archive; false to include only the contents of the
        ///     directory.
        /// </param>
        /// <param name="entryNameEncoding">
        ///     The encoding to use when reading or writing entry names in
        ///     this archive. Specify a value for this parameter only when an encoding is required for
        ///     interoperability with zip archive tools and libraries that do not support UTF-8 encoding for
        ///     entry names.
        /// </param>
        public static void CreateZipFile(this DirectoryInfo @this, FileInfo destinationArchiveFile, CompressionLevel compressionLevel, bool includeBaseDirectory, Encoding entryNameEncoding)
        {
            System.IO.Compression.ZipFile.CreateFromDirectory(@this.FullName, destinationArchiveFile.FullName, compressionLevel, includeBaseDirectory, entryNameEncoding);
        }

        #endregion CreateZipFile

        #endregion DirectoryInfo

        #region FileInfo

        #region OpenZipFile

        /// <summary>Opens a zip archive at the specified path and in the specified mode.</summary>
        /// <param name="this">The @this to act on.</param>
        /// <param name="mode">
        ///     One of the enumeration values that specifies the actions that are allowed
        ///     on the entries in the opened archive.
        /// </param>
        /// <returns>A ZipArchive.</returns>
        public static ZipArchive OpenZipFile(this FileInfo @this, ZipArchiveMode mode)
        {
            return System.IO.Compression.ZipFile.Open(@this.FullName, mode);
        }

        /// <summary>Opens a zip archive at the specified path and in the specified mode.</summary>
        /// <param name="this">
        ///     The path to the archive to open, specified as a relative or absolute
        ///     path. A relative path is interpreted as relative to the current working directory.
        /// </param>
        /// <param name="mode">
        ///     One of the enumeration values that specifies the actions that are allowed
        ///     on the entries in the opened archive.
        /// </param>
        /// <param name="entryNameEncoding">
        ///     The encoding to use when reading or writing entry names in
        ///     this archive. Specify a value for this parameter only when an encoding is required for
        ///     interoperability with zip archive tools and libraries that do not support UTF-8 encoding for
        ///     entry names.
        /// </param>
        /// <returns>A ZipArchive.</returns>
        public static ZipArchive OpenZipFile(this FileInfo @this, ZipArchiveMode mode, Encoding entryNameEncoding)
        {
            return System.IO.Compression.ZipFile.Open(@this.FullName, mode, entryNameEncoding);
        }

        #endregion OpenZipFile

        #region OpenReadZipFile

        /// <summary>
        ///     The path to the archive to open, specified as a relative or absolute path. A relative path is interpreted as
        ///     relative to the current working directory.
        /// </summary>
        /// <param name="this">
        ///     The path to the archive to open, specified as a relative or absolute path. A relative path is
        ///     interpreted as relative to the current working directory.
        /// </param>
        /// <returns>The opened zip archive.</returns>
        public static ZipArchive OpenReadZipFile(this FileInfo @this)
        {
            return System.IO.Compression.ZipFile.OpenRead(@this.FullName);
        }

        #endregion OpenReadZipFile

        #region ExtractZipFileToDirectory

        /// <summary>
        ///     Extracts all the files in the specified zip archive to a directory on the file system
        /// </summary>
        /// <param name="this">The @this to act on.</param>
        /// <param name="destinationDirectoryName">
        ///     The path to the directory in which to place the
        ///     extracted files, specified as a relative or absolute path. A relative path is interpreted as
        ///     relative to the current working directory.
        /// </param>
        public static void ExtractZipFileToDirectory(this FileInfo @this, string destinationDirectoryName)
        {
            System.IO.Compression.ZipFile.ExtractToDirectory(@this.FullName, destinationDirectoryName);
        }

        /// <summary>
        ///     Extracts all the files in the specified zip archive to a directory on the file system and uses the specified
        ///     character encoding for entry names.
        /// </summary>
        /// <param name="this">The path to the archive that is to be extracted.</param>
        /// <param name="destinationDirectoryName">
        ///     The path to the directory in which to place the extracted files, specified as a
        ///     relative or absolute path. A relative path is interpreted as relative to the current working directory.
        /// </param>
        /// <param name="entryNameEncoding">
        ///     The encoding to use when reading or writing entry names in this archive. Specify a
        ///     value for this parameter only when an encoding is required for interoperability with zip archive tools and
        ///     libraries that do not support UTF-8 encoding for entry names.
        /// </param>
        public static void ExtractZipFileToDirectory(this FileInfo @this, string destinationDirectoryName, Encoding entryNameEncoding)
        {
            System.IO.Compression.ZipFile.ExtractToDirectory(@this.FullName, destinationDirectoryName, entryNameEncoding);
        }

        /// <summary>Extracts all the files in the specified zip archive to a directory on the file system.</summary>
        /// <param name="this">The @this to act on.</param>
        /// <param name="destinationDirectory">Pathname of the destination directory.</param>
        public static void ExtractZipFileToDirectory(this FileInfo @this, DirectoryInfo destinationDirectory)
        {
            System.IO.Compression.ZipFile.ExtractToDirectory(@this.FullName, destinationDirectory.FullName);
        }

        /// <summary>
        ///     Extracts all the files in the specified zip archive to a directory on the file system
        ///     and uses the specified character encoding for entry names.
        /// </summary>
        /// <param name="this">The @this to act on.</param>
        /// <param name="destinationDirectory">Pathname of the destination directory.</param>
        /// <param name="entryNameEncoding">
        ///     The encoding to use when reading or writing entry names in
        ///     this archive. Specify a value for this parameter only when an encoding is required for
        ///     interoperability with zip archive tools and libraries that do not support UTF-8 encoding for
        ///     entry names.
        /// </param>
        public static void ExtractZipFileToDirectory(this FileInfo @this, DirectoryInfo destinationDirectory, Encoding entryNameEncoding)
        {
            System.IO.Compression.ZipFile.ExtractToDirectory(@this.FullName, destinationDirectory.FullName, entryNameEncoding);
        }

        #endregion ExtractZipFileToDirectory

        #endregion FileInfo
    }
}