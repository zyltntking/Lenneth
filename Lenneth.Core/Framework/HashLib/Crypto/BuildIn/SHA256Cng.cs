namespace Lenneth.Core.Framework.HashLib.Crypto.BuildIn
{
    internal class Sha256Cng : HashCryptoBuildIn
    {
        public Sha256Cng() 
            : base(new System.Security.Cryptography.SHA256Cng(), 64)
        {
        }
    }
}