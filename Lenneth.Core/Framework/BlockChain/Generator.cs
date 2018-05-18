using System.Collections.Generic;

namespace Lenneth.Core.Framework.BlockChain
{
    public static class Generator<T>
    {
        public static List<Block<T>> BlockChain = new List<Block<T>>();

        /// <summary>
        /// 生成新区块
        /// </summary>
        /// <typeparam name="T">区块保存数据类型</typeparam>
        /// <param name="oblBlock">旧区块</param>
        /// <returns></returns>
        public static Block<T> GeneratorBlock(Block<T> oblBlock)
        {
            var index = oblBlock.Header.Index + 1;
            var prevHash = oblBlock.Header.Hash;
            return new Block<T>(index,prevHash);
        }

        /// <summary>
        /// 检验区块签名
        /// </summary>
        /// <param name="oldBlock">前一区块</param>
        /// <param name="newBlock">当前区块</param>
        /// <returns></returns>
        public static bool IsBlockValid(Block<T> oldBlock, Block<T> newBlock)
        {
            if (oldBlock.Header.Index + 1 != newBlock.Header.Index)
            {
                return false;
            }

            if (!string.Equals(oldBlock.Header.Hash,newBlock.Header.PreviousHash))
            {
                return false;
            }

            if (!string.Equals(new HashCube<T>(newBlock).Hash,newBlock.Header.Hash))
            {
                return false;
            }

            return true;
        }

        /// <summary>
        /// 切换链
        /// </summary>
        /// <param name="newBlockChain"></param>
        public static void SwitchChain(List<Block<T>> newBlockChain)
        {
            if (newBlockChain.Count > BlockChain.Count)
            {
                BlockChain = newBlockChain;
            }
        }
    }
}