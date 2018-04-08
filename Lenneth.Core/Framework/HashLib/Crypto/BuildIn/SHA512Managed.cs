namespace Lenneth.Core.Framework.HashLib.Crypto.BuildIn
{
    internal sealed class Sha512Managed : HashCryptoBuildIn, IHasHMACBuildIn
    {
        public Sha512Managed()
            : base(new System.Security.Cryptography.SHA512Managed(), 128)
        {
        }

        public System.Security.Cryptography.HMAC GetBuildHMAC()
        {
            return new System.Security.Cryptography.HMACSHA512();
        }
    }
}