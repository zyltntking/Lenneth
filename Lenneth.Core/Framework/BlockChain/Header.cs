namespace Lenneth.Core.Framework.BlockChain
{
    /// <summary>
    /// 区块头
    /// </summary>
    public class Header
    {
        /// <summary>
        /// 区块位置
        /// </summary>
        public int Index { get; set; }

        /// <summary>
        /// 区块散列
        /// </summary>
        public string Hash { get; set; }

        /// <summary>
        /// 前一区块散列
        /// </summary>
        public string PreviousHash { get; set; }
    }
}