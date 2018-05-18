using System.Security.Cryptography;
using System.Text;

namespace Lenneth.WebApi.Core.Hash
{
    /// <summary>
    /// SHA384散列类
    /// </summary>
    internal sealed class Sha384Hash : Hash
    {
        /// <summary>
        /// 计算字符串SHA384散列值
        /// </summary>
        /// <param name="input">输入字符串</param>
        /// <returns></returns>
        public override string ComputeHash(string input)
        {
            string result;
            using (var sha = new SHA384CryptoServiceProvider())
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