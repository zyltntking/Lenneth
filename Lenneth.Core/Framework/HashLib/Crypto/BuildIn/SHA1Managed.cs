namespace Lenneth.Core.Framework.HashLib.Crypto.BuildIn
{
    internal sealed class Sha1Managed : HashCryptoBuildIn, IHasHMACBuildIn
    {
        public Sha1Managed()
            : base(new System.Security.Cryptography.SHA1Managed(), 64)
        {
        }

        public System.Security.Cryptography.HMAC GetBuildHMAC()
        {
            return new System.Security.Cryptography.HMACSHA1(new byte[0], true);
        }
    }
}