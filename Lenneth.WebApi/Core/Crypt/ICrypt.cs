using System;

namespace Lenneth.WebApi.Core.Crypt
{
    /// <summary>
    /// 加密接口
    /// </summary>
    public interface ICrypt
    {
        /// <summary>
        /// 加密
        /// </summary>
        /// <param name="encryptString">明文</param>
        /// <returns>密文</returns>
        string Encrypt(string encryptString);

        /// <summary>
        /// 解密
        /// </summary>
        /// <param name="decryptString">密文</param>
        /// <returns>明文</returns>
        string Decrypt(string decryptString);
    }
}
