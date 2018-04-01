using System.Security.Cryptography;
using System.Text;

namespace Lenneth.Core.Framework.Hash
{
    /// <summary>
    /// MD5辅助类
    /// </summary>
    internal sealed class MD5Hash: Hash
    {
        /// <summary>
        /// 计算字符串MD5散列
        /// </summary>
        /// <param name="input">输入字符串</param>
        /// <returns>MD5字符串</returns>
        public override string ComputeHash(string input)
        {
            string result;
            using (var md5 = new MD5CryptoServiceProvider())
            {
                var data = md5.ComputeHash(Encoding.UTF8.GetBytes(input));
                var builder = new StringBuilder();
                foreach (var b in data)
                {
                    builder.Append(b.ToString("x2"));
                }
                result = builder.ToString();
            }
            return result;
        }
    }
}