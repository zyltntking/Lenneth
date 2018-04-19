using System;

namespace Lenneth.Core.Extensions.Extra.EnvironmentExtensions
{
    public static class Extensions
    {
        #region GetFolderPath

        /// <summary>An Environment.SpecialFolder extension method that gets folder path.</summary>
        /// <param name="this">this.</param>
        /// <returns>The folder path.</returns>
        public static string GetFolderPath(this Environment.SpecialFolder @this)
        {
            return Environment.GetFolderPath(@this);
        }

        /// <summary>An Environment.SpecialFolder extension method that gets folder path.</summary>
        /// <param name="this">this.</param>
        /// <param name="option">The option.</param>
        /// <returns>The folder path.</returns>
        public static string GetFolderPath(this Environment.SpecialFolder @this, Environment.SpecialFolderOption option)
        {
            return Environment.GetFolderPath(@this, option);
        }

        #endregion GetFolderPath
    }
}