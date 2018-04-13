namespace Lenneth.Core.Framework.FastDFS.Utility
{
    internal interface IAes
    {
        /// <summary>
        /// 加密数据
        /// </summary>
        /// <param name="data">原始数据</param>
        /// <returns></returns>
        byte[] Encrypt(byte[] data);

        /// <summary>
        /// 解密数据
        /// </summary>
        /// <param name="data">密文数据</param>
        /// <returns></returns>
        byte[] Decrypt(byte[] data);
    }
}