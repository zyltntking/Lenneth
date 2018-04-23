using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Security.AccessControl;
using System.Text;

namespace Lenneth.Core.Extensions.Extra.IOExtensions
{
    public static class Extensions
    {
        #region IEnumerable

        #region IEnumerable<DirectoryInfo>

        #region Delete

        /// <summary>
        ///     An IEnumerable&lt;DirectoryInfo&gt; extension method that deletes the given @this.
        /// </summary>
        /// <param name="this">The @this to act on.</param>
        public static void Delete(this IEnumerable<DirectoryInfo> @this)
        {
            foreach (var t in @this)
            {
                t.Delete();
            }
        }

        #endregion Delete

        #region ForEach

        /// <summary>
        ///     Enumerates for each in this collection.
        /// </summary>
        /// <param name="this">The @this to act on.</param>
        /// <param name="action">The action.</param>
        /// <returns>An enumerator that allows foreach to be used to process for each in this collection.</returns>
        public static IEnumerable<DirectoryInfo> ForEach(this IEnumerable<DirectoryInfo> @this, Action<DirectoryInfo> action)
        {
            var directoryInfos = @this as DirectoryInfo[] ?? @this.ToArray();
            foreach (var t in directoryInfos)
            {
                action(t);
            }
            return directoryInfos;
        }

        #endregion ForEach

        #endregion IEnumerable<DirectoryInfo>

        #region IEnumerable<FileInfo>

        #region Delete

        /// <summary>
        ///     An IEnumerable&lt;FileInfo&gt; extension method that deletes the given @this.
        /// </summary>
        /// <param name="this">The @this to act on.</param>
        public static void Delete(this IEnumerable<FileInfo> @this)
        {
            foreach (var t in @this)
            {
                t.Delete();
            }
        }

        #endregion Delete

        #region ForEach

        /// <summary>
        ///     Enumerates for each in this collection.
        /// </summary>
        /// <param name="this">The @this to act on.</param>
        /// <param name="action">The action.</param>
        /// <returns>An enumerator that allows foreach to be used to process for each in this collection.</returns>
        public static IEnumerable<FileInfo> ForEach(this IEnumerable<FileInfo> @this, Action<FileInfo> action)
        {
            var fileInfos = @this as FileInfo[] ?? @this.ToArray();
            foreach (var t in fileInfos)
            {
                action(t);
            }
            return fileInfos;
        }

        #endregion ForEach

        #endregion IEnumerable<FileInfo>

        #region IEnumerable<string>

        #region PathCombine

        /// <summary>
        ///     An IEnumerable&lt;string&gt; extension method that combine all value to return a path.
        /// </summary>
        /// <param name="this">The @this to act on.</param>
        /// <returns>The path.</returns>
        public static string PathCombine(this IEnumerable<string> @this)
        {
            return Path.Combine(@this.ToArray());
        }

        #endregion PathCombine

        #endregion IEnumerable<string>

        #endregion IEnumerable

        #region DirectoryInfo

        #region Clear

        /// <summary>
        ///     A DirectoryInfo extension method that clears all files and directories in this directory.
        /// </summary>
        /// <param name="obj">The obj to act on.</param>
        public static void Clear(this DirectoryInfo obj)
        {
            Array.ForEach(obj.GetFiles(), x => x.Delete());
            Array.ForEach(obj.GetDirectories(), x => x.Delete(true));
        }

        #endregion Clear

        #region CopyTo

        /// <summary>A DirectoryInfo extension method that copies to.</summary>
        /// <param name="obj">The obj to act on.</param>
        /// <param name="destDirName">Pathname of the destination directory.</param>
        public static void CopyTo(this DirectoryInfo obj, string destDirName)
        {
            obj.CopyTo(destDirName, "*.*", SearchOption.TopDirectoryOnly);
        }

        /// <summary>A DirectoryInfo extension method that copies to.</summary>
        /// <param name="obj">The obj to act on.</param>
        /// <param name="destDirName">Pathname of the destination directory.</param>
        /// <param name="searchPattern">A pattern specifying the search.</param>
        public static void CopyTo(this DirectoryInfo obj, string destDirName, string searchPattern)
        {
            obj.CopyTo(destDirName, searchPattern, SearchOption.TopDirectoryOnly);
        }

        /// <summary>A DirectoryInfo extension method that copies to.</summary>
        /// <param name="obj">The obj to act on.</param>
        /// <param name="destDirName">Pathname of the destination directory.</param>
        /// <param name="searchOption">The search option.</param>
        public static void CopyTo(this DirectoryInfo obj, string destDirName, SearchOption searchOption)
        {
            obj.CopyTo(destDirName, "*.*", searchOption);
        }

        /// <summary>A DirectoryInfo extension method that copies to.</summary>
        /// <exception cref="Exception">Thrown when an exception error condition occurs.</exception>
        /// <param name="obj">The obj to act on.</param>
        /// <param name="destDirName">Pathname of the destination directory.</param>
        /// <param name="searchPattern">A pattern specifying the search.</param>
        /// <param name="searchOption">The search option.</param>
        public static void CopyTo(this DirectoryInfo obj, string destDirName, string searchPattern, SearchOption searchOption)
        {
            var files = obj.GetFiles(searchPattern, searchOption);
            foreach (var file in files)
            {
                var outputFile = destDirName + file.FullName.Substring(obj.FullName.Length);
                var directory = new FileInfo(outputFile).Directory;

                if (directory == null)
                {
                    throw new Exception("The directory cannot be null.");
                }

                if (!directory.Exists)
                {
                    directory.Create();
                }

                file.CopyTo(outputFile);
            }

            // Ensure empty dir are also copied
            var directories = obj.GetDirectories(searchPattern, searchOption);
            foreach (var directory in directories)
            {
                var outputDirectory = destDirName + directory.FullName.Substring(obj.FullName.Length);
                var directoryInfo = new DirectoryInfo(outputDirectory);
                if (!directoryInfo.Exists)
                {
                    directoryInfo.Create();
                }
            }
        }

        #endregion CopyTo

        #region CreateAllDirectories

        /// <summary>
        ///     Creates all directories and subdirectories in the specified @this.
        /// </summary>
        /// <param name="this">The directory @this to create.</param>
        /// <returns>An object that represents the directory for the specified @this.</returns>
        public static DirectoryInfo CreateAllDirectories(this DirectoryInfo @this)
        {
            return Directory.CreateDirectory(@this.FullName);
        }

        /// <summary>
        ///     Creates all the directories in the specified @this, applying the specified Windows security.
        /// </summary>
        /// <param name="this">The directory to create.</param>
        /// <param name="directorySecurity">The access control to apply to the directory.</param>
        /// <returns>An object that represents the directory for the specified @this.</returns>
        public static DirectoryInfo CreateAllDirectories(this DirectoryInfo @this, DirectorySecurity directorySecurity)
        {
            return Directory.CreateDirectory(@this.FullName, directorySecurity);
        }

        #endregion CreateAllDirectories

        #region DeleteDirectoriesWhere

        /// <summary>
        ///     A DirectoryInfo extension method that deletes the directories where.
        /// </summary>
        /// <param name="obj">The obj to act on.</param>
        /// <param name="predicate">The predicate.</param>
        public static void DeleteDirectoriesWhere(this DirectoryInfo obj, Func<DirectoryInfo, bool> predicate)
        {
            obj.GetDirectories().Where(predicate).ForEach(x => x.Delete());
        }

        /// <summary>
        ///     A DirectoryInfo extension method that deletes the directories where.
        /// </summary>
        /// <param name="obj">The obj to act on.</param>
        /// <param name="searchOption">The search option.</param>
        /// <param name="predicate">The predicate.</param>
        public static void DeleteDirectoriesWhere(this DirectoryInfo obj, SearchOption searchOption, Func<DirectoryInfo, bool> predicate)
        {
            obj.GetDirectories("*.*", searchOption).Where(predicate).ForEach(x => x.Delete());
        }

        #endregion DeleteDirectoriesWhere

        #region DeleteFilesWhere

        /// <summary>
        ///     A DirectoryInfo extension method that deletes the files where.
        /// </summary>
        /// <param name="obj">The obj to act on.</param>
        /// <param name="predicate">The predicate.</param>
        public static void DeleteFilesWhere(this DirectoryInfo obj, Func<FileInfo, bool> predicate)
        {
            obj.GetFiles().Where(predicate).ForEach(x => x.Delete());
        }

        /// <summary>
        ///     A DirectoryInfo extension method that deletes the files where.
        /// </summary>
        /// <param name="obj">The obj to act on.</param>
        /// <param name="searchOption">The search option.</param>
        /// <param name="predicate">The predicate.</param>
        public static void DeleteFilesWhere(this DirectoryInfo obj, SearchOption searchOption, Func<FileInfo, bool> predicate)
        {
            obj.GetFiles("*.*", searchOption).Where(predicate).ForEach(x => x.Delete());
        }

        #endregion DeleteFilesWhere

        #region DeleteOlderThan

        /// <summary>
        ///     A DirectoryInfo extension method that deletes the older than.
        /// </summary>
        /// <param name="obj">The obj to act on.</param>
        /// <param name="timeSpan">The time span.</param>
        public static void DeleteOlderThan(this DirectoryInfo obj, TimeSpan timeSpan)
        {
            var minDate = DateTime.Now.Subtract(timeSpan);
            obj.GetFiles().Where(x => x.LastWriteTime < minDate).ToList().ForEach(x => x.Delete());
            obj.GetDirectories().Where(x => x.LastWriteTime < minDate).ToList().ForEach(x => x.Delete());
        }

        /// <summary>
        ///     A DirectoryInfo extension method that deletes the older than.
        /// </summary>
        /// <param name="obj">The obj to act on.</param>
        /// <param name="searchOption">The search option.</param>
        /// <param name="timeSpan">The time span.</param>
        public static void DeleteOlderThan(this DirectoryInfo obj, SearchOption searchOption, TimeSpan timeSpan)
        {
            var minDate = DateTime.Now.Subtract(timeSpan);
            obj.GetFiles("*.*", searchOption).Where(x => x.LastWriteTime < minDate).ToList().ForEach(x => x.Delete());
            obj.GetDirectories("*.*", searchOption).Where(x => x.LastWriteTime < minDate).ToList().ForEach(x => x.Delete());
        }

        #endregion DeleteOlderThan

        #region EnsureDirectoryExists

        /// <summary>
        ///     Creates all directories and subdirectories in the specified @this if the directory doesn't already exists.
        ///     This methods is the same as FileInfo.CreateDirectory however it's less ambigues about what happen if the
        ///     directory already exists.
        /// </summary>
        /// <param name="this">The directory @this to create.</param>
        /// <returns>An object that represents the directory for the specified @this.</returns>
        public static DirectoryInfo EnsureDirectoryExists(this DirectoryInfo @this)
        {
            return Directory.CreateDirectory(@this.FullName);
        }

        /// <summary>
        ///     Creates all directories and subdirectories in the specified @this if the directory doesn't already exists.
        ///     This methods is the same as FileInfo.CreateDirectory however it's less ambigues about what happen if the
        ///     directory already exists.
        /// </summary>
        /// <param name="this">The directory to create.</param>
        /// <param name="directorySecurity">The access control to apply to the directory.</param>
        /// <returns>An object that represents the directory for the specified @this.</returns>
        public static DirectoryInfo EnsureDirectoryExists(this DirectoryInfo @this, DirectorySecurity directorySecurity)
        {
            return Directory.CreateDirectory(@this.FullName, directorySecurity);
        }

        #endregion EnsureDirectoryExists

        #region EnumerateDirectories

        /// <summary>
        ///     Returns an enumerable collection of directory names in a specified @this.
        /// </summary>
        /// <param name="this">The directory to search.</param>
        /// <returns>
        ///     An enumerable collection of the full names (including paths) for the directories in the directory specified by
        ///     <paramref name="this" />.
        /// </returns>
        public static IEnumerable<DirectoryInfo> EnumerateDirectories(this DirectoryInfo @this)
        {
            return Directory.EnumerateDirectories(@this.FullName).Select(x => new DirectoryInfo(x));
        }

        /// <summary>
        ///     Returns an enumerable collection of directory names that match a search pattern in a specified @this.
        /// </summary>
        /// <param name="this">The directory to search.</param>
        /// <param name="searchPattern">
        ///     The search string to match against the names of directories in
        ///     <paramref name="this" />.
        /// </param>
        /// <returns>
        ///     An enumerable collection of the full names (including paths) for the directories in the directory specified by
        ///     <paramref name="this" />
        ///     and that match the specified search pattern.
        /// </returns>
        public static IEnumerable<DirectoryInfo> EnumerateDirectories(this DirectoryInfo @this, string searchPattern)
        {
            return Directory.EnumerateDirectories(@this.FullName, searchPattern).Select(x => new DirectoryInfo(x));
        }

        /// <summary>
        ///     Returns an enumerable collection of directory names that match a search pattern in a specified @this, and
        ///     optionally searches subdirectories.
        /// </summary>
        /// <param name="this">The directory to search.</param>
        /// <param name="searchPattern">
        ///     The search string to match against the names of directories in
        ///     <paramref name="this" />.
        /// </param>
        /// <param name="searchOption">
        ///     One of the enumeration values that specifies whether the search operation should
        ///     include only the current directory or should include all subdirectories.The default value is
        ///     <see cref="F:System.IO.SearchOption.TopDirectoryOnly" /> </param>
        /// <returns>
        ///     An enumerable collection of the full names (including paths) for the directories in the directory specified by
        ///     <paramref
        ///         name="this" />
        ///     and that match the specified search pattern and option.
        /// </returns>
        public static IEnumerable<DirectoryInfo> EnumerateDirectories(this DirectoryInfo @this, string searchPattern, SearchOption searchOption)
        {
            return Directory.EnumerateDirectories(@this.FullName, searchPattern, searchOption).Select(x => new DirectoryInfo(x));
        }

        /// <summary>
        ///     Returns an enumerable collection of directory names that match a search pattern in a specified @this.
        /// </summary>
        /// <param name="this">The directory to search.</param>
        /// <param name="searchPatterns">The search string to match against the names of directories in.</param>
        /// <returns>
        ///     An enumerable collection of the full names (including paths) for the directories in the directory specified by
        ///     <paramref name="this" />
        ///     and that match the specified search pattern.
        /// </returns>
        public static IEnumerable<DirectoryInfo> EnumerateDirectories(this DirectoryInfo @this, IEnumerable<string> searchPatterns)
        {
            return searchPatterns.SelectMany(@this.GetDirectories).Distinct();
        }

        /// <summary>
        ///     Returns an enumerable collection of directory names that match a search pattern in a specified @this, and
        ///     optionally searches subdirectories.
        /// </summary>
        /// <param name="this">The directory to search.</param>
        /// <param name="searchPatterns">
        ///     The search string to match against the names of directories in
        ///     <paramref name="this" />.
        /// </param>
        /// <param name="searchOption">
        ///     One of the enumeration values that specifies whether the search operation should
        ///     include only the current directory or should include all subdirectories.The default value is
        ///     <see cref="F:System.IO.SearchOption.TopDirectoryOnly" />
        /// </param>
        /// <returns>
        ///     An enumerable collection of the full names (including paths) for the directories in the directory specified by
        ///     <paramref name="this" />
        ///     and that match the specified search pattern and option.
        /// </returns>
        public static IEnumerable<DirectoryInfo> EnumerateDirectories(this DirectoryInfo @this, IEnumerable<string> searchPatterns, SearchOption searchOption)
        {
            return searchPatterns.SelectMany(x => @this.GetDirectories(x, searchOption)).Distinct();
        }

        #endregion EnumerateDirectories

        #region EnumerateFiles

        /// <summary>
        ///     Returns an enumerable collection of file names in a specified @this.
        /// </summary>
        /// <param name="this">The directory to search.</param>
        /// <returns>
        ///     An enumerable collection of the full names (including paths) for the files in the directory specified by
        ///     <paramref name="this" />
        /// </returns>
        public static IEnumerable<FileInfo> EnumerateFiles(this DirectoryInfo @this)
        {
            return Directory.EnumerateFiles(@this.FullName).Select(x => new FileInfo(x));
        }

        /// <summary>
        ///     Returns an enumerable collection of file names that match a search pattern in a specified @this.
        /// </summary>
        /// <param name="this">The directory to search.</param>
        /// <param name="searchPattern">
        ///     The search string to match against the names of directories in
        ///     <paramref name="this" />.
        /// </param>
        /// <returns>
        ///     An enumerable collection of the full names (including paths) for the files in the directory specified by
        ///     <paramref name="this" />
        ///     and that match the specified search pattern.
        /// </returns>
        public static IEnumerable<FileInfo> EnumerateFiles(this DirectoryInfo @this, string searchPattern)
        {
            return Directory.EnumerateFiles(@this.FullName, searchPattern).Select(x => new FileInfo(x));
        }

        /// <summary>
        ///     Returns an enumerable collection of file names that match a search pattern in a specified @this, and
        ///     optionally searches subdirectories.
        /// </summary>
        /// <param name="this">The directory to search.</param>
        /// <param name="searchPattern">
        ///     The search string to match against the names of directories in
        ///     <paramref name="this" />.
        /// </param>
        /// <param name="searchOption">
        ///     One of the enumeration values that specifies whether the search operation should
        ///     include only the current directory or should include all subdirectories.The default value is
        ///     <see cref="F:System.IO.SearchOption.TopDirectoryOnly" />
        /// </param>
        /// <returns>
        ///     An enumerable collection of the full names (including paths) for the files in the directory specified by
        ///     <paramref name="this" />
        ///     and that match the specified search pattern and option.
        /// </returns>
        public static IEnumerable<FileInfo> EnumerateFiles(this DirectoryInfo @this, string searchPattern, SearchOption searchOption)
        {
            return Directory.EnumerateFiles(@this.FullName, searchPattern, searchOption).Select(x => new FileInfo(x));
        }

        /// <summary>
        ///     Returns an enumerable collection of file names that match a search pattern in a specified @this.
        /// </summary>
        /// <param name="this">The directory to search.</param>
        /// <param name="searchPatterns">The search string to match against the names of directories in.</param>
        /// <returns>
        ///     An enumerable collection of the full names (including paths) for the files in the directory specified by
        ///     <paramref name="this" />
        ///     and that match the specified search pattern.
        /// </returns>
        public static IEnumerable<FileInfo> EnumerateFiles(this DirectoryInfo @this, IEnumerable<string> searchPatterns)
        {
            return searchPatterns.SelectMany(@this.GetFiles).Distinct();
        }

        /// <summary>
        ///     Returns an enumerable collection of file names that match a search pattern in a specified @this, and
        ///     optionally searches subdirectories.
        /// </summary>
        /// <param name="this">The directory to search.</param>
        /// <param name="searchPatterns">
        ///     The search string to match against the names of directories in
        ///     <paramref name="this" />.
        /// </param>
        /// <param name="searchOption">
        ///     One of the enumeration values that specifies whether the search operation should
        ///     include only the current directory or should include all subdirectories.The default value is
        ///     <see cref="F:System.IO.SearchOption.TopDirectoryOnly" />
        /// </param>
        /// <returns>
        ///     An enumerable collection of the full names (including paths) for the files in the directory specified by
        ///     <paramref name="this" />
        ///     and that match the specified search pattern and option.
        /// </returns>
        public static IEnumerable<FileInfo> EnumerateFiles(this DirectoryInfo @this, IEnumerable<string> searchPatterns, SearchOption searchOption)
        {
            return searchPatterns.SelectMany(x => @this.GetFiles(x, searchOption)).Distinct();
        }

        #endregion EnumerateFiles

        #region EnumerateFileSystemEntries

        /// <summary>
        ///     Returns an enumerable collection of file-system entries in a specified @this.
        /// </summary>
        /// <param name="this">The directory to search.</param>
        /// <returns>
        ///     An enumerable collection of file-system entries in the directory specified by <paramref name="this" />.
        /// </returns>
        public static IEnumerable<string> EnumerateFileSystemEntries(this DirectoryInfo @this)
        {
            return Directory.EnumerateFileSystemEntries(@this.FullName);
        }

        /// <summary>
        ///     Returns an enumerable collection of file-system entries that match a search pattern in a specified @this.
        /// </summary>
        /// <param name="this">The directory to search.</param>
        /// <param name="searchPattern">
        ///     The search string to match against the names of directories in
        ///     <paramref name="this" />.
        /// </param>
        /// <returns>
        ///     An enumerable collection of file-system entries in the directory specified by <paramref name="this" /> and
        ///     that match the specified search pattern.
        /// </returns>
        public static IEnumerable<string> EnumerateFileSystemEntries(this DirectoryInfo @this, string searchPattern)
        {
            return Directory.EnumerateFileSystemEntries(@this.FullName, searchPattern);
        }

        /// <summary>
        ///     Returns an enumerable collection of file names and directory names that match a search pattern in a specified
        ///     @this, and optionally searches subdirectories.
        /// </summary>
        /// <param name="this">The directory to search.</param>
        /// <param name="searchPattern">
        ///     The search string to match against the names of directories in
        ///     <paramref name="this" />.
        /// </param>
        /// <param name="searchOption">
        ///     One of the enumeration values  that specifies whether the search operation should
        ///     include only the current directory or should include all subdirectories.The default value is
        ///     <see cref="F:System.IO.SearchOption.TopDirectoryOnly" />
        /// </param>
        /// <returns>
        ///     An enumerable collection of file-system entries in the directory specified by <paramref name="this" /> and
        ///     that match the specified search pattern and option.
        /// </returns>
        public static IEnumerable<string> EnumerateFileSystemEntries(this DirectoryInfo @this, string searchPattern, SearchOption searchOption)
        {
            return Directory.EnumerateFileSystemEntries(@this.FullName, searchPattern, searchOption);
        }

        /// <summary>
        ///     Returns an enumerable collection of file-system entries that match a search pattern in a specified @this.
        /// </summary>
        /// <param name="this">The directory to search.</param>
        /// <param name="searchPatterns">The search string to match against the names of directories in.</param>
        /// <returns>
        ///     An enumerable collection of file-system entries in the directory specified by <paramref name="this" /> and
        ///     that match the specified search pattern.
        /// </returns>
        public static IEnumerable<string> EnumerateFileSystemEntries(this DirectoryInfo @this, IEnumerable<string> searchPatterns)
        {
            return searchPatterns.SelectMany(x => Directory.EnumerateFileSystemEntries(@this.FullName, x)).Distinct();
        }

        /// <summary>
        ///     Returns an enumerable collection of file names and directory names that match a search pattern in a specified
        ///     @this, and optionally searches subdirectories.
        /// </summary>
        /// <param name="this">The directory to search.</param>
        /// <param name="searchPatterns">
        ///     The search string to match against the names of directories in
        ///     <paramref name="this" />.
        /// </param>
        /// <param name="searchOption">
        ///     One of the enumeration values  that specifies whether the search operation should
        ///     include only the current directory or should include all subdirectories.The default value is
        ///     <see cref="F:System.IO.SearchOption.TopDirectoryOnly" />
        /// </param>
        /// <returns>
        ///     An enumerable collection of file-system entries in the directory specified by <paramref name="this" /> and
        ///     that match the specified search pattern and option.
        /// </returns>
        public static IEnumerable<string> EnumerateFileSystemEntries(this DirectoryInfo @this, IEnumerable<string> searchPatterns, SearchOption searchOption)
        {
            return searchPatterns.SelectMany(x => Directory.EnumerateFileSystemEntries(@this.FullName, x, searchOption)).Distinct();
        }

        #endregion EnumerateFileSystemEntries

        #region GetDirectories

        /// <summary>
        ///     Returns an enumerable collection of directory names that match a search pattern in a specified @this.
        /// </summary>
        /// <param name="this">The directory to search.</param>
        /// <param name="searchPatterns">The search string to match against the names of directories in.</param>
        /// <returns>
        ///     An enumerable collection of the full names (including paths) for the directories in the directory specified by
        ///     <paramref name="this" />
        ///     and that match the specified search pattern.
        /// </returns>
        public static DirectoryInfo[] GetDirectories(this DirectoryInfo @this, IEnumerable<string> searchPatterns)
        {
            return searchPatterns.SelectMany(@this.GetDirectories).Distinct().ToArray();
        }

        /// <summary>
        ///     Returns an enumerable collection of directory names that match a search pattern in a specified @this, and
        ///     optionally searches subdirectories.
        /// </summary>
        /// <param name="this">The directory to search.</param>
        /// <param name="searchPatterns">
        ///     The search string to match against the names of directories in
        ///     <paramref name="this" />.
        /// </param>
        /// <param name="searchOption">
        ///     One of the enumeration values that specifies whether the search operation should
        ///     include only the current directory or should include all subdirectories.The default value is
        ///     <see
        ///         cref="F:System.IO.SearchOption.TopDirectoryOnly" />
        ///     .
        /// </param>
        /// <returns>
        ///     An enumerable collection of the full names (including paths) for the directories in the directory specified by
        ///     <paramref name="this" />
        ///     and that match the specified search pattern and option.
        /// </returns>
        public static DirectoryInfo[] GetDirectories(this DirectoryInfo @this, IEnumerable<string> searchPatterns, SearchOption searchOption)
        {
            return searchPatterns.SelectMany(x => @this.GetDirectories(x, searchOption)).Distinct().ToArray();
        }

        #endregion GetDirectories

        #region GetDirectoriesWhere

        /// <summary>
        ///     Returns an enumerable collection of directory names in a specified @this.
        /// </summary>
        /// <param name="this">The directory to search.</param>
        /// <param name="predicate">The predicate.</param>
        /// <returns>
        ///     An enumerable collection of the full names (including paths) for the directories in the directory specified by
        ///     <paramref name="this" />
        /// </returns>
        public static DirectoryInfo[] GetDirectoriesWhere(this DirectoryInfo @this, Func<DirectoryInfo, bool> predicate)
        {
            return Directory.EnumerateDirectories(@this.FullName).Select(x => new DirectoryInfo(x)).Where(predicate).ToArray();
        }

        /// <summary>
        ///     Returns an enumerable collection of directory names that match a search pattern in a specified @this.
        /// </summary>
        /// <param name="this">The directory to search.</param>
        /// <param name="searchPattern">
        ///     The search string to match against the names of directories in
        ///     <paramref name="this" />.
        /// </param>
        /// <param name="predicate">The predicate.</param>
        /// <returns>
        ///     An enumerable collection of the full names (including paths) for the directories in the directory specified by
        ///     <paramref name="this" />
        ///     and that match the specified search pattern.
        /// </returns>
        public static DirectoryInfo[] GetDirectoriesWhere(this DirectoryInfo @this, string searchPattern, Func<DirectoryInfo, bool> predicate)
        {
            return Directory.EnumerateDirectories(@this.FullName, searchPattern).Select(x => new DirectoryInfo(x)).Where(predicate).ToArray();
        }

        /// <summary>
        ///     Returns an enumerable collection of directory names that match a search pattern in a specified @this, and
        ///     optionally searches subdirectories.
        /// </summary>
        /// <param name="this">The directory to search.</param>
        /// <param name="searchPattern">
        ///     The search string to match against the names of directories in
        ///     <paramref name="this" />.
        /// </param>
        /// <param name="searchOption">
        ///     One of the enumeration values that specifies whether the search operation should
        ///     include only the current directory or should include all subdirectories.The default value is
        ///     <see cref="F:System.IO.SearchOption.TopDirectoryOnly" />
        /// </param>
        /// <param name="predicate">The predicate.</param>
        /// <returns>
        ///     An enumerable collection of the full names (including paths) for the directories in the directory specified by
        ///     <paramref name="this" />
        ///     and that match the specified search pattern and option.
        /// </returns>
        public static DirectoryInfo[] GetDirectoriesWhere(this DirectoryInfo @this, string searchPattern, SearchOption searchOption, Func<DirectoryInfo, bool> predicate)
        {
            return Directory.EnumerateDirectories(@this.FullName, searchPattern, searchOption).Select(x => new DirectoryInfo(x)).Where(predicate).ToArray();
        }

        /// <summary>
        ///     Returns an enumerable collection of directory names that match a search pattern in a specified @this.
        /// </summary>
        /// <param name="this">The directory to search.</param>
        /// <param name="searchPatterns">The search string to match against the names of directories in.</param>
        /// <param name="predicate">The predicate.</param>
        /// <returns>
        ///     An enumerable collection of the full names (including paths) for the directories in the directory specified by
        ///     <paramref name="this" />
        ///     and that match the specified search pattern.
        /// </returns>
        public static DirectoryInfo[] GetDirectoriesWhere(this DirectoryInfo @this, string[] searchPatterns, Func<DirectoryInfo, bool> predicate)
        {
            return searchPatterns.SelectMany(@this.GetDirectories).Distinct().Where(predicate).ToArray();
        }

        /// <summary>
        ///     Returns an enumerable collection of directory names that match a search pattern in a specified @this, and
        ///     optionally searches subdirectories.
        /// </summary>
        /// <param name="this">The directory to search.</param>
        /// <param name="searchPatterns">
        ///     The search string to match against the names of directories in
        ///     <paramref name="this" />.
        /// </param>
        /// <param name="searchOption">
        ///     One of the enumeration values that specifies whether the search operation should
        ///     include only the current directory or should include all subdirectories.The default value is
        ///     <see cref="F:System.IO.SearchOption.TopDirectoryOnly" />
        /// </param>
        /// <param name="predicate">The predicate.</param>
        /// <returns>
        ///     An enumerable collection of the full names (including paths) for the directories in the directory specified by
        ///     <paramref name="this" />
        ///     and that match the specified search pattern and option.
        /// </returns>
        public static DirectoryInfo[] GetDirectoriesWhere(this DirectoryInfo @this, IEnumerable<string> searchPatterns, SearchOption searchOption, Func<DirectoryInfo, bool> predicate)
        {
            return searchPatterns.SelectMany(x => @this.GetDirectories(x, searchOption)).Distinct().Where(predicate).ToArray();
        }

        #endregion GetDirectoriesWhere

        #region GetFiles

        /// <summary>
        ///     Returns an enumerable collection of file names that match a search pattern in a specified @this.
        /// </summary>
        /// <param name="this">The directory to search.</param>
        /// <param name="searchPatterns">The search string to match against the names of directories in.</param>
        /// <returns>
        ///     An enumerable collection of the full names (including paths) for the files in the directory specified by
        ///     <paramref name="this" />
        ///     and that match the specified search pattern.
        /// </returns>
        public static FileInfo[] GetFiles(this DirectoryInfo @this, string[] searchPatterns)
        {
            return searchPatterns.SelectMany(@this.GetFiles).Distinct().ToArray();
        }

        /// <summary>
        ///     Returns an enumerable collection of file names that match a search pattern in a specified @this, and
        ///     optionally searches subdirectories.
        /// </summary>
        /// <param name="this">The directory to search.</param>
        /// <param name="searchPatterns">
        ///     The search string to match against the names of directories in
        ///     <paramref name="this" />.
        /// </param>
        /// <param name="searchOption">
        ///     One of the enumeration values that specifies whether the search operation should
        ///     include only the current directory or should include all subdirectories.The default value is
        ///     <see cref="F:System.IO.SearchOption.TopDirectoryOnly" />
        /// </param>
        /// <returns>
        ///     An enumerable collection of the full names (including paths) for the files in the directory specified by
        ///     <paramref name="this" />
        ///     and that match the specified search pattern and option.
        /// </returns>
        public static FileInfo[] GetFiles(this DirectoryInfo @this, IEnumerable<string> searchPatterns, SearchOption searchOption)
        {
            return searchPatterns.SelectMany(x => @this.GetFiles(x, searchOption)).Distinct().ToArray();
        }

        #endregion GetFiles

        #region GetFilesWhere

        /// <summary>
        ///     Returns an enumerable collection of file names in a specified @this.
        /// </summary>
        /// <param name="this">The directory to search.</param>
        /// <param name="predicate">The predicate.</param>
        /// <returns>
        ///     An enumerable collection of the full names (including paths) for the files in the directory specified by
        ///     <paramref name="this" />
        /// </returns>
        public static FileInfo[] GetFilesWhere(this DirectoryInfo @this, Func<FileInfo, bool> predicate)
        {
            return Directory.EnumerateFiles(@this.FullName).Select(x => new FileInfo(x)).Where(predicate).ToArray();
        }

        /// <summary>
        ///     Returns an enumerable collection of file names that match a search pattern in a specified @this.
        /// </summary>
        /// <param name="this">The directory to search.</param>
        /// <param name="searchPattern">
        ///     The search string to match against the names of directories in
        ///     <paramref name="this" />.
        /// </param>
        /// <param name="predicate">The predicate.</param>
        /// <returns>
        ///     An enumerable collection of the full names (including paths) for the files in the directory specified by
        ///     <paramref name="this" />
        ///     and that match the specified search pattern.
        /// </returns>
        public static FileInfo[] GetFilesWhere(this DirectoryInfo @this, string searchPattern, Func<FileInfo, bool> predicate)
        {
            return Directory.EnumerateFiles(@this.FullName, searchPattern).Select(x => new FileInfo(x)).Where(predicate).ToArray();
        }

        /// <summary>
        ///     Returns an enumerable collection of file names that match a search pattern in a specified @this, and
        ///     optionally searches subdirectories.
        /// </summary>
        /// <param name="this">The directory to search.</param>
        /// <param name="searchPattern">
        ///     The search string to match against the names of directories in
        ///     <paramref name="this" />.
        /// </param>
        /// <param name="searchOption">
        ///     One of the enumeration values that specifies whether the search operation should
        ///     include only the current directory or should include all subdirectories.The default value is
        ///     <see cref="F:System.IO.SearchOption.TopDirectoryOnly" />
        /// </param>
        /// <param name="predicate">The predicate.</param>
        /// <returns>
        ///     An enumerable collection of the full names (including paths) for the files in the directory specified by
        ///     <paramref name="this" />
        ///     and that match the specified search pattern and option.
        /// </returns>
        public static FileInfo[] GetFilesWhere(this DirectoryInfo @this, string searchPattern, SearchOption searchOption, Func<FileInfo, bool> predicate)
        {
            return Directory.EnumerateFiles(@this.FullName, searchPattern, searchOption).Select(x => new FileInfo(x)).Where(predicate).ToArray();
        }

        /// <summary>
        ///     Returns an enumerable collection of file names that match a search pattern in a specified @this.
        /// </summary>
        /// <param name="this">The directory to search.</param>
        /// <param name="searchPatterns">The search string to match against the names of directories in.</param>
        /// <param name="predicate">The predicate.</param>
        /// <returns>
        ///     An enumerable collection of the full names (including paths) for the files in the directory specified by
        ///     <paramref name="this" />
        ///     and that match the specified search pattern.
        /// </returns>
        public static FileInfo[] GetFilesWhere(this DirectoryInfo @this, string[] searchPatterns, Func<FileInfo, bool> predicate)
        {
            return searchPatterns.SelectMany(@this.GetFiles).Distinct().Where(predicate).ToArray();
        }

        /// <summary>
        ///     Returns an enumerable collection of file names that match a search pattern in a specified @this, and
        ///     optionally searches subdirectories.
        /// </summary>
        /// <param name="this">The directory to search.</param>
        /// <param name="searchPatterns">
        ///     The search string to match against the names of directories in
        ///     <paramref name="this" />.
        /// </param>
        /// <param name="searchOption">
        ///     One of the enumeration values that specifies whether the search operation should
        ///     include only the current directory or should include all subdirectories.The default value is
        ///     <see cref="F:System.IO.SearchOption.TopDirectoryOnly" />
        /// </param>
        /// <param name="predicate">The predicate.</param>
        /// <returns>
        ///     An enumerable collection of the full names (including paths) for the files in the directory specified by
        ///     <paramref name="this" />
        ///     and that match the specified search pattern and option.
        /// </returns>
        public static FileInfo[] GetFilesWhere(this DirectoryInfo @this, string[] searchPatterns, SearchOption searchOption, Func<FileInfo, bool> predicate)
        {
            return searchPatterns.SelectMany(x => @this.GetFiles(x, searchOption)).Distinct().Where(predicate).ToArray();
        }

        #endregion GetFilesWhere

        #region GetFileSystemEntries

        /// <summary>
        ///     Returns an enumerable collection of file-system entries in a specified @this.
        /// </summary>
        /// <param name="this">The directory to search.</param>
        /// <returns>
        ///     An enumerable collection of file-system entries in the directory specified by <paramref name="this" />.
        /// </returns>
        public static string[] GetFileSystemEntries(this DirectoryInfo @this)
        {
            return Directory.EnumerateFileSystemEntries(@this.FullName).ToArray();
        }

        /// <summary>
        ///     Returns an enumerable collection of file-system entries that match a search pattern in a specified @this.
        /// </summary>
        /// <param name="this">The directory to search.</param>
        /// <param name="searchPattern">
        ///     The search string to match against the names of directories in
        ///     <paramref name="this" />.
        /// </param>
        /// <returns>
        ///     An enumerable collection of file-system entries in the directory specified by <paramref name="this" /> and hat match the specified search pattern.
        /// </returns>
        public static string[] GetFileSystemEntries(this DirectoryInfo @this, string searchPattern)
        {
            return Directory.EnumerateFileSystemEntries(@this.FullName, searchPattern).ToArray();
        }

        /// <summary>
        ///     Returns an enumerable collection of file names and directory names that match a search pattern in a specified
        ///     @this, and optionally searches subdirectories.
        /// </summary>
        /// <param name="this">The directory to search.</param>
        /// <param name="searchPattern">
        ///     The search string to match against the names of directories in
        ///     <paramref name="this" />.
        /// </param>
        /// <param name="searchOption">
        ///     One of the enumeration values  that specifies whether the search operation should
        ///     include only the current directory or should include all subdirectories.The default value is
        ///     <see cref="F:System.IO.SearchOption.TopDirectoryOnly" />
        /// </param>
        /// <returns>
        ///     An enumerable collection of file-system entries in the directory specified by <paramref name="this" /> and that match the specified search pattern and option.
        /// </returns>
        public static string[] GetFileSystemEntries(this DirectoryInfo @this, string searchPattern, SearchOption searchOption)
        {
            return Directory.EnumerateFileSystemEntries(@this.FullName, searchPattern, searchOption).ToArray();
        }

        /// <summary>
        ///     Returns an enumerable collection of file-system entries that match a search pattern in a specified @this.
        /// </summary>
        /// <param name="this">The directory to search.</param>
        /// <param name="searchPatterns">The search string to match against the names of directories in.</param>
        /// <returns>
        ///     An enumerable collection of file-system entries in the directory specified by <paramref name="this" /> and that match the specified search pattern.
        /// </returns>
        public static string[] GetFileSystemEntries(this DirectoryInfo @this, IEnumerable<string> searchPatterns)
        {
            return searchPatterns.SelectMany(x => Directory.EnumerateFileSystemEntries(@this.FullName, x)).Distinct().ToArray();
        }

        /// <summary>
        ///     Returns an enumerable collection of file names and directory names that match a search pattern in a specified
        ///     @this, and optionally searches subdirectories.
        /// </summary>
        /// <param name="this">The directory to search.</param>
        /// <param name="searchPatterns">
        ///     The search string to match against the names of directories in
        ///     <paramref name="this" />.
        /// </param>
        /// <param name="searchOption">
        ///     One of the enumeration values  that specifies whether the search operation should
        ///     include only the current directory or should include all subdirectories.The default value is
        ///     <see cref="F:System.IO.SearchOption.TopDirectoryOnly" />
        /// </param>
        /// <returns>
        ///     An enumerable collection of file-system entries in the directory specified by <paramref name="this" /> and that match the specified search pattern and option.
        /// </returns>
        public static string[] GetFileSystemEntries(this DirectoryInfo @this, IEnumerable<string> searchPatterns, SearchOption searchOption)
        {
            return searchPatterns.SelectMany(x => Directory.EnumerateFileSystemEntries(@this.FullName, x, searchOption)).Distinct().ToArray();
        }

        #endregion GetFileSystemEntries

        #region GetFileSystemEntriesWhere

        /// <summary>
        ///     Returns an enumerable collection of file-system entries in a specified @this.
        /// </summary>
        /// <param name="this">The directory to search.</param>
        /// <param name="predicate">The predicate.</param>
        /// <returns>
        ///     An enumerable collection of file-system entries in the directory specified by <paramref name="this" />.
        /// </returns>
        public static string[] GetFileSystemEntriesWhere(this DirectoryInfo @this, Func<string, bool> predicate)
        {
            return Directory.EnumerateFileSystemEntries(@this.FullName).Where(predicate).ToArray();
        }

        /// <summary>
        ///     Returns an enumerable collection of file-system entries that match a search pattern in a specified @this.
        /// </summary>
        /// <param name="this">The directory to search.</param>
        /// <param name="searchPattern">
        ///     The search string to match against the names of directories in
        ///     <paramref name="this" />.
        /// </param>
        /// <param name="predicate">The predicate.</param>
        /// <returns>
        ///     An enumerable collection of file-system entries in the directory specified by <paramref name="this" /> and that match the specified search pattern.
        /// </returns>
        public static string[] GetFileSystemEntriesWhere(this DirectoryInfo @this, string searchPattern, Func<string, bool> predicate)
        {
            return Directory.EnumerateFileSystemEntries(@this.FullName, searchPattern).Where(predicate).ToArray();
        }

        /// <summary>
        ///     Returns an enumerable collection of file names and directory names that match a search pattern in a specified
        ///     @this, and optionally searches subdirectories.
        /// </summary>
        /// <param name="this">The directory to search.</param>
        /// <param name="searchPattern">
        ///     The search string to match against the names of directories in
        ///     <paramref name="this" />.
        /// </param>
        /// <param name="searchOption">
        ///     One of the enumeration values  that specifies whether the search operation should
        ///     include only the current directory or should include all subdirectories.The default value is
        ///     <see cref="F:System.IO.SearchOption.TopDirectoryOnly" />
        /// </param>
        /// <param name="predicate">The predicate.</param>
        /// <returns>
        ///     An enumerable collection of file-system entries in the directory specified by <paramref name="this" /> and that match the specified search pattern and option.
        /// </returns>
        public static string[] GetFileSystemEntriesWhere(this DirectoryInfo @this, string searchPattern, SearchOption searchOption, Func<string, bool> predicate)
        {
            return Directory.EnumerateFileSystemEntries(@this.FullName, searchPattern, searchOption).Where(predicate).ToArray();
        }

        /// <summary>
        ///     Returns an enumerable collection of file-system entries that match a search pattern in a specified @this.
        /// </summary>
        /// <param name="this">The directory to search.</param>
        /// <param name="searchPatterns">The search string to match against the names of directories in.</param>
        /// <param name="predicate">The predicate.</param>
        /// <returns>
        ///     An enumerable collection of file-system entries in the directory specified by <paramref name="this" /> and that match the specified search pattern.
        /// </returns>
        public static string[] GetFileSystemEntriesWhere(this DirectoryInfo @this, IEnumerable<string> searchPatterns, Func<string, bool> predicate)
        {
            return searchPatterns.SelectMany(x => Directory.EnumerateFileSystemEntries(@this.FullName, x)).Distinct().Where(predicate).ToArray();
        }

        /// <summary>
        ///     Returns an enumerable collection of file names and directory names that match a search pattern in a specified
        ///     @this, and optionally searches subdirectories.
        /// </summary>
        /// <param name="this">The directory to search.</param>
        /// <param name="searchPatterns">
        ///     The search string to match against the names of directories in
        ///     <paramref name="this" />.
        /// </param>
        /// <param name="searchOption">
        ///     One of the enumeration values  that specifies whether the search operation should
        ///     include only the current directory or should include all subdirectories.The default value is
        ///     <see cref="F:System.IO.SearchOption.TopDirectoryOnly" />
        /// </param>
        /// <param name="predicate">The predicate.</param>
        /// <returns>
        ///     An enumerable collection of file-system entries in the directory specified by <paramref name="this" /> and that match the specified search pattern and option.
        /// </returns>
        public static string[] GetFileSystemEntriesWhere(this DirectoryInfo @this, IEnumerable<string> searchPatterns, SearchOption searchOption, Func<string, bool> predicate)
        {
            return searchPatterns.SelectMany(x => Directory.EnumerateFileSystemEntries(@this.FullName, x, searchOption)).Distinct().Where(predicate).ToArray();
        }

        #endregion GetFileSystemEntriesWhere

        #region GetSize

        /// <summary>
        ///     A DirectoryInfo extension method that gets a size.
        /// </summary>
        /// <param name="this">The @this to act on.</param>
        /// <returns>The size.</returns>
        public static long GetSize(this DirectoryInfo @this)
        {
            return @this.GetFiles("*.*", SearchOption.AllDirectories).Sum(x => x.Length);
        }

        #endregion GetSize

        #region PathCombine

        /// <summary>
        ///     Combines multiples string into a path.
        /// </summary>
        /// <param name="this">The @this to act on.</param>
        /// <param name="paths">A variable-length parameters list containing paths.</param>
        /// <returns>
        ///     The combined paths. If one of the specified paths is a zero-length string, this method returns the other path.
        /// </returns>
        public static string PathCombine(this DirectoryInfo @this, params string[] paths)
        {
            var list = paths.ToList();
            list.Insert(0, @this.FullName);
            return Path.Combine(list.ToArray());
        }

        #endregion PathCombine

        #region PathCombineDirectory

        /// <summary>
        ///     Combines multiples string into a path.
        /// </summary>
        /// <param name="this">The @this to act on.</param>
        /// <param name="paths">A variable-length parameters list containing paths.</param>
        /// <returns>
        ///     The combined paths as a DirectoryInfo. If one of the specified paths is a zero-length string, this method
        ///     returns the other path.
        /// </returns>
        public static DirectoryInfo PathCombineDirectory(this DirectoryInfo @this, params string[] paths)
        {
            var list = paths.ToList();
            list.Insert(0, @this.FullName);
            return new DirectoryInfo(Path.Combine(list.ToArray()));
        }

        #endregion PathCombineDirectory

        #region PathCombineFile

        /// <summary>
        ///     Combines multiples string into a path.
        /// </summary>
        /// <param name="this">The @this to act on.</param>
        /// <param name="paths">A variable-length parameters list containing paths.</param>
        /// <returns>
        ///     The combined paths as a FileInfo. If one of the specified paths is a zero-length string, this method returns
        ///     the other path.
        /// </returns>
        public static FileInfo PathCombineFile(this DirectoryInfo @this, params string[] paths)
        {
            var list = paths.ToList();
            list.Insert(0, @this.FullName);
            return new FileInfo(Path.Combine(list.ToArray()));
        }

        #endregion PathCombineFile

        #endregion DirectoryInfo

        #region FileInfo

        #region AppendAllLines

        /// <summary>
        ///     A FileInfo extension method that appends all lines.
        /// </summary>
        /// <param name="this">The @this to act on.</param>
        /// <param name="contents">The contents.</param>
        public static void AppendAllLines(this FileInfo @this, IEnumerable<string> contents)
        {
            File.AppendAllLines(@this.FullName, contents);
        }

        /// <summary>
        ///     A FileInfo extension method that appends all lines.
        /// </summary>
        /// <param name="this">The @this to act on.</param>
        /// <param name="contents">The contents.</param>
        /// <param name="encoding">The encoding.</param>
        public static void AppendAllLines(this FileInfo @this, IEnumerable<string> contents, Encoding encoding)
        {
            File.AppendAllLines(@this.FullName, contents, encoding);
        }

        #endregion



        #endregion
    }
}