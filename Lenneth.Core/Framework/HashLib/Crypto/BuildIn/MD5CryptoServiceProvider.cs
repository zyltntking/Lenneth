namespace Lenneth.Core.Framework.HashLib.Crypto.BuildIn
{
    internal sealed class Md5CryptoServiceProvider : HashCryptoBuildIn, IHasHMACBuildIn
    {
        public Md5CryptoServiceProvider()
            : base(new System.Security.Cryptography.MD5CryptoServiceProvider(), 64)
        {
        }

        public System.Security.Cryptography.HMAC GetBuildHMAC()
        {
            return new System.Security.Cryptography.HMACMD5();
        }
    }
}