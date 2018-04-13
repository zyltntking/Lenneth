using System;
using System.IO;
using System.Security.Cryptography;
using System.Text;

namespace Lenneth.Core.Framework.Crypt
{
    /// <inheritdoc />
    /// <summary>
    /// 3DES加密类
    /// </summary>
    internal sealed class TripleDes : Crypt
    {
        /// <summary>
        /// 密钥
        /// </summary>
        private readonly string _key;

        /// <summary>
        /// 加密密钥
        /// </summary>
        private byte[] Key
        {
            get
            {
                using (var md5 = new MD5CryptoServiceProvider())
                {
                    var data = md5.ComputeHash(Encoding.UTF8.GetBytes(_key));
                    var builder = new StringBuilder();
                    for (var index = 0; index < data.Length; index++)
                    {
                        if (index % 4 != 0)
                        {
                            builder.Append(data[index].ToString("x2"));
                        }
                    }

                    return Encoding.ASCII.GetBytes(builder.ToString());
                }
            }
        }

        /// <summary>
        /// 加密向量
        /// </summary>
        /// Valkyrie
        /// GenerateIV()
        private byte[] Iv { get; set; } = { 0x56, 0x61, 0x6C, 0x6B, 0x79, 0x72, 0x69, 0x65 };

        /// <summary>
        /// 构造函数
        /// </summary>
        /// <param name="key">加密密钥</param>
        public TripleDes(string key = null)
        {
            _key = string.IsNullOrWhiteSpace(key) ? "Lenneth" : key;
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
                using (var dcsp = new TripleDESCryptoServiceProvider())
                {
                    dcsp.Padding = PaddingMode.PKCS7;
                    var encryptor = dcsp.CreateEncryptor(Key, Iv);
                    var msEncrypt = new MemoryStream();
                    var csEncrypt = new CryptoStream(msEncrypt, encryptor, CryptoStreamMode.Write);
                    using (var swEncrypt = new StreamWriter(csEncrypt))
                    {
                        swEncrypt.Write(encryptString);
                    }
                    var encrypted = msEncrypt.ToArray();
                    result = Convert.ToBase64String(encrypted);
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
                using (var dcsp = new TripleDESCryptoServiceProvider())
                {
                    dcsp.Padding = PaddingMode.PKCS7;
                    var decryptor = dcsp.CreateDecryptor(Key, Iv);
                    var msDecrypt = new MemoryStream(decryptByteArray);
                    var csDecrypt = new CryptoStream(msDecrypt, decryptor, CryptoStreamMode.Read);
                    using (var srDecrypt = new StreamReader(csDecrypt))
                    {
                        result = srDecrypt.ReadToEnd();
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