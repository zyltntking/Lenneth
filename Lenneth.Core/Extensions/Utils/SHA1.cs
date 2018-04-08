using System;

namespace Lenneth.Core.Extensions.Utils
{
    public static class Sha1
    {
        /// <summary>
        /// Calculate SHA1
        /// </summary>
        /// <param name="aFilePath"></param>
        /// <returns></returns>
        public static string Calculate(string aFilePath)
        {
            return Calculate(FileUtils.ReadFile(aFilePath));
        }

        /// <summary>
        /// Calculate SHA1
        /// </summary>
        /// <param name="aData"></param>
        /// <returns></returns>
        public static string Calculate(byte[] aData)
        {
            using (var hasher = System.Security.Cryptography.SHA1.Create())
            {
                byte[] hash = hasher.ComputeHash(aData);
                return BitConverter.ToString(hash).ToUpper().Replace("-", "");
            }
        }
    }
}