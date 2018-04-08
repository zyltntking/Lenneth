namespace Lenneth.Core.Framework.HashLib.Crypto.BuildIn
{
    internal sealed class Sha1CryptoServiceProvider : HashCryptoBuildIn, IHasHMACBuildIn
    {
        public Sha1CryptoServiceProvider() 
            : base(new System.Security.Cryptography.SHA1CryptoServiceProvider(), 64)
        {
        }

        public System.Security.Cryptography.HMAC GetBuildHMAC()
        {
            return new System.Security.Cryptography.HMACSHA1(new byte[0], false);
        }
    }
}