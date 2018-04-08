namespace Lenneth.Core.Framework.HashLib.Crypto.BuildIn
{
    internal class Sha384CryptoServiceProvider : HashCryptoBuildIn
    {
        public Sha384CryptoServiceProvider() 
            : base(new System.Security.Cryptography.SHA384CryptoServiceProvider(), 128)
        {
        }
    }
}