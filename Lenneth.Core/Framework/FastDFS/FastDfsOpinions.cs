using Lenneth.Core.Framework.FastDFS.Utility;

namespace Lenneth.Core.Framework.FastDFS
{
    public sealed class FastDfsOpinions
    {
        /// <summary>
        /// The endpoints defined for this configuration
        /// </summary>
        public EndPointCollection EndPoints { get;  } = new EndPointCollection();

        /// <summary>
        /// The groupname defined for this configuration
        /// </summary>
        public string GroupName { get; set; } = "group1";

        /// <summary>
        /// The PassWord for cryptor
        /// </summary>
        public string PassWord { get; set; } = "";
    }
}