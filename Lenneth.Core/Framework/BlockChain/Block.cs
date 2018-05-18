using System;

namespace Lenneth.Core.Framework.BlockChain
{
    /// <summary>
    /// 区块单元
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public class Block<T>
    {
        /// <summary>
        /// 构造Block
        /// </summary>
        /// <param name="index">Block位置</param>
        /// <param name="prevHash">前一Block签名</param>
        public Block(int index, string prevHash)
        {
            Header = new Header
            {
                Index = index,
                PreviousHash = prevHash,
                Hash = null
            };
            Header.Hash = CalculateHash;
        }

        /// <summary>
        /// 区块头
        /// </summary>
        public Header Header { get; }

        /// <summary>
        /// 生成时间戳
        /// </summary>
        public DateTime TimeStamp { get; } = DateTime.Now;

        /// <summary>
        /// 区块随机标识
        /// </summary>
        public Guid Nonce { get; } = Guid.NewGuid();

        /// <summary>
        /// 区块数据
        /// </summary>
        public T Data { get; set; }

        /// <summary>
        /// 计算当前块散列值
        /// </summary>
        private string CalculateHash
        {
            get
            {
                var cube = new HashCube<T>(this);
                return cube.Hash;
            }
        }
    }
}