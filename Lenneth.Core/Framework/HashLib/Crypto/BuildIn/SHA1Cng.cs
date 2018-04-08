namespace Lenneth.Core.Framework.HashLib.Crypto.BuildIn
{
    internal class Sha1Cng : HashCryptoBuildIn
    {
        public Sha1Cng() 
            : base(new System.Security.Cryptography.SHA1Cng(), 64)
        {
        }
    }
}