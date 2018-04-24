using System;

namespace Lenneth.Core.Extensions.Extra.CoreExtensions
{
    public static partial class Extensions
    {
        #region Type

        #region GetTypeArray

        /// <summary>
        ///     Gets the types of the objects in the specified array.
        /// </summary>
        /// <param name="args">An array of objects whose types to determine.</param>
        /// <returns>An array of  objects representing the types of the corresponding elements in .</returns>
        public static Type[] GetTypeArray(this object[] args)
        {
            return Type.GetTypeArray(args);
        }

        #endregion GetTypeArray

        #endregion Type
    }
}