namespace Lenneth.Core.Framework.HashLib.Crypto.BuildIn
{
    internal sealed class Sha384Managed : HashCryptoBuildIn, IHasHMACBuildIn
    {
        public Sha384Managed() 
            : base(new System.Security.Cryptography.SHA384Managed(), 128)
        {
        }

        public System.Security.Cryptography.HMAC GetBuildHMAC()
        {
            return new System.Security.Cryptography.HMACSHA384();
        }
    }
}