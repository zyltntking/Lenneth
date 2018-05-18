using Newtonsoft.Json;
using System;

namespace Lenneth.Core.Framework.BlockChain
{
    /// <summary>
    /// 散列方
    /// </summary>
    internal class HashCube<T>
    {
        public HashCube(Block<T> block)
        {
            Index = block.Header.Index;
            TimeStamp = block.TimeStamp;
            Nonce = block.Nonce;
            PreviousHash = block.Header.PreviousHash;
        }

        /// <summary>
        /// 位置
        /// </summary>
        private int Index { get; }

        /// <summary>
        /// 时间戳
        /// </summary>
        private DateTime TimeStamp { get; }

        /// <summary>
        /// 块随机标识
        /// </summary>
        private Guid Nonce { get; }

        /// <summary>
        /// 前一块散列值
        /// </summary>
        private string PreviousHash { get; }

        /// <summary>
        /// 计算cube Hash
        /// </summary>
        /// <returns></returns>
        public string Hash
        {
            get
            {
                var cube = new
                {
                    Index,
                    TimeStamp,
                    Nonce,
                    PreviousHash
                };

                var json = JsonConvert.SerializeObject(cube);

                return json.Sha256Hash();
            }
        }
    }
}