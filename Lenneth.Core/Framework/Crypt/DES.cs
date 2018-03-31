using System;
using System.IO;
using System.Security.Cryptography;
using System.Text;

namespace Lenneth.Core.Framework.Crypt
{
    /// <inheritdoc />
    /// <summary>
    /// DES加密类
    /// </summary>
    internal sealed class DES : Crypt
    {
        /// <inheritdoc />
        /// <summary>
        /// 加密密钥
        /// </summary>
        private string Key { get; set; } = "@Lenneth";

        /// <summary>
        /// 加密向量
        /// </summary>
        /// Valkyrie
        private byte[] Iv { get; set; } = { 0x56, 0x61, 0x6C, 0x6B, 0x79, 0x72, 0x69, 0x65 };

        /// <summary>
        /// 初始化加密对象
        /// </summary>
        /// <param name="key">密钥</param>
        /// <param name="iv">位移向量</param>
        public void InitCrypt(string key, byte[] iv)
        {
            Key = key;
            Iv = iv;
        }

        /// <summary>
        /// 构造函数
        /// </summary>
        /// <param name="key">加密密钥</param>
        /// <param name="iv">加密向量</param>
        public DES(string key = null, string iv = null)
        {
            if (key != null && key.Length == 8)
            {
                Key = key;
            }
            if (iv != null && iv.Length == 8)
            {
                Iv = Encoding.UTF8.GetBytes(iv);
            }
        }

        /// <inheritdoc />
        /// <summary>
        /// 加密
        /// </summary>
        /// <param name="encryptString">明文</param>
        /// <returns>密文</returns>
        public override string Encrypt(string encryptString)
        {
            string result;
            try
            {
                
                using (var dcsp = new DESCryptoServiceProvider())
                {
                    dcsp.Padding = PaddingMode.PKCS7;
                    var encryptor = dcsp.CreateEncryptor(Encoding.UTF8.GetBytes(Key), Iv);
                    using (var msEncrypt = new MemoryStream())
                    {
                        using (var csEncrypt = new CryptoStream(msEncrypt, encryptor, CryptoStreamMode.Write))
                        {
                            using (var swEncrypt = new StreamWriter(csEncrypt))
                            {
                                swEncrypt.Write(encryptString);
                            }
                            var encrypted = msEncrypt.ToArray();
                            result = Convert.ToBase64String(encrypted);
                        }
                    }
                }
            }
            catch
            {
                result = encryptString;
            }
            return result;
        }

        /// <inheritdoc />
        /// <summary>
        /// 解密
        /// </summary>
        /// <param name="decryptString">密文</param>
        /// <returns>明文</returns>
        public override string Decrypt(string decryptString)
        {
            string result;
            var decryptByteArray = Convert.FromBase64String(decryptString);
            try
            {
                using (var dcsp = new DESCryptoServiceProvider())
                {
                    dcsp.Padding = PaddingMode.PKCS7;
                    var decryptor = dcsp.CreateDecryptor(Encoding.UTF8.GetBytes(Key), Iv);
                    using (var msDecrypt = new MemoryStream(decryptByteArray))
                    {
                        using (var csDecrypt = new CryptoStream(msDecrypt, decryptor, CryptoStreamMode.Read))
                        {
                            using (var srDecrypt = new StreamReader(csDecrypt))
                            {
                                result = srDecrypt.ReadToEnd();
                            }
                        }
                    }
                }
            }
            catch
            {
                result = decryptString;
            }

            return result;
        }
    }
}
