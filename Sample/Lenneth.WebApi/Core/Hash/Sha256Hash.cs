using System.Security.Cryptography;
using System.Text;

namespace Lenneth.WebApi.Core.Hash
{
    /// <summary>
    /// SHA256散列类
    /// </summary>
    internal sealed class Sha256Hash : Hash
    {
        /// <summary>
        /// 计算SHA256散列值
        /// </summary>
        /// <param name="input">输入字符串</param>
        /// <returns></returns>
        public override string ComputeHash(string input)
        {
            string result;
            using (var sha = new SHA256CryptoServiceProvider())
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