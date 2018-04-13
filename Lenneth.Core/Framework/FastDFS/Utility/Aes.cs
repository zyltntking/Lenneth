using System.IO;
using System.Security.Cryptography;
using System.Text;

namespace Lenneth.Core.Framework.FastDFS.Utility
{
    internal class Aes : IAes
    {
        #region Property & Constructor

        private readonly byte[] _iv = { 52, 124, 218, 199, 49, 102, 202, 39, 98, 217, 90, 254, 234, 149, 174, 100 };

        private readonly string _key;

        private byte[] Iv => _iv;

        private byte[] Key
        {
            get
            {
                using (var md5 = MD5.Create())
                {
                    var data = md5.ComputeHash(Encoding.UTF8.GetBytes(_key));

                    var sb = new StringBuilder();

                    for (var i = 0; i < data.Length; i++)
                    {
                        sb.Append(i.ToString("x2"));
                    }

                    return Encoding.ASCII.GetBytes(sb.ToString());
                }
            }
        }

        public Aes(string key)
        {
            _key = string.IsNullOrWhiteSpace(key) ? "" : key;
        }

        #endregion Property & Constructor

        #region Implementation of IAes

        /// <summary>
        /// 加密数据
        /// </summary>
        /// <param name="data">原始数据</param>
        /// <returns></returns>
        public byte[] Encrypt(byte[] data)
        {
            using (var aes = new AesCryptoServiceProvider())
            {
                var encryptor = aes.CreateEncryptor(Key, Iv);
                using (var msEncrypt = new MemoryStream())
                {
                    using (var csEncrypt = new CryptoStream(msEncrypt, encryptor, CryptoStreamMode.Write))
                    {
                        using (var swEncrypt = new BinaryWriter(csEncrypt))
                        {
                            swEncrypt.Write(data);
                        }
                        return msEncrypt.ToArray();
                    }
                }
            }
        }

        /// <summary>
        /// 解密数据
        /// </summary>
        /// <param name="data">密文数据</param>
        /// <returns></returns>
        public byte[] Decrypt(byte[] data)
        {
            using (var aes = new AesCryptoServiceProvider())
            {
                var decryptor = aes.CreateDecryptor(Key, Iv);
                using (var msDecrypt = new MemoryStream())
                {
                    using (var csDecrypt = new CryptoStream(msDecrypt, decryptor, CryptoStreamMode.Write))
                    {
                        using (var swDecrypt = new BinaryWriter(csDecrypt))
                        {
                            swDecrypt.Write(data);
                        }
                        return msDecrypt.ToArray();
                    }
                }
            }
        }

        #endregion Implementation of IAes
    }
}