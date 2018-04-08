namespace Lenneth.Core.Framework.HashLib.Crypto.BuildIn
{
    internal sealed class Ripemd160Managed : HashCryptoBuildIn, IHasHMACBuildIn
    {
        public Ripemd160Managed()
            : base(new System.Security.Cryptography.RIPEMD160Managed(), 64)
        {
        }

        public System.Security.Cryptography.HMAC GetBuildHMAC()
        {
            return new System.Security.Cryptography.HMACRIPEMD160();
        }
    }
}