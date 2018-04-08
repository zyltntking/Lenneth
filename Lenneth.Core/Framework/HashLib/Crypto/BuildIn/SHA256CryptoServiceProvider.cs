namespace Lenneth.Core.Framework.HashLib.Crypto.BuildIn
{
    internal class Sha256CryptoServiceProvider : HashCryptoBuildIn
    {
        public Sha256CryptoServiceProvider() 
            : base(new System.Security.Cryptography.SHA256CryptoServiceProvider(), 64)
        {
        }
    }
}