namespace Lenneth.Core.Framework.HashLib.Crypto.BuildIn
{
    internal class Sha512CryptoServiceProvider : HashCryptoBuildIn
    {
        public Sha512CryptoServiceProvider() 
            : base(new System.Security.Cryptography.SHA512CryptoServiceProvider(), 128)
        {
        }
    }
}