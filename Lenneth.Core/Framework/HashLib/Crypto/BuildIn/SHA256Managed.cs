namespace Lenneth.Core.Framework.HashLib.Crypto.BuildIn
{
    internal sealed class Sha256Managed : HashCryptoBuildIn, IHasHMACBuildIn
    {
        public Sha256Managed() 
            : base(new System.Security.Cryptography.SHA256Managed(), 64)
        {
        }

        public System.Security.Cryptography.HMAC GetBuildHMAC()
        {
            return new System.Security.Cryptography.HMACSHA256();
        }
    }
}