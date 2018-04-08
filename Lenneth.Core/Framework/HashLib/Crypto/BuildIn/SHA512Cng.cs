namespace Lenneth.Core.Framework.HashLib.Crypto.BuildIn
{
    internal class Sha512Cng : HashCryptoBuildIn
    {
        public Sha512Cng() 
            : base(new System.Security.Cryptography.SHA512Cng(), 128)
        {
        }
    }
}