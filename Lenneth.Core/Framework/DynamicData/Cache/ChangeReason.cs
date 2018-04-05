﻿
namespace Lenneth.Core.Framework.DynamicData
{
    /// <summary>
    ///  The reason for an individual change.  
    /// 
    /// Used to signal consumers of any changes to the underlying cache
    /// </summary>
    public enum ChangeReason
    {
        /// <summary>
        ///  An item has been added
        /// </summary>
        Add = 0,

        /// <summary>
        ///  An item has been updated
        /// </summary>
        Update =1,

        /// <summary>
        ///  An item has removed
        /// </summary>
        Remove =2,

        /// <summary>
        /// Downstream operators will refresh
        /// </summary>
        Refresh =3,

        /// <summary>
        /// An item has been moved in a sorted collection
        /// </summary>
        Moved = 4,
    }
}
