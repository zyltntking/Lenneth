using System.Security.Cryptography;
using System.Text;

namespace Lenneth.Core.Framework.Hash
{
    /// <summary>
    /// RIPEMD160散列类
    /// </summary>
    sealed class RipEmd160Hash : Hash
    {
        /// <summary>
        /// 计算字符串RIPEMD160散列值
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        public override string ComputeHash(string input)
        {
            string result;
            using (var ripemd = RIPEMD160.Create())
            {
                var data = ripemd.ComputeHash(Encoding.UTF8.GetBytes(input));
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
