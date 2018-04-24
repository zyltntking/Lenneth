using System.Data;

namespace Lenneth.Core.Extensions.Extra.DataExtensions
{
    public static partial class Extensions
    {
        #region AddRange

        /// <summary>
        ///     A DataColumnCollection extension method that adds a range to 'columns'.
        /// </summary>
        /// <param name="this">The @this to act on.</param>
        /// <param name="columns">A variable-length parameters list containing columns.</param>
        public static void AddRange(this DataColumnCollection @this, params string[] columns)
        {
            foreach (var column in columns)
            {
                @this.Add(column);
            }
        }

        #endregion AddRange
    }
}