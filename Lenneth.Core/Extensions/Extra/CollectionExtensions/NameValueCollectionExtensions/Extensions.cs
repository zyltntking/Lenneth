using System.Collections.Generic;

namespace Lenneth.Core.Extensions.Extra.CollectionExtensions
{
    public static partial class Extensions
    {
        #region NameValueCollection

        #region ToDictionary

        /// <summary>
        ///     A NameValueCollection extension method that converts the @this to a dictionary.
        /// </summary>
        /// <param name="this">The @this to act on.</param>
        /// <returns>@this as an IDictionary&lt;string,object&gt;</returns>
        public static IDictionary<string, object> ToDictionary(this System.Collections.Specialized.NameValueCollection @this)
        {
            var dict = new Dictionary<string, object>();

            if (@this != null)
            {
                foreach (var key in @this.AllKeys)
                {
                    dict.Add(key, @this[key]);
                }
            }

            return dict;
        }

        #endregion ToDictionary

        #endregion NameValueCollection
    }
}