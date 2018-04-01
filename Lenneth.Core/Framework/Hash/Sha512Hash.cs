using System.Security.Cryptography;
using System.Text;

namespace Lenneth.Core.Framework.Hash
{
    /// <summary>
    /// SHA512散列类
    /// </summary>
    internal sealed class Sha512Hash : Hash
    {
        /// <summary>
        /// 计算字符串SHA256散列值
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        public override string ComputeHash(string input)
        {
            string result;
            using (var sha = new SHA512CryptoServiceProvider())
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