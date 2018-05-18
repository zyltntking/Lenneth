namespace Lenneth.WebApi.Core.Crypt
{
    /// <inheritdoc />
    /// <summary>
    /// 加密接口实现
    /// </summary>
    internal abstract class Crypt : ICrypt
    {
        /// <inheritdoc />
        /// <summary>
        /// 加密
        /// </summary>
        /// <param name="encryptString">明文</param>
        /// <returns>密文</returns>
        public virtual string Encrypt(string encryptString)
        {
            return encryptString;
        }

        /// <inheritdoc />
        /// <summary>
        /// 解密
        /// </summary>
        /// <param name="decryptString">密文</param>
        /// <returns>明文</returns>
        public virtual string Decrypt(string decryptString)
        {
            return decryptString;
        }
    }
}