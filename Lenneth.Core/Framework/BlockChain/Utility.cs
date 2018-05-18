using System.Security.Cryptography;
using System.Text;

namespace Lenneth.Core.Framework.BlockChain
{
    internal static class Utility
    {
        public static string Sha256Hash(this string str)
        {
            using (var provider = new SHA256CryptoServiceProvider())
            {
                var data = provider.ComputeHash(Encoding.UTF8.GetBytes(str));
                var builder = new StringBuilder();
                foreach (var b in data)
                {
                    builder.Append(b.ToString("x2"));
                }
                return builder.ToString();
            }
        }
    }
}