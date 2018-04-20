using System;

namespace Lenneth.Core.Extensions.Extra.CoreExtensions.UIntPtrExtensions
{
    public static class Extensions
    {
        #region Add

        /// <summary>
        ///     Adds an offset to the value of an unsigned pointer.
        /// </summary>
        /// <param name="pointer">The unsigned pointer to add the offset to.</param>
        /// <param name="offset">The offset to add.</param>
        /// <returns>A new unsigned pointer that reflects the addition of  to .</returns>
        public static UIntPtr Add(this UIntPtr pointer, int offset)
        {
            return UIntPtr.Add(pointer, offset);
        }

        #endregion Add

        #region Subtract

        /// <summary>
        ///     Subtracts an offset from the value of an unsigned pointer.
        /// </summary>
        /// <param name="pointer">The unsigned pointer to subtract the offset from.</param>
        /// <param name="offset">The offset to subtract.</param>
        /// <returns>A new unsigned pointer that reflects the subtraction of  from .</returns>
        public static UIntPtr Subtract(this UIntPtr pointer, int offset)
        {
            return UIntPtr.Subtract(pointer, offset);
        }

        #endregion Subtract
    }
}