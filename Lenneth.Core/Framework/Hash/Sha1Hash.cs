using System.Security.Cryptography;
using System.Text;

namespace Lenneth.Core.Framework.Hash
{
    /// <summary>
    /// SHA1散列类
    /// </summary>
    internal sealed class Sha1Hash : Hash
    {
        /// <summary>
        /// 计算字符串SHA1散列值
        /// </summary>
        /// <param name="input">输入字符串</param>
        /// <returns></returns>
        public override string ComputeHash(string input)
        {
            string result;
            using (var sha = new SHA1CryptoServiceProvider())
            {
                var data = sha.ComputeHash(Encoding.UTF8.GetBytes(input));
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